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
        Console.WriteLine(_context.ContextId);
        Console.WriteLine(request.Name);
        var artist = await _context.Artists.FirstOrDefaultAsync(a => a.UserId == request.UserId && a.ArtistId == request.ArtistId);
        Console.WriteLine(request.Name);

        if (artist != null) return artist;
        
        artist = new Artist
        {
            UserId = request.UserId,
            ArtistId = request.ArtistId,
            Name = request.Name,
            Rating = request.Rating
        };
        Console.WriteLine(request.Name+4);

        _context.Artists.Add(artist);
        Console.WriteLine(request.Name+5);

        await _context.SaveChangesAsync();
        Console.WriteLine(request.Name+6);

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