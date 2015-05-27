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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class CustomerList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "customer_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                BindDDL();
                grvCustomerList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        protected void grvCustomerList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvCustomerList.PageIndex = CurrentPageIndex;
            BindData();
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvCustomerList.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            grvCustomerList.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER);
            grvCustomerList.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_ADDRESS);
            grvCustomerList.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_TEL);
            grvCustomerList.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_FAX);
            grvCustomerList.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.IS_OBSOLETE);
        }

        private void BindDDL()
        {
            BindStatusDDL();
        }

        private void BindStatusDDL()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            dt.Rows.Add(0, "--Select a status--");
            dt.Rows.Add(1, "Is current");
            dt.Rows.Add(2, "Is obsolete");

            ddlStatus.DataSource = dt;
            ddlStatus.DataTextField = "Name";
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataBind();
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                string keyword = txtKeyword.Text.Trim();
                bool? isObsolete = ddlStatus.SelectedValue == "0" ? (bool?)null : (ddlStatus.SelectedValue == "1" ? false : true);
                DataTable dt = CustomerManager.SelectAll(keyword, isObsolete, CurrentPageIndex, grvCustomerList.PageSize, SortColumn, SortType);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvCustomerList.VirtualItemCount = totalRows;
                    grvCustomerList.DataSource = dt;
                    grvCustomerList.DataBind();
                    grvCustomerList.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvCustomerList_Sorting(object sender, GridViewSortEventArgs e)
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
            grvCustomerList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvCustomerList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
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
            for (int i = 0; i < grvCustomerList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvCustomerList.Rows[i].FindControl("chkIsDelete");
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
                result.Value = "Customer_Delete";
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

        //Xóa dữ liệu được chọn
        private string removeSelectedRows(List<int> idList)
        {
            string cannotDeletedList = "";
            foreach (int ID in idList)
            {
                if (!CustomerManager.IsBeingUsed(ID))
                {
                    TblCustomer obj = CustomerManager.SelectByID(ID);
                    if (obj != null)
                    {
                        CustomerManager.Delete(obj.CustomerID);

                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                    }
                }
                else
                {
                    DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                    string customerName = dt.AsEnumerable().Where(x => x.Field<int>("CustomerID") == ID).Select(x => x.Field<string>("Name")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", customerName);
                }
            }
            if (!string.IsNullOrEmpty(cannotDeletedList))
            {
                cannotDeletedList = cannotDeletedList.Substring(0, cannotDeletedList.Length - 2);
            }
            return cannotDeletedList;
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        if (e.Value.ToString().Equals("Customer_Delete"))
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

                            for (int i = 0; i < grvCustomerList.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)grvCustomerList.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    int ID = Convert.ToInt32(grvCustomerList.DataKeys[i].Value);
                                    TblCustomer obj = CustomerManager.SelectByID(ID);
                                    if (obj != null)
                                    {
                                        lstData.Add(new JsonData() { Title = "Customer Code", Data = obj.Code });
                                        lstData.Add(new JsonData() { Title = "Customer Name", Data = obj.Name });
                                    }
                                    idList.Add(ID);
                                }
                            }

                            string DataCannotDelete = removeSelectedRows(idList);
                            BindData();
                            if (string.IsNullOrEmpty(DataCannotDelete))
                            {
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data deleted susscessfully!", MSGButton.OK, MSGIcon.Success);
                                OpenMessageBox(msg, null, false, false);
                                LoggingActions("Customer",
                                           LogsAction.Objects.Action.DELETE,
                                           LogsAction.Objects.Status.SUCCESS,
                                           JsonConvert.SerializeObject(lstData));
                            }
                            else
                            {
                                string message = string.Format("{0}:<br/>{1}", ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CANNOT_DELETE), DataCannotDelete);
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Warning);
                                OpenMessageBox(msg, null, false, false);
                            }
                        }
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvCustomerList.PageIndex = 0;
            BindData();
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        //EXPORT EXCEL

        private void CreateExcel(string fileName)
        {
            string keyword = txtKeyword.Text.Trim();
            bool? isObsolete = ddlStatus.SelectedValue == "0" ? (bool?)null : (ddlStatus.SelectedValue == "1" ? false : true);
            //Parameters

            ReportParameter[] parameters = new ReportParameter[1];
            //CompanyName
            string CompanyName = string.Empty;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyName);
            if (setting != null)
                CompanyName = setting.SettingValue;
            parameters[0] = new ReportParameter("CompanyName", CompanyName);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable dtSource = CustomerManager.SelectAllForReport(keyword, isObsolete, SortColumn, SortType);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/CustomerList_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("CustomerListSrc", dtSource));
            viewer.LocalReport.SetParameters(parameters);

            //Chuyển sang Excel
            byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("CustomerList_{0}", today);
            CreateExcel(fileName);
        }

        protected void grvCustomerList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Pages/Customer.aspx?ID=" + ID.ToString());
            }
            else if (e.CommandName == "Quotation")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Pages/CustomerQuotation.aspx?ID=" + ID.ToString());
            }
        }
    }
}