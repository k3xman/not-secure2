# Security Analysis with CodeQL

This repository includes GitHub CodeQL SAST (Static Application Security Testing) analysis to detect security vulnerabilities. **This application intentionally contains bad security practices for educational purposes.**

## CodeQL Analysis

### What CodeQL Detects

CodeQL will identify the following intentionally bad security practices in this educational demo:

#### 1. SQL Injection Vulnerabilities
- **Location**: `BadSecurityService.cs`
- **Detection**: String concatenation in SQL queries
- **Examples**:
  ```csharp
  string sql = $"SELECT * FROM Users WHERE Username = '{username}'";
  string sql = $"INSERT INTO Users VALUES ('{username}', '{password}')";
  ```

#### 2. Poor Password Security
- **Location**: `BadSecurityService.cs` and `User.cs`
- **Detection**: MD5 hashing, exposed password fields
- **Examples**:
  ```csharp
  using var md5 = MD5.Create(); // Cryptographically broken
  public string Password { get; set; } // Exposed in model
  ```

#### 3. Input Validation Issues
- **Location**: `UserController.cs`
- **Detection**: Lack of input validation and sanitization
- **Examples**:
  ```csharp
  public IActionResult Register(string username, string password, string email)
  // No validation attributes or sanitization
  ```

#### 4. Error Message Exposure
- **Location**: `BadSecurityService.cs` and `UserController.cs`
- **Detection**: Internal errors exposed to users
- **Examples**:
  ```csharp
  throw new Exception($"Database error: {ex.Message}");
  ViewBag.ErrorMessage = $"Error: {ex.Message}";
  ```

#### 5. Poor Session Management
- **Location**: `UserController.cs`
- **Detection**: Insecure session handling
- **Examples**:
  ```csharp
  HttpContext.Session.SetString("Username", username);
  ```

## CodeQL Configuration

### Custom Queries
The repository includes custom CodeQL queries in `.github/queries/csharp/` that specifically target the educational security demo patterns.

### Analysis Schedule
- **On Push**: CodeQL runs on every push to master
- **On Pull Request**: CodeQL runs on every PR to master
- **Scheduled**: CodeQL runs daily at 2 AM UTC

### Query Suites Used
- **security-extended**: Comprehensive security analysis
- **security-and-quality**: Additional security and quality checks
- **Custom queries**: Educational demo specific patterns

## Expected CodeQL Findings

When CodeQL analyzes this repository, it will likely find:

### High Severity Issues
1. **SQL Injection**: Multiple instances in `BadSecurityService.cs`
2. **Weak Cryptography**: MD5 usage for password hashing
3. **Information Exposure**: Password hashes exposed in UI

### Medium Severity Issues
1. **Input Validation**: Lack of proper validation
2. **Error Handling**: Internal errors exposed
3. **Session Management**: Poor session security

### Low Severity Issues
1. **Code Quality**: Various code quality issues
2. **Best Practices**: Deviations from security best practices

## Educational Purpose

These findings are **intentional** and serve educational purposes:

1. **Demonstrate Common Vulnerabilities**: Show real-world security issues
2. **CodeQL Detection**: Demonstrate how SAST tools identify problems
3. **Security Awareness**: Highlight the importance of secure coding
4. **Learning Tool**: Provide examples for security training

## Remediation (For Production)

In a production environment, these issues should be fixed:

### SQL Injection
```csharp
// BAD (Current)
string sql = $"SELECT * FROM Users WHERE Username = '{username}'";

// GOOD (Fixed)
var command = new SqlCommand("SELECT * FROM Users WHERE Username = @username");
command.Parameters.AddWithValue("@username", username);
```

### Password Security
```csharp
// BAD (Current)
using var md5 = MD5.Create();

// GOOD (Fixed)
using var hasher = new Rfc2898DeriveBytes(password, salt, 10000);
```

### Input Validation
```csharp
// BAD (Current)
public IActionResult Register(string username, string password)

// GOOD (Fixed)
public IActionResult Register([Required][StringLength(50)] string username, 
                            [Required][MinLength(8)] string password)
```

## CodeQL Results

To view CodeQL analysis results:

1. Go to the **Security** tab in your GitHub repository
2. Click on **Code scanning alerts**
3. Review the findings and their details
4. Use the educational context to understand each vulnerability

## Contributing

When contributing to this educational demo:

1. **Maintain Educational Value**: Keep the bad practices for demonstration
2. **Add Comments**: Clearly mark intentional vulnerabilities
3. **Document**: Explain why each bad practice exists
4. **Test CodeQL**: Ensure new code triggers appropriate alerts

## Resources

- [CodeQL Documentation](https://codeql.github.com/docs/)
- [GitHub Security Lab](https://securitylab.github.com/)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [Microsoft Security Development Lifecycle](https://www.microsoft.com/en-us/securityengineering/sdl/) 