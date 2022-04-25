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
        private DateTimeOffset birthday;

     //   public UserDetailsDb(string username, string firstname, string surname, string emailAddress, DateTimeOffset birthday) : this(Guid.Empty, username, firstname, surname, emailAddress, birthday, new string[0], new string[0]) { }


        public string entityType => "Details";

    }
}
