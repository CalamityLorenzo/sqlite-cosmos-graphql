using cosmosDb.movies.Repos;

namespace GraphQl.WebApi.GraphQl.Movies
{
    [ExtendObjectType("Query")]
    public class MovieQueries
    {
        public async Task<IEnumerable<BasicMovieInfo>> MoviesByTitleDescription(MoviesByTitleDescriptionInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            var result = await repo.Movies.SearchMoviesByTitleDescription(input.SearchTerms);
            return result.Select(mov =>
            new BasicMovieInfo(Id: mov.id.ToString(), Title: mov.Title, YearReleased: mov.yearReleased, Budget: mov.Budget, Tagline: mov.Tagline, Overview: mov.Overview)).ToList();
        }

        public async Task<MovieInfo> GetMovieById(MoviesByIdInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            var result = await repo.Movies.GetMovie(input.MovieId);
            return new MovieInfo(Id: result.id.ToString(), Title: result.Title,
                YearReleased: result.yearReleased,
                Budget: result.Budget,
                Tagline: result.Tagline,
                Overview: result.Overview,
                Revenue: result.Revenue,
                MovieStatus: result.MovieStatus,
                Runtime: result.Runtime,
                VoteCount: result.VoteCount,
                Genres: new string[0],
                Keywords: new string[0]);
        }

        public async Task<MovieInfo> GetMovieByDataLoaderId(MoviesByIdInput input,
            [Service] IMovieUserDb repo,
            [DataLoader] MovieInfoDataLoader dl,
            CancellationToken ct)
        {
            return await dl.LoadAsync(input.MovieId.ToString());
        }
    }

}