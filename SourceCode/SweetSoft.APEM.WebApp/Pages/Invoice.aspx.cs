using SubSonic;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using SweetSoftCMS.ExtraControls.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SweetSoft.CMS.DataAccess;
using SweetSoft.APEM.WebApp.Controls;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Text;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class Invoice : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "invoice_manager";
            }
        }

        private int InvoiceID
        {
            get
            {
                int mInvoiceID = 0;
                if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"].ToString(), out mInvoiceID);

                if (mInvoiceID == 0)
                {
                    if (Session[ViewState["PageID"] + "SweetSoft_InvoiceID"] != null)
                    {
                        int.TryParse(Session[ViewState["PageID"] + "SweetSoft_InvoiceID"].ToString(), out mInvoiceID);
                    }
                }

                return mInvoiceID;

            }
            set { }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            txtTotalPrice.ReadOnly = true;
            txtNetTotal.ReadOnly = true;

            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                Session[ViewState["PageID"] + "SweetSoft_InvoiceID"] = string.Empty;
                BindTaxForDDL();
                LoadData();
                ltrView.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintTaxInvoice.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-eye110'></span> View</a>", InvoiceID);
                ltrView.Visible = true;

                ////Kiểm tra invoice có bị khóa không?
                ////Nếu khóa thì không cho edit hay xóa invoice
                //if(OrderLockingManager.CheckLockingStatus(InvoiceID, OrderLockingType.Invoice))
                //{
                //    AllowEditting(false);
                //}
                ////Ngược lại nếu invoice không khóa
                ////Nếu người dùng không có quyền đặc biệt thì không cho edit hay xóa invoice
                //else if (!RoleManager.AllowUpdateStatus(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                //{
                //    AllowEditting(false);
                //}
            }
            else
            {
                ltrPrint.Text = string.Format(@"<div class='btn-group'>
                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                            <span class='flaticon-printer60'></span> Print <span class='caret'></span>
                        </button>
                        <ul class='dropdown-menu openPrinting' role='menu'>
                            <li>
                                <a href='#' data-href='Printing/PrintTaxInvoicePDF.aspx?ID={0}'>Tax Invoice</a>
                            </li>
                            <li>
                                <a href='#' data-href='Printing/PrintTaxInvoiceServicePDF.aspx?ID={0}'>Service Job Invoice</a>
                            </li>
                        </ul>
                        </div>", InvoiceID);
            }

        }

        private void BindTaxForDDL()
        {
            List<TblTax> list = new TaxManager().SelectAllForDDL(true);
            ddlTax.DataSource = list;
            ddlTax.DataTextField = "TaxCode";
            ddlTax.DataValueField = "TaxID";
            ddlTax.DataBind();
        }

        private void BindContact(int CustomerID)
        {
            ddlContact.Items.Clear();
            var coll = ContactManager.SelectAllByCustomerID(CustomerID);
            if (coll != null && coll.Count > 0)
            {
                //ddlContact.Items.Add(new ListItem(ResourceTextManager.GetApplicationText(ResourceText.CHOOSE_CONTACT), ""));
                foreach (var item in coll)
                {
                    ListItem _item = new ListItem(item.ContactName, item.ContactID.ToString());
                    ddlContact.Items.Add(_item);
                }
            }
        }

        private void LoadData()
        {
            TblInvoice invoice = InvoiceManager.SelectByID(InvoiceID);
            if (invoice != null)
            {
               
                txtName.ReadOnly = true;
                TblCustomer customer = CustomerManager.SelectByID(invoice.CustomerID);
                if (customer != null)
                {
                    txtCode.Text = customer.Code;
                    txtName.Text = customer.Name;

                    BindContact(customer.CustomerID);
                    BindDeliveryOrder(customer.CustomerID);
                }
                txtYourReference.Text = invoice.PONumber;
                txtInvoiceNumber.Text = invoice.InvoiceNo;
                txtInvoiceDate.Text = invoice.InvoiceDate.ToString("dd/MM/yyyy");
                if (ddlContact.Items.FindByValue(invoice.ContactID.ToString()) != null)
                    ddlContact.Items.FindByValue(invoice.ContactID.ToString()).Selected = true;

                txtRemark.Text = invoice.Remark;
                //txtRemarkScreen.Text = invoice.RemarkScreen;
                txtPaymentTerms.Text = invoice.PaymentTern;

                TblCurrency cObj = new CurrencyManager().SelectByID(invoice.CurrencyID);
                if (cObj != null)
                {
                    ltrCurrency.Text = cObj.CurrencyName;
                    txtRMValue.Text = invoice.RMValue.ToString("N4");
                }

                ddlTax.SelectedValue = invoice.TaxID != null ? invoice.TaxID.ToString() : "0";

                ltrPrint.Text = string.Format(@"<div class='btn-group'>
                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                            <span class='flaticon-printer60'></span> Print <span class='caret'></span>
                        </button>
                        <ul class='dropdown-menu openPrinting' role='menu'>
                            <li>
                                <a href='#' data-href='Printing/PrintTaxInvoicePDF.aspx?ID={0}'>Tax Invoice</a>
                            </li>
                            <li>
                                <a href='#' data-href='Printing/PrintTaxInvoiceServicePDF.aspx?ID={0}'>Service Job Invoice</a>
                            </li>
                        </ul>
                        </div>", invoice.InvoiceID);

                /// Trunglc Add - 23-04-2015
                /// 
                bool IsLocking = InvoiceManager.IsInvoiceLocking(InvoiceID);
                bool IsNewInvoice = InvoiceManager.IsNewInvoice(InvoiceID);
                bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
                bool IsAllowEdit = IsNewInvoice ? true : (!IsLocking ? true : (IsLocking ? false : true));

                AllowEditting(IsAllowEdit);
            }
            else
            {
                txtInvoiceNumber.Text = CreateOrderbNumber();
                txtInvoiceDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                BindContact(0);
                btnDelete.Visible = false;
            }


            BindJobInvoices();

        }

        private void BindJobInvoices()
        {
            List<JobInvoice> list = new List<JobInvoice>();
            TblInvoiceDetailCollection coll = InvoiceManager.SelectInvoiceDetailByInvoiceId(InvoiceID);
            if (coll != null && coll.Count > 0)
            {
                foreach (var item in coll)
                {
                    TblJob job = JobManager.SelectByID(item.JobID);
                    if (job != null)
                    {
                        TblOrderConfirmation orderConfirm = OrderConfirmationManager.SelectByID(job.JobID);
                        if (orderConfirm != null)
                        {
                            JobInvoice jobInvoice = new JobInvoice();
                            jobInvoice.JobID = job.JobID;
                            jobInvoice.CustomerPO1 = orderConfirm.CustomerPO1;
                            jobInvoice.CustomerPO2 = orderConfirm.CustomerPO2;
                            jobInvoice.OrderConfirmNo = orderConfirm.OCNumber;
                            jobInvoice.Discount = orderConfirm.Discount.HasValue ? orderConfirm.Discount.Value : 0;

                            TblContact contact = ContactManager.SelectByID(orderConfirm.ContactPersonID);
                            if (contact != null)
                                jobInvoice.ContactPerson = contact.ContactName;

                            jobInvoice.JobNumber = job.JobNumber;
                            jobInvoice.JobRev = job.RevNumber;
                            jobInvoice.JobName = job.JobName;
                            jobInvoice.JobDesign = job.Design;


                            jobInvoice.Discount = orderConfirm.Discount.HasValue ? orderConfirm.Discount.Value : 0;
                            jobInvoice.TaxRate = orderConfirm.TaxPercentage.HasValue ? orderConfirm.TaxPercentage.Value : 0;

                            //Cylinder
                            decimal totalPriceCylinder = 0;
                            DataTable dt = CylinderManager.SelectCylinderSelectForOrderConfirmation(job.JobID);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                decimal.TryParse(dt.Rows[0]["Total"].ToString(), out totalPriceCylinder);
                            }

                            jobInvoice.CylinderTotalPrice = totalPriceCylinder;
                            jobInvoice.CylinderDataSource = dt;
                            jobInvoice.NetTotalCylinderPrice = totalPriceCylinder * (1 - (decimal)(orderConfirm.Discount.HasValue ? orderConfirm.Discount : 0)/100);

                            //Other changer
                            decimal totalPriceOtherCharges = 0;
                            List<OtherChargesExtension> collOtherCharges = OrderConfirmationManager.SelectOtherChargeByJobID(job.JobID);
                            TblOtherChargeCollection collCharger = new TblOtherChargeCollection();
                            foreach (TblOtherCharge items in collOtherCharges)
                            {
                                collCharger.Add(items);
                            }
                            jobInvoice.OtherChargesDataSource = collCharger;
                            if (collCharger != null && collCharger.Count > 0)
                            {
                                foreach (var item1 in collCharger)
                                {
                                    totalPriceOtherCharges = totalPriceOtherCharges + (item1.Quantity.HasValue ? item1.Quantity.Value : 0) * (item1.Charge.HasValue ? item1.Charge.Value : 0);
                                }
                            }
                            jobInvoice.OtherChargesTotalPrice = totalPriceOtherCharges;
                            jobInvoice.NetTotalOtherChargesPrice = totalPriceOtherCharges * (1 - (decimal)(orderConfirm.Discount.HasValue ? orderConfirm.Discount.Value : 0)/100);


                            //Service job
                            decimal totalPriceServiceJob = 0;
                            List<ServiceJobDetailExtension> collAdditionalService = JobManager.SelectServiceJobDetailByID(job.JobID);
                            TblServiceJobDetailCollection collJobDetail = new TblServiceJobDetailCollection();
                            foreach (TblServiceJobDetail dObj in collAdditionalService)
                                collJobDetail.Add(dObj);

                            jobInvoice.ServiceJobDataSource = collJobDetail;
                            if (collJobDetail != null && collJobDetail.Count > 0)
                            {
                                foreach (var item2 in collJobDetail)
                                    totalPriceServiceJob = totalPriceServiceJob + item2.WorkOrderValues;
                            }
                            jobInvoice.ServiceJobTotalPrice = totalPriceServiceJob;
                            jobInvoice.NetTotalServicePrice = totalPriceServiceJob * (1 - (decimal)(orderConfirm.Discount.HasValue ? orderConfirm.Discount.Value : 0) / 100);
                            
                            list.Add(jobInvoice);
                        }
                    }
                }
            }

            Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] = list;
            LoadJobInvoices();
            txtTotalPrice.Text = CalculatorPrice().ToString("N2");
            txtNetTotal.Text = CalculatorNetTotalPrice().ToString("N2");
        }

        private void AllowEditting(bool yesno)
        {
            //Job detail
            ddlContact.Enabled = yesno;
            txtInvoiceDate.Enabled = yesno;
            txtRMValue.Enabled = yesno;
            txtRemark.Enabled = yesno;
            txtYourReference.Enabled = yesno;
            txtPaymentTerms.Enabled = yesno;

            if (divJobNumber.Visible)
                divJobNumber.Visible = yesno;

            btnSave.Visible = yesno;
            btnDelete.Visible = yesno;
            //btnEngraving.Visible = yesno;
            //btnCancel.Visible = yesno;
            //btnSaveRevDetail.Visible = yesno;
            //btnGetCopy.Visible = yesno;

            ///Trunglc Add - 22-04-2015
            ///

            bool IsAllowLock = RoleManager.AllowLock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
            bool IsAllowUnlock = RoleManager.AllowUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
            bool IsNewInvoice = InvoiceManager.IsNewInvoice(InvoiceID);
            bool IsInvoiceLocking = InvoiceManager.IsInvoiceLocking(InvoiceID);

            if (IsNewInvoice)
            {
                btnLock.Visible = IsAllowLock ? true : false;
                btnUnlock.Visible = false;
            }
            else
            {
                btnLock.Visible = IsNewInvoice && IsAllowLock ? true : ((!IsInvoiceLocking && IsAllowLock && yesno ? true : false));
                btnUnlock.Visible = IsInvoiceLocking && IsAllowUnlock && !yesno ? true : false;
            }

            btnDelete.Visible = IsInvoiceLocking ? false : true;

            ///End
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsNewInvoice = InvoiceManager.IsNewInvoice(InvoiceID);
            bool IsLocking = InvoiceManager.IsInvoiceLocking(InvoiceID);

            if (!IsNewInvoice && !IsLocking)
            {
                if (!RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }
            }
            SaveData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("InvoiceList.aspx");
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                int CustomerID = 0;
                int.TryParse(hCustomerID.Value, out CustomerID);

                TblCustomer cust = CustomerManager.SelectByID(CustomerID);
                if (cust != null)
                {
                    hCustomerID.Value = cust.CustomerID.ToString();
                    txtName.Text = cust.Name;
                    txtCode.Text = cust.Code;

                    //Thay Your Reference = Delivery Term//Vì YourReference không dùng đến, chỉ dùng trong chi tiết job
                    txtPaymentTerms.Text = cust.PaymentID != 0 ? cust.PaymentNote : string.Empty;
                    txtYourReference.Text = cust.DeliveryID != 0 ? cust.DeliveryNote : string.Empty;

                    BindDeliveryOrder(cust.CustomerID);

                    //LoadJobNumber(cust.CustomerID);
                    BindContact(cust.CustomerID);
                }
            }
        }

        private void BindDeliveryOrder(int customerID)
        {
            TblDeliveryOrderCollection coll = DeliveryOrderManager.SelectDeliveryOrderByCustomerID(customerID);
            Session[ViewState["PageID"] + "SweetSoft-DeliveryOrder-List"] = coll;
            Session[ViewState["PageID"] + "SweetSoft-JobID-List"] = null;
            ddlDeliveryOrder.Items.Clear();
            ddlDeliveryOrder.Items.Add(new ListItem("--Select delivery order--", "0"));
            if (coll != null && coll.Count > 0)
            {
                divJobNumber.Visible = true;
                foreach (var item in coll)
                {
                    ddlDeliveryOrder.Items.Add(new ListItem(item.DONumber, item.JobID.ToString()));
                }
            }
            else
                divJobNumber.Visible = false;
        }

        private void LoadFromSession()
        {
            divJobNumber.Visible = false;
            txtJobName.Text = "";
            txtDesign.Text = "";
            txtJobNumber.Text = "";
            hdfJobIDTemp.Value = "";

            List<int> exIDs = null;
            if (Session[ViewState["PageID"] + "SweetSoft-JobID-List"] != null)
                exIDs = Session[ViewState["PageID"] + "SweetSoft-JobID-List"] as List<int>;

            ddlDeliveryOrder.Items.Clear();
            ddlDeliveryOrder.Items.Add(new ListItem("--Select delivery order--", "0"));
            if (Session[ViewState["PageID"] + "SweetSoft-DeliveryOrder-List"] != null)
            {
                TblDeliveryOrderCollection coll = Session[ViewState["PageID"] + "SweetSoft-DeliveryOrder-List"] as TblDeliveryOrderCollection;
                if (coll != null && coll.Count > 0)
                {
                    foreach (var item in coll)
                    {
                        if (exIDs != null && exIDs.Count > 0)
                        {
                            if (!exIDs.Contains(item.JobID))
                            {
                                divJobNumber.Visible = true;
                                ddlDeliveryOrder.Items.Add(new ListItem(item.DONumber, item.JobID.ToString()));
                            }
                        }
                        else
                        {
                            if (btnSave.Enabled)
                            {
                                divJobNumber.Visible = true;
                                ddlDeliveryOrder.Items.Add(new ListItem(item.DONumber, item.JobID.ToString()));
                            }
                        }
                    }
                }
            }   
        }

        protected void ddlDeliveryOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            int jobID = 0; int.TryParse(ddlDeliveryOrder.SelectedValue, out jobID);
            TblJob j = JobManager.SelectByID(jobID);
            if (j != null)
            {
                hdfJobIDTemp.Value = j.JobID.ToString();
                txtJobName.Text = j.JobName;
                txtDesign.Text = j.Design;
                txtJobNumber.Text = j.JobNumber + "(Rev " + j.RevNumber + ")";
                TblOrderConfirmation objOC = OrderConfirmationManager.SelectByID(jobID);

                if (objOC != null)
                {
                    txtRemark.Text = objOC.Remark;
                }
            }
            else
            {
                hdfJobIDTemp.Value = string.Empty;
                txtJobName.Text = string.Empty;
                txtDesign.Text = string.Empty;
                txtJobNumber.Text = string.Empty;
            }
            upnlJobRev.Update();
        }
       
        public static string CreateOrderbNumber()
        {
            string _No = "1" + DateTime.Today.ToString("yy") + "3";
            string _MaxNumber = new Select(Aggregate.Max(TblInvoice.InvoiceNoColumn))
                .From(TblInvoice.Schema)
                .Where(TblInvoice.InvoiceNoColumn).Like(_No + "%")
                .ExecuteScalar<string>();
            _MaxNumber = _MaxNumber ?? "";
            if (_MaxNumber.Length > 0)
            {
                string _righ = (int.Parse(_MaxNumber.Substring(_MaxNumber.Length - 5, 5)) + 1).ToString();
                while (_righ.Length < 5)
                    _righ = "0" + _righ;
                _No += _righ;
            }
            else
                _No += "00001";
            return _No;
        }

        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

     
        private void SaveData()
        {
            short? taxID = (short?)null;
            int invoiceId = 0;
            int customerId =0; int.TryParse(hCustomerID.Value, out customerId);
            int contactId = 0; int.TryParse(ddlContact.SelectedValue, out contactId);
            short CurrencyID = 0; short.TryParse(hCurrencyID.Value, out CurrencyID);
            if (CurrencyID == 0)
            {
                if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
                {
                    List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                    if (list != null && list.Count > 0)
                    {
                        JobInvoice j = list.FirstOrDefault();
                        if (j != null)
                        {
                            TblJob jObj = JobManager.SelectByID(j.JobID);
                            CurrencyID = jObj != null ? (short)jObj.CurrencyID : (short)0;
                        }
                    }
                }
            }
            decimal RMValue = 0; decimal.TryParse(txtRMValue.Text.Trim(), out RMValue);

            if (string.IsNullOrEmpty(txtInvoiceDate.Text))
            {
                AddErrorPrompt(txtInvoiceDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (ddlContact.SelectedValue == "0")
            {
                AddErrorPrompt(ddlContact.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            taxID = ddlTax.SelectedValue == "0" ? (short?)null : Convert.ToInt16(ddlTax.SelectedValue);

            if (IsValid)
            {
                List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                if (list == null || list.Count == 0)
                {
                    string mes = "System cannot save Invoice without Delivery Order(s).";
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), mes, MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }

                TblInvoice invoice = InvoiceManager.SelectByID(InvoiceID);
                if (invoice != null)
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }

                    DateTime dt = DateTime.Now;
                    if (!DateTime.TryParseExact(txtInvoiceDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        dt = DateTime.Now;
                        AddErrorPrompt(txtInvoiceDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.DATE_VALIDATION));
                    }
                    invoice.InvoiceDate = dt;
                    invoice.Remark = txtRemark.Text;
                    //invoice.RemarkScreen = txtRemarkScreen.Text;
                    invoice.PaymentTern = txtPaymentTerms.Text;

                    //decimal totalPrice = 0; decimal.TryParse(txtTotalPrice.Text.Trim(), out totalPrice);
                    //decimal netPrice = 0; decimal.TryParse(txtNetTotal.Text.Trim(), out netPrice);
                    invoice.CurrencyID = CurrencyID;
                    invoice.CurrencyValue = 1;
                    invoice.RMValue = RMValue;

                    invoice.TaxID = taxID;

                    invoice.TotalPrice = CalculatorPrice();
                    invoice.NetTotal = CalculatorNetTotalPrice();

                    invoice.PONumber = txtYourReference.Text.Trim();
                    invoice = InvoiceManager.Update(invoice);
                    if (invoice != null)
                    {
                      
                        //Current
                        List<int> CurrentList = InvoiceManager.SelectListJobIDByInvoiceId(invoice.InvoiceID);
                        if (CurrentList != null && CurrentList.Count > 0)
                        {
                           
                            if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
                            {
                                //List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                                if (list != null && list.Count > 0)
                                {
                                    foreach (var item in list)
                                    {
                                        if (!CurrentList.Contains(item.JobID))
                                        {
                                            if (!InvoiceManager.CheckExitJobInDetail(item.JobID))
                                            {
                                                InvoiceManager.InsertDetail(invoice.InvoiceID, item.JobID);
                                            }
                                        }
                                    }

                                    foreach (var item in CurrentList)
                                    {
                                        if ((list.Find(r => r.JobID == item) == null ? true : false))
                                        {
                                            InvoiceManager.DeleteDetailByJobIdAndInvoiceId(item, invoice.InvoiceID);
                                        }
                                    }
                                }
                                else
                                {
                                    InvoiceManager.DeleteDetailByInvoiceId(invoice.InvoiceID);
                                }
                            }
                            else
                            {
                                InvoiceManager.DeleteDetailByInvoiceId(invoice.InvoiceID);
                            }
                        }
                        else
                        {
                            if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
                            {
                                //List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                                if (list != null && list.Count > 0)
                                {
                                    foreach (var item in list)
                                    {
                                        if (!InvoiceManager.CheckExitJobInDetail(item.JobID))
                                        {
                                            InvoiceManager.InsertDetail(invoice.InvoiceID, item.JobID);
                                        }
                                    }
                                }
                            }
                        }

                        if (AllowSaveLogging)
                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.UPDATE_INVOICE), FUNCTION_PAGE, invoice.ToJSONString());

                        /// Trunglc Add - 23-04-2015
                        /// 

                        List<JobInvoice> lstJobInvoice = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;

                        foreach (JobInvoice item in lstJobInvoice)
                        {
                            InvoiceManager.LockJobAndOCAndDO(item.JobID);
                        }

                        /// 
                        /// End
                      

                        Session[ViewState["PageID"] + "SweetSoft_InvoiceID"] = invoice.InvoiceID;
                        LoadData();
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVE_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                    }
                }
                else
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }

                    invoice = new TblInvoice();
                    string invoiceNo = txtInvoiceNumber.Text.Trim();
                    if (InvoiceManager.CheckExitInvoiceNo(invoiceNo))
                    {
                        invoiceNo = CreateOrderbNumber();
                    }

                    invoice.InvoiceNo = invoiceNo;
                    DateTime dt = DateTime.Now;
                    if (!DateTime.TryParseExact(txtInvoiceDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        dt = DateTime.Now;
                        AddErrorPrompt(txtInvoiceDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.DATE_VALIDATION));
                    }
                    invoice.InvoiceDate = dt;
                    invoice.CustomerID = customerId;
                    invoice.ContactID = contactId;
                    invoice.PONumber = string.Empty;

                    invoice.Remark = txtRemark.Text;
                    //invoice.RemarkScreen = txtRemarkScreen.Text;
                    invoice.PaymentTern = txtPaymentTerms.Text;

                    //decimal totalPrice = 0; decimal.TryParse(txtTotalPrice.Text.Trim(), out totalPrice);
                    //decimal netPrice = 0; decimal.TryParse(txtNetTotal.Text.Trim(), out netPrice);
                    invoice.CurrencyID = CurrencyID;
                    invoice.CurrencyValue = 1;
                    invoice.RMValue = RMValue;

                    invoice.TaxID = taxID;

                    invoice.TotalPrice = CalculatorPrice();
                    invoice.NetTotal = CalculatorNetTotalPrice();
                    invoice.PONumber = txtYourReference.Text.Trim();

                    invoice = InvoiceManager.Insert(invoice);
                    if (invoice != null)
                    {
                        if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
                        {
                            //List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                            if (list != null && list.Count > 0)
                            {
                                foreach (var item in list)
                                {
                                    if (!InvoiceManager.CheckExitJobInDetail(item.JobID))
                                    {
                                        InvoiceManager.InsertDetail(invoice.InvoiceID, item.JobID);
                                    }
                                }
                            }
                        }

                        if (AllowSaveLogging)
                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.INSERT_INVOICE), FUNCTION_PAGE, invoice.ToJSONString());

                        /// Trunglc Add - 23-04-2015
                        /// 

                        List<JobInvoice> lstJobInvoice = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;

                        foreach (JobInvoice item in lstJobInvoice)
                        {
                            InvoiceManager.LockJobAndOCAndDO(item.JobID);
                        }

                        /// 
                        /// End

                        Session[ViewState["PageID"] + "SweetSoft_InvoiceID"] = invoice.InvoiceID;
                        invoiceId = invoice.InvoiceID;                       

                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_ADD_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                        LoadData();
                    }
                }
            }
            
            if(!IsValid)
                ShowErrorPromptExtension();

            //if (isCreate)
            //    Response.Redirect(string.Format("OrderInvoice.aspx?ID={0}", invoiceId));
        }


        protected void btnAddJob_Click(object sender, EventArgs e)
        {
            ////Kiểm tra quyền
            //if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            //{
            //    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
            //    OpenMessageBox(msgRole, null, false, false);
            //    return;
            //}

            List<int> IDs= null;
            if (Session[ViewState["PageID"] + "SweetSoft-JobID-List"] != null)
                IDs = Session[ViewState["PageID"] + "SweetSoft-JobID-List"] as List<int>;

            int jobId = 0; int.TryParse(hdfJobIDTemp.Value, out jobId);
            TblJob _Job = JobManager.SelectByID(jobId);
            if (_Job != null)
            {
                if (IDs != null)//Nếu danh sách job đã được tạo
                {
                    short TaxID = 0, CurrencyID = 0;
                    short.TryParse(hCurrencyID.Value, out CurrencyID);
                    short.TryParse(ddlTax.SelectedValue, out TaxID);
                    //Kiểm tra curruncy
                    if (CurrencyID != 0 && CurrencyID != _Job.CurrencyID && IDs.Count > 0)//Nếu CURR của job hiện tại không giống job cũ
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "The currency of this job is different from other jobs in this invoice", MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        return;
                    }
                    //Kiểm tra tax code
                    TblOrderConfirmation oObj = OrderConfirmationManager.SelectByID(_Job.JobID);
                    if (IDs.Count == 0)//Nếu chưa có job nào trong invoice
                    {
                        TblCurrency cObj = new CurrencyManager().SelectByID(_Job.CurrencyID != null ? (short)_Job.CurrencyID : (short)0);
                        if (cObj != null)
                        {
                            decimal RMValue = new CurrencyManager().SelectExchangeRateByCurrencyID((_Job.CurrencyID != null ? (short)_Job.CurrencyID : (short)0), DateTime.Today);
                            hCurrencyID.Value = _Job.CurrencyID.ToString();
                            ltrCurrency.Text = cObj.CurrencyName;
                            txtRMValue.Text = RMValue.ToString("N4");//cObj.RMValue.ToString("N3");
                        }

                        //Set tax cho invoice
                        if (oObj != null)
                        {
                            ddlTax.SelectedValue = oObj.TaxID != null ? oObj.TaxID.ToString() : "0";
                        }
                    }
                    else
                    {
                        short currJobTaxID = oObj.TaxID != null ? Convert.ToInt16(oObj.TaxID.ToString()) : (short)0;
                        if (currJobTaxID != TaxID)
                        {
                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "The tax of this job is different from other jobs in this invoice", MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msg, null, false, false);
                            return;
                        }
                    }
                    IDs.Add(_Job.JobID);
                }
                else//Nếu chưa có job
                {
                    hCurrencyID.Value = _Job.CurrencyID.ToString();
                    TblCurrency cObj = new CurrencyManager().SelectByID(_Job.CurrencyID != null ? (short)_Job.CurrencyID : (short)0);
                    //Kiểm tra tax code
                    TblOrderConfirmation oObj = OrderConfirmationManager.SelectByID(_Job.JobID);
                    if (cObj != null)
                    {
                        decimal RMValue = new CurrencyManager().SelectExchangeRateByCurrencyID((_Job.CurrencyID != null ? (short)_Job.CurrencyID : (short)0), DateTime.Today);
                        ltrCurrency.Text = cObj.CurrencyName;
                        txtRMValue.Text = RMValue.ToString("N4");//cObj.RMValue.ToString("N3");
                    }
                    ddlTax.SelectedValue = oObj.TaxID != null ? oObj.TaxID.ToString() : "0";
                    IDs = new List<int>();
                    IDs.Add(_Job.JobID);
                }
                Session[ViewState["PageID"] + "SweetSoft-JobID-List"] = IDs;
                GetData(_Job);
                LoadJobInvoices();
                LoadFromSession();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsNewInvoice = InvoiceManager.IsNewInvoice(InvoiceID);
                bool IsLocking = InvoiceManager.IsInvoiceLocking(InvoiceID);

                if (!IsNewInvoice && !IsLocking)
                {
                    if (!RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }
                }
                //Kiểm tra quyền
                if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }

                ModalConfirmResult result = new ModalConfirmResult();
                result.Value = "OrderInvoice_Delete";
                CurrentConfirmResult = result;
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Do you want to delete seleted rows?", MSGButton.DeleteCancel, MSGIcon.Error);
                OpenMessageBox(msg, result, false, false);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            bool isDelete = false;
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        if (e.Value.ToString().Equals("OrderInvoice_Delete"))
                        {
                            TblInvoice invoice = InvoiceManager.SelectByID(InvoiceID);
                            if (invoice != null)
                            {
                                if (InvoiceManager.Delete(InvoiceID))
                                {
                                    isDelete = true;
                                    if (AllowSaveLogging)
                                        SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.UPDATE_INVOICE), FUNCTION_PAGE, invoice.ToJSONString());
                                }
                                else
                                {
                                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_DELETE_DATA), MSGButton.OK, MSGIcon.Error);
                                    OpenMessageBox(msg, null, false, false);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_DATA), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msg, null, false, false);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_DATA), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            if (isDelete)
            {
                Response.Redirect("InvoiceList.aspx");
            }
        }


        private void GetData(TblJob job)
        {
            TblOrderConfirmation orderConfirm = OrderConfirmationManager.SelectByID(job.JobID);
            if (orderConfirm != null)
            {
                JobInvoice jobInvoice = new JobInvoice();
                jobInvoice.JobID = job.JobID;
                jobInvoice.CustomerPO1 = orderConfirm.CustomerPO1;
                jobInvoice.CustomerPO2 = orderConfirm.CustomerPO2;
                jobInvoice.OrderConfirmNo = orderConfirm.OCNumber;

                TblContact contact = ContactManager.SelectByID(orderConfirm.ContactPersonID);
                if (contact != null)
                    jobInvoice.ContactPerson = contact.ContactName;

                jobInvoice.JobNumber = job.JobNumber;
                jobInvoice.JobRev = job.RevNumber;
                jobInvoice.JobName = job.JobName;
                jobInvoice.JobDesign = job.Design;

                jobInvoice.Discount = orderConfirm.Discount.HasValue ? orderConfirm.Discount.Value : 0;
                jobInvoice.TaxRate = orderConfirm.TaxPercentage.HasValue ? orderConfirm.TaxPercentage.Value : 0;

                //Cylinder
                decimal totalPriceCylinder = 0;
                DataTable dt = CylinderManager.SelectCylinderSelectForOrderConfirmation(job.JobID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal.TryParse(dt.Rows[0]["Total"].ToString(), out totalPriceCylinder);
                }
                jobInvoice.CylinderTotalPrice = totalPriceCylinder;
                jobInvoice.CylinderDataSource = dt;
                jobInvoice.NetTotalCylinderPrice = totalPriceCylinder * (1 - (decimal)(orderConfirm.Discount.HasValue ? orderConfirm.Discount : 0) / 100);
                
                //Other changer
                decimal totalPriceOtherCharges = 0;
                List<OtherChargesExtension> coll = OrderConfirmationManager.SelectOtherChargeByJobID(job.JobID);
                TblOtherChargeCollection collCharger = new TblOtherChargeCollection();
                foreach (OtherChargesExtension item in coll)
                {
                    TblOtherCharge newItem = (TblOtherCharge)item;
                    collCharger.Add(newItem);
                }
                jobInvoice.OtherChargesDataSource = collCharger;
                if (collCharger != null && collCharger.Count > 0)
                {
                    foreach (var item1 in collCharger)
                    {
                        totalPriceOtherCharges = totalPriceOtherCharges + (item1.Quantity.HasValue ? item1.Quantity.Value : 0) * (item1.Charge.HasValue ? item1.Charge.Value : 0);
                    }
                }
                jobInvoice.OtherChargesTotalPrice = totalPriceOtherCharges;
                jobInvoice.NetTotalOtherChargesPrice = totalPriceOtherCharges * (1 - (decimal)(orderConfirm.Discount.HasValue ? orderConfirm.Discount.Value : 0) / 100);

                //Service job
                decimal totalPriceServiceJob = 0;
                List<ServiceJobDetailExtension> colls = JobManager.SelectServiceJobDetailByID(job.JobID);

                TblServiceJobDetailCollection collJobDetail = new TblServiceJobDetailCollection();
                foreach (TblServiceJobDetail detail in colls)
                    collJobDetail.Add(detail);

                jobInvoice.ServiceJobDataSource = collJobDetail;
                if (collJobDetail != null && collJobDetail.Count > 0)
                {
                    foreach (var item2 in collJobDetail)
                        totalPriceServiceJob = totalPriceServiceJob + item2.WorkOrderValues;
                }

                jobInvoice.ServiceJobTotalPrice = totalPriceServiceJob;
                jobInvoice.NetTotalServicePrice = totalPriceServiceJob * (1 - (decimal)(orderConfirm.Discount.HasValue ? orderConfirm.Discount.Value : 0) / 100);

                List<JobInvoice> jobInvoices;
                if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] == null)
                {
                    jobInvoices = new List<JobInvoice>();
                    jobInvoices.Add(jobInvoice);
                }
                else
                {
                    jobInvoices = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                    if (jobInvoices != null && jobInvoices.Count > 0)
                    {
                        if (jobInvoices.Find(r => r.JobID == jobInvoice.JobID) == null ? true : false)
                            jobInvoices.Add(jobInvoice);
                    }
                    else
                    {
                        jobInvoices.Add(jobInvoice);
                    }
                }

                Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] = jobInvoices;
            }
        }

        private void LoadJobInvoices()
        {
            if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
            {
                List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                rpnlJobInvoice.DataSource = list;
                rpnlJobInvoice.DataBind();
            }

            txtTotalPrice.Text = CalculatorPrice().ToString("N2");
            txtNetTotal.Text = CalculatorNetTotalPrice().ToString("N2");
        }

        protected void rpnlJobInvoice_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            JobInvoiceControl control = e.Item.FindControl("JobInvoice1") as JobInvoiceControl;
            if (control != null)
            {
                JobInvoice item = e.Item.DataItem as JobInvoice;
                if (item != null)
                {
                    control.LoadData(item);

                }
            }
        }

        protected void btnRemoveJob_Click(object sender, EventArgs e)
        {
            //Kiểm tra quyền
            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            if (!btnSave.Visible)
                return;

            int jobId = 0; int.TryParse(hdfJobIdRemove.Value, out jobId);
            if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
            {
                List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                if (list != null && list.Count > 0)
                {
                    JobInvoice item = list.Find(r => r.JobID == jobId);
                    if (item != null)
                        list.Remove(item);

                    Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] = list;
                }
            }
            LoadJobInvoices();


            List<int> IDs = null;
            if (Session[ViewState["PageID"] + "SweetSoft-JobID-List"] != null)
                IDs = Session[ViewState["PageID"] + "SweetSoft-JobID-List"] as List<int>;

            if (IDs != null && IDs.Count > 0)
            {
                if (IDs.Contains(jobId))
                    IDs.Remove(jobId);
            }
            Session[ViewState["PageID"] + "SweetSoft-JobID-List"] = IDs;
            LoadFromSession();
            //divJobNumber.Visible = true;
        }

        private decimal CalculatorPrice()
        {
            decimal totalPrice = 0;
            if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
            {
                List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        //Tính discount
                        decimal price= item.CylinderTotalPrice + item.OtherChargesTotalPrice + item.ServiceJobTotalPrice;
                        decimal priceDiscount = price * (1 - (decimal)item.Discount / 100);
                        totalPrice = totalPrice + priceDiscount;
                    }
                }
            }

            return totalPrice;
        }

        private decimal CalculatorNetTotalPrice()
        {
            decimal totalPrice = 0;
            if (Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] != null)
            {
                List<JobInvoice> list = Session[ViewState["PageID"] + "SweetSoft-JobInvoice-List"] as List<JobInvoice>;
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        //Tính discount
                        decimal price = item.CylinderTotalPrice + item.OtherChargesTotalPrice + item.ServiceJobTotalPrice;
                        decimal priceDiscount = price * (1 - (decimal)item.Discount / 100);
                        decimal priceTaxed = priceDiscount * (1 + (decimal)item.TaxRate / 100);

                        totalPrice = totalPrice + priceTaxed;
                        //Tính thuế
                    }
                }
            }
            return totalPrice;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        //EXPORT EXCEL

        private void CreateExcel(string fileName)
        {
            //Parameters
            ReportParameter[] parameters = new ReportParameter[10];
            string InvoiceNo = string.Empty, SAPCode = string.Empty, CompanyCode = string.Empty, InvoiceDate = string.Empty, PostingDate = string.Empty, CurrencyName = string.Empty, CalcTax = string.Empty, TaxCode = string.Empty, Total = string.Empty, RMValue = string.Empty;
            string InvoiceIDs = string.Format("-{0}-", InvoiceID);
            DataTable dtInvoice = InvoiceManager.SelectForExport(InvoiceIDs);
            if (dtInvoice.Rows.Count > 0)
            {
                InvoiceNo = dtInvoice.Rows[0]["InvoiceNo"].ToString();
                SAPCode = dtInvoice.Rows[0]["SAPCode"].ToString();
                InvoiceDate = dtInvoice.Rows[0]["InvoiceNo"].ToString();
                PostingDate = DateTime.Today.ToString("yyyyMMdd");
                CurrencyName = dtInvoice.Rows[0]["CurrencyName"].ToString();
                CalcTax = dtInvoice.Rows[0]["CalcTax"].ToString();
                TaxCode = dtInvoice.Rows[0]["TaxCode"].ToString();
                Total = string.IsNullOrEmpty(dtInvoice.Rows[0]["TotalPrice"].ToString()) ? string.Empty : ((decimal)dtInvoice.Rows[0]["TotalPrice"]).ToString("N2");
                RMValue = string.IsNullOrEmpty(dtInvoice.Rows[0]["RMValue"].ToString()) ? string.Empty : ((decimal)dtInvoice.Rows[0]["RMValue"]).ToString("N2");
            }
            //InvoiceNo
            parameters[0] = new ReportParameter("InvoiceNo", InvoiceNo);
            //SAPCode
            parameters[1] = new ReportParameter("SAPCode", SAPCode);
            //CompanyCode
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyCode);
            if (setting != null)
                CompanyCode = setting.SettingValue;
            parameters[2] = new ReportParameter("CompanyCode", CompanyCode);
            //InvoiceDate
            parameters[3] = new ReportParameter("InvoiceDate", InvoiceDate);
            //PostingDate
            parameters[4] = new ReportParameter("PostingDate", PostingDate);
            //CurrencyName
            parameters[5] = new ReportParameter("CurrencyName", CurrencyName);
            //CalcTax
            parameters[6] = new ReportParameter("CalcTax", CalcTax);
            //TaxCode
            parameters[7] = new ReportParameter("TaxCode", TaxCode);
            //Total
            parameters[8] = new ReportParameter("Total", Total);
            //RMValue
            parameters[9] = new ReportParameter("RMValue", RMValue);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable dtDetail = InvoiceManager.SelectDetailForExport(InvoiceIDs);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Invoice_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceSAPHeader", dtInvoice));
            viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceSrc", dtDetail));
            viewer.LocalReport.SetParameters(parameters);

            //Chuyển sang Excel
            byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        private void CreateHeadFile(string FileName)
        {

            string InvoiceNo = string.Empty, SAPCode = string.Empty, CompanyCode = string.Empty, InvoiceDate = string.Empty, PostingDate = string.Empty, CurrencyName = string.Empty, CalcTax = string.Empty, TaxCode = string.Empty, Total = string.Empty, RMValue = string.Empty;
            //EXPORT HEAD
            //Get Invocie data
            string InvoiceIDs = string.Format("-{0}-", InvoiceID);
            DataTable dtInvoice = InvoiceManager.SelectForExport(InvoiceIDs);
            if (dtInvoice.Rows.Count > 0)
            {
                InvoiceNo = dtInvoice.Rows[0]["InvoiceNo"].ToString();
                SAPCode = dtInvoice.Rows[0]["SAPCode"].ToString();
                InvoiceDate = dtInvoice.Rows[0]["InvoiceNo"].ToString();
                PostingDate = DateTime.Today.ToString("yyyyMMdd");
                CurrencyName = dtInvoice.Rows[0]["CurrencyName"].ToString();
                CalcTax = dtInvoice.Rows[0]["CalcTax"].ToString();
                TaxCode = dtInvoice.Rows[0]["TaxCode"].ToString();
                Total = string.IsNullOrEmpty(dtInvoice.Rows[0]["TotalPrice"].ToString()) ? string.Empty : ((decimal)dtInvoice.Rows[0]["TotalPrice"]).ToString("N2");
                RMValue = string.IsNullOrEmpty(dtInvoice.Rows[0]["RMValue"].ToString()) ? string.Empty : ((decimal)dtInvoice.Rows[0]["RMValue"]).ToString("N4");
            }
            StringWriter headWriter = new StringWriter();
            headWriter.WriteLine("{0}    {1}    {2}    {3}    {4}    {5}    {6}    {7}    {8}", InvoiceNo, SAPCode, InvoiceDate, PostingDate, CurrencyName, CalcTax, TaxCode, Total, RMValue);
            using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                writer.Write(headWriter.ToString());
            }
            Response.ContentType = "text/plain";

            Response.AddHeader("content-disposition", "attachment;filename=" + string.Format("Head_File_{0}.txt", FileName));
            Response.Clear();

            using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                writer.Write(headWriter.ToString());
            }
            Response.End();
        }

        private void CreatePotisionsFile(string FileName)
        {
            //EXPORT POTISIONS
            //Get invoice detail data
            string InvoiceIDs = string.Format("-{0}-", InvoiceID);
            DataTable dtDetail = InvoiceManager.SelectDetailForExport(InvoiceIDs);

            StringWriter potisionWriter = new StringWriter();
            //Write detail to file
            foreach (var r in dtDetail.AsEnumerable().ToList())
            {
                string dInvoiceNo = r.Field<string>("InvoiceNo");
                string dGLCode = r.Field<string>("GLCode");
                string dTotal = r.Field<double>("Total").ToString("N2");
                string dTaxCode = r.Field<string>("TaxCode");
                string dDescription = r.Field<string>("Description");
                string dJobNumber = r.Field<string>("JobNumber");

                potisionWriter.WriteLine("{0}    {1}    {2}    {3}    {4}    {5}", dInvoiceNo, dGLCode, dTotal, dTaxCode, dDescription, dJobNumber);
            }

            Response.ContentType = "text/plain";

            Response.AddHeader("content-disposition", "attachment;filename=" + string.Format("Positions_File_{0}.txt", FileName));
            Response.Clear();

            using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                writer.Write(potisionWriter.ToString());
            }
            Response.End();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (InvoiceID != 0)
            {
                string InvoiceNo = txtInvoiceNumber.Text.Trim();
                string fileName = string.Format("Invoice_{0}", InvoiceNo);
                CreateExcel(fileName);
            }
            else
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Cannot find invoice", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
            }
        }

        protected void btnExportHead_Click(object sender, EventArgs e)
        {
            if (InvoiceID != 0)
            {
                string InvoiceNo = txtInvoiceNumber.Text.Trim();
                string fileName = string.Format("{0}", InvoiceNo);
                CreateHeadFile(fileName);
            }
            else
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Cannot find invoice", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
            }
        }

        protected void tblExportPotisions_Click(object sender, EventArgs e)
        {
            if (InvoiceID != 0)
            {
                string InvoiceNo = txtInvoiceNumber.Text.Trim();
                string fileName = string.Format("{0}", InvoiceNo);
                CreatePotisionsFile(fileName);
            }
            else
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Cannot find invoice", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
            }
        }

        #region Trunglc Add - 23-04-2015

        protected void btnLock_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowLock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }
            UpdateInvoiceLockStatus(true);
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }
            UpdateInvoiceLockStatus(false);
        }

        private void UpdateInvoiceLockStatus(bool IsLock)
        {
            if (this.InvoiceID > 0)
            {
                InvoiceManager.LockOrUnLockInvoice(this.InvoiceID, IsLock);

                //bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
                bool IsAllowEdit = !IsLock ? true : false;

                AllowEditting(IsAllowEdit);

                string KEY_MESSAGE = IsLock ? ResourceText.LOCK_INVOICE_SAVE_SUCCESSFULLY : ResourceText.UNLOCK_INVOICE_SAVE_SUCCESSFULLY;

                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(KEY_MESSAGE), MSGButton.OK, MSGIcon.Success);
                OpenMessageBox(msg, null, false, false);

            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_NOT_FOUND), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
            }
        }

        #endregion
    }
}