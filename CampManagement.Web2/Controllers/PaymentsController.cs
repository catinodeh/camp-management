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
    public class PaymentsController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        // GET: Payments
        public ActionResult Manage(int? id = null)
        {
            if (id.HasValue)
            {
                var reg = new RegistrationsController().GetById(id.Value);
                return View("Manage", reg);
            }

            return View();
        }

        // GET: Payments/Edit/5
        [HttpPost]
        public ActionResult Manage(int id, RegistrationPayment model)
        {
            var reg = new RegistrationsController().GetById(model.RegistrationId);

            if (ModelState.IsValid)
            {
                var regAmount = reg.GetTotal;
                var amountPaid =
                    db.RegistrationPayments.Where(p => p.RegistrationId == model.RegistrationId)
                        .ToList()
                        .Sum(p => p.Amount);

                if (amountPaid + model.Amount > regAmount)
                {
                    ModelState.AddModelError("",
                        $"Payment cannot exceed the Registration balance of {regAmount - amountPaid:C}");
                }
                else
                {
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.GetUserId();
                    db.RegistrationPayments.Add(model);
                    db.SaveChanges();
                    return View("Manage", reg);
                }
            }
            else
            {
                if (ModelState.Any(m => m.Value.Errors.Count > 0))
                {
                    var errorMessage = ModelState.FirstOrDefault(e => e.Value.Errors.Count > 0).Value;
                    ModelState.AddModelError("", errorMessage.Errors[0].ErrorMessage);
                }
            }

            TempData["Date"] = model.Date == default(DateTime) ? null : model.Date.ToString("dd/MM/yyyy");
            TempData["Amount"] = model.Amount == 0 ? null : model.Amount.ToString();
            TempData["Notes"] = model.Notes;
            return View(reg);
        }

        [HttpPost]
        public JsonResult DeletePayment(int id, int paymentId)
        {
            var payment = db.RegistrationPayments.FirstOrDefault(p => p.PaymentId == paymentId);
            if (payment != null)
            {
                db.RegistrationPayments.Remove(payment);
                db.SaveChanges();
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult GetPayments(int id)
        {
            var payments = (from p in db.RegistrationPayments
                            join u in db.Users on p.CreatedBy equals u.Id
                            where p.RegistrationId == id
                            select new
                            {
                                p.PaymentId,
                                p.PaymentTypeId,
                                p.CreatedDate,
                                p.CreatedBy,
                                u.UserName,
                                p.Amount,
                                p.Date,
                                p.Notes
                            }).ToList();

            return Json(payments);
        }
    }
}
