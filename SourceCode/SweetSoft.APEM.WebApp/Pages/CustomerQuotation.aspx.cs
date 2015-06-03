using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using SweetSoftCMS.ExtraControls.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class CustomerQuotation : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "customer_quotation_manager";
            }
        }

        private int CustomerID
        {
            get
            {
                int _ID = 0;
                if (Session[ViewState["PageID"] + "ID"] != null)
                    int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out _ID);
                else
                    int.TryParse(Request.QueryString["ID"].ToString(), out _ID);
                return _ID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                if (Request.QueryString["ID"] != null)
                {
                    Session[ViewState["PageID"] + "ID"] = Request.QueryString["ID"];
                    BindCustomerData();
                }
                else
                {
                    ResetDataFields();
                }
            }
            ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/QuotationPrinting.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", CustomerID);
            upnPrinting.Update();
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {

        }

        private void BindContact(int CustomerID)
        {
            List<TblContact> source = new List<TblContact>();
            source = ContactManager.SelectAll(CustomerID);
            ddlContacts.DataSource = source;
            ddlContacts.DataTextField = "ContactName";
            ddlContacts.DataValueField = "ContactID";
            ddlContacts.DataBind();
        }

        private void BindQuotationData(int CustomerID)
        {
            TblCustomerQuotation cObj = CustomerQuotationManager.SelectByID(CustomerID);
            if (cObj != null)
            {
                txtQuotationDate.Text = cObj.DateOfQuotation.ToString("dd/MM/yyyy");
                ddlContacts.SelectedValue = cObj.ContactPersonID.ToString();
                txtContactDesignation.Text = cObj.ContactSignation;
                txtQuotationNote.Text = cObj.QuotationText;                
                BindDetail(CustomerID);
                BindAdditional(CustomerID);
                BindOtherCharges(CustomerID);
            }
            else
            {
                txtQuotationDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                ddlContacts.SelectedIndex = 0;
                txtContactDesignation.Text = string.Empty;
                txtQuotationNote.Text = string.Empty;                
                BindPricingMasterDetail();
                BindPricingMasterAdditional();
                BindPricingMasterOtherCharges();
            }
            BindGrid();
            BindAdditionalGrid();
            BindOtherChargesGrid();
        }

        private void BindCustomerData()
        {
            try
            {
                int ID;
                if (int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID))
                {
                    TblCustomer obj = CustomerManager.SelectByID(ID);
                    if (obj != null)
                    {
                        txtCode.Text = obj.Code;
                        txtName.Text = obj.Name;
                        BindContact(obj.CustomerID);
                        BindQuotationData(obj.CustomerID);
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
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
            //Staff
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtQuotationDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlContacts.SelectedIndex = 0;
            txtContactDesignation.Text = string.Empty;
            txtQuotationNote.Text = string.Empty;

            BindContact(0);
            BindQuotationData(0);

            Session[ViewState["PageID"] + "ID"] = null;
        }

        private void CloseEditMode()
        {
            RemoveInvalidRows();
            RemoveInvalidAdditionalRows();
            RemoveInvalidOtherChargesRows();
            grvPrices.EditIndex = -1;
            grvAdditionalService.EditIndex = -1;
            grvOtherCharges.EditIndex = -1;
            BindGrid();
            BindAdditionalGrid();
            BindOtherChargesGrid();
        }

        //Save Quotation
        private void SaveQuotation()
        {
            try
            {
                //-------BEGIN VALIDATION
                int ID = Session[ViewState["PageID"] + "ID"] == null ? 0 : int.Parse(Session[ViewState["PageID"] + "ID"].ToString());
                ///DateOfQuotation
                DateTime quotationDate = new DateTime();
                if (string.IsNullOrEmpty(txtQuotationDate.Text.Trim()))
                    AddErrorPrompt(txtQuotationDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else if (!DateTime.TryParseExact(txtQuotationDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out quotationDate))
                {
                    AddErrorPrompt(txtQuotationDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
                }
                ///Contact
                if (ddlContacts.SelectedValue == "0")
                    AddErrorPrompt(ddlContacts.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///LastName
                if (string.IsNullOrEmpty(txtContactDesignation.Text.Trim()))
                    AddErrorPrompt(txtContactDesignation.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                //-------END VALIDATION


                if (IsValid)
                {
                    //Kiểm tra khách hàng
                    TblCustomer cObj = CustomerManager.SelectByID(ID);
                    if (cObj == null)
                    {
                        MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                        ResetDataFields();
                        return;
                    }
                    CloseEditMode();
                    //LƯU BẢNG GIÁ---------------------------------------------------------------
                    TblCustomerQuotation obj = new TblCustomerQuotation();
                    if (Session[ViewState["PageID"] + "ID"] != null)
                    {
                        obj = CustomerQuotationManager.SelectByID(ID);
                        if (obj != null)//Edit
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            obj.CustomerID = cObj.CustomerID;
                            obj.DateOfQuotation = quotationDate;
                            obj.ContactPersonID = Convert.ToInt32(ddlContacts.SelectedValue);
                            obj.ContactSignation = txtContactDesignation.Text.Trim();
                            obj.QuotationText = txtQuotationNote.Text.Trim();

                            obj = CustomerQuotationManager.Update(obj);
                            if (obj != null)
                            {
                                if (SaveDetail(obj.CustomerID) && SaveAdditional(obj.CustomerID) && SaveOtherCharges(obj.CustomerID))
                                {
                                    //Load detail
                                    BindDetail(obj.CustomerID);
                                    BindAdditional(obj.CustomerID);
                                    BindOtherCharges(obj.CustomerID);
                                    BindGrid();
                                    BindAdditionalGrid();
                                    BindOtherChargesGrid();

                                    //Lưu vào logging
                                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());

                                    Session[ViewState["PageID"] + "ID"] = obj.CustomerID;
                                    MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                                    OpenMessageBox(errMsg, null, false, false);
                                }
                            }
                        }
                        else//Add
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            obj = new TblCustomerQuotation();
                            obj.CustomerID = cObj.CustomerID;
                            obj.DateOfQuotation = quotationDate;
                            obj.ContactPersonID = Convert.ToInt32(ddlContacts.SelectedValue);
                            obj.ContactSignation = txtContactDesignation.Text.Trim();
                            obj.QuotationText = txtQuotationNote.Text.Trim();

                            obj = CustomerQuotationManager.Insert(obj);
                            if (obj != null)
                            {
                                if (SaveDetail(obj.CustomerID) && SaveAdditional(obj.CustomerID) && SaveOtherCharges(obj.CustomerID))
                                {
                                    //Load detail
                                    BindDetail(obj.CustomerID);
                                    BindAdditional(obj.CustomerID);
                                    BindOtherCharges(obj.CustomerID);
                                    BindGrid();
                                    BindAdditionalGrid();
                                    BindOtherChargesGrid();

                                    //Lưu vào logging
                                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());

                                    Session[ViewState["PageID"] + "ID"] = obj.CustomerID;
                                    MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                                    OpenMessageBox(errMsg, null, false, false);
                                }
                            }

                        }
                    }
                    else//Add new
                    {
                        MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                    }
                }
                if (!IsValid)
                    ShowErrorPromptExtension();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveQuotation();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //BindCustomerData();
            Response.Redirect("~/Pages/CustomerList.aspx");
        }

        #region DETAIL
        //Load detail data from db
        private void BindPricingMasterDetail()
        {
            //Load Customer master template
            int CustomerID = 0;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PricingMasterTemplateSetting);
            if (setting != null)
                CustomerID = Convert.ToInt32(setting.SettingValue);

            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source = CustomerQuotationManager.SelectPricingMasterTemplateDetail(CustomerID);

            Session[ViewState["PageID"] + "dtSource"] = source;
        }

        private void BindDetail(int CustomerID)
        {
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source = CustomerQuotationManager.SelectDetail(CustomerID);

            Session[ViewState["PageID"] + "dtSource"] = source;
        }

        //Bind data to grid detail
        private void BindGrid()
        {
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source = (List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"];
            grvPrices.DataSource = source;
            grvPrices.DataBind();
        }

        private void AddNewRow()
        {
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source = (List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"];
            if(source == null)
                source = new List<CustomerQuotationDetailExtension>();

            CustomerQuotationDetailExtension obj = new CustomerQuotationDetailExtension();
            Random rnd = new Random();
            obj.Id = rnd.Next(-1000, -1);
            while (source.Where(x => x.Id == obj.Id).Count() > 0)
                obj.Id = rnd.Next(-1000, -1);

            obj.ProductTypeID = 0;
            obj.ProcessTypeID = 0;
            obj.OldSteelBase = 0;
            obj.NewSteelBase = 0;
            obj.CurrencyID = 0;
            obj.UnitOfMeasure = UnitOfMeasure.cm2.ToString();
            source.Insert(0, obj);
            Session[ViewState["PageID"] + "dtSource"] = source;
        }

        //Remove invalid row from list
        private void RemoveInvalidRows()
        {
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source.AddRange(((List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"]).Where(x => x.ProductTypeID != 0 && x.ProcessTypeID != 0 && !string.IsNullOrEmpty(x.PricingName)).ToList());
            Session[ViewState["PageID"] + "dtSource"] = source;
        }

        //Remove selected row
        private void RemoveSelectedRow(int ID)
        {
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source.AddRange(((List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"]).Where(x => x.Id != ID).ToList());
            Session[ViewState["PageID"] + "dtSource"] = source;
        }

        //Kiểm tra thông tin giá này đã tồn tại trong danh sách chưa?
        private bool CheckRowExists(int ID, string PricingName)
        {
            bool ret = false;
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source = (List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"];
            if (source == null)
                source = new List<CustomerQuotationDetailExtension>();
            ret = source.Where(x => x.Id != ID && x.PricingName == PricingName).Count() > 0 ? true : false;
            return ret;
        }

        //Update data to list
        private void UpdateRow(int RowIndex, int ID, string PricingName, int ProductTypeID, string ProductTypeName, int ProcessTypeID, string ProcessTypeName, string GLCode, string Description, decimal OldSteelBase, decimal NewSteelBase, short CurrencyID, string CurrencyName, string UnitOfMeasure)
        {
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source = (List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"];
            if (source == null)
                source = new List<CustomerQuotationDetailExtension>();
            CustomerQuotationDetailExtension obj = source.Where(x => x.Id == ID).FirstOrDefault();
            if (obj != null)
            {
                //obj.CustomerId = CustomerID;
                obj.ProcessTypeID = ProcessTypeID;
                obj.ProcessTypeName = ProcessTypeName;
                obj.ProductTypeID = ProductTypeID;
                obj.ProductTypeName = ProductTypeName;
                obj.PricingName = PricingName;
                obj.GLCode = GLCode;
                obj.Description = Description;
                obj.OldSteelBase = OldSteelBase;
                obj.NewSteelBase = NewSteelBase;
                obj.CurrencyID = CurrencyID;
                obj.CurrencyName = CurrencyName;
                obj.UnitOfMeasure = UnitOfMeasure;
            }
            source.Sort((x, y) => x.PricingName.CompareTo(y.PricingName));
            Session[ViewState["PageID"] + "dtSource"] = source;
        }



        //Save Detail
        private bool SaveDetail(int CustomerID)
        {
            //CurrenSource
            List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
            source = (List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"];
            if (source == null)
                source = new List<CustomerQuotationDetailExtension>();
            //OldSource
            List<CustomerQuotationDetailExtension> oldSource = new List<CustomerQuotationDetailExtension>();
            oldSource = CustomerQuotationManager.SelectDetail(CustomerID);
            //DeleteSource
            var deleteSource = oldSource.Where(x => !source.Select(a => a.Id).Contains(x.Id));
            //Delete old source
            foreach (var item in deleteSource)
                CustomerQuotationManager.DeleteDetail(item.Id);
            //Update new source
            foreach (CustomerQuotationDetailExtension item in source)
            {
                TblCustomerQuotationDetail obj = new TblCustomerQuotationDetail();
                obj = (TblCustomerQuotationDetail)item;
                obj.CustomerId = CustomerID;
                if (obj.Id < 0)
                {
                    CustomerQuotationManager.InsertDetail(obj);
                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
                else
                {
                    CustomerQuotationManager.UpdateDetail(obj);
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
            }
            BindDetail(CustomerID);
            BindGrid();
            return true;
        }

        //Thêm dòng mới vào chi tiết
        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            RemoveInvalidRows();
            grvPrices.EditIndex = 0;
            AddNewRow();
            BindGrid();
        }

        //Chỉnh sửa dòng
        protected void grvPrices_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RemoveInvalidRows();
            grvPrices.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        //Bind dữ liệu vào dropdownlist khi chỉnh sửa dòng
        protected void grvPrices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(grvPrices.DataKeys[e.Row.RowIndex].Value);
                int ProductTypeID = 0, ProcessTypeID = 0;
                short CurrencyID = 0;
                string UnitOfMeasure = string.Empty;
                //Lấy danh sách chi tiết
                List<CustomerQuotationDetailExtension> source = new List<CustomerQuotationDetailExtension>();
                source = (List<CustomerQuotationDetailExtension>)Session[ViewState["PageID"] + "dtSource"];
                if (source == null)
                    source = new List<CustomerQuotationDetailExtension>();
                //Tìm object theo ID
                CustomerQuotationDetailExtension obj = source.Where(x => x.Id == ID).FirstOrDefault();
                if (obj != null)
                {
                    ProductTypeID = obj.ProductTypeID;
                    ProcessTypeID = obj.ProcessTypeID;
                    CurrencyID = obj.CurrencyID;
                    UnitOfMeasure = obj.UnitOfMeasure;
                }

                //Bind product type
                DropDownList ddlProductType = e.Row.FindControl("ddlProductType") as DropDownList;
                if (ddlProductType != null)
                {
                    ddlProductType.DataSource = ReferenceTableManager.SelectProductTypeForDDL();
                    ddlProductType.DataValueField = "ReferencesID";
                    ddlProductType.DataTextField = "Code";
                    ddlProductType.DataBind();
                    ddlProductType.SelectedValue = ProductTypeID.ToString();
                }
                //Bind process type
                DropDownList ddlProcessType = e.Row.FindControl("ddlProcessType") as DropDownList;
                if (ddlProcessType != null)
                {
                    ddlProcessType.DataSource = ReferenceTableManager.SelectProcessTypeForDDL(false);
                    ddlProcessType.DataValueField = "ReferencesID";
                    ddlProcessType.DataTextField = "Code";
                    ddlProcessType.DataBind();
                    ddlProcessType.SelectedValue = ProcessTypeID.ToString();
                }
                //Bind currency
                DropDownList ddlCurrency = e.Row.FindControl("ddlCurrency") as DropDownList;
                if (ddlCurrency != null)
                {
                    ddlCurrency.DataSource = new CurrencyManager().SelectAllForDDL();
                    ddlCurrency.DataValueField = "CurrencyID";
                    ddlCurrency.DataTextField = "CurrencyName";
                    ddlCurrency.DataBind();
                    ddlCurrency.SelectedValue = CurrencyID.ToString();
                }
                //Bind unit
                DropDownList ddlUnit = e.Row.FindControl("ddlUnit") as DropDownList;
                if (ddlCurrency != null)
                {
                    List<UnitOfMeasure> colls = CustomerQuotationManager.SelectUnitOfMeasureForDDL();
                    ddlUnit.DataSource = colls.Select(x => new { ID = x.ToString(), Name = x.ToString() });
                    ddlUnit.DataTextField = "Name";
                    ddlUnit.DataValueField = "ID";
                    ddlUnit.DataBind();
                    ddlUnit.SelectedValue = UnitOfMeasure;
                }
            }
            catch
            {
            }
        }

        //Cập nhật dữ liệu mới
        protected void grvPrices_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvPrices.DataKeys[e.RowIndex].Value);

            CustomExtraTextbox txtPricingName = grvPrices.Rows[e.RowIndex].FindControl("txtPricingName") as CustomExtraTextbox;
            DropDownList ddlProductType = grvPrices.Rows[e.RowIndex].FindControl("ddlProductType") as DropDownList;
            DropDownList ddlProcessType = grvPrices.Rows[e.RowIndex].FindControl("ddlProcessType") as DropDownList;
            CustomExtraTextbox txtGLCode = grvPrices.Rows[e.RowIndex].FindControl("txtGLCode") as CustomExtraTextbox;
            CustomExtraTextbox txtDescription = grvPrices.Rows[e.RowIndex].FindControl("txtDescription") as CustomExtraTextbox;
            ExtraInputMask txtOldSteelBase = grvPrices.Rows[e.RowIndex].FindControl("txtOldSteelBase") as ExtraInputMask;
            ExtraInputMask txtNewSteelBase = grvPrices.Rows[e.RowIndex].FindControl("txtNewSteelBase") as ExtraInputMask;
            DropDownList ddlCurrency = grvPrices.Rows[e.RowIndex].FindControl("ddlCurrency") as DropDownList;
            DropDownList ddlUnit = grvPrices.Rows[e.RowIndex].FindControl("ddlUnit") as DropDownList;
            string PricingName = string.Empty, ProductTyleName = string.Empty, ProcessTypeName = string.Empty, GLCode = string.Empty, Description = string.Empty, CurrencyName = string
.Empty, UnitOfMeasure = string.Empty;
            int ProductTyleID = 0, ProcessTypeID = 0;
            decimal OldSteel = 0, NewSteel = 0;
            short CurrencyID = 0;
                
            //PricingName
            if (txtPricingName != null && string.IsNullOrEmpty(txtPricingName.Text.Trim()))
                AddErrorPrompt(txtPricingName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            else if (CheckRowExists(ID, txtPricingName.Text.Trim()))
                AddErrorPrompt(txtPricingName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_EXISTS));
            //ProductType
            if (ddlProductType != null && ddlProductType.SelectedValue == "0")
                AddErrorPrompt(ddlProductType.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //ProcessType
            if (ddlProcessType != null && ddlProcessType.SelectedValue == "0")
                AddErrorPrompt(ddlProcessType.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //Currency
            if (ddlCurrency != null && ddlCurrency.SelectedValue == "0")
                AddErrorPrompt(ddlCurrency.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //OldSteelBase
            if (txtOldSteelBase != null)
            {
                if (string.IsNullOrEmpty(txtOldSteelBase.Text.Trim()))
                    AddErrorPrompt(txtOldSteelBase.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else
                {
                    if(!decimal.TryParse(txtOldSteelBase.Text.Trim().Replace(",", ""), out OldSteel))
                        AddErrorPrompt(txtOldSteelBase.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
                    else if (decimal.TryParse(txtOldSteelBase.Text.Trim().Replace(",", ""), out OldSteel) && OldSteel <0)
                        AddErrorPrompt(txtOldSteelBase.ClientID, ResourceTextManager.GetApplicationText(ResourceText.MIN_VALIDATION));
                }

            }
            //NewSteelBase
            if (txtNewSteelBase != null)
            {
                if (string.IsNullOrEmpty(txtNewSteelBase.Text.Trim()))
                    AddErrorPrompt(txtNewSteelBase.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else
                {
                    if (!decimal.TryParse(txtNewSteelBase.Text.Trim().Replace(",", ""), out NewSteel))
                        AddErrorPrompt(txtNewSteelBase.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
                    else if (decimal.TryParse(txtNewSteelBase.Text.Trim().Replace(",", ""), out NewSteel) && NewSteel < 0)
                        AddErrorPrompt(txtNewSteelBase.ClientID, ResourceTextManager.GetApplicationText(ResourceText.MIN_VALIDATION));
                }

            }
            if (txtPricingName != null && string.IsNullOrEmpty(txtPricingName.Text.Trim()))
                AddErrorPrompt(txtPricingName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));

            if (IsValid)
            {
                //PricingName
                PricingName = txtPricingName.Text.Trim();
                //ProductType
                ProductTyleID = Convert.ToInt32(ddlProductType.SelectedValue);
                ProductTyleName = ddlProductType.SelectedItem.Text;
                //ProcessType
                ProcessTypeID = Convert.ToInt32(ddlProcessType.SelectedValue);
                ProcessTypeName = ddlProcessType.SelectedItem.Text;
                //GLCode
                GLCode = txtGLCode.Text.Trim();
                //Description
                Description = txtDescription.Text.Trim();
                //Currency
                CurrencyID = Convert.ToInt16(ddlCurrency.SelectedValue);
                CurrencyName = ddlCurrency.SelectedItem.Text;
                //Unit
                UnitOfMeasure = ddlUnit.SelectedValue;

                //if (CheckRowExists(ID, PricingName))
                //{
                //    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Duplicate information error.", MSGButton.OK, MSGIcon.Error);
                //    OpenMessageBox(msg, null, false, false);
                //    return;
                //}

                UpdateRow(e.RowIndex, ID, PricingName, ProductTyleID, ProductTyleName, ProcessTypeID, ProcessTypeName, GLCode, Description, OldSteel, NewSteel, CurrencyID, CurrencyName, UnitOfMeasure);
                grvPrices.EditIndex = -1;
                BindGrid();
            }
            else
                ShowErrorPromptExtension();
        }

        //Thoát chỉnh sửa
        protected void grvPrices_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RemoveInvalidRows();
            grvPrices.EditIndex = -1;
            BindGrid();
        }

        protected void grvPrices_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = Convert.ToInt32(grvPrices.DataKeys[e.RowIndex].Value);
            RemoveSelectedRow(ID);
            BindGrid();
        }

        protected void grvPrices_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        #endregion

        #region ADDITIONALSERVICES
        //Load detail data from db
        private void BindPricingMasterAdditional()
        {
            //Load Customer master template
            int CustomerID = 0;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PricingMasterTemplateSetting);
            if (setting != null)
                CustomerID = Convert.ToInt32(setting.SettingValue);

            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source = CustomerQuotationManager.SelectPricingMasterTemplateAdditional(CustomerID);

            Session[ViewState["PageID"] + "dtSourceAdditional"] = source;
        }

        private void BindAdditional(int CustomerID)
        {
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source = CustomerQuotationManager.SelectAdditional(CustomerID);

            Session[ViewState["PageID"] + "dtSourceAdditional"] = source;
        }

        //Bind data to grid detail
        private void BindAdditionalGrid()
        {
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source = (List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"];
            grvAdditionalService.DataSource = source;
            grvAdditionalService.DataBind();
        }

        private void AddNewAdditionalRow()
        {
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source = (List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"];
            if (source == null)
                source = new List<CustomerQuotationAdditionalServiceExtention>();

            CustomerQuotationAdditionalServiceExtention obj = new CustomerQuotationAdditionalServiceExtention();
            Random rnd = new Random();
            obj.Id = rnd.Next(-1000, -1);
            while (source.Where(x => x.Id == obj.Id).Count() > 0)
                obj.Id = rnd.Next(-1000, -1);

            obj.Description = string.Empty;
            obj.GLCode = string.Empty;
            obj.Price = 0;
            obj.CurrencyID = 0;
            source.Insert(0, obj);
            Session[ViewState["PageID"] + "dtSourceAdditional"] = source;
        }

        //Remove invalid row from list
        private void RemoveInvalidAdditionalRows()
        {
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source.AddRange(((List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"]).Where(x => !string.IsNullOrEmpty(x.Description) && !string.IsNullOrEmpty(x.GLCode)).ToList());
            Session[ViewState["PageID"] + "dtSourceAdditional"] = source;
        }

        //Remove selected row
        private void RemoveSelectedAdditionalRow(int ID)
        {
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source.AddRange(((List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"]).Where(x => x.Id != ID).ToList());
            Session[ViewState["PageID"] + "dtSourceAdditional"] = source;
        }

        //Kiểm tra thông tin giá này đã tồn tại trong danh sách chưa?
        private bool CheckAdditionalRowExists(int ID, string Description)
        {
            bool ret = false;
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source = (List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"];
            if (source == null)
                source = new List<CustomerQuotationAdditionalServiceExtention>();
            ret = source.Where(x => x.Id != ID && x.Description == Description).Count() > 0 ? true : false;
            return ret;
        }

        //Update data to list
        private void UpdateAdditionalRow(int RowIndex, int ID, string Description, string Category, string CategoryName, string GLCode, decimal Price, short CurrencyID, string CurrencyName)
        {
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source = (List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"];
            if (source == null)
                source = new List<CustomerQuotationAdditionalServiceExtention>();
            CustomerQuotationAdditionalServiceExtention obj = source.Where(x => x.Id == ID).FirstOrDefault();
            if (obj != null)
            {
                //obj.CustomerId = CustomerID;
                obj.Description = Description;
                obj.Category = Category;
                obj.CategoryName = CategoryName;
                obj.GLCode = GLCode;
                obj.Price = Price;
                obj.CurrencyID = CurrencyID;
                obj.CurrencyName = CurrencyName;
            }
            source = source.OrderBy(o => o.Description).ToList<CustomerQuotationAdditionalServiceExtention>();
            Session[ViewState["PageID"] + "dtSourceAdditional"] = source;
        }



        //Save Detail
        private bool SaveAdditional(int CustomerID)
        {
            //CurrenSource
            List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
            source = (List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"];
            if (source == null)
                source = new List<CustomerQuotationAdditionalServiceExtention>();
            //OldSource
            List<CustomerQuotationAdditionalServiceExtention> oldSource = new List<CustomerQuotationAdditionalServiceExtention>();
            oldSource = CustomerQuotationManager.SelectAdditional(CustomerID);
            //DeleteSource
            var deleteSource = oldSource.Where(x => !source.Select(a => a.Id).Contains(x.Id));
            //Delete old source
            foreach (var item in deleteSource)
                CustomerQuotationManager.DeleteAdditional(item.Id);
            //Update new source
            foreach (CustomerQuotationAdditionalServiceExtention item in source)
            {
                TblCustomerQuotationAdditionalService obj = new TblCustomerQuotationAdditionalService();
                obj = (TblCustomerQuotationAdditionalService)item;
                obj.CustomerID = CustomerID;
                if (obj.Id < 0)
                {
                    CustomerQuotationManager.InsertAdditional(obj);
                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
                else
                {
                    CustomerQuotationManager.UpdateAdditional(obj);
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
            }
            BindAdditional(CustomerID);
            BindAdditionalGrid();
            return true;
        }

        protected void tblAddService_Click(object sender, EventArgs e)
        {
            RemoveInvalidAdditionalRows();
            grvAdditionalService.EditIndex = 0;
            AddNewAdditionalRow();
            BindAdditionalGrid();
        }

        protected void grvAdditionalService_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = Convert.ToInt32(grvAdditionalService.DataKeys[e.RowIndex].Value);
            RemoveSelectedAdditionalRow(ID);
            BindAdditionalGrid();
        }

        protected void grvAdditionalService_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RemoveInvalidAdditionalRows();
            grvAdditionalService.EditIndex = e.NewEditIndex;
            BindAdditionalGrid();
        }

        protected void grvAdditionalService_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvAdditionalService.DataKeys[e.RowIndex].Value);

            CustomExtraTextbox txtDescription = grvAdditionalService.Rows[e.RowIndex].FindControl("txtDescription") as CustomExtraTextbox;
            CustomExtraTextbox txtGLCode = grvAdditionalService.Rows[e.RowIndex].FindControl("txtGLCode") as CustomExtraTextbox;
            ExtraInputMask txtPrice = grvAdditionalService.Rows[e.RowIndex].FindControl("txtPrice") as ExtraInputMask;
            DropDownList ddlCurrency = grvAdditionalService.Rows[e.RowIndex].FindControl("ddlCurrency") as DropDownList;
            DropDownList ddlCategory = grvAdditionalService.Rows[e.RowIndex].FindControl("ddlCategory") as DropDownList;
            string GLCode = string.Empty, Description = string.Empty, CurrencyName = string.Empty, Category = string.Empty, CategoryName = string.Empty;
            decimal Price = 0;
            short CurrencyID = 0;

            //Description
            if (txtDescription != null && string.IsNullOrEmpty(txtDescription.Text.Trim()))
                AddErrorPrompt(txtDescription.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            else if(CheckAdditionalRowExists(ID, txtDescription.Text.Trim()))
                AddErrorPrompt(txtDescription.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_EXISTS));
            //GLCode
            if (txtGLCode != null && string.IsNullOrEmpty(txtGLCode.Text.Trim()))
                AddErrorPrompt(txtGLCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //Price
            if (txtPrice != null)
            {
                if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
                    AddErrorPrompt(txtPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else
                {
                    if (!decimal.TryParse(txtPrice.Text.Trim().Replace(",", ""), out Price))
                        AddErrorPrompt(txtPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
                    else if (decimal.TryParse(txtPrice.Text.Trim().Replace(",", ""), out Price) && Price < 0)
                        AddErrorPrompt(txtPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.MIN_VALIDATION));
                }

            }
            //Category
            if (ddlCategory.SelectedValue == "0")
            {
                AddErrorPrompt(ddlCategory.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //Currency
            if (ddlCurrency != null && ddlCurrency.SelectedValue == "0")
                AddErrorPrompt(ddlCurrency.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));

            if (IsValid)
            {
                //Description
                Description = txtDescription.Text.Trim();
                //Category
                Category = ddlCategory.SelectedValue;
                CategoryName = ddlCategory.SelectedItem.Text;

                //GLCode
                GLCode = txtGLCode.Text.Trim();
                //Currency
                CurrencyID = Convert.ToInt16(ddlCurrency.SelectedValue);
                CurrencyName = ddlCurrency.SelectedItem.Text;

                UpdateAdditionalRow(e.RowIndex, ID, Description, Category, CategoryName, GLCode, Price, CurrencyID, CurrencyName);
                grvAdditionalService.EditIndex = -1;
                BindAdditionalGrid();
            }
            else
                ShowErrorPromptExtension();
        }

        protected void grvAdditionalService_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RemoveInvalidAdditionalRows();
            grvAdditionalService.EditIndex = -1;
            BindAdditionalGrid();
        }

        protected void grvAdditionalService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(grvAdditionalService.DataKeys[e.Row.RowIndex].Value);
                short CurrencyID = 0;
                string JobCategory = string.Empty;
                //Lấy danh sách chi tiết
                List<CustomerQuotationAdditionalServiceExtention> source = new List<CustomerQuotationAdditionalServiceExtention>();
                source = (List<CustomerQuotationAdditionalServiceExtention>)Session[ViewState["PageID"] + "dtSourceAdditional"];
                if (source == null)
                    source = new List<CustomerQuotationAdditionalServiceExtention>();
                //Tìm object theo ID
                CustomerQuotationAdditionalServiceExtention obj = source.Where(x => x.Id == ID).FirstOrDefault();
                if (obj != null)
                {
                    CurrencyID = obj.CurrencyID;
                    JobCategory = obj.Category;
                }

                //Category
                DropDownList ddlCategory = e.Row.FindControl("ddlCategory") as DropDownList;
                if (ddlCategory != null)
                {
                    List<TblReference> colls = ReferenceTableManager.SelectJobCategoryForDDL();
                    ddlCategory.DataSource = colls;//colls.Select(x => new { ID = x.ToString(), Name = x.ToString() });
                    ddlCategory.DataTextField = "Name";
                    ddlCategory.DataValueField = "ReferencesID";
                    ddlCategory.DataBind();
                    ddlCategory.SelectedValue = JobCategory;
                }

                //Bind currency
                DropDownList ddlCurrency = e.Row.FindControl("ddlCurrency") as DropDownList;
                if (ddlCurrency != null)
                {
                    ddlCurrency.DataSource = new CurrencyManager().SelectAllForDDL();
                    ddlCurrency.DataValueField = "CurrencyID";
                    ddlCurrency.DataTextField = "CurrencyName";
                    ddlCurrency.DataBind();
                    ddlCurrency.SelectedValue = CurrencyID.ToString();
                }
            }
            catch
            {
            }
        }

        protected void grvAdditionalService_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        #endregion

        #region OTHERCHARGES
        //Load detail data from db
        private void BindPricingMasterOtherCharges()
        {
            //Load Customer master template
            int CustomerID = 0;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PricingMasterTemplateSetting);
            if (setting != null)
                CustomerID = Convert.ToInt32(setting.SettingValue);

            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source = CustomerQuotationManager.SelectPricingMasterTemplateOtherCharges(CustomerID);

            Session[ViewState["PageID"] + "dtSourceOtherCharges"] = source;
        }

        private void BindOtherCharges(int CustomerID)
        {
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source = CustomerQuotationManager.SelectOtherCharges(CustomerID);

            Session[ViewState["PageID"] + "dtSourceOtherCharges"] = source;
        }

        //Bind data to grid detail
        private void BindOtherChargesGrid()
        {
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source = (List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"];
            grvOtherCharges.DataSource = source;
            grvOtherCharges.DataBind();
        }

        private void AddNewOtherChargesRow()
        {
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source = (List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"];
            if (source == null)
                source = new List<CustomerQuotationOtherChargesExtention>();

            CustomerQuotationOtherChargesExtention obj = new CustomerQuotationOtherChargesExtention();
            Random rnd = new Random();
            obj.Id = rnd.Next(-1000, -1);
            while (source.Where(x => x.Id == obj.Id).Count() > 0)
                obj.Id = rnd.Next(-1000, -1);

            obj.Description = string.Empty;
            obj.GLCode = string.Empty;
            obj.Price = 0;
            obj.CurrencyID = 0;
            source.Insert(0, obj);
            Session[ViewState["PageID"] + "dtSourceOtherCharges"] = source;
        }

        //Remove invalid row from list
        private void RemoveInvalidOtherChargesRows()
        {
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source.AddRange(((List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"]).Where(x => !string.IsNullOrEmpty(x.Description) && !string.IsNullOrEmpty(x.GLCode)).ToList());
            Session[ViewState["PageID"] + "dtSourceOtherCharges"] = source;
        }

        //Remove selected row
        private void RemoveSelectedOtherChargesRow(int ID)
        {
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source.AddRange(((List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"]).Where(x => x.Id != ID).ToList());
            Session[ViewState["PageID"] + "dtSourceOtherCharges"] = source;
        }

        //Kiểm tra thông tin giá này đã tồn tại trong danh sách chưa?
        private bool CheckOtherChargesRowExists(int ID, string Description)
        {
            bool ret = false;
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source = (List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"];
            if (source == null)
                source = new List<CustomerQuotationOtherChargesExtention>();
            ret = source.Where(x => x.Id != ID && x.Description == Description).Count() > 0 ? true : false;
            return ret;
        }

        //Update data to list
        private void UpdateOtherChargesRow(int RowIndex, int ID, string Description, string GLCode, decimal Price, short CurrencyID, string CurrencyName)
        {
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source = (List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"];
            if (source == null)
                source = new List<CustomerQuotationOtherChargesExtention>();
            CustomerQuotationOtherChargesExtention obj = source.Where(x => x.Id == ID).FirstOrDefault();
            if (obj != null)
            {
                //obj.CustomerId = CustomerID;
                obj.Description = Description;
                obj.GLCode = GLCode;
                obj.Price = Price;
                obj.CurrencyID = CurrencyID;
                obj.CurrencyName = CurrencyName;
            }
            source = source.OrderBy(o => o.Description).ToList<CustomerQuotationOtherChargesExtention>();
            Session[ViewState["PageID"] + "dtSourceOtherCharges"] = source;
        }

        //Save Detail
        private bool SaveOtherCharges(int CustomerID)
        {
            //CurrenSource
            List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
            source = (List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"];
            if (source == null)
                source = new List<CustomerQuotationOtherChargesExtention>();
            //OldSource
            List<CustomerQuotationOtherChargesExtention> oldSource = new List<CustomerQuotationOtherChargesExtention>();
            oldSource = CustomerQuotationManager.SelectOtherCharges(CustomerID);
            //DeleteSource
            var deleteSource = oldSource.Where(x => !source.Select(a => a.Id).Contains(x.Id));
            //Delete old source
            foreach (var item in deleteSource)
                CustomerQuotationManager.DeleteOtherCharges(item.Id);
            //Update new source
            foreach (CustomerQuotationOtherChargesExtention item in source)
            {
                TblCustomerQuotationOtherCharge obj = new TblCustomerQuotationOtherCharge();
                obj = (TblCustomerQuotationOtherCharge)item;
                obj.CustomerID = CustomerID;
                if (obj.Id < 0)
                {
                    CustomerQuotationManager.InsertOtherChagres(obj);
                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
                else
                {
                    CustomerQuotationManager.UpdateOtherChagres(obj);
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
            }
            BindOtherCharges(CustomerID);
            BindOtherChargesGrid();
            return true;
        }

        protected void btnOtherCharges_Click(object sender, EventArgs e)
        {
            RemoveInvalidOtherChargesRows();
            grvOtherCharges.EditIndex = 0;
            AddNewOtherChargesRow();
            BindOtherChargesGrid();
        }

        protected void grvOtherCharges_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = Convert.ToInt32(grvOtherCharges.DataKeys[e.RowIndex].Value);
            RemoveSelectedOtherChargesRow(ID);
            BindOtherChargesGrid();
        }

        protected void grvOtherCharges_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RemoveInvalidOtherChargesRows();
            grvOtherCharges.EditIndex = e.NewEditIndex;
            BindOtherChargesGrid();
        }

        protected void grvOtherCharges_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvOtherCharges.DataKeys[e.RowIndex].Value);

            CustomExtraTextbox txtDescription = grvOtherCharges.Rows[e.RowIndex].FindControl("txtDescription") as CustomExtraTextbox;
            CustomExtraTextbox txtGLCode = grvOtherCharges.Rows[e.RowIndex].FindControl("txtGLCode") as CustomExtraTextbox;
            ExtraInputMask txtPrice = grvOtherCharges.Rows[e.RowIndex].FindControl("txtPrice") as ExtraInputMask;
            DropDownList ddlCurrency = grvOtherCharges.Rows[e.RowIndex].FindControl("ddlCurrency") as DropDownList;
            string GLCode = string.Empty, Description = string.Empty, CurrencyName = string.Empty;
            decimal Price = 0;
            short CurrencyID = 0;

            //Description
            if (txtDescription != null && string.IsNullOrEmpty(txtDescription.Text.Trim()))
                AddErrorPrompt(txtDescription.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            else if (CheckOtherChargesRowExists(ID, txtDescription.Text.Trim()))
                AddErrorPrompt(txtDescription.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_EXISTS));
            //GLCode
            if (txtGLCode != null && string.IsNullOrEmpty(txtGLCode.Text.Trim()))
                AddErrorPrompt(txtGLCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //Price
            if (txtPrice != null)
            {
                if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
                    AddErrorPrompt(txtPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else
                {
                    if (!decimal.TryParse(txtPrice.Text.Trim().Replace(",", ""), out Price))
                        AddErrorPrompt(txtPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
                    else if (decimal.TryParse(txtPrice.Text.Trim().Replace(",", ""), out Price) && Price < 0)
                        AddErrorPrompt(txtPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.MIN_VALIDATION));
                }

            }
            //Currency
            if (ddlCurrency != null && ddlCurrency.SelectedValue == "0")
                AddErrorPrompt(ddlCurrency.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));

            if (IsValid)
            {
                //Description
                Description = txtDescription.Text.Trim();
                //GLCode
                GLCode = txtGLCode.Text.Trim();
                //Currency
                CurrencyID = Convert.ToInt16(ddlCurrency.SelectedValue);
                CurrencyName = ddlCurrency.SelectedItem.Text;

                UpdateOtherChargesRow(e.RowIndex, ID, Description, GLCode, Price, CurrencyID, CurrencyName);
                grvOtherCharges.EditIndex = -1;
                BindOtherChargesGrid();
            }
            else
                ShowErrorPromptExtension();
        }

        protected void grvOtherCharges_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RemoveInvalidOtherChargesRows();
            grvOtherCharges.EditIndex = -1;
            BindOtherChargesGrid();
        }

        protected void grvOtherCharges_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(grvOtherCharges.DataKeys[e.Row.RowIndex].Value);
                short CurrencyID = 0;
                //Lấy danh sách chi tiết
                List<CustomerQuotationOtherChargesExtention> source = new List<CustomerQuotationOtherChargesExtention>();
                source = (List<CustomerQuotationOtherChargesExtention>)Session[ViewState["PageID"] + "dtSourceOtherCharges"];
                if (source == null)
                    source = new List<CustomerQuotationOtherChargesExtention>();
                //Tìm object theo ID
                CustomerQuotationOtherChargesExtention obj = source.Where(x => x.Id == ID).FirstOrDefault();
                if (obj != null)
                {
                    CurrencyID = obj.CurrencyID;
                }

                //Bind currency
                DropDownList ddlCurrency = e.Row.FindControl("ddlCurrency") as DropDownList;
                if (ddlCurrency != null)
                {
                    ddlCurrency.DataSource = new CurrencyManager().SelectAllForDDL();
                    ddlCurrency.DataValueField = "CurrencyID";
                    ddlCurrency.DataTextField = "CurrencyName";
                    ddlCurrency.DataBind();
                    ddlCurrency.SelectedValue = CurrencyID.ToString();
                }
            }
            catch
            {
            }
        }

        protected void grvOtherCharges_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        #endregion
    }
}