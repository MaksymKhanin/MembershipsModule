using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Contenter.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public int MemebrshipNumber { get; set; }
        public MembershipType Type { get; set; }
        public double AccountBalance { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

    }


    public enum MembershipType
    {
        Primary = 1,
        Secondary = 2
    }
}