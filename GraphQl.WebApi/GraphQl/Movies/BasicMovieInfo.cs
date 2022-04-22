namespace GraphQl.WebApi.GraphQl.Movies
{
    public record BasicMovieInfo (string Id, string Title, int YearReleased, long Budget, string Tagline, string Overview)
    {
    }
}
