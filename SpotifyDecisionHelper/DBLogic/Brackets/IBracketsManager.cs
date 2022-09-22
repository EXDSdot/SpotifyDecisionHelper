namespace SpotifyDecisionHelper.DBLogic.Brackets;

public interface IBracketsManager
{
    public int CurrentBracket(string userId);
    public Task AddNewBracket(string userId);
}