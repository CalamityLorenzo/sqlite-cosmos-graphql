using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosDb.movies.Models.User
{
    internal class UserDetailsDb
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public DateTimeOffset Birthdate { get; set; }
        public string[] FavouriteGenres { get; set; }
        public string[] AvoidGenres { get; set; }

    }
}
