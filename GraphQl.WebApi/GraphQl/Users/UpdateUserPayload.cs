using cosmosDb.movies.Models.User;
using GraphQl.WebApi.GraphQl.Common;

namespace GraphQl.WebApi.GraphQl.Users
{
    public class UpdateUserPayload : BasePayload
    {
        private UserDetailsDb? User;

        public UpdateUserPayload(UserError[] userErrors) : base(userErrors) { }

        public UpdateUserPayload(UserDetailsDb userDetailsDb) : base(new UserError[] { })
        {
            this.User = userDetailsDb;
        }
    }
}