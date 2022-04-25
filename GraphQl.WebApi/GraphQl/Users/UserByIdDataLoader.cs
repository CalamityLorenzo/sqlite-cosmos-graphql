using cosmosDb.movies.Models.User;
using cosmosDb.movies.Repos;

namespace GraphQl.WebApi.GraphQl.Users
{
    public class UserByIdDataLoader : BatchDataLoader<Guid, UserGraphQl>
    {
        private readonly IMovieUserDb repo;

        public UserByIdDataLoader(IBatchScheduler batchScheduler, IMovieUserDb repo) : base(batchScheduler)
        {
            this.repo = repo;
        }

        protected override async Task<IReadOnlyDictionary<Guid, UserGraphQl>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            IList<UserDetailsDb> users = await repo.Users.GetByIds(keys);

            return users.Select(user => new UserGraphQl(
                                            user.UserId,
                                            user.UserName,
                                            user.Firstname,
                                            user.Surname,
                                            user.EmailAddress,
                                            user.Birthdate,
                                            user.FavouriteGenres,
                                            user.AvoidGenres)).ToDictionary(a => a.UserId, b => b);

        }
    }
}
