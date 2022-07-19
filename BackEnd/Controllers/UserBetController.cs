using IBUAPI.Models;
using IBUAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBUAPI.Controllers;

// UserBet controller.
[ApiController]
[Route("api/[controller]")]
public class UserBetController : ControllerBase
{
    public readonly IUserBetRepository userBets;
    private readonly ILogger<UserBetController> _logger;

    public UserBetController(ILogger<UserBetController> logger)
    {
        this.userBets = new UserBetRepository();
        // Fill userBets with random userBets.
        for (int i = 0; i < 10; i++)
        {
            this.userBets.AddUserToBet(1, i);
        }
        _logger = logger;
    }

    // Get all userBets.
    [HttpGet(Name = "GetAllUserBets")]
    public ActionResult<IEnumerable<UserBet>> Get()
    {
        return Ok(userBets.GetAllUserBets());
    }

    // Get by id.
    [HttpGet("{id}", Name = "GetUserBetById")]
    public ActionResult<UserBet> Get(int id)
    {
        try
        {
            return Ok(userBets.GetUserBetById(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add UserBet by user id and bet id.
    [HttpPost("AddUserToBet/{userId}/{betId}", Name = "AddUserToBet")]
    public ActionResult<UserBet> AddUserToBet(int userId, int betId)
    {
        try
        {
            userBets.AddUserToBet(userId, betId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete UserBet by user id and bet id.
    [HttpDelete("DeleteUserBet/{userId}/{betId}")]
    public ActionResult<UserBet> Delete(int userId, int betId)
    {
        try
        {
            userBets.DeleteUserFromBet(userId, betId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get all users from bet by bet id.
    [HttpGet("{betId}/participants", Name = "GetUsersFromBet")]
    public ActionResult<IEnumerable<User>> GetUsersFromBet(int betId)
    {
        try
        {
            return Ok(userBets.GetAllUsersIdsInBet(betId));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get all bets from user by user id.
    [HttpGet("{userId}/bets", Name = "GetBetsFromUser")]
    public ActionResult<IEnumerable<Bet>> GetBetsFromUser(int userId)
    {
        try
        {
            return Ok(userBets.GetAllBetsIdsOfUser(userId));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}