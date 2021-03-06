﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampManagement.Data;
using CampManagement.Domain.Entities;
using CampManagement.Web.Models;
using Microsoft.AspNet.Identity;

namespace CampManagement.Web.Controllers
{
    [Authorize]
    public class RegistrationsController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();
        private readonly string SP_SEARCHPROCEDURE = "dbo.usp_Search @Criteria, @SearchType";

        [HttpPost]
        public JsonResult ConfirmRegistration(int id)
        {
            var reg = db.RegistrationCampers.FirstOrDefault(r => r.RegistrationCamperId == id);
            reg.Registered = true;
            reg.UpdatedBy = User.Identity.GetUserId();
            reg.RegisteredBy = reg.UpdatedBy;
            reg.UpdatedDate = DateTime.Now;
            db.SaveChanges();
            return Json(true);
        }

        [HttpPost]
        public JsonResult ConfirmCancellation(int id)
        {
            var reg = db.RegistrationCampers.FirstOrDefault(r => r.RegistrationCamperId == id);
            reg.UpdatedBy = User.Identity.GetUserId();
            reg.CancelFinalized = DateTime.Now;
            reg.UpdatedDate = reg.CancelFinalized.Value;
            db.SaveChanges();
            return Json(true);
        }

        [HttpPost]
        public JsonResult SaveComments(int id, string notes)
        {
            var reg = db.Registrations.FirstOrDefault(r => r.RegistrationId == id);
            reg.Notes = notes;
            reg.UpdatedBy = User.Identity.GetUserId();
            reg.UpdatedDate = DateTime.Now;
            db.SaveChanges();
            return Json(true);
        }

        [HttpGet]
        public ActionResult Details(int id, int? camperId = null)
        {
            var userid = User.Identity.GetUserId();
            ViewData["user"] = db.Users.FirstOrDefault(u => u.Id == userid)?.UserName;
            ViewData["camperid"] = camperId;
            return View("CurrentSetupHorizontal", GetById(id));
        }

        [HttpGet]
        public ActionResult Finish(int id)
        {
            var reg = db.Registrations.FirstOrDefault(r => r.RegistrationId == id);

            if (reg.CompletedDate == null)
                reg.CompletedDate = DateTime.Now;

            db.SaveChanges();

            SendConfirmation(reg);

            return RedirectToAction("Index");
        }

        private void SendConfirmation(Registration reg)
        {
            try
            {
                var guardian = db.Guardians.Find(reg.GuardianId);
                if (guardian.EmailConfirmed != true)
                {
                    //Guardian email not yet confirmed....
                    var guardianCTR = new GuardiansController();
                    guardianCTR.SendEmailConfirmation(guardian, Request);
                }
            }
            catch (Exception)
            {
                
            }
        }

        [HttpGet]
        public ActionResult FinishAndPay(int id)
        {
            var reg = db.Registrations.FirstOrDefault(r => r.RegistrationId == id);
            if (reg.CompletedDate == null)
                reg.CompletedDate = DateTime.Now;

            db.SaveChanges();

            SendConfirmation(reg);

            return RedirectToAction("Manage","Payments", new { id = id });
        }

        [HttpGet]
        public JsonResult ThisYearRegistrations(int? campId = null,int? filter = null)
        {
            var query = db.CurrentRegistrations.AsQueryable();

            if (campId > 0)
                query = query.Where(r => r.CampId == campId);

            if (filter > 0)
            {
                if (filter == 1)
                    query = query.Where(r => r.Cancelled == false && r.Registered == true && r.CompletedDate.HasValue);

                if (filter == 2)
                    query = query.Where(r => r.Cancelled == false && r.Registered == false);

                if (filter == 3)
                    query = query.Where(r => r.Cancelled == true && r.CancelFinalized.HasValue);

                if (filter == 4)
                    query = query.Where(r => r.Cancelled == true && !r.CancelFinalized.HasValue);

                if (filter == 5)
                    query = query.Where(r => r.Cancelled == false);

                if (filter == 6)
                    query = query.Where(r => (r.TotalPayments == 0 || r.TotalPayments == null) && r.Cancelled != true);
            }

            query = query
                .OrderBy(r => r.CampId)
                .ThenBy(r => r.RegistrationId)
                .ThenBy(r => r.LastName);

            return Json(query.OrderBy(r => r.RegistrationId).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            DateTime now = DateTime.Now;
            var newRegistration = new Registration()
            {
                CreatedDate = now,
                UpdatedDate = now,
                CreatedBy = User.Identity.GetUserId(),
                UpdatedBy = User.Identity.GetUserId()
            };

            db.Registrations.Add(newRegistration);
            db.SaveChanges();

            return RedirectToAction("Index", new {id = newRegistration.RegistrationId});
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return View();

            var registration = GetById(id.Value);
            return View("Manage", registration);
        }

        public ActionResult Search(string criteria)
        {
            List<RegistrationSearchModel> searchResults = null;
            ViewBag.criteria = criteria;
            object[] _params = new object[]
            {
                new SqlParameter() {ParameterName = "Criteria", Value = criteria},
                new SqlParameter() {ParameterName = "SearchType", Value = 1}
            };

            using (var db = new CampManagementDbContext())
            {
                searchResults = db.Database.SqlQuery<RegistrationSearchModel>(SP_SEARCHPROCEDURE, _params).ToList();
            }

            return View("ManageSearchResults", searchResults);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="guardianId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Registrations/{registrationId}/AddGuardian/{guardianId}")]
        public ActionResult AddGuardian(int registrationId, int guardianId)
        {
            var registration = GetById(registrationId);

            if (!registration.GuardianId.HasValue)
            {
                registration.GuardianId = guardianId;
                db.SaveChanges();
            }
            else
            {
                if (registration.GuardianId != guardianId)
                    return Json(new DefaultJsonReturn() {Success = false, Message = "Registration already has a guardian"});
            }

            var guardian = db.Guardians.FirstOrDefault(g => g.GuardianId == guardianId);

            return CurrentSetup(registration);
            //return RedirectToAction("CurrentSetup", new {registrationId = registrationId});
        }

