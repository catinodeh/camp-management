using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public decimal? Amount { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreadtedDate { get; set; }
    }
}
