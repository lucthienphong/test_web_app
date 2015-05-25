using Microsoft.Reporting.WebForms;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class DebitList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "debit_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                grvCrebitList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            //grvJobList.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER);
            //grvJobList.Columns[1].HeaderText = "Job Nr";
            //grvJobList.Columns[2].HeaderText = "Job Name";
            //grvJobList.Columns[3].HeaderText = "Design";
            //grvJobList.Columns[4].HeaderText = "Created date";
        }

        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                string DebitNo = txtDebitNo.Text.Trim();
                string Customer = txtCustomer.Text.Trim();
                DateTime? FromDate = (DateTime?)null;
                DateTime? ToDate = (DateTime?)null;
                DateTime _fromDate = new DateTime();
                DateTime _toDate = new DateTime();
                if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate))
                    FromDate = _fromDate;
                if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate))
                    ToDate = _toDate;

                DataTable dt = DebitManager.SelectAll(DebitNo, Customer, FromDate, ToDate, grvCrebitList.PageIndex, grvCrebitList.PageSize, SortColumn, SortType);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvCrebitList.VirtualItemCount = totalRows;
                    grvCrebitList.DataSource = dt;
                    grvCrebitList.DataBind();
                    grvCrebitList.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvCrebitList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Pages/Debit.aspx?ID=" + ID.ToString());
            }
        }

        protected void grvCrebitList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvCrebitList.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvCrebitList_Sorting(object sender, GridViewSortEventArgs e)
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
            grvCrebitList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvCrebitList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Kiểm tra quyền
            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            bool HasDataToDelete = false;
            for (int i = 0; i < grvCrebitList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvCrebitList.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)//Nếu tồn tại dòng dữ liệu được check thì hiện thông báo xóa
                {
                    HasDataToDelete = true;
                    break;
                }
            }
            if (HasDataToDelete)
            {
                //Gọi message box
                ModalConfirmResult result = new ModalConfirmResult();
                result.Value = "Debit_Delete";
                CurrentConfirmResult = result;
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Do you want to delete seleted rows?", MSGButton.DeleteCancel, MSGIcon.Error);
                OpenMessageBox(msg, result, false, false);
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.SELECT_DATA_TO_DELETE), MSGButton.OK, MSGIcon.Info);
                OpenMessageBox(msg, null, false, false);
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        if (e.Value.ToString().Equals("Debit_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<int> idList = new List<int>();
                            List<JsonData> lstData = new List<JsonData>();
                            for (int i = 0; i < grvCrebitList.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)grvCrebitList.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    int ID = Convert.ToInt32(grvCrebitList.DataKeys[i].Value);
                                    TblDebit obj = DebitManager.SelectByID(ID);
                                    if (obj != null)
                                    {
                                        lstData.Add(new JsonData() { Title = "Debit Code", Data = obj.DebitNo });
                                    }
                                    idList.Add(ID);
                                }
                            }
                            removeSelectedRows(idList);
                            BindData();

                            LoggingActions("Debit",
                                           LogsAction.Objects.Action.DELETE,
                                           LogsAction.Objects.Status.SUCCESS,
                                           JsonConvert.SerializeObject(lstData));

                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data deleted susscessfully!", MSGButton.OK, MSGIcon.Success);
                            OpenMessageBox(msg, null, false, false);
                        }
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        BindData();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        //Xóa dữ liệu được chọn
        private void removeSelectedRows(List<int> idList)
        {
            foreach (int ID in idList)
            {
                TblDebit obj = DebitManager.SelectByID(ID);
                if (obj != null)
                {
                    DebitManager.Delete(obj.DebitID);

                    //Lưu vào logging
                    LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvCrebitList.PageIndex = 0;
            BindData();
        }
    }
}