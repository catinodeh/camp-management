using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain
{
    public  class ExtraActivity
    {
        public int ActivityId { get; set; }
        public int CampSetupId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
