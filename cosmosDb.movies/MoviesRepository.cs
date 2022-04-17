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

        private async Task<Container> ConfigureKeywordsContainer()
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "keywords", PartitionKeyPath = "/keywordType" });
            return containerResponse.Container;
        }

        public async Task<MovieDb> AddNewMovie(MovieDb movie)
        {
            var repsonse = await (await ConfigureMovieContainer()).CreateItemAsync(movie, new PartitionKey(movie.entityType));
            if (repsonse.StatusCode == System.Net.HttpStatusCode.Created)

                return movie;
            else throw new Exception("error");

        }

        public async Task<MovieGenreKeywordDb> AddMovieGenres(Guid id, MovieGenreKeywordDb genre)
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

        public async Task<MovieDb> GetMovieByOldId(long movieId)
        {
            QueryDefinition qd = new QueryDefinition("Select * from movies m where m.MovieId = @movieId").WithParameter("@movieId", movieId);
            var container = await ConfigureMovieContainer();

            return await GetMovie(qd, container);
        }

        public async Task<MovieDb> GetMovie(Guid Id)
        {
            Container container = await ConfigureMovieContainer();
            return await container.ReadItemAsync<MovieDb>(Id.ToString(), new PartitionKey("Movie"));
        }

        private async Task<MovieDb> GetMovie(QueryDefinition qd, Container container)
        {
            IReadOnlyList<FeedRange> feedRanges = await container.GetFeedRangesAsync();
            using (FeedIterator<MovieDb> feedIteraor = container.GetItemQueryIterator<MovieDb>(feedRanges[0], qd,
                null,
                new QueryRequestOptions
                {
                }))
            {
                while (feedIteraor.HasMoreResults)
                {
                    foreach (var item in await feedIteraor.ReadNextAsync())
                    {
                        return item;
                    }
                    throw new ArgumentOutOfRangeException();
                }
                throw new ArgumentOutOfRangeException();

            }
        }
    }
}
