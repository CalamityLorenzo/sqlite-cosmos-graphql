﻿using cosmosDb.movies.Models.Movies;
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

            return (await GetMovieRecords<MovieDb>(qd, container, "Movie")).FirstOrDefault();
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
            return await GetMovieRecords<MovieDb>(qd, container, "Movie");
        }

        public async Task<IEnumerable<MovieDb>> GetMovieByName(string name)
        {
            Container container = await ConfigureMovieContainer();
            var query = "Select * from c where c.Title = @title";
            QueryDefinition qd = new QueryDefinition(query).WithParameter("@title", name);
            return await GetMovieRecords<MovieDb>(qd, container, "Movie");
        }

        public async Task<MoviePersonDb> AddPerson(MoviePersonDb person)
        {
            var container = await ConfigureMovieContainer();
            var itemResponse = await container.CreateItemAsync<MoviePersonDb>(person, new PartitionKey(person.entityType));
            return itemResponse.Resource;
        }



        private static async Task<List<T>> GetMovieRecords<T>(QueryDefinition qd, Container container, string partitionKey)
        {

            List<T> result = new List<T>();
            IReadOnlyList<FeedRange> feedRanges = await container.GetFeedRangesAsync();
            using (FeedIterator<T> feedIteraor = container.GetItemQueryIterator<T>(feedRanges[0], qd,
                null,
                new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(partitionKey)
                }))
            {
                while (feedIteraor.HasMoreResults)
                {
                    result.AddRange(await feedIteraor.ReadNextAsync());
                }
                return result;

            }
        }
        public async Task<MovieCastDb> AddMovieCast(MovieCastDb currentMovieCast)
        {
            var container = await ConfigureMovieContainer();
            var itemResponse = await container.UpsertItemAsync<MovieCastDb>(currentMovieCast, new PartitionKey(currentMovieCast.entityType));
            return itemResponse.Resource;
        }

        public async Task<MoviePersonDb> GetPersonByOldId(int personId)
        {
            var container = await ConfigureMovieContainer();
            var sql = $"Select * from c where c.PersonId = @personId";
            QueryDefinition qd = new QueryDefinition(sql).WithParameter("@personId", personId);
            return (await GetMovieRecords<MoviePersonDb>(qd, container, "MoviePerson")).First();
        }
    }
}
