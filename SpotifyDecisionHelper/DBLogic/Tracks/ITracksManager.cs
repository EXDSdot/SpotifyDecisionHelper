using SpotifyDecisionHelper.DB.Models;

namespace SpotifyDecisionHelper.DBLogic.Tracks;

public interface ITracksManager
{
    Task Add(string userId, string trackId, string albumId);
    Task Remove(string userId, string trackId);
    bool IsMatched(string userId, int bracketId, string trackId);
    List<Track> GetUnmatched(string userId, int bracketId);
}