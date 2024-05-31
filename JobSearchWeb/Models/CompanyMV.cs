using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSearchWeb.Models
{
    public class CompanyMV
    {
        public int CompanyID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string CompanyName { get; set; }
        public string ContactNo { get; set; }
        public string PhoneNo { get; set; }
        public string EmailAddress { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }

    }
}