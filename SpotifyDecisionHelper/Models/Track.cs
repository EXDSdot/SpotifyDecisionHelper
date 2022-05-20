using System.ComponentModel.DataAnnotations;

namespace SpotifyDecisionHelper.Models
{

    public class Track
    {
        
        [Required] public string? Name { set; get; }
        public string? ArtistName { set; get; }

        private static int? curr_id = 0;
        [Key] public int? TrackId { get; set; }

        public int? ArtistId;
        //public Album Album { set; get; }
        public Artist Artist { set; get; }

        public Track(Artist artist, string name)
        {
            Name = name;
            Artist = artist;
            this.TrackId = curr_id++;
        }

        /*public Track(string artistName, string name)
        {
            Artist_Name = artistName;
            Name = name;
        }*/

        public Track(string name, string artistName)
        {
            Name = name;
            ArtistName = artistName;
            this.TrackId = curr_id++;
        }
    }
}