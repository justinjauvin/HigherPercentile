using HigherPercentile.Models;
using Microsoft.EntityFrameworkCore;

namespace HigherPercentile.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
    }
}