using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobSearchWeb.Models
{
    public class UserMV
    {
        public UserMV()
        {
            Company = new CompanyMV();
        }

        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        [Required(ErrorMessage="User Name is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "EmailAddress is Required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "ContactNo is Required")]
        public string ContactNo { get; set; }

        public bool AreYouJobProvider { get;set;}
        public CompanyMV Company { get; set; }

    }
}