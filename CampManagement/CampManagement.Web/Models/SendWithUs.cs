using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace CampManagement.Web.Models
{
    public class SendWithUs
    {
        public static string TEMPLATE_NewGuardian = ConfigurationManager.AppSettings["sendwithus_newguardian"];
        public static string TEMPLATE_Registration = ConfigurationManager.AppSettings["sendwithus_registration"];

        public static void Send(string templateid, string recipientName, string recipientEmail, object templateData)
        {
            string api_key = ConfigurationManager.AppSettings["sendwithus_key"];

            if (ConfigurationManager.AppSettings["debug"] != null)
            {
                recipientName = "Thiago";
                recipientEmail = "thiagodasilva@gmail.com";
            }

            // Let's create a request
            string url = ConfigurationManager.AppSettings["sendwithus_url"];
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            // set headers with api key
            WebHeaderCollection headers = httpWebRequest.Headers;
            headers.Add("Authorization", "Basic " + api_key);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    template = templateid,
                    recipient = new {
                        name = recipientName,
                        address = recipientEmail
                    },
                    template_data = templateData,
                    sender = new {
                        name = ConfigurationManager.AppSettings["sendwithus_name"],
                        address = ConfigurationManager.AppSettings["sendwithus_email"]
                    }
                });

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    // 3) Sample response (JSON)
                    /*
                    {
                        "success" : true,
                        "status" : "OK",
                        "receipt_id" : "unique-receipt-id",
                        "email" : {
                            "name" : "Welcome New User!",
                            "version_name" : "My First Version"
                        }
                    }
                    */
                }
            }
        }
    }
}