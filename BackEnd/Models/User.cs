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

    // Constructor by id, username, email, password.
    public User(int id, string username, string email, string password)
    {
        Id = id;
        // Checks if username is not empty.
        if (username == string.Empty)
            throw new ArgumentException("Username cannot be empty.");
        UserName = username;
        // Checks if email is correct.
        if (!email.Contains("@") || !email.Contains("."))
            throw new ArgumentException("Email is not correct.");
        Email = email;
        // Checks if password is correct.
        if (password.Length < 6)
            throw new ArgumentException("Password is not correct.");
        Password = password;
        BetIds = new List<int>();
    }
}