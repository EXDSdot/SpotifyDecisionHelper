using SpotifyDecisionHelper.DB.Entities;
using SpotifyDecisionHelper.Models;

namespace SpotifyDecisionHelper.DBLogic.Tracks;

public interface ITracksManager
{
    Task<IList<Track>> GetAll();
    Task<Track> FindOrCreate(CreateTrackRequest request);
    Task Delete(string userId, string trackId);
}