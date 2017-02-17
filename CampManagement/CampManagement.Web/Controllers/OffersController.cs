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
    [Authorize]
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

        public JsonResult GetOfferAvailable(int year)
        {
            var totalOffer = (from o in db.Offers
                               join oe in db.OfferEntries on o.OfferId equals oe.OfferId
                               where o.Year == year
                               select oe.Amount)
                               .DefaultIfEmpty(0)
                               .Sum();

            var totalPaymentsOffer = (from p in db.RegistrationPayments
                            where p.Date.Year == DateTime.Now.Year && p.PaymentTypeId == 4
                            select p.Amount)
                            .DefaultIfEmpty(0)
                            .Sum();

            return Json(totalOffer - totalPaymentsOffer, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCampersUsingOffers(int year)
        {
            var entries = (from p in db.RegistrationPayments
                           where p.PaymentTypeId == 4 && p.Date.Year == DateTime.Now.Year
                           select new OfferUsed()
                           {
                               RegistrationId = p.RegistrationId,
                               Amount = p.Amount,
                               Date = p.Date
                           }).ToList();

            int[] registrationIds = entries.Select(r => r.RegistrationId).ToArray();

            var registrations = (from r in db.Registrations
                                join rc in db.RegistrationCampers on r.RegistrationId equals rc.RegistrationId
                                join c in db.Campers on rc.CamperId equals c.CamperId
                                where registrationIds.Contains(r.RegistrationId)
                                select new
                                {
                                    r.RegistrationId,
                                    c.FirstName,
                                    c.LastName
                                }).ToList();

            foreach (var entry in entries)
            {
                entry.CamperNames = string.Join(",",
                                    registrations.Where(r => r.RegistrationId == entry.RegistrationId)
                                        .Select(c => $"{c.FirstName} {c.LastName}")
                                        .ToArray());
            }

            return Json(entries, JsonRequestBehavior.AllowGet);
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