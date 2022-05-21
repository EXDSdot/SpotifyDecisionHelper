using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB.Entities;

namespace SpotifyDecisionHelper.DB
{
    public sealed class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>()
                .HasKey(a => new { a.UserId,a.ArtistId });
            modelBuilder.Entity<Album>()
                .HasKey(a => new { a.UserId,a.AlbumId });
            modelBuilder.Entity<Track>()
                .HasKey(a => new { a.UserId,a.TrackId });
            
            modelBuilder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany(b => b.Albums)
                .HasForeignKey(c => new { c.UserId, c.ArtistId });
            modelBuilder.Entity<Track>()
                .HasOne(a => a.Album)
                .WithMany(b => b.Tracks)
                .HasForeignKey(c => new { c.UserId, c.AlbumId });
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
    }
}