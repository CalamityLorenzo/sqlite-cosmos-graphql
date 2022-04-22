using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Movies;

namespace GraphQl.WebApi.GraphQl
{
    public class MovieInfoDataLoader : BatchDataLoader<string, MovieInfo>
    {
        private IMovieUserDb dbCtx;

        public MovieInfoDataLoader(IBatchScheduler batchScheduler, IMovieUserDb dbCtx) : base(batchScheduler) => this.dbCtx = dbCtx;
        protected override async Task<IReadOnlyDictionary<string, MovieInfo>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            var defId = Guid.Parse(keys.First();
            await dbCtx.Movies.GetMovieKeywords(defId);
            await dbCtx.Movies.GetMovieGenres(defId);
        }
    }
}
