using cosmosDb.movies;
using cosmosDb.movies.Models.User;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlite.movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movies.Tests
{
    [TestClass]
    public class UserTests
    {
        private CosmosClient _client = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        private string dbName = "movies";
        [TestMethod("0. Reset Users")]
        public async Task ResetUserContainer()
        {
            Raw_Repo rp = new();
            await rp.UsersReset();

            Assert.IsTrue(true);
        }

        [TestMethod("1. Add Users")]
        public async Task CreateUsers()
        {
            // New Class
            UserRepository userRepo = new (_client,dbName );
            await userRepo.AddUser(Maurice);
            await userRepo.AddUser(Laura);
            await userRepo.AddUser(Paul);
            await userRepo.AddUser(Karen);
            await userRepo.AddUser(Neil);

            Assert.IsTrue(true);
        }

        [TestMethod("3. Get All User")]
        public async Task GetAllUsers()
        {
            UserRepository userRepo = new(_client, dbName);

            var results = await userRepo.GetUsers();

            Assert.IsTrue(results.ToList().Count == 5);
        }

        [TestMethod("4. Add Reviews (Also Get users)")]
        public async Task CreateUserReviews()
        {
            UserRepository userRepo = new(_client, dbName);

            var Maurice = await userRepo.GetUserByUserName(this.Maurice.UserName);
            var Laura = await userRepo.GetUserByUserName(this.Laura.UserName);
            var Paul = await userRepo.GetUserByUserName(this.Paul.UserName);
            var Karen = await userRepo.GetUserByUserName(this.Karen.UserName);
            var Neil = await userRepo.GetUserByUserName(this.Neil.UserName);
            Assert.IsNotNull(Maurice);
            Assert.IsNotNull(Laura);
            Assert.IsNotNull(Paul);
            Assert.IsNotNull(Karen);
            Assert.IsNotNull(Neil);

            MoviesRepository movieRepo = new(_client, dbName);
            var Movies = new List<MovieDb>
            {
                (await movieRepo.GetMovieByName("Killer Elite")).First(),
            (await movieRepo.GetMovieByName("Something Borrowed")).First(),
            (await movieRepo.GetMovieByName("Batman")).First(),
            (await movieRepo.GetMovieByName("21 Jump Street")).First(),
            (await movieRepo.GetMovieByName("22 Jump Street")).First(),
            (await movieRepo.GetMovieByName("28 Days")).First(),
            (await movieRepo.GetMovieByName("Trippin'")).First(),
            (await movieRepo.GetMovieByName("Twilight")).First(),
            (await movieRepo.GetMovieByName("U2 3D")).First(),
            (await movieRepo.GetMovieByName("UHF")).First(),
            };

            UserReviewsDb MauriceRevews = new(new[]{ new ReviewDb (
                Movies[0].id,
                Movies[0].Title,
                Movies[0].yearReleased.ToString(),
                Maurice.UserId,
                Maurice.UserName,
                @"The drug seekers would come into the emergency room and scream about how much pain they were in. When you told them that you would start elevating their pain with Tylenol or Advil instead of a narcotic they became nasty and combative. They would start telling you what drug and dose they had to have to make their pain tolerable. After dealing with the same drug seekers several times a month it gets old. Some of the doctors would give in and give them a dose of morphine and send them away. Sure that was faster, but ethically she still couldn’t do it. Perhaps that’s why she had longer care times than the other doctors.",
                4.5D
            ),
            new ReviewDb (
                Movies[1].id,
                Movies[1].Title,
                Movies[1].yearReleased.ToString(),
                Maurice.UserId,
                Maurice.UserName,
                @"She didn't understand how changed worked. When she looked at today compared to yesterday, there was nothing that she could see that was different. Yet, when she looked at today compared to last year, she couldn't see how anything was ever the same.",
                3.8D
            )}, Maurice.UserId);

            UserReviewsDb PaulReviews = new(new[]{ new ReviewDb (
                Movies[2].id,
                Movies[2].Title,
                Movies[2].yearReleased.ToString(),
                Paul.UserId,
                Paul.UserName,
               "It had been a rough day. Things hadn't gone as planned and that meant Hannah got yelled at by her boss. It didn't even matter that it wasn't her fault. When things went wrong at work, Hannah got the blame no matter the actual circumstances. It wasn't fair, but there was little she could do without risking her job, and she wasn't in a position to do that with the plans she had.",
                5.5D
            ),
            new ReviewDb (
                Movies[3].id,
                Movies[3].Title,
                Movies[3].yearReleased.ToString(),
                Paul.UserId,
                Paul.UserName,
               "The day had begun on a bright note. The sun finally peeked through the rain for the first time in a week, and the birds were sinf=ging in its warmth. There was no way to anticipate what was about to happen. It was a worst-case scenario and there was no way out of it",
                7.2D
            )}, Paul.UserId);

            UserReviewsDb NeilReviews = new(new[]{ new ReviewDb (
                Movies[4].id,
                Movies[4].Title,
                Movies[4].yearReleased.ToString(),
                Neil.UserId,
                Neil.UserName,
               "Hopes and dreams were dashed that day. It should have been expected, but it still came as a shock. The warning signs had been ignored in favor of the possibility, however remote, that it could actually happen. That possibility had grown from hope to an undeniable belief it must be destiny. That was until it wasn't and the hopes and dreams came crashing down.",
                3.2D
            ),
            new ReviewDb (
                Movies[5].id,
                Movies[5].Title,
                Movies[3].yearReleased.ToString(),
                Neil.UserId,
                Neil.UserName,
                "The clowns had taken over. And yes, they were literally clowns. Over 100 had appeared out of a small VW bug that had been driven up to the bank. Now they were all inside and had taken it over.",
                2.3D
            )}, Neil.UserId);


            _ = await userRepo.AddReview(Maurice.UserId, MauriceRevews.reviews[0]);
            _ = await userRepo.AddReview(Maurice.UserId, MauriceRevews.reviews[1]);
            _ = await userRepo.AddReview(Paul.UserId, PaulReviews.reviews[0]);
            _ = await userRepo.AddReview(Paul.UserId, PaulReviews.reviews[1]);
            _ = await userRepo.AddReview(Neil.UserId, NeilReviews.reviews[0]);
            _ = await userRepo.AddReview(Neil.UserId, NeilReviews.reviews[1]);

        }

        UserDetailsDb Maurice = new UserDetailsDb
          (
              UserId: Guid.NewGuid(),
              UserName: "Maurice_Ferkin",
              Firstname: "Maurice",
              Surname: "Firkin",
              EmailAddress: "Maurice_Ferkin@Email.com",
              Birthdate: DateTime.Parse("14/05/69"),
              FavouriteGenres: new string[] { },
              AvoidGenres: new string[] { }
          );

        UserDetailsDb Karen = new UserDetailsDb
         (
             UserId: Guid.NewGuid(),
             UserName: "Karen.Spanner@email.com",
             Firstname: "Karen",
             Surname: "Spanner",
             EmailAddress: "Karen.Spanner@email.com",
             Birthdate: DateTime.Parse("08/08/88"),
             FavouriteGenres: new string[] { },
             AvoidGenres: new string[] { }
         );


        UserDetailsDb Paul = new UserDetailsDb
        (
            UserId: Guid.NewGuid(),
            UserName: "Paul.Larence@email.com",
            Firstname: "Paul",
            Surname: "Lawrence",
            EmailAddress: "Paul.Larence@email.com",
            Birthdate: DateTime.Parse("30/09/1978"),
            FavouriteGenres: new string[] { "Mystery" },
            AvoidGenres: new string[] { "TV Movie", "Foreign" }
        );

        UserDetailsDb Laura = new UserDetailsDb
         (
             UserId: Guid.NewGuid(),
             UserName: "LozzlePlop",
             Firstname: "Laura",
             Surname: "Lawrence",
             EmailAddress: "LauraEllenLawrence@email.com",
             Birthdate: DateTime.Parse("03/08/83"),
             FavouriteGenres: new string[] { },
             AvoidGenres: new string[] { "Horror", "Crime" }
         );

        UserDetailsDb Neil = new UserDetailsDb
       (
           UserId: Guid.NewGuid(),
           UserName: "Squeals",
           Firstname: "Neil",
           Surname: "Lawrence",
           EmailAddress: "Nail.Lwrence@email.com",
           Birthdate: DateTime.Parse("18/11/80"),
           FavouriteGenres: new string[] { "Comedy", "Action" },
           AvoidGenres: new string[] { "War" }
       );
    }
}
