using IBUAPI.Models;

namespace IBUAPI.Services;

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