        public ActionResult CurrentSetup(int id)
        {
            return View("CurrentSetup", GetById(id));
        }

        private ActionResult CurrentSetup(Registration registration)
        {
            return View("CurrentSetup", registration);
        }

        [HttpDelete]
        [Route("Registrations/{registrationId}/RemoveGuardian/{guardianId}")]
        public ActionResult RemoveGuardian(int registrationId, int guardianId)
        {
            var registration = GetById(registrationId);
            registration.CompletedDate = null;
            registration.GuardianId = null;
            registration.UpdatedDate = DateTime.Now;
            registration.UpdatedBy = User.Identity.GetUserId();
            db.SaveChanges();

            return CurrentSetup(registration);
        }

        [HttpPut]
        [Route("Registrations/{registrationId}/UpdateCamperSetup/{registrationCamperId}")]
        public ActionResult UpdateCamper(int registrationId, int registrationCamperId, UpdateRegistrationCamperModel model)
        {
            var regCamper = db.RegistrationCampers.FirstOrDefault(r => r.RegistrationCamperId == registrationCamperId);
            if (regCamper != null)
            {
                if (model.Grade <= 0)
                    return Json(new DefaultJsonReturn() { Success = false, Message = "Grade should be greater than zero" });

                if (model.CampSetupId == 0)
                    return Json(new DefaultJsonReturn() { Success = false, Message = "Please select a camp setup" });

                var campSetup = db.CampSetups.FirstOrDefault(cs => cs.CampSetupId == model.CampSetupId);

                if (campSetup != null)
                    if (model.Grade < campSetup.FromGrade || model.Grade > campSetup.ToGrade)
                        return Json(new DefaultJsonReturn() { Success = false, Message = "Provided Grade is outside of the Camp range" });

                if (model.CampSetupId != regCamper.CampSetupId)
                {
                    //Cleaning existing Extra Activities...
                    db.Database.ExecuteSqlCommand($"DELETE FROM dbo.RegistrationCamperExtraActivity WHERE RegistrationCamperId = {regCamper.RegistrationCamperId}");
                }

                regCamper.Price = model.Price;
                regCamper.CampSetupId = model.CampSetupId;
                regCamper.Grade = model.Grade;
            }
            
            db.SaveChanges();

            return CurrentSetup(GetById(registrationId));
        }

        [HttpPost]
        public ActionResult CancelCamper(int registrationId,int camperId,string cancelNotes, string redirectToUrl)
        {
            //int registrationId = 0;
            //int camperId = 0;
            //string cancelNotes = "";
            var camper = db.RegistrationCampers.FirstOrDefault(rc => rc.RegistrationId == registrationId && rc.CamperId == camperId);
            
            if (camper != null)
            {
                camper.UpdatedDate = DateTime.Now;
                camper.CancelNotes = cancelNotes;
                camper.Cancelled = true;
                camper.CancelledBy = User.Identity.GetUserId();
            }                       

            db.SaveChanges();

            return Redirect(redirectToUrl);
        }

