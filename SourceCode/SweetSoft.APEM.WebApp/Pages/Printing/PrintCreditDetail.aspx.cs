using System;
using System.Collections.Generic;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System.Web.UI.WebControls;


namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintCreditNote : System.Web.UI.Page
    {
        protected decimal TOTAL = 0;
        protected decimal SubTOTAL = 0;
        protected decimal TaxPercen = 0;

        private int CreditID
        {
            get
            {
                int _creditID = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"], out _creditID);
                }
                return _creditID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected void BindData()
        {
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName) + "<br />";
            string sCompanyInfo = SettingManager.GetSettingValue(SettingNames.CompanyAddress) + "<br />";
            sCompanyInfo += "Phone " + SettingManager.GetSettingValue(SettingNames.CompanyPhone) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += "Fax " + SettingManager.GetSettingValue(SettingNames.CompanyFax) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += SettingManager.GetSettingValue(SettingNames.CompanyWebsite) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += "GST No.:" + SettingManager.GetSettingValue(SettingNames.CompanyGST) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />";
            sCompanyInfo += "TIN No.:" + SettingManager.GetSettingValue(SettingNames.CompanyGST);

            lblCompanyInfo.Text = sCompanyInfo;

            TblCredit credit = CreditManager.SelectByID(CreditID);
            ltrRemark.Text = credit.Remark;
            ltrCNumber.Text = credit.CreditNo;
            ltrCDate.Text = credit.CreditDate.ToString("dd.MM.yyyy");

            this.TaxPercen = credit.TaxID != null ? (decimal)new TaxManager().SelectByID((short)credit.TaxID).TaxPercentage : 0;
            lblTax.Text = this.TaxPercen.ToString() + "%";

            TblCustomer customer = CustomerManager.SelectByID(credit.CustomerID);
            ltrCustomerNumber.Text = customer.Code.ToString();
            ltrCustomerAddress.Text = customer.Address;
            ltrCustomerName.Text = customer.Name;

            TblCurrency cr = new CurrencyManager().SelectByID(credit.CurrencyID);
            ltrCurr.Text = cr.CurrencyName;

            List<TblCreditDetail> source = CreditManager.SellectAllDetail(CreditID);
            rptCreditDetail.DataSource = source;
            rptCreditDetail.DataBind();
        }

        protected void RepeaterItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Label l = e.Item.FindControl("lblSequence") as Label;
            if (l != null)
                l.Text = e.Item.ItemIndex + 1 + "";
            TblCreditDetail detail = (TblCreditDetail)e.Item.DataItem;
            this.SubTOTAL += detail.UnitPrice * detail.Quantity;
            this.TOTAL = this.SubTOTAL * (1 - this.TaxPercen / 100);
            lblSubTotal.Text = this.SubTOTAL.ToString("N2");
            lblAllTotal.Text = this.TOTAL.ToString("N2");
            lblTaxAmount.Text = (this.SubTOTAL * this.TaxPercen / 100).ToString("N2");
        }
    }
}