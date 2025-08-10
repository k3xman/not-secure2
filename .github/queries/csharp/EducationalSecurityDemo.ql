/**
 * @name Educational Security Demo - Bad Practices Detection
 * @description Detects intentionally bad security practices for educational purposes
 * @id csharp/educational-security-demo
 * @kind problem
 * @problem.severity warning
 * @precision medium
 * @tags security
 *       educational
 *       sql-injection
 *       password-security
 *       input-validation
 */

import csharp

/**
 * Detects SQL injection vulnerabilities using string concatenation
 */
predicate sqlInjectionVulnerability(Expr expr) {
  exists(string concatenation |
    concatenation.getAnOperand() instanceof StringLiteral and
    concatenation.getAnOperand().getValue().matches("%SELECT%") and
    concatenation.getAnOperand().getValue().matches("%FROM%") and
    concatenation.getAnOperand().getValue().matches("%WHERE%") and
    concatenation.getAnOperand().getValue().matches("%'%{0}%'%") and
    expr = concatenation
  )
}

/**
 * Detects MD5 usage for password hashing
 */
predicate md5PasswordHashing(MethodCall call) {
  call.getTarget().getName() = "Create" and
  call.getTarget().getDeclaringType().getName() = "MD5" and
  exists(Class c |
    c.getName() = "BadSecurityService" and
    call.getEnclosingCallable().getDeclaringType() = c
  )
}

/**
 * Detects exposed password fields in models
 */
predicate exposedPasswordField(Field field) {
  field.getName() = "Password" and
  field.getDeclaringType().getName() = "User" and
  exists(Property prop |
    prop.getName() = "Password" and
    prop.getDeclaringType() = field.getDeclaringType()
  )
}

/**
 * Detects poor session management
 */
predicate poorSessionManagement(MethodCall call) {
  call.getTarget().getName() = "SetString" and
  call.getTarget().getDeclaringType().getName() = "ISession" and
  call.getAnArgument(0).getValue() = "Username"
}

/**
 * Detects error message exposure
 */
predicate errorMessageExposure(Expr expr) {
  exists(string errorMsg |
    errorMsg.matches("%Database error%") or
    errorMsg.matches("%Failed to%") and
    expr.getValue() = errorMsg
  )
}

/**
 * Detects lack of input validation
 */
predicate noInputValidation(Parameter param) {
  param.getName() = "username" or
  param.getName() = "password" or
  param.getName() = "email" and
  not exists(Attribute attr |
    attr.getTarget().getName() = "Required" or
    attr.getTarget().getName() = "StringLength"
  )
}

from
  Expr sqlExpr, MethodCall md5Call, Field passwordField, 
  MethodCall sessionCall, Expr errorExpr, Parameter inputParam
where
  sqlInjectionVulnerability(sqlExpr) or
  md5PasswordHashing(md5Call) or
  exposedPasswordField(passwordField) or
  poorSessionManagement(sessionCall) or
  errorMessageExposure(errorExpr) or
  noInputValidation(inputParam)
select
  sqlExpr, "SQL injection vulnerability detected using string concatenation" or
  md5Call, "MD5 password hashing detected - cryptographically broken" or
  passwordField, "Password field exposed in model - security risk" or
  sessionCall, "Poor session management detected" or
  errorExpr, "Internal error messages exposed to users" or
  inputParam, "Input parameter lacks validation" 