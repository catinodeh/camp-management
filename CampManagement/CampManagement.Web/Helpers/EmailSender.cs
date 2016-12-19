using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using CampManagement.Web.Helpers;

namespace CampManagement.Web.Helpers
{
    public class EmailHelper
    {
        private MailMessage Message = null;
        private SmtpClient smtpClient = null;
        public MailAddress FromAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailHelper()
        {
            smtpClient = new SmtpClient();
            smtpClient.Host = ConfigurationManager.AppSettings["smtpHost"]; //Configure as your email provider
            smtpClient.UseDefaultCredentials = false;
            //smtpClient.EnableSsl = true;//comment if you don't need SSL 
            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["userName"], ConfigurationManager.AppSettings["password"]);
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            Message = new MailMessage();
        }
        public EmailHelper(string host, int port, string userName, string password, bool ssl)
        : this()
        {
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
            smtpClient.Credentials = new NetworkCredential(userName, password);
        }
        public void AddToAddress(string email, string name = null)
        {
            if (!string.IsNullOrEmpty(email))
            {
                email = email.Replace(",", ";");
                string[] emailList = email.Split(';');
                for (int i = 0; i < emailList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailList[i]))
                        Message.To.Add(new MailAddress(emailList[i], name));
                }
            }
        }
        public void AddCcAddress(string email, string name = null)
        {
            if (!string.IsNullOrEmpty(email))
            {
                email = email.Replace(",", ";");
                string[] emailList = email.Split(';');
                for (int i = 0; i < emailList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailList[i]))
                        Message.CC.Add(new MailAddress(emailList[i], name));
                }
            }
        }
        public void AddBccAddress(string email, string name = null)
        {
            if (!string.IsNullOrEmpty(email))
            {
                email = email.Replace(",", ";");
                string[] emailList = email.Split(';');
                for (int i = 0; i < emailList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailList[i]))
                        Message.Bcc.Add(new MailAddress(emailList[i], name));
                }
            }
        }
        public void AddAttachment(string file, string mimeType)
        {
            Attachment attachment = new Attachment(file, mimeType);
            Message.Attachments.Add(attachment);
        }
        public void AddAttachment(Attachment objAttachment)
        {
            Message.Attachments.Add(objAttachment);
        }
        public void SendMail()
        {
            if (FromAddress == null || (FromAddress != null && FromAddress.Address.Equals("")))
            {
                throw new Exception("From address not defined");
            }
            else
            {
                if (string.IsNullOrEmpty(FromAddress.DisplayName))
                    FromAddress = new MailAddress(FromAddress.Address, string.Empty);
                Message.From = FromAddress;
            }
            if (Message.To.Count <= 0)
            { throw new Exception("To address not defined"); }
            Message.Subject = Subject;
            Message.IsBodyHtml = true;
            Message.Body = Body;
            smtpClient.Send(Message);
        }
        public static string GetFileMimeType(string fileName)
        {
            string fileExt = Path.GetExtension(fileName.ToLower());
            string mimeType = string.Empty;
            switch (fileExt)
            {
                case ".htm":
                case ".html":
                    mimeType = "text/html";
                    break;
                case ".xml":
                    mimeType = "text/xml";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimeType = "image/jpeg";
                    break;
                case ".gif":
                    mimeType = "image/gif";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".bmp":
                    mimeType = "image/bmp";
                    break;
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".doc":
                    mimeType = "application/msword";
                    break;
                case ".docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    mimeType = "application/x-msexcel";
                    break;
                case ".xlsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".csv":
                    mimeType = "application/csv";
                    break;
                case ".ppt":
                    mimeType = "application/vnd.ms-powerpoint";
                    break;
                case ".pptx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case ".rar":
                    mimeType = "application/x-rar-compressed";
                    break;
                case ".zip":
                    mimeType = "application/x-zip-compressed";
                    break;
                default:
                    mimeType = "text/plain";
                    break;
            }
            return mimeType;
        }
    }
}


