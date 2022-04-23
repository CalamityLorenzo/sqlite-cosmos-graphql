namespace GraphQl.WebApi.GraphQl.Movies
{
    public record MovieGraphQl(string Id,
        string Title,
        int YearReleased,
        long Budget,
        string Homepage,
        string Tagline,
        string Overview,
        long Revenue,
        long Runtime,
        string MovieStatus,
        long VoteCount,
        string[]? Genres,
        string[]? Keywords);

}