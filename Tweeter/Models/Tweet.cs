using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweeter.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string Message { get; set; }
        public ApplicationUser Author { get; set; } //gets data from dbo.AspNetUsers
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Tweet> Replies { get; set; } // Self referential (refers to model it is defined in through List<Tweet>)
    
    }
}