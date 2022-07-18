using IBUAPI.Models;
using IBUAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

using IBUAPI.Controllers;

// Bet controller. 
[ApiController]
[Route("[controller]")]
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
            this.bets.AddBet(new Bet
            {
                Name = "Bet" + i,
                Description = "Bet description",
                Status = BetStatus.Open,
                ParticipantIds = new List<int> { 1, 2, 3 },
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                WinnerId = null
            });
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
}
