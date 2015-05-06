using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.MasterPages
{
    public partial class MainMasterPage : System.Web.UI.MasterPage
    {
        private int IsOpen
        {
            get
            {
                int mIsOpen = 0;
                if (Request.QueryString["IsOpen"] != null)
                    int.TryParse(Request.QueryString["IsOpen"].ToString(), out mIsOpen);

                return mIsOpen;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsOpen == 1)
                ltrIsOpen.Text = "<input type='hidden' id='hdfIsOpen' />";

            if (!IsPostBack)
            {
                //Thông tin bản quyền SweetSoft
                string swWebsite = ((AssemblyCompanyWebsite)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyWebsite), false)).Conflicts;
                string swCompanyName = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
                linkSweetSoft.Target = "_blank";
                linkSweetSoft.NavigateUrl = swWebsite;
                linkSweetSoft.Text = string.Format("Designed and Developed by {0}", swCompanyName);

                //user dang nhap
                TblUser obj = UserManager.GetUserByUserName(HttpContext.Current.User.Identity.Name);//ApplicationContext.Current.UserName);
                if (obj != null)
                {
                    ApplicationContext.Current.UserName = obj.UserName;
                    ApplicationContext.Current.UserID = obj.UserID;
                    ApplicationContext.Current.CurrentUserIp = Request.UserHostAddress;
                    lbUserName.Text = obj.DisplayName;
                    //ListMenuByUser(obj.UserName);
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            ApplicationContext.Current.ClearSession("CurrentUser");
            ApplicationContext.Unload();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Pages/Login.aspx");
        }
    }
}