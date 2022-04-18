using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosDb.movies.Models.User
{
    public record UserDetailsDb
    (
         Guid UserId,
         string UserName,
         string Firstname,
         string Surname,
         DateTimeOffset Birthdate,
         string[] FavouriteGenres,
         string[] AvoidGenres
        )
    {
        public string userEntity => "UserDetails";

    }
}
