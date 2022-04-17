using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosDb.movies.Models.User
{
    public record UserReviews(Review[] reviews, string UserName)
    {
        internal string id => "Reviews";
    }

    public record Review(Guid movieId, string MovieName, string YearReleased, string ReviewContent, double rating);
}
