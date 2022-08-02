using IBUAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IBUAPI.Services;

// Public class UserBetRepository implements IUserBetRepository.
public class UserBetRepository : IUserBetRepository
{
    private DataContext _context;

    public UserBetRepository(DataContext context)
    {
        _context = context;
    }

    // Get all userBets.
    public async Task<IEnumerable<UserBet>> GetAllUserBetsAsync()
    {
        return await _context.UserBets.ToListAsync();
    }

    // Get userBet by id. Throw ArgumentException if not found.
    public async Task<UserBet> GetUserBetByIdAsync(int id)
    {
        UserBet? bet = await _context.UserBets.FindAsync(id);
        if (bet == null) throw new ArgumentException("There is no UserBet with id " + id);
        return bet;
    }

    // Add User to bet by ids.
    public async Task AddUserToBetAsync(int betId, int userId, int option)
    {
        //Check if user bet is already in list.
        if (_context.UserBets.Any(b => b.BetId == betId && b.UserId == userId))
            throw new ArgumentException("User is already in this bet.");
        _context.UserBets.Add(new UserBet { Id = GetLastId() + 1, BetId = betId, UserId = userId, VoteOption = option });
        await _context.SaveChangesAsync();
    }

    // Get all users in bet by bet id.
    public IEnumerable<int> GetAllUsersIdsInBet(int betId)
    {
        return _context.UserBets.Where(ub => ub.BetId == betId).Select(ub => ub.UserId);
    }
    // Get all bets of user by user id.
    public IEnumerable<int> GetAllBetsIdsOfUser(int userId)
    {
        return _context.UserBets.Where(ub => ub.UserId == userId).Select(ub => ub.BetId);
    }
    // Delete user from bet by ids.
    public async Task DeleteUserFromBetAsync(int betId, int userId)
    {
        try
        {
            UserBet? userBetToDelete = _context.UserBets.Where(ub => ub.BetId == betId && ub.UserId == userId).Single();
            if (userBetToDelete == null)
                throw new ArgumentException("There are not connection with this user and bet.");
            _context.UserBets.Remove(userBetToDelete);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new ArgumentException("There are not connection with this user and bet.");
        }
    }

    public int GetUserBetId(int userId, int betId)
    {
        UserBet? userBet = _context.UserBets.Where(ub => ub.BetId == betId && ub.UserId == userId).Single();
        if (userBet == null)
            throw new ArgumentException("There are not connection with this user and bet.");
        return userBet.Id;
    }

    public async void ConfirmUserBet(int userId, int betId)
    {
        UserBet userBet = await GetUserBetByIdAsync(GetUserBetId(userId, betId));
        userBet.IsConfirmed = true;
        await _context.SaveChangesAsync();
    }

    public bool AllUserBetsConfirmed(int betId)
    {
        foreach (UserBet user in _context.UserBets.Where(ub => ub.BetId == betId))
        {
            if (!user.IsConfirmed)
                return false;
        }
        return true;
    }

    public async Task DeleteUserBetAsync(int id)
    {
        _context.UserBets.Remove(await GetUserBetByIdAsync(id));
        await _context.SaveChangesAsync();
    }

    public int GetLastId()
    {
        if (_context.UserBets.Count() == 0)
            return 0;
        return _context.UserBets.Max(ub => ub.Id);
    }
}