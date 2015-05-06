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

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintJobServiceDetail : Page
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
                if (Request.QueryString["ID"] != null)
                {
                    int invoiceID = 0;
                    if (int.TryParse(Request.QueryString["ID"], out invoiceID))
                    {
                        TblJobCollection coll = JobManager.SelectJobByInvoiceID(invoiceID);
                        TblJob obj = coll.FirstOrDefault();
                        if (obj != null)
                            return obj.JobID;
                        else
                            return 0;
                    }
                }
                return 0;
            }
        }

        private int InvoiceID
        {
            get
            {
                int invoiceID = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"], out invoiceID);
                }
                return invoiceID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        void BindData()
        {
            TblInvoice invoice = InvoiceManager.SelectByID(InvoiceID);
            if (invoice != null)
            {
                ////Lock invoice khi in lần đầu
                //if (!ObjectLockingManager.Exists(invoice.InvoiceID, ObjectLockingType.INVOICE))
                //    InvoiceManager.LockOrUnLockInvoice(invoice.InvoiceID, true);

                TblJob job = JobManager.SelectByID(JobID);
                if (job != null)
                {
                    //Lấy thông tin OC
                    TblOrderConfirmation ocObj = OrderConfirmationManager.SelectByID(JobID);
                    if (ocObj != null)
                    {
                        DiscountRate = ocObj.Discount != null ? (double)ocObj.Discount : 0;
                        TaxRate = ocObj.TaxPercentage != null ? (double)ocObj.TaxPercentage : 0;
                    }

                    ltrCompany.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
                    ltrInvoice.Text = ResourceTextManager.GetApplicationText(ResourceText.INVOICE);
                    //ltrTitle.Text = ResourceTextManager.GetApplicationText(ResourceText.SERVICE_JOB);

                    ltrInvoiceAndDate.Text = string.Format(" <label class='control-label col-xs-6 col-sm-6'>: {0} / Date</label><label class='control-label col-xs-6 col-sm-6 text-left'>{1} / {2} </label>",
                        "Invoice No",invoice.InvoiceDate.ToString("dd.MM.yyyy"), invoice.InvoiceNo);

                    ltrJobAndDate.Text = string.Format(" <label class='control-label col-xs-6 col-sm-6'>: {0} / Date</label><label class='control-label col-xs-6 col-sm-6 text-left'>{1} / {2} </label>",
                        "Order No", ((DateTime)job.CreatedOn).ToString("dd.MM.yyyy"), job.JobNumber);


                    TblCustomer customer = CustomerManager.SelectByID(job.CustomerID);
                    if (customer != null)
                    {
                        string countryName = string.Empty;
                        TblReference country = ReferenceTableManager.SelectByReferenceID(customer.CountryID);
                        if (country != null)
                            countryName = country.Name;

                        ltrCustomerInfo.Text = string.Format("<label class='control-label'>Attn: {0}<br /><br />{1}<br />{2},<br />{3}, {4}<br />{5}</label>",
                            ResourceTextManager.GetApplicationText(ResourceText.ACCOUNT_PAYABLE), customer.Name,
                            customer.Address, customer.PostCode, customer.City, countryName);
                    }

                    ltrPaymentTerms.Text = string.Format("<label class='control-label col-xs-3 text-left'>{0}: {1}</label>", ResourceTextManager.GetApplicationText(ResourceText.PAYMENT_TERMS), job.PaymentTerms);
                    ltrCompanyInfo1.Text = string.Format("<h6>{0}:<br /><strong>{1}<br />{2}: {3}</strong><br />{4}<br />",
                       ResourceTextManager.GetApplicationText(ResourceText.PLEASE_MAKE_PAYMENT_TO), SettingManager.GetSettingValue(SettingNames.CompanyName),
                       ResourceTextManager.GetApplicationText(ResourceText.ACCOUNT_NUMBER), SettingManager.GetSettingValue(SettingNames.BankAccountNumber),
                       SettingManager.GetSettingValue(SettingNames.BankName));

                    txtBankAddress.Text = SettingManager.GetSettingValue(SettingNames.BankAddress);
                    txtBankAddress.ReadOnly = true;
                    ltrSwiftCode.Text = string.Format("<br/>{0}: {1}</h6>", ResourceTextManager.GetApplicationText(ResourceText.SWIFT_CODE), SettingManager.GetSettingValue(SettingNames.BankSwiftCode));

                    ltrSignature.Text = string.Format("<div class='text-center col-xs-6'><br /><br /><br /><br /><br />........................................................<br /><span style='font-size: 8pt'>{0}</span></div>", SettingManager.GetSettingValue(SettingNames.CompanyName));

                    txtCompanyAddress.Text = SettingManager.GetSettingValue(SettingNames.CompanyAddress);
                    txtCompanyAddress.ReadOnly = true;
                    ltrCompanyInfo2.Text = string.Format("{0}: {1}<br />Fax:  {2}<br />{3}",
                        ResourceTextManager.GetApplicationText(ResourceText.PHONE), SettingManager.GetSettingValue(SettingNames.CompanyPhone)
                        , SettingManager.GetSettingValue(SettingNames.CompanyFax), SettingManager.GetSettingValue(SettingNames.CompanyWebsite));

                    //Bind detail
                    BindServiceJobDetail();
                    
                    //Bind Other Charges
                    BindOtherCharges();

                    //Bind total
                    Discount = TotalPrice * (decimal)(DiscountRate / 100);
                    TotalPriceBeforGST = TotalPrice - Discount;
                    Tax = TotalPriceBeforGST * (decimal)(TaxRate / 100);

                    lblSubTotal.Text = TotalPrice.ToString("N2");
                    ltrDiscountRate.Text = DiscountRate.ToString("N2") + "%";
                    lblDiscount.Text = Discount.ToString("N2");
                    lblSubTotalBefore.Text = TotalPriceBeforGST.ToString("N2");
                    ltrTaxRate.Text = TaxRate.ToString("N2") + "%";
                    lblGST.Text = Tax.ToString("N2");
                    lblTotal.Text = (TotalPrice - Discount + Tax).ToString("N2");
                }
            }
        }

        #region Service job detail
       
        private void BindServiceJobDetail()
        {
            List<ServiceJobDetailExtension> coll = JobManager.SelectServiceJobDetailByID(JobID);
            if (coll == null)
                coll = new List<ServiceJobDetailExtension>();
            TotalPrice = coll.Sum(x => x.WorkOrderValues); 
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

        public string ShowTaxCode(object obj)
        {
            string taxCode = string.Empty;

            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                short taxId = 0; short.TryParse(obj.ToString(), out taxId);

                TblTax tObj = new TaxManager().SelectByID(taxId);
                if (tObj != null)
                    taxCode = tObj.TaxCode;
            }
            return taxCode;
        }
    }

}