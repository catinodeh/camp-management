using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampManagement.Data;
using CampManagement.Domain.Entities.Dashboard;
using CampManagement.Web.Models;

namespace CampManagement.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private CampManagementDbContext db = new CampManagementDbContext();

        public ActionResult Index()
        {
            var payments = db.Database.SqlQuery<PaymentHistory>("EXEC dbo.usp_LastPayments").ToList();
            var campSummaries = db.Database.SqlQuery<CampSummary>("EXEC dbo.usp_CampDashboard").ToList();

            return View(new DashboardViewModel()
            {
                PaymentHistories = payments,
                CampSummaries = campSummaries.OrderBy(c => c.FromGrade).ToList()
            });
        }
    }
}