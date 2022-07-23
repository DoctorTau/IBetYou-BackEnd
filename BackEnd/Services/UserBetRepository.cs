using IBUAPI.Models;

namespace IBUAPI.Services;

// Public class UserBetRepository implements IUserBetRepository.
public class UserBetRepository : IUserBetRepository
{
    private List<UserBet> userBets = new List<UserBet>();

    // Get all userBets.
    public IEnumerable<UserBet> GetAllUserBets()
    {
        return userBets;
    }

    // Get userBet by id. Throw ArgumentException if not found.
    public UserBet GetUserBetById(int id)
    {
        UserBet? bet = userBets.Find(b => b.Id == id);
        if (bet == null) throw new ArgumentException("There is no UserBet with id " + id);
        return bet;
    }

    // Add User to bet by ids.
    public void AddUserToBet(int betId, int userId)
    {
        userBets.Add(new UserBet { BetId = betId, UserId = userId });
    }

    // Get all users in bet by bet id.
    public IEnumerable<int> GetAllUsersIdsInBet(int betId)
    {
        return userBets.Where(ub => ub.BetId == betId).Select(ub => ub.UserId);
    }
    // Get all bets of user by user id.
    public IEnumerable<int> GetAllBetsIdsOfUser(int userId)
    {
        return userBets.Where(ub => ub.UserId == userId).Select(ub => ub.BetId);
    }
    // Delete user from bet by ids.
    public void DeleteUserFromBet(int betId, int userId)
    {
        UserBet? userBetToDelete = userBets.Find(ub => ub.BetId == betId && ub.UserId == userId);
        if (userBetToDelete == null)
            throw new ArgumentException("There are not connection with this user and bet.");
        userBets.Remove(userBetToDelete);
    }

    public int GetUserBetId(int userId, int betId)
    {
        UserBet? userBet = userBets.Find(ub => ub.UserId == userId && ub.BetId == betId);
        if (userBet == null)
            throw new ArgumentException("There are not connection with this user and bet.");
        return userBet.Id;
    }

    public void ConfirmUserBet(int userId, int betId)
    {
        UserBet userBet = GetUserBetById(GetUserBetId(userId,
                                                      betId));
        userBet.IsConfirmed = true;
    }

    public bool AllUserBetsConfirmed(int betId)
    {
        foreach (UserBet user in userBets.Where(ub => ub.BetId == betId))
        {
            if (!user.IsConfirmed)
                return false;
        }
        return true;
    }

    public void DeleteUserBet(int id)
    {
        userBets.Remove(GetUserBetById(id));
    }
}