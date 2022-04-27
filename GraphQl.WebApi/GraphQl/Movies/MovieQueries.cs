using cosmosDb.movies.Repos;

namespace GraphQl.WebApi.GraphQl.Movies
{
    [ExtendObjectType("Query")]
    public class MovieQueries
    {
        [UsePaging(MaxPageSize =10)]
        public async Task<IEnumerable<MovieGraphQl>> MoviesByTitleDescription(MoviesByTitleDescriptionInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            var result = await repo.Movies.SearchMoviesByTitleDescription(input.SearchTerms);
            return result.Select(mov =>
            new MovieGraphQl(
                DbId: mov.id,
                Title: mov.Title,
                Homepage: mov.Homepage,
                YearReleased: mov.yearReleased,
                Budget: mov.Budget,
                Tagline: mov.Tagline,
                Overview: mov.Overview,
                Revenue: mov.Revenue,
                MovieStatus: mov.MovieStatus,
                Runtime: mov.Runtime,
                VoteCount: mov.VoteCount,
                Genres: null,
                Keywords: null
                ));
        }

        public async Task<MovieGraphQl> GetMovieById(MovieByIdInput input,
            [Service] IMovieUserDb repo,
            [DataLoader] MovieGraphQlByIdDataLoader dl,
            CancellationToken ct)
        {
            return await dl.LoadAsync(input.MovieId);
        }

    }

}