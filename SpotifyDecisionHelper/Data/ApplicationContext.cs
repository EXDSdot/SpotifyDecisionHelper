using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.Models;


namespace SpotifyDecisionHelper.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Artist> Artists { set; get; }
        public DbSet<Track> Tracks { set; get; }
        //public DbSet<Album> Albums { set; get; }

        public  ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=SpotifyDecisionHelperDb;Trusted_Connection=True");
        }
    }
}