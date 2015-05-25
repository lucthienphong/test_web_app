using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Logs;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class AllLogs : ModalBasePage
    {
        public override string FUNCTION_PAGE
        {
            get
            {
                return "logs_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                grvLogs.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvLogs.Columns[1].HeaderText = "No";
            grvLogs.Columns[2].HeaderText = "Date Action";
            grvLogs.Columns[3].HeaderText = "User Name";
            grvLogs.Columns[4].HeaderText = "User IP";
            grvLogs.Columns[5].HeaderText = "Action";
        }

        private void BindData()
        {
            DateTime? FromDate = (DateTime?)null;
            DateTime? ToDate = (DateTime?)null;
            DateTime _fromDate = new DateTime();
            DateTime _toDate = new DateTime();
            if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate))
                FromDate = _fromDate;
            if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate))
                ToDate = _toDate;

            List<DataLogsViewObject> allLogsView = DataLogsManager.SelectDataLogs(grvLogs.PageIndex, grvLogs.PageSize, FromDate, ToDate);

            grvLogs.VirtualItemCount = allLogsView.Count > 0 ? allLogsView[0].TotalRow : 0;
            grvLogs.DataSource = allLogsView;
            grvLogs.DataBind();
            grvLogs.PageIndex = CurrentPageIndex;

        }

        protected void grvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvLogs.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

    }
}