using Microsoft.Reporting.WebForms;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintTaxInvoiceServicePDF : System.Web.UI.Page
    {
        private int InvoiceID
        {
            get
            {
                int ID = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"], out ID);
                }
                return ID;
            }
        }

        ReportViewer viewer = new ReportViewer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Lock invoice khi in lần đầu
                TblInvoice invoice = InvoiceManager.SelectByID(InvoiceID);
                if (invoice != null)
                {
                    if (!ObjectLockingManager.Exists(InvoiceID, ObjectLockingType.INVOICE))
                    {
                        InvoiceManager.LockOrUnLockInvoice(InvoiceID, true);                        
                    }
                    TblInvoiceDetailCollection invoiceDetail = InvoiceManager.SelectInvoiceDetailByInvoiceId(InvoiceID);
                    foreach (var item in invoiceDetail)
                    {
                        InvoiceManager.LockJobAndOCAndDO(item.JobID);
                    }
                }

                viewer = new ReportViewer();
                string FileName = string.Format("Invoice_{0}", DateTime.Now.ToString("ddMMyyyy"));
                PrintPDF(FileName);
            }
        }

        private void PrintPDF(string fileName)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            viewer.ProcessingMode = ProcessingMode.Local;

            //Invoice Summary
            DataTable sourceInvoiceSummary = InvoiceManager.SelectInvoiceSummaryForPrint(InvoiceID);
            List<int> JobIDs = InvoiceManager.SelectListJobIDByInvoiceId(InvoiceID);
            int JobID = JobIDs.Count > 0 ? JobIDs.FirstOrDefault() : 0;
            //Job Summary
            DataTable sourceJobSummary = InvoiceManager.SelectJobSummaryForPrint(JobID);
            //Service Job Souce
            List<ServiceJobDetailExtension> AdditionaleServices = JobManager.SelectServiceJobDetailByID(JobID);
            DataTable sourceAdditionalServices = AdditionaleServices.ToDataTable();
            //Other Charges Source
            DataTable sourceOtherCharges = InvoiceManager.SelectOtherChargesForPrint(JobID);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/InvoiceTaxService.rdlc");
            viewer.LocalReport.DataSources.Clear();

            viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceSource", sourceInvoiceSummary));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("JobSource", sourceJobSummary));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("AdditionalServicesSource", sourceAdditionalServices));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("OtherChargesSource", sourceOtherCharges));

            //Chuyển sang PDF
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Write("a");
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "content-disposition: inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }
    }
}