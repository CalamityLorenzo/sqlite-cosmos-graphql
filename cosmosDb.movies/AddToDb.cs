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
            this.db = cm.GetDatabase("movies");
        }
        public async Task AddMovies(MovieDb[] movies)
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/yearReleased" });
            var container = containerResponse.Container;
            var counter = 0;
            foreach (var movie in movies)
            {
                //var obj = System.Text.Json.JsonSerializer.Deserialize<Object>(movie);
                //JsonObject obj = movie.Root.AsObject();
                //obj.Add("id", Guid.NewGuid().ToString());
                //var base64Date = obj["ReleaseDate"].ToString();
                //var dateBytes  =System.Convert.FromBase64String(base64Date);
                //var dateString = System.Text.Encoding.UTF8.GetString(dateBytes);
                //var date = System.DateTime.Parse(dateString);
                //obj["ReleaseDate"] = date.ToShortDateString();
                //obj.Add("yearReleased", date.Year);
                //var node = JsonNode.Parse(obj.ToJsonString());
                await container.CreateItemAsync(movie, new PartitionKey(movie.yearReleased));
                counter += 1;
                Debug.WriteLine(counter);
            }

        }

        public async Task UpdateKeywords(long movieId, List<string> list)
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/yearReleased" });
            var container = containerResponse.Container;
            IReadOnlyList<FeedRange> feedRanges = await container.GetFeedRangesAsync();

            QueryDefinition qd = new QueryDefinition("Select * from movies where MovieId = @movieId").WithParameter("@movieId", movieId);
            using (FeedIterator<MovieDb> feedIteraor = container.GetItemQueryIterator<MovieDb>(feedRanges[0], qd,
                null,
                new QueryRequestOptions
                {
                }))
            {
                while (feedIteraor.HasMoreResults)
                {
                    foreach(var item in await feedIteraor.ReadNextAsync())
                    {
                        item.Keywords = list;
                        await container.UpsertItemAsync(item);
                    }
                }
            }

        }
    }
}
