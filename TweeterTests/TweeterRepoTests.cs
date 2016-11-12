using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tweeter.DAL;
using Tweeter.Models;
using Moq;
using System.Data.Entity;
using System.Linq;


namespace Tweeter.Tests
{
    [TestClass]
    public class TweeterTests
    {
        private Mock<TweeterContext> mock_context { get; set; }
        private Mock<DbSet<Twit>> mock_user_table { get; set; }
        private List<Twit> user_list { get; set; }
        private List<ApplicationUser> application_user_list { get; set; }
        private TweeterRepository repo { get; set; }
        private Mock<DbSet<ApplicationUser>> mock_applicationuser_table { get; set; }

        public void ConnectMockToDataStore()
        {
            var queryable_list = user_list.AsQueryable();
            //var userStore = new Mock<IUserStore<ApplicationUser>>();
            mock_user_table.As<IQueryable<Twit>>().Setup(m => m.Provider).Returns(queryable_list.Provider);
            mock_user_table.As<IQueryable<Twit>>().Setup(m => m.Expression).Returns(queryable_list.Expression);
            mock_user_table.As<IQueryable<Twit>>().Setup(m => m.ElementType).Returns(queryable_list.ElementType);
            mock_user_table.As<IQueryable<Twit>>().Setup(m => m.GetEnumerator()).Returns(() => queryable_list.GetEnumerator());
            mock_context.Setup(u => u.TweeterUsers).Returns(mock_user_table.Object);

            mock_user_table.Setup(t => t.Add(It.IsAny<Twit>())).Callback((Twit u) => user_list.Add(u));
            mock_user_table.Setup(t => t.Remove(It.IsAny<Twit>())).Callback((Twit u) => user_list.Remove(u));

            var queryable_user_list = application_user_list.AsQueryable();
            mock_applicationuser_table.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(queryable_user_list.Provider);
            mock_applicationuser_table.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(queryable_user_list.Expression);
            mock_applicationuser_table.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(queryable_user_list.ElementType);
            mock_applicationuser_table.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(() => queryable_user_list.GetEnumerator());
           
            //mock_context.Setup(c => c.TweeterUsers).Returns(ApplicationUserManager);

            mock_applicationuser_table.Setup(t => t.Add(It.IsAny<ApplicationUser>())).Callback((ApplicationUser u) => application_user_list.Add(u));
            mock_applicationuser_table.Setup(t => t.Remove(It.IsAny<ApplicationUser>())).Callback((ApplicationUser u) => application_user_list.Remove(u));
        }
        [TestInitialize]
        public void Intialize()
        {
            mock_context = new Mock<TweeterContext>();
            mock_user_table = new Mock<DbSet<Twit>>();
            user_list = new List<Twit>();
            repo = new TweeterRepository(mock_context.Object);
            ConnectMockToDataStore();
        }
        [TestCleanup]
        public void TearDown()
        {
            repo = null;
        }
        [TestMethod]
        public void EnsureInstanceOfRepo()
        {
            Assert.IsNotNull(repo);
        }
        //[TestMethod]
        //public void EnsureRepoIsEmpty()
        //{
        //    int expectedCount = 0;
        //    int actualCount = repo.GetAllUsernames().Count();
        //    Assert.AreEqual(expectedCount, actualCount);
        //}
        //[TestMethod]
        //public void EnsureCanAddTwitUser()
        //{
        //    ApplicationUser bob = new ApplicationUser { Username = "Bob" };
        //    Twit new_Twit = new Twit { TwitId = 1, BaseUser = bob  };
        //    repo.CreateUsername(new_Twit);
        //    int expectedCount = 1;
        //    int actualCount = repo.GetAllUsernames().Count();
        //    Assert.AreEqual(expectedCount, actualCount);


        //}
        [TestMethod]
        public void EnsureCanSearchRepoByName()
        {

        }
    }
}
