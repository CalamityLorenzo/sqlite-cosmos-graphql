using GraphQl.WebApi.GraphQl.Users;

namespace GraphQl.WebApi.GraphQl.Types
{
    public class UserGraphQlType : ObjectType<UserGraphQl>
    {

        protected override void Configure(IObjectTypeDescriptor<UserGraphQl> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(f => f.GraphQlId)
                .ResolveNode((ctx, id) => ctx.DataLoader<UserByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
        }
    }
}
