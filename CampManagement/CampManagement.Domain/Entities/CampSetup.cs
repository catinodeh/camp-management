using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("CampSetup")]
    public class CampSetup : IValidatableObject
    {
        [Key]
        public int CampSetupId { get; set; }
        public int CampId { get; set; }
        public int Year { get; set; }
        [Required]
        [DisplayName("Start Day")]
        public System.DateTime StartDay { get; set; }
        [Required]
        [DisplayName("End Day")]
        public System.DateTime EndDay { get; set; }
        [Range((double) 1.0, 1000, ErrorMessage = "Price must be between $1 and $1000")]
        [DisplayName("Current Price")]
        public decimal CurrentPrice { get; set; }
        [Range(1,12)]
        [DisplayName("From Grade")]
        public int FromGrade { get; set; }
        [Range(1, 12)]
        [DisplayName("To Grade")]
        public int ToGrade { get; set; }
        [Range(1, 1000)]
        public int Spaces { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual List<ExtraActivity> ExtraActivities { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDay > EndDay)
                yield return new ValidationResult("Start Date must be before End Date");

            if (StartDay == default(DateTime))
                yield return new ValidationResult("Start Date is required");

            if (EndDay == default(DateTime))
                yield return new ValidationResult("End Date is required");
        }
    }
}
