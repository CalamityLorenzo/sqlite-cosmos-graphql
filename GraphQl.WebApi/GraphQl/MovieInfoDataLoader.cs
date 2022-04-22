using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Movies;
using System.Collections.ObjectModel;

namespace GraphQl.WebApi.GraphQl
{
    public class MovieInfoDataLoader : BatchDataLoader<string, MovieInfo>
    {
        private IMovieUserDb dbCtx;

        public MovieInfoDataLoader(IBatchScheduler batchScheduler, IMovieUserDb dbCtx) : base(batchScheduler) => this.dbCtx = dbCtx;
        protected override async Task<IReadOnlyDictionary<string, MovieInfo>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            var blah = keys.Select(k => dbCtx.Movies.GetMovie(Guid.Parse(k)));
            var x = await Task.WhenAll(blah.ToArray());
            return new ReadOnlyDictionary<string, MovieInfo>(x.ToDictionary(a => a.id.ToString(), result => new MovieInfo(Id: result.id.ToString(), Title: result.Title,
                YearReleased: result.yearReleased,
                Budget: result.Budget,
                Tagline: result.Tagline,
                Overview: result.Overview,
                Revenue: result.Revenue,
                MovieStatus: result.MovieStatus,
                Runtime: result.Runtime,
                VoteCount: result.VoteCount,
                Genres: new string[0],
                Keywords: new string[0])));
        }

    }
}
