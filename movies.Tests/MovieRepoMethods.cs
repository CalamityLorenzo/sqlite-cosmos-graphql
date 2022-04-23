using cosmosDb.movies.Repos;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.Models;
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


        [TestMethod("Create a new movie")]
        public async Task AddMovie()
        {
            IMovieUserDb app = MoviesAppFactory.GetApp(_client, dbName);

            var newMovie = await app.Movies.AddMovie(new MovieDb(Guid.NewGuid(),
                                  "Made up Movie",
                                  1234567,
                                  "https://madeupwebsie.com",
                                  "This is the overview of a movie paulito, pancakes, polenta",
                                  4.2D,
                                  DateTime.Parse("14/01/2001"),
                                  2001,
                                  150000000,
                                  98,
                                  "This movie is open",
                                  "Just when you article",
                                  1.2D, 176000, 01));
            Assert.IsNotNull(newMovie);
        }

        [TestMethod]
        public async Task RemoveMovie()
        {
            var app = MoviesAppFactory.GetApp(_client, dbName);
            var list = await app.Movies.SearchMoviesByTitleDescription("paulito, pancakes, polenta");

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);

            // we are going to delete all instances of the movie
            // And the genre keywords and cast
            foreach (var movie in list)
            {
                await app.Movies.DeleteMovie(movie.id);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task UpdateMovie()
        {
            IMovieUserDb app = MoviesAppFactory.GetApp(_client, dbName);
            var list = await app.Movies.SearchMoviesByTitleDescription("paulito, pancakes, polenta");
            Assert.IsNotNull(list);
            if (list.Count == 0)
            {
                await AddMovie();
                list = await app.Movies.SearchMoviesByTitleDescription("paulito, pancakes, polenta");
            }

            Assert.IsTrue(list.Count > 0);

            var movieToUpdate = list.First();
            var budget = movieToUpdate.Budget;
            var newTitle = "The pickles of why";
            movieToUpdate = movieToUpdate with { Budget = movieToUpdate.Budget + 10, Title = newTitle };

            var updatedMovie = await app.Movies.UpdateMovie(movieToUpdate);
            Assert.IsTrue(updatedMovie.Title == newTitle);
            Assert.IsTrue(updatedMovie.Budget == budget + 10);

        }


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
