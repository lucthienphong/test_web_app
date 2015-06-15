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
using SweetSoft.APEM.Core.LoggingManager;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class OrderConfirmation : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "order_confirmation";
            }
        }

        private int JobID
        {
            get
            {
                int mJobID = 0;
                if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"].ToString(), out mJobID);
                else if (Session[ViewState["PageID"] + "SweetSoft_JobID"] != null)
                    int.TryParse(Session[ViewState["PageID"] + "SweetSoft_JobID"].ToString(), out mJobID);
                return mJobID;
            }
            set { }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                Session[ViewState["PageID"] + "SweetSoft_JobID"] = null;
                LoadDDL();
                ApplyControlResourceTexts();
                if (JobID == 0)
                    btnPricesLookup.Enabled = false;
                LoadData();
            }
            else
            {
                ltrPrint.Text = string.Format(@"<div class='btn-group'>
                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                            <span class='flaticon-printer60'></span> Print <span class='caret'></span>
                        </button>
                        <ul class='dropdown-menu openPrinting' role='menu'>
                            <li>
                                <a href='#' data-href='Printing/PrintOrderConfirmation.aspx?ID={0}'>OC - Normal Job</a>
                            </li>
                            <li>
                                <a href='#' data-href='Printing/PrintJobServiceDetail.aspx?ID={0}'>OC - Service Job</a>
                            </li>
                        </ul>
                        </div>", JobID);
            }
        }

        #region General
        //Load data for Dropdownlist-------------------------------------------------------------
        void LoadDDL()
        {
            BindDDLTax(true);
        }

        void BindDDLTax(bool SelectAll)
        {
            List<TblTax> taxColl = new TaxManager().SelectAllForDDL(SelectAll);
            if (taxColl != null && taxColl.Count > 0)
            {
                ddlTax.DataSource = taxColl;
                ddlTax.DataTextField = "TaxName";
                ddlTax.DataValueField = "TaxID";
                ddlTax.DataBind();
            }
        }

        //Load contacts of customer -------------------------------------------------------------
        private void BindContactPerson(int customerId)
        {
            if (ddlContact.Items.Count > 0)
            {
                ddlContact.Items.Clear();
            }
            var coll = ContactManager.SelectAllByCustomerID(customerId);
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

        //Apply text for control -------------------------------------------------------------
        private void ApplyControlResourceTexts()
        {
            btnPricesLookup.Text = ResourceTextManager.GetApplicationText(ResourceText.PRICES_LOOKUP);
            //Cylinders
            gvClinders.Columns[0].HeaderText = "No";
            gvClinders.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.STEEL_BASE);
            gvClinders.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CYL_NO);
            gvClinders.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
            gvClinders.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.PRICING);
            gvClinders.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CIRCUMFERE);
            gvClinders.Columns[6].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.FACE_WIDTH);
            gvClinders.Columns[7].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.UNIT_PRICE);
            gvClinders.Columns[8].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.QTY);
            gvClinders.Columns[9].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.TOTAL_PRICE);

            //ServiceJob
            grvServiceJobDetail.Columns[0].HeaderText = "No";
            grvServiceJobDetail.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_NUMBER);
            grvServiceJobDetail.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.PRODUCTID);
            grvServiceJobDetail.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
            grvServiceJobDetail.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_VALUES_IN_USD);

            //Ohter charges
            grvOtherCharges.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
            grvOtherCharges.Columns[1].HeaderText = "Pricing";
            grvOtherCharges.Columns[2].HeaderText = "Quantity";//ResourceTextManager.GetApplicationText(ResourceText.qua);
            grvOtherCharges.Columns[3].HeaderText = "Charge";//ResourceTextManager.GetApplicationText(ResourceText.TOTAL_PRICE);
        }
        #endregion

        private void BindQuotationData(int jobID)
        {
            BindQuotationRev(jobID);
            List<JobQuotationDetailExtension> source = new List<JobQuotationDetailExtension>();
            TblJobQuotation cObj = JobQuotationManager.SelectNewestQuotationByJobID(jobID);//Tìm thông tin báo giá, nếu có thì bind dữ liệu
            if (cObj != null)
            {
                txtQuotationNo.Text = cObj.QuotationNo;
                txtQuotationDate.Text = cObj.QuotationDate != null ? ((DateTime)cObj.QuotationDate).ToString("dd/MM/yyyy") : string.Empty;
                txtQuotationNote.Text = cObj.QuotationText;

                source = JobQuotationManager.SelectDetail(cObj.QuotationID, cObj.JobID);
            }
            else//Nếu không thì lấy dữ liệu của Job
            {
                TblJob jObj = JobManager.SelectByID(jobID);
                if (jObj != null)
                    txtQuotationNo.Text = jObj.JobNumber;
                txtQuotationDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txtQuotationNote.Text = string.Empty;
                source = JobQuotationManager.SelectDetail(0, jobID);
            }
            grvPrices.DataSource = source;
            grvPrices.DataBind();
        }

        private void BindQuotationByJobID(int JobID)
        {
            TblJob jObj = JobManager.SelectByID(JobID);
            if (jObj != null)
                txtQuotationNo.Text = jObj.JobNumber;
            txtQuotationDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtQuotationNote.Text = string.Empty;

            List<JobQuotationDetailExtension> source = new List<JobQuotationDetailExtension>();
            source = JobQuotationManager.SelectDetail(0, JobID);
            grvPrices.DataSource = source;
            grvPrices.DataBind();
            lbQuotationMessage.Text = string.Empty;
        }

        private void BindQuotationByQuotationID(int QuotationID)
        {
            TblJobQuotation cObj = JobQuotationManager.SelectByID(QuotationID);//Tìm thông tin báo giá, nếu có thì bind dữ liệu
            if (cObj != null)
            {
                txtQuotationNo.Text = cObj.QuotationNo;
                txtQuotationDate.Text = cObj.QuotationDate != null ? ((DateTime)cObj.QuotationDate).ToString("dd/MM/yyyy") : string.Empty;
                txtQuotationNote.Text = cObj.QuotationText;

                List<JobQuotationDetailExtension> source = new List<JobQuotationDetailExtension>();
                source = JobQuotationManager.SelectDetail(cObj.QuotationID, cObj.JobID);
                grvPrices.DataSource = source;
                grvPrices.DataBind();
                lbQuotationMessage.Text = string.Empty;
            }
        }

        protected void ddlQuotationRev_SelectedIndexChanged(object sender, EventArgs e)
        {
            int RevNumber = Convert.ToInt32(ddlQuotationRev.SelectedItem.Text);
            int QuotationID = Convert.ToInt32(ddlQuotationRev.SelectedValue);
            if (RevNumber != 0)
                BindQuotationByQuotationID(QuotationID);
            else
                BindQuotationByJobID(QuotationID);
        }

        private void BindQuotationRev(int JobID)
        {
            TblJobQuotationCollection list = new TblJobQuotationCollection();
            list = JobQuotationManager.SelectAllQuotationByJobID(JobID);
            ddlQuotationRev.DataSource = list;
            ddlQuotationRev.DataTextField = "RevNumber";
            ddlQuotationRev.DataValueField = "QuotationID";
            ddlQuotationRev.DataBind();
        }

        private void InniJobQuotationPricing(int jobID)
        {
            TblJob job = JobManager.SelectByID(jobID);
            if (job != null)
            {
                TblPricingCollection pricingColl = PricingManager.SelectAll();
                if (pricingColl != null && pricingColl.Count > 0)
                {

                    TblJobQuotation objQuotation = JobQuotationManager.SelectNewestQuotationByJobID(jobID);
                    if (objQuotation == null)
                    {
                        objQuotation = new TblJobQuotation();
                        objQuotation.JobID = jobID;
                        objQuotation.QuotationText = string.Empty;
                        objQuotation = JobQuotationManager.Insert(objQuotation);
                    }

                    foreach (var item in pricingColl)
                    {
                        if (objQuotation != null)
                        {
                            if (!JobQuotationManager.CheckExitsByJobID(jobID, item.PricingID))
                            {
                                TblJobQuotationPricing objPricing = new TblJobQuotationPricing();
                                objPricing.PricingID = item.PricingID;
                                decimal OldPrice = 0;
                                decimal NewPrice = 0;
                                TblCustomerQuotationDetail cusPricing = CustomerQuotationManager.SelectCustomerQuotationPricingByCustomerIDAndPricingID(item.PricingID, job.CustomerID);
                                if (cusPricing != null)
                                { OldPrice = cusPricing.OldSteelBase; NewPrice = cusPricing.NewSteelBase; }
                                objPricing.OldSteelBasePrice = OldPrice;
                                objPricing.NewSteelBasePrice = NewPrice;
                                JobQuotationManager.InsertDetail(objPricing);
                            }
                        }

                    }
                }
            }

            decimal priceDefault = 0;
            //Khởi tạo giá trị unit price mặc định cho cylinder
            TblCylinderCollection coll = CylinderManager.SelectCylinderByJobID(jobID);
            if (coll != null && coll.Count > 0)
            {
                foreach (var item in coll)
                {
                    if (!item.UnitPrice.HasValue || (item.UnitPrice.HasValue && item.UnitPrice.Value == 0))
                    {
                        TblJobQuotationPricing jobPricing = JobQuotationManager.SelectDetailByJobIDAndPricingID(item.JobID, item.PricingID);
                        if (jobPricing != null)
                        {
                            if (Convert.ToBoolean(item.SteelBase))
                                priceDefault = jobPricing.NewSteelBasePrice;
                            else
                                priceDefault = jobPricing.OldSteelBasePrice;
                        }

                        item.UnitPrice = priceDefault;
                        CylinderManager.Update(item);
                    }
                }
            }
        }

        private void UnSelectDropdownlist(DropDownList ddl)
        {
            foreach (ListItem li in ddl.Items)
            {
                ddl.Items.FindByValue(li.Value).Selected = false;
            }
        }

        private void ResetFiled(bool isDeleteName)
        {
            if (isDeleteName)
            {
                txtName.Text = string.Empty;
                txtCode.Text = string.Empty;
                txtPaymentTerms.Text = string.Empty;
                txtDeliveryTerms.Text = string.Empty;
                txtCustomerPO1.Text = string.Empty;
                txtCustomerPO2.Text = string.Empty;
            }

            ddlTax.SelectedIndex = 0;
            txtTaxRate.Text = string.Empty;
            ddlContact.SelectedIndex = 0;
            txtOrderDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtTotalPrice.Text = string.Empty;
            txtDiscount.Text = "0";
            txtNetTotal.Text = string.Empty;
            txtRemark.Text = string.Empty;
            //txtRemarkScreen.Text = string.Empty;

            grvOtherCharges.DataSource = null;
            grvOtherCharges.DataBind();
            gvClinders.DataSource = null;
            gvClinders.DataBind();
        }

        private void LoadData()
        {
            if (!RoleManager.AllowUpdateStatus(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                txtRMValue.Enabled = false;
                txtDiscount.Enabled = false;
            }
            else
            {
                txtRMValue.Enabled = true;
                txtDiscount.Enabled = true;
            }
            txtTotalPrice.Enabled = false;
            txtNetTotal.Enabled = false;

            TblOrderConfirmation orderConfim = OrderConfirmationManager.SelectByID(JobID);
            if (orderConfim != null)
            {
                TblJob job = JobManager.SelectByID(orderConfim.JobID);
                if (job != null)
                {
                    if (JobID > 0)
                    {
                        ddlJobNumber.Items.Clear();
                        ddlRevNumber.Items.Clear();

                        ddlJobNumber.Items.Add(new ListItem(job.JobNumber, job.JobID.ToString()));
                        ddlRevNumber.Items.Add(new ListItem(job.RevNumber.ToString(), job.JobID.ToString()));
                    }

                    txtJobName.Text = job.JobName;
                    txtDesign.Text = job.Design;

                    TblCustomer cus = CustomerManager.SelectByID(job.CustomerID);
                    if (cus != null)
                    {
                        BindContactPerson(cus.CustomerID);
                        txtName.Text = cus.Name;
                        txtCode.Text = cus.Code;
                        hCustomerID.Value = cus.CustomerID.ToString();
                    }

                    short CurrencyID = (job.CurrencyID == null ? (short)0 : (short)job.CurrencyID);
                    hCurrencyID.Value = CurrencyID.ToString();
                    BindJobCurrency(CurrencyID);
                }


                if (ddlContact.Items.FindByValue(orderConfim.ContactPersonID.ToString()) != null)
                    ddlContact.Items.FindByValue(orderConfim.ContactPersonID.ToString()).Selected = true;

                //Bind thông tin thuế
                if (!OrderConfirmationManager.HaveGST(orderConfim.JobID))
                {
                    //BindDDLTax(false);
                    BindDDLTax(true);
                }
                ddlTax.SelectedValue = orderConfim.TaxID != null ? orderConfim.TaxID.ToString() : "0";
                txtTaxRate.Text = orderConfim.TaxPercentage != null ? ((double)orderConfim.TaxPercentage).ToString("N2") : "0";

                txtRMValue.Text = orderConfim.RMValue.ToString("N4");
                txtCurrencyValue.Text = orderConfim.CurrencyValue.ToString("N4");

                BindCylinders(orderConfim.JobID);
                BindQuotationData(orderConfim.JobID);
                LoadOtherCharges(orderConfim.JobID);
                LoadServiceJobDetail(orderConfim.JobID);

                lblOrderNumber.Text = orderConfim.OCNumber;
                txtCustomerPO1.Text = orderConfim.CustomerPO1;
                txtCustomerPO2.Text = orderConfim.CustomerPO2;
                txtOrderDate.Text = orderConfim.OrderDate.ToString("dd/MM/yyyy");
                txtDiscount.Text = orderConfim.Discount.HasValue ? orderConfim.Discount.Value.ToString("N2") : "0";
                txtRemark.Text = orderConfim.Remark;
                //txtRemarkScreen.Text = orderConfim.RemarkScreen;
                txtPaymentTerms.Text = orderConfim.PaymentTerm;
                txtDeliveryTerms.Text = orderConfim.DeliveryTerm;

                AllowEdit(false);
                //ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintOrderConfirmation.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", orderConfim.JobID);
                ltrPrint.Text = string.Format(@"<div class='btn-group'>
                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                            <span class='flaticon-printer60'></span> Print <span class='caret'></span>
                        </button>
                        <ul class='dropdown-menu openPrinting' role='menu'>
                            <li>
                                <a href='#' data-href='Printing/PrintOrderConfirmation.aspx?ID={0}'>OC - Normal Job</a>
                            </li>
                            <li>
                                <a href='#' data-href='Printing/PrintAdditionalJobServices.aspx?ID={0}'>OC - Service Job</a>
                            </li>
                        </ul>
                        </div>", orderConfim.JobID);
            }
            else
            {
                lblOrderNumber.Text = CreateOrderbNumber();
                LoadJobNumber(0);
                BindContactPerson(0);
                LoadOtherCharges(0);
                btnDelete.Visible = false;
                divCylinder.Visible = false;
                divServiceJob.Visible = false;
                divOrderCharger.Visible = false;
                //txtDiscount.ReadOnly = false;
            }

            LoadTotalPrice();

            /// Trunglc Add - 23-04-2015
            /// 
            bool IsLocking = OrderConfirmationManager.IsOrderConfirmationLocking(JobID);
            bool IsNewOrder = OrderConfirmationManager.IsNewOrderConfirmation(JobID);
            bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
            bool IsAllowEdit = IsNewOrder ? true : (!IsLocking ? true : (IsLocking ? false : true));

            AllowEditting(IsAllowEdit);
        }

        private void AllowEdit(bool yesno)
        {
            txtName.Enabled = yesno;
            ddlJobNumber.Enabled = yesno;
            ddlRevNumber.Enabled = yesno;
        }

        private void AllowEditting(bool yesno)
        {
            //Job detail
            ddlContact.Enabled = yesno;
            txtCustomerPO1.Enabled = yesno;
            txtCustomerPO2.Enabled = yesno;
            txtOrderDate.Enabled = yesno;
            ddlTax.Enabled = yesno;
            txtRMValue.Enabled = yesno;
            txtRemark.Enabled = yesno;
            txtPaymentTerms.Enabled = yesno;
            txtDeliveryTerms.Enabled = yesno;
            txtDiscount.Enabled = yesno;

            for (int i = grvServiceJobDetail.Columns.Count - 1; i < grvServiceJobDetail.Columns.Count; i++)
            {
                grvServiceJobDetail.Columns[i].Visible = yesno;
            }

            for (int i = grvOtherCharges.Columns.Count - 1; i < grvOtherCharges.Columns.Count; i++)
            {
                grvOtherCharges.Columns[i].Visible = yesno;
            }

            btnPricesLookup.Visible = yesno;

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
            bool IsNewOrderConfirmation = OrderConfirmationManager.IsNewOrderConfirmation(JobID);
            bool IsOrderConfirmLocking = OrderConfirmationManager.IsOrderConfirmationLocking(JobID);

            if (IsNewOrderConfirmation)
            {
                btnLock.Visible = IsAllowLock ? true : false;
                btnUnlock.Visible = false;
            }
            else
            {
                btnLock.Visible = IsNewOrderConfirmation && IsAllowLock ? true : ((!IsOrderConfirmLocking && IsAllowLock && yesno ? true : false));
                btnUnlock.Visible = IsOrderConfirmLocking && IsAllowUnlock && !yesno ? true : false;
            }

            btnDelete.Visible = IsOrderConfirmLocking ? false : true;

            ///End
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsNewOrderComfirmation = OrderConfirmationManager.IsNewOrderConfirmation(JobID);
                bool IsLocking = OrderConfirmationManager.IsOrderConfirmationLocking(JobID);

                if (!IsNewOrderComfirmation && !IsLocking)
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
                result.Value = "Order_Delete";
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
                        if (e.Value.ToString().Equals("Order_Delete"))
                        {
                            int _jobID = 0;
                            if (JobID > 0) _jobID = JobID;
                            else
                            {
                                if (Session[ViewState["PageID"] + "SweetSoft_JobID"] != null)
                                    int.TryParse(Session[ViewState["PageID"] + "SweetSoft_JobID"].ToString(), out _jobID);
                            }
                            if (_jobID > 0)
                            {
                                ////Reset unit price 
                                //TblCylinderCollection coll = CylinderManager.SelectCylinderByJobID(_jobID);
                                //if (coll != null && coll.Count > 0)
                                //{
                                //    foreach (var item in coll)
                                //    {
                                //        item.UnitPrice = 0;
                                //        item.TaxID = null;
                                //        item.TaxPercentage = null;
                                //        CylinderManager.Update(item);
                                //    }
                                //}

                                TblOrderConfirmation order = OrderConfirmationManager.SelectByID(_jobID);
                                if (order != null)
                                {
                                    if (JobManager.JobHasDO(order.JobID).Length > 0)//Nếu công việc đã tạo DO thì không cho xóa
                                    {
                                        string message = string.Format("There exists a <strong>Delivery Order</strong> has been created for this Order Confirmation: {0}", JobManager.JobHasDO(order.JobID));
                                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Error);
                                        OpenMessageBox(msg, null, false, false);
                                        return;
                                    }
                                    if (OrderConfirmationManager.Delete(order.JobID))
                                    {
                                        isDelete = true;
                                        if (AllowSaveLogging)
                                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.DELETE_ORDER_CONFIRMATION), FUNCTION_PAGE, order.ToJSONString());
                                    }
                                }
                                else
                                {
                                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_ORDER_CONFIRMATION), MSGButton.OK, MSGIcon.Error);
                                    OpenMessageBox(msg, null, false, false);
                                }
                            }
                            else
                            {
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_ORDER_CONFIRMATION), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msg, null, false, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            if (isDelete)
            {
                Response.Redirect("OrderConfirmationList.aspx");
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsNewOrderComfirmation = OrderConfirmationManager.IsNewOrderConfirmation(JobID);
            bool IsLocking = OrderConfirmationManager.IsOrderConfirmationLocking(JobID);

            if (!IsNewOrderComfirmation && !IsLocking)
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
            Response.Redirect("OrderConfirmationList.aspx");
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
                    ResetFiled(true);
                    txtName.Text = cust.Name;
                    txtCode.Text = cust.Code;
                    LoadJobNumber(cust.CustomerID);
                    BindContactPerson(cust.CustomerID);

                    txtPaymentTerms.Text = cust.PaymentID != 0 ? cust.PaymentNote : string.Empty;
                    txtDeliveryTerms.Text = cust.DeliveryID != 0 ? cust.DeliveryNote : string.Empty;
                }
            }
        }

        private void BindJobCurrency(short CurrencyID)
        {
            TblCurrency currency = new CurrencyManager().SelectByID(CurrencyID);
            if (currency != null)
            {
                decimal RMValue = new CurrencyManager().SelectExchangeRateByCurrencyID(CurrencyID, DateTime.Today);
                txtCurrencyValue.Text = currency.CurrencyValue.ToString();
                ltrCurrencyName.Text = currency.CurrencyName;
                txtRMValue.Text = RMValue.ToString("N4");//currency.RMValue.ToString();
                ltrBaseCurrency.Text = "RM";
            }
        }

        protected void ddlRevNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetFiled(false);
            int JobID = 0;
            if (int.TryParse(ddlRevNumber.SelectedValue, out JobID))
            {
                TblJob jObj = JobManager.SelectByID(JobID);
                if (jObj != null)
                {
                    TblOrderConfirmation oObj = OrderConfirmationManager.SelectByID(JobID);
                    if (oObj != null)
                    {
                        Session[ViewState["PageID"] + "SweetSoft_JobID"] = oObj.JobID;
                        LoadData();
                    }
                    else
                    {
                        Session[ViewState["PageID"] + "SweetSoft_JobID"] = null;
                        //InniJobQuotationPricing(JobID);
                        //LoadData();

                        txtJobName.Text = jObj.JobName;
                        txtDesign.Text = jObj.Design;

                        txtCustomerPO1.Text = jObj.CustomerPO1;
                        txtCustomerPO2.Text = jObj.CustomerPO2;

                        short CurrencyID = (jObj.CurrencyID == null ? (short)0 : (short)jObj.CurrencyID);
                        hCurrencyID.Value = CurrencyID.ToString();

                        //Có cho phép nhập thuế hay không?
                        if (!OrderConfirmationManager.HaveGST(JobID))//Nếu không cùng quốc gia thì chỉ cho nhập thuế mặc định
                        {  
                            //BindDDLTax(false);
                            BindDDLTax(true);
                        }

                        BindCylinders(jObj.JobID);
                        BindQuotationData(jObj.JobID);
                        LoadServiceJobDetail(jObj.JobID);
                        LoadOtherCharges(jObj.JobID);
                        BindJobCurrency(CurrencyID);
                        LoadTotalPrice();
                    }
                }
                upnlCylinder.Update();
            }
            else
            {
                pnRecord.Visible = true;
                pnListCylinder.Visible = false;
            }
        }

        protected void ddlJobNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadJobRevNumber(ddlJobNumber.SelectedValue);
            ddlRevNumber_SelectedIndexChanged(null, null);
        }

        #region GridView Cylinder
        private void LoadCylinders()
        {
            DataTable dt = Session[ViewState["PageID"] + "dtSource"] as DataTable;
            gvClinders.DataSource = dt;
            gvClinders.DataBind();

            upnlCylinder.Update();
        }

        private void BindCylinders(int JobID)
        {
            decimal TotalPrice = 0;
            DataTable dt = CylinderManager.SelectCylinderSelectForOrderConfirmation(JobID);
            if (dt.Rows.Count > 0)
            {
                divCylinder.Visible = true;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                    dt.Columns[i].AllowDBNull = true;
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal.TryParse(dt.Rows[0]["Total"].ToString(), out TotalPrice);
                    pnListCylinder.Visible = true;
                    divCylinder.Visible = true;
                    pnRecord.Visible = false;
                }
                else
                {
                    divCylinder.Visible = false;
                    pnListCylinder.Visible = false;
                    pnRecord.Visible = true;
                }

                gvClinders.DataSource = dt;
                gvClinders.DataBind();
                Session[ViewState["PageID"] + "dtSource"] = dt;

                upnlCylinder.Update();
            }
        }
        #endregion

        private void LoadJobRevNumber(string JobNumber)
        {
            //ddlRevNumber.Items.Clear();
            ddlRevNumber.DataSource = JobManager.SelectRevNumberByJobNumber(JobNumber, 1);
            ddlRevNumber.DataTextField = "Value";
            ddlRevNumber.DataValueField = "Key";
            ddlRevNumber.DataBind();
            //upnlJobRev.Update();
        }

        private void LoadJobNumber(int custID)
        {
            btnPricesLookup.Enabled = true;
            ddlJobNumber.DataSource = JobManager.SelectJobNumberByCustomerID(custID, true, true);
            ddlJobNumber.DataBind();
            upnlJobRev.Update();
            ddlJobNumber_SelectedIndexChanged(null, null);
        }

        public static string CreateOrderbNumber()
        {
            string _No = "1" + DateTime.Today.ToString("yy") + "1";
            string _MaxNumber = new Select(Aggregate.Max(TblOrderConfirmation.OCNumberColumn))
                .From(TblOrderConfirmation.Schema)
                .Where(TblOrderConfirmation.OCNumberColumn).Like(_No + "%")
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


        protected string ShowNumberFormat(object obj, string Format)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString(Format) : "0"; }
            return strPrice;
        }



        private void SaveData()
        {
            int JobID = 0;
            if (Request.QueryString["ID"] != null)
                int.TryParse(Request.QueryString["ID"].ToString(), out JobID);

            if (string.IsNullOrEmpty(txtOrderDate.Text))
            {
                AddErrorPrompt(txtOrderDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (ddlJobNumber.SelectedValue == "0")
            {
                AddErrorPrompt(ddlJobNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (ddlRevNumber.SelectedValue == "0")
            {
                AddErrorPrompt(ddlRevNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (ddlTax.SelectedValue == "0")
            {
                AddErrorPrompt(ddlTax.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(txtRMValue.Text.Trim()))
            {
                AddErrorPrompt(txtRMValue.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(txtCurrencyValue.Text.Trim()))
            {
                AddErrorPrompt(txtCurrencyValue.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(ddlContact.SelectedValue))
            {
                AddErrorPrompt(ddlContact.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(ddlJobNumber.SelectedValue))
            {
                AddErrorPrompt(ddlJobNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(ddlRevNumber.SelectedValue))
            {
                AddErrorPrompt(ddlRevNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            string link = string.Empty;

            //Session[ViewState["PageID"] + "SweetSoft_JobID"] = null
            if (IsValid)
            {

                int.TryParse(ddlRevNumber.SelectedValue, out JobID);
                if (JobID == 0)
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_JOB), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    CloseMessageBox();
                    return;
                }

                int ContactPersonID = 0; int.TryParse(ddlContact.SelectedValue, out ContactPersonID);
                if (ContactPersonID == 0)
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CONTACT_PERSON), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    CloseMessageBox();
                    return;
                }

                //Kiểm tra quyền
                if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    CloseMessageBox();
                    return;
                }

                TblOrderConfirmation obj = OrderConfirmationManager.SelectByID(JobID);
                if (obj != null)
                {
                    obj.CustomerPO1 = txtCustomerPO1.Text.Trim();
                    obj.CustomerPO2 = txtCustomerPO2.Text.Trim();
                    obj.ContactPersonID = ContactPersonID;

                    DateTime OrderDate = DateTimeHelper.MinDateTimeValue;
                    if (!DateTime.TryParseExact(txtOrderDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out OrderDate))
                    {
                        OrderDate = DateTimeHelper.MinDateTimeValue;
                        AddErrorPrompt(txtOrderDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.DATE_VALIDATION));
                    }
                    obj.OrderDate = OrderDate;

                    //Thuế
                    short taxID = 0; short.TryParse(ddlTax.SelectedValue, out taxID);
                    TblTax tax = new TaxManager().SelectByID(taxID);
                    if (tax != null)
                    {
                        obj.TaxID = tax.TaxID;
                        obj.TaxPercentage = tax.TaxPercentage;
                    }
                    else
                    {
                        obj.TaxID = null;
                        obj.TaxPercentage = 0;
                    }

                    short currencyID = 0;
                    short.TryParse(hCurrencyID.Value, out currencyID);
                    obj.CurrencyID = currencyID;
                    decimal RMValue = 0; decimal.TryParse(txtRMValue.Text.Trim(), out RMValue);
                    obj.RMValue = RMValue;
                    decimal CurrencyValue = 0; decimal.TryParse(txtCurrencyValue.Text.Trim(), out CurrencyValue);
                    obj.CurrencyValue = CurrencyValue;
                    float discount = 0; float.TryParse(txtDiscount.Text.Trim(), out discount);
                    obj.Discount = discount;
                    obj.Remark = txtRemark.Text;
                    //obj.RemarkScreen = txtRemarkScreen.Text;
                    obj.DeliveryTerm = txtDeliveryTerms.Text;
                    obj.PaymentTerm = txtPaymentTerms.Text;

                    decimal totalPrice = 0; decimal.TryParse(txtSubTotal.Text, out totalPrice);
                    obj.TotalPrice = Math.Round(totalPrice,2);
                    obj = OrderConfirmationManager.Update(obj);
                    if (obj != null)
                    {

                        SaveOtherCharges(obj.JobID);
                        SaveServiceJobDetail(obj.JobID);
                        OrderConfirmationManager.ResetTotalPriceForInvoice(obj.JobID);
                        Session[ViewState["PageID"] + "SweetSoft_JobID"] = obj.JobID;
                        AllowEdit(false);
                        if (AllowSaveLogging)
                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.UPDATE_ORDER_CONFIRMATION), FUNCTION_PAGE, obj.ToJSONString());

                        /// Trunglc Add - 23-04-2015
                        /// Update Status Lock Of Job
                        ///

                        //OrderConfirmationManager.LockJob(obj.JobID);
                        // End

                        LoadData();
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.ORDER_CONFIRMATION_UPDATED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                        CloseMessageBox();
                        //ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintOrderConfirmation.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", obj.JobID);
                        upnPrinting.Update();
                    }
                    //else
                    //    SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.UPDATE_ORDER_CONFIRMATION), FUNCTION_PAGE, ResourceTextManager.GetApplicationText(ResourceText.ORDER_CONFIRMATION_UPDATE_FAILED));
                }
                else
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        CloseMessageBox();
                        return;
                    }

                    obj = new TblOrderConfirmation();
                    obj.JobID = JobID;
                    obj.OCNumber = CreateOrderbNumber();
                    obj.CustomerPO1 = txtCustomerPO1.Text.Trim();
                    obj.CustomerPO2 = txtCustomerPO2.Text.Trim();
                    obj.ContactPersonID = ContactPersonID;

                    DateTime OrderDate = DateTimeHelper.MinDateTimeValue;
                    if (!DateTime.TryParseExact(txtOrderDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out OrderDate))
                    {
                        OrderDate = DateTimeHelper.MinDateTimeValue;
                        AddErrorPrompt(txtOrderDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.DATE_VALIDATION));
                    }
                    obj.OrderDate = OrderDate;


                    //Thuế
                    short taxID = 0; short.TryParse(ddlTax.SelectedValue, out taxID);
                    TblTax tax = new TaxManager().SelectByID(taxID);
                    if (tax != null)
                    {
                        obj.TaxID = tax.TaxID;
                        obj.TaxPercentage = tax.TaxPercentage;
                    }
                    else
                    {
                        obj.TaxID = null;
                        obj.TaxPercentage = 0;
                    }

                    short currencyID = 0;
                    short.TryParse(hCurrencyID.Value, out currencyID);
                    obj.CurrencyID = currencyID;
                    decimal RMValue = 0; decimal.TryParse(txtRMValue.Text.Trim(), out RMValue);
                    obj.RMValue = RMValue;
                    decimal CurrencyValue = 0; decimal.TryParse(txtCurrencyValue.Text.Trim(), out CurrencyValue);
                    obj.CurrencyValue = CurrencyValue;
                    float discount = 0; float.TryParse(txtDiscount.Text.Trim(), out discount);
                    obj.Discount = discount;
                    obj.Remark = txtRemark.Text;
                    //obj.RemarkScreen = txtRemarkScreen.Text;
                    obj.DeliveryTerm = txtDeliveryTerms.Text;
                    obj.PaymentTerm = txtPaymentTerms.Text;
                    decimal totalPrice = 0; decimal.TryParse(txtSubTotal.Text, out totalPrice);
                    obj.TotalPrice = Math.Round(totalPrice, 2);

                    obj = OrderConfirmationManager.Insert(obj);
                    if (obj != null)
                    {
                        SaveOtherCharges(obj.JobID);
                        SaveServiceJobDetail(obj.JobID);
                        AllowEdit(false);

                        if (AllowSaveLogging)
                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.INSERT_ORDER_CONFIRMATION), FUNCTION_PAGE, obj.ToJSONString());

                        /// Trunglc Add - 23-04-2015
                        /// Update Status Lock Of Job
                        ///

                        //OrderConfirmationManager.LockJob(obj.JobID);

                        // End

                        Session[ViewState["PageID"] + "SweetSoft_JobID"] = obj.JobID;
                        LoadData();

                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.ORDER_CONFIRMATION_ADDED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                        CloseMessageBox();
                        //ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintOrderConfirmation.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", obj.JobID);
                    }
                    //else
                    //    SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.INSERT_ORDER_CONFIRMATION), FUNCTION_PAGE, ResourceTextManager.GetApplicationText(ResourceText.ORDER_CONFIRMATION_INSERT_FAILED));
                }
            }
            if (!IsValid)
                ShowErrorPromptExtension();
        }

        protected void btnPricesLookup_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            short taxID = 0; short.TryParse(ddlTax.SelectedValue, out taxID);
            double taxRate = 0;

            TblTax tax = new TaxManager().SelectByID(taxID);
            if (tax != null)
            {
                txtTaxRate.Text = tax.TaxPercentage.ToString();
                taxRate = tax.TaxPercentage;
            }

            List<ServiceJobDetailExtension> coll = (List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"];
            if (coll != null && coll.Count > 0)
            {
                foreach (var item in coll)
                {
                    if (taxID > 0)
                        item.TaxID = taxID;
                    else
                        item.TaxID = null;
                    item.TaxPercentage = (double)taxRate;
                }
            }

            Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
            BindServiceJobDetail();
            LoadTotalPrice();
        }


        #region Other charges
        private void BindOtherCharge()
        {
            List<OtherChargesExtension> coll = (List<OtherChargesExtension>)Session[ViewState["PageID"] + "OtherCharges"];
            grvOtherCharges.DataSource = coll;
            grvOtherCharges.DataBind();
        }

        private void LoadOtherCharges(int jobID)
        {
            List<OtherChargesExtension> coll = OrderConfirmationManager.SelectOtherChargeByJobID(jobID);
            if (coll.Count > 0)
            {
                divOrderCharger.Visible = true;
                grvOtherCharges.DataSource = coll;
                grvOtherCharges.DataBind();
                Session[ViewState["PageID"] + "OtherCharges"] = coll;
            }
        }

        private void RemoveInvalidCharges()
        {
            List<OtherChargesExtension> coll = new List<OtherChargesExtension>();
            coll.AddRange(((List<OtherChargesExtension>)Session[ViewState["PageID"] + "OtherCharges"]).Where(x => x.Charge != 0 || x.Quantity != 0 || !string.IsNullOrEmpty(x.Description)).ToList());
            Session[ViewState["PageID"] + "OtherCharges"] = coll;
        }

        protected void grvOtherCharges_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ////Kiểm tra quyền
            //if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            //{
            //    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
            //    OpenMessageBox(msgRole, null, false, false);
            //    return;
            //}

            RemoveInvalidCharges();
            grvOtherCharges.EditIndex = e.NewEditIndex;
            BindOtherCharge();
        }

        protected void grvOtherCharges_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvOtherCharges.DataKeys[e.RowIndex].Value);
            //TextBox txtDescription = grvOtherCharges.Rows[e.RowIndex].FindControl("txtDescription") as TextBox;
            //DropDownList ddlPricing = grvOtherCharges.Rows[e.RowIndex].FindControl("ddlPricing") as DropDownList;
            //ExtraInputMask txtQuantity = (ExtraInputMask)grvOtherCharges.Rows[e.RowIndex].FindControl("txtQuantity");
            ExtraInputMask txtCharge = (ExtraInputMask)grvOtherCharges.Rows[e.RowIndex].FindControl("txtCharge");

            //if (ddlPricing != null && ddlPricing.SelectedValue == "0")
            //{
            //    AddErrorPrompt(ddlPricing.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //}
            if (string.IsNullOrEmpty(txtCharge.Text))
            {
                AddErrorPrompt(txtCharge.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //if (string.IsNullOrEmpty(txtQuantity.Text))
            //{
            //    AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //}

            if (IsValid)
            {
                //string description = txtDescription != null ? txtDescription.Text.Trim() : string.Empty;
                decimal Charge = 0;
                //int PricingID = Convert.ToInt32(ddlPricing.SelectedValue);
                //string PricingName = ddlPricing.SelectedItem.Text;
                if (txtCharge != null) decimal.TryParse(txtCharge.Text, out Charge);

                //int quantity = 0;
                //if (txtQuantity != null) int.TryParse(txtQuantity.Text.Trim(), out quantity);

                updateOtherChargesRow(e.RowIndex, ID, Charge, e.RowIndex);
                grvOtherCharges.EditIndex = -1;
                BindOtherCharge();
                LoadTotalPrice();
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        //Cập nhật thông tin orther charges
        private void updateOtherChargesRow(int rowIndex, int OtherChargesID, decimal Charge, int index)
        {
            List<OtherChargesExtension> coll = Session[ViewState["PageID"] + "OtherCharges"] as List<OtherChargesExtension>;
            if (coll != null && coll.Count > 0)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    if (coll[i].OtherChargesID == OtherChargesID && i == index)
                    {
                        coll[i].GLCode = string.Empty;
                        coll[i].Charge = Charge;
                    }
                }
            }
            Session[ViewState["PageID"] + "OtherCharges"] = coll;
        }

        protected void grvOtherCharges_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                ////Kiểm tra quyền
                //if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                //{
                //    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                //    OpenMessageBox(msgRole, null, false, false);
                //    return;
                //}

                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                List<OtherChargesExtension> coll = (List<OtherChargesExtension>)Session[ViewState["PageID"] + "OtherCharges"];
                if (coll != null && coll.Count > 0)
                    coll.RemoveAt(RowIndex);

                Session[ViewState["PageID"] + "OtherCharges"] = coll;
                BindOtherCharge();
                LoadTotalPrice();
            }
        }
        protected void btnAddOtherCharges_Click(object sender, EventArgs e)
        {
            ////Kiểm tra quyền
            //if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            //{
            //    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
            //    OpenMessageBox(msgRole, null, false, false);
            //    return;
            //}

            List<OtherChargesExtension> Coll = ((List<OtherChargesExtension>)Session[ViewState["PageID"] + "OtherCharges"]);
            if (Coll == null)
                Coll = new List<OtherChargesExtension>();

            OtherChargesExtension itemNew = new OtherChargesExtension();
            itemNew.OtherChargesID = 0;
            itemNew.GLCode = string.Empty;
            itemNew.Description = string.Empty;
            itemNew.PricingID = 0;
            itemNew.PricingName = string.Empty;
            itemNew.Charge = 0;
            itemNew.JobID = 0;
            itemNew.Quantity = 0;
            Coll.Add(itemNew);

            Session[ViewState["PageID"] + "OtherCharges"] = Coll;
            grvOtherCharges.EditIndex = Coll.Count - 1;
            BindOtherCharge();
        }

        protected void grvOtherCharges_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grvOtherCharges_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RemoveInvalidCharges();
            grvOtherCharges.EditIndex = -1;
            BindOtherCharge();
        }

        protected void grvOtherCharges_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        int CustomerID = 0;
            //        int.TryParse(hCustomerID.Value, out CustomerID);
            //        short CurrencyID = 0;
            //        short.TryParse(hCurrencyID.Value, out CurrencyID);
            //        int? PricingID = (int?)grvOtherCharges.DataKeys[e.Row.RowIndex].Values[1];

            //        DropDownList ddlPricing = e.Row.FindControl("ddlPricing") as DropDownList;
            //        if(ddlPricing != null)
            //        {
            //            ddlPricing.DataSource = CustomerQuotationManager.SelectQuotationOtherChargesForDDL(CustomerID, CurrencyID);
            //            ddlPricing.DataValueField = "ID";
            //            ddlPricing.DataTextField= "Description";
            //            ddlPricing.DataBind();
            //            if (PricingID != null)
            //                ddlPricing.SelectedValue = PricingID.ToString();
            //        }
            //    }
            //}
            //catch
            //{
            //}
        }

        public void SaveOtherCharges(int jobID)
        {
            if (divOrderCharger.Visible)
            {
                List<OtherChargesExtension> Coll = ((List<OtherChargesExtension>)Session[ViewState["PageID"] + "OtherCharges"]);
                if (Coll != null && Coll.Count > 0)
                {
                    List<int> List = OrderConfirmationManager.SelectListOtherChargeIDByJobID(jobID);
                    foreach (var item in Coll)
                    {
                        int ID = item.OtherChargesID;
                        string glCode = item.GLCode;
                        string descript = item.Description;
                        decimal charge = item.Charge.HasValue ? item.Charge.Value : 0;
                        int quantity = item.Quantity.HasValue ? item.Quantity.Value : 0;
                        if (ID > 0)
                        {
                            TblOtherCharge OtherCharge = OrderConfirmationManager.SelectOtherChargeByID(ID);
                            if (item != null)
                            {
                                if (List != null && List.Count > 0)
                                {
                                    if (List.Contains(ID))
                                    {
                                        item.GLCode = glCode;
                                        item.Description = descript;
                                        item.Charge = charge;
                                        item.Quantity = quantity;
                                        OrderConfirmationManager.UpdateOtherCharge(item);
                                        List.Remove(ID);
                                    }
                                }
                            }
                        }
                        else
                        {
                            TblOtherCharge OtherCharge = new TblOtherCharge();
                            item.GLCode = glCode;
                            item.Description = descript;
                            item.Charge = charge;
                            item.JobID = jobID;
                            item.Quantity = quantity;
                            OrderConfirmationManager.InsertOtherCharge(item);
                        }
                    }

                    if (List != null && List.Count > 0)
                    {
                        foreach (var item in List)
                        {
                            OrderConfirmationManager.DeleteOtherCharge(item);
                        }
                    }
                }
                else
                {
                    OrderConfirmationManager.DeleteOtherChargeByJobID(jobID);
                }
            }
        }
        #endregion
        #region prices lookup
        //Lấy các giá trị price trong bảng tblCustomerQuotation_Pricing add to tblJobQuotationPricing
        private void ResetPrice()
        {
            int _jobID = 0;
            if (JobID > 0)
                _jobID = JobID;
            else
                int.TryParse(ddlRevNumber.SelectedValue, out _jobID);
            if (_jobID > 0)
            {
                //Lấy danh sách trục trong Job
                TblCylinderCollection coll = CylinderManager.SelectCylinderByJobID(_jobID);

                //Lấy thông tin Job
                TblJob job = JobManager.SelectByID(_jobID);
                if (job != null)
                {
                    int QuotationRev = Convert.ToInt32(ddlQuotationRev.SelectedItem.Text);
                    int QuotationID = Convert.ToInt32(ddlQuotationRev.SelectedItem.Value);

                    if (QuotationRev == 0)
                    {
                        TblCustomerQuotationDetailCollection pricingColl = CustomerQuotationManager.SelectAllDetail(job.CustomerID);
                        if (pricingColl != null && pricingColl.Count > 0)
                        {
                            foreach (var item in pricingColl)
                            {
                                UpdateUnitPriceCylinder(coll, item.Id, item.NewSteelBase, item.OldSteelBase);
                            }
                        }
                    }
                    else
                    {
                        List<JobQuotationDetailExtension> pricingColl = JobQuotationManager.SelectDetail(QuotationID, job.JobID);
                        if (pricingColl != null && pricingColl.Count > 0)
                        {
                            foreach (var item in pricingColl)
                            {
                                UpdateUnitPriceCylinder(coll, item.PricingID, item.NewSteelBasePrice, item.OldSteelBasePrice);
                            }
                        }
                    }
                    OrderConfirmationManager.ResetTotalPriceForOC(job.JobID);
                    OrderConfirmationManager.ResetTotalPriceForInvoice(job.JobID);

                }
                //LoadData();
                //BindQuotationData(_jobID);
                BindCylinders(_jobID);
                //LoadServiceJobDetail(_jobID);
                //LoadOtherCharges(_jobID);
                lbQuotationMessage.Text = "Job quotation has been reset!";
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetPrice();
                LoadTotalPrice();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private static void UpdateUnitPriceCylinder(TblCylinderCollection coll, int pricingID, decimal newPrice, decimal oldPrice)
        {
            if (coll != null && coll.Count > 0)
            {
                foreach (var item in coll)
                {
                    if (item.PricingID == pricingID)
                    {
                        if (Convert.ToBoolean(item.SteelBase))
                            item.UnitPrice = newPrice;
                        else
                            item.UnitPrice = oldPrice;
                        CylinderManager.Update(item);
                    }
                }
            }
        }

        private void FixPrice()
        {
            int _jobID = 0;
            if (JobID > 0) _jobID = JobID;
            else int.TryParse(ddlRevNumber.SelectedValue, out _jobID);

            if (_jobID > 0)
            {
                TblJob job = JobManager.SelectByID(_jobID);
                if (job != null)
                {
                    TblCylinderCollection coll = CylinderManager.SelectCylinderByJobID(_jobID);

                    DateTime? QuotationDate = (DateTime?)null; DateTime _QuotationDate = new DateTime();
                    if (DateTime.TryParseExact(txtQuotationDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _QuotationDate))
                        QuotationDate = _QuotationDate;
                    else
                        QuotationDate = DateTime.Today;
                    //Lấy quotation mới nhất
                    TblJobQuotation newestQuote = JobQuotationManager.SelectNewestQuotationByJobID(job.JobID);
                    //Lấy thông tin quotation của job hiện tại
                    TblJobQuotation jobQuotation = new TblJobQuotation();
                    jobQuotation.JobID = job.JobID;
                    jobQuotation.QuotationNo = job.JobNumber;
                    jobQuotation.QuotationDate = QuotationDate;
                    jobQuotation.QuotationText = txtQuotationNote.Text;
                    jobQuotation.RevNumber = newestQuote != null ? (int)newestQuote.RevNumber + 1 : 1;
                    jobQuotation = JobQuotationManager.Insert(jobQuotation);

                    //if(jobQuotation != null)
                    foreach (GridViewRow r in grvPrices.Rows)
                    {
                        HiddenField hdfPricingID = r.Cells[0].FindControl("hdfPricingID") as HiddenField;
                        ExtraInputMask txtNewSteelBasePrice = r.Cells[0].FindControl("txtNewSteelBasePrice") as ExtraInputMask;
                        ExtraInputMask txtOldSteelBasePrice = r.Cells[0].FindControl("txtOldSteelBasePrice") as ExtraInputMask;

                        decimal NewPrice = 0;
                        if (txtNewSteelBasePrice != null)
                            decimal.TryParse(txtNewSteelBasePrice.Text, out NewPrice);
                        decimal OldPrice = 0;
                        if (txtOldSteelBasePrice != null)
                            decimal.TryParse(txtOldSteelBasePrice.Text, out OldPrice);

                        short pricingID = 0;
                        if (hdfPricingID != null)
                            short.TryParse(hdfPricingID.Value, out pricingID);

                        if (jobQuotation != null)
                        {
                            BindQuotationRev(job.JobID);
                            ddlQuotationRev.SelectedValue = jobQuotation.QuotationID.ToString();
                            if (pricingID > 0)
                            {
                                TblJobQuotationPricing obj = new TblJobQuotationPricing();
                                obj.QuotationID = jobQuotation.QuotationID;
                                obj.PricingID = pricingID;
                                obj.NewSteelBasePrice = NewPrice;
                                obj.OldSteelBasePrice = OldPrice;
                                obj = JobQuotationManager.InsertDetail(obj);
                                if (obj != null)
                                {
                                    UpdateUnitPriceCylinder(coll, pricingID, obj.NewSteelBasePrice, obj.OldSteelBasePrice);
                                }
                            }
                        }
                    }

                    OrderConfirmationManager.ResetTotalPriceForOC(job.JobID);
                    OrderConfirmationManager.ResetTotalPriceForInvoice(job.JobID);
                    //LoadData();
                    //BindQuotationData(_jobID);
                    //BindQuotationRev(_jobID);
                    BindCylinders(_jobID);
                    //LoadServiceJobDetail(_jobID);
                    //LoadOtherCharges(_jobID);
                    lbQuotationMessage.Text = "Job quotation has been fixed!";
                }
            }
        }
        protected void btnFixPrice_Click(object sender, EventArgs e)
        {
            try
            {
                FixPrice();
                LoadTotalPrice();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }
        #endregion

        private void LoadTotalPrice()
        {
            decimal totalPrice = CalculatorTotalPrice();
            txtSubTotal.Text = totalPrice.ToString("N2");

            //Trừ discount 
            double discount = 0; double.TryParse(txtDiscount.Text.Trim(), out discount);
            decimal priceDiscount = totalPrice * (1 - (decimal)discount / 100);
            txtNetTotal.Text = priceDiscount.ToString("N2");

            double taxRate = 0; double.TryParse(txtTaxRate.Text.Trim().Replace(" %", ""), out taxRate);
            decimal gstPrice = priceDiscount * (decimal)taxRate / 100;
            txtGSTAt.Text = gstPrice.ToString("N2");

            //Tính thuế
            decimal priceTaxed = priceDiscount * (1 + (decimal)taxRate / 100);
            txtTotalPrice.Text = priceTaxed.ToString("N2");
        }

        private decimal CalculatorTotalPrice()
        {
            //Cylinder Total Prices
            decimal totalPrice = 0;
            decimal totalPriceCyLindes = 0;
            for (int i = 0; i < gvClinders.Rows.Count; i++)
            {
                HiddenField hdfPriceTaxed = gvClinders.Rows[i].FindControl("hdfPriceTaxed") as HiddenField;
                if (hdfPriceTaxed != null)
                {
                    decimal price = 0;
                    decimal.TryParse(hdfPriceTaxed.Value, out price);
                    totalPriceCyLindes = totalPriceCyLindes + Math.Round(price, 2);
                }

            }

            //Other charges total prices
            decimal totalPriceOther = 0;
            for (int i = 0; i < grvOtherCharges.Rows.Count; i++)
            {
                Label lblCharge = grvOtherCharges.Rows[i].FindControl("lblCharge") as Label;
                Label lblQuantity = grvOtherCharges.Rows[i].FindControl("lblQuantity") as Label;

                if (lblCharge != null && lblQuantity != null)
                {
                    decimal price = 0; decimal.TryParse(lblCharge.Text, out price);
                    decimal quantity = 0; decimal.TryParse(lblQuantity.Text, out quantity);

                    totalPriceOther = totalPriceOther + Math.Round(price * quantity, 2);
                }

            }

            //Service Job Total Prices
            decimal servicePrice = 0;
            for (int i = 0; i < grvServiceJobDetail.Rows.Count; i++)
            {
                Label lblWorkOrderValues = grvServiceJobDetail.Rows[i].FindControl("lblWorkOrderValues") as Label;
                if (lblWorkOrderValues != null)
                {
                    decimal price = 0; decimal.TryParse(lblWorkOrderValues.Text, out price);
                    servicePrice = servicePrice + Math.Round(price, 2);
                }

            }

            //Tổng tiền
            totalPrice = totalPriceCyLindes + totalPriceOther + servicePrice;
            return totalPrice;
        }

        #region Service job detail
        protected void btnAddServiceDetail_Click(object sender, EventArgs e)
        {
            short taxId = 0; short.TryParse(ddlTax.SelectedValue, out taxId);
            double taxRate = 0; double.TryParse(txtTaxRate.Text.Trim(), out taxRate);

            TblServiceJobDetailCollection Coll = ((TblServiceJobDetailCollection)Session[ViewState["PageID"] + "ServiceJobDetail"]);
            TblServiceJobDetail itemNew = new TblServiceJobDetail();
            itemNew.ServiceJobID = 0;
            itemNew.JobID = 0;
            itemNew.WorkOrderNumber = string.Empty;
            itemNew.ProductID = string.Empty;
            itemNew.Description = string.Empty;
            itemNew.WorkOrderValues = 0;

            if (taxId > 0)
                itemNew.TaxID = taxId;
            else
                itemNew.TaxID = null;
            itemNew.TaxPercentage = taxRate;

            Coll.Add(itemNew);
            Session[ViewState["PageID"] + "ServiceJobDetail"] = Coll;
            BindServiceJobDetail();
        }

        private void BindServiceJobDetail()
        {
            item1 = string.Empty;
            item2 = string.Empty;
            count = 1;

            List<ServiceJobDetailExtension> coll = (List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"];
            if (coll.Count > 0)
            {
                divServiceJob.Visible = true;
                grvServiceJobDetail.DataSource = coll;
                grvServiceJobDetail.DataBind();
                upnlServiceJobDetail.Update();
            }
        }

        protected decimal TotalPrice = 0;
        private void LoadServiceJobDetail(int jobID)
        {
            //divTax
            item1 = string.Empty;
            item2 = string.Empty;
            count = 1;

            List<ServiceJobDetailExtension> coll = JobManager.SelectServiceJobDetailByID(jobID);
            grvServiceJobDetail.DataSource = coll;
            grvServiceJobDetail.DataBind();

            if (coll != null && coll.Count > 0)
                divServiceJob.Visible = true;
            else
                divServiceJob.Visible = false;

            Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
            BindServiceJobDetail();
        }

        protected void grvServiceJobDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ////Kiểm tra quyền
            //if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            //{
            //    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
            //    OpenMessageBox(msgRole, null, false, false);
            //    return;
            //}

            grvServiceJobDetail.EditIndex = e.NewEditIndex;
            BindServiceJobDetail();
        }

        protected void grvServiceJobDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            double taxRate = 0; double.TryParse(txtTaxRate.Text.Trim(), out taxRate);
            short taxID = 0; short.TryParse(ddlTax.SelectedValue.Trim(), out taxID);

            int ID = Convert.ToInt32(grvServiceJobDetail.DataKeys[e.RowIndex].Value);
            ExtraInputMask txtWorkOrderValues = (ExtraInputMask)grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtWorkOrderValues");
            if (txtWorkOrderValues != null && string.IsNullOrEmpty(txtWorkOrderValues.Text))
            {
                AddErrorPrompt(txtWorkOrderValues.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (IsValid)
            {
                string strWorkOrderValues = txtWorkOrderValues != null ? txtWorkOrderValues.Text : string.Empty;
                decimal WorkOrderValues = 0; decimal.TryParse(strWorkOrderValues, out WorkOrderValues);

                updateRow(e.RowIndex, ID, WorkOrderValues, taxID, taxRate);
                grvServiceJobDetail.EditIndex = -1;
                BindServiceJobDetail();
                LoadTotalPrice();
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        //Cập nhật chi tiết ServiceJob
        private void updateRow(int rowIndex, int ServiceJobID, decimal WorkOrderValues, short taxID, double taxRate)
        {
            List<ServiceJobDetailExtension> coll = Session[ViewState["PageID"] + "ServiceJobDetail"] as List<ServiceJobDetailExtension>;
            if (coll != null && coll.Count > 0)
            {
                ServiceJobDetailExtension obj = new ServiceJobDetailExtension();
                obj = coll.Where(x => x.ServiceJobID == ServiceJobID).FirstOrDefault();
                if (obj != null)
                {
                    if (taxID > 0)
                        obj.TaxID = taxID;
                    else
                        obj.TaxID = null;

                    obj.WorkOrderValues = WorkOrderValues;
                    obj.TaxPercentage = taxRate;
                }
            }
            Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
        }

        private string item1 = string.Empty;
        private string item2 = string.Empty;
        private int count = 1;
        protected void grvServiceJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
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
                                lblNo.Text = "1";
                        }
                        item1 = item.WorkOrderNumber;
                        item2 = item.ProductID;
                    }
                }
            }
            catch
            {
            }
        }

        protected void grvServiceJobDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grvServiceJobDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ////Kiểm tra quyền
            //if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            //{
            //    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
            //    OpenMessageBox(msgRole, null, false, false);
            //    return;
            //}
            grvServiceJobDetail.EditIndex = -1;
            BindServiceJobDetail();
        }

        public void SaveServiceJobDetail(int jobID)
        {
            if (divServiceJob.Visible)
            {
                List<ServiceJobDetailExtension> Coll = ((List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"]);
                if (Coll != null && Coll.Count > 0)
                {
                    foreach (var item in Coll)
                    {
                        TblServiceJobDetail obj = JobManager.SelectServiceJobDetailById(item.ServiceJobID);
                        if (obj != null)
                        {
                            obj.WorkOrderValues = item.WorkOrderValues;
                            obj.TaxID = item.TaxID == 0 ? (short?)null : item.TaxID;
                            obj.TaxPercentage = item.TaxPercentage;
                        }
                        JobManager.UpdateServiceJobDetail(obj);
                    }
                }
            }
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

        protected void btnDiscountChanged_Click(object sender, EventArgs e)
        {
            LoadTotalPrice();
        }

        protected void ddlPricing_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlPricing = sender as DropDownList;
            int PricingID = Convert.ToInt32(ddlPricing.SelectedValue);
            TblCustomerQuotationOtherCharge obj = CustomerQuotationManager.SelectOtherChargesByID(PricingID);
            ExtraInputMask txtCharge = grvOtherCharges.Rows[grvOtherCharges.EditIndex].FindControl("txtCharge") as ExtraInputMask;
            if (txtCharge != null)
                txtCharge.Text = obj != null ? obj.Price.ToString("N3") : "0.000";
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
            UpdateOrderConfirmationLockStatus(true);
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }
            UpdateOrderConfirmationLockStatus(false);
        }

        private void UpdateOrderConfirmationLockStatus(bool IsLock)
        {
            if (this.JobID > 0)
            {
                OrderConfirmationManager.LockOrUnlockOrderConfirmation(JobID, IsLock);

                //bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
                bool IsAllowEdit = !IsLock ? true : false;

                AllowEditting(IsAllowEdit);

                string KEY_MESSAGE = IsLock ? ResourceText.LOCK_ORDER_SAVE_SUCCESSFULLY : ResourceText.UNLOCK_ORDER_SAVE_SUCCESSFULLY;

                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(KEY_MESSAGE), MSGButton.OK, MSGIcon.Success);
                OpenMessageBox(msg, null, false, false);

            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_NOT_FOUND), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                ResetFiled(false);
            }
        }

        #endregion
    }
}