using Newtonsoft.Json;

namespace cosmosDb.movies.Models.User
{
    public record UserReviewsDb(ReviewDb[] reviews, [property: JsonProperty("id")] Guid UserId)
    {
        public  string entityType => "Reviews";
    }

    public record ReviewDb(Guid movieId, string MovieName, string YearReleased, Guid userId, string userName, string ReviewContent, double rating);
}
