using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DB.Models;
using SpotifyDecisionHelper.DBLogic.Matches;
using SpotifyDecisionHelper.DBLogic.Tracks;

namespace SpotifyDecisionHelper.DBLogic.Brackets;

public class BracketsManager : IBracketsManager
{
    private readonly ApplicationContext _context;
    private readonly IMatchesManager _matchesManager;
    private readonly ITracksManager _tracksManager;

    public BracketsManager(ApplicationContext context, IMatchesManager matchesManager, ITracksManager tracksManager)
    {
        _context = context;
        _matchesManager = matchesManager;
        _tracksManager = tracksManager;
    }
    
    public int CurrentBracket(string userId)
    {
        return _context.Brackets.Where(x => x.UserId == userId)
            .Max(x => (int?)x.BracketId) ?? 0;
    }

    public async Task AddNewBracket(string userId)
    {
        var bracket = new Bracket
        {
            UserId = userId,
            BracketId = CurrentBracket(userId) + 1
        };
        _context.Brackets.Add(bracket);

        var tracks1 = _context.Tracks
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Rating)
            .ToList()
            .Select((t,i) => new { Track = t, Index = i })
            .Where(ti => (ti.Index+1)%2 == 0 )
            .Select(ti => ti.Track)
            .ToList();
        var tracks2 = _context.Tracks
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Rating)
            .ToList()
            .Select((t,i) => new { Track = t, Index = i })
            .Where(ti =>  ti.Index%2 == 0 )
            .Select(ti => ti.Track)
            .ToList();
        
        foreach (var track1 in tracks1)
        {
            var eligible = tracks2.FirstOrDefault(track2 => !_matchesManager.DoesExist(userId, track1, track2)
                                                     && !_tracksManager.IsMatched(userId, bracket.BracketId, track2.TrackId));
            if (eligible != null)
            {
                tracks2.Remove(eligible);
                await _matchesManager.AddMatch(userId, bracket.BracketId, track1, eligible);
            }
        }

        foreach (var track in _tracksManager.GetUnmatched(userId, bracket.BracketId))
        {
            track.Rating++;
        }

        await _context.SaveChangesAsync();
    }
}