using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampManagement.Domain.Entities.Dashboard;

namespace CampManagement.Web.Models
{
    public class DashboardViewModel
    {
        public List<PaymentHistory> PaymentHistories { get; set; }
        public List<CampSummary> CampSummaries { get; set; }
    }
}