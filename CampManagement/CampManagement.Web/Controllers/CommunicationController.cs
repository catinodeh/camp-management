using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampManagement.Data;
using CampManagement.Domain.Entities;
using CampManagement.Domain.Enum;
using CampManagement.Domain.Models;
using CampManagement.Web.Helpers;
using CampManagement.Web.Models;

namespace CampManagement.Web.Controllers
{
    [Authorize]
    public class CommunicationController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        public ActionResult AllGuardians()
        {
            return View(db.Guardians.Where(g => g.Active && g.Email != null && g.Email.Contains("@")).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Send(EmailModel model)
        {
            List<string> emails = new List<string>(); 
            if (model.Body != null)
            {
                foreach (var guardianId in model.Ids.Split(',').Select(n => Convert.ToInt32(n)))
                {
                    var guardian = db.GuardianEmails.FirstOrDefault(g => g.GuardianId == guardianId);
                    if (guardian != null && !emails.Contains(guardian.Email))
                    {
                        SendWithUs.Send(model.Template, guardian.Name, guardian.Email, new
                        {
                            guardian,
                            pic_id = new Random().Next(1, 15),
                            text = model.Body.ReflectionReplace<GuardianEmail>(guardian),
                            url = $"{Request.Url.ToString().Replace(Request.Url.LocalPath, "") + Url.Action("ConfirmEmail", "Guardians")}/{guardian.RowGuid}"
                        },
                        ConfigurationManager.AppSettings["sendwithus_cc"]);

                        emails.Add(guardian.Email);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}