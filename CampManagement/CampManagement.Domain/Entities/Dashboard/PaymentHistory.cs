using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities.Dashboard
{
    public class PaymentHistory
    {
        public int CampId { get; set; }
        public int CampSetupId { get; set; }
        public string Name { get; set; }

        public decimal[] Dates => new[]
        {
            Date1, Date2, Date3, Date4, Date5, Date6, Date7, Date8, Date9, Date10
        };

        public decimal Date1 { get; set; }
        public decimal Date2 { get; set; }
        public decimal Date3 { get; set; }
        public decimal Date4 { get; set; }
        public decimal Date5 { get; set; }
        public decimal Date6 { get; set; }
        public decimal Date7 { get; set; }
        public decimal Date8 { get; set; }
        public decimal Date9 { get; set; }
        public decimal Date10 { get; set; }
    }
}
