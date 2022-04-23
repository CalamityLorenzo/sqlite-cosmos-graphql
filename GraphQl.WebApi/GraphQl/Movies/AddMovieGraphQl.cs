namespace GraphQl.WebApi.GraphQl.Movies
{
    public record AddMovieGraphQl(
        string Title,
        DateTimeOffset ReleaseDate,
        long Budget,
        string Homepage,
        string Tagline,
        string Overview,
        long Revenue,
        long Runtime,
        double VoteAverage,
        string MovieStatus,
        long VoteCount);



}
