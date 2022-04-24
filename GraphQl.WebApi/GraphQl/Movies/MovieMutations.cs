using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Common;
using sqlite.movies.Models;

namespace GraphQl.WebApi.GraphQl.Movies
{
    [ExtendObjectType("Mutation")]
    public class MovieMutations
    {
        public async Task<AddMoviePayload> AddMovie(AddMovieInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            var movie = input.Movie;
            try
            {
                var result = await repo.Movies.AddMovie(new MovieDb(
                      id: Guid.Empty,
                      Title: movie.Title,
                      Homepage: movie.Homepage,
                      Popularity: 0.0D,
                      ReleaseDate: movie.ReleaseDate,
                      yearReleased: movie.ReleaseDate.Year,
                      Budget: movie.Budget,
                      Tagline: movie.Tagline,
                      Overview: movie.Overview,
                      Revenue: movie.Revenue,
                      VoteAverage: movie.VoteAverage,
                      MovieStatus: movie.MovieStatus,
                      Runtime: movie.Runtime,
                      VoteCount: movie.VoteCount,
                      MovieId: -1
                      ));

                return new AddMoviePayload(new MovieGraphQl(
                        DbId: result.id,
                        Title: result.Title,
                        Homepage: result.Homepage,
                        YearReleased: result.yearReleased,
                        Budget: result.Budget,
                        Tagline: result.Tagline,
                        Overview: result.Overview,
                        Revenue: result.Revenue,
                        MovieStatus: result.MovieStatus,
                        Runtime: result.Runtime,
                        VoteCount: result.VoteCount,
                        Genres: new string[0],
                        Keywords: new string[0]));
            }
            catch (Exception ex)
            {
                List<UserError> errors = new List<UserError>();
                errors.Add(new UserError(ex.Message, ex.Source ?? ""));
                if (ex.InnerException is not null)
                {
                    errors.Add(new UserError(ex.InnerException.Message, ex.InnerException.Source ?? ""));
                }
                return new AddMoviePayload(errors);
            }
        }

        public async Task<UpdateGenrePayload> UpdateMovieGenres(UpdateGenreInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            try
            {
                return new UpdateGenrePayload((await repo.Movies.UpdateMovieGenres(Guid.Parse(input.MovieId), input.Genres)).Genres);
            }
            catch(Exception ex)
            {
                return new UpdateGenrePayload(new UserError[] { new UserError(ex.Message, ex.Source ?? string.Empty) });
            }
        }

        public async Task<UpdateKeywordPayload> UpdateMovieKeywords(UpdateKeywordInput input, [Service] IMovieUserDb repo, CancellationToken ct)
        {
            try
            {
                return new UpdateKeywordPayload((await repo.Movies.UpdateMovieKeywords(Guid.Parse(input.MovieId), input.Keywords)).Keywords);
            }
            catch (Exception ex)
            {
                return new UpdateKeywordPayload(new UserError[] { new UserError(ex.Message, ex.Source ?? string.Empty) });
            }
        }

    }
}
