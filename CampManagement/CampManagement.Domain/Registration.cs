using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public string Notes { get; set; }
        public bool GuardianEmailConfirmed { get; set; }
        public decimal? CampAmount { get; set; }
        public decimal? ExtraAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Balance { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
