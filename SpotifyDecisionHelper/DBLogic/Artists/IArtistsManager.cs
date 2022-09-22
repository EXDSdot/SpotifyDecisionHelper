namespace SpotifyDecisionHelper.DBLogic.Artists;

public interface IArtistsManager
{
    Task Add(string userId, string artistId);
    Task Remove(string userId, string artistId);
}