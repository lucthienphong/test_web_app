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
    public partial class ForgotPassword : ModalBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            GetControlResourceText();
            if (!IsPostBack)
            {
                SetVerificationText();
            }
        }

        private void GetControlResourceText()
        {
            requireValidName.ErrorMessage = "Please enter username";
            requireValidEmail.ErrorMessage = "Please enter email";
            validEmailRegExp.ErrorMessage = "Email is not valid";
            btnRefreshValidCode.ToolTip = "Change the security code";
            btnSubmit.Text = "Submit";
            btnClearText.Text = "Clear";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string stAlertResult = "Your request has been sent";
            bool validCode = CheckValidCode();
            try
            {
                if (validCode)
                {
                    TblUser currentUser = UserManager.GetUserByUserName(txtUserName.Text.Trim());
                    if (currentUser == null)
                        stAlertResult = "Username or email is not correct.";
                    else
                    {
                        if (currentUser.Email.CompareTo(txtEmail.Text.Trim()) != 0)
                            stAlertResult = "Username or email is not correct.";
                        else
                        {
                           
                            string emailSender = "APE MALAYSIA";
                            string emailFromAddress = SettingManager.GetSettingValue(SettingNames.SmtpSenderAccount);
                            string emailToAddress = currentUser.Email;
                            string subject = "Recover password";
                            // Message body content
                            StringBuilder strMessage = new StringBuilder();
                            strMessage.AppendLine("Requirements change password");
                            strMessage.AppendLine("<br />");
                            strMessage.AppendLine(string.Format("Username {0}", txtUserName.Text));
                            strMessage.AppendLine("<br />");
                            strMessage.AppendLine(string.Format("<a href='{0}/ResetPassword.aspx?rk={1}'>{2}</a>", HttpContext.Current.Request.Url.Host, SecurityHelper.Encrypt(currentUser.UserName), "Please click the following address to retrieve your password: "));
                            strMessage.AppendLine("<br />");
                            strMessage.AppendLine("<br />");
                            strMessage.AppendLine("This email is sent automatically from the system administrator.");
                            strMessage.AppendLine("<br />");
                            //Gửi email
                            CommonHelper.SendSmtpMail(emailSender, emailFromAddress, emailToAddress, subject, strMessage.ToString(), true);

                            if (AllowSaveLogging)
                                SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.FORGOT_PASSWORD), FUNCTION_PAGE, strMessage.ToString());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Response.Redirect("/Pages/Login.aspx");
                ProcessException(ex);
            }
            finally
            {
                if (validCode)
                {
                    ShowInforMessage(stAlertResult);
                    ClearForm();
                }
            }
        }

        private Random random = new Random();
        private bool CheckValidCode()
        {
            // On a postback, check the user input.
            if (Session["Captcha"] != null &&
                 txtValidCode.Text != string.Empty &&
                 txtValidCode.Text == Session["Captcha"].ToString())
            {
                SetVerificationText();
                lbError.Text = string.Empty;
                return true;
            }
            else
            {
                SetVerificationText();
                lbError.Text = "Security code is not correct.";
                return false;
            }
        }

        private string GenerateRandomCode()
        {
            string s = "";
            for (int i = 0; i < 4; i++)
                s = String.Concat(s, this.random.Next(10).ToString());
            return s;
        }
        protected void ChangeCaptchaImage(object sender, EventArgs e)
        {
            SetVerificationText();
        }
        public void SetVerificationText()
        {
            Session["Captcha"] = GenerateRandomCode();
            imCaptcha.ImageUrl = "Captcha.ashx?" + Guid.NewGuid().ToString();
        }

        private void ShowInforMessage(string alert)
        {
            string script = @"alert('" + alert + "'); document.location = 'Pages/Login.aspx';";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "jsResultMessage", script, true);
        }

        protected void btnClearText_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtUserName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtValidCode.Text = string.Empty;
        }
    }
}