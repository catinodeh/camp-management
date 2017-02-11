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
    [Authorize]
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

        public ActionResult ExtraActivities(int id)
        {
            var reg = db.RegistrationCampers.FirstOrDefault(r => r.RegistrationCamperId == id);
            var thisCamperActivities = db.RegistrationCamperExtraActivities
                .Where(e => e.RegistrationCamperId == id)
                .ToList();
            var activities = db.ExtraActivities.Where(ea => ea.CampSetupId == reg.CampSetupId).ToList();

            var obj = new UserActivity()
            {
                ExtraActivities = activities,
                CamperExtraActivities = thisCamperActivities
            };

            return View(obj);
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
                return Json(new {Success = true});
            }

            return
                Json(
                    new
                    {
                        Success = false,
                        Message = ModelState.Values.FirstOrDefault(v => v.Errors.Count > 0).Errors[0].ErrorMessage
                    });
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
                return Json(new {Success = true});
            }

            return
                Json(
                    new
                    {
                        Success = false,
                        Message = ModelState.Values.FirstOrDefault(v => v.Errors.Count > 0).Errors[0].ErrorMessage
                    });
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

        [HttpPut]
        public JsonResult Update(int id, string activityIds)
        {
            try
            {
                int[] ids = null;
                //Getting all current activities
                var currentActivities = db.RegistrationCamperExtraActivities.Where(a => a.RegistrationCamperId == id).ToList();

                if (string.IsNullOrEmpty(activityIds))
                {
                    for (int i = 0; i < currentActivities.Count(); i++)
                    {
                        db.RegistrationCamperExtraActivities.Remove(currentActivities[i]);
                    }
                }
                else
                {
                    ids = activityIds?.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                    for (int i = 0; i < currentActivities.Count(); i++)
                    {
                        if (!ids.Contains(currentActivities[i].ActivityId))
                        {
                            //Ids passed don't have the given existing id
                            db.RegistrationCamperExtraActivities.Remove(currentActivities[i]);
                        }
                    }

                    for (int i = 0; i < ids.Length; i++)
                    {
                        int activityId = ids[i];
                        if (!currentActivities.Any(a => a.ActivityId == ids[i]))
                        {
                            var currentPrice = (from rc in db.RegistrationCampers
                                                join cs in db.CampSetups on rc.CampSetupId equals cs.CampSetupId
                                                join ea in db.ExtraActivities on cs.CampSetupId equals ea.CampSetupId
                                                where rc.RegistrationCamperId == id && ea.ActivityId == activityId
                                                select ea.Price).FirstOrDefault();
                            //If activity is not found, it's a new one...
                            db.RegistrationCamperExtraActivities.Add(new RegistrationCamperExtraActivity()
                            {
                                ActivityId = ids[i],
                                RegistrationCamperId = id,
                                Price = currentPrice
                            });
                        }
                    }
                }

                db.SaveChanges();

                return Json(new {Success = true});
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = "Error when assigning extra activities.\n" + ex.Message });
            }
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
