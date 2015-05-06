using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class CurrencyChangedLog : ModalBasePage
    {
        private string CurrentID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    return Request.QueryString["ID"];
                }
                return string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblCurrencyName.Text = ResourceTextManager.GetApplicationText(ResourceText.HISTORYLOGGING);

                //gvCurrency.PageIndex = 0;
                //gvCurrency.PageSize = 5;
                SortType = "D";
                gvCurrency.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        public override void BindData()
        {
            int currencyID = -1;
            if (int.TryParse(CurrentID, out currencyID))
            {
                DateTime? searchDate = (DateTime?)null;
                DateTime _searchDate = new DateTime();
                if (DateTime.TryParseExact(txtSearchDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _searchDate))
                {
                    searchDate = _searchDate;
                }
                DataTable cCol = new CurrencyManager().SelectCurrencyLogByCurrencyID(currencyID, searchDate, CurrentPageIndex, gvCurrency.PageSize, SortColumn, SortType);
                //if (cCol != null)
                //{
                //    gvCurrency.VirtualItemCount = (int)cCol.Rows[0]["RowsCount"];
                //    gvCurrency.DataSource = cCol;
                    
                //    gvCurrency.DataBind();
                //    gvCurrency.PageIndex = CurrentPageIndex;
                //}


                if (cCol.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    int totalRows = 0;
                    if (cCol.Rows.Count > 0)
                        totalRows = (int)cCol.Rows[0]["RowsCount"];
                    gvCurrency.VirtualItemCount = totalRows;
                    gvCurrency.DataSource = cCol;

                    gvCurrency.DataBind();
                    gvCurrency.PageIndex = CurrentPageIndex;
                }
            }

        }

        protected void gvCurrency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            gvCurrency.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void gvCurrency_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortType == "A")
            {
                SortType = "D";
            }
            else
            {
                SortType = "A";
            }

            int oldColumnIndex = SortColumn == null ? 0 : int.Parse(SortColumn);
            SortColumn = e.SortExpression;
            int columnIndex = int.Parse(e.SortExpression);
            gvCurrency.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvCurrency.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}