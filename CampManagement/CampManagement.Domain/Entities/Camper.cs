using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampManagement.Domain.Entities
{
    [Table("Camper")]
    public class Camper
    {
        [Key]
        public int CamperId { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(14)]
        public string Phone { get; set; }
        [StringLength(300)]
        [DisplayName("Cabin Mates")]
        public string CabinMates { get; set; }
        [StringLength(500)]
        [DisplayName("Physical Limitations")]
        public string Limitations { get; set; }
        public bool Active { get; set; }
        [RegularExpression(@"^(M|F)$")]
        [StringLength(1)]
        [Required]
        public string Gender { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
