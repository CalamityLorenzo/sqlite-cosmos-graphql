using Microsoft.Azure.Cosmos;
using sqlite.movies.Models;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace cosmosDb.movies
{
    public class Raw_Repo
    {
        private CosmosClient cm;
        private Database db;

        public Raw_Repo()
        {
            this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            //this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            this.db = cm.GetDatabase("movies");
        }

        public async Task ResetMovieContainer()
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

        public async Task UsersReset()
        {

            try
            {
                var container = db.GetContainer("users");
                await container.DeleteContainerAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex);
            }
        }

        public async Task DeleteKeywordPartition()
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/entityType" });
            var container = containerResponse.Container;
            MoviesRepository mr = new MoviesRepository(this.cm, "movies");
            // Get all the records
            var allKeywords = await mr.GetMovieKeywords();

            List<Task> tasks = new List<Task>();
            var partyKey = new PartitionKey("Keyword");
            foreach (var keyword in allKeywords)
            {
                tasks.Add(container.DeleteItemStreamAsync(keyword.id.ToString(), partyKey));
            }

            Task.WaitAll(tasks.ToArray());
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

        public async Task DeleteGenrePartition()
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/entityType" });
            var container = containerResponse.Container;
            MoviesRepository mr = new MoviesRepository(this.cm, "movies");
            // Get all the records
            var allGenres = await mr.GetMovieGenres();

            List<Task> tasks = new List<Task>();
            var partyKey = new PartitionKey("Genre");
            foreach (var genre in allGenres)
            {
                tasks.Add(container.DeleteItemStreamAsync(genre.id.ToString(), partyKey));
            }

            Task.WaitAll(tasks.ToArray());

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
