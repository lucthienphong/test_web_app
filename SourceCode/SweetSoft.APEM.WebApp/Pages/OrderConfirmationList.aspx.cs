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

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class OrderConfirmationList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "order_confirmation";
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Value.ToString().Equals("ORDER_CONFIRMATION_DELETE"))
                    {
                        //Kiểm tra quyền
                        if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                        {
                            MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msgRole, null, false, false);
                            return;
                        }

                        List<int> idList = new List<int>();
                        for (int i = 0; i < gvOrderConfiList.Rows.Count; i++)
                        {
                            CheckBox chkIsDelete = (CheckBox)gvOrderConfiList.Rows[i].FindControl("chkIsDelete");
                            if (chkIsDelete.Checked)
                            {
                                int ID = Convert.ToInt32(gvOrderConfiList.DataKeys[i].Value);
                                idList.Add(ID);
                                //OrderConfirmationManager.Delete(ID);
                            }
                        }

                        string DataCannotDelete = removeSelectedRows(idList);
                        BindData();
                        if (string.IsNullOrEmpty(DataCannotDelete))
                        {
                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data deleted susscessfully!", MSGButton.OK, MSGIcon.Success);
                            OpenMessageBox(msg, null, false, false);
                        }
                        else
                        {
                            string message = string.Format("{0}:<br/>{1}", "Can not delete following Oder Confirmation(s) because data is begin used", DataCannotDelete);
                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Warning);
                            OpenMessageBox(msg, null, false, false);
                        }
                    }
                    //btnLoadContacts_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        //Xóa dữ liệu được chọn
        private string removeSelectedRows(List<int> idList)
        {
            string cannotDeletedList = "";
            foreach (int ID in idList)
            {
                TblOrderConfirmation obj = OrderConfirmationManager.SelectByID(ID);
                if (JobManager.JobHasDO(ID).Length == 0)
                {                    
                    if (obj != null)
                    {
                        OrderConfirmationManager.Delete(obj.JobID);

                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                    }
                }
                else
                {
                    cannotDeletedList += string.Format("{0}, ", obj.OCNumber);
                }
            }
            if (!string.IsNullOrEmpty(cannotDeletedList))
            {
                cannotDeletedList = cannotDeletedList.Substring(0, cannotDeletedList.Length - 2);
            }
            return cannotDeletedList;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ApplyControlResourceTexts();
                BindData();
                gvOrderConfiList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
            }
            
        }

        private void ApplyControlResourceTexts()
        {
            gvOrderConfiList.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.OCNUMBER);
            gvOrderConfiList.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ORDER_DATE);
            gvOrderConfiList.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            gvOrderConfiList.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_NAME);
            gvOrderConfiList.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NUMBER);
            gvOrderConfiList.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NAME);
        }
        protected void gvOrderConfiList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            gvOrderConfiList.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void gvOrderConfiList_Sorting(object sender, GridViewSortEventArgs e)
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
            gvOrderConfiList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvOrderConfiList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

            BindData();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            #region Trunglc Add
            // Trunglc Add - 27-04-2015
            for (int i = 0; i < gvOrderConfiList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvOrderConfiList.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)
                {
                    int ID = Convert.ToInt32(gvOrderConfiList.DataKeys[i].Value);

                    bool IsNewOrderComfirmation = OrderConfirmationManager.IsNewOrderConfirmation(ID);
                    bool IsLocking = OrderConfirmationManager.IsOrderConfirmationLocking(ID);

                    if (!IsNewOrderComfirmation)
                    {
                        if (IsLocking || !RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                        {
                            MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msgRole, null, false, false);
                            return;
                        }
                    }

                }
            }

            #endregion

            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            bool HasDataToDelete = false;
            for (int i = 0; i < gvOrderConfiList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = gvOrderConfiList.Rows[i].FindControl("chkIsDelete") as CheckBox;
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
                result.Value = "ORDER_CONFIRMATION_DELETE";
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

        public override void BindData()
        {
            LoadPurchaseOrders();
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void LoadPurchaseOrders()
        {
            int totalRows = 0;
            string Customer = txtName.Text.Trim();
            string Job = txtJobNameOrNumber.Text.Trim();
            string OCNumber = txtOrderNumber.Text.Trim();
            DateTime? BeginDate = (DateTime?)null, EndDate = (DateTime?)null;
            DateTime _BeginDate = new DateTime(), _EndDate = new DateTime();
            if (DateTime.TryParseExact(txtfromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _BeginDate))
            {
                BeginDate = _BeginDate;
            }
            if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EndDate))
            {
                EndDate = _EndDate;
            }
            DataTable dt = OrderConfirmationManager.ConfirmOrderSelectAll(Customer, Job, OCNumber, BeginDate, EndDate, CurrentPageIndex, gvOrderConfiList.PageSize, SortColumn, SortType);
            if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
            {
                CurrentPageIndex -= 1;
                BindData();
            }
            else
            {
                if (dt.Rows.Count > 0)
                    totalRows = (int)dt.Rows[0]["RowsCount"];
                gvOrderConfiList.VirtualItemCount = totalRows;
                gvOrderConfiList.DataSource = dt;
                gvOrderConfiList.DataBind();
                gvOrderConfiList.PageIndex = CurrentPageIndex;
            }
        }


        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            gvOrderConfiList.PageIndex = 0;
            BindData();
        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtJobNameOrNumber.Text = string.Empty;
            txtOrderNumber.Text = string.Empty;
            txtfromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            BindData();
        }

        protected string ShowNumberFormat(object obj)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString("N2") : "0"; }
            return strPrice;
        }
    }
}