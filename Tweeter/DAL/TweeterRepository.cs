using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweeter.Models;

namespace Tweeter.DAL
{
    public class TweeterRepository
    {
        public TweeterContext Context { get; set; }

        public TweeterRepository()
        {
            Context = new TweeterContext();
        }
        public TweeterRepository(TweeterContext _context)
        {
            Context = _context;
        }

        public List<string> GetAllUsernames()
        {
            var users = Context.Users.ToList();
            List<string> userList = new List<string>();
            foreach (var user in users)
            {
                userList.Add(user.ToString());
            }
            return userList;
        }

        public void CreateUsername(Twit new_Twit)
        {
            Context.TweeterUsers.Add(new_Twit);
        }
    }
}