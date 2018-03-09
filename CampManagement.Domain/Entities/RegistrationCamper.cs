using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("RegistrationCamper")]
    public class RegistrationCamper
    {
        [Key]
        public int RegistrationCamperId { get; set; }
        public int RegistrationId { get; set; }
        public int CampSetupId { get; set; }
        public int CamperId { get; set; }
        public int Grade { get; set; }
        public decimal Price { get; set; }
        public bool? Registered { get; set; }
        public string RegisteredBy { get; set; }
        public bool Cancelled { get; set; }
        public string CancelledBy { get; set; }
        public string CancelNotes { get; set; }
        public DateTime? CancelFinalized { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual CampSetup CampSetup { get; set; }
        public virtual Camper Camper { get; set; }
        public virtual List<RegistrationCamperExtraActivity> ExtraActivities { get; set; }
    }
}
