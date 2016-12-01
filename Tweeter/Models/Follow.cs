using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tweeter.Models
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }
        public Twit TwitFollower{ get; set; }
        public Twit TwitFollowed { get; set; }
    }
}