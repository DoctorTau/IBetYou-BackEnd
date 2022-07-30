namespace IBUAPI.Models;

// Public Class UserBet to store user id and bet id.
public class UserBet
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BetId { get; set; }
    public bool IsConfirmed { get; set; } = false;
}