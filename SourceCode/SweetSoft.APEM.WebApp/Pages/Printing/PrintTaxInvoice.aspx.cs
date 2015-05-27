using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintTaxInvoice : Page
    {
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
        public TblTax taxRate = new TblTax();
        private decimal subTotalBeforeGST = 0;
        private decimal subTotalBeforeGSTMY = 0;
        private decimal subTotalWithGST = 0;
        private decimal subTotalWithGSTMY = 0;
        public string CurrencyName;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadInformation();
        }
        public TblInvoice iv = new TblInvoice();
        TblCurrency cr = new TblCurrency();
        private void LoadInformation()
        {

            iv = InvoiceManager.SelectByID(InvoiceID);

            if (iv != null)
            {
                #region invoice information
                ltrInvoiceNumber.Text = iv.InvoiceNo;
                ltrInvoiceDate.Text = iv.InvoiceDate.ToString("dd.MM.yyyy");
                #endregion


                #region Customer information
                TblCustomer cust = CustomerManager.SelectByID(iv.CustomerID);
                if (cust != null)
                {
                    ltrCustomerName.Text = cust.Name;
                    string customerCountry = string.Empty;
                    TblReference refer = ReferenceTableManager.SelectByID(cust.CountryID);
                    if (refer != null)
                    {
                        customerCountry = refer.Name;
                    }
                    ltrCustomerAddress.Text = string.Format("{0}<br/>{1}, {2}<br/>{3}<br/>", cust.Address, cust.PostCode, cust.City, customerCountry);
                    ltrSAPCode.Text = cust.SAPCode;
                    //ltrGSTID.Text = string.Format("<strong>GST ID</strong>: {0}<br/><strong>TIN No</strong>: {1}", cust.TaxCode, cust.Tin);
                    ltrGSTID.Text = string.Format("<strong>GST No.</strong>: {0}", cust.TaxCode);

                    refer = new TblReference();
                    refer = ReferenceTableManager.SelectByID(cust.PaymentID ?? -1);
                    if (refer != null)
                    {
                        ltrPaymentTerms.Text = refer.Name;
                    }

                    refer = new TblReference();
                    refer = ReferenceTableManager.SelectByID(cust.DeliveryID ?? -1);
                    if (refer != null)
                    {
                        ltrDeliveryTerm.Text = refer.Name;
                    }

                }
                #endregion
                cr = new CurrencyManager().SelectByID(iv.CurrencyID);
                if (cr != null)
                {
                    ltrExchangeRate.Text = cr.RMValue.ToString();
                    CurrencyName = cr.CurrencyName;
                }
                #region Print Job Template
                TblJobCollection jCol = JobManager.SelectJobByInvoiceID(InvoiceID);
                if (jCol != null && jCol.Count > 0)
                {
                    rptTemplateJob.DataSource = jCol;
                    rptTemplateJob.DataBind();

                    TblJob j = jCol.First();
                    TblCustomer cusShip = CustomerManager.SelectByID(j.ShipToParty ?? -1);
                    if (cusShip != null)
                    {
                        TblReference country = ReferenceTableManager.SelectByID(cusShip.CountryID);
                        string countryName = country != null ? country.Name : string.Empty;
                        string ShipToAddress = string.Format("{0}<br/>{1}<br/>{2}, {3}<br/>{4}", cusShip.Name, cusShip.Address, cusShip.PostCode, cusShip.City, countryName);
                        ltrShipToAddress.Text = ShipToAddress;
                    }
                }
                #endregion

                if (jCol.Count == 1)
                {
                    pnlSubTotal.Visible = false;
                }
                else
                {
                    ltrSubTotalBeforeGST.Text = subTotalBeforeGST.ToString("N2");
                    ltrTotalInvoiceWithGST.Text = subTotalWithGST.ToString("N2");
                    ltrFinalAmount.Text = (subTotalBeforeGST + subTotalWithGST).ToString("N2");

                    ltrSubTotalBeforeGSTMY.Text = subTotalBeforeGSTMY.ToString("N2");
                    ltrTotalInvoiceWithGSTMY.Text = subTotalWithGSTMY.ToString("N2");
                    ltrFinalAmountMY.Text = (subTotalBeforeGSTMY + subTotalWithGSTMY).ToString("N2");
                }
                ltrRemark.Text = iv.Remark.Replace("\n", "<br/>");
            }

        }

        protected void rptTemplateJob_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            #region Find Control
            Literal ltrJobName = e.Item.FindControl("ltrJobName") as Literal;
            Literal ltrJobDesign = e.Item.FindControl("ltrJobDesign") as Literal;
            Literal ltrDONumber = e.Item.FindControl("ltrDONumber") as Literal;
            //Literal ltrDODate = e.Item.FindControl("ltrDODate") as Literal;
            Literal ltrJobNumber = e.Item.FindControl("ltrJobNumber") as Literal;
            //Literal ltrODDate = e.Item.FindControl("ltrODDate") as Literal;
            Label lblSubTotal = e.Item.FindControl("lblSubTotal") as Label;
            //Label lblSubTotalMY = e.Item.FindControl("lblSubTotalMY") as Label;
            Literal ltrDiscountRate = e.Item.FindControl("ltrDiscountRate") as Literal;
            Label lblDiscount = e.Item.FindControl("lblDiscount") as Label;
            //Label lblDiscountMY = e.Item.FindControl("lblDiscountMY") as Label;
            Label lblSubTotalBefore = e.Item.FindControl("lblSubTotalBefore") as Label;
            //Label lblSubTotalBeforeMY = e.Item.FindControl("lblSubTotalBeforeMY") as Label;
            Literal ltrTaxRate = e.Item.FindControl("ltrTaxRate") as Literal;
            Label lblGST = e.Item.FindControl("lblGST") as Label;
            //Label lblGSTMY = e.Item.FindControl("lblGSTMY") as Label;
            Label lblTotal = e.Item.FindControl("lblTotal") as Label;
            //Label lblTotalMY = e.Item.FindControl("lblTotalMY") as Label;
            Repeater rptCylinder = e.Item.FindControl("rptCylinder") as Repeater;
            Repeater rptOtherCharges = e.Item.FindControl("rptOtherCharges") as Repeater;
            #endregion

            TblJob job = e.Item.DataItem as TblJob;
            #region Job Information
            ltrJobName.Text = job.JobName;
            ltrJobDesign.Text = job.Design;
            TblDeliveryOrder od = DeliveryOrderManager.SelectDeliveryOrderByJobID(job.JobID);
            if (od != null)
            {
                ltrDONumber.Text = string.Format("{0} / {1}", od.DONumber, od.OrderDate.ToString("dd.MM.yyyy"));
                //ltrDODate.Text = od.OrderDate.ToString("dd.MM.yyyy");
            }
            ltrJobNumber.Text = string.Format("{0} / {1}", job.JobNumber, ((DateTime)job.CreatedOn).ToString("dd.MM.yyyy"));
            //if (job.CreatedOn != null)
            //{
            //    DateTime a = (DateTime)job.CreatedOn;
            //    ltrODDate.Text = a.ToString("dd.MM.yyyy");
            //}
            #endregion

            DataTable dt = CylinderManager.SelectCylinderSelectForOrderConfirmation(job.JobID);
            if (dt != null)
            {
                decimal sumTotalPrice = 0;
                decimal sumTotalPriceMY = 0;
                decimal discountPer = 0;
                decimal discountReal = 0;
                decimal discountRealMY = 0;
                List<CylinderExtension> listCylinder = new List<CylinderExtension>();

                TblOrderConfirmation _od = OrderConfirmationManager.SelectByID(job.JobID);
                if (_od != null)
                    discountPer = (decimal)(_od.Discount.HasValue ? _od.Discount.Value : 0);

                taxRate = new TaxManager().SelectByID(iv.TaxID ?? -1);

                foreach (DataRow dr in dt.Rows)
                {
                    CylinderExtension c = new CylinderExtension()
                    {
                        No = dr["Sequence"].ToString(),
                        Description = dr["CylDescription"].ToString(),
                        CylinderID = dr["CylinderNo"].ToString(),
                        Width = dr["FaceWidth"].ToString(),
                        Cirf = dr["Circumference"].ToString(),
                        UnitPrice = decimal.Parse(dr["UnitPrice"].ToString()),
                        Qty = int.Parse(dr["Quantity"].ToString()),
                        TotalPrice = decimal.Parse(dr["TotalPrice"].ToString()),
                        //TotalPriceMY = decimal.Parse(dr["TotalPrice"].ToString()) * cr.RMValue,
                        CylBarcode = dr["CylBarcode"].ToString(),
                        CusCylID = dr["CusCylID"].ToString(),
                        CusSteelBaseID = dr["CusSteelBaseID"].ToString(),
                        SteelBase = dr["SteelBaseName"].ToString()
                    };

                    if (taxRate != null)
                    {
                        c.TaxRate = taxRate.TaxPercentage.ToString();
                    }
                    listCylinder.Add(c);
                    sumTotalPrice += c.TotalPrice;
                    sumTotalPriceMY += c.TotalPriceMY;
                }
                if (listCylinder.Count > 0)
                {
                    rptCylinder.DataSource = listCylinder;
                    rptCylinder.DataBind();
                }

                //Other Charges
                int ContinueSequence = 0;
                listCylinder = new List<CylinderExtension>();
                List<OtherChargesExtension> OtherCharges = OrderConfirmationManager.SelectOtherChargeByJobID(job.JobID);
                foreach (OtherChargesExtension item in OtherCharges)
                {
                    ContinueSequence++;
                    CylinderExtension c = new CylinderExtension()
                    {
                        No = ContinueSequence.ToString(),
                        Description = item.Description,
                        CylinderID = string.Empty,
                        Width = string.Empty,
                        Cirf = string.Empty,
                        UnitPrice = (decimal)item.Charge,
                        Qty = (int)item.Quantity,
                        TotalPrice = (decimal)item.Charge * (decimal)item.Quantity,
                        //TotalPriceMY = (decimal)item.Charge * (decimal)item.Quantity * cr.RMValue,
                        CusCylID = string.Empty,
                        CylBarcode = string.Empty,
                        CusSteelBaseID = string.Empty
                    };

                    if (taxRate != null)
                    {
                        c.TaxRate = taxRate.TaxPercentage.ToString();
                    }
                    listCylinder.Add(c);
                    sumTotalPrice += c.TotalPrice;
                    sumTotalPriceMY += c.TotalPriceMY;
                }

                if (listCylinder.Count>0)
                {
                    rptOtherCharges.Visible = true;
                    rptOtherCharges.DataSource = listCylinder;
                    rptOtherCharges.DataBind();
                }

                discountReal = discountPer * sumTotalPrice / 100;
                discountRealMY = discountPer * sumTotalPriceMY / 100;

                if (taxRate != null)
                {
                    ltrTaxRate.Text = ((decimal)taxRate.TaxPercentage).ToString("N2");
                    lblGST.Text = (((sumTotalPrice - discountReal) * (decimal)taxRate.TaxPercentage) / 100).ToString("N2");
                    //lblGSTMY.Text = (((sumTotalPriceMY - discountRealMY) * (decimal)taxRate.TaxPercentage) / 100).ToString("N2");
                    subTotalWithGST += (((sumTotalPrice - discountReal) * (decimal)taxRate.TaxPercentage) / 100);
                    subTotalWithGSTMY += (((sumTotalPriceMY - discountRealMY) * (decimal)taxRate.TaxPercentage) / 100);
                }

                ltrDiscountRate.Text = discountPer.ToString("N2") + "%";
                lblDiscount.Text = discountReal.ToString("N2");
                //lblDiscountMY.Text = discountRealMY.ToString("N2");

                lblSubTotal.Text = sumTotalPrice.ToString();
                //lblSubTotalMY.Text = sumTotalPriceMY.ToString("N2");

                lblSubTotalBefore.Text = (sumTotalPrice - discountReal).ToString("N2");
                subTotalBeforeGST += (sumTotalPrice - discountReal);

                //lblSubTotalBeforeMY.Text = (sumTotalPriceMY - discountReal).ToString("N2");
                //subTotalBeforeGSTMY += (sumTotalPriceMY - discountReal);

                lblTotal.Text = ((sumTotalPrice - discountReal) + ((sumTotalPrice - discountReal) * (decimal)taxRate.TaxPercentage / 100)).ToString("N2");
                //lblTotalMY.Text = ((sumTotalPriceMY - discountRealMY) + ((sumTotalPriceMY - discountRealMY) * (decimal)taxRate.TaxPercentage / 100)).ToString("N2");



            }
        }

        protected string ShowNumberFormat(object obj, string format, string otherCharater)
        {
            string strPrice = string.Empty;
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                decimal price = 0;
                decimal.TryParse(obj.ToString(), out price);
                strPrice = price > 0 ? string.Format("{0}{1}", price.ToString(format), otherCharater) : "";
            }
            return strPrice;
        }

        protected string ShowSteelBase(object obj)
        {
            string SteelBase = string.Empty;
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                bool steel = false;
                bool.TryParse(obj.ToString(), out steel);
                SteelBase = steel ? "New" : "Old";
            }
            return SteelBase;
        }
    }
}
public class CylinderExtension
{
    public string No { get; set; }
    public string Description { get; set; }
    public string CylinderID { get; set; }
    public string Width { get; set; }
    public string Cirf { get; set; }
    public string TaxRate { get; set; }
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalPriceMY { get; set; }
    public string CylBarcode { get; set; }
    public string CusCylID { get; set; }
    public string CusSteelBaseID { set; get; }
    public string SteelBase { set; get; }
}
