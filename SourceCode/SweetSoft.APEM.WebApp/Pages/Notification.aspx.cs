using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class Notification : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "notification_setting";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SortType = "D";
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                grvPageList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();

                string uas = CommonHelper.QueryString("uas");
                if (string.IsNullOrEmpty(uas))
                    RealtimeNotificationManager.UpdateNotification();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvPageList.Columns[0].HeaderText = "Page id";
            grvPageList.Columns[1].HeaderText = "Title";
            grvPageList.Columns[2].HeaderText = "Created by";
            grvPageList.Columns[3].HeaderText = "Created on";
            //grvPageList.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.IS_OBSOLETE);
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                List<NotificationPage> lst = RealtimeNotificationManager.GetAllSupportNotificationPage();
                if (lst != null && lst.Count > 0)
                {
                    DataTable dt = ToDataTable<NotificationPage>(lst);
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
                        SortColumn = grvPageList.SortExpression.Length > 0 ? grvPageList.SortExpression : "3";
                        int columnIndex = int.Parse(SortColumn);
                        grvPageList.Columns[columnIndex].HeaderStyle.CssClass = sortType == "A" ? "sorting_desc" : "sorting_asc";
                        if (oldColumnIndex != columnIndex)
                            grvPageList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

                        string exp = string.Empty;
                        switch (columnIndex)
                        {
                            case 0:
                                exp = "id";
                                break;
                            case 1:
                                exp = "title";
                                break;
                            case 2:
                                exp = "createdby";
                                break;
                            case 3:
                                exp = "createdon";
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
            List<NotificationPage> lst = Session[ViewState["PageID"] + "tableSource"] as List<NotificationPage>;
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

                DataTable dt = ToDataTable<NotificationPage>(lst);

                string exp = string.Empty;
                switch (columnIndex)
                {
                    case 0:
                        exp = "id";
                        break;
                    case 1:
                        exp = "title";
                        break;
                    case 2:
                        exp = "functionid";
                        break;
                    case 3:
                        exp = "url";
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
            foreach (GridViewRow item in grvPageList.Rows)
            {
                HtmlInputCheckBox chkIsDelete = item.FindControl("chkIsDelete") as HtmlInputCheckBox;
                if (chkIsDelete != null && chkIsDelete.Checked == true)
                {
                    Literal ltrPageId = item.FindControl("ltrPageId") as Literal;
                    if (ltrPageId != null && string.IsNullOrEmpty(ltrPageId.Text) == false)
                    {
                        bool isSucess = RealtimeNotificationManager.DeleteNotificationSettingByPageId(ltrPageId.Text);
                        if (isSucess)
                            chkIsDelete.Checked = false;
                    }
                }
            }
            BindData();
        }
    }
}