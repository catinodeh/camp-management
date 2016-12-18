using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("OfferEntry")]
    public class OfferEntry : IValidatableObject
    {
        [Key]
        public int OfferEntryId { get; set; }
        public int OfferId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Notes { get; set; }
        [Range(1, 5000)]
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date.Year != DateTime.Now.Year && Date != default(DateTime))
                yield return new ValidationResult("Donation's date should be in " + DateTime.Now.Year);
        }
    }

    [Table("Offer")]
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public int Year { get; set; }
        public decimal Balance { get; set; }
    }
}
