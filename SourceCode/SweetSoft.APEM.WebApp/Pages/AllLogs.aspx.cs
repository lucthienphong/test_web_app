using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Logs;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Logs.DataAccess;
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

        #region Bind Data

        private void BindData()
        {
            try
            {
                DateTime? FromDate = (DateTime?)null;
                DateTime? ToDate = (DateTime?)null;
                DateTime _fromDate = new DateTime();
                DateTime _toDate = new DateTime();
                if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate))
                    FromDate = _fromDate;
                if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate))
                    ToDate = _toDate;

                string UserName = txtUserName.Text;
                string UserIP = txtUserIP.Text;
                string Action = txtAction.Text;
                string ObjectName = txtObjectName.Text;

                Sort sort = new Sort()
                                    {
                                        ColIndex = int.Parse(SortColumn) > 0 ? int.Parse(SortColumn) : 1,
                                        Type = (int.Parse(SortColumn) == 1 || int.Parse(SortColumn) == 0 ? NumSortType.DESC : (SortType == "A" ? NumSortType.ASC : NumSortType.DESC))
                                    };

                if (string.IsNullOrEmpty(UserName) &&
                    string.IsNullOrEmpty(UserIP) &&
                    string.IsNullOrEmpty(Action) &&
                    string.IsNullOrEmpty(ObjectName) &&
                    FromDate == null &&
                    ToDate == null
                    )
                {
                    FromDate = DateTime.Now;
                    ToDate = DateTime.Now;
                }

                List<DataLogsViewObject> allLogsView = DataLogsManager.SelectDataLogs(grvLogs.PageIndex,
                                                                                      grvLogs.PageSize,
                                                                                      FromDate,
                                                                                      ToDate,
                                                                                      UserName,
                                                                                      UserIP,
                                                                                      Action,
                                                                                      ObjectName,
                                                                                      sort
                                                                                      );
                if (allLogsView.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    int TotalRows = allLogsView.Count > 0 ? allLogsView[0].TotalRow : 0;
                    grvLogs.VirtualItemCount = TotalRows;
                    grvLogs.DataSource = allLogsView;
                    grvLogs.DataBind();
                    grvLogs.PageIndex = CurrentPageIndex;

                    Session[ViewState["PageID"] + "AllLogs"] = allLogsView;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }

        }

        #endregion

        #region Web Methob

        [WebMethod]
        public static string GetUserNameData(string Keyword)
        {
            List<TblAllDataLog> result = new List<TblAllDataLog>();
            result = DataLogsManager.SelectUserNameByKeyWord(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        [WebMethod]
        public static string GetUserIPData(string Keyword)
        {
            List<TblAllDataLog> result = new List<TblAllDataLog>();
            result = DataLogsManager.SelectUserIPByKeyWord(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        [WebMethod]
        public static string GetActionData(string Keyword)
        {
            List<TblAllDataLog> result = new List<TblAllDataLog>();
            result = DataLogsManager.SelectActionByKeyWord(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        [WebMethod]
        public static string GetObjectData(string Keyword)
        {
            List<TblAllDataLog> result = new List<TblAllDataLog>();
            result = DataLogsManager.SelectObjectByKeyWord(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        #endregion

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

        protected void grvLogs_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortType == "A")
            {
                SortType = "D";
            }
            else
            {
                SortType = "A";
            }
            int oldColumnIndex = SortColumn == null ? 1 : int.Parse(SortColumn);
            SortColumn = e.SortExpression;
            int columnIndex = int.Parse(e.SortExpression);
            grvLogs.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvLogs.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

    }
}