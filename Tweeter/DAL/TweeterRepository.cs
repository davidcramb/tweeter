using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Tweeter.Models;

namespace Tweeter.DAL
{
    public class TweeterRepository
    {
        public TweeterContext Context { get; set; }

        public TweeterRepository(TweeterContext _context)
        {
            Context = _context;
        }

        public TweeterRepository()
        {
            Context = new TweeterContext();
        }

        public List<Twit> GetUsers()
        {
            return Context.TweeterUsers.ToList();
        }

        public Twit AddUser(Twit user)
        {
            Context.TweeterUsers.Add(user);
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var error = e;
            }
            return user;
        }
        public List<string> GetUsernames()
        {
            return Context.TweeterUsers.Select(u => u.BaseUser.UserName).ToList();
        }


        public bool UsernameExists(string v)
            {
            if (Context.TweeterUsers.Any(u => u.TwitName.ToLower().Contains(v)))
            {
                return true;
            }
            return false;
        }

        public Twit GetUserByName(string name)
        {
            Twit query = Context.TweeterUsers.FirstOrDefault(n => n.TwitName.ToLower() == name);
            return query;
        }
        public List<Tweet> GetTweets()
        {
            return Context.Tweets.ToList();
        }
        public List<Tweet> GetTweets(int id)
        {
            List<Tweet> TweetList = Context.Tweets.ToList();
            List<Tweet> TweetListById = new List<Tweet>();
            //var match = TweetList.TrueForAll(Twit => Twit.TweetId == id);
            var query = TweetList.Where(tweet => tweet.TweetId == id);
            foreach (var tweet in query)
            {
                TweetListById.Add(tweet);
            }
            return TweetListById;
        }
        public Twit GetTwitUser(int id)
        {
            //ApplicationUser found_application_user = Context.Users.FirstOrDefault(u => u.Id == id.ToString());
            Twit found_user = Context.TweeterUsers.FirstOrDefault(u => u.TwitId == id);
            return found_user;
        }
        public void AddTweet(Tweet new_tweet)
        {
            Context.Tweets.Add(new_tweet);
            Context.SaveChanges();
        }

        public void RemoveTweet(Tweet tweet_to_delete)
        {
            Context.Tweets.Remove(tweet_to_delete);
            Context.SaveChanges();
        }
        public void RemoveTweet(int id)
        {
            var TweetById = Context.Tweets.SingleOrDefault(tweet => tweet.TweetId == id);
            Context.Tweets.Remove(TweetById);
            Context.SaveChanges();
        }
        public List<Twit> GetListOfTwitsUserFollows(int UserId)
        {
            //Twit user = Context.TweeterUsers.First(twit => twit.TwitId == UserId);
            List<Twit> FollowerList = GetTwitUser(UserId).Follows;
            Console.WriteLine(GetTwitUser(UserId));
            Console.WriteLine(GetTwitUser(UserId).TwitName);
            Console.WriteLine(GetTwitUser(UserId).Follows);


            if (FollowerList.Count() == 0)
            {
                return FollowerList ?? Enumerable.Empty<Twit>().ToList();

            }
            else
            {
                return FollowerList;
            }
        }

        public void FollowUser(int UserId, int UserIdOfSomeIdiot)
        {
            Twit IdiotQuery = Context.TweeterUsers.SingleOrDefault(idiot => idiot.TwitId == UserIdOfSomeIdiot);
            Twit userQuery = Context.TweeterUsers.SingleOrDefault(user => user.TwitId == UserId);
            try
            {
                Context.TweeterUsers.SingleOrDefault(twit => twit.TwitId == UserId).Follows.Add(IdiotQuery);
                Context.SaveChanges();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}