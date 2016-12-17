using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampManagement.Data;
using CampManagement.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace CampManagement.Web.Controllers
{
    public class ExtraActivitiesController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        // GET: ExtraActivities
        public ActionResult Index(int id, bool? readOnly = false)
        {
            List<ExtraActivity> result = new List<ExtraActivity>();

            if (id > 0)
            {
                result = db.ExtraActivities.Where(e => e.CampSetupId == id).OrderBy(e => e.Description).ToList();
            }

            return View(readOnly == true ? "IndexReadOnly" : "Index", result);
        }        

        // POST: ExtraActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "CampSetupId,Description,Price")] ExtraActivity extraActivity)
        {
            if (ModelState.IsValid)
            {
                extraActivity.UpdatedBy = User.Identity.GetUserId();
                extraActivity.CreatedDate = extraActivity.UpdatedDate = DateTime.Now;
                extraActivity.CreatedBy = User.Identity.GetUserId();
                db.ExtraActivities.Add(extraActivity);
                db.SaveChanges();
                return Json(new { Success = true });
            }

            return Json(new { Success = false, Message = ModelState.Values.FirstOrDefault(v => v.Errors.Count > 0).Errors[0].ErrorMessage });
        }
        
        // POST: ExtraActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(int id, ExtraActivity extraActivity)
        {
            if (ModelState.IsValid)
            {
                var activity = db.ExtraActivities.FirstOrDefault(a => a.ActivityId == id);
                activity.Description = extraActivity.Description;
                activity.Price = extraActivity.Price;
                activity.UpdatedBy = User.Identity.GetUserId();
                activity.UpdatedDate = DateTime.Now;

                db.SaveChanges();
                return Json(new { Success = true });
            }

            return Json(new { Success = false, Message = ModelState.Values.FirstOrDefault(v => v.Errors.Count > 0).Errors[0].ErrorMessage });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (db.RegistrationCamperExtraActivities.Any(e => e.ActivityId == id))
                return
                    Json(new {Success = false, Message = "There is a registration with this activity. Remove it first."});

            var extraActivity = db.ExtraActivities.FirstOrDefault(ea => ea.ActivityId == id);
            if (extraActivity != null)
                db.ExtraActivities.Remove(extraActivity);

            db.SaveChanges();
            return Json(new {Success = true});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
