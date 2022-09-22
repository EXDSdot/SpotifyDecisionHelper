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
        return _context.Brackets.Max(x => (int?)x.BracketId) ?? 0;
    }

    public async Task AddNewBracket(string userId)
    {
        var bracket = new Bracket
        {
            UserId = userId,
            BracketId = CurrentBracket(userId) + 1
        };
        _context.Brackets.Add(bracket);

        var maxPairs = _context.Tracks
            .Count(t => t.UserId == userId)/2;
        var tracks1 = _context.Tracks
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Rating)
            .Take(maxPairs).ToList();
        var tracks2 = _context.Tracks
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Rating)
            .Skip(maxPairs).ToList();
        
        foreach (var track1 in tracks1)
        {
            foreach (var track2 in tracks2.Where(track2 => 
                         !_matchesManager.DoesExist(userId, track1, track2)
                         && !_tracksManager.IsMatched(userId, bracket.BracketId, track1.TrackId)))
            {
                await _matchesManager.AddMatch(userId, bracket.BracketId, track1, track2);
            }
        }

        foreach (var track in _tracksManager.GetUnmatched(userId, bracket.BracketId))
        {
            track.Rating++;
        }

        await _context.SaveChangesAsync();
    }
}