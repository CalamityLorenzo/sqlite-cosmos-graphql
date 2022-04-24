using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Movies;
using System.Collections.ObjectModel;

namespace GraphQl.WebApi.GraphQl
{
    public class MovieGraphQlByIdDataLoader : BatchDataLoader<Guid, MovieGraphQl>
    {
        private IMovieUserDb dbCtx;

        public MovieGraphQlByIdDataLoader(IBatchScheduler batchScheduler, IMovieUserDb dbCtx) : base(batchScheduler) => this.dbCtx = dbCtx;
        protected override async Task<IReadOnlyDictionary<Guid, MovieGraphQl>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var blah = keys.Select(k => dbCtx.Movies.GetMovie(k));
            var x = await Task.WhenAll(blah.ToArray());
            return new ReadOnlyDictionary<Guid, MovieGraphQl>(x.ToDictionary(a => a.id,
                result => new MovieGraphQl(
                        DbId: result.id,
                        Title: result.Title,
                        Homepage: result.Homepage,
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
