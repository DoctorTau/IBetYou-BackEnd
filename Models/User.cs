namespace IBUAPI.Models;

// Public class User
// Public properties: UserId, UserName, Email, Password, List of bet Ids
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiredAt { get; set; }


    public User()
    {
    }

    // Constructor by id, username, email, password.
    public User(int id, string username, string email)
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
    }
}