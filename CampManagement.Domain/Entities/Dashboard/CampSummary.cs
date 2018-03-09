using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities.Dashboard
{
    public class CampSummary
    {
        public int CampId { get; set; }
        public int CampSetupId { get; set; }
        public string Name { get; set; }
        public int FromGrade { get; set; }
        public int ToGrade { get; set; }
        public int TotalSpaces { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalConfirmed { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalAmount { get; set; }
        public int Total1Payment { get; set; }
        public int Total2Payment { get; set; }
        public int Total3Payment { get; set; }
        public int Total4Payment { get; set; }
        public int QtyPaidFull { get; set; }
        public decimal TotalPaidFull { get; set; }
        public int QtyHasBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public int QtyNoPayment { get; set; }
    }
}
