using IBUAPI.Models.Dto;
using IBUAPI.Models;

namespace IBUAPI.Services;

// Interface IBetRepository.
// Methods: GetAllBets, GetBetById, AddBet, UpdateBet, DeleteBet.
public interface IBetRepository
{
    Task<IEnumerable<GetBetDto>> GetAllBetsAsync();
    Task<GetBetDto> GetBetByIdAsync(int id);
    Task AddBetAsync(CreatingBetDto bet);
    Task UpdateBetAsync(Bet bet);
    Task StartBetAsync(int id);
    Task SetWinnerAsync(int betId, int winnerId);
    Task DeleteBetAsync(int id);
    int GetLastBetId();
    Task<bool> BetExistsAsync(int id);
}


