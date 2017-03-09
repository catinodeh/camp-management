using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampManagement.Web.Helpers
{
    public static class StringExtensions
    {
        public static string ReflectionReplace<T>(this string template, T obj)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var stringToReplace = "@" + property.Name;
                var value = property.GetValue(obj);
                if (value == null) value = "";
                template = template.Replace(stringToReplace, value.ToString());
            }
            return template;
        }
    }
}