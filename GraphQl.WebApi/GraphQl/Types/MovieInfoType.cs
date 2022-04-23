using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Movies;

namespace GraphQl.WebApi.GraphQl.Types
{
    public class MovieInfoType : ObjectType<MovieInfo>
    {

        protected override void Configure(IObjectTypeDescriptor<MovieInfo> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id) // Ctx == middleware service inhector
                .ResolveNode((ctx, id) => ctx.DataLoader<MovieInfoDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.Genres)
                .ResolveWith<MovieInfoResolvers>(t => t.GetGenresAsync(default!, default!, default));
                

            descriptor
                .Field(t => t.Keywords)
                .ResolveWith<MovieInfoResolvers>(t => t.GetKeywordsAsync(default!, default!, default));

        }



        private class MovieInfoResolvers
        {
            // This signature can be anything as longs as the descriptor can resolve it. (except the ct that's required.)
            public async Task<string[]> GetGenresAsync(
                    [Parent] MovieInfo info,
                    [Service] IMovieUserDb repos,
                    CancellationToken ct)
            {
                return await repos.Movies.GetMovieGenres(Guid.Parse(info.Id));
            }

            // This signature can be anything as longs as the descriptor can resolve it. (except the ct that's required.)
            public async Task<string[]> GetKeywordsAsync(
                    [Parent] MovieInfo info,
                    [Service] IMovieUserDb repos,
                    CancellationToken ct)
            {
                return await repos.Movies.GetMovieKeywords(Guid.Parse(info.Id));
            }
        }
    }
}
