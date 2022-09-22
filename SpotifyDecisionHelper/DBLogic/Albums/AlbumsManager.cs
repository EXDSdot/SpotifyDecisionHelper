using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Models;

namespace SpotifyDecisionHelper.DBLogic.Albums;

public class AlbumsManager : IAlbumsManager
{
    private readonly ApplicationContext _context;

    public AlbumsManager(ApplicationContext context)
    {
        _context = context;
    }

    public async Task Add(string userId, string albumId, string artistId)
    {
        if (await _context.Albums.FindAsync(userId, albumId) != null)
            return;
        
        var album = new Album
        {
            UserId = userId,
            AlbumId = albumId,
            ArtistId = artistId,
        };
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(string userId, string albumId)
    {
        var album = _context.Albums.FirstOrDefault(g => g.AlbumId == albumId && g.UserId == userId);

        if (album != null)
        {
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
        }
    }
}