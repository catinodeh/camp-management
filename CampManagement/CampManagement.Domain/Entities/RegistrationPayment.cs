using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    public class RegistrationPayment : IValidatableObject
    {
        [Key]
        public int PaymentId { get; set; }
        public int RegistrationId { get; set; }
        [Range(1, 10, ErrorMessage = "Please provide the payment type")]
        public int PaymentTypeId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        //[Range(1, Double.MaxValue, ErrorMessage = "Amount should be greater than $0")]
        public decimal Amount { get; set; }
        [StringLength(400)]
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date == default(DateTime))
            {
                yield return new ValidationResult("Please select a date");
            }
            else
            {
                if (Date.Year != DateTime.Now.Year)
                {
                    yield return new ValidationResult("Payments need to be in " + DateTime.Now.Year);
                }
            }

            if (Amount == 0)
                yield return new ValidationResult("Amount should be different than $0");
        }
    }
}
