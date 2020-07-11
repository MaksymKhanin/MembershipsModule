using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contenter.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Forename { get; set; }
        public string Sirname { get; set; }
        public string Email { get; set; }


        //think of validation
        public ICollection<Membership> Memberships { get; set; }
    }
}