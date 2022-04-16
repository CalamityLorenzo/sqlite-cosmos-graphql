using Microsoft.Azure.Cosmos;
using sqlite.movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosDb.movies
{
    internal class MoviesRepository
    {
        private CosmosClient cm;
        private Database db;

        public MoviesRepository()
        {
            this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            //this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            this.db = cm.GetDatabase("movies");
        }

        public async Task<MovieDb> AddNewMovie(MovieDb movie)
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/yearReleased" });
            var container = containerResponse.Container;
            var counter = 0;
            var repsonse = await container.CreateItemAsync(movie, new PartitionKey(movie.yearReleased));
            if (repsonse.StatusCode == System.Net.HttpStatusCode.OK)

                return movie;
            else throw new Exception("error", repsonse.Diagnostics);

        }
    }
}
