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
using Newtonsoft.Json;
using SweetSoft.APEM.Core.Logs;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class DeliveryOrderList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "delivery_order_manager";
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Value.ToString().Equals("DELIVERY_ORDERS_DELETE"))
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
                        for (int i = 0; i < gvDeliveryOrder.Rows.Count; i++)
                        {
                            CheckBox chkIsDelete = (CheckBox)gvDeliveryOrder.Rows[i].FindControl("chkIsDelete");
                            if (chkIsDelete.Checked)
                            {
                                int ID = Convert.ToInt32(gvDeliveryOrder.DataKeys[i].Value);
                                TblDeliveryOrder obj = DeliveryOrderManager.SelectDeliveryOrderByJobID(ID);
                                if (JobManager.JobHasInvoice(ID).Length == 0)
                                {
                                    if (obj != null)
                                    {
                                        lstData.Add(new JsonData() { Title = "Delivery Number", Data = obj.DONumber });
                                        lstData.Add(new JsonData() { Title = "Job Number", Data = obj.TblJob.JobNumber + "(Rev " + obj.TblJob.RevNumber.ToString() + ")" });
                                    }
                                }
                                idList.Add(ID);
                                //DeliveryOrderManager.Delete(ID);
                            }
                        }
                        string DataCannotDelete = removeSelectedRows(idList);
                        BindData();
                        if (string.IsNullOrEmpty(DataCannotDelete))
                        {
                            LoggingActions("Delivery",
                                           LogsAction.Objects.Action.DELETE,
                                           LogsAction.Objects.Status.SUCCESS,
                                           JsonConvert.SerializeObject(lstData));

                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data deleted susscessfully!", MSGButton.OK, MSGIcon.Success);
                            OpenMessageBox(msg, null, false, false);
                        }
                        else
                        {
                            string message = string.Format("{0}:<br/>{1}", "Can not delete following Delivery Order(s) because data is begin used", DataCannotDelete);
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
                TblDeliveryOrder obj = DeliveryOrderManager.SelectDeliveryOrderByJobID(ID);
                if (JobManager.JobHasInvoice(ID).Length == 0)
                {
                    if (obj != null)
                    {
                        DeliveryOrderManager.Delete(obj.JobID);

                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                    }
                }
                else
                {
                    cannotDeletedList += string.Format("{0}, ", obj.DONumber);
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
                gvDeliveryOrder.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }

        }

        private void ApplyControlResourceTexts()
        {
            gvDeliveryOrder.Columns[0].HeaderText = "DONumber";//ResourceTextManager.GetApplicationText(ResourceText.DELIVERY_ORDER);
            gvDeliveryOrder.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ORDER_DATE);
            gvDeliveryOrder.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            gvDeliveryOrder.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_NAME);
            gvDeliveryOrder.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NUMBER);
            gvDeliveryOrder.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NAME);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            #region Trunglc Add
            // Trunglc Add - 27-04-2015
            for (int i = 0; i < gvDeliveryOrder.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvDeliveryOrder.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)
                {
                    int ID = Convert.ToInt32(gvDeliveryOrder.DataKeys[i].Value);

                    bool IsNewDeliveryOrder = DeliveryOrderManager.IsNewDeliveryOrder(ID);
                    bool IsLocking = DeliveryOrderManager.IsDeliveryOrderLocking(ID);

                    if (!IsNewDeliveryOrder)
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
            for (int i = 0; i < gvDeliveryOrder.Rows.Count; i++)
            {
                CheckBox chkIsDelete = gvDeliveryOrder.Rows[i].FindControl("chkIsDelete") as CheckBox;
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
                result.Value = "DELIVERY_ORDERS_DELETE";
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

        protected void gvDeliveryOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            gvDeliveryOrder.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void gvDeliveryOrder_Sorting(object sender, GridViewSortEventArgs e)
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
            gvDeliveryOrder.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvDeliveryOrder.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

            BindData();
        }
        public override void BindData()
        {
            try
            {
                LoadDeliveryOrders();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }
        private void LoadDeliveryOrders()
        {
            DataTable devOrders = new DataTable();
            string Customer = txtName.Text.Trim();
            string DONumber = txtDeliveryOrder.Text.Trim();
            string Job = txtJobNumber.Text.Trim();
            DateTime? fromDate = (DateTime?)null;
            DateTime _fromDate = new DateTime();

            if (DateTime.TryParseExact(txtfromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate))
            {
                fromDate = _fromDate;
            }


            DateTime? toDate = (DateTime?)null;
            DateTime _toDate = new DateTime();
            if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate))
            {
                toDate = _toDate;
            }

            devOrders = DeliveryOrderManager.SelectDeliveryOrderByCustID(Customer, DONumber, Job, fromDate, toDate, CurrentPageIndex, gvDeliveryOrder.PageSize, SortColumn, SortType);

            if (devOrders.Rows.Count == 0 && CurrentPageIndex != 0)
            {
                CurrentPageIndex -= 1;
                BindData();
            }
            else
            {
                if (devOrders.Rows.Count > 0)
                {
                    gvDeliveryOrder.VirtualItemCount = (int)devOrders.Rows[0]["RowsCount"];
                }
                gvDeliveryOrder.DataSource = devOrders;
                gvDeliveryOrder.DataBind();
                gvDeliveryOrder.PageIndex = CurrentPageIndex; 
            }
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            gvDeliveryOrder.PageIndex = 0;
            BindData();
        }

        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

    }
}