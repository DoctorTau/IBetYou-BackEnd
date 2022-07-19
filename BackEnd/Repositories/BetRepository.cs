using IBUAPI.Models;

namespace IBUAPI.Repositories;

// Interface IBetRepository.
// Methods: GetAllBets, GetBetById, AddBet, UpdateBet, DeleteBet.
public interface IBetRepository
{
    IEnumerable<Bet> GetAllBets();
    Bet GetBetById(int id);
    void AddBet(Bet bet);
    void UpdateBet(Bet bet);
    void DeleteBet(int id);
    int GetLastBetId();
    bool BetExists(int id);
}

// Realization of IBetRepository.
// Using List<Bet> to store bets.
public class BetRepository : IBetRepository
{
    private List<Bet> bets = new List<Bet>();
    public IEnumerable<Bet> GetAllBets()
    {
        return bets;
    }
    public Bet GetBetById(int id)
    {
        Bet? BetToGet = bets.Find(b => b.Id == id);
        // Check if bet exists. Thrown ArgumentException if not.
        if (BetToGet == null)
            throw new ArgumentException("Bet with this id does not exist.");
        return BetToGet;
    }

    public bool BetExists(int id)
    {
        return bets.Exists(b => b.Id == id);
    }

    public void AddBet(Bet bet)
    {
        bets.Add(bet);
    }
    public void UpdateBet(Bet bet)
    {
        try
        {
            Bet oldBet = GetBetById(bet.Id);
            oldBet.Name = bet.Name;
            oldBet.Description = bet.Description;
            oldBet.Status = bet.Status;
            oldBet.StartDate = bet.StartDate;
            oldBet.EndDate = bet.EndDate;
            oldBet.WinnerId = bet.WinnerId;
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("Bet with this id does not exist");
        }
    }
    public void DeleteBet(int id)
    {
        try
        {
            Bet bet = GetBetById(id);
            bets.Remove(bet);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("Bet with this id does not exist");
        }
    }

    public int GetLastBetId()
    {
        return bets.Count();
    }
}