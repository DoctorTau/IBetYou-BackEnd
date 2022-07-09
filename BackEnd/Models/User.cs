namespace IBUAPI.Models;

// Public class User
// Public properties: UserId, UserName, Email, Password, List of bet Ids
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<int> BetIds { get; set; }

    public User()
    {
        BetIds = new List<int>();
    }
}