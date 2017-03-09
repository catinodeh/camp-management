using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManagement.Domain.Models
{
    public class EmailModel
    {
        public string Template { get; set; }
        public string Ids { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public object ExtraParameters { get; set; }
    }
}
