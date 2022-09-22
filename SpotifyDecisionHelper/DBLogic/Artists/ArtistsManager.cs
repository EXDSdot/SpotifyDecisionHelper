using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Models;

namespace SpotifyDecisionHelper.DBLogic.Artists;

public class ArtistsManager : IArtistsManager
{
    private readonly ApplicationContext _context;

    public ArtistsManager(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task Add(string userId, string artistId)
    {
        if (await _context.Artists.FindAsync(userId, artistId) != null)
            return;
        
        var artist = new Artist
        {
            UserId = userId, 
            ArtistId = artistId,
        };
        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(string userId, string artistId)
    {
        var artist = _context.Artists.FirstOrDefault(g => g.ArtistId == artistId && g.UserId == userId);
        
        if (artist != null)
        {
            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
        }
    }
}