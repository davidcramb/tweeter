using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tweeter.DAL;
using Tweeter.Models;

namespace Tweeter.Controllers
{
    public class TweetController : ApiController
    {
        TweeterRepository Repo = new TweeterRepository();

        // GET api/<controller>
        public IEnumerable<Tweet> Get()
        {
            return Repo.GetTweets();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public Dictionary<string,bool> Post([FromBody]Tweet tweet)
        {
            Dictionary<string, bool> answer = new Dictionary<string, bool>();

            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                int user_id;
                int.TryParse(User.Identity.GetUserId(), out user_id);
                Twit found_user = Repo.GetTwitUser(user_id);
                if (found_user != null)
                {
                    Tweet new_tweet = new Tweet
                    {
                        Message = tweet.Message,
                        ImageURL = tweet.ImageURL,
                        TwitName = found_user
                        //CreatedAt = DateTime.Now
                    };
                    Repo.AddTweet(new_tweet);
                    answer.Add("successful", true);
                }
                else
                {
                    answer.Add("successful", false);
                }
            }
            else
            {
                answer.Add("successful", false);
            }
                    return answer;
            }
        
    

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            Repo.RemoveTweet(id);
        }
    }
}