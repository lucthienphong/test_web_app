using Microsoft.Reporting.WebForms;
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
    public partial class PurchaseOrderReport : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "purchase_order_report_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                ApplyControlText();
                grvSalesReport.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            //grvRemakeReport.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            //grvRemakeReport.Columns[1].HeaderText = "JobNr";
            //grvRemakeReport.Columns[2].HeaderText = "Rev";
            //grvRemakeReport.Columns[3].HeaderText = "JobName";
            //grvRemakeReport.Columns[4].HeaderText = "Design";
            //grvRemakeReport.Columns[5].HeaderText = "Qty";
            //grvRemakeReport.Columns[6].HeaderText = "Delivery";
            //grvRemakeReport.Columns[7].HeaderText = "Engraving";
            //grvRemakeReport.Columns[8].HeaderText = "ReproDate";
            //grvRemakeReport.Columns[9].HeaderText = "Repro Status";
            //grvRemakeReport.Columns[10].HeaderText = "CylinderDate";
            //grvRemakeReport.Columns[11].HeaderText = "Cylinder Status";
        }

        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        private void BindDDL()
        {
            BindSuppliersDDL();
        }

        //Bind OrderStatusDDL()
        private void BindSuppliersDDL()
        {
            List<TblSupplier> source = SupplierManager.SelectSupllierForDDL();
            ddlSuppliers.DataSource = source;
            ddlSuppliers.DataTextField = "Name";
            ddlSuppliers.DataValueField = "SupplierID";
            ddlSuppliers.DataBind();
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                int CustomerID = 0;
                int.TryParse(hCustomerID.Value, out CustomerID);
                int SupplierID = Convert.ToInt32(ddlSuppliers.SelectedValue);
                int BaseCurrencyID = 0;
                //Load Base Currency
                TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCurrencySetting);
                if (setting != null)
                    int.TryParse(setting.SettingValue, out BaseCurrencyID);
                DateTime? FromDate = (DateTime?)null, ToDate = (DateTime?)null;
                DateTime _FromDate = new DateTime(), _ToDate = new DateTime();
                if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _FromDate))
                    FromDate = _FromDate;
                if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ToDate))
                    ToDate = _ToDate;

                DataTable dt = ReportManager.PurchaseOrderReport(CustomerID, SupplierID, FromDate, ToDate, grvSalesReport.PageIndex, grvSalesReport.PageSize, SortColumn, SortType);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvSalesReport.VirtualItemCount = totalRows;
                    grvSalesReport.DataSource = dt;
                    grvSalesReport.DataBind();
                    grvSalesReport.PageIndex = CurrentPageIndex;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvSalesReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvSalesReport.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvSalesReport.PageIndex = 0;
            BindData();
        }

        protected string ShowNumberFormat(object obj, string Format)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString(Format) : "0"; }
            return strPrice;
        }

        #region ExportByCustomer

        private void CreateExportByCustomer(string fileName)
        {
            int CustomerID = 0;
            int.TryParse(hCustomerID.Value, out CustomerID);
            int SupplierID = Convert.ToInt32(ddlSuppliers.SelectedValue);
            int BaseCurrencyID = 0;
            //Load Base Currency
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCurrencySetting);
            if (setting != null)
                int.TryParse(setting.SettingValue, out BaseCurrencyID);
            DateTime? FromDate = (DateTime?)null, ToDate = (DateTime?)null, FromDateInvoice = (DateTime?)null, ToDateInvoice = (DateTime?)null;
            DateTime _FromDate = new DateTime(), _ToDate = new DateTime(), _FromDateInvoice = new DateTime(), _ToDateInvoice = new DateTime(); ;
            if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _FromDate))
                FromDate = _FromDate;
            
            string DateInfo = string.Empty;
            if (FromDate != null && ToDate != null)
            {
                if (DateTime.Compare(_FromDate, _ToDate) == 0)
                    DateInfo = _FromDate.ToString("dd/MM/yyyy");
                else
                    DateInfo = string.Format("From date: {0} - To date: {1}", _FromDate.ToString("dd/MM/yyyy"), _ToDate.ToString("dd/MM/yyyy"));
            }
            else if (FromDate != null)
            {
                DateInfo = string.Format("From date: {0}", _FromDate.ToString("dd/MM/yyyy"));
            }
            else if (ToDate != null)
            {
                DateInfo = string.Format("To date: {0}", _ToDate.ToString("dd/MM/yyyy"));
            }

            //Parameters
            string CreatedBy = ApplicationContext.Current.User.UserName;
            string CreatedOnDate = DateTime.Now.ToString("dd/MM/yyyy");
            string CreatedOnTime = DateTime.Now.ToString("hh:mm tt");

            ReportParameter[] parameters = new ReportParameter[3];
            parameters[0] = new ReportParameter("CreatedBy", CreatedBy);
            parameters[1] = new ReportParameter("CreatedOnDate", CreatedOnDate);
            parameters[2] = new ReportParameter("CreatedOnTime", CreatedOnTime);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable dtSource = ReportManager.PurchaseOrderReport(CustomerID, SupplierID, FromDate, ToDate, 0, 0, SortColumn, SortType);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PurchaseOrderReport_ByCustomers.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("DataPOReport", dtSource));
            viewer.LocalReport.SetParameters(parameters);

            //Chuyển sang Excel
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        private void CreateExportBySupplier(string fileName)
        {
            int CustomerID = 0;
            int.TryParse(hCustomerID.Value, out CustomerID);
            int SupplierID = Convert.ToInt32(ddlSuppliers.SelectedValue);
            int BaseCurrencyID = 0;
            //Load Base Currency
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCurrencySetting);
            if (setting != null)
                int.TryParse(setting.SettingValue, out BaseCurrencyID);
            DateTime? FromDate = (DateTime?)null, ToDate = (DateTime?)null, FromDateInvoice = (DateTime?)null, ToDateInvoice = (DateTime?)null;
            DateTime _FromDate = new DateTime(), _ToDate = new DateTime(), _FromDateInvoice = new DateTime(), _ToDateInvoice = new DateTime(); ;
            if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _FromDate))
                FromDate = _FromDate;

            string DateInfo = string.Empty;
            if (FromDate != null && ToDate != null)
            {
                if (DateTime.Compare(_FromDate, _ToDate) == 0)
                    DateInfo = _FromDate.ToString("dd/MM/yyyy");
                else
                    DateInfo = string.Format("From date: {0} - To date: {1}", _FromDate.ToString("dd/MM/yyyy"), _ToDate.ToString("dd/MM/yyyy"));
            }
            else if (FromDate != null)
            {
                DateInfo = string.Format("From date: {0}", _FromDate.ToString("dd/MM/yyyy"));
            }
            else if (ToDate != null)
            {
                DateInfo = string.Format("To date: {0}", _ToDate.ToString("dd/MM/yyyy"));
            }

            //Parameters
            string CreatedBy = ApplicationContext.Current.User.UserName;
            string CreatedOnDate = DateTime.Now.ToString("dd/MM/yyyy");
            string CreatedOnTime = DateTime.Now.ToString("hh:mm tt");

            ReportParameter[] parameters = new ReportParameter[3];
            parameters[0] = new ReportParameter("CreatedBy", CreatedBy);
            parameters[1] = new ReportParameter("CreatedOnDate", CreatedOnDate);
            parameters[2] = new ReportParameter("CreatedOnTime", CreatedOnTime);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable dtSource = ReportManager.PurchaseOrderReport(CustomerID, SupplierID, FromDate, ToDate, 0, 0, SortColumn, SortType);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PurchaseOrderReport_BySuppliers.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("DataPOReport", dtSource));
            viewer.LocalReport.SetParameters(parameters);

            //Chuyển sang Excel
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        #endregion

        protected void btnByCustomer_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("PurchaseReport_ByCustomers_{0}", today);
            CreateExportByCustomer(fileName);
        }

        protected void btnBySuppliers_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("PurchaseReport_BySuppliers_{0}", today);
            CreateExportBySupplier(fileName);
        }
    }

}