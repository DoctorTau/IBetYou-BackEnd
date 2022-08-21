using AutoMapper;
using IBUAPI.Models.Dto;
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

    public async Task<IEnumerable<GetBetDto>> GetAllBetsAsync()
    {
        var listOfBets = await _context.Bets.ToListAsync();
        var listOfBetDto = new List<GetBetDto>();
        foreach (var bet in listOfBets)
        {
            GetBetDto item = _mapper.Map<GetBetDto>(bet);
            // Get users id of item id from UserBet.
            List<GetUserDto> listOfUsers = await GetUsersByBet(item);
            item.Participants = listOfUsers;
            listOfBetDto.Add(item);
        }
        return listOfBetDto;
    }

    private async Task<List<GetUserDto>> GetUsersByBet(GetBetDto item)
    {
        var listOfUserIds = await _context.UserBets.Where(x => x.BetId == item.Id)
                        .Select(x => x.UserId)
                        .ToListAsync();
        // Get users of item id from User.
        var listOfUsers = _mapper.Map<List<GetUserDto>>(await _context.Users.Where(x => listOfUserIds.Contains(x.Id))
                                                                            .ToListAsync());
        return listOfUsers;
    }

    public async Task<GetBetDto> GetBetByIdAsync(int id)
    {
        Bet? BetToGet = await _context.Bets.FindAsync(id);
        // Check if bet exists. Thrown ArgumentException if not.
        if (BetToGet == null)
            throw new ArgumentException("Bet with this id does not exist.");
        GetBetDto getBetDto = _mapper.Map<GetBetDto>(BetToGet);
        getBetDto.Participants = await GetUsersByBet(getBetDto);
        return getBetDto;
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
