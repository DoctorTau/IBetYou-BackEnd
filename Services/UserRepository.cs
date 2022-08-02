using IBUAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IBUAPI.Services;

// Interface realization using List<User>.
public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }
    public User GetUserById(int id)
    {
        User? UserToGet = _context.Users.FindAsync(id).Result;
        // Check if user exists. Thrown ArgumentException if not.
        if (UserToGet == null)
            throw new ArgumentException("User with this id does not exist.");
        return UserToGet;
    }
    public async Task AddUserAsync(User user)
    {
        _context.Add(user);

        await _context.SaveChangesAsync();
    }

    // Add user to list of users.
    // Parameters: UserName, email, password.

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            User oldUser = GetUserById(user.Id);
            oldUser.UserName = user.UserName;
            oldUser.Email = user.Email;
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("User with this id does not exist");
        }

    }
    public async Task DeleteUserAsync(int id)
    {
        try
        {
            User user = GetUserById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("User with this id does not exist");
        }
    }

    // Get last user id.
    public int GetLastUserId()
    {
        return _context.Users.Max(u => u.Id);
    }

    public bool UserExists(int id)
    {
        return _context.Users.Contains(GetUserById(id));
    }
}