using IBUAPI.Models;

namespace IBUAPI.Services;

// Interface realization using List<User>.
public class UserRepository : IUserRepository
{
    private List<User> users = new List<User>();

    public UserRepository()
    {
        users = new List<User>();
    }

    public IEnumerable<User> GetAllUsers()
    {
        return users;
    }
    public User GetUserById(int id)
    {
        User? UserToGet = users.Find(u => u.Id == id);
        // Check if user exists. Thrown ArgumentException if not.
        if (UserToGet == null)
            throw new ArgumentException("User with this id does not exist.");
        return UserToGet;
    }
    public void AddUser(User user)
    {
        users.Add(user);
    }

    // Add user to list of users.
    // Parameters: UserName, email, password.

    public void UpdateUser(User user)
    {
        try
        {
            User oldUser = GetUserById(user.Id);
            oldUser.UserName = user.UserName;
            oldUser.Email = user.Email;
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("User with this id does not exist");
        }

    }
    public void DeleteUser(int id)
    {
        try
        {
            User user = GetUserById(id);
            users.Remove(user);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("User with this id does not exist");
        }
    }

    // Get last user id.
    public int GetLastUserId()
    {
        return users.Count;
    }

    public bool UserExists(int id)
    {
        return users.Exists(u => u.Id == id);
    }
}