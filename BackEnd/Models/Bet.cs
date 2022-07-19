using System.Text.Json.Serialization;

namespace IBUAPI.Models;

// Enum status for bet.
// Options: Open, Close, Creating, Cancelled.
[JsonConverter(typeof(JsonStringEnumConverter))]
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
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BetStatus Status { get; set; } = BetStatus.Creating;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? WinnerId { get; set; }

    // Constructor
    // Parameters: id, name, description.
    public Bet(int id, string name, string description)
    {
        Id = id;
        // Check if name is not empty. Thrown ArgumentException if not.
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be empty.");
        Name = name;
        Description = description;
        Status = BetStatus.Creating;
    }

    // Set winner method.
    // Parameters: winnerId.
    // Throw Exception if bet status is not Open.
    public void SetWinner(int winnerId)
    {
        if (Status != BetStatus.Open)
            throw new ArgumentException("Bet is not open.");
        WinnerId = winnerId;
        Status = BetStatus.Close;
    }

    internal void StartBet()
    {
        if (Status != BetStatus.Creating)
            throw new ArgumentException("Bet is not creating.");
        Status = BetStatus.Open;
    }
}