using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobSearchWeb.Models
{
    public class JobRequirementsMV
    {
        public JobRequirementsMV()
        {
            Details = new List<JobRequirementDetailsTable>();
        }
      
       // public int JobRequirementDetailsID { get; set; }
        [Required(ErrorMessage = "Required**")]
        public int JobRequirementID { get; set; }
        [Required(ErrorMessage = "Required**")]
        public string JobRequirementDetails { get; set; }
        public int PostJobID { get; set; }

        public List<JobRequirementDetailsTable> Details { get; set; }    

    }
}