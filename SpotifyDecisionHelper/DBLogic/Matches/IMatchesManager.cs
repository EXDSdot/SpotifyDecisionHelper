using SpotifyDecisionHelper.DB.Models;

namespace SpotifyDecisionHelper.DBLogic.Matches;

public interface IMatchesManager
{
    int LastMatch(string userId);
    bool DoesExist(string userId, Track track1, Track track2);

    Task AddMatch(string userId, int bracketId, Track track1, Track track2);
    (string, string)? GetNextMatch(string userId, int bracketId);
}