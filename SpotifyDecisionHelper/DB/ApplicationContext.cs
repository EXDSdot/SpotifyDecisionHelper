#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB.Models;

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
            modelBuilder.Entity<Match>()
                .HasKey(a => new { a.UserId, a.MatchId });
            modelBuilder.Entity<Bracket>()
                .HasKey(a => new { a.UserId,a.BracketId });
            
            modelBuilder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany(b => b.Albums)
                .HasForeignKey(c => new { c.UserId, c.ArtistId });
            modelBuilder.Entity<Track>()
                .HasOne(a => a.Album)
                .WithMany(b => b.Tracks)
                .HasForeignKey(c => new { c.UserId, c.AlbumId });
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Bracket)
                .WithMany(b => b.Matches)
                .HasForeignKey(m => new {m.UserId, m.BracketId});
            modelBuilder.Entity<Track>()
                .HasMany(t => t.Matches)
                .WithMany(m => m.Tracks);
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Bracket> Brackets { get; set; }
    }
}