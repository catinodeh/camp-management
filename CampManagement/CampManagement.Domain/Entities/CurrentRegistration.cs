using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("CurrentRegistration")]
    public class CurrentRegistration
    {
        [Key]
        public int RegistrationCamperId { get; set; }
        public int RegistrationId { get; set; }
        public int CampId { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public int Spaces { get; set; }
        public int CamperId { get; set; }
        public string CamperName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int? GuardianId { get; set; }
        public string GuardianName { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string Phone { get; set; }
        public bool? Cancelled { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelFinalized { get; set; }
        public int Grade { get; set; }
        public decimal Price { get; set; }
        public bool? Registered { get; set; }
        public decimal? TotalRegistration { get; set; }
        public decimal? TotalPayments { get; set; }
    }
}
