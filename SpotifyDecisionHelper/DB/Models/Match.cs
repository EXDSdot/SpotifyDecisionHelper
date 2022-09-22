#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SpotifyDecisionHelper.DB.Models;

public class Match
{
    public string UserId { get; init; }
    public int MatchId { get; init; }
    public ICollection<Track> Tracks { get; init; }

    public int Result { get; set; }
    
    public int BracketId { get; init; }
    public Bracket Bracket { get; set; }
    
}