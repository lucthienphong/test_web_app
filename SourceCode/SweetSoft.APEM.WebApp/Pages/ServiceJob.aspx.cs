using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoftCMS.ExtraControls.Controls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class ServiceJob : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "service_job_manager";
            }
        }

        private int JobID
        {
            get
            {
                int _jobId = 0;
                if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"].ToString(), out _jobId);
                return _jobId;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                ApplyControlText();
                if (JobID > 0)
                {
                    Session[ViewState["PageID"] + "ID"] = JobID;
                    BindJobData();
                }
                else
                    ResetDataFields();

                LoadServiceJobDetail(JobID);
                LoadOtherCharges(JobID);
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvServiceJobDetail.Columns[0].HeaderText = "No";
            grvServiceJobDetail.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_NUMBER);
            grvServiceJobDetail.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.PRODUCTID);
            grvServiceJobDetail.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
        }

        #region JobDetail
        //JOB DETAIL----------------------------------------------------------------------------------------------------------------------------
        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        /// <summary>
        /// Tải danh sách liêu hệ của khách hàng
        /// </summary>
        private void LoadContactDDL()
        {
            int CustomerID = 0;
            int.TryParse(hCustomerID.Value, out CustomerID);
            List<TblContact> source = ContactManager.SelectAll(CustomerID);
            ddlContacts.DataSource = source;
            ddlContacts.DataTextField = "ContactName";
            ddlContacts.DataValueField = "ContactID";
            ddlContacts.DataBind();
        }

        private void BindSaleRepDDL()
        {
            List<TblStaffExeption> colls = StaffManager.ListForDDL(DepartmentSetting.Sale);
            ddlSaleRep.DataSource = colls;
            ddlSaleRep.DataTextField = "FullName";
            ddlSaleRep.DataValueField = "StaffID";
            ddlSaleRep.DataBind();
        }

        private void BindJobCoorRepDDL()
        {
            List<TblStaffExeption> colls = StaffManager.ListForDDL(DepartmentSetting.JobCoordinator);
            ddlJobCoordinator.DataSource = colls;
            ddlJobCoordinator.DataTextField = "FullName";
            ddlJobCoordinator.DataValueField = "StaffID";
            ddlJobCoordinator.DataBind();
        }


        private void BindCurrencyDDL()
        {
            TblCurrencyCollection list = new CurrencyManager().SelectAllForDDL();
            ddlCurrency.DataSource = list;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyID";
            ddlCurrency.DataBind();
        }

        private void BindProductTypeDDL()
        {
            List<TblReference> list = ReferenceTableManager.SelectProductTypeForDDL();
            ddlProductType.DataSource = list;
            ddlProductType.DataValueField = "ReferencesID";
            ddlProductType.DataTextField = "Name";
            ddlProductType.DataBind();
        }

        private void BindBrandOwnerDDL()
        {
            List<TblReference> list = ReferenceTableManager.SelectBrandOwnerForDDL();
            ddlBrandOwner.DataSource = list;
            ddlBrandOwner.DataValueField = "ReferencesID";
            ddlBrandOwner.DataTextField = "Name";
            ddlBrandOwner.DataBind();
        }
    
        private void BindDDL()
        {
            LoadContactDDL();
            BindSaleRepDDL();
            BindJobCoorRepDDL();
            BindCurrencyDDL();
            BindProductTypeDDL();
            BindBrandOwnerDDL();
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            LoadContactDDL();
            int CustomerID = 0;
            int.TryParse(hCustomerID.Value, out CustomerID);
            if (CustomerID != 0)
            {
                TblCustomer obj = CustomerManager.SelectByID(CustomerID);
                txtPaymentTerms.Text = obj != null ? (obj.PaymentID != 0 ? obj.PaymentNote : string.Empty) : string.Empty;
            }
        }

        private void BindJobData()
        {
            try
            {
                if (Session[ViewState["PageID"] + "ID"] == null)
                {
                    ResetDataFields();
                    return;
                }
                int ID;
                if (int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID))
                {
                    //Kiểm tra nhân viên còn tồn tại không
                    JobExtension obj = JobManager.SelectByID(ID);
                    if (obj != null && obj.IsServiceJob == 1)
                    {

                        //Bind JobDetail
                        txtCode.Text = obj.CustomerCode;
                        txtName.Text = obj.CustomerName;
                        hCustomerID.Value = obj.CustomerID.ToString();
                        txtJobNumber.Text = obj.JobNumber;
                        txtJobName.Text = obj.JobName;
                        txtDesign.Text = obj.Design;
                        txtCreatedDate.Text = obj.CreatedOn != null ? ((DateTime)obj.CreatedOn).ToString("dd/MM/yyyy") : "";
                        txtCreatedBy.Text = StaffManager.GetStaffFullNameByUserName(obj.CreatedBy);
                        txtItemCode.Text = obj.ItemCode;

                        ddlBrandOwner.SelectedValue = obj.BrandOwner != null ? obj.BrandOwner.ToString() : "0";

                        //Tải lại danh sách liên hệ của khách hàng
                        LoadContactDDL();
                        ddlContacts.SelectedValue = obj.ContactPersonID.ToString();
                        ddlSaleRep.SelectedValue = obj.SalesRepID.ToString();
                        ddlJobCoordinator.SelectedValue = obj.CoordinatorID != null ? obj.CoordinatorID.ToString() : "0";
                        ddlCurrency.SelectedValue = obj.CurrencyID.ToString();
                        ddlProductType.SelectedValue = obj.ProductTypeID.ToString();
                        if (!string.IsNullOrEmpty(obj.Status))
                        {JobStatus jobStatus = (JobStatus)Enum.Parse(typeof(JobStatus), obj.Status);}
                        
                        txtPaymentTerms.Text = obj.PaymentTerms;
                        //Disable Customer
                        txtName.Enabled = false;
                        //txtCode.Enabled = false;


                        ddlCurrency.Enabled = false;
                        if (Convert.ToBoolean(obj.IsClosed))
                        {
                            AllowEditting(false);
                        }
                        else
                        {
                            /**
                             * Trunglc Add
                             * 27-04-2015
                             */
                            bool IsLocking = JobManager.IsJobLocking(obj.JobID);
                            bool IsNewJob = JobManager.IsNewJob(obj.JobID);
                            bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
                            bool IsAllowEdit = IsNewJob ? true : (!IsLocking ? true : (IsLocking ? false : true));

                            AllowEditting(IsAllowEdit);

                        }

                        Session[ViewState["PageID"] + "ID"] = obj.JobID;
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_NOT_FOUND), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        ResetDataFields();
                    }
                }
                else
                {
                    ResetDataFields();
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void ResetDataFields()
        {
            //JobDetail
            txtCode.Text = "";
            txtName.Text = "";
            hCustomerID.Value = "";

            //BrandOwner
            ddlBrandOwner.SelectedIndex = 0;

            string JobNumber = JobManager.CreateJobNumber();
            txtJobNumber.Text = JobNumber;

            string barcode = string.Format("{0}-0", JobNumber.Replace("/", "-"));
            hBarcode.Value = barcode;
            txtItemCode.Text = "";
            txtJobName.Text = "";
            txtDesign.Text = "";
            txtCreatedBy.Text = StaffManager.GetStaffFullNameByUserName(ApplicationContext.Current.UserName);
            txtCreatedDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlContacts.SelectedIndex = 0;
            ddlSaleRep.SelectedIndex = 0;
            ddlJobCoordinator.SelectedIndex = 0;
            txtPaymentTerms.Text = "";
            //Disable Customer
            txtName.Enabled = true;
            //txtCode.Enabled = true;

            ddlCurrency.Enabled = true;

            Session[ViewState["PageID"] + "ID"] = null;
        }

        private void AllowEditting(bool yesno)
        {
            //Job detail
            ddlBrandOwner.Enabled = yesno;
            txtJobName.Enabled = yesno;
            txtDesign.Enabled = yesno;
            ddlContacts.Enabled = yesno;
            ddlSaleRep.Enabled = yesno;
            ddlJobCoordinator.Enabled = yesno;
            ddlProductType.Enabled = yesno;
            txtPaymentTerms.Enabled = yesno;
            txtItemCode.Enabled = yesno;
            //ServiceJob
            btnAddDetail.Visible = yesno;
            for (int i = grvOtherCharges.Columns.Count - 2; i < grvOtherCharges.Columns.Count; i++)
            {
                grvOtherCharges.Columns[i].Visible = yesno;
            }

            //Other charges
            btnAddOtherCharges.Visible = yesno;
            for (int i = grvServiceJobDetail.Columns.Count - 2; i < grvServiceJobDetail.Columns.Count; i++)
            {
                grvServiceJobDetail.Columns[i].Visible = yesno;
            }

            btnSave.Visible = yesno;
            //btnEngraving.Visible = yesno;
            //btnCancel.Visible = yesno;
            //btnSaveRevDetail.Visible = yesno;
            //btnGetCopy.Visible = yesno;

            ///Trunglc Add - 27-04-2015

            bool IsAllowLock = RoleManager.AllowLock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
            bool IsAllowUnlock = RoleManager.AllowUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
            bool IsNewJob = JobManager.IsNewJob(JobID);
            TblJob objJob = JobManager.SelectByID(JobID);
            if (Convert.ToBoolean(objJob.IsClosed))
            {
                btnLock.Visible = false;
                btnUnlock.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                bool IsJobLocking = JobManager.IsJobLocking(JobID);

                if (IsNewJob)
                {
                    btnLock.Visible = IsAllowLock ? true : false;
                    btnUnlock.Visible = false;
                }
                else
                {

                    btnLock.Visible = Convert.ToBoolean(objJob.IsClosed) ? false : IsNewJob && IsAllowLock ? true : ((!IsJobLocking && IsAllowLock && yesno ? true : false));
                    btnUnlock.Visible = Convert.ToBoolean(objJob.IsClosed) ? false : (IsJobLocking && IsAllowUnlock && !yesno ? true : false);
                }

                btnDelete.Visible = IsJobLocking ? false : true;
            }

            ///End
        }

        private void SaveJob()
        {
            //-------BEGIN VALIDATION
            //Customer
            int cusID = 0, brandOwner = 0;
            short currencyID = 0;
            if (string.IsNullOrEmpty(hCustomerID.Value))
            {
                AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else
            {
                if (int.TryParse(hCustomerID.Value.Trim(), out cusID))
                {
                    TblCustomer cObj = CustomerManager.SelectByID(cusID);
                    if (cObj == null)
                    {
                        AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                        AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                    }
                }
                else
                {
                    AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                    AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
            }
            int.TryParse(ddlBrandOwner.SelectedValue, out brandOwner);
            //JobNumber
            if(string.IsNullOrEmpty(txtJobNumber.Text.Trim()))
            {
                AddErrorPrompt(txtJobNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            
            //JobName
            if (string.IsNullOrEmpty(txtJobName.Text.Trim()))
            {
                AddErrorPrompt(txtJobName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //JobDesign
            if (string.IsNullOrEmpty(txtDesign.Text.Trim()))
            {
                AddErrorPrompt(txtDesign.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //Contact
            if (int.Parse(ddlContacts.SelectedValue) == 0)
            {
                AddErrorPrompt(ddlContacts.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //SaleRep
            if (int.Parse(ddlSaleRep.SelectedValue) == 0)
            {
                AddErrorPrompt(ddlSaleRep.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if(!short.TryParse(ddlCurrency.SelectedValue, out currencyID))
            {
                AddErrorPrompt(ddlCurrency.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (ddlProductType.SelectedValue == "0")
            {
                AddErrorPrompt(ddlProductType.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //-------END VALIDATION

            if (IsValid)
            {
                //LƯU KHÁCH HÀNG---------------------------------------------------------------
                TblJob obj = new TblJob();
                if (Session[ViewState["PageID"] + "ID"] != null)//Edit
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }
                    obj = JobManager.SelectByID(int.Parse(Session[ViewState["PageID"] + "ID"].ToString()));
                    if (obj == null)//Nếu không tồn tại khách hàng thì thông báo lỗi
                    {
                        MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(errMsg, null, false, false);
                        BindJobData();
                        return;
                    }
                    else if (obj.IsServiceJob == 0)
                    {
                        MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(errMsg, null, false, false);
                        BindJobData();
                        return;
                    }

                    if (Convert.ToBoolean(obj.IsClosed))
                    {
                        MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Can't save data because this job has done an revision.", MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(errMsg, null, false, false);
                        BindJobData();
                        return;
                    }

                    obj.JobName = txtJobName.Text.Trim();
                    obj.Design = txtDesign.Text.Trim();
                    obj.RootJobID = 0;
                    //obj.IsServiceJob = 1;
                    obj.PaymentTerms = string.Empty;
                    obj.ContactPersonID = int.Parse(ddlContacts.SelectedValue);
                    obj.SalesRepID = int.Parse(ddlSaleRep.SelectedValue) == 0 ? (int?)null : int.Parse(ddlSaleRep.SelectedValue);
                    obj.CoordinatorID = int.Parse(ddlJobCoordinator.SelectedValue) == 0 ? (int?)null : int.Parse(ddlJobCoordinator.SelectedValue);
                    obj.PaymentTerms = txtPaymentTerms.Text.Trim();
                    obj.CurrencyID = currencyID;
                    obj.ProductTypeID = int.Parse(ddlProductType.SelectedValue);
                    obj.ItemCode = txtItemCode.Text.Trim();
                    obj.BrandOwner = brandOwner;
                    obj = JobManager.Update(obj);
                    if (obj != null)
                    {
                        SaveServiceJobDetail(obj.JobID);
                        SaveOtherCharges(obj.JobID);
                        OrderConfirmationManager.ResetTotalPriceForOC(obj.JobID);
                        OrderConfirmationManager.ResetTotalPriceForInvoice(obj.JobID);
                        if (AllowSaveLogging)
                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.UPDATE_SERVICE_JOB), FUNCTION_PAGE, obj.ToJSONString());
                    }

                    ////Lưu nhật ký
                    //string xmlObj = Common.Helpers.Serialize<TblCustomer>(obj);
                    //LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, xmlObj);

                    Session[ViewState["PageID"] + "ID"] = obj.JobID;
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
                else//Add new
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }

                    obj = new TblJob();
                    obj.IsServiceJob = 1;
                    string numberMessage = string.Empty;
                    string barcode = string.Empty;
                    while(JobManager.JobNumberExists(obj.JobID, txtJobNumber.Text.Trim()))
                    {
                        string newNumber = JobManager.CreateJobNumber();
                        numberMessage = string.Format("<p style='text-align:left;'>- Job Number has changed from <b>{0}</b> to <b>{1}</b></p>", txtJobNumber.Text.Trim(), newNumber);
                        txtJobNumber.Text = newNumber;

                        barcode = string.Format("{0}-0", newNumber.Replace("/", "-"));
                        hBarcode.Value = barcode;
                    }
                    obj.JobNumber = txtJobNumber.Text.Trim();
                    obj.RevNumber = 0;
                    obj.JobBarcode = hBarcode.Value;
                    obj.JobBarcodeImage = Common.Code128Rendering.MakeBarcode64BaseImage(barcode, 1.5, false, true); ;
                    obj.JobName = txtJobName.Text.Trim();
                    obj.Design = txtDesign.Text.Trim();
                    obj.RootJobID = 0;
                    obj.RootJobRevNumber = string.Empty;
                    obj.CommonJobNumber = string.Empty;
                    obj.CustomerID = cusID;
                    obj.ContactPersonID = int.Parse(ddlContacts.SelectedValue);
                    obj.CurrencyID = currencyID;
                    obj.ProductTypeID = int.Parse(ddlProductType.SelectedValue);
                    obj.Status = string.Empty;
                    obj.PaymentTerms = txtPaymentTerms.Text;
                    obj.CreatedOn = DateTime.Now;
                    obj.CreatedBy = ApplicationContext.Current.UserName;
                    obj.PaymentTerms = txtPaymentTerms.Text;
                    obj.SalesRepID = int.Parse(ddlSaleRep.SelectedValue) == 0 ? (int?)null : int.Parse(ddlSaleRep.SelectedValue);
                    obj.CoordinatorID = int.Parse(ddlJobCoordinator.SelectedValue) == 0 ? (int?)null : int.Parse(ddlJobCoordinator.SelectedValue);
                    obj.IsClosed = 0;
                    obj.ItemCode = txtItemCode.Text.Trim();
                    obj.BrandOwner = brandOwner;
                    obj = JobManager.Insert(obj);
                    if (obj != null)
                    {
                        SaveServiceJobDetail(obj.JobID);
                        SaveOtherCharges(obj.JobID);

                        if (AllowSaveLogging)
                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.INSERT_SERVICE_JOB), FUNCTION_PAGE, obj.ToJSONString());
                    }


                    ////Lưu nhật ký
                    //string xmlObj = Common.Helpers.Serialize<TblCustomer>(obj);
                    //LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, xmlObj);
                    string message = string.Format("<p style='text-align:left; font-size:16px;'><b>Data saved successfully with change(s):</b></p>");
                    bool hasChanged = false;
                    
                    Session[ViewState["PageID"] + "ID"] = obj.CustomerID;
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), hasChanged ? message : ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
            }
            if (!IsValid)
                ShowErrorPromptExtension();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsNewJob = JobManager.IsNewJob(JobID);
            bool IsLocking = JobManager.IsJobLocking(JobID);

            if (!IsNewJob && !IsLocking)
            {
                if (!RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }
            }

            SaveJob();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //BindJobData();
            Response.Redirect("ServiceJobList.aspx");
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        if (e.Value.ToString().Equals("Service_Job_Delete"))
                        {
                            TblJob obj = JobManager.SelectByID(JobID);
                            if (obj != null && obj.IsServiceJob == 1)
                            {
                                if (JobManager.DeleteServiceJob(JobID))
                                {
                                    if (AllowSaveLogging)
                                        SaveLogging(ResourceTextManager.GetApplicationText(ActivityLogging.DELETE), FUNCTION_PAGE, obj.ToJSONString());
                                    Response.Redirect("ServiceJobList.aspx", false);
                                }
                            }
                        }

                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        BindData();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        #endregion



        #region Service job detail
        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            RemoverInvalidServices();
            List<ServiceJobDetailExtension> Coll = ((List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"]);
            ServiceJobDetailExtension itemNew = new ServiceJobDetailExtension();
            itemNew.ServiceJobID = 0;
            itemNew.JobID = 0;
            itemNew.WorkOrderNumber = string.Empty;
            itemNew.ProductID = string.Empty;
            itemNew.Description = string.Empty;
            itemNew.WorkOrderValues = 0;
            itemNew.PricingID = 0;
            itemNew.PricingName = string.Empty;
            itemNew.No = Coll.Count + 1;

            //Coll.Add(itemNew);
            //grvServiceJobDetail.EditIndex = Coll.Count - 1;
            Coll.Insert(0, itemNew);
            grvServiceJobDetail.EditIndex = 0;

            Session[ViewState["PageID"] + "ServiceJobDetail"] = Coll;
            BindServiceJobDetail();
        }

        private void RemoverInvalidServices()
        {
            List<ServiceJobDetailExtension> Coll = new List<ServiceJobDetailExtension>();
            Coll.AddRange(((List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"]).Where(x => !string.IsNullOrEmpty(x.ProductID) || !string.IsNullOrEmpty(x.WorkOrderNumber)).ToList());
            Session[ViewState["PageID"] + "ServiceJobDetail"] = Coll;
        }

        private void BindServiceJobDetail()
        {
            List<ServiceJobDetailExtension> coll = (List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"];

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
            int ID = Convert.ToInt32(grvServiceJobDetail.DataKeys[e.RowIndex].Value);
            TextBox txtWorkOrderNumber = grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtWorkOrderNumber") as TextBox;
            TextBox txtProductID = grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtProductID") as TextBox;
            TextBox txtDescription = grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtDescription") as TextBox;
            DropDownList ddlPricing = grvServiceJobDetail.Rows[e.RowIndex].FindControl("ddlPricing") as DropDownList;

            if (txtWorkOrderNumber != null && string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                AddErrorPrompt(txtWorkOrderNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (txtProductID != null && string.IsNullOrEmpty(txtProductID.Text))
            {
                AddErrorPrompt(txtProductID.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (ddlPricing != null && ddlPricing.SelectedValue == "0")
            {
                AddErrorPrompt(ddlPricing.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (IsValid)
            {
                string WorkOrderNumber = txtWorkOrderNumber != null ? txtWorkOrderNumber.Text : string.Empty;
                string ProductID = txtProductID != null ? txtProductID.Text : string.Empty;
                string Description = txtDescription != null ? txtDescription.Text : string.Empty;
                decimal WorkOrderValues = 0;
                int PricingID = Convert.ToInt32(ddlPricing.SelectedValue);
                string PricingName = ddlPricing.SelectedItem.Text;


                updateRow(e.RowIndex, ID, ProductID, Description, WorkOrderNumber, WorkOrderValues, PricingID, PricingName);
                grvServiceJobDetail.EditIndex = -1;
                BindServiceJobDetail();
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, int ServiceJobID, string ProductID, string description, string WorkOrderNumber, decimal WorkOrderValues, int PricingID, string PricingName)
        {
            List<ServiceJobDetailExtension> coll = Session[ViewState["PageID"] + "ServiceJobDetail"] as List<ServiceJobDetailExtension>;
            if (coll != null && coll.Count > 0)
            {
                TblCustomerQuotationAdditionalService obj = CustomerQuotationManager.SelectAdditionalByID(PricingID);
                coll[rowIndex].WorkOrderNumber = WorkOrderNumber;
                coll[rowIndex].ProductID = ProductID;
                coll[rowIndex].Description = description;
                coll[rowIndex].PricingID = PricingID;
                coll[rowIndex].PricingName = PricingName;
                coll[rowIndex].WorkOrderValues = obj != null ? obj.Price : 0;
            }
            coll.Sort((x, y) => x.No.CompareTo(y.No));
            Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
        }

        protected void grvServiceJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
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
                List<ServiceJobDetailExtension> coll = (List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"];
                if (coll != null && coll.Count > 0)
                    coll.RemoveAt(RowIndex);

                Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
                BindServiceJobDetail();
            }
        }

        private string item1 = string.Empty;
        private string item2 = string.Empty;
        private int count = 1;
        protected void grvServiceJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int CustomerID = 0;
                int? PricingID = 0;
                short CurrencyID = 0;
                int.TryParse(hCustomerID.Value, out CustomerID);
                short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);
                PricingID = (int?)grvServiceJobDetail.DataKeys[e.Row.RowIndex].Values[1];

                ServiceJobDetailExtension item = e.Row.DataItem as ServiceJobDetailExtension;
                if (item != null)
                {
                    Label lblNo = e.Row.FindControl("lblNo") as Label;
                    if (lblNo != null)
                    {
                        //if (!item.WorkOrderNumber.Equals(item1) || !item.ProductID.Equals(item2))
                        //{ lblNo.Text = count.ToString(); count++; }
                        //else
                        //    lblNo.Text = string.Empty;
                        lblNo.Text = item.No.ToString();
                    }
                    item1 = item.WorkOrderNumber;
                    item2 = item.ProductID;
                }

                DropDownList ddlPricing = e.Row.FindControl("ddlPricing") as DropDownList;
                if (ddlPricing != null)
                {
                    List<TblCustomerQuotationAdditionalService> source = CustomerQuotationManager.SelectQuotationAdditionalForDDL(CustomerID, CurrencyID);
                    ddlPricing.DataSource = source;
                    ddlPricing.DataTextField = "Description";
                    ddlPricing.DataValueField = "ID";
                    ddlPricing.DataBind();

                    if (PricingID != null)
                        ddlPricing.SelectedValue = PricingID.ToString();
                }
            }
        }

       
        protected void grvServiceJobDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grvServiceJobDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RemoverInvalidServices();
            grvServiceJobDetail.EditIndex = -1;
            BindServiceJobDetail();
        }

        public void SaveServiceJobDetailForCoppy(int jobID, int newJobID)
        {
            List<ServiceJobDetailExtension> Coll = JobManager.SelectServiceJobDetailByID(jobID);
            if (Coll != null && Coll.Count > 0)
            {
                foreach (var item in Coll)
                {
                    TblServiceJobDetail obj = new TblServiceJobDetail();
                    obj.JobID = newJobID;
                    obj.WorkOrderNumber = item.WorkOrderNumber;
                    obj.ProductID = item.ProductID;
                    obj.Description = item.Description;
                    obj.WorkOrderValues = item.WorkOrderValues;
                    JobManager.InsertServiceJobDetail(obj);
                }
            }
        }


        public void SaveServiceJobDetail(int jobID)
        {
            List<ServiceJobDetailExtension> Coll = Session[ViewState["PageID"] + "ServiceJobDetail"] as List<ServiceJobDetailExtension>;
            if (Coll != null && Coll.Count > 0)
            {
                List<int> List = JobManager.SelectListServiceJobIdByJobID(jobID);
                foreach (var item in Coll)
                {
                    int ID = item.ServiceJobID;
                    string WorkOrderNumber = item.WorkOrderNumber;
                    string ProductID = item.ProductID;
                    string GLCodde = item.GLCode;
                    string Description = item.Description;
                    decimal WorkOrderValues = item.WorkOrderValues;
                    int PricingID = (int)item.PricingID;

                    if (ID > 0)
                    {
                        TblServiceJobDetail obj = JobManager.SelectServiceJobDetailById(ID);
                        if (obj != null)
                        {
                            if (List != null && List.Count > 0)
                            {
                                if (List.Contains(ID))
                                {
                                    obj.WorkOrderNumber = WorkOrderNumber;
                                    obj.ProductID = ProductID;
                                    obj.GLCode = GLCodde;
                                    obj.Description = Description;
                                    obj.WorkOrderValues = WorkOrderValues;
                                    obj.PricingID = PricingID;
                                    JobManager.UpdateServiceJobDetail(obj);
                                    List.Remove(ID);
                                }
                            }
                        }
                    }
                    else
                    {
                        TblServiceJobDetail obj = new TblServiceJobDetail();
                        obj.JobID = jobID;
                        obj.WorkOrderNumber = WorkOrderNumber;
                        obj.ProductID = ProductID;
                        obj.GLCode = GLCodde;
                        obj.Description = Description;
                        obj.WorkOrderValues = WorkOrderValues;
                        obj.Description = Description;
                        obj.PricingID = PricingID;
                        JobManager.InsertServiceJobDetail(obj);
                    }
                }

                if (List != null && List.Count > 0)
                {
                    foreach (var item in List)
                    {
                        JobManager.DeleteServiceJobDetail(item);
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

            grvOtherCharges.DataSource = coll;
            grvOtherCharges.DataBind();
            Session[ViewState["PageID"] + "OtherCharges"] = coll;
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
            TextBox txtDescription = grvOtherCharges.Rows[e.RowIndex].FindControl("txtDescription") as TextBox;
            DropDownList ddlPricing = grvOtherCharges.Rows[e.RowIndex].FindControl("ddlPricing") as DropDownList;
            ExtraInputMask txtQuantity = (ExtraInputMask)grvOtherCharges.Rows[e.RowIndex].FindControl("txtQuantity");
            ExtraInputMask txtCharge = (ExtraInputMask)grvOtherCharges.Rows[e.RowIndex].FindControl("txtCharge");

            if (ddlPricing != null && ddlPricing.SelectedValue == "0")
            {
                AddErrorPrompt(ddlPricing.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(txtCharge.Text))
            {
                AddErrorPrompt(txtCharge.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (IsValid)
            {
                string description = txtDescription != null ? txtDescription.Text.Trim() : string.Empty;
                decimal charge = 0;
                int PricingID = Convert.ToInt32(ddlPricing.SelectedValue);
                string PricingName = ddlPricing.SelectedItem.Text;
                if (txtCharge != null) decimal.TryParse(txtCharge.Text, out charge);

                int quantity = 0;
                if (txtQuantity != null) int.TryParse(txtQuantity.Text.Trim(), out quantity);

                updateOtherChargesRow(e.RowIndex, ID, description, PricingID, PricingName, charge, quantity, e.RowIndex);
                grvOtherCharges.EditIndex = -1;
                BindOtherCharge();
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        //Cập nhật thông tin orther charges
        private void updateOtherChargesRow(int rowIndex, int OtherChargesID, string description, int PricingID, string PricingName, decimal charge, int quantity, int index)
        {
            List<OtherChargesExtension> coll = Session[ViewState["PageID"] + "OtherCharges"] as List<OtherChargesExtension>;
            if (coll != null && coll.Count > 0)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    if (coll[i].OtherChargesID == OtherChargesID && i == index)
                    {
                        coll[i].GLCode = string.Empty;
                        coll[i].Description = description;
                        coll[i].PricingID = PricingID;
                        coll[i].PricingName = PricingName;
                        coll[i].Charge = charge;
                        coll[i].Quantity = quantity;
                    }
                }
            }
            OtherChargesExtension newItem = coll[0];
            coll.Add(newItem);
            coll.RemoveAt(0);
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

            int CustomerID = 0;
            int.TryParse(hCustomerID.Value, out CustomerID);
            if (CustomerID == 0)
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Please select customer before adding other charge.", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                return;
            }

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

            //Coll.Add(itemNew);
            //grvOtherCharges.EditIndex = Coll.Count - 1;

            Coll.Insert(0, itemNew);
            grvOtherCharges.EditIndex = 0;

            Session[ViewState["PageID"] + "OtherCharges"] = Coll;
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
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int CustomerID = 0;
                    int.TryParse(hCustomerID.Value, out CustomerID);
                    short CurrencyID = 0;
                    short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);
                    int? PricingID = (int?)grvOtherCharges.DataKeys[e.Row.RowIndex].Values[1];

                    DropDownList ddlPricing = e.Row.FindControl("ddlPricing") as DropDownList;
                    if (ddlPricing != null)
                    {
                        ddlPricing.DataSource = CustomerQuotationManager.SelectQuotationOtherChargesForDDL(CustomerID, CurrencyID);
                        ddlPricing.DataValueField = "ID";
                        ddlPricing.DataTextField = "Description";
                        ddlPricing.DataBind();
                        if (PricingID != null)
                            ddlPricing.SelectedValue = PricingID.ToString();
                    }
                }
            }
            catch
            {
            }
        }

        public void SaveOtherCharges(int jobID)
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

        protected void ddlPricing_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlPricing = sender as DropDownList;
            int PricingID = Convert.ToInt32(ddlPricing.SelectedValue);
            TblCustomerQuotationOtherCharge obj = CustomerQuotationManager.SelectOtherChargesByID(PricingID);
            ExtraInputMask txtCharge = grvOtherCharges.Rows[grvOtherCharges.EditIndex].FindControl("txtCharge") as ExtraInputMask;
            TextBox txtDescription = grvOtherCharges.Rows[grvOtherCharges.EditIndex].FindControl("txtDescription") as TextBox;
            if (txtCharge != null)
                txtCharge.Text = obj != null ? obj.Price.ToString("N3") : "0.000";
            if (txtDescription != null)
                txtDescription.Text = ddlPricing.SelectedValue == "0" ? string.Empty : ddlPricing.SelectedItem.Text;

        }
        #endregion

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsNewJob = JobManager.IsNewJob(JobID);
                bool IsLocking = JobManager.IsJobLocking(JobID);

                if (!IsNewJob && !IsLocking)
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
                result.Value = "Service_Job_Delete";
                CurrentConfirmResult = result;
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Do you want to delete seleted rows?", MSGButton.DeleteCancel, MSGIcon.Error);
                OpenMessageBox(msg, result, false, false);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected string ShowNumberFormat(object obj, string Format)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString(Format) : "0"; }
            return strPrice;
        }

        ///Trunglc Add - 27-04-2015

        protected void btnLock_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowLock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }
            UpdateJobLockStatus(true);
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }
            UpdateJobLockStatus(false);
        }

        private void UpdateJobLockStatus(bool IsLock)
        {
            if (this.JobID > 0)
            {
                JobManager.LockOrUnLockJob(JobID, IsLock);

                TblJob objJob = JobManager.SelectByID(JobID);

                bool IsAllowEdit = !IsLock && !Convert.ToBoolean(objJob.IsClosed) ? true : false;

                AllowEditting(IsAllowEdit);

                string KEY_MESSAGE = IsLock ? ResourceText.LOCK_JOB_SAVE_SUCCESSFULLY : ResourceText.UNLOCK_JOB_SAVE_SUCCESSFULLY;

                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(KEY_MESSAGE), MSGButton.OK, MSGIcon.Success);
                OpenMessageBox(msg, null, false, false);

            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_NOT_FOUND), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                ResetDataFields();
            }
        }
    }
}