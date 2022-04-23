using Microsoft.Azure.Cosmos;

namespace cosmosDb.movies.Repos
{
    public interface IMovieUserDb
    {
        MoviesRepository Movies { get; }
        UserRepository Users { get; }
    }


    internal class MovieUserAppDb : IMovieUserDb
    {
        public MovieUserAppDb(CosmosClient client, string databasename)
        {
            Movies = new(client, databasename);
            Users = new(client, databasename);
        }
        public MoviesRepository Movies { get; }

        public UserRepository Users { get; }
    }

    public static class MoviesAppFactory
    {
        public static IMovieUserDb GetApp(CosmosClient client, string dbName) => new MovieUserAppDb(client, dbName);
    }
}
