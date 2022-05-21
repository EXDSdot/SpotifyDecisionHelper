using SpotifyDecisionHelper.DB.Entities;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.DBLogic.Artists;

public interface IArtistsManager
{
    Task<IList<Artist>> GetAll();
    Task<Artist> FindOrCreate(CreateArtistRequest request);
    Task Delete(string userId, string artistId);
}