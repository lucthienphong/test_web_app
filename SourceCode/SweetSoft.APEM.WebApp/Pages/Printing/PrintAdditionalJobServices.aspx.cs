using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core;
using System.Text;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintAdditionalJobServices : Page
    {
        int CurrenNo { set; get; }
        decimal TotalPrice { set; get; }
        double DiscountRate { set; get; }
        decimal Discount { set; get; }
        double TaxRate { set; get; }
        decimal Tax { set; get; }
        decimal TotalPriceBeforGST { set; get; }

        private int JobID
        {
            get
            {
                int _jobID = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"], out _jobID);
                }
                return _jobID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        void BindData()
        {
            TblJob j = JobManager.SelectByID(JobID);
            if (j != null)
            {
                ltrCompany.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);

                ltrJobNumber.Text = j.JobNumber;
                ltrJobDate.Text = j.CreatedOn.HasValue ? j.CreatedOn.Value.ToString("dd.MM.yyyy") : string.Empty;
                ltrJobName.Text = j.JobName;
                ltrDesign.Text = j.Design;
                TblOrderConfirmation od = OrderConfirmationManager.SelectByID(JobID);
                if (od != null)
                {
                    ltrOCNumber.Text = j.JobNumber;//od.OCNumber;
                    ltrOCDate.Text = string.Format("{0}", od.OrderDate.ToString("dd.MM.yyyy"));
                    ltrPaymentTerm.Text = od.PaymentTerm;
                    ltrDeliveryTerm.Text = od.DeliveryTerm;
                    ltrReferences.Text = Common.Extensions.CombineString(od.CustomerPO1, od.CustomerPO2, ",");
                    DiscountRate = od.Discount != null ? (double)od.Discount : 0;
                    TaxRate = od.TaxPercentage != null ? (double)od.TaxPercentage : 0;
                }
                TblCurrency cr = new CurrencyManager().SelectByID(od.CurrencyID);
                if (cr != null)
                {
                    ltrCurr.Text = cr.CurrencyName;
                }
                TblContact ct = ContactManager.SelectByID(j.ContactPersonID);
                if (ct != null)
                {
                    ltrContact.Text = string.Format("{0}. {1}", ct.Honorific, ct.ContactName);
                }

                TblCustomer c = CustomerManager.SelectByID(j.CustomerID);
                TblReference country = ReferenceTableManager.SelectByID(c.CountryID);
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
                    ltrCustomerInfo.Text = sb.ToString();
                }


                ltrSignature.Text = string.Format("<div class='text-center col-xs-6'><br /><br /><br /><br /><br /><br /><span style='font-size: 8pt'>{0}</span></div>", SettingManager.GetSettingValue(SettingNames.CompanyName));


                //Bind detail
                BindServiceJobDetail();

                //Bind Other Charges
                BindOtherCharges();

                //Bind total
                Discount = TotalPrice * (decimal)(DiscountRate / 100);
                TotalPriceBeforGST = TotalPrice - Discount;
                Tax = TotalPriceBeforGST * (decimal)(TaxRate / 100);

                lblSubTotal.Text = TotalPrice.ToString("N2");
                ltrDiscountRate.Text = DiscountRate.ToString() + "%";
                lblDiscount.Text = Discount.ToString("N2");
                lblSubTotalBefore.Text = TotalPriceBeforGST.ToString("N2");
                ltrTaxRate.Text = TaxRate.ToString() + "%";
                lblGST.Text = Tax.ToString("N2");
                lblTotal.Text = (TotalPrice - Discount + Tax).ToString("N2");

            }
        }

        #region Service job detail
        private void BindServiceJobDetail()
        {
            List<ServiceJobDetailExtension> coll = JobManager.SelectServiceJobDetailByID(JobID);
            if (coll == null)
                coll = new List<ServiceJobDetailExtension>();
            TotalPrice += coll.Sum(x => x.WorkOrderValues);
            CurrenNo = coll.Count;
            rptServiceJob.DataSource = coll;
            rptServiceJob.DataBind();
        }
        #endregion

        #region Other Charges
        private void BindOtherCharges()
        {
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
                        CurrenNo += 1;
                        TblOtherChargeExtension o = new TblOtherChargeExtension()
                        {
                            tblOtherCharges = _ocharges,
                            No = CurrenNo,
                            TotalPrice = (_ocharges.Charge ?? 0) * (_ocharges.Quantity ?? 0)
                        };
                        _otherChargeExtension.Add(o);
                        totalOtherCharge += o.TotalPrice;
                    }
                }

                TotalPrice += _otherChargeExtension.Sum(x => x.TotalPrice);

                rptOtherCharges.DataSource = _otherChargeExtension;
                rptOtherCharges.DataBind();
            }
        }
        #endregion
    }
}