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
    public partial class PrintTaxInvoicePDF : System.Web.UI.Page
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
                        TblInvoiceDetailCollection invoiceDetail = InvoiceManager.SelectInvoiceDetailByInvoiceId(InvoiceID);
                        foreach (var item in invoiceDetail)
                        {
                            InvoiceManager.LockJobAndOCAndDO(item.JobID);
                        }
                    }
                }

                viewer = new ReportViewer();
                viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(MySubreportEventHandler);
                //string FileName = string.Format("Invoice_{0}", DateTime.Now.ToString("ddMMyyyy"));
                PrintPDFSubRpt("FileName");
            }
        }

        void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            int JobID = 0;
            int.TryParse(e.Parameters.Where(x => x.Name == "JobID").Select(x => x.Values[0]).FirstOrDefault(), out JobID);
            DataTable sourceSummary = InvoiceManager.SelectJobSummaryForPrint(JobID);
            DataTable sourceCylinder = CylinderManager.SelectCylinderSelectForOrderConfirmation(JobID);

            DataTable fillSourceCylinder = sourceCylinder.Clone();
            DataRow[]  dataRow = sourceCylinder.Select("Quantity > 0");

            foreach (DataRow r in dataRow)
            {
                fillSourceCylinder.ImportRow(r);
            }

            DataTable sourceOtherCharges = InvoiceManager.SelectOtherChargesForPrint(JobID);
            e.DataSources.Add(new ReportDataSource("JobSummarySource", sourceSummary));
            e.DataSources.Add(new ReportDataSource("JobCylinderSource", fillSourceCylinder));
            e.DataSources.Add(new ReportDataSource("JobOtherChargeSouce", sourceOtherCharges));
        }

        private void PrintPDFSubRpt(string fileName)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable sourceSummary = InvoiceManager.SelectInvoiceSummaryForPrint(InvoiceID);
            DataTable sourceDetail = InvoiceManager.SelectInvoiceJobListForPrint(InvoiceID);
            ReportParameter Remark;
            if (sourceDetail.Rows.Count > 0)
                Remark = new ReportParameter("Remark", sourceSummary.Rows[0]["Remark"].ToString());
            else
                Remark = new ReportParameter("Remark", string.Empty);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/InvoiceTax.rdlc");
            viewer.LocalReport.DataSources.Clear();

            viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceSource", sourceSummary));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("JobListSource", sourceDetail));

            viewer.LocalReport.SetParameters(new ReportParameter[] { Remark });
            //Chuyển sang Excel
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