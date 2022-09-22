namespace SpotifyDecisionHelper.DB.Models;

public class Bracket
{
    public string UserId { get; init; }
    public int BracketId { get; init; }
    
    public ICollection<Match>? Matches { get; set; }
}