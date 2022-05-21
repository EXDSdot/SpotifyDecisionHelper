namespace SpotifyDecisionHelper.Models;

public class CreateTrackRequest
{
    public string UserId { get; set; }
    public string TrackId { get; set; }
        
    public string Name { get; set; }
    public int Rating { get; set; }
        
    public string AlbumId { get; set; }
}