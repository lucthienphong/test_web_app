using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintPurchaseOrder : Page
    {
        private int PurID
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

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrCompany.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
            ltrCompanyFax.Text = string.Format("{0} / {1}", SettingManager.GetSettingValue(SettingNames.CompanyPhone), SettingManager.GetSettingValue(SettingNames.CompanyFax));
            ltrCompanyAddress.Text = SettingManager.GetSettingValue(SettingNames.CompanyAddress);

            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName) + "<br />";
            string sCompanyInfo = SettingManager.GetSettingValue(SettingNames.CompanyAddress) + "<br />";
            sCompanyInfo += "Phone " + SettingManager.GetSettingValue(SettingNames.CompanyPhone) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += "Fax " + SettingManager.GetSettingValue(SettingNames.CompanyFax) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += SettingManager.GetSettingValue(SettingNames.CompanyWebsite) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            sCompanyInfo += "GST No.:" + SettingManager.GetSettingValue(SettingNames.CompanyGST) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />";
            sCompanyInfo += "TIN No.:" + SettingManager.GetSettingValue(SettingNames.CompanyGST);

            lblCompanyInfo.Text = sCompanyInfo;

            LoadInformation();
        }

        private void LoadInformation()
        {
            TblPurchaseOrder purOrder = PurchaseOrderManager.SelectPurchaseOrder(PurID.ToString());

            if (purOrder != null)
            {
                ltrOrderNumber.Text = purOrder.OrderNumber;
                ltrOrderDate.Text = purOrder.OrderDate.ToString("dd.MM.yyyy");
                DateTime bDate = purOrder.RequiredDeliveryDate != null ? (DateTime)purOrder.RequiredDeliveryDate : purOrder.OrderDate;
                ltrDeliveryDate.Text = bDate.ToString("dd.MM.yyyy");

                TblSupplier supplier = new SupplierManager().SelectByID(purOrder.SupplierID);

                //ddlSuplier.SelectedValue = supplier.SupplierID.ToString();
                ltrSupplier.Text = string.Format("{0} <br/> {1} <br/> Tel: {2} - Fax: {3} <br/> Attention to: {4}", supplier.Name, supplier.Address, supplier.Tel, supplier.Fax, supplier.ContactPerson);

                TblJob job = JobManager.SelectByID(purOrder.JobID);
                ltrJobDesign.Text = job.Design.ToString();
                ltrJobName.Text = job.JobName.ToString();
                ltrJobNumber.Text = job.JobNumber.ToString();
                TblCurrency cr = new CurrencyManager().SelectByID(purOrder.CurrencyID);
                if (cr != null)
                {
                    ltrCurr.Text = cr.CurrencyName;
                }

                TblUser uObj = UserManager.GetUserByUserName(purOrder.CreatedBy);
                if (uObj != null)
                {
                    TblStaff sObj = StaffManager.SelectByID(uObj.UserID);
                    if (sObj != null)
                    {
                        ltrContact.Text = string.Format("{0} / {1}", sObj.FirstName, sObj.TelNumber);
                        ltrContactEmail.Text = sObj.Email;
                    }
                }
                TblUser user = UserManager.GetUserByUserName(purOrder.CreatedBy);
                if (user != null)
                {
                    TblStaff staff = StaffManager.SelectByID(user.UserID);
                    if (staff != null)
                        ltrMadeBy.Text = string.Format("{0} {1}", staff.LastName, staff.FirstName);
                }

                TblCustomer cus = CustomerManager.SelectByID(job.CustomerID);
                if (cus != null)
                {
                    ltrCustomer.Text = cus.Name;
                }

                ltrUrgent.Text = string.Format("<span class='fa fa-square-o fa-2x'></span>");
                if (purOrder.IsUrgent == 1)
                {
                    ltrUrgent.Text = string.Format("<span class='fa fa-check-square-o fa-2x'></span>");
                }

                ltrRemark.Text = purOrder.Remark.Replace("\n", "<br />");

                TblCylinderCollection cylinders = CylinderManager.GetCylindersByJobID(job.JobID, job != null ? Convert.ToBoolean(job.IsOutsource) : false);
                if (cylinders != null && cylinders.Count() > 0)
                {

                    #region bind cylinder

                    List<TblCylinderCollectionModel> listCylinderModel = new List<TblCylinderCollectionModel>();
                    TblJobSheet jSheet = JobManager.SelectJobSheetByID(job.JobID);
                    int Seq = 1;
                    foreach (var item in cylinders)
                    {
                        TblCylinderCollectionModel c = new TblCylinderCollectionModel();
                        
                        c.objCylinder = item;
                        c.objCylinder.Sequence = Seq;

                        if (item.POQuantity == null)
                            c.objCylinder.POQuantity = 0;

                        TblPurchaseOrderCylinder p = PurchaseOrderManager.SelectPurchaseOrderByJobId_PurchaseID(item.CylinderID, purOrder.PurchaseOrderID);
                        if (p != null)
                        {
                            c.UnitPriceExtension = p.UnitPrice.ToString();
                            c.Quantity = (int)p.Quantity;
                        }
                        else
                        {
                            c.UnitPriceExtension = item.POUnitPrice != null ? item.POUnitPrice.ToString() : "0";
                            c.Quantity = 0;
                        }
                        //elsex
                        //    c.UnitPriceExtension = item.POUnitPrice != null ? item.POUnitPrice.ToString() : "0";

                        //c.Total = item.POQuantity != null && !string.IsNullOrEmpty(c.UnitPriceExtension) ? (int)item.POQuantity * decimal.Parse(c.UnitPriceExtension) : 0;

                        c.Total = c.Quantity * decimal.Parse(c.UnitPriceExtension);
                        c.CylinderType = jSheet.TypeOfCylinder;
                        listCylinderModel.Add(c);
                        Seq++;
                    }

                    ltrCylinderType.Text = jSheet.TypeOfCylinder;

                    gvClinders.DataSource = listCylinderModel;

                    gvClinders.DataBind();

                    #endregion bind cylinder
                }
            }
        }

        protected void gvClinders_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // add the UnitPrice and QuantityTotal to the running total variables
                quantityTotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Quantity"));
                priceTotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Total"));
            }
            else
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 6;
                    e.Row.Cells[0].Text = string.Format("<strong>{0}</strong>", "Total");
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                    e.Row.Cells[4].Text = quantityTotal.ToString("d");
                    //e.Row.Cells[6].Text = priceTotal.ToString("N2");
                    e.Row.Cells[6].Visible = false;
                    //e.Row.Cells[7].Visible = false;
                    //e.Row.Cells[8].Visible = false;

                    //e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    // for the Footer, display the running totals
                    //e.Row.Cells[6].Text = quantityTotal.ToString("d");
                }
        }

        public int quantityTotal = 0;

        public int priceTotal = 0;
    }
}