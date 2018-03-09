using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("RegistrationCamperExtraActivity")]
    public class RegistrationCamperExtraActivity
    {
        [Key]
        public int ExtraActivityId { get; set; }
        public int RegistrationCamperId { get; set; }
        public int ActivityId { get; set; }
        public decimal Price { get; set; }
    }
}
