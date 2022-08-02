using AutoMapper;
using BackEnd.Models.Dto;
using IBUAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IBUAPI.Services;

// Realization of IBetRepository.
// Using List<Bet> to store bets.
public class BetRepository : IBetRepository
{
    private DataContext _context;
    private IMapper _mapper;

    public BetRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Bet>> GetAllBetsAsync()
    {
        return await _context.Bets.ToListAsync();
    }
    public async Task<Bet> GetBetByIdAsync(int id)
    {
        Bet? BetToGet = await _context.Bets.FindAsync(id);
        // Check if bet exists. Thrown ArgumentException if not.
        if (BetToGet == null)
            throw new ArgumentException("Bet with this id does not exist.");
        return BetToGet;
    }

    public async Task<bool> BetExistsAsync(int id)
    {
        return _context.Bets.Contains(await GetBetByIdAsync(id));
    }

    public async Task AddBetAsync(CreatingBetDto bet)
    {
        Bet betToAdd = _mapper.Map<Bet>(bet);
        betToAdd.Id = GetLastBetId() + 1;
        _context.Bets.Add(betToAdd);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateBetAsync(Bet bet)
    {
        try
        {
            Bet oldBet = await GetBetByIdAsync(bet.Id);
            oldBet.Name = bet.Name;
            oldBet.Description = bet.Description;
            oldBet.Status = bet.Status;
            oldBet.StartDate = bet.StartDate;
            oldBet.EndDate = bet.EndDate;
            oldBet.WinnerId = bet.WinnerId;
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("Bet with this id does not exist");
        }
    }
    public async Task DeleteBetAsync(int id)
    {
        try
        {
            Bet bet = await GetBetByIdAsync(id);
            _context.Bets.Remove(bet);
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("Bet with this id does not exist");
        }
    }

    public int GetLastBetId()
    {
        if (_context.Bets.Count() == 0)
            return 0;
        return _context.Bets.Max(b => b.Id);
    }

    public async Task StartBetAsync(int id)
    {
        Bet betToStart = await GetBetByIdAsync(id);
        if (betToStart.Status != BetStatus.Creating)
            throw new ArgumentException("Bet is not in creating status.");
        betToStart.Status = BetStatus.Open;
        betToStart.StartDate = DateTime.Now.ToUniversalTime();
        await _context.SaveChangesAsync();
    }

    public async Task SetWinnerAsync(int id, int winnerId)
    {
        Bet betToSet = await GetBetByIdAsync(id);
        if (betToSet.Status != BetStatus.Open)
            throw new ArgumentException("Bet is not in open status.");
        betToSet.Status = BetStatus.Close;
        betToSet.WinnerId = winnerId;
        betToSet.EndDate = DateTime.Now.ToUniversalTime();
        await _context.SaveChangesAsync();
    }
}
