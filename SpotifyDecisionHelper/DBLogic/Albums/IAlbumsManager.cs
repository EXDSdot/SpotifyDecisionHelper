using SpotifyDecisionHelper.DB.Entities;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.DBLogic.Albums;

public interface IAlbumsManager
{
    Task<IList<Album>> GetAll();
    Task<Album> FindOrCreate(CreateAlbumRequest request);
    Task Delete(string userId, string albumId);
}