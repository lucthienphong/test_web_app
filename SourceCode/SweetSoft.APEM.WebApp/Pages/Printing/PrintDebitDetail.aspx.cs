using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintDebitDetail : System.Web.UI.Page
    {
        protected decimal TOTAL = 0;
        protected decimal SubTOTAL = 0;
        protected decimal TaxPercen = 0;

        private int DebitID
        {
            get
            {
                int _debitID = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"], out _debitID);
                }
                return _debitID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected void BindData()
        {
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);

            TblDebit debit = DebitManager.SelectByID(DebitID);
            ltrRemark.Text = debit.Remark;
            ltrCNumber.Text = debit.DebitNo;
            ltrCDate.Text = debit.DebitDate.ToString("dd.MM.yyyy");

            this.TaxPercen = debit.TaxID != null ? (decimal)new TaxManager().SelectByID((short)debit.TaxID).TaxPercentage : 0;
            lblTax.Text = this.TaxPercen.ToString() + "%";

            TblCustomer customer = CustomerManager.SelectByID(debit.CustomerID);
            ltrCustomerNumber.Text = customer.Code.ToString();
            ltrCustomerAddress.Text = customer.Address;
            ltrCustomerName.Text = customer.Name;

            TblCurrency cr = new CurrencyManager().SelectByID(debit.CurrencyID);
            ltrCurr.Text = cr.CurrencyName;

            List<TblDebitDetail> source = DebitManager.SellectAllDetail(DebitID);
            rptDebitDetail.DataSource = source;
            rptDebitDetail.DataBind();

            ltrPaymentTerms.Text = debit.TermsOfPayment;

        }

        protected void RepeaterItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Label l = e.Item.FindControl("lblSequence") as Label;
            if (l != null)
                l.Text = e.Item.ItemIndex + 1 + "";
            TblDebitDetail detail = (TblDebitDetail)e.Item.DataItem;            
            this.SubTOTAL += detail.UnitPrice * detail.Quantity;
            this.TOTAL = this.SubTOTAL * (1 + this.TaxPercen / 100);
            lblSubTotal.Text = this.SubTOTAL.ToString("N2");
            lblAllTotal.Text = this.TOTAL.ToString("N2");
            lblTaxAmount.Text = (this.SubTOTAL * this.TaxPercen / 100).ToString("N2");
        }
    }
}