using IBUAPI.Models;
using IBUAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBUAPI.Controllers;

// UserController class. Contains methods for user management.
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly IUserRepository users;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        users = new UserRepository();
        // Full users by random users.
        for (int i = 0; i < 10; i++)
        {
            users.AddUser(new User
            {
                UserName = "User" + i,
                Email = "user" + i + "@gmail.com",
                Password = "123456"
            });
        }
        _logger = logger;
    }

    // Get all users.
    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        try
        {
            return Ok(users.GetAllUsers());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get user by id. Return bad request if user does not exist.
    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        try
        {
            var user = users.GetUserById(id);
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Check if user exists.
    [HttpGet("Exists/{id}")]
    public ActionResult<bool> Exists(int id)
    {
        try
        {
            return Ok(users.UserExists(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add user.
    // Parameters: UserName, Email, Password.
    [HttpPost]
    public ActionResult<User> AddUser(string UserName,
                                   string Email,
                                   string Password)
    {
        try
        {
            User userToAdd = new User(users.GetLastUserId() + 1, UserName, Email, Password);
            users.AddUser(userToAdd);
            return Ok(userToAdd);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update user.
    // Parameters: UserName, Email, Password.
    [HttpPut("{id}")]
    public ActionResult<User> UpdateUser(int id,
                                        string UserName,
                                        string Email,
                                        string Password)
    {
        try
        {
            User userToUpdate = new User(id, UserName, Email, Password);
            users.UpdateUser(userToUpdate);
            return Ok(userToUpdate);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete user by id.
    // Parameters: id.
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        try
        {
            users.DeleteUser(id);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}