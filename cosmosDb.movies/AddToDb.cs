using Microsoft.Azure.Cosmos;
using sqlite.movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        public async Task AddMovies(JsonDocument[] movies)
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/yearReleased" });
            var container = containerResponse.Container;

            foreach (var movie in movies)
            {
                //var obj = System.Text.Json.JsonSerializer.Deserialize<Object>(movie);
                await container.CreateItemAsync(movie.ToString());
            }

        }
    }
}
