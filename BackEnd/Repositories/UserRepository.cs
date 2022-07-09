using IBUAPI.Models;

namespace IBUAPI.Repositories;

// Interface for UserRepository.
// Methods: GetAllUsers, GetUserById, AddUser, UpdateUser, DeleteUser.
public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
}

// Interface realization using List<User>.
public class UserRepository : IUserRepository
{
    private List<User> users = new List<User>();
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
    public void UpdateUser(User user)
    {
        try
        {
            User oldUser = GetUserById(user.Id);
            oldUser.UserName = user.UserName;
            oldUser.Email = user.Email;
            oldUser.Password = user.Password;
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
}