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
        _logger = logger;
    }

    // Get all users.
    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
    {
        try
        {
            return Ok(await _users.GetAllUsers());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get user by id. Return bad request if user does not exist.
    [HttpGet("GetUser/{id}")]
    public ActionResult<User> GetById(int id)
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
    [HttpPost("AddUser")]
    public async Task<ActionResult<User>> AddUserAsync(string UserName,
                                   string Email)
    {
        try
        {
            User userToAdd = new User(1, UserName, Email);
            await _users.AddUserAsync(userToAdd);
            return CreatedAtAction(nameof(GetById), new { id = userToAdd.Id }, userToAdd);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update user.
    // Parameters: UserName, Email, Password.
    [HttpPut("Update")]
    public async Task<ActionResult<User>> UpdateUserAsync(int id,
                                        string UserName,
                                        string Email)
    {
        try
        {
            User userToUpdate = new User(id, UserName, Email);
            await _users.UpdateUserAsync(userToUpdate);
            return Ok(userToUpdate);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete user by id.
    // Parameters: id.
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        try
        {
            await _users.DeleteUserAsync(id);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}