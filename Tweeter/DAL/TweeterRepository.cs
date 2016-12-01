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

        internal void UnfollowUser(string userName, string user_to_unfollow)
        {
            throw new NotImplementedException();
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
        public Twit GetTwitUser(string name)
        {
            Twit found_user = Context.TweeterUsers.FirstOrDefault(u => u.TwitName.ToLower() == name.ToLower());
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
        public List<Follow> GetListOfTwitsUserFollows(int UserId)
        {
            //Twit user = Context.TweeterUsers.First(twit => twit.TwitId == UserId);
            List<Follow> FollowerList = Context.AllFollows.Where(u => u.TwitFollower.TwitId == UserId).ToList();
            if (FollowerList.Count() == 0)
            {
                return FollowerList ?? Enumerable.Empty<Follow>().ToList();
            }
            else
            {
                return FollowerList;
            }
        }

        public bool FollowUser(int UserId, int UserIdToFollow)
        {
            //Twit IdiotQuery = Context.TweeterUsers.SingleOrDefault(idiot => idiot.TwitId == UserIdToFollow);
            //Twit userQuery = Context.TweeterUsers.SingleOrDefault(user => user.TwitId == UserId);
            Follow userQuery = Context.AllFollows.SingleOrDefault(b => b.TwitFollower.TwitId == UserId);
            Follow IdiotQuery = Context.AllFollows.SingleOrDefault(b => b.TwitFollowed.TwitId == UserIdToFollow);
            if (IdiotQuery == null || userQuery == null)
            {
                return false;
            }
            if (userQuery != IdiotQuery) {
                try
                {
                    Context.AllFollows.Add(IdiotQuery);
                    Context.SaveChanges();
                    return true;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            return true;
        }
        public bool FollowUser(string currentUser, string userToFollow)
        {
            GetTwitUser(currentUser);
            Twit found_current = GetTwitUser(currentUser);
            Twit found_user_to_follow = GetTwitUser(userToFollow);
            Follow followed_twit = new Follow { TwitFollower = found_current, TwitFollowed = found_user_to_follow };
            if (found_current == null || found_user_to_follow == null)
            {
                return false;
            }
            if (currentUser == userToFollow)
            {
                return false;
            }
            else
            {
            Context.AllFollows.Add(followed_twit);
            //found_current.Add(found_user_to_follow);
            Context.SaveChanges();
            return true;

            }
        }
    }
}