using BackEnd.Models.Dto;
using IBUAPI.Models;
using IBUAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBUAPI.Controllers;

// Bet controller. 
[ApiController]
[Route("api/[controller]")]
public class BetController : ControllerBase
{
    public readonly IBetRepository _bets;
    private readonly ILogger<BetController> _logger;

    public BetController(IBetRepository bets, ILogger<BetController> logger)
    {
        this._bets = bets;

        _logger = logger;
    }

    // Get all bets.
    [HttpGet(Name = "GetAllBets")]
    public ActionResult<IEnumerable<Bet>> GetAll()
    {
        return Ok(_bets.GetAllBets());
    }

    // Get bet by id.
    [HttpGet("{id}", Name = "GetBetById")]
    public ActionResult<Bet> GetById(int id)
    {
        try
        {
            return Ok(_bets.GetBetById(id));
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
            return Ok(_bets.BetExists(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add bet.
    // Parameters: Name, Description.
    [HttpPost("AddBet")]
    public ActionResult<Bet> AddBet(CreatingBetDto bet)
    {
        try
        {
            _bets.AddBet(bet);
            return CreatedAtAction(nameof(GetById),
                                  new { id = _bets.GetLastBetId() },
                                  _bets.GetBetById(_bets.GetLastBetId()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update bet.
    // Parameters: BetId, Name, Description.
    [HttpPut("UpdateBet/{id}")]
    public ActionResult<Bet> Put(int id, String name, String description)
    {
        try
        {
            Bet betToUpdate = new Bet(id, name, description);
            _bets.UpdateBet(betToUpdate);
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
            _bets.DeleteBet(id);
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
            Bet betToUpdate = _bets.GetBetById(betId);
            betToUpdate.SetWinner(winnerId);
            return Ok(betToUpdate);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Start the bet by id.
    // Parameters: BetId.
    [HttpPut("{betId}/start")]
    public ActionResult StartBet(int betId)
    {
        try
        {
            Bet betToUpdate = _bets.GetBetById(betId);
            betToUpdate.StartBet();
            return Ok(betToUpdate);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
