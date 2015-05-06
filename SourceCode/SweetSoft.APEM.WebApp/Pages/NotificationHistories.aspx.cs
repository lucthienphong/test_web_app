using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class NotificationHistories : ModalBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SortType = "D";
                SortColumn = "2";
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                grvPageList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvPageList.Columns[0].HeaderText = "Title";
            grvPageList.Columns[1].HeaderText = "Created by";
            grvPageList.Columns[2].HeaderText = "Created on";
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                TblNotificationCollection lst = RealtimeNotificationManager.GetNotificationForCurrentStaff(txtKeyword.Text.Trim(), null);
                if (lst != null && lst.Count > 0)
                {
                    DataTable dt = ToDataTable<TblNotification>(lst);
                    if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                    {
                        CurrentPageIndex -= 1;
                        BindData();
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)
                            totalRows = dt.Rows.Count;
                        grvPageList.VirtualItemCount = totalRows;

                        string sortType = SortType;

                        int oldColumnIndex = SortColumn == null ? 0 : int.Parse(SortColumn);
                        int columnIndex = int.Parse(SortColumn);
                        grvPageList.Columns[columnIndex].HeaderStyle.CssClass = sortType == "A" ? "sorting_desc" : "sorting_asc";
                        if (oldColumnIndex != columnIndex)
                            grvPageList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

                        string exp = string.Empty;
                        switch (columnIndex)
                        {
                            case 0:
                                exp = "Title";
                                break;
                            case 1:
                                exp = "CreatedBy";
                                break;
                            case 2:
                                exp = "CreatedOn";
                                break;
                            case 3:
                                exp = "IsObsolete";
                                break;
                        }

                        //Sort the data.
                        dt.DefaultView.Sort = exp + " " + (sortType == "A" ? "ASC" : "DESC");

                        grvPageList.DataSource = dt;
                        grvPageList.DataBind();
                        grvPageList.PageIndex = CurrentPageIndex;
                    }
                }
                else
                {
                    grvPageList.DataSource = null;
                    grvPageList.DataBind();
                }
                Session[ViewState["PageID"] + "tableSource"] = lst;
            }
            catch (Exception ex)
            {
                //ProcessException(ex);
            }
        }

        protected void grvPageList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        public static DataTable ToDataTable<T>(IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        protected void grvPageList_Sorting(object sender, GridViewSortEventArgs e)
        {
            TblNotificationCollection lst = Session[ViewState["PageID"] + "tableSource"] as TblNotificationCollection;
            if (lst != null && lst.Count > 0)
            {
                if (SortType == "A")
                {
                    SortType = "D";
                }
                else
                {
                    SortType = "A";
                }
                string sortType = SortType;

                int oldColumnIndex = SortColumn == null ? 0 : int.Parse(SortColumn);
                SortColumn = e.SortExpression;
                int columnIndex = int.Parse(SortColumn);
                grvPageList.Columns[columnIndex].HeaderStyle.CssClass = sortType == "A" ? "sorting_desc" : "sorting_asc";
                if (oldColumnIndex != columnIndex)
                    grvPageList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

                DataTable dt = ToDataTable<TblNotification>(lst);

                string exp = string.Empty;
                switch (columnIndex)
                {
                    case 0:
                        exp = "Title";
                        break;
                    case 1:
                        exp = "CreatedBy";
                        break;
                    case 2:
                        exp = "CreatedOn";
                        break;
                    case 3:
                        exp = "IsObsolete";
                        break;
                }

                //Sort the data.
                dt.DefaultView.Sort = exp + " " + (sortType == "A" ? "ASC" : "DESC");
                grvPageList.DataSource = dt;
                grvPageList.DataBind();
            }
        }

        protected void grvPageList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPageList.EditIndex = -1;

            CurrentPageIndex = e.NewPageIndex;
            grvPageList.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> lst = new List<int>();
            foreach (GridViewRow item in grvPageList.Rows)
            {
                HtmlInputCheckBox chkIsDelete = item.FindControl("chkIsDelete") as HtmlInputCheckBox;
                if (chkIsDelete != null && chkIsDelete.Checked == true)
                {
                    int id = 0;
                    int.TryParse(chkIsDelete.Value, out id);
                    if (id > 0)
                        lst.Add(id);
                }
            }
            if (lst != null && lst.Count > 0)
            {
                RealtimeNotificationManager.RemoveCurrentUsersFromReceiveList(lst);
                BindData();
            }
        }

        protected void btnMarkAsRead_Click(object sender, EventArgs e)
        {
            List<int> lst = new List<int>();
            foreach (GridViewRow item in grvPageList.Rows)
            {
                HtmlInputCheckBox chkIsDelete = item.FindControl("chkIsObsoleteView") as HtmlInputCheckBox;
                if (chkIsDelete != null && chkIsDelete.Checked == true)
                {
                    int id = 0;
                    int.TryParse(chkIsDelete.Value, out id);
                    if (id > 0)
                        lst.Add(id);
                }
            }

            if (lst != null && lst.Count > 0)
            {
                RealtimeNotificationManager.MarkAsRead(lst);
                ltrDelete.Text = "<input type='hidden' id='hdfdone' value='1' />";
            }
        }

        protected void btnMarkAsUnRead_Click(object sender, EventArgs e)
        {
            List<int> lst = new List<int>();
            foreach (GridViewRow item in grvPageList.Rows)
            {
                HtmlInputCheckBox chkIsDelete = item.FindControl("chkIsObsoleteView") as HtmlInputCheckBox;
                if (chkIsDelete != null && chkIsDelete.Checked == true)
                {
                    int id = 0;
                    int.TryParse(chkIsDelete.Value, out id);
                    if (id > 0)
                        lst.Add(id);
                }
            }

            if (lst != null && lst.Count > 0)
            {
                RealtimeNotificationManager.MarkAsUnRead(lst);
                ltrDelete.Text = "<input type='hidden' id='hdfdone' value='1' />";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvPageList.PageIndex = 0;
            BindData();
        }
    }
}