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
    public class CampsController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        // GET: Camps/Create
        public ActionResult Create()
        {
            ViewData["data"] = db.Camps.OrderBy(c => c.Name).ToList();
            return View(new Camp());
        }

        // POST: Camps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CampId,Name,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Camp camp)
        {
            if (ModelState.IsValid)
            {
                db.Camps.Add(camp);

                camp.CreatedDate = camp.UpdatedDate = DateTime.Now;
                camp.CreatedBy = User.Identity.GetUserId();
                camp.UpdatedBy = User.Identity.GetUserId();

                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewData["data"] = db.Camps.OrderBy(c => c.Name).ToList();
            return View(camp);
        }

        // GET: Camps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Camp camp = db.Camps.Find(id);
            if (camp == null)
            {
                return HttpNotFound();
            }
            ViewData["data"] = db.Camps.OrderBy(c => c.Name).ToList();
            return View("Create", camp);
        }

        // POST: Camps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CampId,Name,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Camp camp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(camp).State = EntityState.Modified;
                camp.UpdatedBy = User.Identity.GetUserId();
                camp.UpdatedDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewData["data"] = db.Camps.OrderBy(c => c.Name).ToList();
            return View("Create", camp);
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