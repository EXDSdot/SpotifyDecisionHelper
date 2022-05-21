using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Entities;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.DBLogic.Tracks;

public class TracksManager : ITracksManager
{
    private readonly ApplicationContext _context;

    public TracksManager(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IList<Track>> GetAll() => await _context.Tracks.ToListAsync();

    public async Task<Track> FindOrCreate(CreateTrackRequest request)
    {
        var track = await _context.Tracks.FirstOrDefaultAsync(a=>a.UserId == request.UserId && a.TrackId == request.TrackId);

        if (track != null) return track;
        
        track = new Track
        {
            UserId = request.UserId,
            TrackId = request.TrackId,
            Name = request.Name,
            Rating = request.Rating,
            AlbumId = request.AlbumId
        };

        _context.Tracks.Add(track);

        await _context.SaveChangesAsync();

        return track;
    }

    public async Task Delete(string userId, string trackId)
    {
        var track = _context.Tracks.FirstOrDefault(g => g.TrackId == trackId && g.UserId == userId);
        
        if (track != null)
        {
            _context.Tracks.Remove(track);
            
            await _context.SaveChangesAsync();
        }
    }
}