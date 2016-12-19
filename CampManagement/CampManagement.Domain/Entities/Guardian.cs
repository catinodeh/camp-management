using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("Guardian")]
    public class Guardian
    {
        [Key]
        public int GuardianId { get; set; }
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        [StringLength(14)]
        [Required]
        public string Phone { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(100)]
        public string Address1 { get; set; }
        [StringLength(50)]
        public string Address2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(5)]
        public string Zip { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Guid RowGuid { get; set; }
    }
}
