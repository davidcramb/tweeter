using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tweeter.Models
{
    public class Twit
    {
        [Key]
        public string Username { get; set; } //prevents duplicates instead of using int userid
        public ApplicationUser BaseUser { get; set; }
        public List<Twit> Followers { get; set; }
    }
}