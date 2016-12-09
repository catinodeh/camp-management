using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain
{
    public class Offer
    {
        public int OfferId { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public decimal? Amount { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreadtedDate { get; set; }
    }
}
