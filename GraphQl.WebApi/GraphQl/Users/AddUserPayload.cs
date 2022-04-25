
using GraphQl.WebApi.GraphQl.Common;
using GraphQl.WebApi.GraphQl.Users;

public class AddUserPayload : BasePayload
{
    public AddUserPayload(IEnumerable<UserError> errors) : base(errors)
    {
    }

    public AddUserPayload(UserGraphQl user) : base(Enumerable.Empty<UserError>())
    {
        this.User = user;
    }

    public UserGraphQl User { get; }
}
