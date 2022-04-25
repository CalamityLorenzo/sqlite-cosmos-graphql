using cosmosDb.movies.Models.User;
using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Common;

namespace GraphQl.WebApi.GraphQl.Users
{
    [ExtendObjectType("Mutation")]
    public class UserMutations
    {
        public async Task<AddUserPayload> AddUser(AddUserInput input, [Service] IMovieUserDb app, CancellationToken ct)
        {
            try
            {
                var dbUser = new UserDetailsDb(input.Username, input.Firstname, input.Surname, input.EmailAddress, input.Birthday);

                var newDbUser = await app.Users.Add(dbUser);
                return new AddUserPayload(new UserGraphQl(
                    newDbUser.UserId,
                    newDbUser.UserName,
                    newDbUser.Firstname,
                    newDbUser.Surname,
                    newDbUser.EmailAddress,
                    newDbUser.Birthdate,
                    new string[0],
                    new string[0]
                    ));
            }
            catch (Exception ex)
            {
                return new AddUserPayload(new UserError[] { new UserError(ex.Message, ex.StackTrace ?? "") });
            }
        }

        public async Task<UpdateUserPayload> UpdateUser(UpdateUserInput input, [Service] IMovieUserDb app, CancellationToken ct)
        {
            try
            {
                var dbUser = new UserDetailsDb(input.User.UserId, input.User.Username, input.User.Firstname, input.User.Surname, input.User.EmailAddress, input.User.Birthdate, input.User.FavouriteGenres, input.User.AvoidGenres);

                return new UpdateUserPayload(await app.Users.Update(dbUser));
                
            }
            catch (Exception ex)
            {
                return new UpdateUserPayload(new UserError[] { new UserError(ex.Message, ex.StackTrace ?? "") });
            }

        }
    }
}
