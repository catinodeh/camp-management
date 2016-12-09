using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain
{
    public class RegistrationPayment
    {
        public int PaymentId { get; set; }
        public int RegistrationId { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
