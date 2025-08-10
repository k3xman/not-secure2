using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

// WARNING: This controller demonstrates BAD security practices for educational purposes only!
// DO NOT use this code in production applications!
public class UserController : Controller
{
    private readonly BadSecurityService _badSecurityService;
    private readonly ILogger<UserController> _logger;

    public UserController(BadSecurityService badSecurityService, ILogger<UserController> logger)
    {
        _badSecurityService = badSecurityService;
        _logger = logger;
    }

    // BAD PRACTICE: Displaying all users with exposed passwords
    public IActionResult Index()
    {
        try
        {
            var users = _badSecurityService.GetAllUsers();
            return View(users);
        }
        catch (Exception ex)
        {
            // BAD: Exposing internal errors to users
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
            return View(new List<User>());
        }
    }

    // BAD PRACTICE: Registration form
    public IActionResult Register()
    {
        return View();
    }

    // BAD PRACTICE: Registration with poor security
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(string username, string password, string email)
    {
        try
        {
            // BAD: No input validation or sanitization
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username and password are required";
                return View();
            }

            // BAD: Using vulnerable service
            bool success = _badSecurityService.CreateUser(username, password, email);
            
            if (success)
            {
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to create user";
                return View();
            }
        }
        catch (Exception ex)
        {
            // BAD: Exposing internal errors
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
            return View();
        }
    }

    // BAD PRACTICE: Login form
    public IActionResult Login()
    {
        return View();
    }

    // BAD PRACTICE: Authentication with poor security
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        try
        {
            // BAD: No input validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username and password are required";
                return View();
            }

            // BAD: Using vulnerable authentication
            bool isAuthenticated = _badSecurityService.AuthenticateUser(username, password);
            
            if (isAuthenticated)
            {
                // BAD: No proper session management
                HttpContext.Session.SetString("Username", username);
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Todo");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }
        catch (Exception ex)
        {
            // BAD: Exposing internal errors
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
            return View();
        }
    }

    // BAD PRACTICE: Search users with SQL injection vulnerability
    public IActionResult Search(string searchTerm)
    {
        try
        {
            // BAD: No input validation or sanitization
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }

            // BAD: Using vulnerable search method
            var users = _badSecurityService.SearchUsers(searchTerm);
            return View("Index", users);
        }
        catch (Exception ex)
        {
            // BAD: Exposing internal errors
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // BAD PRACTICE: Get user by username with SQL injection vulnerability
    public IActionResult Details(string username)
    {
        try
        {
            // BAD: No input validation
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction(nameof(Index));
            }

            // BAD: Using vulnerable method
            var user = _badSecurityService.GetUserByUsername(username);
            
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
        catch (Exception ex)
        {
            // BAD: Exposing internal errors
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // BAD PRACTICE: Delete user with SQL injection vulnerability
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(string username)
    {
        try
        {
            // BAD: No input validation
            if (string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "Username is required";
                return RedirectToAction(nameof(Index));
            }

            // BAD: Using vulnerable delete method
            bool success = _badSecurityService.DeleteUser(username);
            
            if (success)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete user";
            }
        }
        catch (Exception ex)
        {
            // BAD: Exposing internal errors
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    // BAD PRACTICE: Logout without proper session cleanup
    public IActionResult Logout()
    {
        // BAD: No proper session cleanup
        HttpContext.Session.Clear();
        TempData["SuccessMessage"] = "Logged out successfully!";
        return RedirectToAction("Index", "Todo");
    }
} 