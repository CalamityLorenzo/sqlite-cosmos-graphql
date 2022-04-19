using cosmosDb.movies;
using cosmosDb.movies.Models.Movies;
using Microsoft.Azure.Cosmos;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.Models;
using sqlite.movies.MovieCtx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies.Tests
{
    [TestClass]
    public class CreateMovieData
    {
        [TestMethod("0. Reset Movies")]
        public async Task ResetMovies()
        {
            Raw_Repo addTodb = new();
            await addTodb.MoviesReset();
            Assert.IsTrue(true);
        }

        [TestMethod("1. Create Movies")]
        public async Task CreateMovies()
        {
            try
            {
                MovieDbCtx ctx = new();
                var allMovies = ctx.Movies
                                    .ToList();

                List<MovieDb> movies = new();

                foreach (var item in allMovies)
                {
                    var str = Encoding.UTF8.GetString(item.ReleaseDate, 0, item.ReleaseDate.Length);
                    if (str != "0000-00-00")
                    {
                        var funkyChicken = new MovieDb
                        (
                            id: Guid.NewGuid(),
                            MovieId: item.MovieId,
                            Title: item.Title,
                            Budget: item.Budget,
                            Popularity: item.Popularity,
                            Homepage: item.Homepage,
                            ReleaseDate: DateTime.Parse(str),
                            yearReleased: DateTime.Parse(str).Year,
                            Revenue: item.Revenue.Value,
                            Runtime: item.Runtime.Value,
                            Tagline: item.Tagline,
                            Overview: item.Overview,
                            VoteAverage: item.VoteAverage,
                            VoteCount: item.VoteCount,
                            MovieStatus: item.MovieStatus
                        );
                        movies.Add(funkyChicken);
                    }


                }
                MoviesRepository moviesRepo = new();
                foreach (var item in movies)
                {
                    await moviesRepo.AddNewMovie(item);
                }
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {

                throw;
                Assert.IsTrue(false);
            }
        }
        [TestMethod("2. Create keywords")]
        public async Task CreateKeywords()
        {
            try
            {

                MovieDbCtx ctx = new();
                //var movies = ctx.Movies.ToList();
                MoviesRepository movieRepo = new();

                SqliteConnection sql = new SqliteConnection("Data Source=../../../../movies.db");

                using SqliteCommand cmd = sql.CreateCommand();
                sql.Open();
                cmd.CommandText = @"Select mk.movie_id as movieid, k.keyword_name  as keyword
                                    from keyword k inner join movie_keywords mk on mk.keyword_id = k.keyword_id";
                var data = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);

                int currentMovieId = 0;
                Dictionary<int, List<string>> movieDict = new();
                while (data.Read())
                {
                    // add new entry 
                    if (currentMovieId != data.GetInt32(0))
                    {
                        currentMovieId = data.GetInt32(0);
                        var keywords = new List<string>() { data.GetString(1) };
                        movieDict.Add(currentMovieId, keywords);
                    }
                    else
                    {
                        var entry = movieDict[currentMovieId];
                        entry.Add(data.GetString(1));
                    }

                }



                foreach (var movie in movieDict)
                {
                    MovieDb movieDb = await movieRepo.GetMovieByOldId(movie.Key);
                    var keywords = new MovieKeywordDb
                    (
                        id: movieDb.id,
                        Keywords: movie.Value.ToArray()
                    );
                    await movieRepo.AddMovieKeywords(movieDb.id, keywords);
                };
                List<MovieDb> nodes = new();
            }
            catch (CosmosException)
            {
                throw;
            }
        }
        [TestMethod("3. Create Genres")]
        public async Task CreateGenres()
        {
            try
            {
                SqliteConnection sql = new SqliteConnection("Data Source=../../../../movies.db");

                using SqliteCommand cmd = sql.CreateCommand();
                sql.Open();
                cmd.CommandText = @"Select mk.movie_id as movieid, k.genre_name  as keyword
                                    from genre k inner join movie_genres mk on mk.genre_id = k.genre_id";
                var data = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);

                int currentMovieId = 0;
                Dictionary<int, List<string>> movieDict = new();
                while (data.Read())
                {
                    // add new entry 
                    if (currentMovieId != data.GetInt32(0))
                    {
                        currentMovieId = data.GetInt32(0);
                        var keywords = new List<string>() { data.GetString(1) };
                        movieDict.Add(currentMovieId, keywords);
                    }
                    else
                    {
                        var entry = movieDict[currentMovieId];
                        entry.Add(data.GetString(1));
                    }

                }
                MoviesRepository movieRepo = new();
                foreach (var movie in movieDict)
                {
                    MovieDb movieDb = await movieRepo.GetMovieByOldId(movie.Key);
                    var keywords = new MovieGenreDb
                    (
                        id: movieDb.id,
                        Genres: movie.Value.ToArray()
                    );
                    Debug.WriteLine(movie);
                    await movieRepo.AddMovieGenres(movieDb.id, keywords);
                }
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod("4. Create People")]
        public async Task CreatePeople()
        {
            MoviesRepository repo = new();

            MovieDbCtx ctx = new MovieDbCtx();
            var allPeople = ctx.People.ToList();

            foreach (var person in allPeople)
            {
                MoviePersonDb moviePerson = new(
                    Guid.NewGuid(),
                    (int)person.PersonId,
                    person.PersonName!
                    );
                await repo.AddPerson(moviePerson);
            }
        }

        [TestMethod("6. Search by title/overview")]
        public async Task SearchMovies()
        {
            MoviesRepository repo = new();
            var results = await repo.SearchMoviesByTitleDescription("ome");

            Assert.IsTrue(results.Count > 1000);
        }

    }
}