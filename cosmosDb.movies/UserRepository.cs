using cosmosDb.movies.Models.User;
using Microsoft.Azure.Cosmos;

namespace cosmosDb.movies
{
    public class UserRepository
    {
        private CosmosClient cm;
        private Database db;

        public UserRepository()
        {
            this.cm = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            this.db = cm.GetDatabase("movies");

        }

        private async Task<Container> ConfigureContainer()
        {
            var containerResponse = await db.CreateContainerIfNotExistsAsync(new ContainerProperties { Id = "users", PartitionKeyPath = "/entityType" });
            return containerResponse.Container;
        }

        public async Task<UserDetailsDb> AddUser(UserDetailsDb newUser)
        {
            try
            {
                var container = await this.ConfigureContainer();
                var itemResponse = await container.CreateItemAsync<UserDetailsDb>(newUser, new PartitionKey(newUser.entityType));
                if (itemResponse.StatusCode == System.Net.HttpStatusCode.Created)
                    return itemResponse.Resource;
                else
                    throw new Exception("User not created");
            }
            catch (CosmosException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserDetailsDb>> GetUsers()
        {
            var container = await this.ConfigureContainer();
            var qd = new QueryDefinition("Select * from  u");
            return await UserRepository.GetRecords<UserDetailsDb>(qd, container, "Details");
            //throw new Exception();
        }

        public async Task<UserDetailsDb> GetUserByUserName(string userName)
        {
            var container = await this.ConfigureContainer();
            var qd = new QueryDefinition("Select * from users u where u.UserName = @username").WithParameter("@username", userName);
            var allRecordsButPleaseBeOnly1 = await UserRepository.GetRecords<UserDetailsDb>(qd, container, "Details");
            return allRecordsButPleaseBeOnly1.First();
        }

        private static async Task<List<T>> GetRecords<T>(QueryDefinition qd, Container container, string partitionKeyValue)
        {
            List<T> results = new List<T>();
            using FeedIterator<T> feedIterator = container.GetItemQueryIterator<T>(qd, null, new QueryRequestOptions { PartitionKey = new PartitionKey(partitionKeyValue) });
            while (feedIterator.HasMoreResults)
            {
                results.AddRange(await feedIterator.ReadNextAsync());
            }

            return results;
        }

        public async Task<UserDetailsDb> UpdateUser(UserDetailsDb user)
        {
            try
            {
                var container = await this.ConfigureContainer();
                var itemResponse = await container.UpsertItemAsync<UserDetailsDb>(user, new PartitionKey(user.entityType));
                if (itemResponse.StatusCode == System.Net.HttpStatusCode.Created)
                    return itemResponse.Resource;
                else
                    throw new Exception("User not created");
            }
            catch (CosmosException)
            {
                throw;
            }

        }

        public async Task<UserReviewsDb> GetReviews(Guid userID)
        {
            try
            {
                var container = await this.ConfigureContainer();
                try
                {
                    return await container.ReadItemAsync<UserReviewsDb>(userID.ToString(), new PartitionKey("Reviews"));
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
            }
            catch (CosmosException)
            {
                throw;
            }


        }

        public async Task<UserReviewsDb> AddReview(Guid UserId, ReviewDb userReview)
        {
            try
            {
                // get reviews
                var container = await this.ConfigureContainer();
                var allUserReviews = await this.GetReviews(UserId);

                if (allUserReviews is null)
                {
                    allUserReviews = new UserReviewsDb(new[] { userReview }, UserId);
                }
                else
                {
                    var allReviews = allUserReviews.reviews.Append(userReview);
                    
                    allUserReviews = allUserReviews with
                    {
                        reviews = allReviews.ToArray()
                    };
                }
                var itemResponse = await container.UpsertItemAsync(allUserReviews, new PartitionKey(allUserReviews.entityType));
                return itemResponse.Resource;
            }
            catch (CosmosException)
            {
                throw;
            }
        }
    }
}
