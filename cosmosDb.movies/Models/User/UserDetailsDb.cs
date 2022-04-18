using Newtonsoft.Json;

namespace cosmosDb.movies.Models.User
{
    public record UserDetailsDb
    (
         [property:JsonProperty("id")]
         Guid UserId,
         string UserName,
         string Firstname,
         string Surname,
         string EmailAddress,
         DateTimeOffset Birthdate,
         string[] FavouriteGenres,
         string[] AvoidGenres
        )
    {
        public string entityType => "Details";

    }
}
