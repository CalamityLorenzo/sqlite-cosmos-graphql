using Newtonsoft.Json;

namespace cosmosDb.movies.Models.Movies
{
    public record MovieCastDb([property: JsonProperty("id")] Guid movieId, CastDb[] Cast)
    {
        public string entityType => "Cast";
    }
    public record CastDb(
        string PersonId,
        string PersonName,
        string CharacterName,
        string Gender,
        int CastOrder);
}
