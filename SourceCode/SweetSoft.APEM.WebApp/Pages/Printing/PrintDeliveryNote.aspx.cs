using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintDeliveryNote : Page
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
                quantityTotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Quantity"));
            }
            else
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 5;
                    e.Row.Cells[0].Text = "Total Cylinders:";
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[7].Text = quantityTotal.ToString("d");
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TblDeliveryOrder d = DeliveryOrderManager.SelectDeliveryOrderByJobID(JobID);
            if (d != null)
            {
                TblJob j = JobManager.SelectByID(JobID);
                TblCustomer c = CustomerManager.SelectByID(j.ShipToParty != null ? (int)j.ShipToParty : 0);
                TblReference country = ReferenceTableManager.SelectByID(c != null ? c.CountryID : 0);
                string countryName = country != null ? country.Name : string.Empty;
                ltrOrderNo.Text = j.JobNumber;
                ltrRemark.Text = d.OtherItem.Replace("\n", "<br/>");
                if (j.CreatedOn!=null)
                {
                    DateTime date = (DateTime)j.CreatedOn;
                    ltrODate.Text = string.Format("{0}", date.ToString("dd.MM.yyyy"));    
                }
                
                if (c != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"
                        {0} <br/>
                        {1} <br/>
                        {2}, {3} <br/>
                        {4} </br>
                    ", c.Name, c.Address, c.PostCode, c.City, countryName);
                    ltrAddress.Text = sb.ToString();
                }

                ltrCompany.Text = string.Format("{0} {1}", SettingManager.GetSettingValue(SettingNames.CompanyName), SettingManager.GetSettingValue(SettingNames.CompanyAddress));
                ltrCompanyPhone.Text = SettingManager.GetSettingValue(SettingNames.CompanyPhone);
                ltrCompanyFax.Text = SettingManager.GetSettingValue(SettingNames.CompanyFax);
                ltrCompanyWebsite.Text = SettingManager.GetSettingValue(SettingNames.CompanyWebsite);
                ltrJobName.Text = j.JobName;
                ltrDesign.Text = j.Design;
                ltrDeliveryOrder.Text = d.DONumber;
                ltrDate.Text = string.Format("{0}", d.OrderDate.ToString("dd.MM.yyyy"));

                DataTable dt = CylinderManager.SelectCylinderSelectForDeliveryOrder(JobID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvClinders.DataSource = dt;
                    gvClinders.DataBind();
                }
                //ltrAdditional.Text = d.OtherItem;
            }
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