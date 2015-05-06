using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintDeliveryOrder : Page
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

        public int rowsSpan = 0;

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
                    e.Row.Cells[0].ColumnSpan = 7;
                    e.Row.Cells[0].Text = "Total Cylinders Delivered:";
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Text = quantityTotal.ToString("d");
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TblDeliveryOrder d = DeliveryOrderManager.SelectDeliveryOrderByJobID(JobID);
            if (d != null)
            {
                TblUser obj = UserManager.GetUserByUserName(d.ModifiedBy);                
                TblStaff sObj = StaffManager.SelectByID(obj != null ? obj.UserID : 0);
                ltrDCCreator.Text = sObj != null ? string.Format("{0} {1}", sObj.FirstName, sObj.LastName) : string.Empty;
                TblJob j = JobManager.SelectByID(JobID);
                ltrOrderNo.Text = j.JobNumber;
                ltrRemark.Text =  d.OtherItem.Replace("\n", "<br/>");
                if (j.CreatedOn != null)
                {
                    DateTime date = (DateTime)j.CreatedOn;
                    ltrODate.Text = string.Format("{0}", date.ToString("dd.MM.yyyy"));
                }
                TblCustomer c = CustomerManager.SelectByID(j.ShipToParty != null ? (int)j.ShipToParty : 0);
                TblReference country = ReferenceTableManager.SelectByID(c != null ? c.CountryID : 0);
                string countryName = country != null ? country.Name : string.Empty;
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

                ltrCompany.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
                txtAddress.Text = SettingManager.GetSettingValue(SettingNames.CompanyAddress);
                ltrCompanyPhone.Text = SettingManager.GetSettingValue(SettingNames.CompanyPhone);
                ltrCompanyFax.Text = SettingManager.GetSettingValue(SettingNames.CompanyFax);
                ltrCompanyWebsite.Text = SettingManager.GetSettingValue(SettingNames.CompanyWebsite);

                ltrReference.Text = Common.Extensions.CombineString(d.CustomerPO1, d.CustomerPO2, ",");
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
                ltrAdditional.Text = d.OtherItem;

                #region Packing Dimension
                int totalPackingValue = 0;
                DataTable _dt = new DataTable();
                _dt.Columns.Add("Name", typeof(string));
                _dt.Columns.Add("Quantity", typeof(int));
                List<DeliveryOrderPackingDimensionExtension> doPackingCol = DeliveryOrderManager.SelectPackingDimensionDetail(j.JobID);
                if (doPackingCol != null)
                {
                    foreach (DeliveryOrderPackingDimensionExtension item in doPackingCol)
                    {
                        _dt.Rows.Add(item.PackingDimensionName, item.Quantity);
                        totalPackingValue += int.Parse(item.Quantity.ToString());
                        rowsSpan += 1;
                    }
                }

                rptPackingList.DataSource = _dt;
                rptPackingList.DataBind();
                ltrTotalPacking.Text = totalPackingValue.ToString();

                ltrGrossWeight.Text = d.GrossWeigth.ToString();
                ltrNetWeight.Text = d.NetWeight.ToString();

                string baseCountry = SettingManager.GetSettingValue(SettingNames.BaseCountrySetting);
                if (!string.IsNullOrEmpty(baseCountry))
                {
                    TblReference rf = ReferenceTableManager.SelectByID(int.Parse(SettingManager.GetSettingValue(SettingNames.BaseCountrySetting)));
                    if (rf!=null)
                    {
                        ltrCountryOrigin.Text = rf.Name;   
                    }
                }

                TblReference col = ReferenceTableManager.SelectByReferenceID(d.PackingID?? -1);
                if (col!=null)
                {
                    ltrPackingType.Text = col.Name;
                }

                #endregion
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