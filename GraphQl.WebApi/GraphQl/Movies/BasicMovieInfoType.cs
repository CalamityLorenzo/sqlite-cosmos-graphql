namespace GraphQl.WebApi.GraphQl.Movies
{
    public class BasicMovieInfoType : ObjectType<BasicMovieInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<BasicMovieInfo> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(a => a.Id);
                

        }
    }
}
