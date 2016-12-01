using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweeter.DAL;
using Moq;
using System.Data.Entity;
using Tweeter.Models;
using System.Collections.Generic;
using System.Linq;


namespace Tweeter.Tests.DAL
{
    [TestClass]
    public class TweetRepoTests
    {

        private Mock<TweeterContext> mock_context { get; set; }
        private Mock<DbSet<Tweet>> mock_tweets { get; set; }
        private Mock<DbSet<Twit>> mock_users { get; set; }
        private Mock<DbSet<ApplicationUser>> mock_application_users { get; set; }
        private Mock<DbSet<Follow>> mock_follows { get; set; }
        private List<Tweet> tweets { get; set; }
        private List<Twit> users { get; set; }
        private List<ApplicationUser> app_users { get; set; }
        private List<Follow> follows { get; set; }

        private TweeterRepository repo { get; set; }
        private Twit test_bot;
        private Twit Bob;
        private Twit Joe;
        private Tweet new_tweet;
        private Tweet last_tweet;
        private Follow followed;
        private Follow followed2;

        public void ConnectToDatastore()
        {
            var query_tweets = tweets.AsQueryable();
            var query_users = users.AsQueryable();
            var query_application_users = app_users.AsQueryable();
            var query_follows = follows.AsQueryable();

            mock_tweets.As<IQueryable<Tweet>>().Setup(m => m.Provider).Returns(query_tweets.Provider);
            mock_tweets.As<IQueryable<Tweet>>().Setup(m => m.Expression).Returns(query_tweets.Expression);
            mock_tweets.As<IQueryable<Tweet>>().Setup(m => m.ElementType).Returns(query_tweets.ElementType);
            mock_tweets.As<IQueryable<Tweet>>().Setup(m => m.GetEnumerator()).Returns(() => query_tweets.GetEnumerator());
            mock_context.Setup(m => m.Tweets).Returns(mock_tweets.Object);
            mock_tweets.Setup(t => t.Add(It.IsAny<Tweet>())).Callback((Tweet t) => tweets.Add(t));
            mock_tweets.Setup(t => t.Remove(It.IsAny<Tweet>())).Callback((Tweet t) => tweets.Remove(t));

            mock_users.As<IQueryable<Twit>>().Setup(m => m.Provider).Returns(query_users.Provider);
            mock_users.As<IQueryable<Twit>>().Setup(m => m.Expression).Returns(query_users.Expression);
            mock_users.As<IQueryable<Twit>>().Setup(m => m.ElementType).Returns(query_users.ElementType);
            mock_users.As<IQueryable<Twit>>().Setup(m => m.GetEnumerator()).Returns(() => query_users.GetEnumerator());

            mock_context.Setup(c => c.TweeterUsers).Returns(mock_users.Object);
            mock_users.Setup(u => u.Add(It.IsAny<Twit>())).Callback((Twit t) => users.Add(t));
            mock_users.Setup(u => u.Remove(It.IsAny<Twit>())).Callback((Twit t) => users.Remove(t));

            mock_application_users.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(query_application_users.Provider);
            mock_application_users.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(query_application_users.Expression);
            mock_application_users.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(query_application_users.ElementType);
            mock_application_users.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(() => query_application_users.GetEnumerator());
        
            mock_context.Setup(c => c.User_Database).Returns(mock_application_users.Object);
            mock_application_users.Setup(u => u.Add(It.IsAny<ApplicationUser>())).Callback((ApplicationUser a) => app_users.Add(a));
            mock_application_users.Setup(u => u.Remove(It.IsAny<ApplicationUser>())).Callback((ApplicationUser a) => app_users.Remove(a));

            
            mock_follows.As<IQueryable<Follow>>().Setup(m => m.Provider).Returns(query_follows.Provider);
            mock_follows.As<IQueryable<Follow>>().Setup(m => m.Expression).Returns(query_follows.Expression);
            mock_follows.As<IQueryable<Follow>>().Setup(m => m.ElementType).Returns(query_follows.ElementType);
            mock_follows.As<IQueryable<Follow>>().Setup(m => m.GetEnumerator()).Returns(() => query_follows.GetEnumerator());

            mock_context.Setup(c => c.AllFollows).Returns(mock_follows.Object);
            mock_follows.Setup(u => u.Add(It.IsAny<Follow>())).Callback((Follow a) => follows.Add(a));
            mock_follows.Setup(u => u.Remove(It.IsAny<Follow>())).Callback((Follow a) => follows.Remove(a));
            /*
             * Below mocks the 'Users' getter that returns a list of ApplicationUsers
             * mock_user_manager_context.Setup(c => c.Users).Returns(mock_users.Object);
             * 
             */

            /* IF we just add a Username field to the Twit model
             * mock_context.Setup(c => c.TweeterUsers).Returns(mock_users.Object); Assuming mock_users is List<Twit>
             */
        }

