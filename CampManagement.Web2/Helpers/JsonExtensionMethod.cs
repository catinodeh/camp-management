using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampManagement.Web.Helpers
{
    public static class JsonExtensionMethod
    {
        public static string ToJson(this object o)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return jsonSerializer.Serialize(o);
        }

        public static T FromJson<T>(this string s)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return serializer.Deserialize<T>(s);
        }
    }
}