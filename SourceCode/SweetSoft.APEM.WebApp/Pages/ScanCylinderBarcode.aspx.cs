using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class ScanCylinderBarcode : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "cylinder_scanning_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = 0; i < 50; i++)
                {
                    ddlNumberItem.Items.Add(new ListItem((i + 1).ToString()));
                }
                ddlNumberItem.Items.Insert(0, new ListItem("Select number item:", ""));
            }
        }
    }
}