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

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class PurchaseOrder : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "purchase_order_manager";
            }
        }

        private string purorderID;

        public string PurorderID
        {
            get { return purorderID; }
            set { purorderID = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    PurorderID = Request.QueryString["ID"];
                }
                BindDDL();
                LoadData();

            }
        }

        private void BindSupplierForDDL()
        {
            //throw new NotImplementedException();
            SupplierManager s = new SupplierManager();
            var supList = s.GetAllSupplier(false);

            ddlSuplier.Items.Add(new ListItem(ResourceTextManager.GetApplicationText(ResourceText.CHOOSE_SUPPLIER), ""));
            foreach (var item in supList)
            {
                ListItem _item = new ListItem(item.Name, item.SupplierID.ToString());
                ddlSuplier.Items.Add(_item);
            }
        }

        private void BindDDL()
        {
            BindCurrencyDDL();
            BindSupplierForDDL();
        }

        private void BindCurrencyDDL()
        {
            TblCurrencyCollection list = new CurrencyManager().SelectAllForDDL();
            ddlCurrency.DataSource = list;
            ddlCurrency.DataValueField = "CurrencyID";
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataBind();
        }

        private void LoadData()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    string purID = Request.QueryString["ID"];
                    txtName.ReadOnly = true;
                    //txtCode.ReadOnly = true;
                    ddlJobNumber.Enabled = false;
                    ddlRevNumber.Enabled = false;
                    btnDelete.Visible = true;
                    TblPurchaseOrder purOrder = PurchaseOrderManager.SelectPurchaseOrder(purID);
                    if (purOrder != null)
                    {
                        lblOrderNumber.Text = purOrder.OrderNumber;
                        txtOrderDate.Text = purOrder.OrderDate.ToString("dd/MM/yyyy");
                        DateTime bDate = purOrder.RequiredDeliveryDate != null ? (DateTime)purOrder.RequiredDeliveryDate : purOrder.OrderDate;
                        txtBaseDeliveryDate.Text = bDate.ToString("dd/MM/yyyy");
                        TblSupplier supplier = new SupplierManager().SelectByID(purOrder.SupplierID);

                        ddlSuplier.SelectedValue = supplier.SupplierID.ToString();
                        ddlCurrency.SelectedValue = purOrder.CurrencyID.ToString();
                        txtContactName.Text = purOrder.ContactName;
                        txtContactPhone.Text = purOrder.ContactPhone;
                        txtContactFax.Text = purOrder.ContactEmail;

                        /*Từ tblJob có được JobID - JobNumber - RevNumber - CustomerID*/
                        TblJob job = JobManager.SelectByID(purOrder.JobID);
                        if (job != null)
                        {
                            ddlJobNumber.Items.Add(new ListItem(job.JobNumber, job.JobID.ToString()));
                            ddlRevNumber.Items.Add(new ListItem(job.RevNumber.ToString(), job.JobID.ToString()));
                            txtJobName.Text = job.JobName;
                            txtDesign.Text = job.Design;
                            if (Convert.ToBoolean(job.IsOutsource))
                                ddlSuplier.Enabled = false;
                            else
                                ddlSuplier.Enabled = true;
                            BindCylinders(purOrder.JobID, Convert.ToBoolean(job.IsOutsource));
                        }

                        TblCustomer cus = CustomerManager.SelectByID(job.CustomerID);
                        if (cus != null)
                        {
                            txtName.Text = cus.Name;
                            txtCode.Text = cus.Code;
                        }

                        txtTotalCylinder.Text = purOrder.TotalNumberOfCylinders.ToString();

                        chkIsUrgent.Checked = Convert.ToBoolean(purOrder.IsUrgent);

                        txtRemark.Text = purOrder.Remark;

                        ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintPurchaseOrder.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", PurorderID);
                    }
                }
                else
                {
                    ResetField();
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void ResetField()
        {
            txtOrderDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            lblOrderNumber.Text = CreateOrderbNumber();
            //Load Base Currency
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCurrencySetting);
            if (setting != null)
                ddlCurrency.SelectedValue = setting.SettingValue;
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Value.ToString().Equals("DELETE_PURCHASE_ORDER"))
                    {

                        if (!string.IsNullOrEmpty(Request.QueryString["ID"]) || ViewState["ID"] != null)
                        {
                            string purID = !string.IsNullOrEmpty(Request.QueryString["ID"]) ? Request.QueryString["ID"] : ViewState["ID"].ToString();
                            TblPurchaseOrder purOder = PurchaseOrderManager.SelectPurchaseOrder(purID);
                            if (purOder != null)
                            {
                                PurchaseOrderManager.DeleteAllPurchaseOrderCylinder(purOder.PurchaseOrderID);

                                if (PurchaseOrderManager.DeletePurchaseOrder(purOder.PurchaseOrderID))
                                {
                                    Response.Redirect("PurchaseOrderList.aspx", false);
                                }
                            }

                        }
                    }

                    if (e.Value.ToString().Equals("RECREATE_PURCHASE_ORDER"))
                    {
                        lblOrderNumber.Text = CreateOrderbNumber();
                        SaveData();
                    }

                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "DELETE_PURCHASE_ORDER";
            CurrentConfirmResult = result;
            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.WARNING), ResourceTextManager.GetApplicationText(ResourceText.DO_YOU_REALLY_WAN_TO_DELETE_THIS_PURCHASE), MSGButton.DeleteCancel, MSGIcon.Error);
            OpenMessageBox(msg, result, false, false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PurchaseOrderList.aspx");
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                TblCustomer cust = CustomerManager.SelectByKeyword(txtName.Text.Trim()).FirstOrDefault();
                if (cust != null)
                {
                    LoadJobNumber(cust.CustomerID);
                }
            }
        }

        protected void ddlRevNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            string JobID = ddlRevNumber.SelectedValue;

            int b = 0;
            bool a = int.TryParse(JobID, out b);

            if (a)
            {
                TblJob j = JobManager.SelectByID(b);
                if (j != null)
                {
                    txtJobName.Text = j.JobName;
                    txtDesign.Text = j.Design;
                    if (Convert.ToBoolean(j.IsOutsource))
                    {
                        ddlSuplier.SelectedValue = j.SupplierID.ToString();
                        ddlSuplier.Enabled = false;
                    }
                    else
                    {
                        ddlSuplier.Enabled = true;
                    }
                }
                BindCylinders(b, j != null ? Convert.ToBoolean(j.IsOutsource) : false);
                upnlCylinder.Update();
            }
            else
            {
                pnRecord.Visible = true;
                pnListCylinder.Visible = false;
                txtTotalCylinder.Text = "0";
            }
        }

        protected void ddlJobNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadJobRevNumber(ddlJobNumber.SelectedValue);
            ddlRevNumber_SelectedIndexChanged(null, null);
        }

        #region GridView Cylinder

        protected void gvCylinders_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvClinders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvClinders.EditIndex = e.NewEditIndex;
            List<CylinderOderModel> dtSource = (List<CylinderOderModel>)Session[ViewState["PageID"] +  "dtSource"];
            if (dtSource != null)
            {
                gvClinders.DataSource = dtSource;
                gvClinders.DataBind();
            }

        }

        protected void gvClinders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvClinders_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvClinders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            List<CylinderOderModel> dtSource = (List<CylinderOderModel>)Session[ViewState["PageID"] + "dtSource"];
            if (dtSource != null)
            {
                GridView gv = (GridView)sender;
                HiddenField hdfCylinderID = gv.Rows[e.RowIndex].FindControl("hdfCylinderID") as HiddenField;
                ExtraInputMask txtPrice = gv.Rows[e.RowIndex].FindControl("txtPrice") as ExtraInputMask;
                ExtraInputMask txtQuantity = gv.Rows[e.RowIndex].FindControl("txtQuantity") as ExtraInputMask;
                if (txtPrice != null && hdfCylinderID != null)
                {
                    List<CylinderOderModel> _dtSource = new List<CylinderOderModel>();
                    foreach (var item in dtSource)
                    {
                        if (item.objCylinder.CylinderID == int.Parse(hdfCylinderID.Value))
                        {
                            TblCustomerQuotationDetail cqObj = CustomerQuotationManager.SelectDetailByID(item.objCylinder.PricingID);

                            item.UnitPriceExtension = decimal.Parse(txtPrice.Text);
                            item.Quantity = int.Parse(txtQuantity.Text);
                            //item.Total = item.objCylinder.POQuantity != null ? decimal.Parse(item.UnitPriceExtension) * (decimal)item.objCylinder.POQuantity : 1;
                            item.Total = item.UnitPriceExtension * item.Quantity * (decimal)(cqObj.UnitOfMeasure == UnitOfMeasure.cm2.ToString() ? item.objCylinder.Circumference * item.objCylinder.FaceWidth/100 : (double)1); //item.UnitPriceExtension * item.Quantity;
                        }

                        _dtSource.Add(item);
                    }
                    Session[ViewState["PageID"] +  "dtSource"] = _dtSource;

                    gvClinders.EditIndex = -1;
                    gvClinders.DataSource = _dtSource;
                    gvClinders.DataBind();
                }
            }

        }
        #endregion

        private void LoadJobRevNumber(string JobNumber)
        {
            ddlRevNumber.DataSource = JobManager.SelectRevNumberByJobNumber(JobNumber, 0);
            ddlRevNumber.DataTextField = "Value";
            ddlRevNumber.DataValueField = "Key";
            ddlRevNumber.DataBind();
            upnlJobRev.Update();
        }

        private void LoadJobNumber(int custID)
        {
            ddlJobNumber.DataSource = JobManager.SelectJobNumberByCustomerID(custID, false, false);
            ddlJobNumber.DataBind();
            upnlJobRev.Update();
            ddlJobNumber_SelectedIndexChanged(null, null);
        }

        public static string CreateOrderbNumber()
        {
            string _No = "1" + DateTime.Today.ToString("yy") + "6";
            string _MaxNumber = new Select(Aggregate.Max(TblPurchaseOrder.OrderNumberColumn))
                .From(TblPurchaseOrder.Schema)
                .Where(TblPurchaseOrder.OrderNumberColumn).Like(_No + "%")
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

        private void BindCylinders(int JobID, bool IsOutsource)
        {
            TblCylinderCollection cylinders = CylinderManager.GetCylindersByJobID(JobID, IsOutsource);
            if (cylinders != null && cylinders.Count() > 0)
            {
                pnRecord.Visible = false;
                pnListCylinder.Visible = true;

                #region bind cylinder

                int purID = 0;
                bool a = int.TryParse(Request.QueryString["ID"], out purID);

                List<CylinderOderModel> listCylinderModel = new List<CylinderOderModel>();

                foreach (var item in cylinders)
                {
                    CylinderOderModel c = new CylinderOderModel();
                    c.objCylinder = item;

                    if (item.POQuantity == null)
                        c.objCylinder.POQuantity = 0;

                    if (a)
                    {
                        TblPurchaseOrderCylinder p = PurchaseOrderManager.SelectPurchaseOrderByJobId_PurchaseID(item.CylinderID, purID);
                        if (p != null)
                        {
                            c.UnitPriceExtension = p.UnitPrice;
                            c.Quantity = p.Quantity != null ? (int)p.Quantity : 1;
                        }
                        else
                        {
                            c.UnitPriceExtension = item.POUnitPrice ?? 0;
                        }
                    }
                    else
                    {
                        c.Quantity = item.Quantity != null ? (int)item.Quantity : 1;
                        c.UnitPriceExtension = item.POUnitPrice ?? 0;
                    }

                    TblCustomerQuotationDetail cqObj = CustomerQuotationManager.SelectDetailByID(c.objCylinder.PricingID);

                    c.Total = cqObj != null ? ( (int)c.Quantity * c.UnitPriceExtension * (decimal)(cqObj.UnitOfMeasure == UnitOfMeasure.cm2.ToString() ? c.objCylinder.Circumference * c.objCylinder.FaceWidth / 100 : (double)1)) : 0; //;
                    TblJobSheet jSheet = JobManager.SelectJobSheetByID(int.Parse(ddlRevNumber.SelectedValue));

                    c.CylinderType = jSheet.TypeOfCylinder;

                    listCylinderModel.Add(c);
                }

                gvClinders.DataSource = listCylinderModel;
                gvClinders.DataBind();
                Session[ViewState["PageID"] + "dtSource"] = listCylinderModel;
                #endregion

                txtTotalCylinder.Text = cylinders.Count().ToString();
            }
            else
            {
                pnRecord.Visible = true;
                pnListCylinder.Visible = false;
                txtTotalCylinder.Text = "0";
                Session[ViewState["PageID"] + "dtSource"] = null;
            }
        }

        private void SaveData()
        {
            try
            {
                int cusID = 0, revNumber = 0, supID = 0;
                short CurrencyID = 0;

                bool isNew = false;
                if (string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    isNew = true;
                }

                if (isNew)
                {

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
                }
                if (string.IsNullOrEmpty(txtOrderDate.Text))
                {
                    AddErrorPrompt(txtOrderDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }

                if (string.IsNullOrEmpty(txtBaseDeliveryDate.Text))
                {
                    AddErrorPrompt(txtBaseDeliveryDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }

                if (string.IsNullOrEmpty(ddlSuplier.SelectedValue))
                {
                    AddErrorPrompt(ddlSuplier.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
                else
                    int.TryParse(ddlSuplier.SelectedValue, out supID);

                if (string.IsNullOrEmpty(ddlCurrency.SelectedValue))
                {
                    AddErrorPrompt(ddlCurrency.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
                else
                    short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);

                if (string.IsNullOrEmpty(txtContactName.Text))
                {
                    AddErrorPrompt(txtContactName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }

                if (string.IsNullOrEmpty(txtContactPhone.Text))
                {
                    AddErrorPrompt(txtContactPhone.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }

                if (string.IsNullOrEmpty(txtContactFax.Text))
                {
                    AddErrorPrompt(txtContactFax.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }

                if (!IsValid)
                {
                    ShowErrorPromptExtension();
                    return;
                }


                if (isNew)
                {
                    if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }

                    #region validation

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

                    if (string.IsNullOrEmpty(ddlJobNumber.SelectedValue))
                    {
                        AddErrorPrompt(ddlJobNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                    }

                    if (string.IsNullOrEmpty(ddlRevNumber.SelectedValue))
                    {
                        AddErrorPrompt(ddlRevNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                    }
                    else
                        int.TryParse(ddlRevNumber.SelectedValue, out revNumber);


                    if (!IsValid)
                    {
                        ShowErrorPromptExtension();
                        return;
                    }

                    #endregion



                    if (Session[ViewState["PageID"] + "dtSource"] == null || ((List<CylinderOderModel>)Session[ViewState["PageID"] + "dtSource"]).Count() == 0)
                    {
                        MessageBox _msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NO_CYLINDER_IN_JOB), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(_msgRole, null, false, false);
                        return;
                    }

                    List<CylinderOderModel> listCylinderModel = (List<CylinderOderModel>)Session[ViewState["PageID"] + "dtSource"];

                    TblPurchaseOrder purOrder = PurchaseOrderManager.SelectPurchaseOrderByOrderNumber(lblOrderNumber.Text);
                    if (purOrder == null)
                    {
                        purOrder = new TblPurchaseOrder();
                        purOrder.SupplierID = int.Parse(ddlSuplier.SelectedValue);
                        purOrder.OrderDate = DateTime.ParseExact(txtOrderDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        purOrder.OrderNumber = lblOrderNumber.Text;
                        purOrder.RequiredDeliveryDate = DateTime.ParseExact(txtBaseDeliveryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        purOrder.CylinderType = listCylinderModel.First().CylinderType;
                        purOrder.Remark = txtRemark.Text;
                        purOrder.TotalNumberOfCylinders = int.Parse(txtTotalCylinder.Text);
                        purOrder.IsUrgent = Convert.ToByte(chkIsUrgent.Checked);
                        purOrder.JobID = int.Parse(ddlRevNumber.SelectedValue);
                        purOrder.CurrencyID = CurrencyID;
                        purOrder.ContactName = txtContactName.Text.Trim();
                        purOrder.ContactPhone = txtContactPhone.Text.Trim();
                        purOrder.ContactEmail = txtContactFax.Text.Trim();

                        TblPurchaseOrder _purOrder = PurchaseOrderManager.InsertPurchaseOrder(purOrder);

                        if (_purOrder != null)
                        {

                            foreach (var item in listCylinderModel)
                            {
                                TblPurchaseOrderCylinder pc = new TblPurchaseOrderCylinder();
                                pc.CylinderNo = item.objCylinder.SteelBase == 1 ? CylinderManager.CreateCylinderNumber() : string.Empty;//Nếu SteelBase = 1 thì mới sinh number
                                pc.CylinderID = item.objCylinder.CylinderID;
                                pc.Circumference = item.objCylinder.Circumference;
                                pc.FaceWidth = item.objCylinder.FaceWidth;
                                pc.JobID = item.objCylinder.JobID;
                                TblCustomerQuotationDetail cqd = CustomerQuotationManager.SelectDetailByID(item.objCylinder.PricingID);
                                pc.Unit = cqd != null ? cqd.UnitOfMeasure : string.Empty;
                                pc.PurchaseOrderID = _purOrder.PurchaseOrderID;
                                pc.UnitPrice = item.UnitPriceExtension;
                                pc.Quantity = item.Quantity;
                                PurchaseOrderManager.InsertPurchase_CylinderOrder(pc);

                                TblCylinder cObj = CylinderManager.SelectByID(item.objCylinder.CylinderID);
                                if (cObj != null)
                                {
                                    if (cObj.SteelBase == 1)
                                    {
                                        cObj.CylinderNo = pc.CylinderNo;
                                        CylinderManager.Update(cObj);
                                    }
                                }
                            }

                            TblJob jObj = JobManager.SelectByID(_purOrder.JobID);
                            if (jObj != null)
                            {
                                BindCylinders(jObj.JobID, Convert.ToBoolean(jObj.IsOutsource));
                            }
                        }
                        txtName.ReadOnly = true;
                        //txtCode.ReadOnly = true;
                        ddlJobNumber.Enabled = false;
                        ddlRevNumber.Enabled = false;
                        ViewState["ID"] = _purOrder.PurchaseOrderID.ToString();
                        PurorderID = ViewState["ID"].ToString();

                        MessageBox _msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_IS_SAVED), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(_msgRole, null, false, false);
                    }
                    else
                    {
                        ModalConfirmResult result = new ModalConfirmResult();
                        result.Value = "RECREATE_PURCHASE_ORDER";
                        CurrentConfirmResult = result;

                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), string.Format(ResourceTextManager.GetApplicationText(ResourceText.THIS_JOB_ALREADY_PURCHASE_ORDER), lblOrderNumber.Text, CreateOrderbNumber()), MSGButton.YesNo, MSGIcon.Info);
                        OpenMessageBox(msgRole, result, false, false);


                    }
                }
                if (!isNew)
                {
                    UpdateData();
                }
                btnDelete.Visible = true;

                ltrPrint.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintPurchaseOrder.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", ViewState["ID"].ToString());
                upnPrinting.Update();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void UpdateData()
        {
            if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            //UPDATE
            string purID = string.Empty;
            TblPurchaseOrder purOrder = new TblPurchaseOrder();
            if (Request.QueryString["ID"]!=null)
            {
                purID = Request.QueryString["ID"];
                purOrder = PurchaseOrderManager.SelectPurchaseOrder(purID);
            }
            else
            {
                purOrder = PurchaseOrderManager.SelectPurchaseOrderByOrderNumber(lblOrderNumber.Text);
            }

            if (purOrder == null)
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.PURCHASE_ORDER_DELETED), MSGButton.OK, MSGIcon.Warning);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            int _supplier = -1;
            int.TryParse(ddlSuplier.SelectedValue,out _supplier);
            purOrder.SupplierID = _supplier;
            purOrder.OrderDate = DateTime.ParseExact(txtOrderDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            //purOrder.OrderNumber = lblOrderNumber.Text;
            purOrder.RequiredDeliveryDate = DateTime.ParseExact(txtBaseDeliveryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            //purOrder.CylinderType = listCylinderModel.First().CylinderType;
            //purOrder.Remark = txtRemark.Text;
            //purOrder.TotalNumberOfCylinders = int.Parse(txtTotalCylinder.Text);
            purOrder.IsUrgent = Convert.ToByte(chkIsUrgent.Checked);
            //purOrder.JobID = int.Parse(ddlRevNumber.SelectedValue);
            purOrder.Remark = txtRemark.Text;

            short CurrencyID = 0;
            short.TryParse(ddlCurrency.SelectedValue, out CurrencyID);
            purOrder.CurrencyID = CurrencyID;
            purOrder.ContactName = txtContactName.Text.Trim();
            purOrder.ContactPhone = txtContactPhone.Text.Trim();
            purOrder.ContactEmail = txtContactFax.Text.Trim();

            List<CylinderOderModel> listCylinderModel = (List<CylinderOderModel>)Session[ViewState["PageID"] + "dtSource"];

            if (PurchaseOrderManager.UpdatePurchaseOrder(purOrder) != null)
            {
                foreach (var item in listCylinderModel)
                {
                    TblPurchaseOrderCylinder pc = PurchaseOrderManager.SelectPurchaseOrderByJobId_PurchaseID(item.objCylinder.CylinderID, purOrder.PurchaseOrderID);
                    if (pc != null)
                    {
                        pc.UnitPrice = item.UnitPriceExtension;
                        pc.Quantity = item.Quantity;
                        PurchaseOrderManager.UpdatePurchase_Cylinder(pc);
                    }
                    else
                    {
                        pc = new TblPurchaseOrderCylinder();
                        pc.CylinderNo = item.objCylinder.SteelBase == 1 ? CylinderManager.CreateCylinderNumber() : string.Empty;//Nếu SteelBase = 1 thì mới sinh number
                        pc.CylinderID = item.objCylinder.CylinderID;
                        pc.Circumference = item.objCylinder.Circumference;
                        pc.FaceWidth = item.objCylinder.FaceWidth;
                        pc.JobID = item.objCylinder.JobID;
                        TblCustomerQuotationDetail cqd = CustomerQuotationManager.SelectDetailByID(item.objCylinder.PricingID);
                        pc.Unit = cqd != null ? cqd.UnitOfMeasure : string.Empty;
                        pc.PurchaseOrderID = purOrder.PurchaseOrderID;
                        pc.UnitPrice = item.UnitPriceExtension;
                        pc.Quantity = item.Quantity;
                        PurchaseOrderManager.InsertPurchase_CylinderOrder(pc);

                        TblCylinder cObj = CylinderManager.SelectByID(item.objCylinder.CylinderID);
                        if (cObj != null)
                        {
                            if (cObj.SteelBase == 1)
                            {
                                cObj.CylinderNo = pc.CylinderNo;
                                CylinderManager.Update(cObj);
                            }
                        }
                    }
                    TblJob jObj = JobManager.SelectByID(purOrder.JobID);
                    if (jObj != null)
                    {
                        BindCylinders(jObj.JobID, Convert.ToBoolean(jObj.IsOutsource));
                    }
                }
            }
            MessageBox _msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_IS_SAVED), MSGButton.OK, MSGIcon.Success);
            OpenMessageBox(_msgRole, null, false, false);
            ViewState["ID"] = purOrder.PurchaseOrderID;
        }

        protected void ddlSuplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            short SupplierID = Convert.ToInt16(ddlSuplier.SelectedValue);
            TblSupplier obj = new SupplierManager().SelectByID(SupplierID);
            if(obj != null)
            {
                txtContactName.Text = obj.ContactPerson;
                txtContactPhone.Text = obj.Tel;
                txtContactFax.Text = obj.Fax;
            }
        }


    }

    public class CylinderOderModel
    {
        public TblCylinder objCylinder { get; set; }
        public decimal UnitPriceExtension { get; set; }
        public decimal Total { get; set; }
        public string CylinderType { get; set; }
        public int Quantity{ get; set; }
        public string Unit { set; get; }
    }
}