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
    public class CampersController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        // GET: Guardians
        public ActionResult Manage(int? id = null)
        {
            Camper model = new Camper() { CamperId = 0 };
            if (id.HasValue)
                model = db.Campers.FirstOrDefault(g => g.CamperId == id.Value);

            return View(model);
        }

        public List<Camper> GetActives()
        {
            return db.Campers.OrderBy(g => g.FirstName).ToList();
        }

        public JsonResult GetAll()
        {
            return Json(db.Campers.OrderBy(g => g.FirstName).ToList(), JsonRequestBehavior.AllowGet);
        }


        // POST: Campers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Camper camper)
        {
            if (ModelState.IsValid)
            {
                camper.Phone = camper.Phone != null ? camper.Phone.Replace("(", "").Replace(")", "").Replace("-", "") : null;
                camper.CreatedDate = camper.UpdatedDate = DateTime.Now;
                camper.CreatedBy = User.Identity.GetUserId();
                camper.UpdatedBy = User.Identity.GetUserId();
                db.Campers.Add(camper);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View("Manage", camper);
        }

        // POST: Campers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Camper camper)
        {
            if (ModelState.IsValid)
            {
                camper.Phone = camper.Phone != null ? camper.Phone.Replace("(", "").Replace(")", "").Replace("-", "") : null;
                camper.UpdatedBy = User.Identity.GetUserId();
                camper.UpdatedDate = DateTime.Now;
                db.Entry(camper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View("Manage", camper);
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
