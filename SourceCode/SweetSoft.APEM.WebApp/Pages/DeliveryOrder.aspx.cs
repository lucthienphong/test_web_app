using SubSonic;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using SweetSoftCMS.ExtraControls.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class DeliveryOrder : ModalBasePage
    {
        private int job_OrderConfirmation;

        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "delivery_order_manager";
            }
        }

        public int JobID
        {
            get
            {
                int ID = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"].ToString(), out ID);
                }
                else if (Session[ViewState["PageID"] + "ID"] != null)
                {
                    int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID);
                }
                return ID;
            }
            set { job_OrderConfirmation = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ViewState["PackingDimesionSource"] = (new Random()).Next().ToString();
                BindDDL();
                BindDOData();
                if (JobID > 0)
                {
                    base.SaveBaseDataBeforeEdit();
                }
            }
        }

        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        private void BindDDL()
        {
            BindPackingForDDL();
        }

        private void BindPackingForDDL()
        {
            List<TblReference> list = ReferenceTableManager.SelectPackingForDDL();
            ddlPacking.DataSource = list;
            ddlPacking.DataTextField = "Name";
            ddlPacking.DataValueField = "ReferencesID";
            ddlPacking.DataBind();
        }

        private void BindDOData()
        {
            try
            {
                if (JobID != 0)
                {
                    TblOrderConfirmation od = OrderConfirmationManager.SelectByID(JobID);
                    if (od != null)
                    {

                        ddlJob.Items.Clear();
                        ddlJob.Items.Add(new ListItem(od.TblJob.JobNumber, od.JobID.ToString()));
                        ddlJob.Enabled = false;
                        ddlJob_SelectedIndexChanged(null, null);
                        btnDelete.Visible = true;
                        int jobID = od.JobID;
                        txtName.Enabled = false;
                        //txtCode.Enabled= false;
                        pnRecord.Visible = false;
                        pnListCylinder.Visible = true;

                        TblDeliveryOrder d = DeliveryOrderManager.SelectDeliveryOrderByJobID(jobID);
                        if (d != null)
                        {
                            txtOrderDate.Text = d.OrderDate.ToString("dd/MM/yyyy");
                            lblOrderNumber.Text = d.DONumber;
                            txtRemark.Text = d.OtherItem;
                            txtPO1.Text = d.CustomerPO1;
                            txtPO2.Text = d.CustomerPO2;
                            txtGrossWeight.Text = d.GrossWeigth;
                            txtNetWeight.Text = d.NetWeight;
                            ddlPacking.SelectedValue = d.PackingID.ToString();
                        }

                        TblJob job = JobManager.SelectByID(jobID);
                        if (job != null)
                        {
                            txtJobName.Text = job.JobName;
                            txtDesign.Text = job.Design;
                            TblCustomer c = CustomerManager.SelectByID(job.CustomerID);
                            txtName.Text = c.Name;
                            txtCode.Text = c.Code;
                            hCustomerID.Value = c.CustomerID.ToString();
                        }

                        if (d != null && job != null)
                        {
                            LoadContactPerson(job.CustomerID.ToString());
                            ddlContact.SelectedValue = d.ContactPersonID.ToString();
                        }

                        ltrPrinting.Text = string.Format(@"<div class='btn-group'>
                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                            <span class='flaticon-printer60'></span> Print <span class='caret'></span>
                        </button>
                        <ul class='dropdown-menu openPrinting' role='menu'>
                            <li>
                                <a href='#' data-href='Printing/PrintDeliveryOrderPDF.aspx?ID={0}'>Delivery order</a>
                            </li>
                            <li>
                                <a href='#' data-href='Printing/PrintDeliveryOrderNotePDF.aspx?ID={0}'>Delivery note</a>
                            </li>
                        </ul>
                        </div>", job.JobID);

                        BindCylinders(jobID);
                        BindPackingDimetion(jobID);
                        BindPackingDimetionGrid();

                        upnlCylinder.Update();

                        /// Trunglc Add - 23-04-2015
                        /// 
                        bool IsLocking = DeliveryOrderManager.IsDeliveryOrderLocking(JobID);
                        bool IsNewDeliveryOrder = DeliveryOrderManager.IsNewDeliveryOrder(JobID);
                        bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
                        bool IsAllowEdit = IsNewDeliveryOrder ? true : (!IsLocking ? true : (IsLocking ? false : true));

                        AllowEditting(IsAllowEdit);
                    }
                    else
                    {
                        pnRecord.Visible = true;
                        pnListCylinder.Visible = false;
                    }
                }
                else
                {
                    lblOrderNumber.Text = DeliveryOrderManager.CreateDONumber();
                    txtOrderDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    LoadJobByCustomer(0);
                    LoadContactPerson("0");
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void BindCylinders(int JobID)
        {
            //decimal TotalPrice = 0;
            DataTable dt = CylinderManager.SelectCylinderSelectForDeliveryOrder(JobID);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                //decimal.TryParse(dt.Rows[0]["Total"].ToString(), out TotalPrice);
                pnListCylinder.Visible = true;
                pnRecord.Visible = false;

                gvClinders.DataSource = dt;
                gvClinders.DataBind();
            }
            else
            {
                pnListCylinder.Visible = false;
                pnRecord.Visible = true;
            }
        }



        private void SaveData()
        {
            try
            {
                int cusID = 0, supID = 0;

                #region VALIDATION
                if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }

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

                if (string.IsNullOrEmpty(txtOrderDate.Text))
                {
                    AddErrorPrompt(txtOrderDate.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }

                if (string.IsNullOrEmpty(ddlJob.SelectedValue))
                {
                    AddErrorPrompt(ddlJob.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }

                if (string.IsNullOrEmpty(ddlContact.SelectedValue))
                {
                    AddErrorPrompt(ddlContact.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
                else
                    int.TryParse(ddlContact.SelectedValue, out supID);

                if(ddlJob.SelectedValue == "0")
                    AddErrorPrompt(ddlJob.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));

                if (!IsValid)
                {
                    ShowErrorPromptExtension();
                    return;
                }
                #endregion VALIDATION

                if (!IsValid)
                {
                    ShowErrorPromptExtension();
                    return;
                }

                TblDeliveryOrder devO = DeliveryOrderManager.SelectDeliveryOrderByJobID(int.Parse(ddlJob.SelectedValue));//Lấy thông tin DO theo JobID
                if (devO != null)//Cập nhật DO
                {
                    devO.CustomerPO1 = txtPO1.Text.Trim();
                    devO.CustomerPO2 = txtPO2.Text.Trim();

                    devO.ContactPersonID = int.Parse(ddlContact.SelectedValue);
                    DateTime orderDate = DateTime.ParseExact(txtOrderDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    devO.OrderDate = orderDate;
                    devO.OtherItem = txtRemark.Text;
                    devO.GrossWeigth = txtGrossWeight.Text;
                    devO.NetWeight = txtNetWeight.Text;

                    int PackingID = 0; int.TryParse(ddlPacking.SelectedValue, out PackingID);
                    devO.PackingID = PackingID == 0 ? (int?)null : PackingID;//int.Parse(ddlPacking.SelectedValue);
                    if (DeliveryOrderManager.UpdateDeliveryOrder(devO) != null)
                    {
                        SavePackingDimension(devO.JobID);
                        SaveCustomerPO(devO.JobID);

                        ////Trunglc Add - 23-04-2015
                        ////Update Status Lock Of Job
                        ////DeliveryOrderManager.LockJobAndOC(devO.JobID);
                        ////End

                        LoggingActions("Delivery Order",
                            LogsAction.Objects.Action.UPDATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Delivery Number", Data = devO.DONumber } ,
                                new JsonData() { Title = "Job Number", Data = devO.TblJob.JobNumber + "(Rev " + devO.TblJob.RevNumber.ToString() + ")" } 
                            }));

                        Session[ViewState["PageID"] + "ID"] = devO.JobID;
                        MessageBox _msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_IS_SAVED), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(_msgRole, null, false, false);
                    }
                    ltrPrinting.Text = string.Format(@"<div class='btn-group'>
                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                            <span class='flaticon-printer60'></span> Print <span class='caret'></span>
                        </button>
                        <ul class='dropdown-menu openPrinting' role='menu'>
                            <li>
                                <a href='#' data-href='Printing/PrintDeliveryOrderPDF.aspx?ID={0}'>Delivery order</a>
                            </li>
                            <li>
                                <a href='#' data-href='Printing/PrintDeliveryOrderNotePDF.aspx?ID={0}'>Delivery note</a>
                            </li>
                        </ul>
                    </div>", devO.JobID);
                    upnlPrinting.Update();
                }
                else//Thêm DO
                {
                    devO = new TblDeliveryOrder();
                    devO.JobID = int.Parse(ddlJob.SelectedValue);
                    devO.DONumber = DeliveryOrderManager.CreateDONumber();

                    devO.CustomerPO1 = txtPO1.Text.Trim();
                    devO.CustomerPO2 = txtPO2.Text.Trim();

                    devO.ContactPersonID = int.Parse(ddlContact.SelectedValue);
                    DateTime orderDate = DateTime.ParseExact(txtOrderDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    devO.OrderDate = orderDate;
                    devO.OtherItem = txtRemark.Text;
                    devO.GrossWeigth = txtGrossWeight.Text;
                    devO.NetWeight = txtNetWeight.Text;
                    int PackingID = 0; int.TryParse(ddlPacking.SelectedValue, out PackingID);
                    devO.PackingID = PackingID == 0 ? (int?)null : PackingID;//int.Parse(ddlPacking.SelectedValue);

                    // Trunglc Add - 11-05-2015

                    TblJob objJob = JobManager.SelectByID(int.Parse(ddlJob.SelectedValue));
                    if (objJob != null)
                    {
                        objJob.Status = Enum.GetName(typeof(JobStatus), JobStatus.Delivered);
                        JobManager.Update(objJob);
                    }

                    // End

                    if (DeliveryOrderManager.InsertDeliveryOrder(devO) != null)
                    {
                        SavePackingDimension(devO.JobID);
                        SaveCustomerPO(devO.JobID);

                        ////Trunglc Add - 23-04-2015
                        ////Update Status Lock Of Job
                        ////DeliveryOrderManager.LockJobAndOC(devO.JobID);
                        ////End

                        LoggingActions("Delivery Order",
                            LogsAction.Objects.Action.CREATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Delivery Number", Data = devO.DONumber } ,
                                new JsonData() { Title = "Job Number", Data = devO.TblJob.JobNumber + "(Rev " + devO.TblJob.RevNumber.ToString() + ")" } 
                            }));

                        Session[ViewState["PageID"] + "ID"] = devO.JobID;
                        MessageBox _msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_IS_SAVED), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(_msgRole, null, false, false);
                    }
                    ltrPrinting.Text = string.Format(@"<div class='btn-group'>
                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                            <span class='flaticon-printer60'></span> Print <span class='caret'></span>
                        </button>
                        <ul class='dropdown-menu openPrinting' role='menu'>
                            <li>
                                <a href='#' data-href='Printing/PrintDeliveryOrderPDF.aspx?ID={0}'>Delivery order</a>
                            </li>
                            <li>
                                <a href='#' data-href='Printing/PrintDeliveryOrderNotePDF.aspx?ID={0}'>Delivery note</a>
                            </li>
                        </ul>
                    </div>", devO.JobID);
                    upnlPrinting.Update();
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        /// <summary>
        /// Update Customer PO Number to Job and OC once DO was created/edited
        /// </summary>
        /// <param name="JobID"></param>
        private void SaveCustomerPO(int JobID)
        {
            TblOrderConfirmation oc = OrderConfirmationManager.SelectByID(JobID);
            if (oc != null)
            {
                oc.CustomerPO1 = txtPO1.Text.Trim();
                oc.CustomerPO2 = txtPO2.Text.Trim();
                OrderConfirmationManager.Update(oc);
            }

            TblJob job = JobManager.SelectByID(JobID);
            if (job != null)
            {
                job.CustomerPO1 = txtPO1.Text.Trim();
                job.CustomerPO2 = txtPO2.Text.Trim();
                JobManager.Update(job);
            }
        }

        private void SavePackingDimension(int JobID)
        {
            List<DeliveryOrderPackingDimensionExtension> oldSource = DeliveryOrderManager.SelectPackingDimensionDetail(JobID);
            List<DeliveryOrderPackingDimensionExtension> Source = (List<DeliveryOrderPackingDimensionExtension>)Session[ViewState["PageID"] + "PackingDimesion"];
            if (Source == null)
                Source = new List<DeliveryOrderPackingDimensionExtension>();
            List<DeliveryOrderPackingDimensionExtension> deletedSource = oldSource.Where(o => !Source.Select(x => x.PackingDimensionID).Contains(o.PackingDimensionID)).ToList();

            //Xóa dữ liệu cũ
            foreach (DeliveryOrderPackingDimensionExtension item in deletedSource)
                DeliveryOrderManager.DeletePackingDimension(JobID, item.PackingDimensionID);
            //Thêm dữ liệu mới
            foreach (DeliveryOrderPackingDimensionExtension item in Source)
            {
                TblDeliveryOrderPackingDimension obj = new TblDeliveryOrderPackingDimension();
                obj = oldSource.Where(x => x.PackingDimensionID == item.PackingDimensionID).FirstOrDefault();
                if (obj != null)
                {
                    obj.Quantity = item.Quantity;
                    DeliveryOrderManager.UpdatePackingDimension(obj);
                }
                else
                {
                    obj = new TblDeliveryOrderPackingDimension();
                    obj.JobID = JobID;
                    obj.PackingDimensionID = item.PackingDimensionID;
                    obj.Quantity = item.Quantity;
                    DeliveryOrderManager.InsertPackingDimension(obj);
                }
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
                        #region Xóa delivery order
                        if (e.Value.ToString().Equals("DELETE_DELIVERY_ORDER"))
                        {
                            TblDeliveryOrder devOder = DeliveryOrderManager.SelectDeliveryOrderByJobID(JobID);
                            if (devOder != null)
                            {
                                if (JobManager.JobHasInvoice(devOder.JobID).Length > 0)//Nếu công việc đã tạo DO thì không cho xóa
                                {
                                    string message = string.Format("There exists a <strong>Invoice</strong> has been created for this Order Confirmation: {0}", JobManager.JobHasInvoice(devOder.JobID));
                                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Error);
                                    OpenMessageBox(msg, null, false, false);
                                    return;
                                }
                                if (DeliveryOrderManager.Delete(devOder.JobID))
                                {
                                    if (AllowSaveLogging)
                                    {
                                        SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.DELETE_ORDER_CONFIRMATION), FUNCTION_PAGE, devOder.ToJSONString());
                                        LoggingActions("Delivery Order",
                                                LogsAction.Objects.Action.DELETE,
                                                LogsAction.Objects.Status.SUCCESS,
                                                JsonConvert.SerializeObject(new List<JsonData>() { 
                                                    new JsonData() { Title = "Delivery Number", Data = devOder.DONumber } ,
                                                    new JsonData() { Title = "Job Number", Data = devOder.TblJob.JobNumber + "(Rev " + devOder.TblJob.RevNumber.ToString() + ")" } 
                                                }));
                                    }
                                    Response.Redirect("DeliveryOrderList.aspx", false);
                                }
                            }
                            else
                            {
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_ORDER_CONFIRMATION), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msg, null, false, false);
                            }
                        }
                        #endregion Xóa delivery order
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void AllowEditting(bool yesno)
        {
            //Job detail
            ddlContact.Enabled = yesno;
            txtPO1.Enabled = yesno;
            txtPO2.Enabled = yesno;
            txtOrderDate.Enabled = yesno;
            txtRemark.Enabled = yesno;
            ddlPacking.Enabled = yesno;
            txtGrossWeight.Enabled = yesno;
            txtNetWeight.Enabled = yesno;

            gvPackingDimension.Columns[gvPackingDimension.Columns.Count - 1].Visible = yesno;

            btnAddPackingDimension.Visible = yesno;
            btnDeletePackingDimension.Visible = yesno;

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
            bool IsNewDeliveryOrder = DeliveryOrderManager.IsNewDeliveryOrder(JobID);
            bool IsDeliveryOrderLocking = DeliveryOrderManager.IsDeliveryOrderLocking(JobID);

            if (IsNewDeliveryOrder)
            {
                btnLock.Visible = IsAllowLock ? true : false;
                btnUnlock.Visible = false;
            }
            else
            {
                btnLock.Visible = IsNewDeliveryOrder && IsAllowLock ? true : ((!IsDeliveryOrderLocking && IsAllowLock && yesno ? true : false));
                btnUnlock.Visible = IsDeliveryOrderLocking && IsAllowUnlock && !yesno ? true : false;
            }

            btnDelete.Visible = IsDeliveryOrderLocking ? false : true;

            ///End
        }

        protected void btnLoadContacts_Click(object sender, EventArgs e)
        {
            int CustomerID = 0;
            int.TryParse(hCustomerID.Value, out CustomerID);

            TblCustomer cust = CustomerManager.SelectByID(CustomerID);

            if (cust != null)
            {
                LoadContactPerson(cust.CustomerID.ToString());
                //Load list job by Customer ID
                LoadJobByCustomer(cust.CustomerID);
                ddlJob_SelectedIndexChanged(null, null);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsNewDeliveryOrder = DeliveryOrderManager.IsNewDeliveryOrder(JobID);
            bool IsLocking = DeliveryOrderManager.IsDeliveryOrderLocking(JobID);

            if (!IsNewDeliveryOrder && !IsLocking)
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool IsNewDeliveryOrder = DeliveryOrderManager.IsNewDeliveryOrder(JobID);
            bool IsLocking = DeliveryOrderManager.IsDeliveryOrderLocking(JobID);

            if (!IsNewDeliveryOrder && !IsLocking)
            {
                if (!RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }
            }

            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "DELETE_DELIVERY_ORDER";
            CurrentConfirmResult = result;
            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.WARNING), ResourceTextManager.GetApplicationText(ResourceText.DO_YOU_REALLY_WAN_TO_DELETE_THIS_PURCHASE), MSGButton.DeleteCancel, MSGIcon.Error);
            OpenMessageBox(msg, result, false, false);
        }

        protected void ddlJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            string JobID = ddlJob.SelectedValue;

            int b = -1;
            bool a = int.TryParse(JobID, out b);

            if (a)
            {
                TblJob j = JobManager.SelectByID(b);
                if (j != null)
                {
                    txtJobName.Text = j.JobName;
                    txtDesign.Text = j.Design;
                    txtRev.Text = j.RevNumber.ToString();
                    txtJobNR.Text = j.JobNumber;
                    ddlContact.SelectedValue = j.ContactPersonID.ToString();

                    TblOrderConfirmation od = OrderConfirmationManager.SelectByID(j.JobID);
                    if (od != null)
                    {
                        txtPO1.Text = od.CustomerPO1;
                        txtPO2.Text = od.CustomerPO2;
                        txtRemark.Text = od.Remark;
                    }
                }
                TblDeliveryOrder d = DeliveryOrderManager.SelectDeliveryOrderByJobID(b);
                if (d != null)
                {
                    txtOrderDate.Text = d.OrderDate.ToString("dd/MM/yyyy");
                    ddlContact.SelectedValue = d.ContactPersonID.ToString();
                }
                BindCylinders(b);
                upnlCylinder.Update();
            }
            else
            {
                pnRecord.Visible = true;
                pnListCylinder.Visible = false;
            }
        }

        #region PACKINGDIMETION
        private void BindPackingDimetion(int JobID)
        {
            List<DeliveryOrderPackingDimensionExtension> source = DeliveryOrderManager.SelectPackingDimensionDetail(JobID);
            if (source == null)
                source = new List<DeliveryOrderPackingDimensionExtension>();
            Session[ViewState["PageID"] + "PackingDimesion"] = source;
        }

        private void BindPackingDimetionGrid()
        {
            List<DeliveryOrderPackingDimensionExtension> source = (List<DeliveryOrderPackingDimensionExtension>)Session[ViewState["PageID"] + "PackingDimesion"];
            if (source == null)
                source = new List<DeliveryOrderPackingDimensionExtension>();
            gvPackingDimension.DataSource = source;
            gvPackingDimension.DataBind();
        }

        private void AddNewRow()
        {
            List<DeliveryOrderPackingDimensionExtension> source = (List<DeliveryOrderPackingDimensionExtension>)Session[ViewState["PageID"] + "PackingDimesion"];
            if (source == null)
                source = new List<DeliveryOrderPackingDimensionExtension>();
            DeliveryOrderPackingDimensionExtension obj = new DeliveryOrderPackingDimensionExtension();
            Random rnd = new Random();
            int rndID = rnd.Next(-1000, -1);
            while(source.Where(x => x.VitualID == rndID).Count() > 0)
                rndID = rnd.Next(-1000, -1);
            obj.VitualID = rndID;
            obj.PackingDimensionID = 0;
            obj.PackingDimensionName = string.Empty;
            obj.Quantity = 0;
            obj.JobID = 0;
            source.Insert(0, obj);
            Session[ViewState["PageID"] + "PackingDimesion"] = source;
        }

        private void RemoveInvalidRows()
        {
            List<DeliveryOrderPackingDimensionExtension> source = new List<DeliveryOrderPackingDimensionExtension>();
            List<DeliveryOrderPackingDimensionExtension> oldSource = (List<DeliveryOrderPackingDimensionExtension>)Session[ViewState["PageID"] + "PackingDimesion"];
            if (oldSource != null)
                source.AddRange(oldSource.Where(x => !string.IsNullOrEmpty(x.PackingDimensionName)).ToList());
            Session[ViewState["PageID"] + "PackingDimesion"] = source;
        }

        private void RemoveSelectedRows(List<int> IDs)
        {
            List<DeliveryOrderPackingDimensionExtension> source = new List<DeliveryOrderPackingDimensionExtension>();
            List<DeliveryOrderPackingDimensionExtension> oldSource = (List<DeliveryOrderPackingDimensionExtension>)Session[ViewState["PageID"] + "PackingDimesion"];
            if(oldSource != null)
            source.AddRange(oldSource.Where(x => !IDs.Contains(x.PackingDimensionID)).ToList());
            Session[ViewState["PageID"] + "PackingDimesion"] = source;
        }

        private void UpdateRow(int RowIndex, int VitualID, int PackingDimesionID, string PackingDimensionName, int Quantity)
        {
            List<DeliveryOrderPackingDimensionExtension> source = (List<DeliveryOrderPackingDimensionExtension>)Session[ViewState["PageID"] + "PackingDimesion"];
            if (source == null)
                source = new List<DeliveryOrderPackingDimensionExtension>();
            DeliveryOrderPackingDimensionExtension obj = source.Where(x => x.VitualID == VitualID).FirstOrDefault();
            if(obj != null)
            {
                obj.PackingDimensionID = PackingDimesionID;
                obj.PackingDimensionName = PackingDimensionName;
                obj.Quantity = Quantity;
            }
            Session[ViewState["PageID"] + "PackingDimesion"] = source;
        }

        protected void btnAddPackingDimension_Click(object sender, EventArgs e)
        {
            RemoveInvalidRows();
            AddNewRow();
            gvPackingDimension.EditIndex = 0;
            BindPackingDimetionGrid();
        }
        #endregion

        protected void btnDeletePackingDimension_Click(object sender, EventArgs e)
        {
            List<int> IDs = new List<int>();
            for (int i = 0; i < gvPackingDimension.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvPackingDimension.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)
                {
                    int ID = 0;
                    if (int.TryParse(gvPackingDimension.DataKeys[i].Values[1].ToString(), out ID))
                    {
                        IDs.Add(ID);
                    }
                }
            }
            RemoveSelectedRows(IDs);
            BindPackingDimetionGrid();
        }

        protected void gvPackingDimension_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPackingDimension.EditIndex = -1;
            RemoveInvalidRows();
            BindPackingDimetionGrid();
            btnAddPackingDimension.Enabled = true;
            btnDeletePackingDimension.Enabled = true;

        }

        protected void gvPackingDimension_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RemoveInvalidRows();
            gvPackingDimension.EditIndex = e.NewEditIndex;
            BindPackingDimetionGrid();

            btnAddPackingDimension.Enabled = false;
            btnDeletePackingDimension.Enabled = false;
        }

        protected void gvPackingDimension_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            List<DeliveryOrderPackingDimensionExtension> source = (List<DeliveryOrderPackingDimensionExtension>)Session[ViewState["PageID"] + "PackingDimesion"];
            if (source == null)
                source = new List<DeliveryOrderPackingDimensionExtension>();
            int VitualID = Convert.ToInt32(gvPackingDimension.DataKeys[e.RowIndex].Values[0]);
            ExtraInputMask txtQuantity = gvPackingDimension.Rows[e.RowIndex].FindControl("txtQuantity") as ExtraInputMask;
            DropDownList ddlPackingDimension = gvPackingDimension.Rows[e.RowIndex].FindControl("ddlPackingDimension") as DropDownList;
            int Quantity = 0, PackingDimensionID = 0;

            if (string.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else if(!int.TryParse(txtQuantity.Text.Trim().Replace(",", ""), out Quantity))
            {
                AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE));
            }
            else if (Quantity < 0)
            {
                AddErrorPrompt(txtQuantity.ClientID, ResourceTextManager.GetApplicationText(ResourceText.MIN_VALIDATION));
            }

            if (ddlPackingDimension.SelectedValue == "0")
            {
                AddErrorPrompt(ddlPackingDimension.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else
            {
                PackingDimensionID = Convert.ToInt32(ddlPackingDimension.SelectedValue);
                if(source.Where(x => x.VitualID != VitualID && x.PackingDimensionID == PackingDimensionID).Count() > 0)
                    AddErrorPrompt(ddlPackingDimension.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_EXISTS));
            }

            if (!IsValid)
                ShowErrorPromptExtension();
            else
            {
                string PackingDimensionName = ddlPackingDimension.SelectedItem.Text;
                UpdateRow(e.RowIndex, VitualID, PackingDimensionID, PackingDimensionName, Quantity);

                gvPackingDimension.EditIndex = -1;
                BindPackingDimetionGrid();

                btnAddPackingDimension.Enabled = true;
                btnDeletePackingDimension.Enabled = true;
            }
        }

        protected void gvPackingDimension_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int? PackingDimensionID = (int?)gvPackingDimension.DataKeys[e.Row.RowIndex].Values[1];
                    DropDownList ddlPackingDimension = e.Row.FindControl("ddlPackingDimension") as DropDownList;
                    if (ddlPackingDimension != null)
                    {                        
                        ddlPackingDimension.DataSource = ReferenceTableManager.SelectPackingDimensionForDDL();
                        ddlPackingDimension.DataTextField = "Name";
                        ddlPackingDimension.DataValueField = "ReferencesID";
                        ddlPackingDimension.DataBind();
                        if (PackingDimensionID != null && PackingDimensionID > 0)
                            ddlPackingDimension.SelectedValue = PackingDimensionID.ToString();
                    }
                }
            }
            catch
            {
            }
        }

        protected void gvPackingDimension_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvPackingDimension_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected string ShowNumberFormat(object obj)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString("N2") : "0"; }
            return strPrice;
        }

        private void LoadContactPerson(string custID)
        {
            List<TblContact> ct = ContactManager.SelectAllByCustomerID(int.Parse(custID));
            if (ct != null && ct.Count() > 0)
            {
                //ddlContact.Items.Add(new ListItem(ResourceTextManager.GetApplicationText(ResourceText.CHOOSE_CONTACT), ""));
                foreach (var item in ct)
                {
                    ddlContact.Items.Add(new ListItem(item.ContactName, item.ContactID.ToString()));
                }
            }
        }

        private void LoadJobByCustomer(int p_CustomerID)
        {
            List<TblJob> lstJob = JobManager.SelectJobHasCreatedOCByCustomer(p_CustomerID);

            if (lstJob != null)
            {
                ddlJob.DataSource = lstJob;
                ddlJob.DataTextField = "JobNumber";
                ddlJob.DataValueField = "JobID";
                ddlJob.DataBind();
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
            UpdateDeliveryOrderLockStatus(true);
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!RoleManager.AllowUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }
            UpdateDeliveryOrderLockStatus(false);
        }

        private void UpdateDeliveryOrderLockStatus(bool IsLock)
        {
            if (this.JobID > 0)
            {
                DeliveryOrderManager.LockOrUnlockDeliveryOrder(JobID, IsLock);

                //bool IsAllowEditUnlock = RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID);
                bool IsAllowEdit = !IsLock ? true : false;

                AllowEditting(IsAllowEdit);

                string KEY_MESSAGE = IsLock ? ResourceText.LOCK_DELIVERY_ORDER_SAVE_SUCCESSFULLY : ResourceText.UNLOCK_DELIVERY_ORDER_SAVE_SUCCESSFULLY;

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