using System;
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
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(14)]
        public string Phone { get; set; }
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
