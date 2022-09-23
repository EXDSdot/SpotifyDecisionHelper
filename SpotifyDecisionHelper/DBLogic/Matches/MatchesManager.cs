using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Models;
using SpotifyDecisionHelper.Migrations;

namespace SpotifyDecisionHelper.DBLogic.Matches;

public class MatchesManager : IMatchesManager
{
    private readonly ApplicationContext _context;

    public MatchesManager(ApplicationContext context)
    {
        _context = context;
    }
    
    public int LastMatch(string userId)
    {
        return _context.Matches.Where(m => m.UserId == userId)
            .Max(x => (int?)x.MatchId) ?? 0;
    }
    public bool DoesExist(string userId, Track track1, Track track2)
    {
        return _context.Matches.FirstOrDefault(m => m.UserId == userId 
                                                    && m.Tracks.Any(t => t.TrackId == track1.TrackId)
                                                    && m.Tracks.Any(t => t.TrackId == track2.TrackId)) != null;
    }

    public async Task AddMatch(string userId, int bracketId, Track track1, Track track2)
    {
        var match = new Match
        {
            UserId = userId, 
            MatchId = LastMatch(userId)+1,
            BracketId = bracketId,
            Tracks = new List<Track>() {track1, track2}
        };
        _context.Matches.Add(match);
        await _context.SaveChangesAsync();
    }
    public Match? GetNextMatch(string userId, int bracketId)
    {
        var match = _context.Matches
            .Include(m => m.Tracks)
            .FirstOrDefault(m => m.UserId == userId && m.Result == 0 && m.BracketId == bracketId);
        return match;
    }

    public void ApplyResult(string userId, int matchId, int result)
    {
        var match = _context.Matches
            .Include(m => m.Tracks)
            .FirstOrDefault(m => m.UserId == userId && m.MatchId == matchId);
        match.Result = result;
        
        var track = match.Tracks.ToArray()[match.Result-1];
        track.Rating += 3;
        
        _context.Entry(track)
            .Reference(t => t.Album)
            .Load();
        var album = track.Album;
        album.Rating += 2;
        
        _context.Entry(album)
            .Collection(a => a.Tracks)
            .Load();
        foreach (var t in album.Tracks.Where(t => t.TrackId != track.TrackId))
        {
            t.Rating += 2;
        }
        
        _context.Entry(album)
            .Reference(a => a.Artist)
            .Load();
        var artist = album.Artist;
        artist.Rating++;
        
        _context.Entry(artist)
            .Collection(a => a.Albums)
            .Load();
        foreach (var a in artist.Albums.Where(a => a.AlbumId != album.AlbumId))
        {
            a.Rating++;
            
            _context.Entry(a)
                .Collection(a => a.Tracks)
                .Load();
            foreach (var t in album.Tracks)
            {
                t.Rating++;
            }
        }

        _context.SaveChanges();
    }
}