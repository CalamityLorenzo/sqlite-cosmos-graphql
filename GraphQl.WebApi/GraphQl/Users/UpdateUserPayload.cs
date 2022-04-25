using cosmosDb.movies.Models.User;
using GraphQl.WebApi.GraphQl.Common;

namespace GraphQl.WebApi.GraphQl.Users
{
    public class UpdateUserPayload : BasePayload
    {
        public UserGraphQl? User;

        public UpdateUserPayload(UserError[] userErrors) : base(userErrors) { }

        public UpdateUserPayload(UserGraphQl userDetailsDb) : base(new UserError[] { })
        {
            this.User = userDetailsDb;
        }
    }
}