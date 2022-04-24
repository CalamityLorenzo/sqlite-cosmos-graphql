using cosmosDb.movies.Models.Movies;
using Microsoft.Azure.Cosmos;
using sqlite.movies.Models;

namespace cosmosDb.movies
{
    public class MoviesRepository
    {
        private CosmosClient cm;
        private Database db;

        public MoviesRepository(CosmosClient client, string databasename)
        {
            this.cm = client;
            this.db = cm.GetDatabase(databasename);
        }
        private async Task<Container> ConfigureMovieContainer()
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "movies", PartitionKeyPath = "/entityType" });
            return containerResponse.Container;
        }
        /// Find a list of movies 
        public async Task<IList<MovieDb>> SearchMoviesByTitleDescription(string searchTerm)
        {
            Container container = await ConfigureMovieContainer();

            var sql = $"Select * from c where c.Title LIKE @searchTerm or c.Overview LIKE @searchTerm";
            QueryDefinition qd = new QueryDefinition(sql).WithParameter("@searchTerm", $"%{searchTerm}%");
            return await GetMovieRecords<MovieDb>(qd, container, "Movie");
        }

        /// <summary>
        /// Create a new instance of a movie.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<MovieDb> AddMovie(MovieDb movie)
        {
            if (movie.id != Guid.Empty)
                throw new ArgumentException("Movie Already exists?");
            else
            {
                var response = await (await ConfigureMovieContainer()).CreateItemAsync(movie with { id = Guid.NewGuid() }, new PartitionKey(movie.entityType));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return response.Resource;
                else throw new Exception("error movie not created");
            }

        }

        public async Task DeleteMovie(Guid id)
        {
            var container = await ConfigureMovieContainer();

            async Task TryDelete<T>(Func<Task<ItemResponse<T>>> func)
            {
                try
                {
                    await func();
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ; // Frankly just carry on
                }
            }

            await TryDelete(() => container.DeleteItemAsync<MovieDb>(id.ToString(), new PartitionKey("Movie")));
            await TryDelete(() => container.DeleteItemAsync<MovieGenreDb>(id.ToString(), new PartitionKey("Genre")));
            await TryDelete(() => container.DeleteItemAsync<MovieKeywordDb>(id.ToString(), new PartitionKey("Keyword")));
            await TryDelete(() => container.DeleteItemAsync<MovieCastDb>(id.ToString(), new PartitionKey("Cast")));
        }

        public async Task<MovieDb> UpdateMovie(MovieDb movie)
        {
            var response = await (await ConfigureMovieContainer())
                    .UpsertItemAsync(movie, new PartitionKey(movie.entityType));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return response.Resource;
            else throw new Exception("error movie not update");

        }

        /// <summary>
        /// Adds anew list of generes to a particular movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task<MovieGenreDb> AddMovieGenres(Guid id, MovieGenreDb genre)
        {
            var response = await (await ConfigureMovieContainer())
                .CreateItemAsync(genre, new PartitionKey(genre.entityType));
            return genre;

        }

        public async Task<MovieGenreDb> UpdateMovieGenres(Guid id, string[] genres)
        {
            MovieGenreDb genre = new(id, genres);

            var response = await (await ConfigureMovieContainer())
                .ReplaceItemAsync(genre, genre.id.ToString(), new PartitionKey(genre.entityType));
            return genre;
        }

        /// <summary>
        /// Add a new list of keywords to a particular movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public async Task<MovieKeywordDb> AddMovieKeywords(Guid id, MovieKeywordDb keywords)
        {
            var response = await (await ConfigureMovieContainer())
                .CreateItemAsync(keywords, new PartitionKey(keywords.entityType));
            return keywords;
        }

        /// <summary>
        /// Add a new list of keywords to a particular movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public async Task<MovieKeywordDb> UpdateMovieKeywords(Guid id, string[] keywords)
        {
            MovieKeywordDb keyword = new(id, keywords);
            var response = await (await ConfigureMovieContainer())
                .ReplaceItemAsync(keyword, keyword.id.ToString(), new PartitionKey(keyword.entityType));
            return keyword;
        }

        public async Task<MoviePersonDb> AddPerson(MoviePersonDb person)
        {
            var container = await ConfigureMovieContainer();
            var itemResponse = await container.CreateItemAsync<MoviePersonDb>(person, new PartitionKey(person.entityType));
            return itemResponse.Resource;
        }
        public async Task<MovieCastDb> AddMovieCast(MovieCastDb currentMovieCast)
        {
            var container = await ConfigureMovieContainer();
            var itemResponse = await container.CreateItemAsync<MovieCastDb>(currentMovieCast, new PartitionKey(currentMovieCast.entityType));
            return itemResponse.Resource;
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

        public async Task<IEnumerable<MovieDb>> GetMovieByName(string name)
        {
            Container container = await ConfigureMovieContainer();
            var query = "Select * from c where c.Title = @title";
            QueryDefinition qd = new QueryDefinition(query).WithParameter("@title", name);
            return await GetMovieRecords<MovieDb>(qd, container, "Movie");
        }

        public async Task<MoviePersonDb> GetPersonByOldId(int personId)
        {
            var container = await ConfigureMovieContainer();
            var sql = $"Select * from c where c.PersonId = @personId";
            QueryDefinition qd = new QueryDefinition(sql).WithParameter("@personId", personId);
            return (await GetMovieRecords<MoviePersonDb>(qd, container, "MoviePerson")).First();
        }

        public async Task<string[]> GetMovieKeywords(Guid id)
        {
            Container container = await ConfigureMovieContainer();
            try
            {
                return (await container.ReadItemAsync<MovieKeywordDb>(id.ToString(), new PartitionKey("Keyword"))).Resource?.Keywords ?? new string[] { };
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new string[0];
            }
        }

        internal async Task<List<MovieKeywordDb>> GetMovieKeywords()
        {
            Container container = await ConfigureMovieContainer();

            var sql = "Select * from c";
            QueryDefinition qd = new QueryDefinition(sql);
            return (await GetMovieRecords<MovieKeywordDb>(qd, container, "Keyword"));
        }

        internal async Task<List<MovieGenreDb>> GetMovieGenres()
        {
            Container container = await ConfigureMovieContainer();

            var sql = "Select * from c";
            QueryDefinition qd = new QueryDefinition(sql);
            return (await GetMovieRecords<MovieGenreDb>(qd, container, "Genre"));
        }

        public async Task<string[]> GetMovieGenres(Guid id)
        {
            try
            {
                Container container = await ConfigureMovieContainer();
                var items = (await container.ReadItemAsync<MovieGenreDb>(id.ToString(),
                    new PartitionKey("Genre"))).Resource?.Genres ?? new string[] { };
                return items;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new string[0];
            }
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

    }
}
