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
using CampManagement.Web.Models;
using Microsoft.AspNet.Identity;

namespace CampManagement.Web.Controllers
{
    [Authorize]
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

        [HttpPost]
        public JsonResult SendConfirmationEmail(int id)
        {
            SendEmailConfirmation(db.Guardians.Find(id), Request);
            return Json(true);
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
                guardian.Phone = guardian.Phone != null ? guardian.Phone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "") : null;
                guardian.CreatedDate = guardian.UpdatedDate = DateTime.Now;
                guardian.CreatedBy = User.Identity.GetUserId();
                guardian.UpdatedBy = User.Identity.GetUserId();
                guardian.EmailConfirmed = false;
                guardian.Active = true;
                guardian.RowGuid = Guid.NewGuid();
                db.Guardians.Add(guardian);
                db.SaveChanges();

                if (!string.IsNullOrEmpty(guardian.Email))
                    SendEmailConfirmation(guardian, Request);

                if (Request["autoclose"] == "1")
                {
                    TempData["close"] = true;
                }

                return RedirectToAction("Manage");
            }

            return View("Manage", guardian);
        }

        public void SendEmailConfirmation(Guardian guardian, HttpRequestBase request)
        {
            if (guardian.Email == null || guardian.Email.IndexOf("@") < 0) return;
            var url = request?.Url == null ? "" : request.Url.ToString();
            if (!string.IsNullOrEmpty(request?.Url?.Query)) url = url.Replace(request.Url.Query, "");
            if (!string.IsNullOrEmpty(request?.Url?.LocalPath)) url = url.Replace(request.Url.LocalPath, "");

            SendWithUs.Send(SendWithUs.TEMPLATE_NewGuardian, guardian.Name, guardian.Email, new
            {
                name = guardian.Name,
                button_text = "Confirmar!",
                url = $"{url + "/Guardians/ConfirmEmail"}/{guardian.RowGuid}"
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmEmail(Guid id)
        {
            var guardian = db.Guardians.FirstOrDefault(g => g.RowGuid == id);
            if (guardian != null)
            {
                guardian.EmailConfirmed = true;
                guardian.UpdatedDate = DateTime.Now;
                var otherEmails =
                    db.Guardians.Where(
                            e => e.Email == guardian.Email && (e.EmailConfirmed == false || e.EmailConfirmed == null))
                        .ToList();

                foreach (var otherEmail in otherEmails)
                {
                    otherEmail.EmailConfirmed = true;
                    otherEmail.UpdatedDate = DateTime.Now;
                }
            }

            db.SaveChanges();

            return View("EmailConfirmed");
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
                var currentData = db.Guardians.Where(g => g.GuardianId == guardian.GuardianId)
                                    .Select(g => new
                                    {
                                        g.Email,
                                        g.RowGuid
                                    }).FirstOrDefault();

                if (currentData?.Email != guardian.Email)
                {
                    guardian.EmailConfirmed = false;
                    if (!string.IsNullOrEmpty(guardian.Email))
                    {
                        var url = Request.Url.ToString();
                        if (Request.Url.Query != "") url = url.Replace(Request.Url.Query, "");
                        if (Request.Url.LocalPath != "") url = url.Replace(Request.Url.LocalPath, "");

                        SendWithUs.Send(SendWithUs.TEMPLATE_NewGuardian, guardian.Name, guardian.Email, new
                        {
                            name = guardian.Name,
                            button_text = "Confirmar!",
                            url = $"{url + Url.Action("ConfirmEmail")}/{currentData?.RowGuid}"
                        });
                    }   
                }

                guardian.EmailConfirmed = guardian.EmailConfirmed ?? true;
                guardian.Phone = guardian.Phone != null ? guardian.Phone.Replace(" ","").Replace("(", "").Replace(")", "").Replace("-", "") : null;
                guardian.UpdatedBy = User.Identity.GetUserId();
                guardian.UpdatedDate = DateTime.Now;
                db.Entry(guardian).State = EntityState.Modified;
                db.SaveChanges();

                if (Request["autoclose"] == "1")
                {
                    TempData["close"] = true;
                }

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
