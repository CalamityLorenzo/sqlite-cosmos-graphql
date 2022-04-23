using cosmosDb.movies.Repos;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies.Tests
{
    [TestClass]
    public class MovieRepoMethods
    {
        private CosmosClient _client = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        private string dbName = "movies";

        [TestMethod]
        public async Task GetKeywordsForMovie()
        {
            IMovieUserDb app = MoviesAppFactory.GetApp(_client, dbName);

            var movies = (await app.Movies.GetMovieByName("Back to the Future")).ToList();

            Assert.IsNotNull(movies);
            Assert.IsTrue(movies.Count > 0);



            var keywords = await app.Movies.GetMovieKeywords(movies.First().id);

            Assert.IsNotNull(keywords);
            Assert.IsTrue(keywords.Length > 0);
        }

        [TestMethod]
        public async Task GetGenreForMovie()
        {
            IMovieUserDb app = MoviesAppFactory.GetApp(_client, dbName);

            var movies = (await app.Movies.GetMovieByName("Back to the Future")).ToList();

            Assert.IsNotNull(movies);
            Assert.IsTrue(movies.Count > 0);

            var genre = await app.Movies.GetMovieGenres(movies.First().id);

            Assert.IsNotNull(genre);
            Assert.IsTrue(genre.Length > 0);

        }
    }

}
