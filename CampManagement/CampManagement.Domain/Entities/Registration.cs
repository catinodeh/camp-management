using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CampManagement.Domain.Entities
{
    [Table("Registration")]
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }
        public int? GuardianId { get; set; }
        public string Notes { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public decimal GetTotal
        {
            get
            {
                return Campers != null ? 
                    Campers.Sum(c => c.Price) + 
                        Campers.Where(c => c.ExtraActivities != null)
                        .Sum(c => c.ExtraActivities.Sum(ea => ea.Price)) : 0;
            }
        }

        public virtual Guardian Guardian { get; set; }
        public virtual List<RegistrationCamper> Campers { get; set; }
    }
}
