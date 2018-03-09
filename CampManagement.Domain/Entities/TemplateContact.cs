using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Entities
{
    public class TemplateContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid? RowGuid { get; set; }
    }
}
