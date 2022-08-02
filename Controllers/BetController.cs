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
    public async Task<ActionResult<IEnumerable<Bet>>> GetAll()
    {
        return Ok(await _bets.GetAllBetsAsync());
    }

    // Get bet by id.
    [HttpGet("{id}", Name = "GetBetById")]
    public async Task<ActionResult<Bet>> GetById(int id)
    {
        try
        {
            return Ok(await _bets.GetBetByIdAsync(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Check if bet exists.
    [HttpGet("Exists/{id}", Name = "BetExists")]
    public async Task<ActionResult<bool>> Exists(int id)
    {
        try
        {
            return Ok(await _bets.BetExistsAsync(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add bet.
    // Parameters: Name, Description.
    [HttpPost("AddBet")]
    public async Task<ActionResult<Bet>> AddBet(CreatingBetDto bet)
    {
        try
        {
            await _bets.AddBetAsync(bet);
            return CreatedAtAction(nameof(GetById),
                                  new { id = _bets.GetLastBetId() },
                                  _bets.GetBetByIdAsync(_bets.GetLastBetId()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update bet.
    // Parameters: BetId, Name, Description.
    [HttpPut("UpdateBet/{id}")]
    public async Task<ActionResult<Bet>> Put(int id, String name, String description, List<Option> options)
    {
        try
        {
            Bet betToUpdate = new Bet(id, name, description, options);
            await _bets.UpdateBetAsync(betToUpdate);
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
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _bets.DeleteBetAsync(id);
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
    public async Task<ActionResult<Bet>> SetWinner(int betId, int winnerId)
    {
        try
        {
            Bet betToUpdate = await _bets.GetBetByIdAsync(betId);
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
    public async Task<ActionResult> StartBet(int betId)
    {
        try
        {
            Bet betToUpdate = await _bets.GetBetByIdAsync(betId);
            betToUpdate.StartBet();
            return Ok(betToUpdate);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
