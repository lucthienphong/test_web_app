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

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class Login : ModalBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool Authenticate(string userName, string passWord)
        {
            return Membership.ValidateUser(userName, passWord);
        }

        protected void btnLogin_ServerClick(object sender, EventArgs e)
        {
            AppCache.Clear();
            //Nếu 
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                //messageBox.Visible = true;\
                lbMessage.Visible = true;
                lbMessage.Text = "*" + ResourceTextManager.GetApplicationText(ResourceText.INVALID_LOGIN);
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                //messageBox.Visible = true;
                lbMessage.Visible = true;
                lbMessage.Text = "*" + ResourceTextManager.GetApplicationText(ResourceText.INVALID_LOGIN);
                return;
            }
            if (Authenticate(txtUserName.Text, txtPassword.Text))
            {
                //Kiểm tra người dùng có trong tblUser không
                TblUser loginUser = UserManager.GetUserByUserName(txtUserName.Text.Trim());
                if (loginUser != null)
                {
                    if (this.Page != null)
                    {
                        ModalBasePage page = this.Page as ModalBasePage;
                        if (page != null)
                            page.CheckFunctionPermission(loginUser.UserName);
                    }
                    Session["DisplayName"] = loginUser.DisplayName;
                    //Lưu tên đăng nhập vào hệ thống
                    ApplicationContext.Current.UserName = loginUser.UserName;
                    ApplicationContext.Current.UserID = loginUser.UserID;
                    ApplicationContext.Current.CurrentUserIp = Request.UserHostAddress;
                    FormsAuthentication.SetAuthCookie(loginUser.UserName, checkRemember.Checked);//false);
                    FormsAuthentication.RedirectFromLoginPage(loginUser.UserName, checkRemember.Checked);//, false);

                    //Lưu vào lịch sử hệ thống
                    if (AllowSaveLogging)
                        LoggingManager.LogAction(loginUser.UserName, ActivityLoggingHelper.LOGIN, "Login succeefull");
                }
                else
                {
                    FormsAuthentication.SignOut();
                }
            }
            else
            {
                //messageBox.Visible = true;
                lbMessage.Visible = true;
                lbMessage.Text = "*" + ResourceTextManager.GetApplicationText(ResourceText.INVALID_LOGIN);
                //Lưu vào lịch sử hệ thống
                if (AllowSaveLogging)
                {
                    LoggingManager.LogAction(txtUserName.Text.Trim(), Request.UserHostAddress, ActivityLoggingHelper.LOGIN, "", string.Format("{0} Login failed", txtUserName.Text.Trim()));
                }
            }
        }
    }
}