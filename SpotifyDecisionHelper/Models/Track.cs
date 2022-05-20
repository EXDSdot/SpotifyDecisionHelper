using System.ComponentModel.DataAnnotations;

namespace SpotifyDecisionHelper.Models
{

    public class Track
    {
        
        [Key] public string Name { set; get; }
        [Required]
        public string ArtistName { set; get; }

        private static int curr_id = 0;
        [Required] public int Id;

        [Required] public int ArtistId;
        //public Album Album { set; get; }
        //public Artist Artist { set; get; }

        /*public Track(Artist artist, string name)
        {
            Name = name;
            Artist = artist;
            this.Id = curr_id++;
        }*/

        /*public Track(string artistName, string name)
        {
            Artist_Name = artistName;
            Name = name;
        }*/

        public Track(string name, string artistName)
        {
            Name = name;
            ArtistName = artistName;
            this.Id = curr_id++;
        }
    }
}