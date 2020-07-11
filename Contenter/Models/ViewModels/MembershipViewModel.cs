using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contenter.Models.ViewModels
{
    public class MembershipViewModel
    {
        public int Id { get; set; }
        public double MemebrshipNumber { get; set; }
        public string Type { get; set; }
        public double AccountBalance { get; set; }
        public int PersonId { get; set; }
    }
}