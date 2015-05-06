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
    public partial class PrintDeliveryOrderNotePDF : System.Web.UI.Page
    {
        private int JobID
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
                viewer = new ReportViewer();
                string FileName = string.Format("DO_{0}", DateTime.Now.ToString("ddMMyyyy"));
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

            DataTable sourceSummary = DeliveryOrderManager.SelectDeliveryOrderForPrint(JobID);
            DataTable sourceCylinder = CylinderManager.SelectCylinderSelectForDeliveryOrder(JobID);

            int baseCountryID = 0;
            int.TryParse(SettingManager.GetSettingValue(SettingNames.BaseCountrySetting), out baseCountryID);
            TblReference country = ReferenceTableManager.SelectByID(baseCountryID);

            ReportParameter BaseCountry = new ReportParameter("BaseCountry", country != null ? country.Name : string.Empty);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/DeliveryOrderNote.rdlc");
            viewer.LocalReport.DataSources.Clear();

            viewer.LocalReport.DataSources.Add(new ReportDataSource("JobSource", sourceSummary));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("CylinderSource", sourceCylinder));

            viewer.LocalReport.SetParameters(new ReportParameter[] { BaseCountry });
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