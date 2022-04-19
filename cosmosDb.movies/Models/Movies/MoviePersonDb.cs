using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosDb.movies.Models.Movies
{
    public record MoviePersonDb(
        Guid id,
        int PersonId,
        string Name
        )
    {
        public string entityType => "MoviePerson";
    }
}
