using GraphQl.WebApi.GraphQl.Common;

namespace GraphQl.WebApi.GraphQl.Movies
{
    public class AddMoviePayload : BasePayload
    {
        public MovieGraphQl Movie { get; }
        public AddMoviePayload(IEnumerable<UserError> errors) : base(errors)
        {
        }

        public AddMoviePayload(MovieGraphQl movie) : base(Enumerable.Empty<UserError>()) => this.Movie = movie;

    }
}
