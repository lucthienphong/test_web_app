using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class Customer : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "customer_manager";
            }
        }

        protected int CustomerID
        {
            get
            {
                int ID = 0;
                if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"], out ID);
                else if(Session[ViewState["PageID"] + "ID"] != null)
                    int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID);
                return ID;
            }
        }

        protected string IsShow
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                ApplyControlText();
                if (Request.QueryString["ID"] != null)
                {
                    BindCustomerData();
                    base.SaveBaseDataBeforeEdit();
                }
                else
                {
                    ResetDataFields();
                }
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvContacts.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CONTACT_NAME);
            grvContacts.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CONTACT_TITLE);
            grvContacts.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CONTACT_DESIGNATION);
        }

        private void BindDDL()
        {
            BindSaleRepDDL();
            BindDeliveryDDL();
            BindPaymentDDL();
            BindCountryDDL();
            BindCustomerGroupDDL();
        }

        private void BindDeliveryDDL()
        {
            List<TblReference> list = ReferenceTableManager.SelectDeliveryForDDL();
            ddlDelivery.DataSource = list;
            ddlDelivery.DataTextField = "Name";
            ddlDelivery.DataValueField = "ReferencesID";
            ddlDelivery.DataBind();
        }

        private void BindPaymentDDL()
        {
            List<TblReference> list = ReferenceTableManager.SelectPaymentForDDL();
            ddlPayment.DataSource = list;
            ddlPayment.DataTextField = "Name";
            ddlPayment.DataValueField = "ReferencesID";
            ddlPayment.DataBind();
        }

        private void BindCountryDDL()
        {
            ddlCountry.Items.Clear();
            ddlCountry.Items.Add(new ListItem("--Select country--", "0"));
            TblReferenceCollection coll = ReferenceTableManager.SelectReferenceByType(ReferenceTypeHelper.Country);
            if (coll != null && coll.Count > 0)
            {
                foreach (var item in coll)
                    ddlCountry.Items.Add(new ListItem(item.Code, item.ReferencesID.ToString()));
            }
        }
        
        private void BindCustomerGroupDDL()
        {
            ddlCustomerGroup.Items.Clear();
            ddlCustomerGroup.Items.Add(new ListItem("--Select group--", "0"));
            TblReferenceCollection coll = ReferenceTableManager.SelectReferenceByType(ReferenceTypeHelper.CustomerGroup);
            if (coll != null && coll.Count > 0)
            {
                foreach (var item in coll)
                    ddlCustomerGroup.Items.Add(new ListItem(item.Name, item.ReferencesID.ToString()));
            }
        }

        
        private void BindSaleRepDDL()
        {
            List<TblStaffExeption> colls = StaffManager.ListForDDL(DepartmentSetting.Sale);
            ddlSaleRep.DataSource = colls;
            ddlSaleRep.DataTextField = "FullName";
            ddlSaleRep.DataValueField = "StaffID";
            ddlSaleRep.DataBind();
        }

        private void BindCustomerData()
        {
            try
            {
                int ID;
                if (int.TryParse(Request.QueryString["ID"], out ID))
                {
                    //Kiểm tra nhân viên còn tồn tại không
                    TblCustomer obj = CustomerManager.SelectByID(ID);
                    if (obj != null)
                    {
                        bool isBrand = obj.IsBrand == 1 ? true : false;
                        IsShow = isBrand ? "none" : "normal";
                        //ShowControl(isBrand);
                        chkIsBrand.Checked = isBrand;
                        txtCode.Text = obj.Code;
                        txtName.Text = obj.Name;
                        txtAddress.Text = obj.Address;
                        txtCity.Text = obj.City;
                        txtTelNumber.Text = obj.Tel;
                        txtFax.Text = obj.Fax;
                        txtPostcode.Text = obj.PostCode;

                        //txtCountry.Text = obj.Country;
                        ddlSaleRep.SelectedValue = obj.SaleRepID == null ? "0" : obj.SaleRepID.ToString();
                        if (obj.DeliveryID != null)
                            ddlDelivery.SelectedValue = obj.DeliveryID.ToString();
                        else
                            ddlDelivery.SelectedIndex = 0;
                        if (obj.PaymentID != null)
                            ddlPayment.SelectedValue = obj.PaymentID.ToString();
                        else
                            ddlPayment.SelectedIndex = 0;
                        chkIsObsolete.Checked = obj.IsObsolete;

                        txtSaleRecords.Text = obj.SaleRecords;
                        txtPackingRequirement.Text = obj.PackagingRequirement;
                        txtTechData.Text = obj.TechData;
                        txtForwarderName.Text = obj.ForwarderName;
                        txtForwarderAddress.Text = obj.ForwarderAddress;
                        txtEmail.Text = obj.Email;
                        txtShortName.Text = obj.ShortName;
                        txtGSTCode.Text = obj.TaxCode;
                        txtTIN.Text = obj.Tin;
                        txtSAPCode.Text = obj.SAPCode;
                        txtInternalOrderNo.Text = obj.InternalOrderNo;
                        BindContractData(obj.CustomerID, false);

                        if (ddlCountry.Items.FindByValue(obj.CountryID.ToString()) != null)
                            ddlCountry.Items.FindByValue(obj.CountryID.ToString()).Selected = true;
                        if (obj.GroupID.HasValue && obj.GroupID.Value > 0)
                        {
                            if (ddlCustomerGroup.Items.FindByValue(obj.GroupID.Value.ToString()) != null)
                                ddlCustomerGroup.Items.FindByValue(obj.GroupID.Value.ToString()).Selected = true;
                        }

                        Session[ViewState["PageID"] + "ID"] = obj.CustomerID;
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
                    IsShow = "none";
                    //ShowControl(false);
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
            txtCode.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtTelNumber.Text = "";
            txtFax.Text = "";
            txtPostcode.Text = "";
            //txtCountry.Text = "";
            ddlSaleRep.SelectedValue = "0";

            ddlDelivery.SelectedIndex = 0;
            ddlPayment.SelectedIndex = 0;
            chkIsObsolete.Checked = false;

            txtSaleRecords.Text = "";
            txtPackingRequirement.Text = "";
            txtTechData.Text = "";
            txtForwarderName.Text = "";
            txtForwarderAddress.Text = "";
            txtEmail.Text = "";
            txtShortName.Text = "";
            txtGSTCode.Text = "";
            txtTIN.Text = "";
            
            txtInternalOrderNo.Text = "";
            BindContractData(0, false);

            Session[ViewState["PageID"] + "ID"] = null;
        }

        //SAVE DATA
        //---------------------------------------------------------------------------------------------------------------------------
        //Return "cannot delete" contacts
        private List<TblContact> GetDeleteList(int ID)
        {
            List<TblContact> oldContacts = ContactManager.SelectAll(ID, false);
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];
            List<TblContact> deleteList = new List<TblContact>();
            deleteList = oldContacts.Where(x => !source.Select(s => s.ContactID).Contains(x.ContactID)).ToList<TblContact>();

            return deleteList;
        }

        private string GetCannotDeleteList(List<TblContact> deleteList)
        {
            string ret = string.Empty;
            foreach (TblContact item in deleteList)
            {
                if (ContactManager.IsBeingUsed(item.ContactID))
                    ret += (item.ContactName + ", ");
            }
            if (!string.IsNullOrEmpty(ret))
                ret = ret.Substring(0, ret.Length - 2);
            return ret;
        }

        //Save contact
        private void SaveContacts(int CustomerID)
        {
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];
            List<TblContact> deleteList = GetDeleteList(CustomerID);
            //Xóa các contact đã bị xóa
            if (deleteList != null && deleteList.Count >0)
            {
                foreach (TblContact obj in deleteList)
                {
                    ContactManager.Delete(obj.ContactID);
                }
            }

            //Cập nhật các contact mới
            if (source != null && source.Count > 0)
            {
                foreach (TblContact obj in source)
                {
                    if (obj.ContactID > 0)
                    {
                        obj.CustomerID = CustomerID;
                        ContactManager.Update(obj);
                    }
                    else
                    {
                        obj.CustomerID = CustomerID;
                        ContactManager.Insert(obj);
                    }

                    //Lưu vào logging
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
            }
        }

        private void SaveCustomer()
        {
           
            try
            {
                //-------BEGIN VALIDATION
                int ID = Session[ViewState["PageID"] + "ID"] == null ? 0 : int.Parse(Session[ViewState["PageID"] + "ID"].ToString());
                ///Code
                if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                    AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else if(CustomerManager.Exists(ID, txtCode.Text.Trim()))
                    AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE_EXISTS));
                ///Name
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                    AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///Address
                if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
                    AddErrorPrompt(txtAddress.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///City
                if (string.IsNullOrEmpty(txtCity.Text.Trim()))
                    AddErrorPrompt(txtCity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///TelNumber
                if (string.IsNullOrEmpty(txtTelNumber.Text.Trim()))
                    AddErrorPrompt(txtTelNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///Fax
                if (string.IsNullOrEmpty(txtFax.Text.Trim()))
                    AddErrorPrompt(txtFax.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///Postcode
                if (string.IsNullOrEmpty(txtPostcode.Text.Trim()))
                    AddErrorPrompt(txtPostcode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                //Country
                if (ddlCountry.SelectedValue.Equals("0"))
                    AddErrorPrompt(ddlCountry.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                //SaleRep
                if (ddlSaleRep.SelectedValue == "0")
                    AddErrorPrompt(ddlSaleRep.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                //Short Name
                if (string.IsNullOrEmpty(txtShortName.Text.Trim()))
                    AddErrorPrompt(txtShortName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                //Group
                if (ddlCustomerGroup.SelectedValue.Equals("0"))
                    AddErrorPrompt(ddlCustomerGroup.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                //SapeCode - Customer Code
                if (string.IsNullOrEmpty(txtSAPCode.Text.Trim()))
                    AddErrorPrompt(txtSAPCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                //SAP
                if (string.IsNullOrEmpty(txtInternalOrderNo.Text.Trim()))
                    AddErrorPrompt(txtInternalOrderNo.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));

                if (grvContacts.Rows.Count == 0)
                {
                    string errorMessage = "Please enter the contact's information";
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), errorMessage, MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }

               
             
                string message = GetCannotDeleteList(GetDeleteList(ID));
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format("{0}:<br/> {1}", ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CAN_NOT_DELETE_CONTACT), message);
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }

                if (IsValid)
                {
                    //LƯU KHÁCH HÀNG---------------------------------------------------------------
                    TblCustomer obj = new TblCustomer();
                    if (Session[ViewState["PageID"] + "ID"] != null)//Edit
                    {
                        //Kiểm tra quyền
                        if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                        {
                            MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msgRole, null, false, false);
                            return;
                        }


                        obj = CustomerManager.SelectByID(int.Parse(Session[ViewState["PageID"] + "ID"].ToString()));
                        if (obj == null)//Nếu không tồn tại khách hàng thì thông báo lỗi
                        {
                            MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(errMsg, null, false, false);
                            BindCustomerData();
                            return;
                        }
                        
                        obj.Code = txtCode.Text.Trim();
                        obj.Name = txtName.Text.Trim();
                        obj.Address = txtAddress.Text.Trim();
                        obj.City = txtCity.Text.Trim();
                        obj.Tel = txtTelNumber.Text.Trim();
                        obj.Fax = txtFax.Text.Trim();
                        obj.PostCode = txtPostcode.Text.Trim();
                        obj.SaleRepID = ddlSaleRep.SelectedValue == "0" ? (int?)0 : Convert.ToInt32(ddlSaleRep.SelectedValue);
                        int? DeliveryID = ddlDelivery.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlDelivery.SelectedValue);
                        obj.DeliveryID = DeliveryID;
                        obj.DeliveryNote = ddlDelivery.SelectedValue == "0" ? "N/A" : ddlDelivery.SelectedItem.Text;
                        int? PaymentID = ddlPayment.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlPayment.SelectedValue);
                        obj.PaymentID = PaymentID;
                        obj.PaymentNote = ddlPayment.SelectedValue == "0" ? "N/A" : ddlPayment.SelectedItem.Text;
                        obj.IsObsolete = chkIsObsolete.Checked;
                        obj.SaleRecords = txtSaleRecords.Text.Trim();
                        obj.PackagingRequirement = txtPackingRequirement.Text.Trim();
                        obj.TechData = txtTechData.Text.Trim();
                        obj.ForwarderName = txtForwarderName.Text.Trim();
                        obj.ForwarderAddress = txtForwarderAddress.Text.Trim();

                        int countryID = 0; int.TryParse(ddlCountry.SelectedValue, out countryID);
                        int groupID = 0; int.TryParse(ddlCustomerGroup.SelectedValue, out groupID);

                        obj.Email = txtEmail.Text;
                        obj.ShortName = txtShortName.Text.Trim();
                        obj.TaxCode = txtGSTCode.Text.Trim();
                        obj.Tin = txtTIN.Text.Trim();
                        obj.SAPCode = txtSAPCode.Text.Trim();
                        obj.InternalOrderNo = txtInternalOrderNo.Text.Trim();
                        obj.CountryID = countryID;
                        obj.GroupID = groupID;
                        obj.IsBrand = chkIsBrand.Checked ? (byte)1 : (byte)0;
                        obj = CustomerManager.Update(obj);
                        if (!chkIsBrand.Checked)
                            SaveContacts(obj.CustomerID);

                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());

                        LoggingActions("Customer",
                            LogsAction.Objects.Action.UPDATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Customer Code", Data = obj.Code } ,
                                new JsonData() { Title = "Customer Name", Data = obj.Name } 
                            }));

                        Session[ViewState["PageID"] + "ID"] = obj.CustomerID;
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_SAVED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
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

                        obj = new TblCustomer();
                        obj.Code = txtCode.Text.Trim();
                        obj.Name = txtName.Text.Trim();
                        obj.Address = txtAddress.Text.Trim();
                        obj.City = txtCity.Text.Trim();
                        obj.Tel = txtTelNumber.Text.Trim();
                        obj.Fax = txtFax.Text.Trim();
                        obj.PostCode = txtPostcode.Text.Trim();
                        //obj.Country = txtCountry.Text.Trim();
                        obj.SaleRepID = ddlSaleRep.SelectedValue == "0" ? (int?)0 : Convert.ToInt32(ddlSaleRep.SelectedValue);

                        int? DeliveryID = ddlDelivery.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlDelivery.SelectedValue);
                        obj.DeliveryID = DeliveryID;
                        obj.DeliveryNote = ddlDelivery.SelectedValue == "0" ? string.Empty : ddlDelivery.SelectedItem.Text;
                        int? PaymentID = ddlPayment.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlPayment.SelectedValue);
                        obj.PaymentID = PaymentID;
                        obj.PaymentNote = ddlPayment.SelectedValue == "0" ? string.Empty : ddlPayment.SelectedItem.Text;
                        obj.IsObsolete = chkIsObsolete.Checked;

                        obj.SaleRecords = txtSaleRecords.Text.Trim();
                        obj.PackagingRequirement = txtPackingRequirement.Text.Trim();
                        obj.TechData = txtTechData.Text.Trim();
                        obj.ForwarderName = txtForwarderName.Text.Trim();
                        obj.ForwarderAddress = txtForwarderAddress.Text.Trim();

                        int countryID = 0; int.TryParse(ddlCountry.SelectedValue, out countryID);
                        int groupID = 0; int.TryParse(ddlCustomerGroup.SelectedValue, out groupID);

                        obj.Email = txtEmail.Text;
                        obj.ShortName = txtShortName.Text.Trim();
                        obj.TaxCode = txtGSTCode.Text.Trim();
                        obj.Tin = txtTIN.Text.Trim();
                        obj.SAPCode = txtSAPCode.Text.Trim();
                        obj.InternalOrderNo = txtInternalOrderNo.Text.Trim();
                        obj.CountryID = countryID;
                        obj.GroupID = groupID;
                        obj.IsBrand = chkIsBrand.Checked ? (byte)1 : (byte)0;
                        obj = CustomerManager.Insert(obj);
                        if (!chkIsBrand.Checked)
                            SaveContacts(obj.CustomerID);

                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());

                        LoggingActions("Customer",
                            LogsAction.Objects.Action.CREATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Customer Code", Data = txtCode.Text } ,
                                new JsonData() { Title = "Customer Name", Data = txtName.Text } 
                            }));

                        Session[ViewState["PageID"] + "ID"] = obj.CustomerID;
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_SAVED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);

                        
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
            SaveCustomer();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/CustomerList.aspx");
        }


        #region Contacts
        //ADD, EDIT, DELETE DEPARTMENT
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        public void BindContractData(int CustomerID, bool AddNew)
        {
            List<TblContact> source = ContactManager.SelectAll(CustomerID, AddNew);
            grvContacts.DataSource = source;
            grvContacts.DataBind();
            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        private void BindGrid()
        {
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];
            grvContacts.DataSource = source;
            grvContacts.DataBind();
        }

        /// <summary>
        /// Thêm dòng mới vào datagridview => Không sử dụng
        /// </summary>
        private void addRow()
        {
            //Get list of department
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];
            TblContact obj = new TblContact();

            Random rnd = new Random();
            short randID = 0;
            while (source.Where(x => x.ContactID == randID).Count() > 0)
            {
                randID = (short)rnd.Next(-10000, -1);
            }

            obj.ContactID = randID;
            obj.ContactName = "";
            obj.Honorific = "";
            obj.Designation = "";
            obj.Tel = "";
            obj.Email = "";
            obj.CustomerID = 0;

            source.Insert(0, obj);

            //Update list
            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, int contactID, string contactName, string honorific, string designation,string tel,string email)
        {
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];

            TblContact obj = new TblContact();
            obj = source.Where(x => x.ContactID == contactID).FirstOrDefault();
            if (obj != null)
            {
                TblContact newObj = new TblContact();
                newObj.ContactID = obj.ContactID;
                newObj.ContactName = contactName;
                newObj.Honorific = honorific;
                newObj.Designation = designation;
                newObj.Tel = tel;
                newObj.Email = email;

                source.Remove(obj);
                source.Add(newObj);
            }
            else
            {
                obj = new TblContact();
                obj.ContactName = contactName;
                obj.Honorific = honorific;
                obj.Designation = designation;
                obj.Tel = tel;
                obj.Email = email;
                source.Add(obj);
            }
            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        //Xóa dữ liệu không hợp lệ
        private void removeInvalidRows()
        {
            //Get list of department
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];
            if (source != null)
                source.RemoveAll(x => string.IsNullOrEmpty(x.ContactName));

            Session[ViewState["PageID"] + "tableSource"] = source;
        }

        //Xóa dữ liệu được chọn
        private void removeSelectedRows(List<int> idList)
        {
            //Get list of department
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];
            source.RemoveAll(x => idList.Contains(x.ContactID));

            Session[ViewState["PageID"] + "tableSource"] = source;
        }
        #endregion

        protected void grvContacts_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grvContacts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            removeInvalidRows();
            grvContacts.EditIndex = e.NewEditIndex;
            BindGrid();

            btnAddContact.Visible = false;
            btnDeleteContact.Visible = false;
        }

        protected void grvContacts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvContacts.DataKeys[e.RowIndex].Value);
            TextBox txtContactName = (TextBox)grvContacts.Rows[e.RowIndex].FindControl("txtContactName");
            if (string.IsNullOrEmpty(txtContactName.Text))
            {
                AddErrorPrompt(txtContactName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            TextBox txtHonorific = (TextBox)grvContacts.Rows[e.RowIndex].FindControl("txtHonorific");
            if (string.IsNullOrEmpty(txtHonorific.Text))
            {
                AddErrorPrompt(txtHonorific.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            TextBox txtDesignation = (TextBox)grvContacts.Rows[e.RowIndex].FindControl("txtDesignation");
            if (string.IsNullOrEmpty(txtDesignation.Text))
            {
                AddErrorPrompt(txtDesignation.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            TextBox txtPhone = (TextBox)grvContacts.Rows[e.RowIndex].FindControl("txtPhone");
            TextBox txtEmail = (TextBox)grvContacts.Rows[e.RowIndex].FindControl("txtEmail");
            string tel = string.Empty;
            string email = string.Empty;

            if (txtPhone != null)
                tel = txtPhone.Text.Trim();
            if (txtEmail != null)
                email = txtEmail.Text.Trim();

            if (IsValid)
            {
                string ContactName = txtContactName.Text.Trim();
                string Honorific = txtHonorific.Text.Trim();
                string Designation = txtDesignation.Text.Trim();

                updateRow(e.RowIndex, ID, ContactName, Honorific, Designation, tel, email);
                grvContacts.EditIndex = -1;
                BindGrid();

                btnAddContact.Visible = true;
                btnDeleteContact.Visible = true;
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        protected void grvContacts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvContacts.EditIndex = -1;
            removeInvalidRows();
            BindGrid();

            btnAddContact.Visible = true;
            btnDeleteContact.Visible = true;
        }

        protected void grvContacts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            int ID = 0;
            if (Session[ViewState["PageID"] + "ID"] != null)
                int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID);
            removeInvalidRows();
            addRow();
            grvContacts.EditIndex = 0;
            //BindContractData(ID, true);
            BindGrid();
            btnAddContact.Visible = false;
            btnDeleteContact.Visible = false;
        }

        protected void btnDeleteContact_Click(object sender, EventArgs e)
        {
            List<TblContact> source = (List<TblContact>)Session[ViewState["PageID"] + "tableSource"];
            List<int> idList = new List<int>();
            string nameList = string.Empty;
            for (int i = 0; i < grvContacts.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvContacts.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)//Nếu tồn tại dòng dữ liệu được check thì hiện thông báo xóa
                {
                    int ID = 0;
                    if (int.TryParse(grvContacts.DataKeys[i].Value.ToString(), out ID))
                    {
                        if (!ContactManager.IsBeingUsed(ID))
                            idList.Add(ID);
                        else
                        {
                            string ContactName = source.Where(x => x.ContactID == ID).Select(x => x.ContactName).FirstOrDefault();
                            if(!string.IsNullOrEmpty(ContactName))
                            {
                                nameList += (ContactName + ", ");
                            }
                        }
                    }
                }
            }

            if (idList.Count() > 0 || !string.IsNullOrEmpty(nameList))
            {
                removeSelectedRows(idList);
                BindGrid();

                if (!string.IsNullOrEmpty(nameList))
                {
                    nameList = nameList.Substring(0, nameList.Length - 2);
                    string message = string.Format("{0}:<br/> {1}", ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CAN_NOT_DELETE_CONTACT), nameList);
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                }
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.SELECT_DATA_TO_DELETE), MSGButton.OK, MSGIcon.Info);
                OpenMessageBox(msg, null, false, false);
            }
        }
        #endregion

        //private void ShowControl(bool isBrand)
        //{
        //    if (isBrand)
        //    {
        //        //divLeft.Visible = false;
        //        divRight.Visible = false;
        //        divShipping.Visible = false;
        //    }
        //    else
        //    {
        //        divRight.Visible = true;
        //        divShipping.Visible = true;
        //    }
        //}
        protected void chkIsBrand_CheckedChanged(object sender, EventArgs e)
        {
            //ShowControl(chkIsBrand.Checked);
        }

        protected void btnPricingMaster_Click(object sender, EventArgs e)
        {
            if (CustomerID != 0)
                Response.Redirect(string.Format("CustomerQuotation.aspx?ID={0}", CustomerID));
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_CUSTOMER), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
            }
        }
    }
}