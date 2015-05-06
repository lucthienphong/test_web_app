using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class ScanJobBarcode : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "job_scanning_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}