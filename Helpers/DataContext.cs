namespace WebApi.Helpers;

using IBUAPI.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Bet> Bets => Set<Bet>();
    public DbSet<UserBet> UserBets => Set<UserBet>();
}