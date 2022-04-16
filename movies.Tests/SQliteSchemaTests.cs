using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.MovieCtx;
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
            MovieDbCtx ctx = new();
            var cnt = ctx.Movies.Count();
            Debug.WriteLine(cnt);
            Assert.IsTrue(cnt > 1);
        }

        [TestMethod]
        public void MoviesGenres()
        {
            MovieDbCtx ctx = new();
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
            MovieDbCtx ctx = new();
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
            MovieDbCtx ctx = new();
            var alLCast = ctx.Movies
                                .Include(a => a.MovieCasts).Select(a => a.MovieCasts).ToList();

            //var things = ctx.MovieGenres.Include(a => a.Movie).ToList();

            var cnt = alLCast.Count();
            Debug.WriteLine(cnt);
            Assert.IsTrue(cnt > 1);
        }
    }
}