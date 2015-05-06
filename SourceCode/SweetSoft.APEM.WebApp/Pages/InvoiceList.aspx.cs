using Microsoft.Reporting.WebForms;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class InvoiceList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "invoice_manager";
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {

                if (e != null)
                {
                    if (e.Value.ToString().Equals("Invoice_Delte"))
                    {
                        List<int> idList = new List<int>();
                        for (int i = 0; i < grvInvoiceList.Rows.Count; i++)
                        {
                            CheckBox chkIsDelete = (CheckBox)grvInvoiceList.Rows[i].FindControl("chkIsDelete");
                            if (chkIsDelete.Checked)
                            {
                                int ID = Convert.ToInt32(grvInvoiceList.DataKeys[i].Value);
                                TblInvoice invoice = InvoiceManager.SelectByID(ID);
                                if (invoice != null)
                                {
                                    if (InvoiceManager.Delete(ID))
                                    {
                                        //Lưu vào logging
                                        if (AllowSaveLogging)
                                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.DELETE_SERVICE_JOB), FUNCTION_PAGE_ID, invoice.ToJSONString());
                                    }
                                }
                            }
                        }

                        BindData();

                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.WARNING), "The data have been deleted", MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                    }
                    //btnLoadContacts_Click(null, null);
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
                BindData();
                grvInvoiceList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
            }
            
        }

        private void ApplyControlResourceTexts()
        {
            grvInvoiceList.Columns[0].HeaderText = "Invoice no";//ResourceTextManager.GetApplicationText(ResourceText.INVOICE_NUMBER);
            grvInvoiceList.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.INVOICE_DATE);
            grvInvoiceList.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            grvInvoiceList.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_NAME);
            grvInvoiceList.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NUMBER);
            grvInvoiceList.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NAME);
        }
        protected void grvInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvInvoiceList.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvInvoiceList_Sorting(object sender, GridViewSortEventArgs e)
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
            grvInvoiceList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvInvoiceList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

            BindData();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            #region Trunglc Add
            // Trunglc Add - 27-04-2015
            for (int i = 0; i < grvInvoiceList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvInvoiceList.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)
                {
                    int ID = Convert.ToInt32(grvInvoiceList.DataKeys[i].Value);

                    bool IsNewInvoice = InvoiceManager.IsNewInvoice(ID);
                    bool IsLocking = InvoiceManager.IsInvoiceLocking(ID);

                    if (!IsNewInvoice)
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
            for (int i = 0; i < grvInvoiceList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = grvInvoiceList.Rows[i].FindControl("chkIsDelete") as CheckBox;
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
                result.Value = "Invoice_Delte";
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
            //BindData();
        }

        private void LoadPurchaseOrders()
        {
            int totalRows = 0;
            string Customer = txtName.Text.Trim();
            string InvoiceNo = txtOrderNumber.Text.Trim();
            string Job = txtJobName.Text.Trim();
            DateTime? fromDate = (DateTime?)null;
            DateTime _fromDate = new DateTime();
            if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate))
            {
                fromDate = _fromDate;
            }

            DateTime? toDate = (DateTime?)null;
            DateTime _toDate = new DateTime();
            if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate))
            {
                toDate = _toDate;
            }

            DataTable dt = InvoiceManager.InvoiceSelectAll(Customer, InvoiceNo, Job, fromDate, toDate, CurrentPageIndex, grvInvoiceList.PageSize, SortColumn, SortType);
            if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
            {
                CurrentPageIndex -= 1;
                BindData();
            }
            else
            {
                if (dt.Rows.Count > 0)
                    totalRows = (int)dt.Rows[0]["RowsCount"];
                grvInvoiceList.VirtualItemCount = totalRows;
                grvInvoiceList.DataSource = dt;
                grvInvoiceList.DataBind();
                grvInvoiceList.PageIndex = CurrentPageIndex;
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
            grvInvoiceList.PageIndex = 0;
            BindData();
        }

        protected string ShowNumberFormat(object obj)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString("N2") : "0"; }
            return strPrice;
        }

        private string GetInvoiceIDs()
        {
            string InvoiceIDs = string.Empty;
            for (int i = 0; i < grvInvoiceList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvInvoiceList.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)
                {
                    int ID = Convert.ToInt32(grvInvoiceList.DataKeys[i].Value);
                    InvoiceIDs += string.Format("-{0}-", ID);
                }
            }
            return InvoiceIDs;
        }

        private void CreateExcel(string fileName)
        {
            //Parameters
            ReportParameter[] parameters = new ReportParameter[10];
            string InvoiceNo = string.Empty, SAPCode = string.Empty, CompanyCode = string.Empty, InvoiceDate = string.Empty, PostingDate = string.Empty, CurrencyName = string.Empty, CalcTax = string.Empty, TaxCode = string.Empty, Total = string.Empty, RMValue = string.Empty;
            string InvoiceIDs = GetInvoiceIDs();
            DataTable dtInvoice = InvoiceManager.SelectForExport(InvoiceIDs);
            if (dtInvoice.Rows.Count > 0)
            {
                InvoiceNo = dtInvoice.Rows[0]["InvoiceNo"].ToString();
                SAPCode = dtInvoice.Rows[0]["SAPCode"].ToString();
                InvoiceDate = dtInvoice.Rows[0]["InvoiceNo"].ToString();
                PostingDate = DateTime.Today.ToString("yyyyMMdd");
                CurrencyName = dtInvoice.Rows[0]["CurrencyName"].ToString();
                CalcTax = dtInvoice.Rows[0]["CalcTax"].ToString();
                TaxCode = dtInvoice.Rows[0]["TaxCode"].ToString();
                Total = string.IsNullOrEmpty(dtInvoice.Rows[0]["TotalPrice"].ToString()) ? string.Empty : ((decimal)dtInvoice.Rows[0]["TotalPrice"]).ToString("N2");
                RMValue = string.IsNullOrEmpty(dtInvoice.Rows[0]["RMValue"].ToString()) ? string.Empty : ((decimal)dtInvoice.Rows[0]["RMValue"]).ToString("N2");
            }
            //InvoiceNo
            parameters[0] = new ReportParameter("InvoiceNo", InvoiceNo);
            //SAPCode
            parameters[1] = new ReportParameter("SAPCode", SAPCode);
            //CompanyCode
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyCode);
            if (setting != null)
                CompanyCode = setting.SettingValue;
            parameters[2] = new ReportParameter("CompanyCode", CompanyCode);
            //InvoiceDate
            parameters[3] = new ReportParameter("InvoiceDate", InvoiceDate);
            //PostingDate
            parameters[4] = new ReportParameter("PostingDate", PostingDate);
            //CurrencyName
            parameters[5] = new ReportParameter("CurrencyName", CurrencyName);
            //CalcTax
            parameters[6] = new ReportParameter("CalcTax", CalcTax);
            //TaxCode
            parameters[7] = new ReportParameter("TaxCode", TaxCode);
            //Total
            parameters[8] = new ReportParameter("Total", Total);
            //RMValue
            parameters[9] = new ReportParameter("RMValue", RMValue);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable dtDetail = InvoiceManager.SelectDetailForExport(InvoiceIDs);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Invoice_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceSAPHeader", dtInvoice));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceSrc", dtDetail));
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

        private void CreateHeadFile(string FileName)
        {

            string InvoiceNo = string.Empty, SAPCode = string.Empty, CompanyCode = string.Empty, InvoiceDate = string.Empty, PostingDate = string.Empty, CurrencyName = string.Empty, CalcTax = string.Empty, TaxCode = string.Empty, Total = string.Empty, RMValue = string.Empty;
            //EXPORT HEAD
            //Get Invocie data
            string InvoiceIDs = GetInvoiceIDs();
            DataTable dtInvoice = InvoiceManager.SelectForExport(InvoiceIDs);
            StringWriter headWriter = new StringWriter();
            foreach (var r in dtInvoice.AsEnumerable().ToList())
            {
                InvoiceNo = r.Field<string>("InvoiceNo");
                SAPCode = r.Field<string>("SAPCode");
                InvoiceDate = r.Field<string>("InvoiceNo");
                PostingDate = DateTime.Today.ToString("yyyyMMdd");
                CurrencyName = r.Field<string>("CurrencyName");
                CalcTax = r.Field<string>("CalcTax");
                TaxCode = r.Field<string>("TaxCode");
                Total = r.Field<decimal>("TotalPrice").ToString("N2");
                RMValue = r.Field<decimal>("RMValue").ToString("N2");

                headWriter.WriteLine("{0}    {1}    {2}    {3}    {4}    {5}    {6}    {7}    {8}", InvoiceNo, SAPCode, InvoiceDate, PostingDate, CurrencyName, CalcTax, TaxCode, Total, RMValue);
            }
            Response.ContentType = "text/plain";

            Response.AddHeader("content-disposition", "attachment;filename=" + string.Format("Head_File_{0}.txt", FileName));
            Response.Clear();

            using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                writer.Write(headWriter.ToString());
            }
            Response.End();
        }

        private void CreatePotisionsFile(string FileName)
        {
            //EXPORT POTISIONS
            //Get invoice detail data
            string InvoiceIDs = GetInvoiceIDs();
            DataTable dtDetail = InvoiceManager.SelectDetailForExport(InvoiceIDs);

            StringWriter potisionWriter = new StringWriter();
            //Write detail to file
            foreach (var r in dtDetail.AsEnumerable().ToList())
            {
                string dInvoiceNo = r.Field<string>("InvoiceNo");
                string dGLCode = r.Field<string>("GLCode");
                string dTotal = r.Field<double>("Total").ToString("N2");
                string dTaxCode = r.Field<string>("TaxCode");
                string dDescription = r.Field<string>("Description");
                string dJobNumber = r.Field<string>("JobNumber");

                potisionWriter.WriteLine("{0}    {1}    {2}    {3}    {4}    {5}", dInvoiceNo, dGLCode, dTotal, dTaxCode, dDescription, dJobNumber);
            }

            Response.ContentType = "text/plain";

            Response.AddHeader("content-disposition", "attachment;filename=" + string.Format("Positions_File_{0}.txt", FileName));
            Response.Clear();

            using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                writer.Write(potisionWriter.ToString());
            }
            Response.End();
        }

        protected void btnExportHead_Click(object sender, EventArgs e)
        {
            if (GetInvoiceIDs().Length != 0)
            {
                string fileName = string.Format("{0}", DateTime.Today.ToString("dd.MM.yyyy"));
                CreateHeadFile(fileName);
            }
            else
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "No invoice has been selected", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
            }
        }

        protected void tblExportPotisions_Click(object sender, EventArgs e)
        {
            if (GetInvoiceIDs().Length != 0)
            {
                string fileName = string.Format("{0}", DateTime.Today.ToString("dd.MM.yyyy"));
                CreatePotisionsFile(fileName);
            }
            else
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "No invoice has been selected", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (GetInvoiceIDs().Length != 0)
            {
                string fileName = string.Format("Invoices_{0}", DateTime.Today.ToString("dd.MM.yyyy"));
                CreateExcel(fileName);
            }
            else
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "No invoice has been selected", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
            }
        }
    }
}