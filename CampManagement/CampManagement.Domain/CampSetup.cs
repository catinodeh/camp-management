using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain
{
    public class CampSetup
    {
        public int CampSetupId { get; set; }
        public int Year { get; set; }
        public System.DateTime StartDay { get; set; }
        public System.DateTime EndDay { get; set; }
        public decimal CurrentPrice { get; set; }
        public int FromGrade { get; set; }
        public int ToGrade { get; set; }
        public int Spaces { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
