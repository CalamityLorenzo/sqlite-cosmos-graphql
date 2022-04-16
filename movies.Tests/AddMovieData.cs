using cosmosDb.movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.Models;
using sqlite.movies.MovieCtx;
using System;
using System.Collections.Generic;
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
        public async Task CreateMovies()
        {
            try
            {
                MovieDbCtx ctx = new();
                var allMovies = ctx.Movies
                                    .ToList();

                List<MovieDb> nodes = new();
                var opts = new JsonSerializerOptions
                {
                    MaxDepth = 2
                };
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
            }
            catch (Exception ex)
            {
                ;
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
                movies.ForEach(async m =>
                {
                    var keywords = m.Keywords.Select(a => a.KeywordName).ToList();
                    await addTodb.UpdateKeywords(m.MovieId, keywords );

                });
                List<MovieDb> nodes = new();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
