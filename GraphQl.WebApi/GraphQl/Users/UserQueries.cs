using cosmosDb.movies.Models.User;
using cosmosDb.movies.Repos;

namespace GraphQl.WebApi.GraphQl.Users
{
    [ExtendObjectType("Query")]
    public class UserQueries
    {
        public async Task<UserGraphQl> GetUserById(UserByIdInput input,

            [Service] IMovieUserDb repo,
            CancellationToken ct)
        {
            UserDetailsDb user = await repo.Users.Get(input.Id);
            return new UserGraphQl(user.UserId, user.UserName, user.Firstname, user.Surname, user.EmailAddress, user.Birthdate, user.FavouriteGenres, user.AvoidGenres);
        }

        public  Task<UserGraphQl> GetUserByUsername(UserByUsernameInput input, UsersByUsernameDataLoader dataloader, CancellationToken ct) => dataloader.LoadAsync(input.Username, ct);

        public async Task<IEnumerable<UserGraphQl>> GetUsers(
            [Service] IMovieUserDb repo
            ) => (await repo.Users.GetAll()).Select(user => new UserGraphQl(
                                            user.UserId,
                                            user.UserName,
                                            user.Firstname,
                                            user.Surname,
                                            user.EmailAddress,
                                            user.Birthdate,
                                            user.FavouriteGenres,
                                            user.AvoidGenres));

        

    }
}
