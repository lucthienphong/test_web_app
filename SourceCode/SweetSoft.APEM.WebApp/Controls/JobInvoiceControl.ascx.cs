using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.CMS.DataAccess;

namespace SweetSoft.APEM.WebApp.Controls
{
    public partial class JobInvoiceControl : System.Web.UI.UserControl
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        private JobInvoice jobInvoice;
        public void LoadData(JobInvoice item)
        {
            int jobID = 0;
            string jobNumber = string.Empty;
            decimal total =0;
            decimal priceTaxed =0;
            decimal priceDiscount = 0;

            if (item != null)
            {
                hdfJobID.Value = item.JobID.ToString();
                jobInvoice = item;
                txtCustomerPO1.Text = item.CustomerPO1;
                txtCustomerPO2.Text = item.CustomerPO2;
                txtDesignValue.Text = item.JobDesign;
                txtJobNameValue.Text = item.JobName;
                jobID = item.JobID;
                jobNumber = item.JobNumber + "(REV " + item.JobRev + ")";

                total = item.ServiceJobTotalPrice + item.OtherChargesTotalPrice + item.CylinderTotalPrice;
                //Trừ discount
                priceDiscount = total * (1 - (decimal)item.Discount / 100);
                priceTaxed = priceDiscount * (1 + (decimal)item.TaxRate / 100);

                decimal netTotal = item.NetTotalCylinderPrice + item.NetTotalOtherChargesPrice + item.NetTotalServicePrice;
                txtTotal.Text = total.ToString("N2");
                txtNetTotal.Text = priceTaxed.ToString("N2");//netTotal.ToString("N2");

                LoadCylinders(item.CylinderDataSource);
                LoadOtherCharges(item.OtherChargesDataSource);
                LoadServiceJobDetail(item.ServiceJobDataSource);
            }

            string title = string.Format("{0}: {1} | {2}: {3} | {4}: {5}",
                "Sub Total", total.ToString("N2"),
                "Sub Total before GST", priceDiscount.ToString("N2"),
                "Final amount", priceTaxed.ToString("N2"));

            ltrCollapTitle.Text = string.Format("<h4 style='font-size: 14px;font-weight: bold;margin-top: 0px !important;margin-bottom: 0px !important;' data-parent='#accordion' href='#job-invoice-{0}' aria-expanded='true' aria-controls='collapseOne' data-toggle='collapse'><a  style='text-decoration: none' >{1}{2}</a>&nbsp;&nbsp;<small>{3}</small> <a class='pull-right' href='javascript:;' style='color: #428bca;' onclick='DoRemoveJob({0}); return false;'><span style='font-size: 18px;' class='glyphicon glyphicon-remove'></span></a></h4>", item.JobID,
                ResourceTextManager.GetApplicationText(ResourceText.JOB_NUMBER) + ":", jobNumber, title);
            ltrCollapseBody.Text = string.Format("<div id='job-invoice-{0}' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingOne'>",jobID);
        }


        private void LoadCylinders(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                gvClinders.DataSource = dt;
                gvClinders.DataBind();
            }
            else
                divCylinder.Visible = false;
        }

        private void LoadOtherCharges(TblOtherChargeCollection coll)
        {
            if (coll != null && coll.Count > 0)
            {
                grvOtherCharges.DataSource = coll;
                grvOtherCharges.DataBind();
            }
            else
                divOtherCharger.Visible = false;
        }

        private void LoadServiceJobDetail(TblServiceJobDetailCollection coll)
        {
            if (coll != null && coll.Count > 0)
            {
                grvServiceJobDetail.DataSource = coll;
                grvServiceJobDetail.DataBind();
            }
            else
                divJobService.Visible = false;
        }

        protected string ShowNumberFormat(object obj)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString("N2") : "0"; }
            return strPrice;
        }

        #region Service job detail

       
        private void BindServiceJobDetail()
        {
            TblServiceJobDetailCollection coll = (TblServiceJobDetailCollection)Session[ViewState["PageID"] + "ServiceJobDetail"];

            grvServiceJobDetail.DataSource = coll;
            grvServiceJobDetail.DataBind();
        }

        protected decimal TotalPrice = 0;
        private void LoadServiceJobDetail(int jobID)
        {
            List<ServiceJobDetailExtension> coll = JobManager.SelectServiceJobDetailByID(jobID);
            grvServiceJobDetail.DataSource = coll;

            grvServiceJobDetail.DataBind();
            Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
        }

        private string item1 = string.Empty;
        private string item2 = string.Empty;
        private int count = 1;
        protected void grvServiceJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TblServiceJobDetail item = e.Row.DataItem as TblServiceJobDetail;
                if (item != null)
                {
                    Label lblNo = e.Row.FindControl("lblNo") as Label;
                    if (lblNo != null)
                    {
                        if (!item.WorkOrderNumber.Equals(item1) || !item.ProductID.Equals(item2))
                        { lblNo.Text = count.ToString(); count++; }
                        else
                            lblNo.Text = string.Empty;
                    }
                    item1 = item.WorkOrderNumber;
                    item2 = item.ProductID;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalPrice = e.Row.FindControl("lblTotalPrice") as Label;
                if (lblTotalPrice != null) lblTotalPrice.Text = CalculatorPrice().ToString("N2");
            }
        }

        private decimal CalculatorPrice()
        {
            decimal totalPrice = 0;
            TblServiceJobDetailCollection coll = (TblServiceJobDetailCollection)Session[ViewState["PageID"] + "ServiceJobDetail"];
            if (coll != null && coll.Count > 0)
            {
                totalPrice = coll.Sum(x => x.WorkOrderValues);
            }
            return totalPrice;
        }

   
        protected void grvServiceJobDetail_DataBound(object sender, EventArgs e)
        {
            for (int i = grvServiceJobDetail.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = grvServiceJobDetail.Rows[i];
                GridViewRow previousRow = grvServiceJobDetail.Rows[i - 1];

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

        #endregion 
       
    }
}