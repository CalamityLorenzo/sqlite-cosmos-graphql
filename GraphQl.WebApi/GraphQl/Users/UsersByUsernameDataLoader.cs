using cosmosDb.movies.Models.User;
using cosmosDb.movies.Repos;

namespace GraphQl.WebApi.GraphQl.Users
{
    public class UsersByUsernameDataLoader : BatchDataLoader<string, UserGraphQl>
    {
        private readonly IMovieUserDb repo;

        public UsersByUsernameDataLoader(IBatchScheduler batchScheduler, IMovieUserDb repo) : base(batchScheduler)
        {
            this.repo = repo;
        }

        protected override async Task<IReadOnlyDictionary<string, UserGraphQl>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            IList<UserDetailsDb> users = await repo.Users.GetByUsernames(keys);

            return users.Select(user => new UserGraphQl(
                                            user.UserId,
                                            user.UserName,
                                            user.Firstname,
                                            user.Surname,
                                            user.EmailAddress,
                                            user.Birthdate,
                                            user.FavouriteGenres,
                                            user.AvoidGenres)).ToDictionary(a => a.Username, b => b);

        }
    }
}
