using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotifyDecisionHelper.DB.Entities
{

    public class Track
    {
        public string UserId { get; set; }
        public string TrackId { get; set; }
        
        public string Name { get; set; }
        public int Rating { get; set; }
        
        public string AlbumId { get; set; }
        public Album Album { get; set; }
    }
}