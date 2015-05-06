﻿using System;
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
    public partial class PrintCreditNote : System.Web.UI.Page
    {
        protected decimal TOTAL = 0;

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
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);

            TblCredit credit = CreditManager.SelectByID(CreditID);
            ltrRemark.Text = credit.Remark;
            ltrCNumber.Text = credit.CreditNo;
            ltrCDate.Text = credit.CreditDate.ToString("dd.MM.yyyy");

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
            this.TOTAL += detail.UnitPrice * detail.Quantity;
            lblAllTotal.Text = this.TOTAL.ToString("N3");
        }
    }
}