        [TestInitialize]
        public void Initialize()
        {
            mock_context = new Mock<TweeterContext>();
            mock_tweets = new Mock<DbSet<Tweet>>();
            mock_users = new Mock<DbSet<Twit>>();
            mock_application_users = new Mock<DbSet<ApplicationUser>>();
            mock_follows = new Mock<DbSet<Follow>>();
            repo = new TweeterRepository(mock_context.Object);
            tweets = new List<Tweet>();
            users = new List<Twit>();
            app_users = new List<ApplicationUser>();
            follows = new List<Follow>();
            test_bot = new Twit { TwitName = "TestBot", TwitId = 1, BaseUser = new ApplicationUser() { UserName = "Testbot", Id = "1" } };
            Bob = new Twit { TwitName = "Bob", TwitId = 2, BaseUser = new ApplicationUser() { UserName = "Bob", Id = "2" }/*, Follows =  new List<Twit> { test_bot } */};
            Joe = new Twit { TwitName = "Joe", TwitId = 3, BaseUser = new ApplicationUser() { UserName = "Joe", Id = "3" }/*, Follows =  new List<Twit> { test_bot } */};
            followed = new Follow { TwitFollower = Bob, TwitFollowed = test_bot };
            followed2 = new Follow { TwitFollower = Joe, TwitFollowed = test_bot };
            new_tweet = new Tweet { TweetId = 1, Message = "Hi, I'm Bob!" };
            last_tweet = new Tweet { TweetId = 2, Message = "Go to hell, Bob." };
            tweets.Add(new_tweet); tweets.Add(last_tweet);
            users.Add(Bob); users.Add(Joe);
            ConnectToDatastore();

            /* 
             1. Install Identity into Tweeter.Tests (using statement needed)
             2. Create a mock context that uses 'UserManager' instead of 'TweeterContext'
             */
        }
        [TestCleanup]
        public void TearDown()
        {
            repo = null;
        }
        [TestMethod]
        public void EnsureContext()
        {
            Assert.IsNotNull(repo);
        }
        [TestMethod]
        public void EnsureCanGetTweets()
        {
            var tweets = repo.GetTweets();
            Assert.IsInstanceOfType(tweets, typeof(List<Tweet>));
        }
        [TestMethod]
        public void EnsureCanGetTweetByTweetId()
        {
            var tweet = repo.GetTweets(1);
            Console.WriteLine(tweet[0]);
            Assert.IsTrue(tweet[0].TweetId == 1);
        }

        [TestMethod]
        public void EnsureCanAddTweet()
        {
            Tweet third_tweet = new Tweet { TweetId = 3, Message = "Rude! I prefer civil discourse in 140 characters or less." };
            repo.AddTweet(third_tweet);
            int expectedtweets = 3;
            int actualtweets = repo.GetTweets().Count();
            Assert.AreEqual(expectedtweets, actualtweets);
        }
        [TestMethod]
        public void EnsureCanRemoveTweet()
        {
            repo.RemoveTweet(last_tweet);
            int expectedtweets = 1;
            int actualtweets = repo.GetTweets().Count();
            Assert.AreEqual(expectedtweets, actualtweets);
        }
        [TestMethod]
        public void EnsureCanRemoveTweetByTweetId()
        {
            Assert.IsTrue(repo.GetTweets().Count() == 2);
            repo.RemoveTweet(2);
            Assert.IsTrue(repo.GetTweets().Count() == 1);
        }
        [TestMethod]
        public void RepoGetTwitUserFromUserID()
        {
            int user_id = 2;
            Twit FoundUser = repo.GetTwitUser(user_id);
            string expectedUser = "Bob";
            string actualUser = FoundUser.TwitName.ToString();
            Assert.IsTrue(expectedUser == actualUser);
        }
        [TestMethod]
        public void EnsureFollowersExist()
        {
            Assert.IsNotNull(repo.Context.TweeterUsers);
        }
        [TestMethod]
        public void EnsureCanListTwitsUserIsFollowingByUserId()
        {
            int actuallFollowers = repo.GetListOfTwitsUserFollows(2).Count();
            int expectedFollowers = 1;
            Assert.AreEqual(expectedFollowers, actuallFollowers);
        }

        [TestMethod]
        public void EnsureCanAddUserToFollowingByUserId()
        {
            repo.FollowUser(2, 3);
            Assert.IsTrue(repo.GetListOfTwitsUserFollows(2).Count() == 2);
        }
        [TestMethod]
        public void EnsureCanFollowWithString()
        {
            string current_user = "Joe";
            string user_to_follow = "Bob";
            int expected_follows_count = 2;
            bool follow_successful = repo.FollowUser(current_user, user_to_follow);
            int actual_follow_count = repo.GetListOfTwitsUserFollows(3).Count();
            Assert.IsTrue(follow_successful);
            Assert.AreEqual(expected_follows_count, actual_follow_count);
        }
        [TestMethod]
        public void EnsureUserCannotFollowNonExistentTwit()
        {

        }
        [TestMethod]
        public void EnsureUserCannotFollowSelf()
        {
            int BobFollowers = repo.GetListOfTwitsUserFollows(2).Count();
            Assert.IsTrue(BobFollowers == 1); // In test, Bob only follows twit bot
            Assert.IsTrue(Bob.TwitId == 2); // We'll pass in Bob's userId into the add follower method
            repo.FollowUser(2, 2);
            Assert.AreEqual(BobFollowers, repo.GetListOfTwitsUserFollows(2).Count()); //Bobfollowers should still = 1

        }
    }
}