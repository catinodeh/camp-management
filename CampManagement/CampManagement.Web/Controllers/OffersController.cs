using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampManagement.Data;
using CampManagement.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace CampManagement.Web.Controllers
{
    public class OffersController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        // GET: Offers
        public ActionResult Manage(int? id = null)
        {
            Offer offerSetup = null;
            offerSetup = db.Offers.FirstOrDefault(o => o.Year == DateTime.Now.Year);

            if (offerSetup == null)
            {
                offerSetup = new Offer()
                {
                    Year = DateTime.Now.Year,
                    Balance = 0
                };

                db.Offers.Add(offerSetup);
                db.SaveChanges();
            }

            ViewBag.OfferId = offerSetup.OfferId;
            return View(new OfferEntry()
            {
                OfferEntryId = 0,
                OfferId = offerSetup.OfferId
            });
        }

        public JsonResult GetOfferEntries(int year)
        {
            var entries = (from o in db.Offers
                            join oe in db.OfferEntries on o.OfferId equals oe.OfferId
                            join u in db.Users on oe.CreatedBy equals u.Id
                            where o.Year == year
                            select new
                            {
                                oe.OfferEntryId,
                                oe.Name,
                                oe.Amount,
                                oe.Date,
                                oe.Notes,
                                u.UserName
                            }).ToList();

            return Json(entries, JsonRequestBehavior.AllowGet);
        }

        // POST: Campers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OfferEntry offerentry)
        {
            if (ModelState.IsValid)
            {
                offerentry.CreatedDate = DateTime.Now;
                offerentry.CreatedBy = User.Identity.GetUserId();
                db.OfferEntries.Add(offerentry);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View("Manage", offerentry);
        }
    }
}