using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
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
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class Job : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "job_manager";
            }
        }

        protected int JobID
        {
            get
            {
                int _jobID = 0;
                if (Session[ViewState["PageID"] + "ID"] != null)
                    int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out _jobID);
                else if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"].ToString(), out _jobID);
                return _jobID;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                hPiValue.Value = JobManager.PiNumber().ToString();
                ApplyControlText();

                if (Request.QueryString["ID"] != null)
                {
                    BindJobData();
                    ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintJobDetail.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", JobID);
                    ltrPrint.Visible = true;
                    btnCreatePO.Visible = true;
                    base.SaveBaseDataBeforeEdit();
                }
                else
                {
                    ResetDataFields();
                    ltrPrint.Visible = false;
                }
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            //grvServiceJobDetail.Columns[0].HeaderText = "Seq";
            //grvServiceJobDetail.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_NUMBER);
            //grvServiceJobDetail.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.PRODUCTID);
            //grvServiceJobDetail.Columns[3].HeaderText = "GLCode";
            //grvServiceJobDetail.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
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

        [WebMethod]
        public static string GetBrandOwnerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectBrandOwnerByKeyword(Keyword);
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

        private void BindTypeOfOrderDDL()
        {
            List<TypeOfOrder> colls = JobManager.SelectTypeOfOrderForDDL();
            ddlTypeOfOrder.DataSource = colls.Select(x => new { ID = x.ToString(), Name = x.GetDescription() });
            ddlTypeOfOrder.DataTextField = "Name";
            ddlTypeOfOrder.DataValueField = "ID";
            ddlTypeOfOrder.DataBind();
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

        private void BindSupplierForDDL()
        {
            List<TblSupplier> colls = SupplierManager.SelectSupllierForDDL();
            ddlSupplier.DataSource = colls;
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierID";
            ddlSupplier.DataBind();
        }

        private void BindJobInternalExternalDDL()
        {
            List<JobInternalExternal> colls = JobManager.SelectJobInternalExternalForDDL();
            ddlIsInternal.DataSource = colls.Select(x => new { ID = x.ToString(), Name = x.ToString() });
            ddlIsInternal.DataTextField = "Name";
            ddlIsInternal.DataValueField = "ID";
            ddlIsInternal.DataBind();

            ddlInExternal.DataSource = colls.Select(x => new { ID = x.ToString(), Name = x.ToString() });
            ddlInExternal.DataTextField = "Name";
            ddlInExternal.DataValueField = "ID";
            ddlInExternal.DataBind();
        }

        private void BindJobStatusDDL()
        {
            List<JobStatus> colls = JobManager.SelectJobStatusForDDL();
            ddlStatus.DataSource = colls.Select(x => new { ID = x.ToString(), Name = x.GetDescription() });
            ddlStatus.DataTextField = "Name";
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataBind();
        }

        private void BindRevNumberDDL(int JobID)
        {
            DataTable dt = JobManager.SelectRevNumberForDDL(JobID);
            ddlRevNumber.DataSource = dt;
            ddlRevNumber.DataValueField = "ID";
            ddlRevNumber.DataTextField = "Name";
            ddlRevNumber.DataBind();
        }

        private void BindCurrencyDDL()
        {
            TblCurrencyCollection list = new CurrencyManager().SelectAllForDDL();
            ddlCurrency.DataSource = list;
            ddlCurrency.DataValueField = "CurrencyID";
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataBind();
        }

        private void BindProductTypeDDL()
        {
            List<TblReference> list = ReferenceTableManager.SelectProductTypeForDDL();
            ddlMainProductType.DataSource = list;
            ddlMainProductType.DataValueField = "ReferencesID";
            ddlMainProductType.DataTextField = "Name";
            ddlMainProductType.DataBind();
        }

        private void BindBackingDDL()
        {
            List<TblBacking> list = BackingManager.SelectForDDL();
            ddlBacking.DataSource = list;
            ddlBacking.DataValueField = "BackingID";
            ddlBacking.DataTextField = "BackingName";
            ddlBacking.DataBind();
        }

        private void BindSupplygDDL()
        {
            List<TblSupply> list = SupplyManager.SelectForDDL();
            ddlSupply.DataSource = list;
            ddlSupply.DataValueField = "SupplyID";
            ddlSupply.DataTextField = "SupplyName";
            ddlSupply.DataBind();
        }

        private void BindTypeOfCylinderDDL()
        {
            List<TypeOfCylinder> list = JobManager.SelectTypeOfCylinderForDDL();
            ddlTypeOfCylinder.DataSource = list.Select(x => new { Name = x, ID = x }); ;
            ddlTypeOfCylinder.DataValueField = "ID";
            ddlTypeOfCylinder.DataTextField = "Name";
            ddlTypeOfCylinder.DataBind();
        }

        private void BindPrintingDDL()
        {
            List<JobPrinting> list = JobManager.SelectPrintingForDDL();
            ddlPrinting.DataSource = list.Select(x => new { Name = x, ID = x }); ;
            ddlPrinting.DataValueField = "ID";
            ddlPrinting.DataTextField = "Name";
            ddlPrinting.DataBind();
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
            BindTypeOfOrderDDL();
            LoadContactDDL();
            BindSaleRepDDL();
            BindJobCoorRepDDL();
            BindSupplierForDDL();
            BindJobInternalExternalDDL();
            BindJobStatusDDL();
            BindCurrencyDDL();
            BindProductTypeDDL();
            BindBackingDDL();
            BindSupplygDDL();
            BindTypeOfCylinderDDL();
            BindPrintingDDL();
            BindBrandOwnerDDL();
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            LoadContactDDL();
        }

        private void BindJobData()
        {
            try
            {
                if (JobID == 0)
                {
                    ResetDataFields();
                }
                else
                {
                    //Kiểm tra nhân viên còn tồn tại không
                    JobExtension obj = JobManager.SelectByID(JobID);
                    if (obj != null && obj.IsServiceJob == 0)
                    {
                        //Bind JobDetail
                        txtCode.Text = obj.CustomerCode;
                        txtName.Text = obj.CustomerName;
                        hCustomerID.Value = obj.CustomerID.ToString();

                        ddlBrandOwner.SelectedValue = obj.BrandOwner != null ? obj.BrandOwner.ToString() : "0";

                        txtShipToPartyCode.Text = obj.ShipToPartyCode;
                        txtShipToParty.Text = obj.ShipToPartyName;
                        lbShipToPartyAddress.Text = obj.ShipToPartyAddress;
                        hdShipToPartyID.Value = (obj.ShipToParty ?? 0).ToString();

                        txtJobNumber.Text = obj.JobNumber;
                        ddlTypeOfOrder.SelectedValue = obj.TypeOfOrder;

                        hBarcode.Value = obj.JobBarcode;
                        imgBarcode.ImageUrl = obj.JobBarcodeImage;

                        BindRevNumberDDL(obj.JobID);
                        ddlRevNumber.SelectedValue = obj.JobID.ToString();

                        txtJobName.Text = obj.JobName;
                        txtDesign.Text = obj.Design;
                        txtDrawing.Text = obj.DrawingNumber;
                        txtItemCode.Text = obj.ItemCode;
                        if (obj.IsOutsource != null)//Nếu giá trị Outsource != null thì gán giá trị đã lưu
                        {
                            chkIsOutsource.Checked = Convert.ToBoolean((byte)obj.IsOutsource);
                            if (Convert.ToBoolean((byte)obj.IsOutsource))
                            {
                                ddlSupplier.Enabled = true;
                                ddlSupplier.SelectedValue = obj.SupplierID.ToString();
                            }
                            else
                                ddlSupplier.Enabled = false;
                        }
                        else//Ngược lại thì gán giá trị mặc định
                        {
                            chkIsOutsource.Checked = false;
                            ddlSupplier.Enabled = false;
                        }
                        ddlCurrency.SelectedValue = obj.CurrencyID.ToString();
                        ddlMainProductType.SelectedValue = obj.ProductTypeID.ToString();

                        //Nếu RootJobID != null (job được sinh tự động từ hệ thống) => Lấy theo RootJobID
                        //Ngược lại (Root Job) được người dùng nhập vào => Lấy theo RootJobNo
                        txtRootJobNumber.Text = obj.RootJobNo;//obj.RootJobID != 0 ? obj.RootJobNumber : obj.RootJobNo;
                        txtRootJobRevNumber.Text = obj.RootJobRevNumber;
                        txtCommonJobNumber.Text = obj.CommonJobNumber;
                        txtCreatedDate.Text = obj.CreatedOn != null ? ((DateTime)obj.CreatedOn).ToString("dd/MM/yyyy") : "";
                        txtCreatedBy.Text = obj.CreatedBy;//StaffManager.GetStaffFullNameByUserName(obj.CreatedBy);
                        txtCustomerPO1.Text = obj.CustomerPO1;
                        txtCustomerPO2.Text = obj.CustomerPO2;

                        //Tải lại danh sách liên hệ của khách hàng
                        LoadContactDDL();
                        ddlContacts.SelectedValue = obj.ContactPersonID.ToString();
                        ddlSaleRep.SelectedValue = obj.SalesRepID.ToString();
                        ddlJobCoordinator.SelectedValue = obj.CoordinatorID != null ? obj.CoordinatorID.ToString() : "0";

                        ddlInExternal.SelectedValue = obj.InternalExternal;
                        txtViewRevisionDetail.Text = obj.RevisionDetail;

                        ddlStatus.SelectedValue = obj.Status;

                        //txtShipToParty.Text = obj.ShipToParty;
                        txtJobRemark.Text = obj.Remark;
                        BindRevisionHistory(obj.JobID);

                        //Bind Jobsheet
                        BindJobSheetData(obj.JobID);

                        //Bind Job Cylinder
                        BindCylinderData(obj.JobID);

                        ////Bind Service Job
                        //LoadServiceJobDetail(JobID);

                        //Bind Other Charges
                        LoadOtherCharges(JobID);

                        //Show button
                        btnSaveRevision.Visible = true;
                        btnGetCopy.Visible = true;
                        btnEngraving.Visible = true;
                        //Disable Customer
                        txtName.Enabled = false;
                        //txtCode.Enabled = false;

                        //if (Convert.ToBoolean(obj.IsClosed))
                        //{
                        //    AllowEditting(false);
                        //}
                        //else
                        //{
                        /**
                         * Trunglc Add
                         * 22-04-2015
                         * Edit
                         * - 23-04-2015
                         */
                        bool IsLocking = JobManager.IsJobLocking(obj.JobID);
                        bool IsNewJob = JobManager.IsNewJob(obj.JobID);
                        bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
                        bool IsAllowEdit = IsNewJob ? true : (!IsLocking ? true : (IsLocking ? false : true));

                        AllowEditting(IsAllowEdit);

                        //}

                        Session[ViewState["PageID"] + "ID"] = obj.JobID;

                        DisableSelectingCustomer();
                        //DisableSelectingProductType();
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_NOT_FOUND), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        ResetDataFields();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void BindRevisionHistory(int JobID)
        {
            grvRevisionHistory.DataSource = JobManager.SelectRevionHistoryByID(JobID);
            grvRevisionHistory.DataBind();

            grvRevisionDetail.DataSource = JobManager.SelectRevionHistoryByID(JobID);
            grvRevisionDetail.DataBind();
        }

        private void ResetDataFields()
        {
            //JobDetail
            txtCode.Text = "";
            txtName.Text = "";
            hCustomerID.Value = "";

            //BrandOwner
            ddlBrandOwner.SelectedIndex = 0;

            //ShipToParty
            txtShipToParty.Text = "";
            txtShipToPartyCode.Text = "";
            lbShipToPartyAddress.Text = "";
            hdShipToPartyID.Value = "";
            //Job barcode và Job number phải giống nhau
            string JobNumber = JobManager.CreateJobNumber();
            txtJobNumber.Text = JobNumber;
            ddlTypeOfOrder.SelectedIndex = 0;

            string barcode = string.Format("{0}-0", JobNumber.Replace("/", "-"));
            hBarcode.Value = barcode;
            imgBarcode.ImageUrl = Common.Code128Rendering.MakeBarcode64BaseImage(barcode, 1.5, false, true);

            ddlCurrency.SelectedIndex = 0;
            ddlMainProductType.SelectedIndex = 0;

            BindRevNumberDDL(0);
            ddlRevNumber.SelectedIndex = 0;
            txtJobName.Text = "";
            txtDesign.Text = "";
            txtDrawing.Text = "";
            txtRootJobNumber.Text = "";
            txtRootJobRevNumber.Text = "";
            txtCommonJobNumber.Text = "";
            txtCreatedBy.Text = ApplicationContext.Current.UserName;//StaffManager.GetStaffFullNameByUserName(ApplicationContext.Current.UserName);
            txtCreatedDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlContacts.SelectedIndex = 0;
            ddlSaleRep.SelectedIndex = 0;
            ddlJobCoordinator.SelectedIndex = 0;
            txtCustomerPO1.Text = "";
            txtCustomerPO2.Text = "";
            txtItemCode.Text = "";
            chkIsOutsource.Checked = false;
            ddlSupplier.SelectedIndex = 0;
            ddlSupplier.Enabled = false;

            ddlStatus.SelectedIndex = 0;

            txtShipToParty.Text = "";
            txtJobRemark.Text = "";

            //JobSheet
            BindCylinderData(0);
            BindJobSheetData(0);

            //Hide button
            btnSaveRevision.Visible = false;
            btnGetCopy.Visible = false;
            btnEngraving.Visible = false;
            //Disable Customer
            txtName.Enabled = true;
            //txtCode.Enabled = true;

            Session[ViewState["PageID"] + "ID"] = null;
        }

        private void AllowEditting(bool yesno)
        {
            //Job detail
            ddlBrandOwner.Enabled = yesno;
            ddlTypeOfOrder.Enabled = yesno;
            txtJobName.Enabled = yesno;
            txtDesign.Enabled = yesno;
            txtDrawing.Enabled = yesno;
            txtRootJobNumber.Enabled = yesno;
            txtRootJobRevNumber.Enabled = yesno;
            txtCustomerPO1.Enabled = yesno;
            txtCustomerPO2.Enabled = yesno;
            txtItemCode.Enabled = yesno;
            chkIsOutsource.Enabled = yesno;
            ddlSupplier.Enabled = yesno;
            txtCommonJobNumber.Enabled = yesno;
            ddlContacts.Enabled = yesno;
            ddlSaleRep.Enabled = yesno;
            ddlJobCoordinator.Enabled = yesno;
            ddlStatus.Enabled = yesno;
            txtShipToParty.Enabled = yesno;
            txtJobRemark.Enabled = yesno;
            ddlInExternal.Enabled = yesno;
            txtViewRevisionDetail.Enabled = yesno;
            if (ddlCurrency.Enabled)//Nếu có thể edit currency thì set allow edit
                ddlCurrency.Enabled = yesno;
            ddlMainProductType.Enabled = yesno;

            //Job sheet
            txtReproOperator.Enabled = yesno;
            txtCircumference.Enabled = yesno;
            txtFaceWidth.Enabled = yesno;
            txtDiameter.Enabled = yesno;
            txtDiameterDiff.Enabled = yesno;

            //Parameters
            btnAddContact.Visible = yesno;
            btnDeleteContact.Visible = yesno;
            for (int i = grvCylinders.Columns.Count - 2; i < grvCylinders.Columns.Count; i++)
            {
                grvCylinders.Columns[i].Visible = yesno;
            }
            for (int j = 0; j < grvCylinders.Rows.Count; j++)
            {
                CheckBox chk = grvCylinders.Rows[j].FindControl("chkIsPivot") as CheckBox;
                if (chk != null)
                    chk.Enabled = yesno;
            }
            //Repro
            txtReproDate.Enabled = yesno;
            chkHasIrisProof.Enabled = yesno;
            txtIrisProof.Enabled = yesno;
            txtCylinderDate.Enabled = yesno;
            chkPreApproval.Enabled = yesno;
            radLeavingAPEM.Enabled = yesno;
            radExpected.Enabled = yesno;
            txtDeilveryNotes.Enabled = yesno;
            //Eyemark
            chkEyeMark.Enabled = yesno;
            txtEMWidth.Enabled = yesno;
            txtEMHeight.Enabled = yesno;
            txtEMColor.Enabled = yesno;
            ddlBacking.Enabled = yesno;
            btnMoreEyeMark.Visible = yesno;
            btnClearEyeMark.Visible = yesno;
            //Barcode
            chkBarcode.Enabled = yesno;
            txtBarcodeSize.Enabled = yesno;
            txtBarcodeColor.Enabled = yesno;
            ddlSupply.Enabled = yesno;
            txtBWR.Enabled = yesno;
            chkTraps.Enabled = yesno;
            txtSize.Enabled = yesno;
            txtUNSizeH.Enabled = yesno;
            txtUNSizeV.Enabled = yesno;
            chkOpaqueInk.Enabled = yesno;
            txtOpaqueInkRate.Enabled = yesno;
            chkIsEndless.Enabled = yesno;
            radPrintingDirectionD.Enabled = yesno;
            radPrintingDirectionL.Enabled = yesno;
            radPrintingDirectionR.Enabled = yesno;
            radPrintingDirectionU.Enabled = yesno;
            txtColorTarget.Enabled = yesno;
            //Cylinder, Proofing and S + T
            ddlTypeOfCylinder.Enabled = yesno;
            ddlPrinting.Enabled = yesno;
            txtProofingMaterial.Enabled = yesno;
            txtNumberOfRepeatH.Enabled = yesno;
            txtNumberOfRepeatV.Enabled = yesno;
            txtSRRemark.Enabled = yesno;

            //Other charges
            btnAddOtherCharges.Visible = yesno;
            for (int i = grvOtherCharges.Columns.Count - 2; i < grvOtherCharges.Columns.Count; i++)
            {
                grvOtherCharges.Columns[i].Visible = yesno;
            }

            btnSave.Visible = yesno;
            btnDelete.Visible = yesno;
            //btnEngraving.Visible = yesno;
            //btnPrintCylinder.Visible = yesno;
            //btnCancel.Visible = yesno;
            //btnSaveRevDetail.Visible = yesno;
            //btnGetCopy.Visible = yesno;

            ///Trunglc Add - 22-04-2015
            ///Edit - 23-04-2015            

            bool IsAllowLock = RoleManager.AllowLock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
            bool IsAllowUnlock = RoleManager.AllowUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
            bool IsNewJob = JobManager.IsNewJob(JobID);

            bool IsJobLocking = JobManager.IsJobLocking(JobID);

            if (IsNewJob)
            {
                btnLock.Visible = IsAllowLock ? true : false;
                btnUnlock.Visible = false;
            }
            else
            {

                btnLock.Visible = IsNewJob && IsAllowLock ? true : ((!IsJobLocking && IsAllowLock && yesno ? true : false));
                btnUnlock.Visible = (IsJobLocking && IsAllowUnlock && !yesno ? true : false);
            }

            btnDelete.Visible = IsJobLocking ? false : true;

            ///End
        }

        private void SaveJob()
        {
            //-------BEGIN VALIDATION
            //Customer
            int cusID = 0, brandOwner = 0, shipToParty = 0, revNumber = 0, rootJobID = 0;
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
            //ShipToParty
            if (string.IsNullOrEmpty(hdShipToPartyID.Value))//Nếu không tìm thấy mã
            {
                AddErrorPrompt(txtShipToPartyCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                AddErrorPrompt(txtShipToParty.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else//Nếu tìm thấy mã
            {
                if (int.TryParse(hdShipToPartyID.Value.Trim(), out shipToParty))//Nếu mã hợp lệ
                {
                    TblCustomer cObj = CustomerManager.SelectByID(shipToParty);
                    if (cObj == null)//Nếu không tìm thấy thông tin theo mã
                    {
                        AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                        AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                    }
                }
                else//Nếu không hợp lệ
                {
                    AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                    AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
            }

            //JobNumber
            if (string.IsNullOrEmpty(txtJobNumber.Text.Trim()))
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
            //RootJob
            if (!string.IsNullOrEmpty(hRootJobID.Value))
            {
                int.TryParse(hRootJobID.Value.Trim(), out rootJobID);
            }

            //Currency
            if (ddlCurrency.SelectedValue == "0")
            {
                AddErrorPrompt(ddlCurrency.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else
            {
                short.TryParse(ddlCurrency.SelectedValue, out currencyID);
            }
            if (ddlMainProductType.SelectedValue == "0")
            {
                AddErrorPrompt(ddlMainProductType.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //-------END VALIDATION

            if (IsValid)
            {
                string CylNoAppearsMultiTimes = CylsHaveTheSameNumber();
                if (!string.IsNullOrEmpty(CylNoAppearsMultiTimes))
                {
                    string message = string.Format("The cylinder number '{0}' appears multi times in list of cylinders. Please change the cylinder number before saving data.", CylNoAppearsMultiTimes);
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }
                //LƯU KHÁCH HÀNG---------------------------------------------------------------
                TblJob obj = new TblJob();
                if (JobID != 0)//Edit
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }


                    obj = JobManager.SelectByID(JobID);
                    if (obj == null)//Nếu không tồn tại khách hàng thì thông báo lỗi
                    {
                        MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(errMsg, null, false, false);
                        BindJobData();
                        return;
                    }
                    else if (obj.IsServiceJob == 1)
                    {
                        MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(errMsg, null, false, false);
                        BindJobData();
                        return;
                    }

                    //if (Convert.ToBoolean(obj.IsClosed))
                    //{
                    //    MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Can't save data because this job has done an revision.", MSGButton.OK, MSGIcon.Error);
                    //    OpenMessageBox(errMsg, null, false, false);
                    //    BindJobData();
                    //    return;
                    //}

                    //obj.CustomerID = cusID;
                    //obj.JobNumber = txtJobNumber.Text.Trim();
                    //obj.RevNumber = revNumber;

                    obj.JobName = txtJobName.Text.Trim();
                    obj.Design = txtDesign.Text.Trim();

                    //obj.RootJobID = rootJobID;
                    obj.RootJobRevNumber = txtRootJobRevNumber.Text.Trim();
                    obj.CommonJobNumber = txtCommonJobNumber.Text.Trim();
                    obj.IsServiceJob = 0;
                    obj.PaymentTerms = string.Empty;
                    //obj.CreatedOn = DateTime.Today();
                    //obj.CreatedBy = ApplicationContext.Current.UserName;

                    obj.TypeOfOrder = ddlTypeOfOrder.SelectedValue;
                    obj.BrandOwner = brandOwner;
                    obj.DrawingNumber = txtDrawing.Text.Trim();
                    obj.ShipToParty = shipToParty;
                    obj.CurrencyID = currencyID;
                    obj.ProductTypeID = Convert.ToInt32(ddlMainProductType.SelectedValue);
                    obj.ItemCode = txtItemCode.Text.Trim();
                    obj.IsOutsource = Convert.ToByte(chkIsOutsource.Checked);
                    obj.SupplierID = chkIsOutsource.Checked ? Convert.ToInt16(ddlSupplier.SelectedValue) : (byte)0;

                    obj.ContactPersonID = int.Parse(ddlContacts.SelectedValue);
                    obj.SalesRepID = int.Parse(ddlSaleRep.SelectedValue) == 0 ? (int?)null : int.Parse(ddlSaleRep.SelectedValue);
                    obj.CoordinatorID = int.Parse(ddlJobCoordinator.SelectedValue) == 0 ? (int?)null : int.Parse(ddlJobCoordinator.SelectedValue);

                    //Lưu các giá trị RootJobNo, RootJobRev
                    //if (obj.RootJobID == 0)//Chỉ cho lưu khi Job này không được copy từ 1 job khác
                    {
                        obj.RootJobNo = txtRootJobNumber.Text.Trim();
                        obj.RootJobRevNumber = txtRootJobRevNumber.Text.Trim();
                    }
                    obj.CustomerPO1 = txtCustomerPO1.Text.Trim();
                    obj.CustomerPO2 = txtCustomerPO2.Text.Trim();

                    obj.Status = ddlStatus.SelectedValue;

                    //obj.ShipToParty = txtShipToParty.Text.Trim();

                    obj.Remark = txtJobRemark.Text.Trim();

                    obj.InternalExternal = ddlInExternal.SelectedValue;

                    obj.RevisionDetail = txtViewRevisionDetail.Text.Trim();

                    obj = JobManager.Update(obj);
                    if (obj != null)
                    {
                        //SaveServiceJobDetail(obj.JobID);
                        SaveJobSheet(obj.JobID);
                        SaveCylinders(obj.JobID);
                        SaveOtherCharges(obj.JobID);

                        OrderConfirmationManager.ResetTotalPriceForOC(obj.JobID);
                        OrderConfirmationManager.ResetTotalPriceForInvoice(obj.JobID);
                    }
                    BindRevisionHistory(obj.JobID);

                    LoggingActions("Job",
                            LogsAction.Objects.Action.UPDATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Job Number", Data = obj.JobNumber } ,
                                new JsonData() { Title = "Job Rev", Data = obj.RevNumber.ToString() }
                            }));

                    //Lưu vào logging
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());

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
                    obj.CustomerID = cusID;

                    string numberMessage = string.Empty;
                    while (JobManager.JobNumberExists(obj.JobID, txtJobNumber.Text.Trim()))
                    {
                        string newNumber = JobManager.CreateJobNumber();
                        numberMessage = string.Format("<p style='text-align:left;'>- Job Number has changed from <b>{0}</b> to <b>{1}</b></p>", txtJobNumber.Text.Trim(), newNumber);
                        txtJobNumber.Text = newNumber;

                        string barcode = string.Format("{0}-0", newNumber.Replace("/", "-"));
                        hBarcode.Value = barcode;
                        imgBarcode.ImageUrl = Common.Code128Rendering.MakeBarcode64BaseImage(barcode, 1.5, false, true);
                    }
                    obj.JobNumber = txtJobNumber.Text.Trim();

                    obj.RevNumber = revNumber;

                    //string barcodeMessage = string.Empty;
                    //while (JobManager.JobBarcodeExists(obj.JobID, hBarcode.Value))
                    //{
                    //    string newBar = JobManager.CreateJobBarcode();
                    //    barcodeMessage = string.Format("<p style='text-align:left;'>- Barcode Number has changed from <b>{0}</b> to <b>{1}</b></p>", hBarcode.Value, newBar);
                    //    hBarcode.Value = newBar;
                    //    imgBarcode.ImageUrl = Common.Code128Rendering.MakeBarcode64BaseImage(newBar, 1.5, false, true);
                    //}
                    obj.JobBarcode = hBarcode.Value;
                    obj.JobBarcodeImage = imgBarcode.ImageUrl;

                    obj.JobName = txtJobName.Text.Trim();
                    obj.Design = txtDesign.Text.Trim();

                    obj.RootJobID = rootJobID;
                    obj.RootJobRevNumber = txtRootJobRevNumber.Text.Trim();
                    obj.CommonJobNumber = txtCommonJobNumber.Text.Trim();

                    obj.CreatedOn = DateTime.Now;
                    obj.CreatedBy = ApplicationContext.Current.UserName;

                    obj.IsServiceJob = 0;
                    obj.PaymentTerms = string.Empty;

                    obj.TypeOfOrder = ddlTypeOfOrder.SelectedValue;
                    obj.BrandOwner = brandOwner;
                    obj.DrawingNumber = txtDrawing.Text.Trim();
                    obj.ShipToParty = shipToParty;
                    obj.CurrencyID = currencyID;
                    obj.ProductTypeID = Convert.ToInt32(ddlMainProductType.SelectedValue);
                    obj.ItemCode = txtItemCode.Text.Trim();
                    obj.IsOutsource = Convert.ToByte(chkIsOutsource.Checked);
                    obj.SupplierID = chkIsOutsource.Checked ? Convert.ToInt16(ddlSupplier.SelectedValue) : (byte)0;

                    obj.ContactPersonID = int.Parse(ddlContacts.SelectedValue);
                    obj.SalesRepID = int.Parse(ddlSaleRep.SelectedValue) == 0 ? (int?)null : int.Parse(ddlSaleRep.SelectedValue);
                    obj.CoordinatorID = int.Parse(ddlJobCoordinator.SelectedValue) == 0 ? (int?)null : int.Parse(ddlJobCoordinator.SelectedValue);

                    obj.RootJobNo = txtRootJobNumber.Text.Trim();
                    obj.RootJobRevNumber = txtRootJobRevNumber.Text.Trim();
                    txtCustomerPO1.Text = txtCustomerPO1.Text.Trim();
                    txtCustomerPO2.Text = txtCustomerPO2.Text.Trim();

                    obj.Status = ddlStatus.SelectedValue;

                    //obj.ShipToParty = txtShipToParty.Text.Trim();

                    obj.Remark = txtJobRemark.Text.Trim();

                    obj.InternalExternal = ddlInExternal.SelectedValue;

                    obj.RevisionDetail = txtRevisionDetail.Text.Trim();

                    obj.IsClosed = 0;

                    obj = JobManager.Insert(obj);
                    if (obj != null)
                    {
                        //SaveServiceJobDetail(obj.JobID);
                        SaveJobSheet(obj.JobID);
                        SaveCylinders(obj.JobID);
                        SaveOtherCharges(obj.JobID);
                    }
                    BindRevisionHistory(obj.JobID);

                    //Show button
                    btnSaveRevision.Visible = true;
                    btnGetCopy.Visible = true;
                    ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintJobDetail.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", obj.JobID);
                    ltrPrint.Visible = true;
                    btnCreatePO.Visible = true;

                    LoggingActions("Job",
                            LogsAction.Objects.Action.CREATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Job Number", Data = obj.JobNumber } ,
                                new JsonData() { Title = "Job Rev", Data = obj.RevNumber.ToString() }
                            }));

                    //Lưu vào logging
                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());
                    string message = string.Format("<p style='text-align:left; font-size:16px;'><b>Data saved successfully with change(s):</b></p>");
                    bool hasChanged = false;
                    if (!string.IsNullOrEmpty(numberMessage))// || !string.IsNullOrEmpty(barcodeMessage))
                    {
                        message = string.Format("{0}{1}{2}", message, numberMessage, "");
                        hasChanged = true;
                    }
                    Session[ViewState["PageID"] + "ID"] = obj.JobID;
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
            Response.Redirect("~/Pages/JobList.aspx");
        }

        //REVISION
        //Create a Revision
        protected void btnSaveRevision_Click(object sender, EventArgs e)
        {
            //#region Trunglc Add - 06-05-2015

            //// Check exist Invoice has created by Job

            //bool IsExistInvoiceByJobID = JobManager.IsExistInvoiceCreatedByJobID(JobID);

            //if (IsExistInvoiceByJobID)
            //{
            //    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CANNOT_SAVE_REVISION), MSGButton.OK, MSGIcon.Error);
            //    OpenMessageBox(msgRole, null, false, false);
            //    return;
            //}

            //#endregion

            //Gọi message box
            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "Job_Revision_Create_More";
            CurrentConfirmResult = result;
            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Are you sure want to create new revision?", MSGButton.YesNo, MSGIcon.Warning);
            OpenMessageBox(msg, result, false, false);
        }

        protected void btnGetCopy_Click(object sender, EventArgs e)
        {
            //Gọi message box
            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "Job_Revision_Get_Copy";
            CurrentConfirmResult = result;
            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Are you sure want to create a copy from this job?", MSGButton.YesNo, MSGIcon.Warning);
            OpenMessageBox(msg, result, false, false);
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        //Tạo mới revision
                        if (e.Value.ToString().Equals("Job_Revision_Create_More"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowUpdateStatus(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            if (JobID != 0)
                            {
                                //Load old job data
                                JobExtension oldJob = JobManager.SelectByID(JobID);

                                TblJob obj = new TblJob();

                                //Bind data to Revision form
                                txtRevisionCustomerName.Text = oldJob.CustomerName;
                                txtRevisionNumber.Text = oldJob.RevNumber.ToString();
                                txtRevisionJobNumber.Text = oldJob.JobNumber;

                                //Close Old Job
                                //int closedJobID = JobManager.CloseOldJob(JobID);

                                //Create new Job
                                obj.CopyFrom(oldJob);
                                //obj.JobBarcode = JobManager.CreateJobBarcode();

                                obj.RevisionFromJob = oldJob.JobID;//oldJob.RevisionFromJob == null ? oldJob.JobID : oldJob.RevisionFromJob;
                                obj.RevisionRootNumber = oldJob.RevNumber;
                                obj.RevisionDetail = string.Empty;
                                obj.InternalExternal = string.Empty;
                                obj.IsClosed = 0;
                                obj.JobID = 0;

                                obj.RevNumber = JobManager.JobRevisionNumber(oldJob.JobNumber);
                                obj.JobBarcode = string.Format("{0}-{1}", oldJob.JobBarcode.Substring(0, 10), obj.RevNumber);
                                string Image64Base = Common.Code128Rendering.MakeBarcode64BaseImage(obj.JobBarcode, 1.5, false, true);
                                obj.JobBarcodeImage = Image64Base;
                                imgBarcode.ImageUrl = Image64Base;

                                obj.Status = Enum.GetName(typeof(JobStatus), JobStatus.Actived);

                                obj = JobManager.Insert(obj);
                                if (obj != null)
                                {
                                    JobID = obj.JobID;
                                    BindRevNumberDDL(obj.JobID);
                                    ddlRevNumber.SelectedValue = obj.JobID.ToString();

                                    //Create Jobsheet
                                    TblJobSheet oldSObj = JobManager.SelectJobSheetByID((int)obj.RevisionFromJob);
                                    if (oldSObj != null)
                                    {
                                        TblJobSheet sObj = new TblJobSheet();
                                        sObj.CopyFrom(oldSObj); ;
                                        sObj.JobID = obj.JobID;
                                        JobManager.InsertJobSheet(sObj);
                                    }

                                    //Create Cylinders
                                    TblCylinderCollection cylColl = CylinderManager.SelectCollections((int)obj.RevisionFromJob);
                                    foreach (TblCylinder c in cylColl)
                                    {
                                        TblCylinder cObj = new TblCylinder();
                                        cObj.CopyFrom(c);
                                        cObj.CylinderID = 0;
                                        cObj.CylinderBarcode = obj.JobBarcode + (c.Sequence.ToString().Length > 1 ? "" : "0") + c.Sequence.ToString();
                                        cObj.JobID = (int)obj.JobID;
                                        CylinderManager.Insert(cObj);
                                    }
                                    BindCylinderData(obj.JobID);

                                    //Bind revision information
                                    hRevisionRootJob.Value = obj.JobID.ToString();
                                    BindCylinderView(obj.JobID);

                                    //save to notification
                                    RealtimeNotificationManager
                                        .CreateOrDismissNotification(obj.ToJSONString(),
                                        SweetSoft.APEM.Core.Manager.CommandType.Insert.ToString(),
                                        ApplicationContext.Current.User.UserName,
                                        null, null, true,
                                        ResourceTextManager.GetApplicationText(ResourceText.JOB_NOTIFICATION_REVISION),
                                        "Job");
                                    Session[ViewState["PageID"] + "ID"] = obj.JobID;
                                    CloseMessageBox();
                                    string script = string.Format("{0}({1});", "OpenRevision", "");
                                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", script, true);
                                    lbMessage.Text = string.Empty;
                                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());

                                    LoggingActions("Job",
                                                    LogsAction.Objects.Action.CREATE_REVISION,
                                                    LogsAction.Objects.Status.SUCCESS,
                                                    JsonConvert.SerializeObject(new List<JsonData>() { 
                                                        new JsonData() { Title = "Job Number", Data = obj.JobNumber } ,
                                                        new JsonData() { Title = "Job Rev - Old", Data = (obj.RevNumber - 1).ToString() },
                                                        new JsonData() { Title = "Job Rev - New", Data = obj.RevNumber.ToString() }
                                                    }));
                                }
                            }
                        }
                        else if (e.Value.ToString().Equals("Job_Revision_Get_Copy"))
                        {
                            if (JobID != 0)
                            {
                                //Load old job data
                                JobExtension oldJob = JobManager.SelectByID(JobID);
                                TblJob obj = new TblJob();

                                //Create new Job
                                obj.CopyFrom(oldJob);
                                obj.JobID = 0;
                                string JobNumber = JobManager.CreateJobNumber();
                                obj.JobNumber = JobNumber;
                                obj.RootJobID = oldJob.JobID;
                                obj.RootJobNo = oldJob.JobNumber;
                                obj.RootJobRevNumber = oldJob.RevNumber.ToString();//JobManager.JobRootRevNumber(oldJob.JobID).ToString();

                                // Trunglc edit - 20150506

                                obj.CustomerPO1 = null;
                                obj.CustomerPO2 = null;

                                obj.JobBarcode = string.Format("{0}-0", JobNumber.Replace("/", "-"));
                                string Image64Base = Common.Code128Rendering.MakeBarcode64BaseImage(obj.JobBarcode, 1.5, false, true);
                                obj.JobBarcodeImage = Image64Base;
                                imgBarcode.ImageUrl = Image64Base;

                                obj.RevNumber = 0;
                                obj.RevisionFromJob = null;
                                obj.RevisionRootNumber = null;
                                obj.InternalExternal = string.Empty;
                                obj.RevisionDetail = string.Empty;
                                obj.IsClosed = 0;

                                obj.Status = Enum.GetName(typeof(JobStatus), JobStatus.Actived);

                                obj = JobManager.Insert(obj);
                                if (obj != null)
                                {
                                    //Create Jobsheet
                                    TblJobSheet oldSObj = JobManager.SelectJobSheetByID((int)obj.RootJobID);
                                    if (oldSObj != null)
                                    {
                                        TblJobSheet sObj = new TblJobSheet();
                                        sObj.CopyFrom(oldSObj);
                                        sObj.JobID = obj.JobID;
                                        JobManager.InsertJobSheet(sObj);
                                    }

                                    //Create Cylinders
                                    TblCylinderCollection cylColl = CylinderManager.SelectCollections((int)obj.RootJobID);
                                    foreach (TblCylinder c in cylColl)
                                    {
                                        TblCylinder cObj = new TblCylinder();
                                        cObj.CopyFrom(c);
                                        cObj.CylinderID = 0;
                                        cObj.CylinderBarcode = obj.JobBarcode + (c.Sequence.ToString().Length > 1 ? "" : "0") + c.Sequence.ToString();
                                        cObj.JobID = (int)obj.JobID;
                                        CylinderManager.Insert(cObj);
                                    }
                                    BindCylinderData(obj.JobID);

                                    //save to notification
                                    RealtimeNotificationManager
                                        .CreateOrDismissNotification(obj.ToJSONString(),
                                        SweetSoft.APEM.Core.Manager.CommandType.Insert.ToString(),
                                        ApplicationContext.Current.User.UserName,
                                        null, null, true,
                                        ResourceTextManager.GetApplicationText(ResourceText.JOB_NOTIFICATION_COPY),
                                        "Job");
                                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());

                                    LoggingActions("Job",
                                                    LogsAction.Objects.Action.GET_COPY,
                                                    LogsAction.Objects.Status.SUCCESS,
                                                    JsonConvert.SerializeObject(new List<JsonData>() { 
                                                        new JsonData() { Title = "[Old Job] - Number", Data = oldJob.JobNumber } ,
                                                        new JsonData() { Title = "[Old Job] - Rev", Data = oldJob.RevNumber.ToString() },
                                                        new JsonData() { Title = "[New Job] - Number", Data = obj.JobNumber } ,
                                                        new JsonData() { Title = "[New Job] - Rev", Data = obj.RevNumber.ToString() }
                                                    }));

                                    Response.Redirect("~/Pages/Job.aspx?ID=" + obj.JobID.ToString(), false);
                                }
                            }
                        }
                        else if (e.Value.ToString().Equals("Job_Delete"))
                        {
                            if (JobID != 0)
                            {
                                TblJob obj = JobManager.SelectByID(JobID);
                                if (obj != null && obj.IsServiceJob == 0)
                                {
                                    string Reasons = JobManager.IsBeingUsedFor(JobID);
                                    if (!string.IsNullOrEmpty(Reasons))
                                    {
                                        string message = string.Format("<p style='text-align:left; margin-bottom:0;'><strong>Cannot delete this job because:</strong></p>{0}", Reasons);
                                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Error);
                                        OpenMessageBox(msg, null, false, false);
                                        return;
                                    }
                                    JobManager.Delete(JobID);

                                    LoggingActions("Job",
                                                    LogsAction.Objects.Action.DELETE,
                                                    LogsAction.Objects.Status.SUCCESS,
                                                    JsonConvert.SerializeObject(new List<JsonData>() { 
                                                        new JsonData() { Title = "Job Number", Data = obj.JobNumber } ,
                                                        new JsonData() { Title = "Job Rev", Data = obj.RevNumber.ToString() }
                                                    }));

                                    LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                                    Response.Redirect("~/Pages/JobList.aspx", false);
                                }
                            }
                        }
                        else if (e.Value.ToString().Equals("Job_Create_Stealbase_PO"))
                        {
                            CreateStealBasePO();
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
                //LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, ex.StackTrace);
                ProcessException(ex);
            }
        }

        /// <summary>
        /// Lưu revision detail after create a revision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveRevDetail_Click(object sender, EventArgs e)
        {
            int JobID = 0;
            if (int.TryParse(hRevisionRootJob.Value, out JobID))
            {
                JobExtension obj = JobManager.SelectByID(JobID);
                if (obj != null)
                {
                    obj.InternalExternal = ddlIsInternal.SelectedValue;
                    obj.RevisionDetail = txtRevisionDetail.Text.Trim();
                    JobManager.Update(obj);
                    BindRevisionHistory(JobID);
                    lbMessage.Text = "Data saved successfully!";
                }
            }
        }

        protected void ddlRevNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            string JobID = ddlRevNumber.SelectedValue;
            Response.Redirect("~/Pages/Job.aspx?ID=" + JobID);
        }
        #endregion


        #region JobSheet
        //JOB SHEET----------------------------------------------------------------------------------------------------------------------------
        //ADD, EDIT, DELETE DEPARTMENT
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        private void BindCylinderData(int JobID)
        {
            DataTable dt = CylinderManager.SelectAll(JobID);
            for (int i = 0; i < dt.Columns.Count; i++)
                dt.Columns[i].ReadOnly = false;
            Session[ViewState["PageID"] + "tableSource"] = dt;
            BindGrid();
        }

        private void BindCylinderView(int JobID)
        {
            DataTable dtSource = CylinderManager.SelectAll(JobID);
            grvCylinderView.DataSource = dtSource;
            grvCylinderView.DataBind();
        }

        private void BindGrid()
        {
            DataTable dtSource = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            grvCylinders.DataSource = dtSource;
            grvCylinders.DataBind();
        }

        /// <summary>
        /// Thêm dòng mới vào datagridview => Không sử dụng
        /// </summary>
        private void addRow()
        {
            //Get list of cylinder
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            DataRow r = source.NewRow();

            Random rnd = new Random();
            int randID = (int)rnd.Next(-10000, -1);
            int maxSequence = 0;
            double dirameter = 0, diramenterDiff = 0;
            double.TryParse(txtDiameterDiff.Text.Trim(), out diramenterDiff);
            double.TryParse(txtDiameter.Text.Trim(), out dirameter);
            while (source.AsEnumerable().Where(x => x.Field<int>("CylinderID") == randID).Count() > 0)
            {
                randID = (int)rnd.Next(-10000, -1);
            }
            maxSequence = source.Rows.Count > 0 ? source.AsEnumerable().Max(x => x.Field<int>("Sequence")) + 1 : 1;
            dirameter = source.Rows.Count > 0 ? source.AsEnumerable().Max(x => x.Field<double>("Dirameter")) + diramenterDiff : dirameter;
            r["CylinderID"] = randID;
            r["Sequence"] = maxSequence;
            r["SteelBase"] = 1;
            r["CylinderNo"] = string.Empty;
            r["CylinderBarcode"] = hBarcode.Value + (maxSequence.ToString().Length < 2 ? "0" + maxSequence.ToString() : maxSequence.ToString());
            r["CusSteelBaseID"] = string.Empty;
            r["CusCylinderID"] = string.Empty;
            r["Color"] = string.Empty;
            r["CylinderStatusID"] = 0;
            r["CylinderStatusName"] = string.Empty;
            r["Protocol"] = EngravingProtocol.None.ToString();
            r["PricingID"] = (short)0;
            r["PricingName"] = string.Empty;
            r["ProductTypeID"] = 0;
            r["ProcessTypeID"] = 0;
            r["CylType"] = string.Empty;
            r["Quantity"] = 1;
            r["Dirameter"] = dirameter;
            r["Dept"] = string.Empty;
            r["IsPivotCylinder"] = source.Rows.Count > 0 ? 0 : 1;

            source.Rows.InsertAt(r, 0);
            source.DefaultView.Sort = "";
            //Update list
            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int RowIndex, int CylinderID, int Sequence, string CusSteelBaseID, string CusCylinderID, bool SteelBase, string Color, short CylinderStatusID, string CylinderStatusName, string Protocol, int PricingID, string PricingName, string Dept, double Dirameter)
        {
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];

            DataRow r = source.NewRow();
            r = source.AsEnumerable().Where(x => x.Field<int>("CylinderID") == CylinderID).FirstOrDefault();
            if (r != null)
            {
                decimal UnitPrice = 0;
                TblReference processType = new TblReference();
                TblReference productType = new TblReference();
                TblCustomerQuotationDetail obj = CustomerQuotationManager.SelectDetailByID(PricingID);
                if (obj != null)
                {
                    processType = ReferenceTableManager.SelectByID(obj.ProcessTypeID);
                    productType = ReferenceTableManager.SelectByID(obj.ProductTypeID);
                    int CurrencyID = Convert.ToInt16(ddlCurrency.SelectedValue);
                    if (obj.CurrencyID != CurrencyID)
                        UnitPrice = 0;
                    else
                    {
                        UnitPrice = SteelBase ? obj.NewSteelBase : obj.OldSteelBase;
                    }
                }
                else
                {
                    processType = new TblReference();
                    productType = new TblReference();
                }

                r["SteelBase"] = SteelBase;
                r["Sequence"] = Sequence;
                r["CylinderBarcode"] = hBarcode.Value + (Sequence.ToString().Length == 1 ? string.Format("0{0}", Sequence) : Sequence.ToString());
                r["CusCylinderID"] = CusCylinderID;
                r["CusSteelBaseID"] = CusSteelBaseID;
                r["Color"] = Color;
                r["CylinderStatusID"] = CylinderStatusID;
                r["CylinderStatusName"] = CylinderStatusName;
                r["Protocol"] = Protocol;
                r["PricingID"] = PricingID;
                r["PricingName"] = PricingName;
                r["ProcessTypeID"] = processType != null ? processType.ReferencesID : 0;
                r["ProductTypeID"] = productType != null ? productType.ReferencesID : 0;
                r["CylType"] = (obj == null) ? string.Empty : string.Format("{0}-{1}", productType != null ? productType.Code : string.Empty, processType != null ? processType.Code : string.Empty);
                r["Dept"] = Dept;
                r["UnitPrice"] = UnitPrice;
                r["Dirameter"] = Dirameter;
                r.AcceptChanges();
            }

            source.DefaultView.Sort = "Sequence";
            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        //Xóa dữ liệu không hợp lệ
        private void removeInvalidRows()
        {
            //Get list of department
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            List<DataRow> r = source.AsEnumerable().Where(x => x.Field<short>("CylinderStatusID") == 0).ToList();
            foreach (DataRow item in r)
            {
                item.Delete();
            }
            source.AcceptChanges();
            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        //Xóa dữ liệu được chọn
        private void removeSelectedRows(List<int> idList)
        {
            //Get list of department
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];

            var r = source.AsEnumerable().Where(x => idList.Contains(x.Field<int>("CylinderID")));
            foreach (var item in r)
            {
                item.Delete();
            }
            source.AcceptChanges();
            for (int i = 0; i < source.Rows.Count; i++)
            {
                source.Rows[i]["Sequence"] = i + 1;
            }
            source.AcceptChanges();
            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        private void updatePriceToCurrency()
        {
            //Get list of department
            string result = string.Empty;
            int CustomerID = 0; int.TryParse(hCustomerID.Value, out CustomerID);
            short CurrencyID = 0; short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);

            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            if (source != null)
            {
                List<DataRow> rows = source.AsEnumerable().ToList();
                foreach (DataRow r in rows)
                {
                    int PricingID = Convert.ToInt32(r["PricingID"]);
                    TblCustomerQuotationDetail cdObj = CustomerQuotationManager.SelectDetailByID(PricingID);
                    if (cdObj != null)
                    {
                        if (cdObj.CurrencyID == CurrencyID)
                            r["UnitPrice"] = Convert.ToBoolean(r["SteelBase"]) ? cdObj.NewSteelBase : cdObj.OldSteelBase;
                        else
                        {
                            r["UnitPrice"] = 0;
                            result += string.Format(" {0};", r["CylinderNo"].ToString());
                        }
                    }
                    else
                    {
                        r["UnitPrice"] = 0;
                        result += string.Format(" {0};", r["CylinderNo"].ToString());
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    lbCylsError.Text = string.Format("Cannot update price for cylinder(s): {0}", result);
                }
            }
        }

        private void DisableSelectingCustomer()
        {
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            if (source == null)
                source = new DataTable();
            List<ServiceJobDetailExtension> Coll = ((List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"]);
            if (Coll == null)
                Coll = new List<ServiceJobDetailExtension>();
            if (source.Rows.Count > 0 || Coll.Count > 0)
            {
                txtName.Enabled = false;
                ddlCurrency.Enabled = false;
            }
            else
            {
                if (btnSave.Visible)
                {
                    txtName.Enabled = true;
                    ddlCurrency.Enabled = true;
                }
            }
        }

        private void DisableSelectingProductType()
        {
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            if (source == null)
                source = new DataTable();
            if (source.Rows.Count > 0)
            {
                ddlMainProductType.Enabled = false;
            }
            else
            {
                ddlMainProductType.Enabled = true;
            }
        }



        /// <summary>
        /// Kiểm tra cylinder có bị trùng mã số không?
        /// </summary>
        /// <returns></returns>
        private string CylsHaveTheSameNumber()
        {
            string result = string.Empty;
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            if (source != null)
            {
                List<DataRow> rows = source.AsEnumerable().ToList();
                foreach (DataRow r in rows)
                {
                    if (rows.Where(x => x.Field<int>("CylinderID") != r.Field<int>("CylinderID") && x.Field<string>("CylinderNo") == r.Field<string>("CylinderNo")).Count() > 0)
                    {
                        result = r.Field<string>("CylinderNo");
                        break;
                    }
                }
            }
            return result;
        }
        #endregion

        protected void grvCylinders_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grvCylinders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            removeInvalidRows();
            grvCylinders.EditIndex = e.NewEditIndex;
            //int CylinderID = 0;
            //if (int.TryParse(grvCylinders.DataKeys[grvCylinders.EditIndex].Values[0].ToString(), out CylinderID))
            //{
            //    DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            //    if (source != null)
            //    {
            //        int? ProductTypeID = source.AsEnumerable().Where(x => x.Field<int>("CylinderID") == CylinderID).Select(x => x.Field<int>("ProductTypeID")).FirstOrDefault();
            //        ddlMainProductType.SelectedValue = ProductTypeID != null ? ProductTypeID.ToString() : "0";
            //    }
            //}

            BindGrid();

            DropDownList ddlStatus = grvCylinders.Rows[e.NewEditIndex].FindControl("ddlStatus") as DropDownList;
            ddlStatusChanged(ddlStatus);

            grvCylinders.Columns[grvCylinders.Columns.Count - 1].Visible = false;
            btnAddContact.Visible = false;
            btnDeleteContact.Visible = false;
            btnPrintCylinder.Visible = false;
        }

        protected void grvCylinders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvCylinders.DataKeys[e.RowIndex].Values[0]);
            CustomExtraTextbox txtSequence = grvCylinders.Rows[e.RowIndex].FindControl("txtSequence") as CustomExtraTextbox;
            CustomExtraTextbox txtCusCylinderID = grvCylinders.Rows[e.RowIndex].FindControl("txtCusCylinderID") as CustomExtraTextbox;
            CustomExtraTextbox txtCusSteelBaseID = grvCylinders.Rows[e.RowIndex].FindControl("txtCusSteelBaseID") as CustomExtraTextbox;
            DropDownList ddlSteelBase = grvCylinders.Rows[e.RowIndex].FindControl("ddlSteelBase") as DropDownList;
            TextBox txtColor = (TextBox)grvCylinders.Rows[e.RowIndex].FindControl("txtColor");
            DropDownList ddlStatus = grvCylinders.Rows[e.RowIndex].FindControl("ddlStatus") as DropDownList;
            DropDownList ddlProtocol = grvCylinders.Rows[e.RowIndex].FindControl("ddlProtocol") as DropDownList;
            DropDownList ddlPricing = grvCylinders.Rows[e.RowIndex].FindControl("ddlPricing") as DropDownList;
            DropDownList ddlDept = grvCylinders.Rows[e.RowIndex].FindControl("ddlDept") as DropDownList;
            ExtraInputMask txtDirameter = grvCylinders.Rows[e.RowIndex].FindControl("txtDirameter") as ExtraInputMask;

            short CylinderStatusID = 0;
            string CylinderStatusName = string.Empty;

            if (string.IsNullOrEmpty(txtSequence.Text))
            {
                AddErrorPrompt(txtSequence.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //if (string.IsNullOrEmpty(txtColor.Text))
            //{
            //    AddErrorPrompt(txtColor.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //}
            if (ddlStatus.SelectedValue == "0")
            {
                AddErrorPrompt(ddlStatus.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else
            {
                CylinderStatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                CylinderStatusName = ddlStatus.SelectedItem.Text;
            }
            TblCylinderStatus csObj = CylinderStatusManager.SelectCylinderStatusByID(CylinderStatusID);
            if (csObj != null && Convert.ToBoolean(csObj.Invoice))
            {
                if (ddlPricing.SelectedValue == "0")
                {
                    AddErrorPrompt(ddlPricing.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
            }

            if (IsValid)
            {
                int Sequence = int.Parse(txtSequence.Text.Trim());
                string CusSteelBaseID = txtCusSteelBaseID.Text.Trim();
                string CusCylinderID = txtCusCylinderID.Text.Trim();
                bool SteelBase = Convert.ToBoolean(Convert.ToInt32(ddlSteelBase.SelectedValue));
                string Color = txtColor != null ? txtColor.Text.Trim() : string.Empty;
                string Protocol = ddlProtocol.SelectedValue;
                int PricingID = int.Parse(ddlPricing.SelectedValue);
                string PricingName = PricingID != 0 ? ddlPricing.SelectedItem.Text : string.Empty;
                string Dept = ddlDept.SelectedItem.Value.Trim();
                double Dirameter = Convert.ToDouble(txtDirameter.Text);

                updateRow(e.RowIndex, ID, Sequence, CusSteelBaseID, CusCylinderID, SteelBase, Color, CylinderStatusID, CylinderStatusName, Protocol, PricingID, PricingName, Dept, Dirameter);
                grvCylinders.EditIndex = -1;
                BindGrid();

                grvCylinders.Columns[grvCylinders.Columns.Count - 1].Visible = true;
                btnAddContact.Visible = true;
                btnDeleteContact.Visible = true;
                btnPrintCylinder.Visible = true;
                DisableSelectingCustomer();
                //DisableSelectingProductType();
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        private void CancelEditingCylinder()
        {
            grvCylinders.EditIndex = -1;
            removeInvalidRows();
            BindGrid();

            grvCylinders.Columns[grvCylinders.Columns.Count - 1].Visible = true;
            btnAddContact.Visible = true;
            btnDeleteContact.Visible = true;
            btnPrintCylinder.Visible = true;
        }

        protected void grvCylinders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CancelEditingCylinder();
        }


        protected void grvCylinders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        //SteelBaseSource
        private DataTable dtSteelBase()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(byte));
            dt.Columns.Add("Name", typeof(string));

            dt.Rows.Add(1, "New");
            dt.Rows.Add(0, "Old");

            return dt;
        }

        protected void grvCylinders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int CustomerID = 0, ProductTypeID = 0;
                short CurrencyID = 0;
                int.TryParse(hCustomerID.Value, out CustomerID);
                int.TryParse(ddlMainProductType.SelectedValue, out ProductTypeID);
                short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);
                byte? SteelBase = Convert.ToByte(grvCylinders.DataKeys[e.Row.RowIndex].Values[1]);
                short? PricingID = (short?)grvCylinders.DataKeys[e.Row.RowIndex].Values[2];
                string Protocol = grvCylinders.DataKeys[e.Row.RowIndex].Values[3].ToString();
                short? CylinderStatusID = (short?)grvCylinders.DataKeys[e.Row.RowIndex].Values[4];
                string Dept = grvCylinders.DataKeys[e.Row.RowIndex].Values[5].ToString();

                //Status
                DropDownList ddlStatus = e.Row.FindControl("ddlStatus") as DropDownList;
                if (ddlStatus != null)
                {
                    ddlStatus.DataSource = CylinderStatusManager.SelectForDDL();
                    ddlStatus.DataValueField = "CylinderStatusID";
                    ddlStatus.DataTextField = "CylinderStatusName";
                    ddlStatus.DataBind();
                    if (SteelBase != null)
                        ddlStatus.SelectedValue = CylinderStatusID.ToString();
                }

                //SteelBase
                DropDownList ddlSteelBase = e.Row.FindControl("ddlSteelBase") as DropDownList;
                if (ddlSteelBase != null)
                {
                    ddlSteelBase.DataSource = dtSteelBase();
                    ddlSteelBase.DataValueField = "Value";
                    ddlSteelBase.DataTextField = "Name";
                    ddlSteelBase.DataBind();
                    if (SteelBase != null)
                        ddlSteelBase.SelectedValue = SteelBase.ToString();
                }

                //Pricing
                DropDownList ddlPricing = e.Row.FindControl("ddlPricing") as DropDownList;
                if (ddlPricing != null)
                {
                    ddlPricing.DataSource = CustomerQuotationManager.SelectQuotationForDDL(CustomerID, CurrencyID, ProductTypeID);
                    //ddlPricing.DataSource = PricingManager.SelectForDDL();
                    ddlPricing.DataValueField = "ID";
                    ddlPricing.DataTextField = "PricingName";
                    ddlPricing.DataBind();
                    if (PricingID != null)
                        ddlPricing.SelectedValue = PricingID.ToString();
                }

                //Protocol
                DropDownList ddlProtocol = e.Row.FindControl("ddlProtocol") as DropDownList;
                if (ddlProtocol != null)
                {
                    List<EngravingProtocol> colls = JobManager.SelectEngravingProtocolForDDL();
                    ddlProtocol.DataSource = colls.Select(x => new { ID = x.ToString(), Name = x.ToString() });
                    ddlProtocol.DataTextField = "Name";
                    ddlProtocol.DataValueField = "ID";
                    ddlProtocol.DataBind();
                    if (!string.IsNullOrEmpty(Protocol))
                        ddlProtocol.SelectedValue = Protocol;
                }

                //Dept
                DropDownList ddlDept = e.Row.FindControl("ddlDept") as DropDownList;
                if (ddlDept != null)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add(".", "...");
                    data.Add("S", "S");
                    data.Add("R", "R");
                    data.Add("P", "P");
                    data.Add("O", "O");

                    ddlDept.DataSource = data;
                    ddlDept.DataTextField = "Value";
                    ddlDept.DataValueField = "Key";
                    ddlDept.DataBind();
                    if (!string.IsNullOrEmpty(Dept))
                        ddlProtocol.SelectedValue = Dept;
                }
            }
        }

        private bool SaveCylinders(int JobID)
        {
            try
            {
                //Exit Editing Mode
                CancelEditingCylinder();

                //Get old data
                DataTable oldSource = CylinderManager.SelectAll(JobID);
                var oldRows = oldSource.AsEnumerable();
                DataTable dtSource = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                var rows = dtSource.AsEnumerable();
                //Get rows which need to be deleted
                var deletedRows = oldRows.Where(x => !rows.Select(x1 => x1.Field<int>("CylinderID")).Contains(x.Field<int>("CylinderID")));
                foreach (var r in deletedRows)
                {
                    int ID = r.Field<int>("CylinderID");
                    CylinderManager.Delete(ID);
                }
                foreach (var r in rows)
                {
                    double _circumference = 0, _faceWidth = 0;
                    double.TryParse(txtCircumference.Text.Trim(), out _circumference);
                    double.TryParse(txtFaceWidth.Text.Trim(), out _faceWidth);
                    TblCylinder obj = new TblCylinder();
                    obj.CylinderID = r.Field<int>("CylinderID");
                    obj.Sequence = r.Field<int>("Sequence");
                    obj.SteelBase = r.Field<byte>("SteelBase");
                    obj.CylinderNo = r.Field<string>("CylinderNo");
                    obj.CylinderBarcode = r.Field<string>("CylinderBarcode");
                    obj.CusSteelBaseID = r.Field<string>("CusSteelBaseID");
                    obj.CusCylinderID = r.Field<string>("CusCylinderID");
                    obj.Color = r.Field<string>("Color");
                    obj.CylinderStatusID = r.Field<short>("CylinderStatusID");
                    obj.Protocol = r.Field<string>("Protocol");
                    obj.PricingID = r.Field<short>("PricingID");
                    obj.ProcessTypeID = r.Field<int>("ProcessTypeID");
                    obj.ProductTypeID = r.Field<int>("ProductTypeID");
                    obj.UnitPrice = r.Field<decimal>("UnitPrice");
                    obj.Quantity = 1;
                    obj.Circumference = _circumference;
                    obj.FaceWidth = _faceWidth;
                    obj.Dirameter = r.Field<double>("Dirameter");
                    obj.Dept = r.Field<string>("Dept");
                    obj.IsPivotCylinder = r.Field<byte>("IsPivotCylinder");
                    obj.JobID = JobID;
                    if (obj.CylinderID < 0)
                        CylinderManager.Insert(obj);
                    else
                        CylinderManager.Update(obj);
                }
                BindCylinderData(JobID);
                return true;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                return false;
            }
        }


        //Jobsheet ----------------------------------------------------
        private void BindJobSheetData(int JobID)
        {
            TblJobSheet obj = JobManager.SelectJobSheetByID(JobID);
            if (obj != null)
            {
                //General
                txtReproOperator.Text = obj.ReproOperator;
                txtCircumference.Text = obj.Circumference.ToString("N2");
                txtFaceWidth.Text = obj.FaceWidth.ToString("N2");
                txtDiameter.Text = obj.Diameter.ToString("N2");
                txtDiameterDiff.Text = obj.DiameterDiff.ToString("N2");
                //Delivery
                txtReproDate.Text = obj.ReproDate != null ? ((DateTime)obj.ReproDate).ToString("dd/MM/yyyy") : string.Empty;
                chkHasIrisProof.Checked = (bool)obj.HasIrisProof;
                txtIrisProof.Text = (bool)obj.HasIrisProof ? (obj.IrisProof == null ? string.Empty : ((int)obj.IrisProof).ToString("N0")) : string.Empty;
                txtCylinderDate.Text = obj.CylinderDate != null ? ((DateTime)obj.CylinderDate).ToString("dd/MM/yyyy") : string.Empty;
                chkPreApproval.Checked = obj.PreAppoval = chkPreApproval.Checked;
                radLeavingAPEM.Checked = obj.LeavingAPE;
                txtDeilveryNotes.Text = obj.DeilveryNotes;
                //Repro
                chkEyeMark.Checked = (bool)obj.EyeMark;
                txtEMWidth.Text = (bool)obj.EyeMark ? (obj.EMWidth == null ? string.Empty : ((int)obj.EMWidth).ToString("N0")) : string.Empty;
                txtEMHeight.Text = (bool)obj.EyeMark ? (obj.EMHeight == null ? string.Empty : ((int)obj.EMHeight).ToString("N0")) : string.Empty;
                txtEMColor.Text = obj.EMColor;
                ddlBacking.SelectedValue = (bool)obj.EyeMark ? (obj.BackingID == null ? "0" : obj.BackingID.ToString()) : "0";
                imgViewEyeMark.Src = obj.EMPonsition == null ? string.Empty : string.Format("/img/eye-mark/eye-mark-{0}.png", obj.EMPonsition.ToString());
                hEyeMarkPosition.Value = (bool)obj.EyeMark ? (obj.EMPonsition == null ? string.Empty : obj.EMPonsition.ToString()) : string.Empty;
                chkBarcode.Checked = (bool)obj.Barcode;
                txtBarcodeSize.Text = obj.BarcodeSize != null ? ((int)obj.BarcodeSize).ToString("N0") : string.Empty;
                txtBarcodeColor.Text = obj.BarcodeColor;
                txtBarcodeNo.Text = obj.BarcodeNo;
                ddlSupply.SelectedValue = (bool)obj.Barcode ? (obj.SupplyID == null ? "0" : obj.SupplyID.ToString()) : "0";
                txtBWR.Text = (bool)obj.Barcode ? (obj.Bwr == null ? string.Empty : ((double)obj.Bwr).ToString("N2")) : string.Empty;
                chkTraps.Checked = (bool)obj.Traps;
                txtSize.Text = (bool)obj.Barcode ? (obj.Size == null ? string.Empty : ((double)obj.Size).ToString("N3")) : string.Empty;
                txtUNSizeV.Text = obj.UNSizeV == null ? string.Empty : ((double)obj.UNSizeV).ToString("N2");
                txtUNSizeH.Text = obj.UNSizeH == null ? string.Empty : ((double)obj.UNSizeH).ToString("N2");
                chkOpaqueInk.Checked = obj.OpaqueInk;
                txtOpaqueInkRate.Text = (bool)obj.OpaqueInk ? (obj.OpaqueInkRate == null ? string.Empty : ((int)obj.OpaqueInkRate).ToString("N0")) : string.Empty;
                chkIsEndless.Checked = (bool)obj.IsEndless;
                if (obj.PrintingDirection.ToLower() == "u")
                    radPrintingDirectionU.Checked = true;
                else if (obj.PrintingDirection.ToLower() == "d")
                    radPrintingDirectionD.Checked = true;
                else if (obj.PrintingDirection.ToLower() == "l")
                    radPrintingDirectionL.Checked = true;
                else if (obj.PrintingDirection.ToLower() == "r")
                    radPrintingDirectionR.Checked = true;
                txtColorTarget.Text = obj.ColorTarget;
                //Cylinder, Proofing, S+T
                ddlTypeOfCylinder.SelectedValue = obj.TypeOfCylinder;
                ddlPrinting.SelectedValue = obj.Printing;
                txtProofingMaterial.Text = obj.ProofingMaterial;
                txtActualPrintingMaterial.Text = obj.ActualPrintingMaterial;
                txtMaterialWidth.Text = obj.MaterialWidth;
                txtNumberOfRepeatH.Text = obj.NumberOfRepeatH == null ? string.Empty : obj.NumberOfRepeatH.ToString();
                txtNumberOfRepeatV.Text = obj.NumberOfRepeatV == null ? string.Empty : obj.NumberOfRepeatV.ToString();
                txtSRRemark.Text = obj.SRRemark;
            }
            else
            {
                ResetJobSheet();
            }
        }

        private void ResetJobSheet()
        {
            //General
            txtReproOperator.Text = string.Empty;
            txtCircumference.Text = string.Empty;
            txtFaceWidth.Text = string.Empty;
            txtDiameter.Text = string.Empty;
            txtDiameterDiff.Text = "0.02";
            //Delivery
            txtReproDate.Text = string.Empty;
            chkHasIrisProof.Checked = false;
            txtIrisProof.Text = string.Empty;
            txtCylinderDate.Text = string.Empty;
            chkPreApproval.Checked = false;
            radLeavingAPEM.Checked = false;
            radExpected.Checked = false;
            txtDeilveryNotes.Text = string.Empty;
            //Repro
            chkEyeMark.Checked = false;
            txtEMWidth.Text = string.Empty;
            txtEMHeight.Text = string.Empty;
            txtEMColor.Text = string.Empty;
            ddlBacking.SelectedIndex = 0;
            imgViewEyeMark.Src = string.Empty;
            hEyeMarkPosition.Value = string.Empty;
            chkBarcode.Checked = false;
            txtBarcodeColor.Text = string.Empty;
            txtBarcodeNo.Text = string.Empty;//hBarcode.Value;
            ddlSupply.SelectedIndex = 0;
            txtBWR.Text = string.Empty;
            chkTraps.Checked = false;
            txtSize.Text = string.Empty;
            txtUNSizeV.Text = string.Empty;
            txtUNSizeH.Text = string.Empty;
            chkOpaqueInk.Checked = false;
            txtOpaqueInkRate.Text = string.Empty;
            chkIsEndless.Checked = false;
            radPrintingDirectionU.Checked = false;
            radPrintingDirectionD.Checked = false;
            radPrintingDirectionL.Checked = false;
            radPrintingDirectionR.Checked = false;
            txtColorTarget.Text = string.Empty;
            //Cylinder, Proofing, S+T
            ddlTypeOfCylinder.SelectedIndex = 0;
            ddlPrinting.SelectedIndex = 0;
            txtProofingMaterial.Text = string.Empty; ;
            txtNumberOfRepeatH.Text = string.Empty; ;
            txtNumberOfRepeatV.Text = string.Empty; ;
            txtSRRemark.Text = string.Empty; ;
        }

        private string GetPrintingDirection()
        {
            string ret = string.Empty;
            if (radPrintingDirectionU.Checked)
                ret = "U";
            else if (radPrintingDirectionD.Checked)
                ret = "D";
            else if (radPrintingDirectionL.Checked)
                ret = "L";
            else if (radPrintingDirectionR.Checked)
                ret = "R";
            return ret;
        }

        private bool SaveJobSheet(int JobID)
        {
            try
            {
                int? irisProof = (int?)null, emWidth = (int?)null, emHeight = (int?)null, opaqueInkRate = (int?)null, numberOfRepeatH = (int?)null, numberOfRepeatV = (int?)null, barcodeSize = (int?)null;
                short? backingID = (short?)null, supplyID = (short?)null, emPonsition = (short?)null;
                double? size = (double?)null, circumference = (double?)null, faceWidth = (double?)null, diameter = (double?)null, diameterDiff = (double?)null, bwr = (double?)null, unSizeV = (double?)null, unSizeH = (double?)null;
                DateTime? reproDate = (DateTime?)null, cylinderDate = (DateTime?)null;

                int _irisProof = 0, _emWidth = 0, _emHeight = 0, _opaqueInkRate = 0, _numberOfRepeatH = 0, _numberOfRepeatV = 0, _barcodeSize = 0;
                short _emPonsition = 0;
                double _size = 0, _circumference = 0, _faceWidth = 0, _diameter = 0, _diameterDiff = 0, _bwr = 0, _unSizeV = 0, _unSizeH = 0;
                DateTime _reproDate = new DateTime(), _cylinderDate = new DateTime();

                //Jobsheet
                if (double.TryParse(txtCircumference.Text.Trim().Replace(",", ""), out _circumference))
                    circumference = _circumference;
                if (double.TryParse(txtFaceWidth.Text.Trim().Replace(",", ""), out _faceWidth))
                    faceWidth = _faceWidth;
                if (double.TryParse(txtDiameter.Text.Trim().Replace(",", ""), out _diameter))
                    diameter = _diameter;
                if (double.TryParse(txtDiameterDiff.Text.Trim().Replace(",", ""), out _diameterDiff))
                    diameterDiff = _diameterDiff;
                //Delivery
                if (DateTime.TryParseExact(txtReproDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _reproDate))
                    reproDate = _reproDate;
                if (int.TryParse(txtIrisProof.Text.Trim().Replace(",", ""), out _irisProof))
                    irisProof = _irisProof;
                if (DateTime.TryParseExact(txtCylinderDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _cylinderDate))
                    cylinderDate = _cylinderDate;
                //Repro
                if (int.TryParse(txtEMWidth.Text.Trim().Replace(",", ""), out _emWidth))
                    emWidth = _emWidth;
                if (int.TryParse(txtEMHeight.Text.Trim().Replace(",", ""), out _emHeight))
                    emHeight = _emHeight;
                backingID = Convert.ToInt16(ddlBacking.SelectedValue) != 0 ? Convert.ToInt16(ddlBacking.SelectedValue) : (short?)null;
                if (short.TryParse(hEyeMarkPosition.Value, out _emPonsition))
                    emPonsition = _emPonsition;
                supplyID = Convert.ToInt16(ddlSupply.SelectedValue) != 0 ? Convert.ToInt16(ddlSupply.SelectedValue) : (short?)null;
                if (double.TryParse(txtBWR.Text.Trim().Replace(",", ""), out _bwr))
                    bwr = _bwr;
                if (double.TryParse(txtSize.Text.Trim().Replace(",", ""), out _size))
                    size = _size;
                if (double.TryParse(txtUNSizeV.Text.Trim().Replace(",", ""), out _unSizeV))
                    unSizeV = _unSizeV;
                if (double.TryParse(txtUNSizeH.Text.Trim().Replace(",", ""), out _unSizeH))
                    unSizeH = _unSizeH;
                if (int.TryParse(txtOpaqueInkRate.Text.Trim().Replace(",", ""), out _opaqueInkRate))
                    opaqueInkRate = _opaqueInkRate;
                if (int.TryParse(txtNumberOfRepeatH.Text.Trim().Replace(",", ""), out _numberOfRepeatH))
                    numberOfRepeatH = _numberOfRepeatH;
                if (int.TryParse(txtNumberOfRepeatV.Text.Trim().Replace(",", ""), out _numberOfRepeatV))
                    numberOfRepeatV = _numberOfRepeatV;
                if (int.TryParse(txtBarcodeSize.Text.Trim().Replace(",", ""), out _barcodeSize))
                    barcodeSize = _barcodeSize;

                TblJobSheet obj = JobManager.SelectJobSheetByID(JobID);
                if (obj != null)
                {

                    //General
                    obj.ReproOperator = txtReproOperator.Text.Trim();
                    obj.Circumference = _circumference;
                    obj.FaceWidth = _faceWidth;
                    obj.Diameter = _diameter;
                    obj.DiameterDiff = _diameterDiff;
                    //Delivery
                    obj.ReproDate = reproDate;
                    obj.HasIrisProof = chkHasIrisProof.Checked;
                    obj.IrisProof = chkHasIrisProof.Checked ? irisProof : (int?)null;
                    obj.CylinderDate = cylinderDate;
                    obj.PreAppoval = chkPreApproval.Checked;
                    obj.LeavingAPE = radLeavingAPEM.Checked;
                    obj.DeilveryNotes = txtDeilveryNotes.Text.Trim();
                    //Repro
                    obj.EyeMark = chkEyeMark.Checked;
                    obj.EMWidth = chkEyeMark.Checked ? emWidth : (int?)null;
                    obj.EMHeight = chkEyeMark.Checked ? emHeight : (int?)null;
                    obj.EMColor = chkEyeMark.Checked ? txtEMColor.Text.Trim() : string.Empty;
                    obj.BackingID = chkEyeMark.Checked ? backingID : (short?)null;
                    obj.EMPonsition = chkEyeMark.Checked ? emPonsition : default(Nullable<short>);//(short?)null;
                    obj.Barcode = chkBarcode.Checked;
                    obj.BarcodeSize = barcodeSize;
                    obj.BarcodeColor = chkBarcode.Checked ? txtBarcodeColor.Text.Trim() : string.Empty;
                    obj.BarcodeNo = txtBarcodeNo.Text.Trim();
                    obj.SupplyID = supplyID;
                    obj.Bwr = bwr;
                    obj.Traps = chkTraps.Checked;
                    obj.Size = size;
                    obj.UNSizeV = unSizeV;
                    obj.UNSizeH = unSizeH;
                    obj.OpaqueInk = chkOpaqueInk.Checked;
                    obj.OpaqueInkRate = chkOpaqueInk.Checked ? opaqueInkRate : (int?)null;
                    obj.IsEndless = chkIsEndless.Checked;
                    obj.PrintingDirection = GetPrintingDirection();
                    obj.ColorTarget = txtColorTarget.Text.Trim();
                    //Cylinder, Proofing, S+T
                    obj.TypeOfCylinder = ddlTypeOfCylinder.SelectedValue;
                    obj.Printing = ddlPrinting.SelectedValue;
                    obj.ProofingMaterial = txtProofingMaterial.Text.Trim();
                    obj.ActualPrintingMaterial = txtActualPrintingMaterial.Text.Trim();
                    obj.MaterialWidth = txtMaterialWidth.Text.Trim();
                    obj.NumberOfRepeatH = numberOfRepeatH;
                    obj.NumberOfRepeatV = numberOfRepeatV;
                    obj.SRRemark = txtSRRemark.Text.Trim();

                    JobManager.UpdateJobSheet(obj);
                }
                else
                {
                    obj = new TblJobSheet();
                    //General
                    obj.JobID = JobID;
                    obj.ReproOperator = txtReproOperator.Text.Trim();
                    obj.Circumference = _circumference;
                    obj.FaceWidth = _faceWidth;
                    obj.Diameter = _diameter;
                    obj.DiameterDiff = _diameterDiff;
                    //Delivery
                    obj.ReproDate = reproDate;
                    obj.HasIrisProof = chkHasIrisProof.Checked;
                    obj.IrisProof = chkHasIrisProof.Checked ? irisProof : (int?)null;
                    obj.CylinderDate = cylinderDate;
                    obj.PreAppoval = chkPreApproval.Checked;
                    obj.LeavingAPE = radLeavingAPEM.Checked;
                    obj.DeilveryNotes = txtDeilveryNotes.Text.Trim();
                    //Repro
                    obj.EyeMark = chkEyeMark.Checked;
                    obj.EMWidth = emWidth;//chkEyeMark.Checked ? emWidth : (int?)null;
                    obj.EMHeight = emHeight;//chkEyeMark.Checked ? emHeight : (int?)null;
                    obj.EMColor = txtEMColor.Text.Trim();//chkEyeMark.Checked ? txtEMColor.Text.Trim() : string.Empty;
                    obj.BackingID = backingID;//chkEyeMark.Checked ? backingID : (short?)null;
                    obj.EMPonsition = emPonsition;//chkEyeMark.Checked ? emPonsition : (short?)null;
                    obj.Barcode = chkBarcode.Checked;
                    obj.BarcodeSize = barcodeSize;
                    obj.BarcodeColor = chkBarcode.Checked ? txtBarcodeColor.Text.Trim() : string.Empty;
                    obj.BarcodeNo = txtBarcodeNo.Text.Trim();
                    obj.SupplyID = supplyID;
                    obj.Bwr = bwr;
                    obj.Traps = chkTraps.Checked;
                    obj.Size = size;
                    obj.UNSizeV = unSizeV;
                    obj.UNSizeH = unSizeH;
                    obj.OpaqueInk = chkOpaqueInk.Checked;
                    obj.OpaqueInkRate = chkOpaqueInk.Checked ? opaqueInkRate : (int?)null;
                    obj.IsEndless = chkIsEndless.Checked;
                    obj.PrintingDirection = GetPrintingDirection();
                    obj.ColorTarget = txtColorTarget.Text.Trim();
                    //Cylinder, Proofing, S+T
                    obj.TypeOfCylinder = ddlTypeOfCylinder.SelectedValue;
                    obj.Printing = ddlPrinting.SelectedValue;
                    obj.ProofingMaterial = txtProofingMaterial.Text.Trim();
                    obj.ActualPrintingMaterial = txtActualPrintingMaterial.Text.Trim();
                    obj.MaterialWidth = txtMaterialWidth.Text.Trim();
                    obj.NumberOfRepeatH = numberOfRepeatH;
                    obj.NumberOfRepeatV = numberOfRepeatV;
                    obj.SRRemark = txtSRRemark.Text.Trim();

                    JobManager.InsertJobSheet(obj);
                }
                imgViewEyeMark.Src = obj.EMPonsition == null ? string.Empty : string.Format("/img/eye-mark/eye-mark-{0}.png", obj.EMPonsition.ToString());
                return true;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                return false;
            }
        }
        #endregion

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            int ID = 0, CustomerID = 0, ProductTypeID = 0;
            if (Session[ViewState["PageID"] + "ID"] != null)
                int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID);
            int.TryParse(hCustomerID.Value, out CustomerID);
            int.TryParse(ddlMainProductType.SelectedValue, out ProductTypeID);
            if (CustomerID == 0)
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Please select customer before adding cylinder.", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                return;
            }

            if (ProductTypeID == 0)
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Please select product type before adding cylinder.", MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                return;
            }

            removeInvalidRows();
            addRow();
            //grvCylinders.EditIndex = grvCylinders.Rows.Count;
            grvCylinders.EditIndex = 0;
            //BindContractData(ID, true);
            BindGrid();
            grvCylinders.Columns[grvCylinders.Columns.Count - 1].Visible = false;
            btnAddContact.Visible = false;
            btnDeleteContact.Visible = false;
            btnPrintCylinder.Visible = false;
        }

        protected void btnDeleteContact_Click(object sender, EventArgs e)
        {
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            List<int> idList = new List<int>();
            for (int i = 0; i < grvCylinders.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvCylinders.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)//Nếu tồn tại dòng dữ liệu được check thì hiện thông báo xóa
                {
                    int ID = 0;
                    if (int.TryParse(grvCylinders.DataKeys[i].Values[0].ToString(), out ID))
                    {
                        idList.Add(ID);
                    }
                }
            }

            if (idList.Count() > 0)
            {
                removeSelectedRows(idList);
                BindGrid();
                DisableSelectingCustomer();
                //DisableSelectingProductType();
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.SELECT_DATA_TO_DELETE), MSGButton.OK, MSGIcon.Info);
                OpenMessageBox(msg, null, false, false);
            }
        }

        protected void btnResetPivot_Click(object sender, EventArgs e)
        {
            DataTable dtSource = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            var rows = dtSource.AsEnumerable();
            double dirameter = 0, dirameterDiff = 0;
            int pivot = 0;
            double.TryParse(txtDiameter.Text.Trim(), out dirameter);
            double.TryParse(txtDiameterDiff.Text.Trim(), out dirameterDiff);

            for (int i = 0; i < grvCylinders.Rows.Count; i++)
            {
                CheckBox chkIsPivot = grvCylinders.Rows[i].FindControl("chkIsPivot") as CheckBox;
                int CylinderID = Convert.ToInt32(grvCylinders.DataKeys[i].Values[0]);
                var r = rows.Where(x => x.Field<int>("CylinderID") == CylinderID).FirstOrDefault();
                r["IsPivotCylinder"] = chkIsPivot.Checked;
                if (chkIsPivot.Checked)
                {
                    r["Dirameter"] = dirameter;
                    pivot = (int)r["Sequence"];
                }
                r.AcceptChanges();
            }

            var aboveRows = rows.Where(x => x.Field<int>("Sequence") < pivot).OrderByDescending(x => x.Field<int>("Sequence"));
            int step = 0;
            foreach (var r in aboveRows)
            {
                step++;
                r["Dirameter"] = dirameter - (step * dirameterDiff);
                r["IsPivotCylinder"] = false;
                r.AcceptChanges();
            }

            var belowRows = rows.Where(x => x.Field<int>("Sequence") > pivot).OrderBy(x => x.Field<int>("Sequence"));
            step = 0;
            foreach (var r in belowRows)
            {
                step++;
                r["Dirameter"] = dirameter + (step * dirameterDiff);
                r["IsPivotCylinder"] = false;
                r.AcceptChanges();
            }

            Session[ViewState["PageID"] + "tableSource"] = dtSource;
            BindGrid();
        }

        #region Service job detail
        //protected void btnAddDetail_Click(object sender, EventArgs e)
        //{
        //    int CustomerID = 0;
        //    int.TryParse(hCustomerID.Value, out CustomerID);
        //    if (CustomerID == 0)
        //    {
        //        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Please select customer before adding additional service.", MSGButton.OK, MSGIcon.Error);
        //        OpenMessageBox(msg, null, false, false);
        //        return;
        //    }

        //    RemoverInvalidServices();
        //    List<ServiceJobDetailExtension> Coll = ((List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"]);
        //    ServiceJobDetailExtension itemNew = new ServiceJobDetailExtension();
        //    itemNew.ServiceJobID = 0;
        //    itemNew.JobID = 0;
        //    itemNew.WorkOrderNumber = string.Empty;
        //    itemNew.ProductID = string.Empty;
        //    itemNew.GLCode = string.Empty;
        //    itemNew.Description = string.Empty;
        //    itemNew.WorkOrderValues = 0;
        //    itemNew.PricingID = 0;
        //    itemNew.PricingName = string.Empty;

        //    Coll.Add(itemNew);
        //    Session[ViewState["PageID"] + "ServiceJobDetail"] = Coll;
        //    grvServiceJobDetail.EditIndex = grvServiceJobDetail.Rows.Count;
        //    BindServiceJobDetail();
        //}

        //private void BindServiceJobDetail()
        //{
        //    List<ServiceJobDetailExtension> coll = (List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"];

        //    grvServiceJobDetail.DataSource = coll;
        //    grvServiceJobDetail.DataBind();
        //}

        //protected decimal TotalPrice = 0;
        //private void LoadServiceJobDetail(int jobID)
        //{
        //    List<ServiceJobDetailExtension> coll = JobManager.SelectServiceJobDetailByID(jobID);
        //    grvServiceJobDetail.DataSource = coll;

        //    grvServiceJobDetail.DataBind();
        //    Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
        //}

        //protected void grvServiceJobDetail_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    RemoverInvalidServices();
        //    grvServiceJobDetail.EditIndex = e.NewEditIndex;
        //    BindServiceJobDetail();
        //}

        //protected void grvServiceJobDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    int ID = Convert.ToInt32(grvServiceJobDetail.DataKeys[e.RowIndex].Value);
        //    TextBox txtWorkOrderNumber = grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtWorkOrderNumber") as TextBox;
        //    TextBox txtProductID = grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtProductID") as TextBox;
        //    TextBox txtGLCode = grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtGLCode") as TextBox;
        //    TextBox txtDescription = grvServiceJobDetail.Rows[e.RowIndex].FindControl("txtDescription") as TextBox;
        //    DropDownList ddlPricing = grvServiceJobDetail.Rows[e.RowIndex].FindControl("ddlPricing") as DropDownList;

        //    if (txtWorkOrderNumber != null && string.IsNullOrEmpty(txtWorkOrderNumber.Text))
        //    {
        //        AddErrorPrompt(txtWorkOrderNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
        //    }
        //    if (txtProductID != null && string.IsNullOrEmpty(txtProductID.Text))
        //    {
        //        AddErrorPrompt(txtProductID.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
        //    }
        //    if (txtGLCode != null && string.IsNullOrEmpty(txtGLCode.Text))
        //    {
        //        AddErrorPrompt(txtGLCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
        //    }
        //    if (ddlPricing != null && ddlPricing.SelectedValue == "0")
        //    {
        //        AddErrorPrompt(ddlPricing.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
        //    }

        //    if (IsValid)
        //    {
        //        string WorkOrderNumber = txtWorkOrderNumber != null ? txtWorkOrderNumber.Text : string.Empty;
        //        string ProductID = txtProductID != null ? txtProductID.Text : string.Empty;
        //        string GLCode = txtGLCode != null ? txtGLCode.Text : string.Empty;
        //        string Description = txtDescription != null ? txtDescription.Text : string.Empty;
        //        decimal WorkOrderValues = 0;
        //        int PricingID = Convert.ToInt32(ddlPricing.SelectedValue);
        //        string PricingName = ddlPricing.SelectedItem.Text;


        //        updateRow(e.RowIndex, ID, ProductID, GLCode, Description, WorkOrderNumber, WorkOrderValues, PricingID, PricingName);
        //        grvServiceJobDetail.EditIndex = -1;
        //        BindServiceJobDetail();
        //        DisableSelectingCustomer();
        //    }
        //    if (!IsValid)
        //    {
        //        ShowErrorPromptExtension();
        //    }
        //}

        ////Cập nhật dòng dữ liệu
        //private void updateRow(int rowIndex, int ServiceJobID, string ProductID, string glCode, string description, string WorkOrderNumber, decimal WorkOrderValues, int PricingID, string PricingName)
        //{
        //    List<ServiceJobDetailExtension> coll = Session[ViewState["PageID"] + "ServiceJobDetail"] as List<ServiceJobDetailExtension>;
        //    if (coll != null && coll.Count > 0)
        //    {
        //        TblCustomerQuotationAdditionalService obj = CustomerQuotationManager.SelectAdditionalByID(PricingID);
        //        coll[rowIndex].WorkOrderNumber = WorkOrderNumber;
        //        coll[rowIndex].ProductID = ProductID;
        //        coll[rowIndex].GLCode = glCode;
        //        coll[rowIndex].Description = description;
        //        coll[rowIndex].PricingID = PricingID;
        //        coll[rowIndex].PricingName = PricingName;
        //        coll[rowIndex].WorkOrderValues = obj != null ? obj.Price : 0;
        //    }
        //    Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
        //}

        //protected void grvServiceJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "delete")
        //    {
        //        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //        int RowIndex = gvr.RowIndex;
        //        List<ServiceJobDetailExtension> coll = (List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"];
        //        if (coll != null && coll.Count > 0)
        //            coll.RemoveAt(RowIndex);

        //        Session[ViewState["PageID"] + "ServiceJobDetail"] = coll;
        //        BindServiceJobDetail();
        //        DisableSelectingCustomer();
        //    }
        //}

        //private string item1 = string.Empty;
        //private string item2 = string.Empty;
        //private int count = 1;
        //protected void grvServiceJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        int CustomerID = 0;
        //        int? PricingID = 0;
        //        short CurrencyID = 0;
        //        int.TryParse(hCustomerID.Value, out CustomerID);
        //        short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);
        //        PricingID = (int?)grvServiceJobDetail.DataKeys[e.Row.RowIndex].Values[1];

        //        TblServiceJobDetail item = e.Row.DataItem as TblServiceJobDetail;
        //        if (item != null)
        //        {
        //            Label lblNo = e.Row.FindControl("lblNo") as Label;
        //            if (lblNo != null)
        //            {
        //                if (!item.WorkOrderNumber.Equals(item1) || !item.ProductID.Equals(item2))
        //                { lblNo.Text = count.ToString(); count++; }
        //                else
        //                    lblNo.Text = string.Empty;
        //            }
        //            item1 = item.WorkOrderNumber;
        //            item2 = item.ProductID;
        //        }

        //        DropDownList ddlPricing = e.Row.FindControl("ddlPricing") as DropDownList;
        //        if (ddlPricing != null)
        //        {
        //            List<TblCustomerQuotationAdditionalService> source = CustomerQuotationManager.SelectQuotationAdditionalForDDL(CustomerID, CurrencyID);
        //            ddlPricing.DataSource = source;
        //            ddlPricing.DataTextField = "Description";
        //            ddlPricing.DataValueField = "ID";
        //            ddlPricing.DataBind();

        //            if (PricingID != null)
        //                ddlPricing.SelectedValue = PricingID.ToString();
        //        }
        //    }
        //}


        //protected void grvServiceJobDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{

        //}

        //private void RemoverInvalidServices()
        //{
        //    List<ServiceJobDetailExtension> Coll = new List<ServiceJobDetailExtension>();
        //    Coll.AddRange(((List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"]).Where(x => !string.IsNullOrEmpty(x.ProductID) || !string.IsNullOrEmpty(x.WorkOrderNumber)).ToList());
        //    Session[ViewState["PageID"] + "ServiceJobDetail"] = Coll;
        //}

        //private void CancelEditingServices()
        //{            
        //    RemoverInvalidServices();
        //    grvServiceJobDetail.EditIndex = -1;
        //    BindServiceJobDetail();
        //}

        //protected void grvServiceJobDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    CancelEditingServices();
        //}

        //public void SaveServiceJobDetailForCopy(int jobID, int newJobID)
        //{
        //    List<ServiceJobDetailExtension> Coll = JobManager.SelectServiceJobDetailByID(jobID);
        //    if (Coll != null && Coll.Count > 0)
        //    {
        //        foreach (var item in Coll)
        //        {
        //            TblServiceJobDetail obj = new TblServiceJobDetail();
        //            obj.JobID = newJobID;
        //            obj.WorkOrderNumber = item.WorkOrderNumber;
        //            obj.ProductID = item.ProductID;
        //            obj.Description = item.Description;
        //            obj.WorkOrderValues = item.WorkOrderValues;
        //            JobManager.InsertServiceJobDetail(obj);
        //        }
        //    }
        //}


        //public void SaveServiceJobDetail(int jobID)
        //{
        //    CancelEditingServices();
        //    List<ServiceJobDetailExtension> Coll = ((List<ServiceJobDetailExtension>)Session[ViewState["PageID"] + "ServiceJobDetail"]);
        //    if (Coll != null && Coll.Count > 0)
        //    {
        //        List<int> List = JobManager.SelectListServiceJobIdByJobID(jobID);
        //        foreach (var item in Coll)
        //        {
        //            int ID = item.ServiceJobID;
        //            string WorkOrderNumber = item.WorkOrderNumber;
        //            string ProductID = item.ProductID;
        //            string GLCode = item.GLCode;
        //            string Description = item.Description;
        //            decimal WorkOrderValues = item.WorkOrderValues;
        //            int PricingID = (int)item.PricingID;

        //            if (ID > 0)
        //            {
        //                TblServiceJobDetail obj = JobManager.SelectServiceJobDetailById(ID);
        //                if (obj != null)
        //                {
        //                    if (List != null && List.Count > 0)
        //                    {
        //                        if (List.Contains(ID))
        //                        {
        //                            obj.WorkOrderNumber = WorkOrderNumber;
        //                            obj.ProductID = ProductID;
        //                            obj.GLCode = GLCode;
        //                            obj.Description = Description;
        //                            obj.WorkOrderValues = WorkOrderValues;
        //                            obj.PricingID = PricingID;
        //                            JobManager.UpdateServiceJobDetail(obj);
        //                            List.Remove(ID);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                TblServiceJobDetail obj = new TblServiceJobDetail();
        //                obj.JobID = jobID;
        //                obj.WorkOrderNumber = WorkOrderNumber;
        //                obj.ProductID = ProductID;
        //                obj.GLCode = GLCode;
        //                obj.Description = Description;
        //                obj.WorkOrderValues = WorkOrderValues;
        //                obj.Description = Description;
        //                obj.PricingID = PricingID;
        //                JobManager.InsertServiceJobDetail(obj);
        //            }
        //        }

        //        if (List != null && List.Count > 0)
        //        {
        //            foreach (var item in List)
        //            {
        //                JobManager.DeleteServiceJobDetail(item);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        JobManager.DeleteServiceJobDetailByJobId(jobID);
        //    }
        //}

        //protected void grvServiceJobDetail_DataBound(object sender, EventArgs e)
        //{
        //    for (int i = grvServiceJobDetail.Rows.Count - 1; i > 0; i--)
        //    {
        //        GridViewRow row = grvServiceJobDetail.Rows[i];
        //        GridViewRow previousRow = grvServiceJobDetail.Rows[i - 1];

        //        Label lblWorkOrderNumber;
        //        Label lblProductID;

        //        string valueAfter1 = string.Empty;
        //        lblWorkOrderNumber = (Label)row.FindControl("lblWorkOrderNumber");
        //        if (lblWorkOrderNumber != null) valueAfter1 = lblWorkOrderNumber.Text;

        //        string valueBefore1 = string.Empty;
        //        lblWorkOrderNumber = (Label)previousRow.FindControl("lblWorkOrderNumber");
        //        if (lblWorkOrderNumber != null) valueBefore1 = lblWorkOrderNumber.Text;

        //        string valueAfter2 = string.Empty;
        //        lblProductID = (Label)row.FindControl("lblProductID");
        //        if (lblProductID != null) valueAfter2 = lblProductID.Text;

        //        string valueBefore2 = string.Empty;
        //        lblProductID = (Label)previousRow.FindControl("lblProductID");
        //        if (lblProductID != null) valueBefore2 = lblProductID.Text;

        //        if (valueAfter1.Equals(valueBefore1) && valueAfter2.Equals(valueBefore2))
        //        {
        //            if (previousRow.Cells[0].RowSpan == 0)
        //            {
        //                if (row.Cells[0].RowSpan == 0)
        //                {
        //                    previousRow.Cells[0].RowSpan += 2;
        //                }
        //                else
        //                {
        //                    previousRow.Cells[0].RowSpan = row.Cells[0].RowSpan + 1;
        //                }
        //                row.Cells[0].Visible = false;
        //            }

        //            if (previousRow.Cells[1].RowSpan == 0)
        //            {
        //                if (row.Cells[1].RowSpan == 0)
        //                {
        //                    previousRow.Cells[1].RowSpan += 2;
        //                }
        //                else
        //                {
        //                    previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;
        //                }
        //                row.Cells[1].Visible = false;
        //            }

        //            if (previousRow.Cells[2].RowSpan == 0)
        //            {
        //                if (row.Cells[2].RowSpan == 0)
        //                {
        //                    previousRow.Cells[2].RowSpan += 2;
        //                }
        //                else
        //                {
        //                    previousRow.Cells[2].RowSpan = row.Cells[2].RowSpan + 1;
        //                }
        //                row.Cells[2].Visible = false;
        //            }
        //        }
        //    }
        //}

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
            OtherChargesExtension newOC = coll[0];
            coll.Add(newOC);
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
            Coll.Insert(0, itemNew);

            Session[ViewState["PageID"] + "OtherCharges"] = Coll;
            grvOtherCharges.EditIndex = 0;
            //grvOtherCharges.EditIndex = Coll.Count - 1;
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

            //Gọi message box
            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "Job_Delete";
            CurrentConfirmResult = result;
            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Are you sure want to delete this job?", MSGButton.YesNo, MSGIcon.Warning);
            OpenMessageBox(msg, result, false, false);
        }

        protected void btnEngraving_Click(object sender, EventArgs e)
        {
            int ID;
            if (int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID))
            {
                Response.Redirect("~/Pages/JobEngraving.aspx?ID=" + ID.ToString());
            }
        }

        protected void chkIsOutsource_CheckedChanged(object sender, EventArgs e)
        {
            ddlSupplier.Enabled = chkIsOutsource.Checked;
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePriceToCurrency();
        }

        protected void btnCallRevision_Click(object sender, EventArgs e)
        {

        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlStatus = sender as DropDownList;
            ddlStatusChanged(ddlStatus);
        }

        protected void ddlMainProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grvCylinders.EditIndex >= 0)
            {

                DropDownList ddlPricing = grvCylinders.Rows[grvCylinders.EditIndex].FindControl("ddlPricing") as DropDownList;
                if (ddlPricing != null)
                {
                    int CustomerID = 0, ProductTypeID = 0;
                    short CurrencyID = 0;
                    int.TryParse(hCustomerID.Value, out CustomerID);
                    int.TryParse(ddlMainProductType.SelectedValue, out ProductTypeID);
                    short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);

                    ddlPricing.DataSource = CustomerQuotationManager.SelectQuotationForDDL(CustomerID, CurrencyID, ProductTypeID);
                    ddlPricing.DataValueField = "ID";
                    ddlPricing.DataTextField = "PricingName";
                    ddlPricing.DataBind();
                }
            }
        }

        protected string ShowNumberFormat(object obj, string Format)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString(Format) : "0"; }
            return strPrice;
        }

        ///Trunglc Add - 22-04-2015

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

                bool IsAllowEdit = !IsLock ? true : false;

                AllowEditting(IsAllowEdit);
                grvCylinders.Enabled = IsAllowEdit;

                string KEY_MESSAGE = IsLock ? ResourceText.LOCK_JOB_SAVE_SUCCESSFULLY : ResourceText.UNLOCK_JOB_SAVE_SUCCESSFULLY;

                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(KEY_MESSAGE), MSGButton.OK, MSGIcon.Success);
                OpenMessageBox(msg, null, false, false);

                LoggingActions("Job",
                    IsLock ? LogsAction.Objects.Action.LOCK : LogsAction.Objects.Action.UNLOCK,
                                LogsAction.Objects.Status.SUCCESS,
                                JsonConvert.SerializeObject(new List<JsonData>() { 
                                    new JsonData() { Title = "Job Number", Data = objJob.JobNumber } ,
                                    new JsonData() { Title = "Job Rev", Data = objJob.RevNumber.ToString() }
                                }));
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_NOT_FOUND), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                ResetDataFields();
            }
        }

        protected void btnCreatePO_Click(object sender, EventArgs e)
        {
            //Gọi message box
            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "Job_Create_Stealbase_PO";
            CurrentConfirmResult = result;
            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Are you sure want to create Purchase Order for this Job?", MSGButton.YesNo, MSGIcon.Warning);
            OpenMessageBox(msg, result, false, false);
        }

        private void CreateStealBasePO()
        {
            TblOrderConfirmation oc = OrderConfirmationManager.SelectByID(JobID);

            if (oc != null)
            {
                string message = "This job has created order confirm. Cannot create purchase order.";
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                return;
            }

            TblPurchaseOrder purOrder = new TblPurchaseOrder();
            //purOrder.SupplierID = 0;
            purOrder.OrderDate = DateTime.Now;
            purOrder.OrderNumber = PurchaseOrder.CreateOrderbNumber();
            //purOrder.RequiredDeliveryDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            purOrder.CylinderType = ddlTypeOfCylinder.SelectedValue;
            purOrder.Remark = string.Empty;
            purOrder.TotalNumberOfCylinders = grvCylinders.Rows.Count;
            purOrder.IsUrgent = Convert.ToByte(false);
            purOrder.JobID = int.Parse(ddlRevNumber.SelectedValue);
            purOrder.CurrencyID = short.Parse(ddlCurrency.SelectedValue);

            TblPurchaseOrder _purOrder = PurchaseOrderManager.InsertPurchaseOrder(purOrder);

            if (_purOrder != null)
            {
                DataTable dtSource = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                var rows = dtSource.AsEnumerable();
                foreach (var r in rows)
                {
                    double _circumference = 0, _faceWidth = 0;
                    double.TryParse(txtCircumference.Text.Trim(), out _circumference);
                    double.TryParse(txtFaceWidth.Text.Trim(), out _faceWidth);

                    TblPurchaseOrderCylinder pc = new TblPurchaseOrderCylinder();
                    pc.CylinderNo = r.Field<byte>("SteelBase") == 1 ? CylinderManager.CreateCylinderNumber() : string.Empty;//Nếu SteelBase = 1 thì mới sinh number
                    pc.CylinderID = r.Field<int>("CylinderID");
                    pc.Circumference = _circumference;
                    pc.FaceWidth = _faceWidth;
                    pc.JobID = JobID;
                    TblCustomerQuotationDetail cqd = CustomerQuotationManager.SelectDetailByID(r.Field<short>("PricingID"));
                    pc.Unit = cqd != null ? cqd.UnitOfMeasure : string.Empty;
                    pc.PurchaseOrderID = _purOrder.PurchaseOrderID;
                    pc.UnitPrice = r.Field<decimal>("UnitPrice");
                    pc.Quantity = 1;
                    PurchaseOrderManager.InsertPurchase_CylinderOrder(pc);

                    TblCylinder cObj = CylinderManager.SelectByID(r.Field<int>("CylinderID"));
                    if (cObj != null)
                    {
                        if (cObj.SteelBase == 1)
                        {
                            cObj.CylinderNo = pc.CylinderNo;
                            CylinderManager.Update(cObj);
                        }
                    }
                }

                LoggingActions("Purchase Order",
                                LogsAction.Objects.Action.CREATE,
                                LogsAction.Objects.Status.SUCCESS,
                                JsonConvert.SerializeObject(new List<JsonData>() { 
                                    new JsonData() { Title = "Order Number", Data = _purOrder.OrderNumber } ,
                                    new JsonData() { Title = "Job Number", Data = _purOrder.TblJob.JobNumber }
                                }));

                string linkView = "New Purchase Order has created. You can view <a id='btnPOEdit' href='javascript:void(0);' data-id='" + _purOrder.PurchaseOrderID + "'>detail</a>.";
                MessageBox _msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), linkView, MSGButton.OK, MSGIcon.Success);
                OpenMessageBox(_msgRole, null, false, false);
            }
        }

        private void ddlStatusChanged(DropDownList ddlStatus)
        {
            short StatusID = 0;
            if (ddlStatus != null)
                short.TryParse(ddlStatus.SelectedValue, out StatusID);
            TblCylinderStatus csObj = CylinderStatusManager.SelectCylinderStatusByID(StatusID);
            if (csObj != null)
            {
                DropDownList ddlPricing = grvCylinders.Rows[grvCylinders.EditIndex].FindControl("ddlPricing") as DropDownList;
                if (ddlPricing != null)
                {
                    if (Convert.ToBoolean(csObj.Invoice))//Nếu trạng thái cylinder cho lập invoice thì enable pricing
                    {
                        ddlPricing.Enabled = true;
                    }
                    else//Ngược lại, khóa pricing và set về mặc định không có báo giá
                    {
                        ddlPricing.SelectedIndex = 0;
                        ddlPricing.Enabled = false;
                    }
                }
            }
        }
    }
}