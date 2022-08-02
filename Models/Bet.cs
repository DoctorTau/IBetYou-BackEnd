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
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new List<string>();
    public BetStatus Status { get; set; } = BetStatus.Creating;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public int? WinnerId { get; set; } = null;

    public Bet()
    {

    }

    // Constructor
    // Parameters: id, name, description.
    public Bet(int id,
               string name,
               string description,
               List<string> options)
    {
        Id = id;
        // Check if name is not empty. Thrown ArgumentException if not.
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be empty.");
        Name = name;
        Description = description;
        // Check if there are at least two options. Thrown ArgumentException if not.
        if (options.Count < 2)
            throw new ArgumentException("There must be at least two options.");
        Options = options;
        Status = BetStatus.Creating;
    }
}