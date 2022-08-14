using IBUAPI.Models;

namespace IBUAPI.Services;

// Interface for UserRepository.
// Methods: GetAllUsers, GetUserById, AddUser, UpdateUser, DeleteUser.
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserByIdAsync(int id);
    int GetLastUserId();
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<bool> UserExistsAsync(int id);
}

