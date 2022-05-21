using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Entities;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.DBLogic.Albums;

public class AlbumsManager : IAlbumsManager
{
    private readonly ApplicationContext _context;

    public AlbumsManager(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<IList<Album>> GetAll() => await _context.Albums.ToListAsync();
    
    public async Task<Album> FindOrCreate(CreateAlbumRequest request)
    {
        var album = await _context.Albums.FirstOrDefaultAsync(a=>a.UserId == request.UserId && a.AlbumId == request.AlbumId);

        if (album != null) return album;
        
        album = new Album
        {
            UserId = request.UserId,
            AlbumId = request.AlbumId,
            Name = request.Name,
            Rating = request.Rating,
            ArtistId = request.ArtistId
        };

        _context.Albums.Add(album);

        await _context.SaveChangesAsync();
        return album;
    }

    public async Task Delete(string userId, string albumId)
    {
        var album = _context.Albums.FirstOrDefault(g => g.AlbumId == albumId && g.UserId == userId);
        
        if (album != null)
        {
            _context.Albums.Remove(album);
            
            await _context.SaveChangesAsync();
        }
    }
}