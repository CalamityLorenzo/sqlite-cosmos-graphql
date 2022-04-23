using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Common;
using sqlite.movies.Models;

namespace GraphQl.WebApi.GraphQl.Movies
{
    [ExtendObjectType("Query")]
    public class MovieQueries
    {
        public async Task<IEnumerable<MovieGraphQl>> MoviesByTitleDescription(MoviesByTitleDescriptionInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            var result = await repo.Movies.SearchMoviesByTitleDescription(input.SearchTerms);
            return result.Select(mov =>
            new MovieGraphQl(
                Id: mov.id.ToString(), 
                Title: mov.Title,
                Homepage:mov.Homepage,
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

        public async Task<MovieGraphQl> GetMovieById(MovieByIdInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            var result = await repo.Movies.GetMovie(input.MovieId);
            return new MovieGraphQl(Id: result.id.ToString(), Title: result.Title,
                YearReleased: result.yearReleased,
                Homepage: result.Homepage,
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

        public async Task<MovieGraphQl> GetMovieByDataLoaderId(MovieByIdInput input,
            [Service] IMovieUserDb repo,
            [DataLoader] MovieGraphQlByIdDataLoader dl,
            CancellationToken ct)
        {
            return await dl.LoadAsync(input.MovieId.ToString());
        }

           }

}