namespace GraphQl.WebApi.GraphQl.Movies
{
    public record MovieGraphQl(
        Guid DbId,
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
        string[]? Keywords)
    {

        public Guid GraphId { get; set; } = DbId;
    }

}