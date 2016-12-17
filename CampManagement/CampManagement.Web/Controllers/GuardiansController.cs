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
    public class GuardiansController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        // GET: Guardians
        public ActionResult Manage(int? id = null)
        {
            Guardian model = new Guardian() { GuardianId = 0};
            if (id.HasValue)
                model = db.Guardians.FirstOrDefault(g => g.GuardianId == id.Value);

            return View(model);
        }

        public List<Guardian> GetActives()
        {
            return db.Guardians.OrderBy(g => g.Name).ToList();
        }

        public JsonResult GetAll()
        {
            return Json(db.Guardians.OrderBy(g => g.Name).ToList(), JsonRequestBehavior.AllowGet);
        }

        // POST: Guardians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Guardian guardian)
        {
            if (ModelState.IsValid)
            {
                guardian.Phone = guardian.Phone != null ? guardian.Phone.Replace("(", "").Replace(")", "").Replace("-", "") : null;
                guardian.CreatedDate = guardian.UpdatedDate = DateTime.Now;
                guardian.CreatedBy = User.Identity.GetUserId();
                guardian.UpdatedBy = User.Identity.GetUserId();
                db.Guardians.Add(guardian);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View("Manage", guardian);
        }

        // POST: Guardians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guardian guardian)
        {
            if (ModelState.IsValid)
            {
                guardian.Phone = guardian.Phone != null ? guardian.Phone.Replace("(", "").Replace(")", "").Replace("-", "") : null;
                guardian.UpdatedBy = User.Identity.GetUserId();
                guardian.UpdatedDate = DateTime.Now;
                db.Entry(guardian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View("Manage", guardian);
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
