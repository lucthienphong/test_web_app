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
    public partial class PrintOrderConfirmation : Page
    {
        private int quantityTotal = 0;

        private decimal totalPrice = 0;
        public string ocDiscount = "0";
        public string ocTaxRate = "0";

        private int InvoiceID
        {
            get
            {
                if (Request.QueryString["InvoiceID"] != null)
                {
                    int a = 0;
                    if (int.TryParse(Request.QueryString["InvoiceID"], out a))
                    {
                        return a;
                    }
                }
                return 0;
            }
        }

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
                totalPrice += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalPrice"));
            }
            else
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 6;
                    e.Row.Cells[0].Text = "Total Cylinders:";
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Text = quantityTotal.ToString("d");
                    e.Row.Cells[7].Text = totalPrice.ToString("N3");
                    e.Row.Cells[8].Text = totalPrice.ToString("N3");
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName) + "<br />";
            ltrCompanyPhone.Text = SettingManager.GetSettingValue(SettingNames.CompanyPhone);
            ltrCompanyFax.Text = SettingManager.GetSettingValue(SettingNames.CompanyFax);
            ltrCompanyEmail.Text = SettingManager.GetSettingValue(SettingNames.CompanyEmail);
            ltrISDN.Text = SettingManager.GetSettingValue(SettingNames.CompanyISDN);
            ltrCompanyAddress.Text = SettingManager.GetSettingValue(SettingNames.CompanyAddress);

            ltrCompany.Text = SettingManager.GetSettingValue(SettingNames.CompanyName) + "<br />";
            string sCompanyInfo = SettingManager.GetSettingValue(SettingNames.CompanyAddress) + "<br />";
            sCompanyInfo += "Phone " + SettingManager.GetSettingValue(SettingNames.CompanyPhone) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += "Fax " + SettingManager.GetSettingValue(SettingNames.CompanyFax) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += SettingManager.GetSettingValue(SettingNames.CompanyWebsite) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += "GST No.:" + SettingManager.GetSettingValue(SettingNames.CompanyGST) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />";
            sCompanyInfo += "TIN No.:" + SettingManager.GetSettingValue(SettingNames.CompanyGST);

            lblCompanyInfo.Text = sCompanyInfo;

            TblOrderConfirmation od = OrderConfirmationManager.SelectByID(JobID);
            if (od != null)
            {

                TblUser obj = UserManager.GetUserByUserName(od.ModifiedBy);
                TblStaff sObj = StaffManager.SelectByID(obj != null ? obj.UserID : 0);
                ltrOCCreator.Text = sObj != null ? string.Format("{0} {1}", sObj.FirstName, sObj.LastName) : string.Empty;
                ocDiscount = od.Discount != null ? ((double)od.Discount).ToString("N2") : "0.00";
                ocTaxRate = od.TaxPercentage != null ? ((double)od.TaxPercentage).ToString("N2") : "0.00";
                TblJob j = JobManager.SelectByID(JobID);
                if (j.CreatedOn != null)
                {
                    DateTime date = (DateTime)j.CreatedOn;
                    ltrJobDate.Text = string.Format("{0}", date.ToString("dd.MM.yyyy"));
                }

                ltrJobNumber.Text = j.JobNumber + (j.RevNumber > 0 ? string.Format(" (R{0})", j.RevNumber) : string.Empty);
                ltrReferences.Text = Common.Extensions.CombineString(od.CustomerPO1, od.CustomerPO2, ",");
                
                TblCustomer c = CustomerManager.SelectByID(j.CustomerID);
                TblContact ct = ContactManager.SelectByID(j.ContactPersonID);
                TblJobSheet js = JobManager.SelectJobSheetByID(JobID);
                TblReference country = ReferenceTableManager.SelectByID(c.CountryID);
                string countryName = country != null ? country.Name : string.Empty;
                if (js != null)
                {
                    ltrIris.Text = js.IrisProof != null ? ((int)js.IrisProof).ToString() : string.Empty;
                }
                if (ct != null)
                {
                    ltrContact.Text = string.Format("{0}. {1}", ct.Honorific, ct.ContactName);
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
                    ltrCustomer.Text = sb.ToString();
                }

                ltrJobName.Text = j.JobName;
                ltrDesign.Text = j.Design;
                ltrJobNumber_Up.Text = j.JobNumber + (j.RevNumber > 0 ? string.Format(" (R{0})", j.RevNumber) : string.Empty);
                ltrOCDate.Text = string.Format("{0}", od.OrderDate.ToString("dd.MM.yyyy"));
                decimal sumTotalPrice = 0 ;
                decimal rmValue = od.TotalPrice.HasValue ? od.TotalPrice.Value : 0;
                decimal discount = (decimal)(od.Discount.HasValue ? od.Discount.Value : 0);
                decimal totalPrice = rmValue * (1 - discount / 100);

                #region Other Charges
                List<int> listOtherCharges = OrderConfirmationManager.SelectListOtherChargeIDByJobID(JobID);
                decimal totalOtherCharge = 0;
                if (listOtherCharges != null && listOtherCharges.Count > 0)
                {
                    List<TblOtherChargeExtension> _otherChargeExtension = new List<TblOtherChargeExtension>();
                    foreach (var item in listOtherCharges)
                    {
                        TblOtherCharge _ocharges = new TblOtherCharge();
                        _ocharges = OrderConfirmationManager.SelectOtherChargeByID(item);
                        if (_ocharges != null)
                        {
                            TblOtherChargeExtension o = new TblOtherChargeExtension()
                            {
                                tblOtherCharges = _ocharges,
                                TotalPrice = (_ocharges.Charge ?? 0) * (_ocharges.Quantity ?? 0)
                            };
                            _otherChargeExtension.Add(o);
                            totalOtherCharge += o.TotalPrice;
                        }
                    }
                    rptOtherCharges.DataSource = _otherChargeExtension;
                    rptOtherCharges.DataBind();
                }
                #endregion

                #region Service Detail
                List<ServiceJobDetailExtension> serviceCharges = JobManager.SelectServiceJobDetailByID(JobID);
                decimal totalServiceCharge = 0;
                if (serviceCharges!=null && serviceCharges.Count>0)
                {
                    rptServicesCharges.DataSource = serviceCharges;
                    rptServicesCharges.DataBind();
                    foreach (var item in serviceCharges)
                    {
                        totalServiceCharge += item.WorkOrderValues;
                    }
                }
                #endregion
                DataTable dt = CylinderManager.SelectCylinderSelectForOrderConfirmation(JobID);

                if (dt != null && dt.Rows.Count > 0)
                {
                 
                    rptCylinder.DataSource = dt;
                    rptCylinder.DataBind();

                    sumTotalPrice = decimal.Parse(dt.Compute("Sum(TotalPrice)", "").ToString()) + totalOtherCharge + totalServiceCharge;
                    decimal _tax = od.TaxPercentage!=null ? decimal.Parse(od.TaxPercentage.ToString()): 1;
                }

                //Tổng chưa chiết khấu & thuế
                //decimal Total = od.TotalPrice != null ? (decimal)od.TotalPrice : 0;
                //decimal DiscountRate = od.Discount != null ? (decimal)od.Discount : 0;
                decimal TaxRate = od.TaxPercentage != null ? (decimal)od.TaxPercentage : 0;

                lblSubTotal.Text = sumTotalPrice.ToString("N2");
                lblDiscount.Text = (sumTotalPrice * discount / 100).ToString("N2");
                lblSubTotalBefore.Text = (sumTotalPrice * (1 - discount / 100)).ToString("N2");
                lblGST.Text = (sumTotalPrice * (1 - discount / 100) * (TaxRate / 100)).ToString("N2");
                lblTotal.Text = (sumTotalPrice * (1 - discount / 100) * (1 + TaxRate / 100)).ToString("N2");
                

                TblCurrency cr = new CurrencyManager().SelectByID(od.CurrencyID);
                ltrCurr.Text = cr.CurrencyName;
                if (cr != null)
                {
                    decimal currencyValue = (totalPrice * cr.CurrencyValue) / cr.RMValue;
                    ltrCurrency.Text = string.Format("{1} {0}", currencyValue.ToString("N3"), cr.CurrencyName);
                    ltrBaseCurrency.Text = string.Format("(base on RM {0} = {1} {2})", cr.RMValue, cr.CurrencyName, cr.CurrencyValue);
                }
                ltrPaymentTerms.Text = od.PaymentTerm;
                ltrDeliveryTerm.Text = od.DeliveryTerm;
                ltrRemark.Text = od.Remark.Replace("\n", "<br/>");
                //ltrAdditional.Text = d.OtherItem;
            }
        }

        protected string ShowNumberFormat(object obj, string target)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { 
                decimal price = 0; 
                decimal.TryParse(obj.ToString(), out price);
                strPrice = price > 0 ? price.ToString(target) : "0"; 
            }
            return strPrice;
        }
    }

    public class TblOtherChargeExtension {
        public TblOtherCharge tblOtherCharges { get; set; }
        public int No
        {
            set;
            get;
        }
        public decimal TotalPrice
        {
            get;
            set;    
        }
    }
}
