using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    public class Registration
    {
        [Key]
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
