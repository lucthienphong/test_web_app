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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class PurchaseOrderList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "purchase_order_manager";
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e) {
            try
            {
                if (e != null)
                {
                    if (e.Value.ToString().Equals("PURCHASE_ORDERS_DELETE"))
                    {
                        List<int> idList = new List<int>();
                        for (int i = 0; i < gvPurchaseOrder.Rows.Count; i++)
                        {
                            CheckBox chkIsDelete = (CheckBox)gvPurchaseOrder.Rows[i].FindControl("chkIsDelete");
                            if (chkIsDelete.Checked)
                            {
                                int ID = Convert.ToInt32(gvPurchaseOrder.DataKeys[i].Value);
                                PurchaseOrderManager.DeleteAllPurchaseOrderCylinder(ID);
                                PurchaseOrderManager.DeletePurchaseOrder(ID);
                            }
                        }
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.WARNING), "The data have been deleted", MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                    }
                    btnLoadContacts_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ApplyControlResourceTexts();
                gvPurchaseOrder.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
            
        }

        private void ApplyControlResourceTexts()
        {
            gvPurchaseOrder.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.OCNUMBER);
            gvPurchaseOrder.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ORDER_DATE);
            gvPurchaseOrder.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DELIVERY_DATE);
            gvPurchaseOrder.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            gvPurchaseOrder.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_NAME);
            gvPurchaseOrder.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NUMBER);
            gvPurchaseOrder.Columns[6].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NAME);
        }

        protected void gvPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            gvPurchaseOrder.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void gvPurchaseOrder_Sorting(object sender, GridViewSortEventArgs e)
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
            gvPurchaseOrder.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvPurchaseOrder.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

            BindData();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            bool HasDataToDelete = false;
            for (int i = 0; i < gvPurchaseOrder.Rows.Count; i++)
            {
                CheckBox chkIsDelete = gvPurchaseOrder.Rows[i].FindControl("chkIsDelete") as CheckBox;
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
                result.Value = "PURCHASE_ORDERS_DELETE";
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
            try
            {
                LoadPurchaseOrders();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void LoadPurchaseOrders()
        {
            DataTable purOrders = new DataTable();
            string Customer = txtName.Text.Trim();
            string Job = txtJobNumber.Text.Trim();
            string OrderNumber = txtPurchaseOrder.Text.Trim();
            DateTime? FromDate = (DateTime?)null;
            DateTime _FromDate = new DateTime();
            if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _FromDate))
            {
                FromDate = _FromDate;
            }
            DateTime? ToDate = (DateTime?)null;
            DateTime _ToDate = new DateTime();
            if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ToDate))
            {
                ToDate = _ToDate;
            }

            purOrders = PurchaseOrderManager.GetPurchaseOrdersBy(Customer, Job, OrderNumber, FromDate, ToDate, CurrentPageIndex, gvPurchaseOrder.PageSize, SortColumn, SortType);

            if (purOrders.Rows.Count == 0 && CurrentPageIndex != 0)
            {
                CurrentPageIndex -= 1;
                BindData();
            }
            else
            {
                if (purOrders.Rows.Count>0)
                {
                    gvPurchaseOrder.VirtualItemCount = (int)purOrders.Rows[0]["RowsCount"];    
                }
                
                gvPurchaseOrder.DataSource = purOrders;
                
                gvPurchaseOrder.DataBind();
                gvPurchaseOrder.PageIndex = CurrentPageIndex;
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
            gvPurchaseOrder.PageIndex = 0;
            BindData();
        }
    }
}