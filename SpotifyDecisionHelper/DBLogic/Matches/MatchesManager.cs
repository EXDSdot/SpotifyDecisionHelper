using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Models;

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
    public (string, string)? GetNextMatch(string userId, int bracketId)
    {
        var match = _context.Matches.FirstOrDefault(m => m.UserId == userId && m.Result == 0);
        return (match != null ? (match.Tracks.First().TrackId, match.Tracks.Last().TrackId) : null);
    }
}