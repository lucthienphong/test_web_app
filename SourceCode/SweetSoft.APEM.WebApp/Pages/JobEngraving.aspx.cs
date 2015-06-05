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
using System.Text;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class JobEngraving : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "job_engraving_manager";
            }
        }

        private int JobID
        {
            get
            {
                int ID = 0;
                if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"], out ID);
                else if (Session[ViewState["PageID"] + "ID"] != null)
                    int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out ID);
                return ID;
            }
        }

        protected bool MechanicalVisible = false;

        protected string MechanicalDisplay
        {
            get{
                return MechanicalVisible ? "normal" : "none";
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
                    Session[ViewState["PageID"] + "ID"] = Request.QueryString["ID"];
                    BindJobData();
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
            //grvServiceJobDetail.Columns[0].HeaderText = "No";
            //grvServiceJobDetail.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_NUMBER);
            //grvServiceJobDetail.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.PRODUCTID);
            //grvServiceJobDetail.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
            //grvServiceJobDetail.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.WORK_ORDER_VALUES_IN_USD);
        }

        //JOB DETAIL----------------------------------------------------------------------------------------------------------------------------
        [WebMethod]
        public static string SelectValues(string Keyword)
        {
            List<string> list = new List<string>();
            TblEngravingScreenAngle result = new TblEngravingScreenAngle();
            if (!string.IsNullOrEmpty(Keyword))
            {
                List<string> values = Keyword.Split('|').ToList<string>();
                if (values != null && values.Count > 1)
                {
                    string screen = values[0];
                    int angle = 0; int.TryParse(values[1], out angle);
                    result = JobEngravingManager.SelectEngravingScreenAngleByScreenAndAngle(screen, angle);
                    if (result != null)
                    {
                        list.Add(result.Sh.HasValue ? result.Sh.ToString() : string.Empty);
                        list.Add(result.Hl.HasValue ? result.Hl.ToString() : string.Empty);
                        list.Add(result.Ch.HasValue ? result.Ch.ToString() : string.Empty);
                        list.Add(result.Midtone.HasValue ? result.Midtone.ToString() : string.Empty);
                    }
                }
            }
            string ret = new JavaScriptSerializer().Serialize(list);
            return ret;
        }

        [WebMethod]
        public static string SelectCellDepth(string Keyword)
        {
            List<string> list = new List<string>();
            TblEngravingStylu result = new TblEngravingStylu();
            if (!string.IsNullOrEmpty(Keyword))
            {
                List<string> values = Keyword.Split('|').ToList<string>();
                if (values != null && values.Count > 1)
                {
                    int stylus = 0; int.TryParse(values[0], out stylus);
                    int sh = 0; int.TryParse(values[1], out sh);

                    result = JobEngravingManager.SelectEngravingStylusByStylusAndSH(stylus, sh);
                    if (result != null)
                    {
                        list.Add(result.CellDepth.HasValue ? result.CellDepth.Value.ToString() : string.Empty);
                    }
                }
            }
            string ret = new JavaScriptSerializer().Serialize(list);
            return ret;
        }

        private void BindRevNumberDDL(int JobID)
        {
            DataTable dt = JobManager.SelectRevNumberForDDL(JobID);
            ddlRevNumber.DataSource = dt;
            ddlRevNumber.DataValueField = "ID";
            ddlRevNumber.DataTextField = "Name";
            ddlRevNumber.DataBind();
        }

        private void BindDDL()
        {

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
                    //ltrPrint.Text = string.Format("<a data-href='Printing/PrintCylinderCertificate.aspx?ID={2}' class='btn btn-transparent' id='btnPrint'>{0}{1}</a>",
                    //    "<span class='flaticon-printer60'></span>", ResourceTextManager.GetApplicationText(ResourceText.PRINT), ID);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"<ul class='dropdown-menu openPrinting' role='menu' style='right: 0; left: auto'>
                                    <li><a href='javascript:;' data-href='Printing/PrintCylinderCertificate.aspx?ID={0}'>{1}</a></li>
                                    <li><a href='javascript:;' data-href='Printing/PrintMechanicalEngraving.aspx?ID={0}'>{2}</a></li>
                                    <li><a href='javascript:;' data-href='Printing/PrintEngravingProtocol.aspx?ID={0}'>{3}</a></li>
                                    <li><a href='javascript:;' data-href='Printing/PrintEtchingProtocol.aspx?ID={0}'>{4}</a></li>
                                </ul>", ID, "Cylinder certificate", "EMG Protocol", "DLS Protocol", "Laser Etching Protocol");
                    ltrPrint.Text = sb.ToString();
                    //Kiểm tra nhân viên còn tồn tại không
                    JobExtension obj = JobManager.SelectByID(ID);
                    if (obj != null)
                    {
                        //Bind JobDetail
                        txtCode.Text = obj.CustomerCode;
                        txtName.Text = obj.CustomerName;
                        hCustomerID.Value = obj.CustomerID.ToString();
                        txtJobNumber.Text = obj.JobNumber;

                        BindRevNumberDDL(obj.JobID);
                        ddlRevNumber.SelectedValue = obj.JobID.ToString();

                        TblEngraving engraving = JobEngravingManager.SelectByID(obj.JobID);
                        if (engraving != null)
                        {
                            //General
                            txtJobCoOrd.Text = engraving.JobCoOrd;
                            txtChromeThickness.Text = engraving.ChromeThickness;
                            txtRoughness.Text = engraving.Roughness;
                            //EMG
                            txtEngravingStart.Text = engraving.EngravingStart != null ? ((double)engraving.EngravingStart).ToString("N2") : "";
                            txtEngravingWidth.Text = engraving.EngravingWidth != null ? ((double)engraving.EngravingWidth).ToString("N2") : "";
                            txtFileSizeHEMG.Text = engraving.FileSizeHEMG != null ? ((double)engraving.FileSizeHEMG).ToString("N2") : "";
                            txtFileSizeVEMG.Text = engraving.FileSizeVEMG != null ? ((double)engraving.FileSizeVEMG).ToString("N2") : "";
                            txtRemarkEMG.Text = engraving.SRRemarkEMG;
                            //DLS
                            chkEngravingOnBoader.Checked = engraving.EngravingOnBoader != null ? Convert.ToBoolean(engraving.EngravingOnBoader) : false;
                            chkEngravingOnNut.Checked = engraving.EngravingOnNut != null ? Convert.ToBoolean(engraving.EngravingOnNut) : false;
                            txtFileSizeHDLS.Text = engraving.FileSizeHDLS != null ? ((double)engraving.FileSizeHDLS).ToString("N2") : "";
                            txtFileSizeVDLS.Text = engraving.FileSizeVDLS != null ? ((double)engraving.FileSizeVDLS).ToString("N2") : "";
                            txtRemarkDLS.Text = engraving.SRRemarkDLS;
                            //Etching
                            txtLaseStart.Text = engraving.LaserStart;
                            txtFileSizeHEtching.Text = engraving.FileSizeHEtching != null ? ((double)engraving.FileSizeHEtching).ToString("N2") : "";
                            txtFileSizeVEtching.Text = engraving.FileSizeVEtching != null ? ((double)engraving.FileSizeVEtching).ToString("N2") : "";
                            txtEngrStartEtching.Text = engraving.EngrStartEtching != null ? ((double)engraving.EngrStartEtching).ToString("N2") : "";
                            txtEngrWidthEtching.Text = engraving.EngrWidthEtching != null ? ((double)engraving.EngrWidthEtching).ToString("N2") : "";
                            txtLaserOperator.Text = engraving.LaserOperator;
                            txtFinalControl.Text = engraving.FinalControl;
                            txtSRRemarkEtching.Text = engraving.SRRemarkEtching;
                        }

                        BindEngravingDetail(obj.JobID);
                        BindTobacco(obj.JobID);
                        BindEtching(obj.JobID);

                        if (Convert.ToBoolean(obj.IsClosed))
                        {
                            AllowEditting(false);
                        }

                        Session[ViewState["PageID"] + "ID"] = obj.JobID;
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_JOB), MSGButton.OK, MSGIcon.Error);
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
            txtJobNumber.Text = "";
            BindRevNumberDDL(0);
            ddlRevNumber.SelectedIndex = 0;

            Session[ViewState["PageID"] + "ID"] = null;
        }

        private void AllowEditting(bool yesno)
        {
            //Job detail
            txtEngravingStart.Enabled = yesno;
            //txtEngravingWidth.Enabled = yesno;

            btnSave.Visible = yesno;
            //btnCancel.Visible = yesno;
            //btnSaveRevDetail.Visible = yesno;
            //btnGetCopy.Visible = yesno;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Job.aspx?ID=" + JobID);
        }

        private void SaveData()
        {
            if (IsValid)
            {
                //Kiểm tra quyền
                if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }

                if (JobID != 0)
                {
                    JobExtension obj = JobManager.SelectByID(JobID);

                    if (obj == null)
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_JOB), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        return;
                    }

                    double EngravingStart = 0, EngravingWidth = 0, FileSizeHEMG = 0, FileSizeVEMG = 0, FileSizeHDLS = 0, FileSizeVDLS = 0, FileSizeHEtching = 0, FileSizeVEtching = 0, EngrStartEtching = 0, EngrWidthEtching = 0;
                    double? _EngravingStart = (double?)null, _EngravingWidth = (double?)null, _FileSizeHEMG = (double?)null, _FileSizeVEMG = (double?)null, _FileSizeHDLS = (double?)null, _FileSizeVDLS = (double?)null, _FileSizeHEtching = (double?)null, _FileSizeVEtching = (double?)null, _EngrStartEtching = (double?)null, _EngrWidthEtching = (double?)null;
                    TblEngraving engraving = JobEngravingManager.SelectByID(obj.JobID);
                    if (engraving != null)
                    {
                        //Update
                        //General
                        engraving.JobCoOrd = txtJobCoOrd.Text.Trim();
                        engraving.ChromeThickness = txtChromeThickness.Text.Trim();
                        engraving.Roughness = txtRoughness.Text;
                        //EMG
                        if (MechanicalVisible)
                        {
                            if (double.TryParse(txtEngravingStart.Text.Trim(), out EngravingStart))
                                _EngravingStart = EngravingStart;
                            if (double.TryParse(txtEngravingWidth.Text.Trim(), out EngravingWidth))
                                _EngravingWidth = EngravingWidth;
                            if (double.TryParse(txtFileSizeHEMG.Text.Trim(), out FileSizeHEMG))
                                _FileSizeHEMG = FileSizeHEMG;
                            if (double.TryParse(txtFileSizeVEMG.Text.Trim(), out FileSizeVEMG))
                                _FileSizeVEMG = FileSizeVEMG;

                            engraving.EngravingStart = _EngravingStart;
                            engraving.EngravingWidth = _EngravingWidth;
                            engraving.FileSizeHEMG = _FileSizeHEMG;
                            engraving.FileSizeVEMG = _FileSizeVEMG;
                            engraving.SRRemarkEMG = txtRemarkEMG.Text.Trim();
                        }
                        else
                        {
                            engraving.EngravingStart = null;
                            engraving.FileSizeHEMG = null;
                            engraving.FileSizeVEMG = null;
                            engraving.SRRemarkEMG = string.Empty;
                        }

                        //DLS
                        if (divTobacco.Visible)
                        {
                            if (double.TryParse(txtFileSizeHDLS.Text.Trim(), out FileSizeHDLS))
                                _FileSizeHDLS = FileSizeHDLS;
                            if (double.TryParse(txtFileSizeVDLS.Text.Trim(), out FileSizeVDLS))
                                _FileSizeVDLS = FileSizeVDLS;

                            engraving.EngravingOnNut = Convert.ToByte(chkEngravingOnNut.Checked);
                            engraving.EngravingOnBoader = Convert.ToByte(chkEngravingOnBoader.Checked);
                            engraving.FileSizeHDLS = _FileSizeHDLS;
                            engraving.FileSizeVDLS = _FileSizeVDLS;
                            engraving.SRRemarkDLS = txtRemarkDLS.Text.Trim();
                        }
                        else
                        {
                            engraving.EngravingOnNut = 0;
                            engraving.EngravingOnBoader = 0;
                            engraving.FileSizeHDLS = null;
                            engraving.FileSizeVDLS = null;
                            engraving.SRRemarkDLS = string.Empty;
                        }

                        //Etching
                        if (divEtching.Visible)
                        {
                            if (double.TryParse(txtFileSizeHEtching.Text.Trim(), out FileSizeHEtching))
                                _FileSizeHEtching = FileSizeHEtching;
                            if (double.TryParse(txtFileSizeHEtching.Text.Trim(), out FileSizeHEtching))
                                _FileSizeVEtching = FileSizeVEtching;
                            if (double.TryParse(txtEngrStartEtching.Text.Trim(), out EngrStartEtching))
                                _EngrStartEtching = EngrStartEtching;
                            if (double.TryParse(txtEngrWidthEtching.Text.Trim(), out EngrWidthEtching))
                                _EngrWidthEtching = EngrWidthEtching;

                            engraving.LaserStart = txtLaseStart.Text.Trim();
                            engraving.FileSizeHEtching = _FileSizeHEtching;
                            engraving.FileSizeVEtching = _FileSizeVEtching;
                            engraving.EngrStartEtching = _EngrStartEtching;
                            engraving.EngrWidthEtching = _EngrWidthEtching;
                            engraving.LaserOperator = txtLaserOperator.Text.Trim();
                            engraving.FinalControl = txtFinalControl.Text.Trim();
                            engraving.SRRemarkEtching = txtSRRemarkEtching.Text.Trim();
                        }
                        else
                        {
                            engraving.LaserStart = string.Empty;
                            engraving.FileSizeHEtching = null;
                            engraving.FileSizeVEtching = null;
                            engraving.EngrStartEtching = null;
                            engraving.EngrWidthEtching = null;
                            engraving.LaserOperator = string.Empty;
                            engraving.FinalControl = string.Empty;
                            engraving.SRRemarkEtching = string.Empty;
                        }

                        engraving = JobEngravingManager.Update(engraving);
                        if (engraving != null)
                        {
                            SaveEngravingDetail(engraving.JobID);
                            SaveEngravingTobacco(engraving.JobID);
                            SaveEngravingEtching(engraving.JobID);

                            BindEngravingDetail(engraving.JobID);
                            BindTobacco(engraving.JobID);
                            BindEtching(engraving.JobID);

                            LoggingActions("Job Engraving",
                                        LogsAction.Objects.Action.UPDATE,
                                        LogsAction.Objects.Status.SUCCESS,
                                        JsonConvert.SerializeObject(new List<JsonData>() { 
                                            new JsonData() { Title = "Job Number", Data = obj.JobNumber } ,
                                            new JsonData() { Title = "Job Rev", Data = obj.RevNumber.ToString() }
                                        }));

                            //Update detail
                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_UPDATED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                            OpenMessageBox(msg, null, false, false);
                        }
                    }
                    else
                    {
                        //Update
                        //General
                        engraving = new TblEngraving();
                        engraving.JobID = JobID;
                        engraving.JobCoOrd = txtJobCoOrd.Text.Trim();
                        engraving.ChromeThickness = txtChromeThickness.Text.Trim();
                        engraving.Roughness = txtRoughness.Text;
                        //EMG
                        if (MechanicalVisible)
                        {
                            if (double.TryParse(txtEngravingStart.Text.Trim(), out EngravingStart))
                                _EngravingStart = EngravingStart;
                            if (double.TryParse(txtEngravingWidth.Text.Trim(), out EngravingWidth))
                                _EngravingWidth = EngravingWidth;
                            if (double.TryParse(txtFileSizeHEMG.Text.Trim(), out FileSizeHEMG))
                                _FileSizeHEMG = FileSizeHEMG;
                            if (double.TryParse(txtFileSizeVEMG.Text.Trim(), out FileSizeVEMG))
                                _FileSizeVEMG = FileSizeVEMG;

                            engraving.EngravingStart = _EngravingStart;
                            engraving.EngravingWidth = _EngravingWidth;
                            engraving.FileSizeHEMG = _FileSizeHEMG;
                            engraving.FileSizeVEMG = _FileSizeVEMG;
                            engraving.SRRemarkEMG = txtRemarkEMG.Text.Trim();
                        }
                        else
                        {
                            engraving.EngravingStart = null;
                            engraving.EngravingWidth = null;
                            engraving.FileSizeHEMG = null;
                            engraving.FileSizeVEMG = null;
                            engraving.SRRemarkEMG = string.Empty;
                        }

                        //DLS
                        if (divTobacco.Visible)
                        {
                            if (double.TryParse(txtFileSizeHDLS.Text.Trim(), out FileSizeHDLS))
                                _FileSizeHDLS = FileSizeHDLS;
                            if (double.TryParse(txtFileSizeVDLS.Text.Trim(), out FileSizeVDLS))
                                _FileSizeVDLS = FileSizeVDLS;

                            engraving.EngravingOnNut = Convert.ToByte(chkEngravingOnNut.Checked);
                            engraving.EngravingOnBoader = Convert.ToByte(chkEngravingOnBoader.Checked);
                            engraving.FileSizeHDLS = _FileSizeHDLS;
                            engraving.FileSizeVDLS = _FileSizeVDLS;
                            engraving.SRRemarkDLS = txtRemarkDLS.Text.Trim();
                        }
                        else
                        {
                            engraving.EngravingOnNut = 0;
                            engraving.EngravingOnBoader = 0;
                            engraving.FileSizeHDLS = null;
                            engraving.FileSizeVDLS = null;
                            engraving.SRRemarkDLS = string.Empty;
                        }

                        //Etching
                        if (divEtching.Visible)
                        {
                            if (double.TryParse(txtFileSizeHEtching.Text.Trim(), out FileSizeHEtching))
                                _FileSizeHEtching = FileSizeHEtching;
                            if (double.TryParse(txtFileSizeHEtching.Text.Trim(), out FileSizeHEtching))
                                _FileSizeVEtching = FileSizeVEtching;
                            if (double.TryParse(txtEngrStartEtching.Text.Trim(), out EngrStartEtching))
                                _EngrStartEtching = EngrStartEtching;
                            if (double.TryParse(txtEngrWidthEtching.Text.Trim(), out EngrWidthEtching))
                                _EngrWidthEtching = EngrWidthEtching;

                            engraving.LaserStart = txtLaseStart.Text.Trim();
                            engraving.FileSizeHEtching = _FileSizeHEtching;
                            engraving.FileSizeVEtching = _FileSizeVEtching;
                            engraving.EngrStartEtching = _EngrStartEtching;
                            engraving.EngrWidthEtching = _EngrWidthEtching;
                            engraving.LaserOperator = txtLaserOperator.Text.Trim();
                            engraving.FinalControl = txtFinalControl.Text.Trim();
                            engraving.SRRemarkEtching = txtSRRemarkEtching.Text.Trim();
                        }
                        else
                        {
                            engraving.LaserStart = string.Empty;
                            engraving.FileSizeHEtching = null;
                            engraving.FileSizeVEtching = null;
                            engraving.EngrStartEtching = null;
                            engraving.EngrWidthEtching = null;
                            engraving.LaserOperator = string.Empty;
                            engraving.FinalControl = string.Empty;
                            engraving.SRRemarkEtching = string.Empty;
                        }

                        engraving = JobEngravingManager.Insert(engraving);
                        if (engraving != null)
                        {
                            //insert detail
                            SaveEngravingDetail(engraving.JobID);
                            SaveEngravingTobacco(engraving.JobID);
                            SaveEngravingEtching(engraving.JobID);

                            BindEngravingDetail(engraving.JobID);
                            BindTobacco(engraving.JobID);
                            BindEtching(engraving.JobID);

                            LoggingActions("Job Engraving",
                                        LogsAction.Objects.Action.CREATE,
                                        LogsAction.Objects.Status.SUCCESS,
                                        JsonConvert.SerializeObject(new List<JsonData>() { 
                                            new JsonData() { Title = "Job Number", Data = obj.JobNumber } ,
                                            new JsonData() { Title = "Job Rev", Data = obj.RevNumber.ToString() }
                                        }));

                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                            OpenMessageBox(msg, null, false, false);
                        }
                    }
                }
            }
            if (!IsValid)
                ShowErrorPromptExtension();
        }

        #region EngravingDetail
        //Load dữ liệu chi tiết
        private void BindEngravingDetail(int JobID)
        {
            DataTable dt = JobManager.tblEngravingDetail_SelectAll(JobID);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                    dt.Columns[i].AllowDBNull = true;
                }
            }

            if (dt.Rows.Count > 0)
            {
                MechanicalVisible = true;                
                Session[ViewState["PageID"] + "tableSource"] = dt;
                BindGrid();
            }
            else
            {
                MechanicalVisible = false;
            }
        }

        //Bind dữ liệu ra lưới
        private void BindGrid()
        {
            int count = -1;
            DataTable dtSource = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                foreach (DataRow row in dtSource.Rows)
                {
                    int ID = 0; int.TryParse(row["EngravingID"].ToString(), out ID);
                    if (ID <= 0)
                    {
                        row["EngravingID"] = count;
                        count--;
                    }
                }
            }
            Session[ViewState["PageID"] + "tableSource"] = dtSource;
            grvCylinder.DataSource = dtSource;
            grvCylinder.DataBind();
        }

        //Xóa dữ liệu không hợp lệ
        private void removeInvalidRows()
        {
            //Get list of department
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            var r = source.AsEnumerable().Where(x => string.IsNullOrEmpty(x.Field<string>("PricingName")));
            foreach (var item in r)
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

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, int EngravingID, string Color, int? stylus, string screen, int? angle, int? gamma, int? sh, int? hl, int? ch, int? mt, double? cellDepth, int? coSh, int? coCh, int? crSh, int? crCh, int cylinderID)
        {
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            if (source != null && source.Rows.Count > 0)
            {
                DataRow r = source.NewRow();
                r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                if (r != null)
                {
                    r["Color"] = Color;
                    r["Stylus"] = (object)stylus ?? DBNull.Value;
                    r["Screen"] = (object)screen ?? DBNull.Value;
                    r["Angle"] = (object)angle ?? DBNull.Value;
                    r["Gamma"] = (object)gamma ?? DBNull.Value;
                    r["Wall"] = DBNull.Value;
                    r["Sh"] = (object)sh ?? DBNull.Value;
                    r["Hl"] = (object)hl ?? DBNull.Value;
                    r["Ch"] = (object)ch ?? DBNull.Value;
                    r["Mt"] = (object)mt ?? DBNull.Value;
                    r["CellDepth"] = (object)cellDepth ?? DBNull.Value;
                    r["CopperSh"] = (object)coSh ?? DBNull.Value;
                    r["CopperCh"] = (object)coCh ?? DBNull.Value;
                    r["ChromeSh"] = (object)crSh ?? DBNull.Value;
                    r["ChromeCh"] = (object)crCh ?? DBNull.Value;
                    r.AcceptChanges();
                }

                Session[ViewState["PageID"] + "tableSource"] = source;
            }
        }

        private void SaveEngravingDetail(int jobID)
        {
            if (!MechanicalVisible)
                return;
            //Lấy danh sách cũ
            List<int> list = JobEngravingManager.GetListEngravingIdByJobID(jobID);
            TblEngravingDetail objDetail;
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            if (source == null)
                source = new DataTable();

            List<int> ListToDelete = new List<int>();
            ListToDelete = list.Where(x => !source.AsEnumerable().Select(a => a.Field<int>("EngravingID")).Contains(x)).ToList();

            foreach (DataRow row in source.AsEnumerable().ToList())
            {
                int engravingID = (int)row["EngravingID"];
                if (engravingID < 0)//Insert
                {
                    objDetail = new TblEngravingDetail();
                    objDetail.CylinderID = (int)row["CylinderID"];
                    objDetail.JobID = jobID;
                    int sequence = 0; int.TryParse(row["Sequence"].ToString(), out sequence);
                    objDetail.Sequence = sequence;

                    int stylus = 0; int.TryParse(row["Stylus"].ToString(), out stylus);
                    int angle = 0; int.TryParse(row["Angle"].ToString(), out angle);
                    int gamma = 0; int.TryParse(row["Gamma"].ToString(), out gamma);
                    int sh = 0; int.TryParse(row["Sh"].ToString(), out sh);
                    int hl = 0; int.TryParse(row["Hl"].ToString(), out hl);
                    int ch = 0; int.TryParse(row["Ch"].ToString(), out ch);
                    int mt = 0; int.TryParse(row["Mt"].ToString(), out mt);
                    float cellDepth = 0; float.TryParse(row["CellDepth"].ToString(), out cellDepth);
                    int copperSh = 0; int.TryParse(row["CopperSh"].ToString(), out copperSh);
                    int copperCh = 0; int.TryParse(row["CopperCh"].ToString(), out copperCh);
                    int chromeSh = 0; int.TryParse(row["ChromeSh"].ToString(), out chromeSh);
                    int chromeCh = 0; int.TryParse(row["ChromeCh"].ToString(), out chromeCh);

                    objDetail.Color = row["Color"].ToString();

                    if (!string.IsNullOrEmpty(row["Stylus"].ToString())) objDetail.Stylus = stylus;
                    else objDetail.Stylus = null;

                    objDetail.Screen = row["Screen"].ToString();

                    if (!string.IsNullOrEmpty(row["Angle"].ToString())) objDetail.Angle = angle;
                    else objDetail.Angle = null;

                    if (!string.IsNullOrEmpty(row["Gamma"].ToString())) objDetail.Gamma = gamma;
                    else objDetail.Gamma = null;

                    if (!string.IsNullOrEmpty(row["Sh"].ToString())) objDetail.Sh = sh;
                    else objDetail.Sh = null;

                    if (!string.IsNullOrEmpty(row["Hl"].ToString())) objDetail.Hl = hl;
                    else objDetail.Hl = null;

                    if (!string.IsNullOrEmpty(row["Ch"].ToString())) objDetail.Ch = ch;
                    else objDetail.Ch = null;

                    if (!string.IsNullOrEmpty(row["Mt"].ToString())) objDetail.Mt = mt;
                    else objDetail.Mt = null;

                    if (!string.IsNullOrEmpty(row["CellDepth"].ToString())) objDetail.CellDepth = cellDepth;
                    else objDetail.CellDepth = null;

                    if (!string.IsNullOrEmpty(row["CopperSh"].ToString())) objDetail.CopperSh = copperSh;
                    else objDetail.CopperSh = null;

                    if (!string.IsNullOrEmpty(row["CopperCh"].ToString())) objDetail.CopperCh = copperCh;
                    else objDetail.CopperCh = null;

                    if (!string.IsNullOrEmpty(row["ChromeSh"].ToString())) objDetail.ChromeSh = chromeSh;
                    else objDetail.ChromeSh = null;

                    if (!string.IsNullOrEmpty(row["ChromeCh"].ToString())) objDetail.ChromeCh = chromeCh;
                    else objDetail.ChromeCh = null;

                    objDetail.IsCopy = Convert.ToByte(row["IsCopy"]);
                    JobEngravingManager.InsertDetail(objDetail);
                }
                else//Update
                {
                    objDetail = JobEngravingManager.SelectDetailByID(engravingID);

                    int stylus = 0; int.TryParse(row["Stylus"].ToString(), out stylus);
                    int angle = 0; int.TryParse(row["Angle"].ToString(), out angle);
                    int gamma = 0; int.TryParse(row["Gamma"].ToString(), out gamma);
                    int sh = 0; int.TryParse(row["Sh"].ToString(), out sh);
                    int hl = 0; int.TryParse(row["Hl"].ToString(), out hl);
                    int ch = 0; int.TryParse(row["Ch"].ToString(), out ch);
                    int mt = 0; int.TryParse(row["Mt"].ToString(), out mt);
                    float cellDepth = 0; float.TryParse(row["CellDepth"].ToString(), out cellDepth);
                    int copperSh = 0; int.TryParse(row["CopperSh"].ToString(), out copperSh);
                    int copperCh = 0; int.TryParse(row["CopperCh"].ToString(), out copperCh);
                    int chromeSh = 0; int.TryParse(row["ChromeSh"].ToString(), out chromeSh);
                    int chromeCh = 0; int.TryParse(row["ChromeCh"].ToString(), out chromeCh);

                    objDetail.Color = row["Color"].ToString();

                    if (!string.IsNullOrEmpty(row["Stylus"].ToString())) objDetail.Stylus = stylus;
                    else objDetail.Stylus = null;

                    objDetail.Screen = row["Screen"].ToString();

                    if (!string.IsNullOrEmpty(row["Angle"].ToString())) objDetail.Angle = angle;
                    else objDetail.Angle = null;

                    if (!string.IsNullOrEmpty(row["Gamma"].ToString())) objDetail.Gamma = gamma;
                    else objDetail.Gamma = null;

                    if (!string.IsNullOrEmpty(row["Sh"].ToString())) objDetail.Sh = sh;
                    else objDetail.Sh = null;

                    if (!string.IsNullOrEmpty(row["Hl"].ToString())) objDetail.Hl = hl;
                    else objDetail.Hl = null;

                    if (!string.IsNullOrEmpty(row["Ch"].ToString())) objDetail.Ch = ch;
                    else objDetail.Ch = null;

                    if (!string.IsNullOrEmpty(row["Mt"].ToString())) objDetail.Mt = mt;
                    else objDetail.Mt = null;

                    if (!string.IsNullOrEmpty(row["CellDepth"].ToString())) objDetail.CellDepth = cellDepth;
                    else objDetail.CellDepth = null;

                    if (!string.IsNullOrEmpty(row["CopperSh"].ToString())) objDetail.CopperSh = copperSh;
                    else objDetail.CopperSh = null;

                    if (!string.IsNullOrEmpty(row["CopperCh"].ToString())) objDetail.CopperCh = copperCh;
                    else objDetail.CopperCh = null;

                    if (!string.IsNullOrEmpty(row["ChromeSh"].ToString())) objDetail.ChromeSh = chromeSh;
                    else objDetail.ChromeSh = null;

                    if (!string.IsNullOrEmpty(row["ChromeCh"].ToString())) objDetail.ChromeCh = chromeCh;
                    else objDetail.ChromeCh = null;

                    objDetail.IsCopy = Convert.ToByte(row["IsCopy"]);
                    JobEngravingManager.UpdateDetail(objDetail);
                }
            }

            foreach (int EngravingID in ListToDelete)
                JobEngravingManager.DeleteDetail(EngravingID);
        }

        protected void grvCylinder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteDetail"))
            {
                int EngravingID = Convert.ToInt32(e.CommandArgument);
                DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                var r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                if (r != null)
                {
                    r.Delete();
                }
                source.AcceptChanges();
                Session[ViewState["PageID"] + "tableSource"] = source;
                CloseMessageBox();
                BindGrid();
            }

            else if (e.CommandName.Equals("Copy"))
            {
                int EngravingID = Convert.ToInt32(e.CommandArgument);
                Random rnd = new Random();
                int rndID = rnd.Next(-1000, -1);
                DataTable source = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                if (source != null && source.Rows.Count > 0)
                {
                    while (source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == rndID).Count() > 0)
                        rndID = rnd.Next(-1000, -1);
                    DataRow desRow = source.NewRow();
                    DataRow r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                    if (r != null)
                    {
                        desRow.ItemArray = r.ItemArray.Clone() as object[];
                        //desRow["Sequence"] = source.Rows.Count + 1;
                        desRow["Color"] = string.Format("{0}_Double_{1}", desRow["Color"], desRow["Sequence"]);
                        desRow["IsCopy"] = true;
                        desRow["EngravingID"] = rndID;
                        source.Rows.Add(desRow);
                        source = source.AsEnumerable().OrderBy(x => x.Field<int>("Sequence")).AsDataView().ToTable();
                    }

                    CloseMessageBox();
                    Session[ViewState["PageID"] + "tableSource"] = source;
                    BindGrid();
                }
            }

        }

        protected void grvCylinder_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvCylinder.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void grvCylinder_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvCylinder.DataKeys[e.RowIndex].Value);
            CustomExtraTextbox txtColor = grvCylinder.Rows[e.RowIndex].FindControl("txtColor") as CustomExtraTextbox;
            ExtraInputMask txtStylus = grvCylinder.Rows[e.RowIndex].FindControl("txtStylus") as ExtraInputMask;
            CustomExtraTextbox txtScreen = grvCylinder.Rows[e.RowIndex].FindControl("txtScreen") as CustomExtraTextbox;
            ExtraInputMask txtAngle = grvCylinder.Rows[e.RowIndex].FindControl("txtAngle") as ExtraInputMask;
            ExtraInputMask txtGamma = grvCylinder.Rows[e.RowIndex].FindControl("txtGamma") as ExtraInputMask;
            ExtraInputMask txtSh = grvCylinder.Rows[e.RowIndex].FindControl("txtSh") as ExtraInputMask;
            ExtraInputMask txtHl = grvCylinder.Rows[e.RowIndex].FindControl("txtHl") as ExtraInputMask;
            ExtraInputMask txtCh = grvCylinder.Rows[e.RowIndex].FindControl("txtCh") as ExtraInputMask;
            ExtraInputMask txtMt = grvCylinder.Rows[e.RowIndex].FindControl("txtMt") as ExtraInputMask;
            ExtraInputMask txtCellDepth = grvCylinder.Rows[e.RowIndex].FindControl("txtCellDepth") as ExtraInputMask;
            ExtraInputMask txtActualCopperSh = grvCylinder.Rows[e.RowIndex].FindControl("txtActualCopperSh") as ExtraInputMask;
            ExtraInputMask txtActualCopperCh = grvCylinder.Rows[e.RowIndex].FindControl("txtActualCopperCh") as ExtraInputMask;
            ExtraInputMask txtActualChromeSh = grvCylinder.Rows[e.RowIndex].FindControl("txtActualChromeSh") as ExtraInputMask;
            ExtraInputMask txtActualChromeCh = grvCylinder.Rows[e.RowIndex].FindControl("txtActualChromeCh") as ExtraInputMask;
            HiddenField hdfCylinderID = grvCylinder.Rows[e.RowIndex].FindControl("hdfCylinderID") as HiddenField;

            if (IsValid)
            {
                int stylus = 0, angle = 0, gamma = 0, sh = 0, hl = 0, ch = 0, mt = 0, copSh = 0, copCh = 0, crSh = 0, crCh = 0;
                string Color = txtColor.Text.Trim();
                string screen = txtScreen.Text.Trim();
                double cellDepth = 0;

                int? _stylus = (int?)null, _angle = (int?)null, _gamma = (int?)null, _sh = (int?)null, _hl = (int?)null, _ch = (int?)null, _mt = (int?)null, _copSh = (int?)null, _copCh = (int?)null, _crSh = (int?)null, _crCh = (int?)null;
                double? _cellDepth = (double?)null;
                if (int.TryParse(txtStylus.Text.Trim(), out stylus))
                    _stylus = stylus;
                if (int.TryParse(txtAngle.Text.Trim(), out angle))
                    _angle = angle;
                if (int.TryParse(txtGamma.Text.Trim(), out gamma))
                    _gamma = gamma;
                if (int.TryParse(txtSh.Text.Trim(), out sh))
                    _sh = sh;
                if (int.TryParse(txtHl.Text.Trim(), out hl))
                    _hl = hl;
                if (int.TryParse(txtCh.Text.Trim(), out ch))
                    _ch = ch;
                if (int.TryParse(txtMt.Text.Trim(), out mt))
                    _mt = mt;
                if (double.TryParse(txtCellDepth.Text.Trim(), out cellDepth))
                    _cellDepth = Convert.ToDouble(cellDepth.ToString("#####"));
                if (int.TryParse(txtActualCopperSh.Text.Trim(), out copSh))
                    _copSh = copSh;
                if (int.TryParse(txtActualCopperCh.Text.Trim(), out copCh))
                    _copCh = copCh;
                if (int.TryParse(txtActualChromeSh.Text.Trim(), out crSh))
                    _crSh = crSh;
                if (int.TryParse(txtActualChromeCh.Text.Trim(), out crCh))
                    _crCh = crCh;

                int cylinderID = 0; int.TryParse(hdfCylinderID.Value, out cylinderID);

                updateRow(e.RowIndex, ID, Color, _stylus, screen, _angle, _gamma, _sh, _hl, _ch, _mt, _cellDepth, _copSh, _copCh, _crSh, _crCh, cylinderID);
                grvCylinder.EditIndex = -1;
                BindGrid();
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        protected void grvCylinder_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grvCylinder_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvCylinder.EditIndex = -1;
            BindGrid();
        }
        #endregion

        #region Tobacco
        //Bind dữ liệu chi tiết
        private void BindTobacco(int JobID)
        {
            DataTable dt = JobEngravingManager.TobaccoSelectAll(JobID);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                    dt.Columns[i].AllowDBNull = true;
                }

                if (dt.AsEnumerable().Where(x => x.Field<int>("Sequence") == 17 && x.Field<int>("CylinderID") == -1).Count() == 0)
                {
                    DataRow r = dt.NewRow();
                    r["EngravingID"] = -1;
                    r["Sequence"] = 17;
                    r["CylinderNo"] = string.Empty;
                    r["CylinderID"] = -1;
                    r["CylinderStatusName"] = string.Empty;
                    r["Color"] = "Barcode";
                    r["IsCopy"] = false;
                    r["MasterScreen"] = false;
                    r["ImageSmoothness"] = false;
                    r["LaserA"] = false;
                    r["LaserB"] = false;
                    dt.Rows.Add(r);
                }
            }
            if (dt.Rows.Count > 0)
            {
                divTobacco.Visible = true;
                Session[ViewState["PageID"] + "tobaccoSource"] = dt;
                BindTobaccoGrid();
            }
            else
            {
                divTobacco.Visible = false;
            }
        }

        //Bind dữ liệu ra lưới
        private void BindTobaccoGrid()
        {
            int count = -1;
            DataTable dtSource = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];
            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                foreach (DataRow row in dtSource.Rows)
                {
                    int ID = 0; int.TryParse(row["EngravingID"].ToString(), out ID);
                    if (ID <= 0)
                    {
                        row["EngravingID"] = count;
                        count--;
                    }
                }
            }

            Session[ViewState["PageID"] + "tobaccoSource"] = dtSource;
            grvTobacco.DataSource = dtSource;
            grvTobacco.DataBind();
        }

        //Xóa dữ liệu không hợp lệ
        private void removeInvalidTobaccoRows()
        {
            //Get list of department
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];
            var r = source.AsEnumerable().Where(x => string.IsNullOrEmpty(x.Field<string>("PricingName")));
            foreach (var item in r)
            {
                item.Delete();
            }
            source.AcceptChanges();
            Session[ViewState["PageID"] + "tobaccoSource"] = source;
        }

        //Xóa dữ liệu được chọn
        private void removeTobaccoSelectedRows(List<int> idList)
        {
            //Get list of department
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];

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
            Session[ViewState["PageID"] + "tobaccoSource"] = source;
        }

        //Cập nhật dòng dữ liệu
        private void updateTobaccoRow(int rowIndex, int EngravingID, string Color, string Screen, byte MasterScreen, string Angle, string Elongation, string Distotion, string Resolution, int? Hexagonal, string HexaName, byte ImageSmoothness, string UnsharpMasking, string Antialiasing, string LineworkWidening, string EngravingStart, string EngravingWidth, int? CellShape, string CellShapeName, int? Gradation, string GraName, string Gamma, byte LaserA, byte LaserB, string CellWidth, string ChannelWidth, string CellDepth, string EngravingTime, string Beam, string Threshold, string CheckedBy, DateTime? CheckedOn)
        {
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];
            if (source != null && source.Rows.Count > 0)
            {
                DataRow r = source.NewRow();
                r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                if (r != null)
                {
                    r["Color"] = Color;
                    r["Screen"] = Screen;
                    r["MasterScreen"] = MasterScreen;
                    r["Angle"] = Angle;
                    r["Elongation"] = Elongation;
                    r["Distotion"] = Distotion;
                    r["Resolution"] = Resolution;
                    r["Hexagonal"] = (object)Hexagonal ?? DBNull.Value;
                    r["HexaName"] = HexaName;
                    r["ImageSmoothness"] = ImageSmoothness;
                    r["UnsharpMasking"] = UnsharpMasking;
                    r["Antialiasing"] = Antialiasing;
                    r["LineworkWidening"] = LineworkWidening;
                    r["EngravingStart"] = EngravingStart;
                    r["EngravingWidth"] = EngravingWidth;
                    r["CellShape"] = (object)CellShape ?? DBNull.Value;
                    r["CellShapeName"] = CellShapeName;
                    r["Gradation"] = (object)Gradation ?? DBNull.Value;
                    r["GraName"] = GraName;
                    r["Gamma"] = Gamma;
                    r["LaserA"] = LaserA;
                    r["LaserB"] = LaserB;
                    r["CellWidth"] = CellWidth;
                    r["ChannelWidth"] = ChannelWidth;
                    r["CellDepth"] = CellDepth;
                    r["EngravingTime"] = EngravingTime;
                    r["Beam"] = Beam;
                    r["Threshold"] = Threshold;
                    r["CheckedBy"] = CheckedBy;
                    r["CheckedOn"] = (object)CheckedOn ?? DBNull.Value;

                    r.AcceptChanges();
                }

                Session[ViewState["PageID"] + "tobaccoSource"] = source;
            }
        }

        //Lưu dữ liệu chi tiết
        private void SaveEngravingTobacco(int jobID)
        {
            if (!divTobacco.Visible)
                return;
            //Lấy danh sách cũ
            List<int> list = JobEngravingManager.GetListEngravingIdTobaccoByJobID(jobID);
            TblEngravingTobacco objDetail;
            DataTable source = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];
            if (source == null)
                source = new DataTable();

            List<int> ListToDelete = new List<int>();
            ListToDelete = list.Where(x => !source.AsEnumerable().Select(a => a.Field<int>("EngravingID")).Contains(x)).ToList();

            foreach (DataRow row in source.AsEnumerable().ToList())
            {
                int engravingID = (int)row["EngravingID"];
                if (engravingID < 0)//Insert
                {
                    objDetail = new TblEngravingTobacco();
                    objDetail.CylinderID = (int)row["CylinderID"];
                    objDetail.JobID = jobID;
                    int sequence = 0; int.TryParse(row["Sequence"].ToString(), out sequence);
                    objDetail.Sequence = sequence;

                    objDetail.Screen = row["Screen"].ToString();
                    objDetail.MasterScreen = Convert.ToByte(row["MasterScreen"]);
                    objDetail.Angle = row["Angle"].ToString();
                    objDetail.Elongation = row["Elongation"].ToString();
                    objDetail.Distotion = row["Distotion"].ToString();
                    objDetail.Resolution = row["Resolution"].ToString();
                    objDetail.Hexagonal = string.IsNullOrEmpty(row["Hexagonal"].ToString()) ? (int?)null : Convert.ToInt32(row["Hexagonal"]);
                    objDetail.ImageSmoothness = Convert.ToByte(row["ImageSmoothness"]);
                    objDetail.UnsharpMasking = row["UnsharpMasking"].ToString();
                    objDetail.Antialiasing = row["Antialiasing"].ToString();
                    objDetail.LineworkWidening = row["LineworkWidening"].ToString();
                    objDetail.EngravingStart = row["EngravingStart"].ToString();
                    objDetail.EngravingWidth = row["EngravingWidth"].ToString();
                    objDetail.CellShape = string.IsNullOrEmpty(row["CellShape"].ToString()) ? (int?)null : Convert.ToInt32(row["CellShape"]);
                    objDetail.Gradation = string.IsNullOrEmpty(row["Gradation"].ToString()) ? (int?)null : Convert.ToInt32(row["Gradation"]);
                    objDetail.Gamma = row["Gamma"].ToString();
                    objDetail.LaserA = Convert.ToByte(row["LaserA"]);
                    objDetail.LaserB = Convert.ToByte(row["LaserB"]);
                    objDetail.CellWidth = row["CellWidth"].ToString();
                    objDetail.ChannelWidth = row["ChannelWidth"].ToString();
                    objDetail.CellDepth = row["CellDepth"].ToString();
                    objDetail.EngravingTime = row["EngravingTime"].ToString();
                    objDetail.Beam = row["Beam"].ToString();
                    objDetail.Threshold = row["Threshold"].ToString();
                    objDetail.CheckedBy = row["CheckedBy"].ToString();
                    objDetail.CheckedOn = string.IsNullOrEmpty(row["CheckedOn"].ToString()) ? (DateTime?)null : (DateTime)row["CheckedOn"];
                    objDetail.IsCopy = Convert.ToByte(row["IsCopy"]);
                    objDetail.Color = row["Color"].ToString();
                    JobEngravingManager.TobaccoInsert(objDetail);
                }
                else//Update
                {
                    objDetail = JobEngravingManager.SelectTobaccoByID(engravingID);
                    int sequence = 0; int.TryParse(row["Sequence"].ToString(), out sequence);
                    objDetail.Sequence = sequence;

                    objDetail.Screen = row["Screen"].ToString();
                    objDetail.MasterScreen = Convert.ToByte(row["MasterScreen"]);
                    objDetail.Angle = row["Angle"].ToString();
                    objDetail.Elongation = row["Elongation"].ToString();
                    objDetail.Distotion = row["Distotion"].ToString();
                    objDetail.Resolution = row["Resolution"].ToString();
                    objDetail.Hexagonal = string.IsNullOrEmpty(row["Hexagonal"].ToString()) ? (int?)null : Convert.ToInt32(row["Hexagonal"]);
                    objDetail.ImageSmoothness = Convert.ToByte(row["ImageSmoothness"]);
                    objDetail.UnsharpMasking = row["UnsharpMasking"].ToString();
                    objDetail.Antialiasing = row["Antialiasing"].ToString();
                    objDetail.LineworkWidening = row["LineworkWidening"].ToString();
                    objDetail.EngravingStart = row["EngravingStart"].ToString();
                    objDetail.EngravingWidth = row["EngravingWidth"].ToString();
                    objDetail.CellShape = string.IsNullOrEmpty(row["CellShape"].ToString()) ? (int?)null : Convert.ToInt32(row["CellShape"]);
                    objDetail.Gradation = string.IsNullOrEmpty(row["Gradation"].ToString()) ? (int?)null : Convert.ToInt32(row["Gradation"]);
                    objDetail.Gamma = row["Gamma"].ToString();
                    objDetail.LaserA = Convert.ToByte(row["LaserA"]);
                    objDetail.LaserB = Convert.ToByte(row["LaserB"]);
                    objDetail.CellWidth = row["CellWidth"].ToString();
                    objDetail.ChannelWidth = row["ChannelWidth"].ToString();
                    objDetail.CellDepth = row["CellDepth"].ToString();
                    objDetail.EngravingTime = row["EngravingTime"].ToString();
                    objDetail.Beam = row["Beam"].ToString();
                    objDetail.Threshold = row["Threshold"].ToString();
                    objDetail.CheckedBy = row["CheckedBy"].ToString();
                    objDetail.CheckedOn = string.IsNullOrEmpty(row["CheckedOn"].ToString()) ? (DateTime?)null : (DateTime)row["CheckedOn"];
                    objDetail.IsCopy = Convert.ToByte(row["IsCopy"]);
                    objDetail.Color = row["Color"].ToString();
                    JobEngravingManager.TobaccoUpdate(objDetail);
                }
            }
            foreach (int EngravingID in ListToDelete)
                JobEngravingManager.TobaccoDelete(EngravingID);
        }

        protected void grvTobaccco_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteDetail"))
            {
                int EngravingID = Convert.ToInt32(e.CommandArgument);
                DataTable source = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];
                var r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                if (r != null)
                {
                    r.Delete();
                }
                source.AcceptChanges();
                int MaxSequence = 10;
                List<DataRow> rs = source.AsEnumerable().Where(x => x.Field<byte>("IsCopy") == 1).OrderBy(x => x.Field<int>("Sequence")).ToList();
                foreach (DataRow item in rs)
                {
                    MaxSequence += 1;
                    item["Sequence"] = MaxSequence;
                }
                source.AcceptChanges();
                Session[ViewState["PageID"] + "tobaccoSource"] = source;
                BindTobaccoGrid();
            }
            else if (e.CommandName.Equals("Copy"))
            {
                int EngravingID = Convert.ToInt32(e.CommandArgument);
                //Get list of department
                Random rnd = new Random();
                int randID = rnd.Next(-1000, -0);
                DataTable source = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];
                if (source != null && source.Rows.Count > 0)
                {
                    while (source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == randID).Count() > 0)
                        randID = rnd.Next(-1000, -1);
                    DataRow desRow = source.NewRow();
                    DataRow r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                    int maxSequence = source.AsEnumerable().Where(x => x.Field<int>("CylinderID") > 0).Max(x => x.Field<int>("Sequence"));
                    if (r != null)
                    {
                        desRow.ItemArray = r.ItemArray.Clone() as object[];
                        desRow["Sequence"] = maxSequence < 11 ? 11 : maxSequence + 1;
                        desRow["Color"] = string.Format("{0}_double_{1}", desRow["Color"], desRow["Sequence"]);
                        desRow["IsCopy"] = true;
                        desRow["EngravingID"] = randID;
                        source.Rows.Add(desRow);
                        source = source.AsEnumerable().OrderBy(x => x.Field<int>("Sequence")).AsDataView().ToTable();
                    }
                    Session[ViewState["PageID"] + "tobaccoSource"] = source;
                    BindTobaccoGrid();
                }
            }
        }

        protected void grvTobaccco_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvTobacco.EditIndex = e.NewEditIndex;
            BindTobaccoGrid();
        }

        protected void grvTobaccco_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvTobacco.DataKeys[e.RowIndex].Value);
            CustomExtraTextbox txtColor = grvTobacco.Rows[e.RowIndex].FindControl("txtColor") as CustomExtraTextbox;
            CustomExtraTextbox txtScreen = grvTobacco.Rows[e.RowIndex].FindControl("txtScreen") as CustomExtraTextbox;
            CheckBox chkMasterScreen = grvTobacco.Rows[e.RowIndex].FindControl("chkMasterScreen") as CheckBox;
            CustomExtraTextbox txtAngle = grvTobacco.Rows[e.RowIndex].FindControl("txtAngle") as CustomExtraTextbox;
            CustomExtraTextbox txtElongation = grvTobacco.Rows[e.RowIndex].FindControl("txtElongation") as CustomExtraTextbox;
            CustomExtraTextbox txtDistotion = grvTobacco.Rows[e.RowIndex].FindControl("txtDistotion") as CustomExtraTextbox;
            CustomExtraTextbox txtResolution = grvTobacco.Rows[e.RowIndex].FindControl("txtResolution") as CustomExtraTextbox;
            DropDownList ddlHexagonal = grvTobacco.Rows[e.RowIndex].FindControl("ddlHexagonal") as DropDownList;
            CheckBox chkImageSmoothness = grvTobacco.Rows[e.RowIndex].FindControl("chkImageSmoothness") as CheckBox;
            CustomExtraTextbox txtUnsharpMasking = grvTobacco.Rows[e.RowIndex].FindControl("txtUnsharpMasking") as CustomExtraTextbox;
            CustomExtraTextbox txtAntialiasing = grvTobacco.Rows[e.RowIndex].FindControl("txtAntialiasing") as CustomExtraTextbox;
            CustomExtraTextbox txtLineworkWidening = grvTobacco.Rows[e.RowIndex].FindControl("txtLineworkWidening") as CustomExtraTextbox;
            CustomExtraTextbox txtEngravingStart = grvTobacco.Rows[e.RowIndex].FindControl("txtEngravingStart") as CustomExtraTextbox;
            CustomExtraTextbox txtEngravingWidth = grvTobacco.Rows[e.RowIndex].FindControl("txtEngravingWidth") as CustomExtraTextbox;
            DropDownList ddlCellShape = grvTobacco.Rows[e.RowIndex].FindControl("ddlCellShape") as DropDownList;
            DropDownList ddlGradation = grvTobacco.Rows[e.RowIndex].FindControl("ddlGradation") as DropDownList;
            TextBox txtGamma = grvTobacco.Rows[e.RowIndex].FindControl("txtGamma") as TextBox;
            CheckBox chkLaserA = grvTobacco.Rows[e.RowIndex].FindControl("chkLaserA") as CheckBox;
            CheckBox chkLaserB = grvTobacco.Rows[e.RowIndex].FindControl("chkLaserB") as CheckBox;
            CustomExtraTextbox txtCellWidth = grvTobacco.Rows[e.RowIndex].FindControl("txtCellWidth") as CustomExtraTextbox;
            CustomExtraTextbox txtChannelWidth = grvTobacco.Rows[e.RowIndex].FindControl("txtChannelWidth") as CustomExtraTextbox;
            CustomExtraTextbox txtCellDepth = grvTobacco.Rows[e.RowIndex].FindControl("txtCellDepth") as CustomExtraTextbox;
            CustomExtraTextbox txtEngravingTime = grvTobacco.Rows[e.RowIndex].FindControl("txtEngravingTime") as CustomExtraTextbox;
            CustomExtraTextbox txtBeam = grvTobacco.Rows[e.RowIndex].FindControl("txtBeam") as CustomExtraTextbox;
            CustomExtraTextbox txtThreshold = grvTobacco.Rows[e.RowIndex].FindControl("txtThreshold") as CustomExtraTextbox;
            CustomExtraTextbox txtCheckedBy = grvTobacco.Rows[e.RowIndex].FindControl("txtCheckedBy") as CustomExtraTextbox;
            ExtraInputMask txtCheckedOn = grvTobacco.Rows[e.RowIndex].FindControl("txtCheckedOn") as ExtraInputMask;

            int? Hexagonal = (int?)null, CellShape = (int?)null, Gradation = (int?)null;
            DateTime? CheckedOn = (DateTime?)null;
            DateTime _CheckedOn = new DateTime();

            string Color = txtColor.Text.Trim();
            string Screen = txtScreen.Text.Trim();
            byte MasterScreen = Convert.ToByte(chkMasterScreen.Checked);
            string Angle = txtAngle.Text.Trim();
            string Elongation = txtElongation.Text.Trim();
            string Distotion = txtDistotion.Text.Trim();
            string Resolution = txtResolution.Text.Trim();
            Hexagonal = ddlHexagonal.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlHexagonal.SelectedValue);
            string HexaName = ddlHexagonal.SelectedValue == "0" ? string.Empty : ddlHexagonal.SelectedItem.Text;
            byte ImageSmoothness = Convert.ToByte(chkImageSmoothness.Checked);
            string UnsharpMasking = txtUnsharpMasking.Text.Trim();
            string Antialiasing = txtAntialiasing.Text.Trim();
            string LineworkWidening = txtLineworkWidening.Text.Trim();
            string EngravingStart = txtEngravingStart.Text.Trim();
            string EngravingWidth = txtEngravingWidth.Text.Trim();
            CellShape = ddlCellShape.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCellShape.SelectedValue);
            string CellShapeName = ddlCellShape.SelectedValue == "0" ? string.Empty : ddlCellShape.SelectedItem.Text;
            Gradation = ddlGradation.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlGradation.SelectedValue);
            string GraName = ddlGradation.SelectedValue == "0" ? string.Empty : ddlGradation.SelectedItem.Text;
            string Gamma = txtGamma.Text.Trim();
            byte LaserA = Convert.ToByte(chkLaserA.Checked);
            byte LaserB = Convert.ToByte(chkLaserB.Checked);
            string CellWidth = txtCellWidth.Text.Trim();
            string ChannelWidth = txtChannelWidth.Text.Trim();
            string CellDepth = txtCellDepth.Text.Trim();
            string EngravingTime = txtEngravingTime.Text.Trim();
            string Beam = txtBeam.Text.Trim();
            string Threshold = txtThreshold.Text.Trim();
            string CheckedBy = txtCheckedBy.Text.Trim();
            if (DateTime.TryParseExact(txtCheckedOn.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CheckedOn))
                CheckedOn = _CheckedOn;

            updateTobaccoRow(e.RowIndex, ID, Color, Screen, MasterScreen, Angle, Elongation, Distotion, Resolution, Hexagonal, HexaName, ImageSmoothness, UnsharpMasking, Antialiasing, LineworkWidening, EngravingStart, EngravingWidth, CellShape, CellShapeName, Gradation, GraName, Gamma, LaserA, LaserB, CellWidth, ChannelWidth, CellDepth, EngravingTime, Beam, Threshold, CheckedBy, CheckedOn);
            grvTobacco.EditIndex = -1;
            BindTobaccoGrid();
        }

        protected void grvTobaccco_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvTobacco.EditIndex = -1;
            BindTobaccoGrid();
        }

        protected void grvTobaccco_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataTable source = (DataTable)Session[ViewState["PageID"] + "tobaccoSource"];
                int EngravingID = Convert.ToInt32(grvTobacco.DataKeys[e.Row.RowIndex].Value);
                if (source == null)
                    source = new DataTable();
                DataRow currRow = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();

                //Hexagonal
                DropDownList ddlHexagonal = e.Row.FindControl("ddlHexagonal") as DropDownList;
                if (ddlHexagonal != null)
                {
                    List<TblReference> listHexa = ReferenceTableManager.SelectHexagonalForDDL();
                    ddlHexagonal.DataSource = listHexa;
                    ddlHexagonal.DataTextField = "Name";
                    ddlHexagonal.DataValueField = "ReferencesID";
                    ddlHexagonal.DataBind();
                    if (currRow != null)
                    {
                        ddlHexagonal.SelectedValue = currRow["Hexagonal"].ToString();
                    }
                }

                //CellShape
                DropDownList ddlCellShape = e.Row.FindControl("ddlCellShape") as DropDownList;
                if (ddlCellShape != null)
                {
                    List<TblReference> listCellShape = ReferenceTableManager.SelectCellShapeForDDL();
                    ddlCellShape.DataSource = listCellShape;
                    ddlCellShape.DataTextField = "Name";
                    ddlCellShape.DataValueField = "ReferencesID";
                    ddlCellShape.DataBind();
                    if (currRow != null)
                    {
                        ddlCellShape.SelectedValue = currRow["CellShape"].ToString();
                    }
                }

                //Gradation
                DropDownList ddlGradation = e.Row.FindControl("ddlGradation") as DropDownList;
                if (ddlGradation != null)
                {
                    List<TblReference> listGradation = ReferenceTableManager.SelectGradationForDDL();
                    ddlGradation.DataSource = listGradation;
                    ddlGradation.DataTextField = "Name";
                    ddlGradation.DataValueField = "ReferencesID";
                    ddlGradation.DataBind();
                    if (currRow != null)
                    {
                        ddlGradation.SelectedValue = currRow["Gradation"].ToString();
                    }
                }
            }
            catch
            {
            }
        }

        protected void grvTobaccco_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion

        #region Etching
        //Bind dữ liệu chi tiết
        private void BindEtching(int JobID)
        {
            DataTable dt = JobEngravingManager.EtchingSelectAll(JobID);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                    dt.Columns[i].AllowDBNull = true;
                }

                if (dt.AsEnumerable().Where(x => x.Field<int>("CylinderID") == -1).Count() == 0)
                {
                    int MaxSequence = dt.AsEnumerable().Max(x => x.Field<int>("Sequence"));
                    DataRow r = dt.NewRow();
                    r["EngravingID"] = -1;
                    r["Sequence"] = MaxSequence > 10 ? MaxSequence + 1 : 11;
                    r["CylinderNo"] = string.Empty;
                    r["CylinderID"] = -1;
                    r["CylinderStatusName"] = string.Empty;
                    r["Color"] = "Barcode";
                    r["IsCopy"] = false;
                    dt.Rows.Add(r);
                }
            }
            if (dt.Rows.Count > 0)
            {
                divEtching.Visible = true;
                Session[ViewState["PageID"] + "etchingSource"] = dt;
                BindEtchingGrid();
            }
            else
            {
                divEtching.Visible = false;
            }
        }

        //Bind dữ liệu ra lưới
        private void BindEtchingGrid()
        {
            int count = -1;
            DataTable dtSource = (DataTable)Session[ViewState["PageID"] + "etchingSource"];
            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                foreach (DataRow row in dtSource.Rows)
                {
                    int ID = 0; int.TryParse(row["EngravingID"].ToString(), out ID);
                    if (ID <= 0)
                    {
                        row["EngravingID"] = count;
                        count--;
                    }
                }
            }

            Session[ViewState["PageID"] + "etchingSource"] = dtSource;
            grvEtching.DataSource = dtSource;
            grvEtching.DataBind();
        }

        //Xóa dữ liệu không hợp lệ
        private void removeInvalidEtchingRows()
        {
            //Get list of department
            DataTable source = (DataTable)Session[ViewState["PageID"] + "etchingSource"];
            var r = source.AsEnumerable().Where(x => string.IsNullOrEmpty(x.Field<string>("PricingName")));
            foreach (var item in r)
            {
                item.Delete();
            }
            source.AcceptChanges();
            Session[ViewState["PageID"] + "etchingSource"] = source;
        }

        //Xóa dữ liệu được chọn
        private void removeEtchingSelectedRows(List<int> idList)
        {
            //Get list of department
            DataTable source = (DataTable)Session[ViewState["PageID"] + "etchingSource"];

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
            Session[ViewState["PageID"] + "etchingSource"] = source;
        }

        //Cập nhật dòng dữ liệu
        private void updateEtchingRow(int rowIndex, int EngravingID, string Color, string screen, string cellType, double? angle, string gamma, double? targetCellSize, double? targetCellWall, double? targetCellDepth, int? developingTime, int? etchingTime, double? chromeCellSize, double? chromeCellWall, double? chromeCellDepth)
        {
            DataTable source = (DataTable)Session[ViewState["PageID"] + "etchingSource"];
            if (source != null && source.Rows.Count > 0)
            {
                DataRow r = source.NewRow();
                r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                if (r != null)
                {
                    r["Color"] = Color;
                    r["ScreenLpi"] = screen;
                    r["CellType"] = cellType;
                    r["Angle"] = (object)angle ?? DBNull.Value;
                    r["Gamma"] = gamma;
                    r["TargetCellSize"] = (object)targetCellSize ?? DBNull.Value; ;
                    r["TargetCellWall"] = (object)targetCellWall ?? DBNull.Value; ;
                    r["TargetCellDepth"] = (object)targetCellDepth ?? DBNull.Value; ;
                    r["DevelopingTime"] = (object)developingTime ?? DBNull.Value; ;
                    r["EtchingTime"] = (object)etchingTime ?? DBNull.Value; ;
                    r["ChromeCellSize"] = (object)chromeCellSize ?? DBNull.Value; ;
                    r["ChromeCellWall"] = (object)chromeCellWall ?? DBNull.Value; ;
                    r["ChromeCellDepth"] = (object)chromeCellDepth ?? DBNull.Value; ;
                    r.AcceptChanges();
                }

                Session[ViewState["PageID"] + "etchingSource"] = source;
            }
        }

        //Lưu dữ liệu chi tiết
        private void SaveEngravingEtching(int jobID)
        {
            if (!divEtching.Visible)
                return;
            //Lấy danh sách cũ
            List<int> list = JobEngravingManager.GetListEngravingIdEtchingByJobID(jobID);
            TblEngravingEtching objDetail;
            DataTable source = (DataTable)Session[ViewState["PageID"] + "etchingSource"];
            if (source == null)
                source = new DataTable();
            List<int> ListToDelete = new List<int>();
            ListToDelete = list.Where(x => !source.AsEnumerable().Select(a => a.Field<int>("EngravingID")).Contains(x)).ToList();

            foreach (DataRow row in source.AsEnumerable().ToList())
            {
                int engravingID = (int)row["EngravingID"];
                if (engravingID < 0)//Insert
                {
                    objDetail = new TblEngravingEtching();
                    objDetail.CylinderID = (int)row["CylinderID"];
                    objDetail.JobID = jobID;
                    int sequence = 0; int.TryParse(row["Sequence"].ToString(), out sequence);
                    objDetail.Sequence = sequence;

                    string Color = row["Color"].ToString();
                    string ScreenLpi = row["ScreenLpi"].ToString();
                    string CellType = row["CellType"].ToString();
                    double Angle = 0; double.TryParse(row["Angle"].ToString(), out Angle);
                    string Gamma = row["Gamma"].ToString();
                    double TargetCellSize = 0; double.TryParse(row["TargetCellSize"].ToString(), out TargetCellSize);
                    double TargetCellWall = 0; double.TryParse(row["TargetCellWall"].ToString(), out TargetCellWall);
                    double TargetCellDepth = 0; double.TryParse(row["TargetCellDepth"].ToString(), out TargetCellDepth);
                    int DevelopingTime = 0; int.TryParse(row["DevelopingTime"].ToString(), out DevelopingTime);
                    int EtchingTime = 0; int.TryParse(row["EtchingTime"].ToString(), out EtchingTime);
                    double ChromeCellSize = 0; double.TryParse(row["ChromeCellSize"].ToString(), out ChromeCellSize);
                    double ChromeCellWall = 0; double.TryParse(row["ChromeCellWall"].ToString(), out ChromeCellWall);
                    double ChromeCellDepth = 0; double.TryParse(row["ChromeCellDepth"].ToString(), out Angle);

                    if (!string.IsNullOrEmpty(row["Color"].ToString())) objDetail.Color = Color;
                    else objDetail.Color = string.Empty;

                    if (!string.IsNullOrEmpty(row["ScreenLpi"].ToString())) objDetail.ScreenLpi = ScreenLpi;
                    else objDetail.ScreenLpi = string.Empty;

                    if (!string.IsNullOrEmpty(row["CellType"].ToString())) objDetail.CellType = CellType;
                    else objDetail.CellType = string.Empty;

                    if (!string.IsNullOrEmpty(row["Angle"].ToString())) objDetail.Angle = Angle;
                    else objDetail.Angle = null;

                    if (!string.IsNullOrEmpty(row["Gamma"].ToString())) objDetail.Gamma = Gamma;
                    else objDetail.Gamma = string.Empty;

                    if (!string.IsNullOrEmpty(row["TargetCellSize"].ToString())) objDetail.TargetCellSize = TargetCellSize;
                    else objDetail.TargetCellSize = null;

                    if (!string.IsNullOrEmpty(row["TargetCellWall"].ToString())) objDetail.TargetCellWall = TargetCellWall;
                    else objDetail.TargetCellWall = null;

                    if (!string.IsNullOrEmpty(row["TargetCellDepth"].ToString())) objDetail.TargetCellDepth = TargetCellDepth;
                    else objDetail.TargetCellDepth = null;

                    if (!string.IsNullOrEmpty(row["DevelopingTime"].ToString())) objDetail.DevelopingTime = DevelopingTime;
                    else objDetail.DevelopingTime = null;

                    if (!string.IsNullOrEmpty(row["EtchingTime"].ToString())) objDetail.EtchingTime = EtchingTime;
                    else objDetail.EtchingTime = null;

                    if (!string.IsNullOrEmpty(row["ChromeCellSize"].ToString())) objDetail.ChromeCellSize = ChromeCellSize;
                    else objDetail.ChromeCellSize = null;

                    if (!string.IsNullOrEmpty(row["ChromeCellWall"].ToString())) objDetail.ChromeCellWall = ChromeCellWall;
                    else objDetail.ChromeCellWall = null;

                    if (!string.IsNullOrEmpty(row["ChromeCellDepth"].ToString())) objDetail.ChromeCellDepth = ChromeCellDepth;
                    else objDetail.ChromeCellDepth = null;

                    objDetail.IsCopy = Convert.ToByte(row["IsCopy"]);
                    JobEngravingManager.EtchingInsert(objDetail);
                }
                else//Update
                {
                    objDetail = JobEngravingManager.SelectEtchingByID(engravingID);

                    int sequence = 0; int.TryParse(row["Sequence"].ToString(), out sequence);
                    objDetail.Sequence = sequence;

                    string Color = row["Color"].ToString();
                    string ScreenLpi = row["ScreenLpi"].ToString();
                    string CellType = row["CellType"].ToString();
                    double Angle = 0; double.TryParse(row["Angle"].ToString(), out Angle);
                    string Gamma = row["Gamma"].ToString();
                    double TargetCellSize = 0; double.TryParse(row["TargetCellSize"].ToString(), out TargetCellSize);
                    double TargetCellWall = 0; double.TryParse(row["TargetCellWall"].ToString(), out TargetCellWall);
                    double TargetCellDepth = 0; double.TryParse(row["TargetCellDepth"].ToString(), out TargetCellDepth);
                    int DevelopingTime = 0; int.TryParse(row["DevelopingTime"].ToString(), out DevelopingTime);
                    int EtchingTime = 0; int.TryParse(row["EtchingTime"].ToString(), out EtchingTime);
                    double ChromeCellSize = 0; double.TryParse(row["ChromeCellSize"].ToString(), out ChromeCellSize);
                    double ChromeCellWall = 0; double.TryParse(row["ChromeCellWall"].ToString(), out ChromeCellWall);
                    double ChromeCellDepth = 0; double.TryParse(row["ChromeCellDepth"].ToString(), out Angle);

                    if (!string.IsNullOrEmpty(row["Color"].ToString())) objDetail.Color = Color;
                    else objDetail.Color = string.Empty;

                    if (!string.IsNullOrEmpty(row["ScreenLpi"].ToString())) objDetail.ScreenLpi = ScreenLpi;
                    else objDetail.ScreenLpi = string.Empty;

                    if (!string.IsNullOrEmpty(row["CellType"].ToString())) objDetail.CellType = CellType;
                    else objDetail.CellType = string.Empty;

                    if (!string.IsNullOrEmpty(row["Angle"].ToString())) objDetail.Angle = Angle;
                    else objDetail.Angle = null;

                    if (!string.IsNullOrEmpty(row["Gamma"].ToString())) objDetail.Gamma = Gamma;
                    else objDetail.Gamma = string.Empty;

                    if (!string.IsNullOrEmpty(row["TargetCellSize"].ToString())) objDetail.TargetCellSize = TargetCellSize;
                    else objDetail.TargetCellSize = null;

                    if (!string.IsNullOrEmpty(row["TargetCellWall"].ToString())) objDetail.TargetCellWall = TargetCellWall;
                    else objDetail.TargetCellWall = null;

                    if (!string.IsNullOrEmpty(row["TargetCellDepth"].ToString())) objDetail.TargetCellDepth = TargetCellDepth;
                    else objDetail.TargetCellDepth = null;

                    if (!string.IsNullOrEmpty(row["DevelopingTime"].ToString())) objDetail.DevelopingTime = DevelopingTime;
                    else objDetail.DevelopingTime = null;

                    if (!string.IsNullOrEmpty(row["EtchingTime"].ToString())) objDetail.EtchingTime = EtchingTime;
                    else objDetail.EtchingTime = null;

                    if (!string.IsNullOrEmpty(row["ChromeCellSize"].ToString())) objDetail.ChromeCellSize = ChromeCellSize;
                    else objDetail.ChromeCellSize = null;

                    if (!string.IsNullOrEmpty(row["ChromeCellWall"].ToString())) objDetail.ChromeCellWall = ChromeCellWall;
                    else objDetail.ChromeCellWall = null;

                    if (!string.IsNullOrEmpty(row["ChromeCellDepth"].ToString())) objDetail.ChromeCellDepth = ChromeCellDepth;
                    else objDetail.ChromeCellDepth = null;

                    objDetail.IsCopy = Convert.ToByte(row["IsCopy"]);

                    JobEngravingManager.EtchingUpdate(objDetail);

                    TblCylinder objCylinder = CylinderManager.SelectByID(objDetail.CylinderID);

                    if (objCylinder != null)
                    {
                        objCylinder.Color = objDetail.Color;
                        CylinderManager.Update(objCylinder);
                    }
                }
            }

            foreach (int EngravingID in ListToDelete)
                JobEngravingManager.EtchingDelete(EngravingID);
        }

        protected void grvEtching_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteDetail"))
            {
                int EngravingID = Convert.ToInt32(e.CommandArgument);
                DataTable source = (DataTable)Session[ViewState["PageID"] + "etchingSource"];
                var r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                if (r != null)
                {
                    r.Delete();
                }
                source.AcceptChanges();
                int MaxSequence = source.AsEnumerable().Where(x => x.Field<int>("CylinderID") > 0 && x.Field<byte>("IsCopy") == 0).Max(x => x.Field<int>("Sequence"));
                List<DataRow> rs = source.AsEnumerable().Where(x => x.Field<int>("CylinderID") < 0 || x.Field<byte>("IsCopy") == 1).OrderBy(x => x.Field<int>("Sequence")).ToList();
                foreach (DataRow item in rs)
                {
                    MaxSequence += 1;
                    item["Sequence"] = MaxSequence;
                }
                source.AcceptChanges();
                Session[ViewState["PageID"] + "etchingSource"] = source;
                BindEtchingGrid();
            }
            else if (e.CommandName.Equals("Copy"))
            {
                int EngravingID = Convert.ToInt32(e.CommandArgument);
                //Get list of department
                Random rnd = new Random();
                int randID = rnd.Next(-1000, -0);
                DataTable source = (DataTable)Session[ViewState["PageID"] + "etchingSource"];
                if (source != null && source.Rows.Count > 0)
                {
                    DataRow barcodeRow = source.AsEnumerable().Where(x => x.Field<int>("CylinderID") == -1).FirstOrDefault();
                    int maxSequence = 11;//Default sequence no. for double is 11
                    if (barcodeRow != null)
                    {
                        maxSequence = Convert.ToInt32(barcodeRow["Sequence"]);
                        barcodeRow["Sequence"] = maxSequence + 1;
                    }
                    while (source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == randID).Count() > 0)
                        randID = rnd.Next(-1000, -1);
                    DataRow desRow = source.NewRow();
                    DataRow r = source.AsEnumerable().Where(x => x.Field<int>("EngravingID") == EngravingID).FirstOrDefault();
                    if (r != null)
                    {
                        desRow.ItemArray = r.ItemArray.Clone() as object[];
                        desRow["Sequence"] = maxSequence;
                        desRow["IsCopy"] = true;
                        desRow["EngravingID"] = randID;
                        source.Rows.Add(desRow);
                        source = source.AsEnumerable().OrderBy(x => x.Field<int>("Sequence")).AsDataView().ToTable();
                    }
                    Session[ViewState["PageID"] + "etchingSource"] = source;
                    BindEtchingGrid();
                }
            }
        }

        protected void grvEtching_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvEtching.EditIndex = e.NewEditIndex;
            BindEtchingGrid();
        }

        protected void grvEtching_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvEtching.DataKeys[e.RowIndex].Value);
            CustomExtraTextbox txtColor = grvEtching.Rows[e.RowIndex].FindControl("txtColor") as CustomExtraTextbox;
            CustomExtraTextbox txtScreenLpi = grvEtching.Rows[e.RowIndex].FindControl("txtScreen") as CustomExtraTextbox;
            CustomExtraTextbox txtCellType = grvEtching.Rows[e.RowIndex].FindControl("txtCellType") as CustomExtraTextbox;
            ExtraInputMask txtAngle = grvEtching.Rows[e.RowIndex].FindControl("txtAngle") as ExtraInputMask;
            CustomExtraTextbox txtGamma = grvEtching.Rows[e.RowIndex].FindControl("txtGamma") as CustomExtraTextbox;
            ExtraInputMask txtTargetCellSize = grvEtching.Rows[e.RowIndex].FindControl("txtTargetCellSize") as ExtraInputMask;
            ExtraInputMask txtTargetCellWall = grvEtching.Rows[e.RowIndex].FindControl("txtTargetCellWall") as ExtraInputMask;
            ExtraInputMask txtTargetCellDepth = grvEtching.Rows[e.RowIndex].FindControl("txtTargetCellDepth") as ExtraInputMask;
            ExtraInputMask txtDevelopingTime = grvEtching.Rows[e.RowIndex].FindControl("txtDevelopingTime") as ExtraInputMask;
            ExtraInputMask txtEtchingTime = grvEtching.Rows[e.RowIndex].FindControl("txtEtchingTime") as ExtraInputMask;
            ExtraInputMask txtChromeCellSize = grvEtching.Rows[e.RowIndex].FindControl("txtChromeCellSize") as ExtraInputMask;
            ExtraInputMask txtChromeCellWall = grvEtching.Rows[e.RowIndex].FindControl("txtChromeCellWall") as ExtraInputMask;
            ExtraInputMask txtChromeCellDepth = grvEtching.Rows[e.RowIndex].FindControl("txtChromeCellDepth") as ExtraInputMask;

            if (IsValid)
            {
                string Color = txtColor.Text;
                string ScreenLpi = txtScreenLpi.Text.Trim();
                string CellType = txtCellType.Text.Trim();
                string Gamma = txtGamma.Text.Trim();
                double? Angle = (double?)null, TargetCellSize = (double?)null, TargetCellWall = (double?)null, TargetCellDepth = (double?)null, ChromeCellSize = (double?)null, ChromeCellWall = (double?)null, ChromeCellDepth = (double?)null;
                double _Angle = 0, _TargetCellSize = 0, _TargetCellWall = 0, _TargetCellDepth = 0, _ChromeCellSize = 0, _ChromeCellWall = 0, _ChromeCellDepth = 0;
                int? DevelopingTime = (int?)null, EtchingTime = (int?)null;
                int _DevelopingTime = 0, _EtchingTime = 0;
                if (double.TryParse(txtAngle.Text.Trim(), out _Angle))
                    Angle = _Angle;
                if (double.TryParse(txtTargetCellSize.Text.Trim(), out _TargetCellSize))
                    TargetCellSize = _TargetCellSize;
                if (double.TryParse(txtTargetCellWall.Text.Trim(), out _TargetCellWall))
                    TargetCellWall = _TargetCellWall;
                if (double.TryParse(txtTargetCellDepth.Text.Trim(), out _TargetCellDepth))
                    TargetCellDepth = _TargetCellDepth;
                if (int.TryParse(txtDevelopingTime.Text.Trim(), out _DevelopingTime))
                    DevelopingTime = _DevelopingTime;
                if (int.TryParse(txtEtchingTime.Text.Trim(), out _EtchingTime))
                    EtchingTime = _EtchingTime;
                if (double.TryParse(txtChromeCellSize.Text.Trim(), out _ChromeCellSize))
                    ChromeCellSize = _ChromeCellSize;
                if (double.TryParse(txtChromeCellWall.Text.Trim(), out _ChromeCellWall))
                    ChromeCellWall = _ChromeCellWall;
                if (double.TryParse(txtChromeCellDepth.Text.Trim(), out _ChromeCellDepth))
                    ChromeCellDepth = _ChromeCellDepth;

                updateEtchingRow(e.RowIndex, ID, Color, ScreenLpi, CellType, Angle, Gamma, TargetCellSize, TargetCellWall, TargetCellDepth, DevelopingTime, EtchingTime, ChromeCellSize, ChromeCellWall, ChromeCellDepth);
                grvEtching.EditIndex = -1;
                BindEtchingGrid();
            }
            if (!IsValid)
            {
                ShowErrorPromptExtension();
            }
        }

        protected void grvEtching_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvEtching.EditIndex = -1;
            BindEtchingGrid();
        }

        protected void grvEtching_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion

        #region Common
        protected string ShowNumberFormat(object obj)
        {
            string strPrice = "0";
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                decimal price = 0;
                decimal.TryParse(obj.ToString(), out price);
                strPrice = price > 0 ? price.ToString("N5") : "0";
            }
            return strPrice;
        }

        protected string ShowDatetimeFormat(object obj)
        {
            string strDate = string.Empty;
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                DateTime tmpDate = (DateTime)obj;
                strDate = tmpDate.ToString("dd/MM/yyyy");
            }
            return strDate;
        }
        #endregion
    }
}