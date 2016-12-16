using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    public class Camper
    {
        [Key]
        public int CamperId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}
