using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contenter.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Sirname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int FollowersNumber { get; set; }
        public int FollowingNumber { get; set; }
        

    }
}