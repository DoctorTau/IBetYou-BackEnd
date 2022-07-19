using IBUAPI.Models;

namespace IBUAPI.Services;

// Interface for UserRepository.
// Methods: GetAllUsers, GetUserById, AddUser, UpdateUser, DeleteUser.
public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    int GetLastUserId();
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
    bool UserExists(int id);
}

