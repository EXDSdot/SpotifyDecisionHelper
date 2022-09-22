namespace SpotifyDecisionHelper.DBLogic.Tracks;

public interface ITracksManager
{
    Task Add(string userId, string trackId, string albumId);
    Task Remove(string userId, string trackId);
}