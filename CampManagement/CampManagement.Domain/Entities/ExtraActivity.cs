using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("ExtraActivity")]
    public  class ExtraActivity
    {
        [Key]
        public int ActivityId { get; set; }
        public int CampSetupId { get; set; }
        [Required]
        [StringLength(5)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}
