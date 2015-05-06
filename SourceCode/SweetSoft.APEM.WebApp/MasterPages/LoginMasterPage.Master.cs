using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.MasterPages
{
    public partial class LoginMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool HasLoggedIn = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                if (HasLoggedIn)
                    Response.Redirect("~/Pages/Main.aspx");
            }
        }
    }
}