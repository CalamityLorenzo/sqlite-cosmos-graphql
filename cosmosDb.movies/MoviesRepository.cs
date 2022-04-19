using cosmosDb.movies.Models.Movies;
using Microsoft.Azure.Cosmos;
using sqlite.movies.Models;

namespace cosmosDb.movies
{
    public class MoviesRepository
    {
        private CosmosClient cm;
        private Database db;

        public MoviesRepository()
        {
            this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            //this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            this.db = cm.GetDatabase("movies");
        }
        private async Task<Container> ConfigureMovieContainer()
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/entityType" });
            return containerResponse.Container;
        }


        public async Task<MovieDb> AddNewMovie(MovieDb movie)
        {
            var repsonse = await (await ConfigureMovieContainer()).CreateItemAsync(movie, new PartitionKey(movie.entityType));
            if (repsonse.StatusCode == System.Net.HttpStatusCode.Created)

                return movie;
            else throw new Exception("error");

        }

        public async Task<MovieGenreDb> AddMovieGenres(Guid id, MovieGenreDb genre)
        {
            var response = await (await ConfigureMovieContainer())
                .UpsertItemAsync(genre, new PartitionKey(genre.entityType));
            return genre;

        }

        public async Task<MovieKeywordDb> AddMovieKeywords(Guid id, MovieKeywordDb keywords)
        {
            var response = await (await ConfigureMovieContainer())
                .UpsertItemAsync(keywords, new PartitionKey(keywords.entityType));
            return keywords;
        }

        public async Task<MovieDb?> GetMovieByOldId(long movieId)
        {
            QueryDefinition qd = new QueryDefinition("Select * from movies m where m.MovieId = @movieId").WithParameter("@movieId", movieId);
            var container = await ConfigureMovieContainer();

            return (await GetMovie(qd, container)).FirstOrDefault();
        }

        public async Task<MovieDb> GetMovie(Guid Id)
        {
            Container container = await ConfigureMovieContainer();
            return await container.ReadItemAsync<MovieDb>(Id.ToString(), new PartitionKey("Movie"));
        }

        public async Task<IList<MovieDb>> SearchMoviesByTitleDescription(string searchTerm)
        {
            Container container = await ConfigureMovieContainer();

            var sql = $"Select * from c where c.Title LIKE @searchTerm or c.Overview LIKE @searchTerm";
            QueryDefinition qd = new QueryDefinition(sql).WithParameter("@searchTerm", $"%{searchTerm}%");
            return await GetMovie(qd, container);
        }

        public async Task<IEnumerable<MovieDb>> GetMovieByName(string name)
        {
            Container container = await ConfigureMovieContainer();
            var query = "Select * from c where c.Title = @title";
            QueryDefinition qd = new QueryDefinition(query).WithParameter("@title", name);
            return await GetMovie(qd, container);
        }

        public async Task<MoviePersonDb> AddPerson(MoviePersonDb person)
        {
            var container = await ConfigureMovieContainer();
            var itemResponse = await container.CreateItemAsync<MoviePersonDb>(person, new PartitionKey(person.entityType));
            return itemResponse.Resource;
        }



        private static async Task<List<MovieDb>> GetMovie(QueryDefinition qd, Container container)
        {

            List<MovieDb> result = new List<MovieDb>();
            IReadOnlyList<FeedRange> feedRanges = await container.GetFeedRangesAsync();
            using (FeedIterator<MovieDb> feedIteraor = container.GetItemQueryIterator<MovieDb>(feedRanges[0], qd,
                null,
                new QueryRequestOptions
                {
                }))
            {
                while (feedIteraor.HasMoreResults)
                {
                    result.AddRange(await feedIteraor.ReadNextAsync());
                }
                return result;

            }
        }
    }
}
