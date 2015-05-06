using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class QuotationPrinting : Page
    {
        private int CustomerID {
            get {
                if (Request.QueryString["ID"]!=null)
                {
                    int a = 0;
                    if (int.TryParse(Request.QueryString["ID"],out a))
                    {
                        return a;
                    }
                }
                return 0;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
            ltrCompanyPhone.Text = SettingManager.GetSettingValue(SettingNames.CompanyPhone);
            ltrCompanyFax.Text = SettingManager.GetSettingValue(SettingNames.CompanyFax);
            ltrCompanyEmail.Text = SettingManager.GetSettingValue(SettingNames.CompanyEmail);
            ltrISDN.Text = SettingManager.GetSettingValue(SettingNames.CompanyISDN);
            ltrCompanyAddress.Text = SettingManager.GetSettingValue(SettingNames.CompanyAddress);

            TblCustomerQuotation cQ = CustomerQuotationManager.SelectByID(CustomerID);
            if (cQ!=null)
            {
                TblCustomer cu = CustomerManager.SelectByID(cQ.CustomerID);
                if (cu!=null)
                {
                    string Address = cu.Address.Replace("\n", "<br />");
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"
                        {0} <br/>
                        {1} <br/>
                        {2} <br/>
                    ", cu.Name, Address, cu.PostCode);
                    ltrAddress.Text = sb.ToString();
                    ltrDeliveryTerms.Text = cu.DeliveryNote;
                    ltrPaymentTerms.Text = cu.PaymentNote;
                }
                TblContact ct = ContactManager.SelectByID(cQ.ContactPersonID);
                if (ct != null)
                {
                    ltrContact.Text = "Attn: " + ct.ContactName;
                }
                TblCurrency curObj = new CurrencyManager().SelectByID(cQ.CurrencyID != null ? (short)cQ.CurrencyID : (short)0);
                List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
                source = CustomerQuotationManager.SelectDetail(CustomerID);
                if (source.Count > 0)
                {
                    divPricing.Visible = true;
                    grvPrices.DataSource = source;
                    grvPrices.DataBind();
                }
                else
                    divPricing.Visible = false;

                List<CustomerQuotationAdditionalServiceExtention> sourceAdditional = new List<CustomerQuotationAdditionalServiceExtention>();
                sourceAdditional = CustomerQuotationManager.SelectAdditional(CustomerID);
                if (sourceAdditional.Count > 0)
                {
                    divAdditional.Visible = true;
                    grvAdditionalServices.DataSource = sourceAdditional;
                    grvAdditionalServices.DataBind();
                }
                else
                    divAdditional.Visible = false;

                List<CustomerQuotationOtherChargesExtention> sourceOtherCharges = new List<CustomerQuotationOtherChargesExtention>();
                sourceOtherCharges = CustomerQuotationManager.SelectOtherCharges(CustomerID);
                if (sourceOtherCharges.Count > 0)
                {
                    divOthers.Visible = true;
                    grvOtherCharges.DataSource = sourceOtherCharges;
                    grvOtherCharges.DataBind();
                }
                else
                    divOthers.Visible = false;

                ltrDate.Text = string.Format("Date: {0}", cQ.DateOfQuotation.ToString("dd/MM/yyyy"));
                ltrNotes.Text = cQ.QuotationText.Replace("\n", "<br />");
                //ltrDesignation.Text = cQ.ContactSignation;
            }            
        }     
    }
}