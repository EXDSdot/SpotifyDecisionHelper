using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Entities;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.DBLogic.Artists;

public class ArtistsManager : IArtistsManager
{
    private readonly ApplicationContext _context;

    public ArtistsManager(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IList<Artist>> GetAll() => await _context.Artists.ToListAsync();

    public async Task<Artist> FindOrCreate(CreateArtistRequest request)
    {
        var artist = await _context.Artists.FirstOrDefaultAsync(a => a.UserId == request.UserId && a.ArtistId == request.ArtistId);

        if (artist != null) return artist;
        
        artist = new Artist
        {
            UserId = request.UserId,
            ArtistId = request.ArtistId,
            Name = request.Name,
            Rating = request.Rating
        };

        _context.Artists.Add(artist);

        await _context.SaveChangesAsync();
        return artist;
    }

    public async Task Delete(string userId, string artistId)
    {
        var artist = _context.Artists.FirstOrDefault(g => g.ArtistId == artistId && g.UserId == userId);
        
        if (artist != null)
        {
            _context.Artists.Remove(artist);
            
            await _context.SaveChangesAsync();
        }
    }
}