        [HttpDelete]
        [Route("Registrations/{registrationId}/RemoveCamper/{camperId}")]
        public ActionResult RemoveCamper(int registrationId, int camperId)
        {
            var camper = db.RegistrationCampers.FirstOrDefault(rc => rc.RegistrationId == registrationId && rc.CamperId == camperId);
            var registration = db.Registrations.FirstOrDefault(r => r.RegistrationId == registrationId);

            if (registration != null)
            {
                registration.CompletedDate = null;
                registration.UpdatedDate = DateTime.Now;
                registration.UpdatedBy = User.Identity.GetUserId();
            }

            if (camper != null)
                db.RegistrationCampers.Remove(camper);

            db.SaveChanges();

            return CurrentSetup(GetById(registrationId));
        }

        [HttpPost]
        [Route("Registrations/{registrationId}/AddCamper/{camperId}")]
        public ActionResult AddCamper(int registrationId, int camperId)
        {
            try
            {
                var camper = db.Campers.FirstOrDefault(g => g.CamperId == camperId);
                // Save today's date.
                var today = DateTime.Today;
                // Calculate the age.
                var age = today.Year - camper.BirthDate.Year;
                // Do stuff with it.
                if (camper.BirthDate > today.AddYears(-age)) age--;

                var currentGrade = age - 5 <= 0 ? 1 : (age - 5 > 12 ? 12 : age - 5);
                var campSetupId = db.CampSetups
                                    .Where(c => currentGrade >= c.FromGrade 
                                                    && currentGrade <= c.ToGrade 
                                                    && c.Year == DateTime.Now.Year)
                                    .Select(s => new
                                    {
                                        s.CurrentPrice,
                                        s.CampSetupId
                                    }).FirstOrDefault();

                var registrationCamper = db.RegistrationCampers.FirstOrDefault(rc => rc.RegistrationId == registrationId && rc.CamperId == camperId);

                if (campSetupId == null)
                    return Json(new DefaultJsonReturn() { Success = false, Message = $"There is no camp available for the {currentGrade}th grade!" });

                if (registrationCamper != null)
                    return Json(new DefaultJsonReturn() { Success = false, Message = "Camper already added to this registration!" });

                if (db.RegistrationCampers.Any(c => c.CamperId == camperId && c.RegistrationId != registrationId && c.CreatedDate.Year == DateTime.Now.Year && !c.Cancelled))
                    return Json(new DefaultJsonReturn() { Success = false, Message = "Camper is already in another registration!" });

                //Searching for previous grade and adding +1
                var pastYear = DateTime.Now.Year - 1;
                var previousGrade = (from rc in db.RegistrationCampers
                                    join cs in db.CampSetups on rc.CampSetupId equals cs.CampSetupId
                                    join ca in db.Camps on cs.CampId equals ca.CampId
                                    where rc.CamperId == camper.CamperId && cs.Year == pastYear
                                    select rc.Grade).FirstOrDefault();

                if (previousGrade > 0)
                    currentGrade = (previousGrade > 11 ? 11 : previousGrade) + 1;

                db.RegistrationCampers.Add(new RegistrationCamper()
                {
                    CampSetupId = campSetupId.CampSetupId,
                    Price = campSetupId.CurrentPrice,
                    CamperId = camperId,
                    RegistrationId = registrationId,
                    Registered = false,
                    CreatedBy = User.Identity.GetUserId(),
                    UpdatedBy = User.Identity.GetUserId(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Grade = 0 //currentGrade
                });

                db.SaveChanges();

                return CurrentSetup(GetById(registrationId));
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(DateTime.Now.ToString("ddMMyyyyHHmmss")+".log", ex.Message);
                return Json(new DefaultJsonReturn() { Success = false, Message = "Oops...There was an error trying to add this camper!" });
            }         
            
            //return View("~/Views/Campers/Detail.cshtml", camper);
        }


        #region Private Methods

        public Registration GetById(int registrionid)
        {
            Registration registration = db.Registrations.Include("Guardian").FirstOrDefault(r => r.RegistrationId == registrionid);
            registration.Campers = db.RegistrationCampers.Include("Camper").Where(rc => rc.RegistrationId == registrionid).ToList();

            foreach (var registrationCamper in registration.Campers)
            {
                registrationCamper.CampSetup = db.CampSetups.FirstOrDefault(cs => cs.CampSetupId == registrationCamper.CampSetupId);
                registrationCamper.ExtraActivities = db.RegistrationCamperExtraActivities.Where(ea => ea.RegistrationCamperId == registrationCamper.RegistrationCamperId).ToList();
            }

            //return db.Registrations
            //        .Include("Guardian")
            //        .Include("Campers.Camper")
            //        .Include("Campers.CampSetup")
            //        .Include("Campers.ExtraActivities")
            //        .FirstOrDefault(r => r.RegistrationId == registrionid);

            return registration;
        }
        #endregion
    }
}