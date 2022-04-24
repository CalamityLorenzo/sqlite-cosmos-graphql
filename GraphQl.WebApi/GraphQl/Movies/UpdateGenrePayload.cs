using GraphQl.WebApi.GraphQl.Common;

namespace GraphQl.WebApi.GraphQl.Movies
{
    public class UpdateGenrePayload : BasePayload
    {
        public UpdateGenrePayload(IEnumerable<UserError> errors) : base(errors)
        {
        }

        public UpdateGenrePayload(string[] Genres) : base(Enumerable.Empty<UserError>())
        {
            this.Genres = Genres;
        }

        public string[] Genres { get; } = new string[0];
    }
}
