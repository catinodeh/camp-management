using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain
{
    public class RegistrationCamper
    {
        public int RegistrationCamperId { get; set; }
        public int Grade { get; set; }
        public bool? Registered { get; set; }
        public string RegisteredBy { get; set; }
        public bool? Cancelled { get; set; }
        public string CancelledBy { get; set; }
        public string CancelNotes { get; set; }
        public DateTime? CancelFinalized { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}
