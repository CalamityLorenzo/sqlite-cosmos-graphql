using Microsoft.Azure.Cosmos;
using sqlite.movies.Models;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace cosmosDb.movies
{
    public class AddToDb
    {
        private CosmosClient cm;
        private Database db;

        public AddToDb()
        {
            this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            //this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            this.db = cm.GetDatabase("movies");
        }

        public async Task MoviesReset()
        {
            try
            {
                var container = db.GetContainer("movies");
                await container.DeleteContainerAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex);
            }

        }

        public async Task AddMovies(MovieDb[] movies)
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/entityType" });
            var container = containerResponse.Container;
            var counter = 0;
            foreach (var movie in movies)
            {
                await container.CreateItemAsync(movie, new PartitionKey(movie.yearReleased));
                counter += 1;
                Debug.WriteLine(counter);
            }

        }



        private async Task updateMovieAsync(QueryDefinition qd, Action<MovieDb> updateMovieAction, Container container)
        {
            IReadOnlyList<FeedRange> feedRanges = await container.GetFeedRangesAsync();
            using (FeedIterator<MovieDb> feedIteraor = container.GetItemQueryIterator<MovieDb>(feedRanges[0], qd,
                null,
                new QueryRequestOptions
                {
                }))
            {
                while (feedIteraor.HasMoreResults)
                {
                    foreach (var item in await feedIteraor.ReadNextAsync())
                    {
                        updateMovieAction(item);
                        await container.UpsertItemAsync(item);
                    }
                }
            }
        }
    }
}
