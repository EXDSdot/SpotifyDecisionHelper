namespace SpotifyDecisionHelper.Models
{

    public class Track
    {
        public string Name { set; get; }
        //public Album Album { set; get; }
        public Artist Artist { set; get; }

        public Track(Artist artist, string name)
        {
            Name = name;
            Artist = artist;
        }
    }
}