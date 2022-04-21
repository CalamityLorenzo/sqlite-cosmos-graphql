using cosmosDb.movies.Repos;

namespace GraphQl.WebApi.GraphQl.Movies
{
    [ExtendObjectType("Query")]
    public class MoviesQueries
    {
        public async Task<IEnumerable<BasicMovieInfo>> MoviesByTitleDescription(MoviesByTitleDescriptionInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            var result = await repo.Movies.SearchMoviesByTitleDescription(input.SearchTerms);

            return result.Select(mov =>
            new BasicMovieInfo
            {
                
            }).ToList();
        }
    }
}
