namespace IBUAPI.Services;

using IBUAPI.Models;

public interface IUserBetRepository
{
    // Get all UserBets.
    IEnumerable<UserBet> GetAllUserBets();

    // Get userBet by id. 
    UserBet GetUserBetById(int id);

    // Get UserBet id by user id and bet id. 
    int GetUserBetId(int userId, int betId);

    // Add User to bet by ids.
    void AddUserToBet(int betId, int userId);

    // Confirm UserBet by user id and bet id.
    void ConfirmUserBet(int userId, int betId);

    // Get all users in bet by bet id.
    IEnumerable<int> GetAllUsersIdsInBet(int betId);

    // Get all bets of user by user id.
    IEnumerable<int> GetAllBetsIdsOfUser(int userId);

    // Delete user from bet by ids.
    void DeleteUserFromBet(int betId, int userId);

    // Checks if all users of one bet confirmed their participants.
    bool AllUserBetsConfirmed(int betId);
    void DeleteUserBet(int id);

    int GetLastId();
}

