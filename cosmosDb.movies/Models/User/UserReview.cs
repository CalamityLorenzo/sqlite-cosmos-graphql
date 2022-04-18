using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosDb.movies.Models.User
{
    public record UserReviewsDb(ReviewDb[] reviews, string UserName)
    {
        internal string userEntity => "Reviews";
    }

    public record ReviewDb(Guid movieId, string MovieName, string YearReleased, string ReviewContent, double rating);
}
