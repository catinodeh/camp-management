using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    [Table("AspNetUsers")]
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}
