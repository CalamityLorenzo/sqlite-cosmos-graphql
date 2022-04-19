using Newtonsoft.Json;

namespace cosmosDb.movies.Models.Movies
{
    internal record CastDb(
        [property: JsonProperty("id")] Guid movieId,
        string PersonId,
        string PersonName,
        string CharacterName,
        string Gender,
        int CastOrder)
    {
        public string entityType => "Cast";
    }
}
