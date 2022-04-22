using cosmosDb.movies.Repos;
using GraphQl.WebApi.GraphQl.Movies;
using static CosmosDb.Movies.AspNetDi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMovieUserDb>(InitializeMovieUserDb(builder.Configuration.GetSection("MovieUserApp")));
builder.Services.AddGraphQLServer()
                .AddQueryFieldToMutationPayloads()
                //.AddGlobalObjectIdentification()
                .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<MovieQueries>();
                //.AddType<BasicMovieInfoType>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();

app.Run();
