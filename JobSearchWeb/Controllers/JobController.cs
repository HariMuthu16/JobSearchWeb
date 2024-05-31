using JobSearchWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSearchWeb.Controllers
{
    public class JobController : Controller
    {
        private DBJobModel db = new DBJobModel();

        // GET: Job
        public ActionResult PostJob()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var job = new PostJobMV();
            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(),
                "JobCategoryID","JobCategory","0");
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(),
                "JobNatureID", "JobNature", "0");

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostJob(PostJobMV postJobMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            postJobMV.UserID = userid;
            postJobMV.CompanyID = companyid;
            if(ModelState.IsValid) 
            {
                var post = new PostJobTable();

                post.PostJobID = postJobMV.PostJobID;
                post.UserID = postJobMV.UserID;
                post.CompanyID = postJobMV.CompanyID;
                post.JobCategoryID = postJobMV.JobCategoryID;
                post.JobTitle = postJobMV.JobTitle;
                post.JobDescription = postJobMV.JobDescription;
                post.MinSalary = postJobMV.MinSalary;
                post.MaxSalary = postJobMV.MaxSalary;
                post.Location = postJobMV.Location;
                post.Vacancy = postJobMV.Vacancy;
                post.JobNatureID = postJobMV.JobNatureID;
                post.PostDate = DateTime.Now;
                post.ApplicationLastDate = postJobMV.ApplicationLastDate;
                post.LastDate = postJobMV.ApplicationLastDate;
                // post.JobRequirementID = postJobMV.jobr
                post.JobStatusID = 1;
                post.WebSiteUrl = postJobMV.WebSiteUrl;
                db.PostJobTables.Add(post); 
                db.SaveChanges();

                return RedirectToAction("CompanyJobsList");
            }





            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(),
                "JobCategoryID", "JobCategory", "0");
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(),
                "JobNatureID", "JobNature", "0");



            return View(postJobMV);
        }

        public ActionResult CompanyJobsList()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
             var allpost = db.PostJobTables.Where(c=>c.CompanyID == companyid && c.UserID == userid).ToList();
            return View(allpost);
        }

        public ActionResult AllCompanyPendingJobs()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var allpost = db.PostJobTables.ToList();
            if (allpost.Count()>0)
            {
                allpost = allpost.OrderByDescending(o=>o.PostJobID).ToList();
            }
            return View(allpost);
        }






        public ActionResult AddRequirements(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var details = db.JobRequirementDetailsTables.Where(j=>j.PostJobID == id).ToList();
            if(details.Count()> 0)
            {
                details = details.OrderBy(r=>r.JobRequirementID).ToList();  
            }
            
            var requirements = new JobRequirementsMV();
            requirements.Details = details;
            requirements.PostJobID = (int)id;
            ViewBag.JobRequirementID = new SelectList(
                db.JobRequirementTables.ToList(),
                "JobRequirementID", "JobRequirementTitle", "0");

          return View(requirements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRequirements(JobRequirementsMV jobRequirementsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            try
            {
                var requirements = new JobRequirementDetailsTable();
                requirements.JobRequirementID = jobRequirementsMV.JobRequirementID;
                requirements.JobRequirementDetails = jobRequirementsMV.JobRequirementDetails;
                requirements.PostJobID = jobRequirementsMV.PostJobID;
                db.JobRequirementDetailsTables.Add(requirements);
                db.SaveChanges();
                return RedirectToAction("AddRequirements", new {id=requirements.PostJobID});
            }
            catch (Exception )
            {
                var details = db.JobRequirementDetailsTables.Where(j => j.PostJobID == jobRequirementsMV.PostJobID).ToList();
                if (details.Count() > 0)
                {
                    details = details.OrderBy(r => r.JobRequirementID).ToList();
                }
                jobRequirementsMV.Details = details;
                ModelState.AddModelError("JobRequirementID", "Required*");
               
            }

           // jobRequirementsMV.JobRequirementDetails = string.Empty;

           //var details = db.JobRequirementDetailsTables.Where(j => j.PostJobID == requirements.PostJobID).ToList();
           // if (details.Count() > 0)
           // {
           //     details = details.OrderBy(r => r.JobRequirementID).ToList();
           // }
           // jobRequirementsMV.Details = details;

            ViewBag.JobRequirementID = new SelectList(
               db.JobRequirementTables.ToList(),
               "JobRequirementID", "JobRequirementTitle", jobRequirementsMV.JobRequirementID);

            return View(jobRequirementsMV);
          
        }

        public ActionResult DeleteRequirements(int? id)
        {
            var jobpostid = db.JobRequirementDetailsTables.Find(id).PostJobID;
            var requirements = db.JobRequirementDetailsTables.Find(id);
            db.Entry(requirements).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("AddRequirements", new {id = jobpostid });
        }

        public ActionResult DeleteJobPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var jobpost = db.PostJobTables.Find(id);
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("CompanyJobsList");
        }

        public ActionResult ApprovedPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 2;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }

        public ActionResult CanceledPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 3;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }




        public ActionResult JobDetails(int? id)
        {
            var getpostjob = db.PostJobTables.Find(id);
            var postjob = new PostJobDetailMV();

            postjob.PostJobID = getpostjob.PostJobID;
            postjob.Company = getpostjob.CompanyTable.CompanyName;
            postjob.JobCategory = getpostjob.JobCategoryTable.JobCategory;
            postjob.JobTitle = getpostjob.JobTitle;
            postjob.JobDescription = getpostjob.JobDescription;
            postjob.MinSalary = getpostjob.MinSalary;
            postjob.MaxSalary = getpostjob.MaxSalary;
            postjob.Location = getpostjob.Location;
            postjob.Vacancy = getpostjob.Vacancy;
            postjob.JobNature = getpostjob.JobNatureTable.JobNature;
            postjob.PostDate = getpostjob.PostDate;
            postjob.ApplicationLastDate = getpostjob.ApplicationLastDate;
            postjob.WebSiteUrl = getpostjob.WebSiteUrl;

            getpostjob.JobRequirementDetailsTables = getpostjob.JobRequirementDetailsTables.OrderBy(d=>d.JobRequirementID).ToList();
            int jobrequirementid = 0;
            var jobrequirements = new JobRequirementMV();
           foreach (var detail in getpostjob.JobRequirementDetailsTables)
            {
                var jobrequirementsdetails = new JobRequirementDetailMV();

                if (jobrequirementid == 0)
                { 
                    jobrequirements.JobRequirementID = detail.JobRequirementID;
                    jobrequirements.JobRequirementTitle = detail.JobRequirementTable.JobRequirementTitle;
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequirementid = detail.JobRequirementID;

                }
                else if (jobrequirementid == detail.JobRequirementID)
                {
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequirementid = detail.JobRequirementID;
                }
                else if(jobrequirementid != detail.JobRequirementID)
                {
                    postjob.Requirements.Add(jobrequirements);
                    jobrequirements = new JobRequirementMV();
                    jobrequirements.JobRequirementID = detail.JobRequirementID;
                    jobrequirements.JobRequirementTitle = detail.JobRequirementTable.JobRequirementTitle;
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequirementid = detail.JobRequirementID;

                }

            }
            postjob.Requirements.Add(jobrequirements);
            return View(postjob);
        }

        public ActionResult FilterJobs()
        {
            var obj = new FilterJobsMV();
            var date = DateTime.Now.Date;
            var result = db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID== 2).ToList();
            obj.Result = result;
            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(),
                "JobCategoryID", "JobCategory", "0");
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(),
                "JobNatureID", "JobNature", "0");


            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterJobs(FilterJobsMV filterJobsMV)
        {   
            var date = DateTime.Now.Date;
            var result = db.PostJobTables.Where(r=>r.ApplicationLastDate >= date && r.JobStatusID == 2 && r.JobCategoryID == filterJobsMV.JobCategoryID && r.JobNatureID == filterJobsMV.JobNatureID).ToList();
            filterJobsMV.Result = result;

            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(),
                "JobCategoryID", "JobCategory", filterJobsMV.JobCategoryID);
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(),
                "JobNatureID", "JobNature", filterJobsMV.JobNatureID);


            return View(filterJobsMV);
        }


        public ActionResult FindJobs()
        {
           // var jobs = new PostJobTable();
            var job = db.PostJobTables.Where(c => c.JobStatusID == 2).ToList();
            return View(job);
        }



    }
}