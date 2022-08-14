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
    public async Task<User> GetUserByIdAsync(int id)
    {
        User? UserToGet = await _context.Users.FindAsync(id);
        // Check if user exists. Thrown ArgumentException if not.
        if (UserToGet == null)
            throw new ArgumentException("User with this id does not exist.");
        return UserToGet;
    }
    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    // Add user to list of users.
    // Parameters: UserName, email, password.

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            User oldUser = await GetUserByIdAsync(user.Id);
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
            User user = await GetUserByIdAsync(id);
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
        if (_context.Users.Count() == 0)
            return 0;
        return _context.Users.Max(u => u.Id);
    }

    public async Task<bool> UserExistsAsync(int id)
    {
        return _context.Users.Contains(await GetUserByIdAsync(id));
    }
}