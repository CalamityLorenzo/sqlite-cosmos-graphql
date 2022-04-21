using cosmosDb.movies.Repos;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CosmosDb.Movies
{
    public static class AspNetDi
    {
        public static IMovieUserDb InitializeMovieUserDb(IConfigurationSection configSection)
        {
            string dbName = configSection.GetSection("DbName").Value;
            string connStr = configSection.GetSection("ConnectionString").Value;

            CosmosClient client = new CosmosClient(connStr);
            return new MovieUserAppDb(client, dbName);
        }
    }
}
