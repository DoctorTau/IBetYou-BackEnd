namespace BackEnd.Repositories
{
    public interface UserBetRepository
    {
        // Add User to bet by ids.
        void AddUserToBet(int betId, int userId);

        // Get all users in bet by bet id.
        IEnumerable<User> GetAllUsersInBet(int betId);

        // Get all bets of user by user id.
        IEnumerable<Bet> GetAllBetsOfUser(int userId);

        // Delete user from bet by ids.
        void DeleteUserFromBet(int betId, int userId);

    }
}

// Public class UserBetRepository, which provides access to users and bets.
public class UserBetRepository : IUserBetRepository
{
    // Private fields.
    private readonly ApplicationDbContext _context;
    public readonly IUserRepository _userRepository;
    public readonly IBetRepository _betRepository;

    // Public constructor.
    public UserBetRepository(ApplicationDbContext context, IUserRepository userRepository, IBetRepository betRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _betRepository = betRepository;
    }

    // Add User to bet by ids.
    public void AddUserToBet(int betId, int userId)
    {
        var bet = _betRepository.GetBetById(betId);
        var user = _userRepository.GetUserById(userId);
        bet.Users.Add(user);
        _context.SaveChanges();
    }

    // Get all users in bet by bet id.
    public IEnumerable<User> GetAllUsersInBet(int betId)
    {
        var bet = _betRepository.GetBetById(betId);
        return bet.Users;
    }

    // Get all bets of user by user id.
    public IEnumerable<Bet> GetAllBetsOfUser(int userId)
    {
        var user = _userRepository.GetUserById(userId);
        return user.Bets;
    }

    // Delete user from bet by ids.
    public void DeleteUserFromBet(int betId, int userId)
    {
        var bet = _betRepository.GetBetById(betId);
        var user = _userRepository.GetUserById(userId);
        bet.Users.Remove(user);
        _context.SaveChanges();
    }

    // Set a winner Id to bet by ids.
    // Throw argument exception if bet is not closed, user is not participant.
    public void SetWinnerId(int betId, int userId)
    {
        var bet = _betRepository.GetBetById(betId);
        var user = _userRepository.GetUserById(userId);
        if (bet.Status != BetStatus.Close)
        {
            throw new ArgumentException("Bet is not closed.");
        }
        if (!bet.Users.Contains(user))
        {
            throw new ArgumentException("User is not participant.");
        }
        bet.WinnerId = userId;
        _context.SaveChanges();
    }

}