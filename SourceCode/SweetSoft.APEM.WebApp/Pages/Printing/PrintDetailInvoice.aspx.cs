using SweetSoft.APEM.Core;
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
    public partial class PrintDetailInvoice : Page
    {
        private int count = 1;

        private string item1 = string.Empty;

        private string item2 = string.Empty;

        private int quantityTotal = 0;

        private decimal totalPrice = 0;

        private int InvoiceID
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

        public string ShowTaxCode(object obj)
        {
            string taxCode = string.Empty;
            short taxId = 0;
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                short.TryParse(obj.ToString(), out taxId);

            TblTax tax = new TaxManager().SelectByID(taxId);
            if (tax != null)
                taxCode = tax.TaxCode;
            return taxCode;
        }

        protected void grvServiceJobDetail_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            for (int i = gv.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gv.Rows[i];
                GridViewRow previousRow = gv.Rows[i - 1];

                Label lblWorkOrderNumber;
                Label lblProductID;

                string valueAfter1 = string.Empty;
                lblWorkOrderNumber = (Label)row.FindControl("lblWorkOrderNumber");
                if (lblWorkOrderNumber != null) valueAfter1 = lblWorkOrderNumber.Text;

                string valueBefore1 = string.Empty;
                lblWorkOrderNumber = (Label)previousRow.FindControl("lblWorkOrderNumber");
                if (lblWorkOrderNumber != null) valueBefore1 = lblWorkOrderNumber.Text;

                string valueAfter2 = string.Empty;
                lblProductID = (Label)row.FindControl("lblProductID");
                if (lblProductID != null) valueAfter2 = lblProductID.Text;

                string valueBefore2 = string.Empty;
                lblProductID = (Label)previousRow.FindControl("lblProductID");
                if (lblProductID != null) valueBefore2 = lblProductID.Text;

                if (valueAfter1.Equals(valueBefore1) && valueAfter2.Equals(valueBefore2))
                {
                    if (previousRow.Cells[0].RowSpan == 0)
                    {
                        if (row.Cells[0].RowSpan == 0)
                        {
                            previousRow.Cells[0].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[0].RowSpan = row.Cells[0].RowSpan + 1;
                        }
                        row.Cells[0].Visible = false;
                    }

                    if (previousRow.Cells[1].RowSpan == 0)
                    {
                        if (row.Cells[1].RowSpan == 0)
                        {
                            previousRow.Cells[1].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;
                        }
                        row.Cells[1].Visible = false;
                    }

                    if (previousRow.Cells[2].RowSpan == 0)
                    {
                        if (row.Cells[2].RowSpan == 0)
                        {
                            previousRow.Cells[2].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[2].RowSpan = row.Cells[2].RowSpan + 1;
                        }
                        row.Cells[2].Visible = false;
                    }
                }
            }
        }

        protected void grvServiceJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TblServiceJobDetail item = e.Row.DataItem as TblServiceJobDetail;
                if (item != null)
                {
                    Label lblNo = e.Row.FindControl("lblNo") as Label;
                    Label lblWorkOrderValuesTaxed = e.Row.FindControl("lblWorkOrderValuesTaxed") as Label;
                    if (lblNo != null)
                    {
                        if (!item.WorkOrderNumber.Equals(item1) || !item.ProductID.Equals(item2))
                        { lblNo.Text = count.ToString(); count++; }
                        else
                            lblNo.Text = string.Empty;
                    }

                    if (lblWorkOrderValuesTaxed != null)
                    {
                        lblWorkOrderValuesTaxed.Text = (item.WorkOrderValues * (1 - (decimal)(item.TaxPercentage.HasValue ? item.TaxPercentage.Value : 0) / 100)).ToString("N2");
                    }

                    item1 = item.WorkOrderNumber;
                    item2 = item.ProductID;
                }
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
                    e.Row.Cells[0].ColumnSpan = 5;
                    e.Row.Cells[0].Text = "Total Cylinders:";
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[6].Text = quantityTotal.ToString("d");
                    e.Row.Cells[8].Text = totalPrice.ToString("N2");
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TblInvoice invoice = InvoiceManager.SelectByID(InvoiceID);
            if (invoice != null)
            {
                Session["INVOICE"] = invoice;
                TblInvoiceDetailCollection invoiceDetails = InvoiceManager.SelectInvoiceDetailByInvoiceId(invoice.InvoiceID);
                if (invoiceDetails != null & invoiceDetails.Count > 0)
                {
                    rptJobInvoice.DataSource = invoiceDetails;
                    rptJobInvoice.DataBind();
                }
            }
        }

        protected void rptJobInvoice_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            TblInvoiceDetail invoiceDetail = e.Item.DataItem as TblInvoiceDetail;
            TblInvoice invoice = Session["INVOICE"] as TblInvoice;

            #region e.item.FindControl("Literal")

            Literal ltrInvoiceNo = e.Item.FindControl("ltrInvoiceNo") as Literal;
            Literal ltrDate = e.Item.FindControl("ltrDate") as Literal;
            Literal ltrCustomer = e.Item.FindControl("ltrCustomer") as Literal;
            Literal ltrCompany = e.Item.FindControl("ltrCompany") as Literal;
            Literal ltrCompanyPhone = e.Item.FindControl("ltrCompanyPhone") as Literal;
            Literal ltrCompanyFax = e.Item.FindControl("ltrCompanyFax") as Literal;
            Literal ltrJobName = e.Item.FindControl("ltrJobName") as Literal;
            Literal ltrCompanyWebsite = e.Item.FindControl("ltrCompanyWebsite") as Literal;
            Literal ltrDesign = e.Item.FindControl("ltrDesign") as Literal;
            Literal ltrNetTotal = e.Item.FindControl("ltrNetTotal") as Literal;
            Literal ltrCurrency = e.Item.FindControl("ltrCurrency") as Literal;
            Literal ltrBaseCurrency = e.Item.FindControl("ltrBaseCurrency") as Literal;
            Literal ltrPaymentTerms = e.Item.FindControl("ltrPaymentTerms") as Literal;
            Literal ltrDeliveryTerm = e.Item.FindControl("ltrDeliveryTerm") as Literal;
            Literal ltrServiceJob = e.Item.FindControl("ltrServiceJob") as Literal;

            #endregion e.item.FindControl("Literal")

            ltrCompany.Text = string.Format("{0} {1}", SettingManager.GetSettingValue(SettingNames.CompanyName), SettingManager.GetSettingValue(SettingNames.CompanyAddress));
            ltrCompanyPhone.Text = SettingManager.GetSettingValue(SettingNames.CompanyPhone);
            ltrCompanyFax.Text = SettingManager.GetSettingValue(SettingNames.CompanyFax);
            ltrCompanyWebsite.Text = SettingManager.GetSettingValue(SettingNames.CompanyWebsite);
            ltrInvoiceNo.Text = invoice.InvoiceNo;
            ltrDate.Text = string.Format("{0}", invoice.InvoiceDate.ToString("dd/MM/yyyy"));

            #region Customer

            TblCustomer c = CustomerManager.SelectByID(invoice.CustomerID);
            if (c != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"
                        {0} <br/>
                        {1} <br/>
                        {2} <br/>
                    ", c.Name, c.Address, c.PostCode);
                ltrCustomer.Text = sb.ToString();
            }

            #endregion Customer

            #region Bind services job

            TblJob j = JobManager.SelectByID(invoiceDetail.JobID);
            if (j != null)
            {
                ltrJobName.Text = j.JobName;
                ltrDesign.Text = j.Design;

                GridView grvServiceJobDetail = e.Item.FindControl("grvServiceJobDetail") as GridView;
                grvServiceJobDetail.Columns[0].HeaderText = "No";
                grvServiceJobDetail.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_NUMBER);
                grvServiceJobDetail.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.PRODUCTID);
                grvServiceJobDetail.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
                grvServiceJobDetail.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.TAX_CODE);
                grvServiceJobDetail.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.TAX_RATE);
                grvServiceJobDetail.Columns[6].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_VALUES_IN_USD);

                List<ServiceJobDetailExtension> coll = JobManager.SelectServiceJobDetailByID(j.JobID);
                if (coll != null && coll.Count > 0)
                {
                    ltrServiceJob.Text = "<label class='control-label' style='font-weight: 700'>Service Job</label>";
                    grvServiceJobDetail.DataSource = coll;
                    grvServiceJobDetail.DataBind();
                }
                else
                {
                    var divServiceJob = e.Item.FindControl("divServiceJob") as object;
                }
            }

            #endregion Bind services job

            #region load cylinders

            GridView gvClinders = e.Item.FindControl("gvClinders") as GridView;
            DataTable dt = CylinderManager.SelectCylinderSelectForOrderConfirmation(invoiceDetail.JobID);
            if (dt != null && dt.Rows.Count > 0)
            {
                gvClinders.DataSource = dt;
                gvClinders.DataBind();
            }

            #endregion load cylinders

            #region contact name

            Literal ltrContact = e.Item.FindControl("ltrContact") as Literal;
            TblContact ct = ContactManager.SelectByID(invoice.ContactID);
            if (ct != null)
            {
                ltrContact.Text = string.Format("Attn: {0}. {1}", ct.Honorific, ct.ContactName);
            }

            #endregion contact name

            #region Order confirmation

            TblOrderConfirmation od = OrderConfirmationManager.SelectByID(invoiceDetail.JobID);
            if (od != null)
            {
                decimal rmValue = 0;
                if (od.Discount != null && od.TotalPrice != null)
                {
                    rmValue = (decimal)od.TotalPrice;
                    ltrNetTotal.Text = rmValue.ToString("N2");
                }
                else
                {
                    rmValue = (decimal)od.TotalPrice;
                    ltrNetTotal.Text = rmValue.ToString("N2");
                }

                TblCurrency cr = new CurrencyManager().SelectByID(od.CurrencyID);
                if (cr != null)
                {
                    decimal currencyValue = (rmValue * cr.CurrencyValue) / cr.RMValue;
                    ltrCurrency.Text = string.Format("{1} {0}", currencyValue.ToString("N2"), cr.CurrencyName);
                    ltrBaseCurrency.Text = string.Format("(base on RM {0} = {1} {2})", cr.RMValue, cr.CurrencyName, cr.CurrencyValue);
                }
                ltrPaymentTerms.Text = od.PaymentTerm;
                ltrDeliveryTerm.Text = od.DeliveryTerm;
            }

            #endregion Order confirmation

            #region Other charges

            List<OtherChargesExtension> odc = OrderConfirmationManager.SelectOtherChargeByJobID(invoiceDetail.JobID);
            if (odc != null)
            {
                GridView gvOtherCharges = e.Item.FindControl("gvOtherCharges") as GridView;
                gvOtherCharges.DataSource = odc;
                gvOtherCharges.DataBind();
            }

            #endregion Other charges
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