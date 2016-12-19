using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampManagement.Data;
using CampManagement.Web.Models;

namespace CampManagement.Web.Controllers
{
    public class RegistrationsController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();
        private readonly string SP_SEARCHPROCEDURE = "dbo.usp_Search @Criteria, @SearchType";

        // GET: Registrations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Search(string criteria)
        {
            List<RegistrationSearchModel> searchResults = null;
            object[] _params = new object[]
            {
                new SqlParameter() { ParameterName = "Criteria", Value = criteria },
                new SqlParameter() { ParameterName = "SearchType", Value = 1 }
            };

            using (var db = new CampManagementDbContext())
            {
                searchResults = db.Database.SqlQuery<RegistrationSearchModel>(SP_SEARCHPROCEDURE, _params).ToList();
            }

            return View("ManageSearchResultCreate", searchResults);
        }

        [HttpPost]
        public ActionResult AddGuardian(int id)
        {
            var guardian = db.Guardians.FirstOrDefault(g => g.GuardianId == id);
            return View("~/Views/Guardians/Detail.cshtml", guardian);
        }

        [HttpPost]
        public ActionResult AddCamper(int id)
        {            
            var camper = db.Campers.FirstOrDefault(g => g.CamperId == id);
            var camps = (from c in db.Camps
                         join cs in db.CampSetups on c.CampId equals cs.CampId
                         where cs.Year == DateTime.Now.Year
                         orderby c.Name
                         select new AvailableCamps()
                         {
                            CampSetupId = cs.CampSetupId,
                            Description = c.Name,
                            FromGrade = cs.FromGrade,
                            ToGrade = cs.ToGrade,
                            Price = cs.CurrentPrice
                         }).ToList();

            var CurrentGrade = (DateTime.Now.Year - camper.BirthDate.Year) + 3;
            ViewBag.Grade = CurrentGrade;
            ViewBag.Camps = camps;
            ViewBag.CampSetupId = camps.FirstOrDefault(c => CurrentGrade >= c.FromGrade && CurrentGrade <= c.ToGrade)?.CampSetupId;

            return View("~/Views/Campers/Detail.cshtml", camper);
        }
    }
}