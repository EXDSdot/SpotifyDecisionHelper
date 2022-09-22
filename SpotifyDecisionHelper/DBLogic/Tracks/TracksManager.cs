using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Models;

namespace SpotifyDecisionHelper.DBLogic.Tracks;

public class TracksManager : ITracksManager
{
    private readonly ApplicationContext _context;

    public TracksManager(ApplicationContext context)
    {
        _context = context;
    }
    public async Task Add(string userId, string trackId, string albumId)
    {
        if (await _context.Tracks.FindAsync(userId, trackId) != null)
            return;
        
        var track = new Track
        {
            UserId = userId,
            TrackId = trackId,
            AlbumId = albumId,
        };
        _context.Tracks.Add(track);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(string userId, string trackId)
    {
        var track = _context.Tracks.FirstOrDefault(g => g.TrackId == trackId && g.UserId == userId);
        
        if (track != null)
        {
            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
        }
    }

    public bool IsMatched(string userId, int bracketId, string trackId)
    {
        return _context.Tracks.FirstOrDefault(t => t.Matches.Any(m => m.UserId == userId && m.BracketId == bracketId)) != null;
    }
    public List<Track> GetUnmatched(string userId, int bracketId)
    {
        return _context.Tracks.Where(t => t.Matches.All(m => m.BracketId != bracketId)).ToList();
    }
}