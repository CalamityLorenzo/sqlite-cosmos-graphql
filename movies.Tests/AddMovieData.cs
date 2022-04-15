using cosmosDb.movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.Context;
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
            MoviesDbCtx ctx = new();
            var allMovies = ctx.Movies
                                .ToList();
            var allGenres = ctx.Genres.ToList();
            var allKeyWords = ctx.Keywords.ToList();

            var allGenreMovies = ctx.MovieGenres.ToList();
            var allKeywordMovies = ctx.MovieKeywords.ToList();

            List<JsonDocument> documents = new();
            var opts = new JsonSerializerOptions
            {
                MaxDepth = 2
            };
            foreach (var item in allMovies)
            {
                documents.Add(System.Text.Json.JsonSerializer.SerializeToDocument(new { item, Keywords = allKeywordMovies.Where(a => a.MovieId == item.MovieId).Select(b => allKeyWords.Any(c => c.KeywordId == b.KeywordId)) }, opts));
            }

            AddToDb addTodb = new();

            await addTodb.AddMovies(documents.ToArray());
        }
    }
}
