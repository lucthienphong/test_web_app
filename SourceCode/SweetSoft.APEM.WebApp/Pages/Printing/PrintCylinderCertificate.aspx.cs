using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintCylinderCertificate : Page
    {
        private int quantityTotal = 0;

        private int JobID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                {
                    int a = 0;
                    if (int.TryParse(Request.QueryString["ID"], out a))
                    {
                        return a;
                    }
                }
                return 0;
            }
        }

        protected void gvClinders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // add the UnitPrice and QuantityTotal to the running total variables
                quantityTotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Quantity"));
            }
            else
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 6;
                    e.Row.Cells[0].Text = "Total Cylinders Delivered:";
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    // for the Footer, display the running totals
                    e.Row.Cells[6].Text = quantityTotal.ToString("d");
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
            ltrAddress.Text = ResourceTextManager.GetApplicationText(ResourceText.CERTIFICATE_OF_ANALYSIS);

            TblJob j = JobManager.SelectByID(JobID);
            if (j != null)
            {
                ltrJobNumber.Text = j.JobNumber;
                ltrDesign.Text = j.Design;
                TblUser user = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
                ltrCertPreparedBy.Text = user != null ? user.DisplayName : string.Empty;
                ltrDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                TblJobSheet jobSheet = JobManager.SelectJobSheetByID(j.JobID);
                if (jobSheet != null)
                {
                    ltrCylCircumferenceMM.Text = jobSheet.Circumference.ToString("N2");
                    ltrCylFacewidthMM.Text = jobSheet.FaceWidth.ToString("N2");
                    ltrDigits.Text = jobSheet.BarcodeNo;
                    ltrColour.Text = jobSheet.BarcodeColor;
                }
                TblCustomer c = CustomerManager.SelectByID(j.CustomerID);
                if (c != null)
                {
                    ltrCustomer.Text = c.Name;
                }

                DataTable dt = JobManager.tblEngravingDetail_SelectAll(j.JobID);
                grvCylinder.DataSource = dt;
                grvCylinder.DataBind();
            }
        }

        protected string ShowNumberFormat(object obj)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString("N5") : "0"; }
            return strPrice;
        }
    }
}