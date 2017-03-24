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

        public ActionResult RegisteredGuardians()
        {
            return View(db.RegisteredGuardianEmails.Where(g => g.Email != null && g.Email.Contains("@")).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Send(EmailModel model)
        {
            List<string> emails = new List<string>(); 
            if (!string.IsNullOrEmpty(model.Body))
            {
                if (model.Method == 1)
                {
                    int[] GuardianIds = model.Ids.Split(',').Select(g => Convert.ToInt32(g)).ToArray();
                    //BCC all guardians..
                    SendWithUs.Send(model.Template, "New Life Church", "media@newlifeus.com", new
                    {
                        pic_id = new Random().Next(1, 15),
                        text = model.Body
                    },
                    null,
                    db.Guardians.Where(g => GuardianIds.Contains(g.GuardianId)).Select(g => g.Email).Distinct().ToArray());
                }

                if (model.Method == 2)
                {
                    //One email per guardian
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
                            new string[] { ConfigurationManager.AppSettings["sendwithus_cc"] });

                            emails.Add(guardian.Email);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}