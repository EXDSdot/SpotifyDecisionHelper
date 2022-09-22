namespace SpotifyDecisionHelper.DBLogic.Albums;

public interface IAlbumsManager
{
    Task Add(string userId, string albumId, string artistId);
    Task Remove(string userId, string albumId);
}