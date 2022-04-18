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

        public UserDetailsDb AddUser(UserDetailsDb newUser)
        {
            throw new Exception();
        }

        public IEnumerable<UserDetailsDb> GetUsers()
        {
            throw new Exception();
        }

        public UserDetailsDb UpdateUser(UserDetailsDb user)
        {
            throw new NotFiniteNumberException();
        }

        public IEnumerable<UserReviewsDb> GetReviews(Guid userID)
        {
            throw new Exception();
        }

        public Task AddReview(Guid UserId, ReviewDb userReview)
        {
            throw new Exception();
        }
    }
}
