using System.ComponentModel.DataAnnotations;

namespace SpotifyDecisionHelper.DB.Entities
{
    public class Album
    {
        public string UserId { get; set; }
        [Key]
        public string AlbumId { get; set; }
        
        public string Name { get; set; }
        public int Rating { get; set; }
        
        public string ArtistId { get; set; }
        public Artist Artist { get; set;}

        public ICollection<Track>? Tracks { get; set; }
    }
}