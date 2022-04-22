namespace GraphQl.WebApi.GraphQl.Movies
{
    public record MovieInfo(string Id,
        string Title,
        int YearReleased,
        long Budget,
        string Tagline,
        string Overview,
        long Revenue,
        long Runtime,
        string MovieStatus,
        long VoteCount,
        string Genres,
        string Keywords);

}