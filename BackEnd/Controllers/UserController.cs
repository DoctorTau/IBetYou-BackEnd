using IBUAPI.Models;
using IBUAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBUAPI.Controllers;

// UserController class. Contains methods for user management.
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly IUserRepository _users;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository users, ILogger<UserController> logger)
    {
        _users = users;
        // Fill users by random users.
        for (int i = 0; i < 10; i++)
        {
            _users.AddUser(new User
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
            return Ok(_users.GetAllUsers());
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
            var user = _users.GetUserById(id);
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
            return Ok(_users.UserExists(id));
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
            User userToAdd = new User(_users.GetLastUserId() + 1, UserName, Email, Password);
            _users.AddUser(userToAdd);
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
            _users.UpdateUser(userToUpdate);
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
            _users.DeleteUser(id);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}