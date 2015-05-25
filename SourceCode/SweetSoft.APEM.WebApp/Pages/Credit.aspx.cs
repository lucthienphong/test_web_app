using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
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
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class Credit : ModalBasePage
    {
        #region Credit
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "credit_manager";
            }
        }

        private int CreditID
        {
            get
            {
                int ID = 0;
                if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"].ToString(), out ID);
                else if(Session[ViewState["PageID"] + "ID"] != null)
                    int.TryParse(Request.QueryString["ID"].ToString(), out ID);
                return ID;
            }
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
                    BindCreditData();
                    ltrPrinting.Text = string.Format("<a id='printing' href='javascript:;' data-href='Printing/PrintCreditDetail.aspx?ID={0}' class='btn btn-transparent'><span class='flaticon-printer60'></span> Print</a>", CreditID);
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
            grvDetail.Columns[0].HeaderText = "Description";
            grvDetail.Columns[1].HeaderText = "Job Order No.";
            grvDetail.Columns[2].HeaderText = "Qty Pcs.";
            grvDetail.Columns[3].HeaderText = "Unit price";
            grvDetail.Columns[4].HeaderText = "Total";
        }

        private void BindCurrencyDDL()
        {
            TblCurrencyCollection list = new CurrencyManager().SelectAllForDDL();
            ddlCurrency.DataSource = list;
            ddlCurrency.DataValueField = "CurrencyID";
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataBind();
        }

        private void BindDDL()
        {
            BindCurrencyDDL();
        }

        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        private void BindCreditData()
        {
            try
            {
                if (CreditID != 0)
                {
                    //Kiểm tra nhân viên còn tồn tại không
                    TblCredit obj = CreditManager.SelectByID(CreditID);
                    if (obj != null)
                    {
                        //Bind Credit
                        lblCreditNumber.Text = obj.CreditNo;

                        hCustomerID.Value = obj.CustomerID.ToString();
                        TblCustomer cObj = CustomerManager.SelectByID(obj.CustomerID);
                        txtCode.Text = cObj.Code;
                        txtName.Text = cObj.Name;

                        txtCreditDate.Text = obj.CreditDate.ToString("dd/MM/yyyy");
                        ddlCurrency.SelectedValue = obj.CurrencyID.ToString();
                        txtRemark.Text = obj.Remark;

                        BindDetailData(CreditID);
                        BindGrid();
                        Session[ViewState["PageID"] + "ID"] = obj.CreditID;
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
            lblCreditNumber.Text = CreditManager.CreateCreditNumber();
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            hCustomerID.Value = string.Empty;
            txtCreditDate.Text = DateTime.Today.ToString("dd/MM/yyyy");//string.Empty;

            txtRemark.Text = string.Empty;
            ddlCurrency.SelectedIndex = 0;

            BindDetailData(0);
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Gọi message box
            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "Credit_Delete";
            CurrentConfirmResult = result;
            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Are you sure want to delete this credit?", MSGButton.YesNo, MSGIcon.Warning);
            OpenMessageBox(msg, result, false, false);
        }

        private void SaveData()
        {
            #region Validation
            int CustomerID = 0;
            short CurrencyID = 0;
            DateTime CreditDate = new DateTime();
            int.TryParse(hCustomerID.Value, out CustomerID);
            //Customer
            if (CustomerID == 0 || string.IsNullOrEmpty(txtCode.Text.Trim()) || string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //CreditDate
            if (string.IsNullOrEmpty(txtCreditDate.Text.Trim()))
                AddErrorPrompt(txtCreditDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            else if (!DateTime.TryParseExact(txtCreditDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out CreditDate))
                AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
            //Currency
            if (ddlCurrency.SelectedValue == "0")
                AddErrorPrompt(ddlCurrency.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            else
                CurrencyID = Convert.ToInt16(ddlCurrency.SelectedValue);

            if (!IsValid)
            {
                ShowErrorPromptExtension();
                return;
            }
            #endregion

            #region SaveData
            try
            {
                TblCredit obj = CreditManager.SelectByID(CreditID);
                if (obj != null)//Update
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }

                    obj.CustomerID = CustomerID;
                    obj.CreditDate = CreditDate;
                    obj.CurrencyID = CurrencyID;
                    obj.Remark = txtRemark.Text.Trim();
                    obj.Total = CalculateTotal();

                    CreditManager.Update(obj);
                    SaveDetailData(obj.CreditID);
                    //Lưu vào logging
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());

                    LoggingActions("Credit",
                            LogsAction.Objects.Action.UPDATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Credit Code", Data = obj.CreditNo } ,
                            }));

                    BindDetailData(obj.CreditID);

                    Session[ViewState["PageID"] + "ID"] = obj.CreditID;
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
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

                    obj = new TblCredit();
                    obj.CreditNo = lblCreditNumber.Text.Trim();
                    obj.CustomerID = CustomerID;
                    obj.CreditDate = CreditDate;
                    obj.CurrencyID = CurrencyID;
                    obj.Remark = txtRemark.Text.Trim();
                    obj.Total = CalculateTotal();

                    bool CreditNoChanged = false;
                    string oldNumber = lblCreditNumber.Text.Trim();

                    while (CreditManager.CreditNumberExists(obj.CreditID, obj.CreditNo))
                    {
                        lblCreditNumber.Text = CreditManager.CreateCreditNumber();
                        obj.CreditNo = lblCreditNumber.Text.Trim();
                        CreditNoChanged = true;
                    }

                    CreditManager.Insert(obj);
                    SaveDetailData(obj.CreditID);
                    //Lưu vào logging
                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());

                    LoggingActions("Credit",
                            LogsAction.Objects.Action.UPDATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Credit Code", Data = obj.CreditNo }
                            }));

                    BindDetailData(obj.CreditID);

                    Session[ViewState["PageID"] + "ID"] = obj.CreditID;

                    string message = ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS);
                    if (CreditNoChanged)
                        message += string.Format("<br/>System has created a new No. for this credit. The No. '{0}' has been used by another credit", oldNumber);

                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
            #endregion
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void SaveDetailData(int CreditID)
        {
            List<TblCreditDetail> oldSource = CreditManager.SellectAllDetail(CreditID);
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();
            //Xóa dữ liệu cũ
            List<TblCreditDetail> deleteSource = oldSource.Where(x => !source.Select(n => n.CreditDetailID).Contains(x.CreditDetailID)).ToList();
            foreach (var item in deleteSource)
                CreditManager.DeleteDetail(item.CreditDetailID);

            //Cập nhật dữu liệu mới
            foreach (TblCreditDetail item in source)
            {
                item.CreditID = CreditID;
                if (item.CreditDetailID < 0)
                    CreditManager.InsertDetail(item);
                else
                    CreditManager.UpdateDetail(item);
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        if (e.Value.ToString().Equals("Credit_Delete"))
                        {
                            CreditManager.Delete(CreditID);
                            LoggingActions("Credit",
                                            LogsAction.Objects.Action.DELETE,
                                            LogsAction.Objects.Status.SUCCESS,
                                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                                new JsonData() { Title = "Credit Code", Data = CreditManager.SelectByID(CreditID).CreditNo }
                                            }));
                            Response.Redirect("~/Pages/CreditList.aspx", false);
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

        #region CreditDetail
        //Lấy dữ liệu chi tiết
        private void BindDetailData(int CreditID)
        {
            List<TblCreditDetail> source = CreditManager.SellectAllDetail(CreditID);
            if (source == null)
                source = new List<TblCreditDetail>();
            Session[ViewState["PageID"] + "Source"] = source;

        }

        //Bind dữ liệu vào lưới
        private void BindGrid()
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();
            grvDetail.DataSource = source;
            grvDetail.DataBind();
        }


        private void AddNewRow()
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();
            
            TblCreditDetail obj = new TblCreditDetail();
            Random rnd = new Random();
            int RandID = rnd.Next(-1000, 0);
            while(source.Where(x => x.CreditDetailID == RandID).Count() > 0)
                RandID = rnd.Next(-1000, 0);
            obj.CreditDetailID = RandID;                
            obj.Quantity = 1;
            source.Insert(0, obj);
            Session[ViewState["PageID"] + "Source"] = source;
        }

        private void UpdateRow(int Index, int CreditDetailID, string JobOrderNo, string Description, int Quantity, decimal UnitPrice)
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();

            TblCreditDetail obj = source.Where(x => x.CreditDetailID == CreditDetailID).FirstOrDefault();
            if (obj != null)
            {
                obj.JobOrderNo = JobOrderNo;
                obj.Description = Description;
                obj.Quantity = Quantity;
                obj.UnitPrice = UnitPrice;
            }

            Session[ViewState["PageID"] + "Source"] = source;
        }

        private void RemoveInvalidRows()
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();

            source = source.Where(x => !string.IsNullOrEmpty(x.JobOrderNo)).ToList();
            Session[ViewState["PageID"] + "Source"] = source;
        }

        private void RemoveSelectedRows(List<int> IDs)
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();

            source = source.Where(x => !IDs.Contains(x.CreditDetailID)).ToList();
            Session[ViewState["PageID"] + "Source"] = source;
        }

        private decimal CalculateTotal()
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();

            return source.Sum(x => x.Quantity * x.UnitPrice);
        }

        protected void grvDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grvDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grvDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RemoveInvalidRows();
            grvDetail.EditIndex = e.NewEditIndex;
            BindGrid();

            btnAddDetail.Visible = false;
            btnDeleteDetail.Visible = false;
        }

        protected void grvDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvDetail.DataKeys[e.RowIndex].Value);
            CustomExtraTextbox txtDescription = grvDetail.Rows[e.RowIndex].FindControl("txtDescription") as CustomExtraTextbox;
            CustomExtraTextbox txtJobOrderNo = grvDetail.Rows[e.RowIndex].FindControl("txtJobOrderNo") as CustomExtraTextbox;
            ExtraInputMask txtQuantity = grvDetail.Rows[e.RowIndex].FindControl("txtQuantity") as ExtraInputMask;
            ExtraInputMask txtUnitPrice = grvDetail.Rows[e.RowIndex].FindControl("txtUnitPrice") as ExtraInputMask;

            string JobOrderNo = string.Empty, Description = string.Empty;
            int Quantity = 0;
            decimal UnitPrice = 0;
            //Job Order No
            if (string.IsNullOrEmpty(txtJobOrderNo.Text))
            {
                AddErrorPrompt(txtJobOrderNo.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            //Quantity
            if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else
            {
                if (int.TryParse(txtQuantity.Text.Trim().Replace(",", ""), out Quantity))
                {
                    if(Quantity <= 0)
                        AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.MIN_VALIDATION));
                }
                else
                    AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
            }
            //Unit Price
            if (string.IsNullOrEmpty(txtUnitPrice.Text))
            {
                AddErrorPrompt(txtUnitPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else
            {
                if (decimal.TryParse(txtUnitPrice.Text.Trim().Replace(",", ""), out UnitPrice))
                {
                    if (Quantity <= 0)
                        AddErrorPrompt(txtUnitPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.MIN_VALIDATION));
                }
                else
                    AddErrorPrompt(txtUnitPrice.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
            }

            if (IsValid)
            {
                JobOrderNo = txtJobOrderNo.Text.Trim();
                Description = txtDescription.Text.Trim();

                UpdateRow(e.RowIndex, ID, JobOrderNo, Description, Quantity, UnitPrice);
                grvDetail.EditIndex = -1;
                BindGrid();

                btnAddDetail.Visible = true;
                btnDeleteDetail.Visible = true;
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        protected void grvDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvDetail.EditIndex = -1;
            RemoveInvalidRows();
            BindGrid();

            btnAddDetail.Visible = true;
            btnDeleteDetail.Visible = true;
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();
            RemoveInvalidRows();
            AddNewRow();
            grvDetail.EditIndex = 0;
            //BindContractData(ID, true);
            BindGrid();

            btnAddDetail.Visible = false;
            btnDeleteDetail.Visible = false;
        }

        protected void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            List<TblCreditDetail> source = (List<TblCreditDetail>)Session[ViewState["PageID"] + "Source"];
            if (source == null)
                source = new List<TblCreditDetail>();
            List<int> idList = new List<int>();
            for (int i = 0; i < grvDetail.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvDetail.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)//Nếu tồn tại dòng dữ liệu được check thì hiện thông báo xóa
                {
                    int ID = 0;
                    if (int.TryParse(grvDetail.DataKeys[i].Values[0].ToString(), out ID))
                    {
                        idList.Add(ID);
                    }
                }
            }

            if (idList.Count() > 0)
            {
                RemoveSelectedRows(idList);
                BindGrid();
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.SELECT_DATA_TO_DELETE), MSGButton.OK, MSGIcon.Info);
                OpenMessageBox(msg, null, false, false);
            }
        }

        #endregion
    }
}