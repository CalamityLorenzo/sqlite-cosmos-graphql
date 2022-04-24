namespace GraphQl.WebApi.GraphQl.Movies
{
    public record UpdateKeywordInput(
        string MovieId,
        string[] Keywords
    );
}
