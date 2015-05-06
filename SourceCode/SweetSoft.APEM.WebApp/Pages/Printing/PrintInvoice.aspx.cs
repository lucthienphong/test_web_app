using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintInvoice : Page
    {
        private int count = 1;

        private double grandTotal = 0;

        private string item1 = string.Empty;

        private string item2 = string.Empty;

        private int quantityTotal = 0;

        private decimal totalPrice = 0;

        private TblCurrency CurrencyVoice { get; set; }

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
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
            ltr_CompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
            ltrAccountNumber.Text = string.Format("Account number: {0}", SettingManager.GetSettingValue(SettingNames.BankAccountNumber));
            ltrBankName.Text = SettingManager.GetSettingValue(SettingNames.BankName);
            ltrSwiftCode.Text = string.Format("Swift code: {0}", SettingManager.GetSettingValue(SettingNames.BankSwiftCode));
            txtAddress.Text = SettingManager.GetSettingValue(SettingNames.BankAddress);
            TblInvoice invoice = InvoiceManager.SelectByID(InvoiceID);
            if (invoice != null)
            {
                ////Lock invoice khi in lần đầu
                //if (!ObjectLockingManager.Exists(invoice.InvoiceID, ObjectLockingType.INVOICE))
                //    InvoiceManager.LockOrUnLockInvoice(invoice.InvoiceID, true);

                ltrInvoiceNo.Text = invoice.InvoiceNo;
                ltrDate.Text = invoice.InvoiceDate.ToString("dd/MM/yyyy");

                TblCustomer cus = CustomerManager.SelectByID(invoice.CustomerID);
                if (cus != null)
                {
                    string countryName = string.Empty;
                    TblReference country = ReferenceTableManager.SelectByReferenceID(cus.CountryID);
                    if (country != null)
                        countryName = country.Name;

                    ltrCustomer.Text = string.Format("{0} <br/> {1} <br/> {2}, {3} <br/> {4} </br>", cus.Name, cus.Address, cus.PostCode, cus.City, countryName);
                }

                TblContact ct = ContactManager.SelectByID(invoice.ContactID);
                if (ct != null)
                {
                    ltrContact.Text = ct.ContactName;
                }
                ltrReference.Text = invoice.PONumber;

                TblInvoiceDetailCollection invoiceDetails = InvoiceManager.SelectInvoiceDetailByInvoiceId(invoice.InvoiceID);
                if (invoiceDetails != null & invoiceDetails.Count > 0)
                {
                    TblOrderConfirmation od = OrderConfirmationManager.SelectByID(invoiceDetails.First().JobID);
                    if (od != null)
                    {
                        TblCurrency curnc = new CurrencyManager().SelectByID(od.CurrencyID);
                        if (curnc != null)
                        {
                            CurrencyVoice = curnc;
                        }
                    }
                    rptInvoiceDetails.DataSource = invoiceDetails;
                    rptInvoiceDetails.DataBind();
                    ltrGrandTotal.Text = ExchangeCurrency(grandTotal).ToString("N3");
                }
            }
        }

        protected void rptInvoiceDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            #region e.item.FindControl("Control")

            Literal ltrTrHeading = e.Item.FindControl("ltrTrHeading") as Literal;
            Literal ltrSTWidth = e.Item.FindControl("ltrSTWidth") as Literal;
            Literal ltrSTCirf = e.Item.FindControl("ltrSTCirf") as Literal;
            Literal ltrSTQty = e.Item.FindControl("ltrSTQty") as Literal;
            Literal ltrSTUnitPrice = e.Item.FindControl("ltrSTUnitPrice") as Literal;
            Literal ltrSTTotal = e.Item.FindControl("ltrSTTotal") as Literal;

            Literal ltrEGQty = e.Item.FindControl("ltrEGQty") as Literal;
            Literal ltrEGUnitPrice = e.Item.FindControl("ltrEGUnitPrice") as Literal;
            Literal ltrEGTotal = e.Item.FindControl("ltrEGTotal") as Literal;

            Literal ltrSVQty = e.Item.FindControl("ltrSVQty") as Literal;
            Literal ltrSVUnitPrice = e.Item.FindControl("ltrSVUnitPrice") as Literal;
            Literal ltrSVTotal = e.Item.FindControl("ltrSVTotal") as Literal;

            #endregion e.item.FindControl("Control")

            TblInvoiceDetail ind = e.Item.DataItem as TblInvoiceDetail;
            TblJob j = JobManager.SelectByID(ind.JobID);
            if (j != null)
            {
                #region Job Information

                DateTime jobDate = j.CreatedOn ?? DateTime.Now;
                string jobInformation = string.Format("Job number :{0} - Date:{1} <br/>", j.JobNumber, jobDate.ToString("dd/MM/yyyy"));
                TblOrderConfirmation oc = OrderConfirmationManager.SelectByID(j.JobID);
                if (oc != null)
                {
                    jobInformation += string.Format("Customer PO: {0} <br/>", oc.CustomerPO1);
                }
                TblDeliveryOrder dO = DeliveryOrderManager.SelectDeliveryOrderByJobID(j.JobID);
                if (dO != null)
                {
                    DateTime doDate = dO.CreatedOn ?? DateTime.Now;
                    jobInformation += string.Format("DO number: {0} - DO date: {1}<br/>", dO.DONumber, doDate.ToString("dd/MM/yyyy"));
                }

                jobInformation += string.Format("Design: {0}", j.Design);

                #endregion Job Information

                #region steel base

                TblPurchaseOrderCollection od = PurchaseOrderManager.SelectPurchaseOrderByJobID(ind.JobID);
                if (od != null && od.Count > 0)
                {
                    int t = 0;
                    decimal total = 0;
                    foreach (var _od in od)
                    {
                        TblPurchaseOrderCylinderCollection odc = PurchaseOrderManager.SelectPurchaseCylinderOrderByPurchaseID(_od.PurchaseOrderID);
                        if (odc != null && odc.Count > 0)
                        {
                            t += odc.Sum(q => q.Quantity);
                            total += odc.Sum(q => q.Quantity * q.UnitPrice);
                        }
                    }
                    ltrSTQty.Text = t.ToString();
                    TblJobSheet js = JobManager.SelectJobSheetByID(ind.JobID);
                    if (js != null)
                    {
                        ltrSTCirf.Text = js.FaceWidth.ToString("N2");
                        ltrSTWidth.Text = js.FaceWidth.ToString("N2");
                        ltrSTTotal.Text = ExchangeCurrency((double)total).ToString("N2");
                        grandTotal += (double)total;
                    }
                    ltrSTUnitPrice.Text = ExchangeCurrency((double)(total / (decimal)t)).ToString("N3");
                }

                #endregion steel base

                #region Engraving

                TblCylinderCollection cys = CylinderManager.SelectCylinderByJobID(ind.JobID);
                if (cys != null && cys.Count > 0)
                {
                    ltrEGQty.Text = cys.Count.ToString();
                    double egTotal = cys.Sum(x => x.FaceWidth * x.Circumference * (double)(x.UnitPrice ?? 0) * (double)(1 + (x.TaxPercentage ?? 0) / 100) / 100);
                    ltrEGTotal.Text = ExchangeCurrency(egTotal).ToString("N3");
                    ltrEGUnitPrice.Text = cys.Average(x => x.FaceWidth * x.Circumference * (double)(x.UnitPrice ?? 0) * (double)(1 + (x.TaxPercentage ?? 0) / 100) / 100).ToString("N3");
                    grandTotal += cys.Sum(x => x.FaceWidth * x.Circumference * (double)(x.UnitPrice ?? 0) * (double)(1 + (x.TaxPercentage ?? 0) / 100) / 100);
                }

                #endregion Engraving

                #region Service

                List<ServiceJobDetailExtension> svs = JobManager.SelectServiceJobDetailByID(ind.JobID);
                if (svs != null && svs.Count > 0)
                {
                    ltrSVQty.Text = svs.Count.ToString();
                    ltrSVTotal.Text = ExchangeCurrency(svs.Sum(q => (double)q.WorkOrderValues + q.TaxPercentage ?? 0)).ToString("N3");
                    ltrSVUnitPrice.Text = ExchangeCurrency((svs.Sum(q => (double)q.WorkOrderValues + q.TaxPercentage ?? 0) / svs.Count)).ToString("N3");
                    grandTotal += svs.Sum(q => (double)q.WorkOrderValues + q.TaxPercentage ?? 0);
                }

                #endregion Service

                #region bind other charges

                Repeater rptOtherCharges = e.Item.FindControl("rptOtherCharges") as Repeater;
                List<OtherChargesExtension> odcharges = OrderConfirmationManager.SelectOtherChargeByJobID(j.JobID);
                if (odcharges != null)
                {
                    ltrTrHeading.Text = string.Format("<td rowspan='{0}'>{1}</td>", odcharges.Count + 3, jobInformation);
                    rptOtherCharges.DataSource = odcharges;
                    rptOtherCharges.DataBind();
                }

                #endregion bind other charges
            }
        }

        protected void rptOtherCharges_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            #region e.item.FindControl("Control")

            Literal ltrODChargeDescription = e.Item.FindControl("ltrODChargeDescription") as Literal;
            Literal ltrODChargeQty = e.Item.FindControl("ltrODChargeQty") as Literal;
            Literal ltrODChargeUnitPrice = e.Item.FindControl("ltrODChargeUnitPrice") as Literal;
            Literal ltrODChargeTotal = e.Item.FindControl("ltrODChargeTotal") as Literal;

            #endregion e.item.FindControl("Control")

            TblOtherCharge odcharge = e.Item.DataItem as TblOtherCharge;
            ltrODChargeDescription.Text = odcharge.Description;
            ltrODChargeQty.Text = odcharge.Quantity.ToString();
            ltrODChargeUnitPrice.Text = ExchangeCurrency((double)odcharge.Charge).ToString();
            decimal charge = odcharge.Charge ?? 0;
            int quantity = odcharge.Quantity ?? 1;
            decimal total = quantity * charge;
            ltrODChargeTotal.Text = ExchangeCurrency((double)total).ToString("N3");
            grandTotal += (double)total;
        }

        protected string ShowNumberFormat(object obj)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString("N2") : "0"; }
            return strPrice;
        }

        private double ExchangeCurrency(double rmValue)
        {
            if (CurrencyVoice != null)
            {
                return (rmValue * (double)CurrencyVoice.CurrencyValue) / (double)CurrencyVoice.RMValue;
            }
            return rmValue;
        }
    }
}