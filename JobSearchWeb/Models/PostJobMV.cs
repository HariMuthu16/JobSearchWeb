using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobSearchWeb.Models
{
    public class PostJobMV
    {
        public int PostJobID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int JobCategoryID { get; set; }

        [Required(ErrorMessage = "Required*")]
        [StringLength(1000,ErrorMessage = "Do not Enter more than 500 Characters!!" )]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Required*")]
        public string JobDescription { get; set; }

        [Required(ErrorMessage = "Required*")]
        public int MinSalary { get; set; }

        [Required(ErrorMessage = "Required*")]
        public int MaxSalary { get; set; }

        [Required(ErrorMessage = "Required*")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Required*")]
        public int Vacancy { get; set; }
        public int JobNatureID { get; set; }
        public System.DateTime PostDate { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime ApplicationLastDate { get; set; } = 
            DateTime.Now.AddDays(15);

        //public System.DateTime LastDate { get; set; }
        //public int JobRequirementID { get; set; }
        public int JobStatusID { get; set; }

        [Required(ErrorMessage = "Required*")]
        public string WebSiteUrl { get; set; }



    }
}