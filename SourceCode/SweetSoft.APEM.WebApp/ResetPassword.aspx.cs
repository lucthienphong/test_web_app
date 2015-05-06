using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class ResetPassword : ModalBasePage
    {
        private string ResetKey
        {
            get
            {
                return CommonHelper.QueryString("rk");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(ResetKey))
                    DoReset();
                else
                    Response.Redirect("/Pages/Login.aspx");
            }
        }

        private void DoReset()
        {
            string stAlertResult = string.Empty;
            try
            {
                string username = SecurityHelper.Decrypt(ResetKey);
                TblUser currentUser = UserManager.GetUserByUserName(username);
                if (currentUser == null)
                    stAlertResult = "Unable to recover passwords";
                else
                {
                    lblMessage.Text = string.Format("{0}: {1}", "Username", currentUser.UserName);

                    string emailSender = "APE MALAYSIA";
                    string emailFromAddress = SettingManager.GetSettingValue(SettingNames.SmtpSenderAccount);
                    string emailToAddress = currentUser.Email;
                    string subject = "Recover password";
                    // Message body content
                    StringBuilder strMessage = new StringBuilder();
                    strMessage.AppendLine("Account information and your password on the system is:");
                    strMessage.AppendLine("<br />");
                    strMessage.AppendLine(string.Format("Username: {0}", currentUser.UserName));
                    strMessage.AppendLine(string.Format("Password: {0}", UserManager.ResetPassword(currentUser.UserName)));
                    strMessage.AppendLine("<br />");
                    strMessage.AppendLine(string.Format("<a href='{0}/Pages/Main.aspx?IsOpen=1'>{1}</a>", HttpContext.Current.Request.Url.Host, "Đi đến link sau để thay đổi mật khẩu: "));
                    strMessage.AppendLine("<br />");
                    strMessage.AppendLine("<br />");
                    strMessage.AppendLine("This email is sent automatically from the system administrator.");
                    strMessage.AppendLine("<br />");

                    //Gửi email
                    CommonHelper.SendSmtpMail(emailSender, emailFromAddress, emailToAddress, subject, strMessage.ToString(), true);
                    stAlertResult = "A new password has been sent to your email";

                    if (AllowSaveLogging)
                        SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.RESET_PASSWORD), FUNCTION_PAGE, strMessage.ToString());
                }
            }
            catch
            {
                Response.Redirect("/Pages/Login.aspx");
            }
            finally
            {
                ShowInforMessage(stAlertResult);
            }
        }

        private void ShowInforMessage(string alert)
        {
            string script = @"alert('" + alert + "');" + " document.location = '/Pages/Login.aspx';";
            Page.ClientScript.RegisterStartupScript(typeof(string), "jsResultMessage", script, true);
        }
    }
}