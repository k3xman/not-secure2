using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services;

// WARNING: This class demonstrates BAD security practices for educational purposes only!
// DO NOT use this code in production applications!
public class BadSecurityService
{
    private readonly string _connectionString;

    public BadSecurityService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("db") ?? string.Empty;
    }

    // BAD PRACTICE 8: Directly executing raw SQL from user input (EXTREMELY DANGEROUS)
    // Example exploit: ExecuteRawSql("DROP TABLE Users; --")
    public int ExecuteRawSql(string sql)
    {
        // This method is for educational demonstration ONLY!
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        try
        {
            connection.Open();
            return command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception($"Raw SQL execution error: {ex.Message}");
        }
    }

    // BAD PRACTICE 1: SQL Injection vulnerability
    // Example exploit: username = "' OR 1=1 --"
    public User? GetUserByUsername(string username)
    {
        string sql = $"SELECT Id, Username, Password, Email, CreatedAt, IsActive FROM Users WHERE Username = '{username}'";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32("Id"),
                    Username = reader.GetString("Username"),
                    Password = reader.GetString("Password"),
                    Email = reader.GetString("Email"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    IsActive = reader.GetBoolean("IsActive")
                };
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Database error: {ex.Message}");
        }

        return null;
    }

    // BAD PRACTICE 2: Another SQL Injection vulnerability
    // Example exploit: searchTerm = "%' OR 1=1 --"
    public List<User> SearchUsers(string searchTerm)
    {
        string sql = $"SELECT Id, Username, Password, Email, CreatedAt, IsActive FROM Users WHERE Username LIKE '%{searchTerm}%' OR Email LIKE '%{searchTerm}%'";

        var users = new List<User>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("Id"),
                    Username = reader.GetString("Username"),
                    Password = reader.GetString("Password"),
                    Email = reader.GetString("Email"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    IsActive = reader.GetBoolean("IsActive")
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Database error: {ex.Message}");
        }

        return users;
    }

    // BAD PRACTICE 3: Poor password hashing using MD5 (broken algorithm)
    public string HashPassword(string password)
    {
        using var md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(password);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes).ToLower();
    }

    // BAD PRACTICE 4: Creating user with poor security
    public bool CreateUser(string username, string password, string email)
    {
        string hashedPassword = HashPassword(password);
        // Use GETDATE() for SQL Server
        string sql = $"INSERT INTO Users (Username, Password, Email, CreatedAt, IsActive) VALUES ('{username}', '{hashedPassword}', '{email}', GETDATE(), 1)";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        try
        {
            connection.Open();
            int result = command.ExecuteNonQuery();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create user: {ex.Message}");
        }
    }

    // BAD PRACTICE 5: Authentication with poor security
    public bool AuthenticateUser(string username, string password)
    {
        var user = GetUserByUsername(username);
        if (user == null) return false;
        string hashedInputPassword = HashPassword(password);
        return user.Password == hashedInputPassword;
    }

    // BAD PRACTICE 6: Deleting user with SQL injection
    // Example exploit: username = "' OR 1=1 --"
    public bool DeleteUser(string username)
    {
        string sql = $"DELETE FROM Users WHERE Username = '{username}'";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        try
        {
            connection.Open();
            int result = command.ExecuteNonQuery();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete user: {ex.Message}");
        }
    }

    // BAD PRACTICE 7: Getting all users with passwords exposed
    public List<User> GetAllUsers()
    {
        string sql = "SELECT Id, Username, Password, Email, CreatedAt, IsActive FROM Users";

        var users = new List<User>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("Id"),
                    Username = reader.GetString("Username"),
                    Password = reader.GetString("Password"),
                    Email = reader.GetString("Email"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    IsActive = reader.GetBoolean("IsActive")
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Database error: {ex.Message}");
        }

        return users;
    }
}
