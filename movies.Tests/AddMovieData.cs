using cosmosDb.movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.Models;
using sqlite.movies.MovieCtx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace movies.Tests
{
    [TestClass]
    public class AddMovieData
    {
        [TestMethod]

        public async Task ResetMovies()
        {
            AddToDb addTodb = new();
            await addTodb.MoviesReset();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task CreateMovies()
        {
            try
            {
                DbContextOptionsBuilder<MovieDbCtx> opts = new DbContextOptionsBuilder<MovieDbCtx>();
                opts.UseSqlite("Data Source=../../../../movies.db");
                opts.LogTo((str) => Debug.WriteLine(str));

                MovieDbCtx ctx = new(opts.Options);
                var allMovies = ctx.Movies
                                    .ToList();

                List<MovieDb> nodes = new();

                foreach (var item in allMovies)
                {
                    var str = Encoding.UTF8.GetString(item.ReleaseDate, 0, item.ReleaseDate.Length);
                    if (str != "0000-00-00")
                    {
                        var funkyChicken = new MovieDb
                        {
                            id = Guid.NewGuid(),
                            MovieId = item.MovieId,
                            Title = item.Title,
                            Budget = item.Budget,
                            Popularity = item.Popularity,
                            Homepage = item.Homepage,
                            ReleaseDate = DateTime.Parse(str),
                            yearReleased = DateTime.Parse(str).Year,
                            Revenue = item.Revenue.Value,
                            Runtime = item.Runtime.Value,
                            Tagline = item.Tagline,
                            Overview = item.Overview,
                            VoteAverage = item.VoteAverage,
                            VoteCount = item.VoteCount,
                            Genres = new List<string>(),
                            Keywords = new List<string>(),
                            MovieStatus = item.MovieStatus
                        };
                        nodes.Add(funkyChicken);
                    }


                }
                AddToDb addTodb = new();
                await addTodb.AddMovies(nodes.ToArray());
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                ;
                Assert.IsTrue(false);
            }
        }
        [TestMethod]
        public async Task CreateKeywords()
        {
            try
            {

                MovieDbCtx ctx = new();
                var movies = ctx.Movies.
                                        Include(a => a.Keywords).ToList();
                AddToDb addTodb = new();
                foreach (var movie in movies)
                {
                    var keywords = movie.Keywords.Select(a => a.KeywordName).ToList();
                    await addTodb.UpdateKeywords(movie.MovieId, keywords);

                };
                List<MovieDb> nodes = new();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [TestMethod]
        public async Task CreateGenres()
        {
            try
            {

                MovieDbCtx ctx = new();
                var movies = ctx.Movies.
                                        Include(a => a.Genres).ToList();

                AddToDb addToDb = new();
                foreach (var movie in movies)
                {
                    var genres = movie.Genres.Select(a => a.GenreName).ToList();
                    await addToDb.UpdateGenres(movie.MovieId, genres);
                }
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}