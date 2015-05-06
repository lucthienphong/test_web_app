using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net.Mail;
using System.IO;
using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.Core
{
    public class CommonHelper
    {
        public static bool IsValidEmail(string Email)
        {
            return Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// Gets query string value by name
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static string QueryString(string Name)
        {
            string result = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString[Name] != null)
                result = HttpContext.Current.Request.QueryString[Name].ToString();
            return result;
        }

        /// <summary>
        /// Gets boolean value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static bool QueryStringBool(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            return (resultStr == "YES" || resultStr == "TRUE" || resultStr == "1");
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            int result = -1;
            Int32.TryParse(resultStr, out result);
            return result;
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string queryName, int defaultValue)
        {
            string resultStr = QueryString(queryName).ToUpperInvariant();
            if (resultStr.Length > 0)
            {
                return Int32.Parse(resultStr);
            }
            return defaultValue;
        }

        public static byte QueryStringByte(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            byte result = byte.MaxValue;
            if (!string.IsNullOrEmpty(resultStr))
                result = byte.Parse(resultStr);
            return result;
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static long QueryStringLong(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            long result;
            Int64.TryParse(resultStr, out result);
            return result;
        }

        /// <summary>
        /// Gets GUID value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static Guid? QueryStringGUID(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            Guid? result = null;
            try
            {
                result = new Guid(resultStr);
            }
            catch
            {
            }
            return result;
        }

        public static void SendSmtpMail(string emailSender, string emailFromAddress, string emailToAddress, string subject, string bodyMessage, bool isBodyHtml)
        {
            SendSmtpMail(emailSender, emailFromAddress, emailToAddress, subject, bodyMessage, isBodyHtml, string.Empty);
        }

        public static void SendSmtpMail(string emailSender, string emailFromAddress, string emailToAddress, string subject, string bodyMessage, bool isBodyHtml, string attachmentPath)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(SettingManager.GetSettingValue(SettingNames.SmtpSenderAccount), emailSender);

            //System.Net.NetworkCredential credential = new System.Net.NetworkCredential(
            //              emailFromAddress,
            //            fromPass);
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(
                            SettingManager.GetSettingValue(SettingNames.SmtpSenderAccount),
                            SettingManager.GetSettingValue(SettingNames.SmtpSenderPassword));
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = SettingManager.GetSettingValueBoolean(SettingNames.SmtpUsingSSL);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = credential;
            smtpClient.Host = SettingManager.GetSettingValue(SettingNames.SmtpMailServerAddress);
            smtpClient.Port = SettingManager.GetSettingValueInt(SettingNames.SmtpPort, 25); //587

            message.From = fromAddress;
            //if (emailToAddress != null)
            //foreach (string mailto in emailToAddress)
            //        message.To.Add(mailto);
            message.To.Add(emailToAddress);
            message.ReplyTo = new MailAddress(emailFromAddress, emailSender);
            message.Subject = subject;
            message.IsBodyHtml = isBodyHtml;
            message.Body = bodyMessage;

            //attach file
            bool isExists = false;
            if (!string.IsNullOrEmpty(attachmentPath))
            {
                isExists = File.Exists(attachmentPath);
                if (isExists)
                {
                    Attachment attach = new Attachment(attachmentPath);
                    message.Attachments.Add(attach);
                }
            }

            // Send SMTP mail
            smtpClient.Send(message);

            //delete file if it exists
            if (isExists)
            {
                message.Dispose();
                if (!FileIsOpen(attachmentPath))
                    File.Delete(attachmentPath);
            }
        }

        /// <summary>
        /// Check file is opened, return true if file is opened 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool FileIsOpen(string filePath)
        {
            bool results = false;
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    try
                    {
                        stream.ReadByte();
                    }
                    catch (IOException)
                    {
                        results = true;
                    }
                    finally
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
            catch (IOException)
            {
                results = true;  //file is opened at another location
            }

            return results;
        }

        /// <summary>
        /// Status of object in Project
        /// </summary>
        /// <param name="objStatus"></param>
        /// <returns></returns>

        public static string GetFullApplicationPath()
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        }
    }

    public class ProduceProcessingStatus
    {
        public const string Kinh_Doanh_Chuyen = "Kinh_Doanh_Chuyển";
        public const string Da_Giao_Viec = "Đã_Giao_Việc";
        public const string Dang_Thuc_Hien = "Đang_Thực_Hiện";
        public const string Da_Hoan_Thanh = "Đã_Hoàn_Thành";
        public const string Huy = "Hủy";
    }

    public class GraphicProduceType
    {
        public const string Do_Hoa_Lam_Mau = "Đồ_Họa_Làm_Mẫu";
        public const string Do_Hoa_Chep_Khac = "Đồ_Họa_Chép_Khắc";
    }

    public class DeptCode {
        public const string Do_Hoa_Lam_Mau = "DHLM";
        public const string Do_Hoa_Chep_Khac = "DHCK";
    }
}
