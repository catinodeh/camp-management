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
using CampManagement.Web.Filters;
using Microsoft.AspNet.Identity;

namespace CampManagement.Web.Controllers
{
    [Authorize]
    public class CampSetupsController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        public JsonResult GetThisYearSetups()
        {
            int thiYear = DateTime.Now.Year;
            var list = (from c in db.Camps
                        join cs in db.CampSetups on c.CampId equals cs.CampId
                        where cs.Year == thiYear
                        orderby c.Name
                        select new
                        {
                            c.CampId,
                            c.Name
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: CampSetups
        [GetModelStateFromTempData]
        public ActionResult Manage(int id)
        {
            CampSetup model = null;
            var thisYear = DateTime.Now.Year;
            if (TempData["model"] != null) model = (CampSetup) TempData["model"];
            var list = db.CampSetups
                        .Include("ExtraActivities")
                        .Where(cs => cs.CampId == id && cs.Year <= thisYear).Take(5).ToList();

            if (model != null)
            {
                if (list.Any(l => l.Year == model.Year))
                    list[list.FindIndex(l => l.Year == model.Year)] = model;
                else
                    list.Add(model);
            }
            else
            {
                if (!list.Any(m => m.Year == thisYear))
                    list.Add(new CampSetup()
                    {
                        Year = thisYear,
                        CampSetupId = 0
                    });
            }

            ViewData["CampName"] = db.Camps.FirstOrDefault(c => c.CampId == id).Name;

            return View(list.OrderByDescending(l => l.Year).ToList());
        }

        [HttpGet]
        public ActionResult Create(CampSetup model)
        {
            if (model.Year == 0) model = null;
            return View("CreateOrEdit", model);
        }

        // POST: CampSetups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SetTempDataModelState]
        public ActionResult CreateOrEdit(CampSetup campSetup)
        {
            if (ModelState.IsValid)
            {
                campSetup.UpdatedBy = User.Identity.GetUserId();
                if (campSetup.CampSetupId == 0)
                {
                    campSetup.CreatedDate = campSetup.UpdatedDate = DateTime.Now;
                    campSetup.CreatedBy = User.Identity.GetUserId();
                    db.CampSetups.Add(campSetup);
                }
                else
                {             
                    campSetup.UpdatedBy = User.Identity.GetUserId();
                    campSetup.UpdatedDate = DateTime.Now;
                    db.Entry(campSetup).State = EntityState.Modified;
                }

                
                db.SaveChanges();
                return RedirectToAction("Edit", "Camps", new { id = campSetup.CampId});
            }

            TempData["model"] = campSetup;
            return RedirectToAction("Manage", new { id = campSetup.CampId });
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
