using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampManagement.Web.Models
{
    public class DefaultJsonReturn
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}