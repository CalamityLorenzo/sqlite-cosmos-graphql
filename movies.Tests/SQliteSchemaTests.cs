using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.Context;
using System.Diagnostics;
using System.Linq;

namespace movies.Tests
{
    [TestClass]
    public class SQliteSchemaTests
    {
        [TestMethod]
        public void MoviesAvailable()
        {
            MoviesDbCtx ctx = new();
            var cnt = ctx.Movies.Count();
            Debug.WriteLine(cnt);
            Assert.IsTrue(cnt > 1);
        }

        [TestMethod]
        public void MoviesGenres()
        {
            MoviesDbCtx ctx = new();
            var allGenres = ctx.Movies
                                .Include(a => a.Genres).Select(a => a.Genres).ToList();

            //var things = ctx.MovieGenres.Include(a => a.Movie).ToList();

            var cnt = allGenres.Count();
            Debug.WriteLine(cnt);
            Assert.IsTrue(cnt > 1);
        }

        [TestMethod]
        public void MovieKeywords()
        {
            MoviesDbCtx ctx = new();
            var allKeywords = ctx.Movies
                                .Include(a => a.Keywords).Select(a => a.Keywords).ToList();

            //var things = ctx.MovieGenres.Include(a => a.Movie).ToList();

            var cnt = allKeywords.Count();
            Debug.WriteLine(cnt);
            Assert.IsTrue(cnt > 1);
        }

        [TestMethod]
        public void MovieCast()
        {
            MoviesDbCtx ctx = new();
            var alLCast = ctx.Movies
                                .Include(a => a.Cast).Select(a => a.Cast).ToList();

            //var things = ctx.MovieGenres.Include(a => a.Movie).ToList();

            var cnt = alLCast.Count();
            Debug.WriteLine(cnt);
            Assert.IsTrue(cnt > 1);
        }
    }
}