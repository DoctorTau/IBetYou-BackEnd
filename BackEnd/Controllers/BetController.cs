using IBUAPI.Models;
using IBUAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBUAPI.Controllers;

// Bet controller. 
[ApiController]
[Route("api/[controller]")]
public class BetController : ControllerBase
{
    public readonly IBetRepository bets;
    private readonly ILogger<BetController> _logger;

    public BetController(ILogger<BetController> logger)
    {
        this.bets = new BetRepository();
        // Fill bets with random bets.
        for (int i = 0; i < 10; i++)
        {
            this.bets.AddBet(new Bet(i + 1,
                                     "Bet" + i,
                                     "Bet description"));
        }
        _logger = logger;
    }

    // Get all bets.
    [HttpGet(Name = "GetAllBets")]
    public ActionResult<IEnumerable<Bet>> Get()
    {
        return Ok(bets.GetAllBets());
    }

    // Get bet by id.
    [HttpGet("{id}", Name = "GetBetById")]
    public ActionResult<Bet> Get(int id)
    {
        try
        {
            return Ok(bets.GetBetById(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Check if bet exists.
    [HttpGet("Exists/{id}", Name = "BetExists")]
    public ActionResult<bool> Exists(int id)
    {
        try
        {
            return Ok(bets.BetExists(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add bet.
    // Parameters: Name, Description.
    [HttpPost]
    public ActionResult<Bet> Post(String name, String description)
    {
        try
        {
            Bet betToAdd = new Bet(bets.GetLastBetId(), name, description);
            bets.AddBet(betToAdd);
            return CreatedAtRoute("GetBetById", betToAdd);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update bet.
    // Parameters: BetId, Name, Description.
    [HttpPut("{id}")]
    public ActionResult<Bet> Put(int id, String name, String description)
    {
        try
        {
            Bet betToUpdate = new Bet(id, name, description);
            bets.UpdateBet(betToUpdate);
            return Ok(betToUpdate);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete bet.
    [HttpDelete("{id}")]
    /// <summary>
    /// Delete bet.
    /// </summary>
    /// <param name="id">Id of bet to delete.</param>
    /// <returns>Status of operation.</returns>
    public ActionResult Delete(int id)
    {
        try
        {
            bets.DeleteBet(id);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Set winner id to bet. 
    // Parameters: BetId, WinnerId.
    [HttpPut("{betId}/winner/{winnerId}")]
    /// <summary>
    /// Sets winner id to bet and closes the bet.
    /// </summary>
    /// <param name="betId"> Bet id.</param>
    /// <param name="winnerId"> User id.</param>
    /// <returns> Status of operation result.</returns>
    public ActionResult<Bet> SetWinner(int betId, int winnerId)
    {
        try
        {
            Bet betToUpdate = bets.GetBetById(betId);
            betToUpdate.SetWinner(winnerId);
            return Ok(betToUpdate);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
