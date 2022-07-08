namespace IBUAPI.Models;

// Enum status for bet.
// Options: Open, Close, Creating, Cancelled.
public enum BetStatus
{
    Open,
    Close,
    Creating,
    Cancelled
}

// Public class Bet.
// Public properties: BetId, Name, Description, Status, list of participant Ids, nullable StartDate, nullable EndDate, nullable winnerId.
public class Bet
{
    public int BetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BetStatus Status { get; set; } = BetStatus.Creating;
    public List<int> ParticipantIds { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? WinnerId { get; set; }

    public Bet()
    {
        ParticipantIds = new List<int>();
    }

    // Constructor
    // Parameters: id, name, description.
    public Bet(int id, string name, string description)
    {
        BetId = id;
        Name = name;
        Description = description;
        Status = BetStatus.Creating;
        ParticipantIds = new List<int>();
    }
}