using Microsoft.Reporting.WebForms;
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class ProgressEngraving : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "progress_engraving_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                ApplyControlText();
                grvProgressEngraving.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvProgressEngraving.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            grvProgressEngraving.Columns[1].HeaderText = "JobNr";
            grvProgressEngraving.Columns[2].HeaderText = "Rev";
            grvProgressEngraving.Columns[3].HeaderText = "JobName";
            grvProgressEngraving.Columns[4].HeaderText = "Design";
            grvProgressEngraving.Columns[5].HeaderText = "Qty";
            grvProgressEngraving.Columns[6].HeaderText = "Delivery";
            grvProgressEngraving.Columns[7].HeaderText = "Engraving";
            grvProgressEngraving.Columns[8].HeaderText = "ReproDate";
            grvProgressEngraving.Columns[9].HeaderText = "Repro Status";
            grvProgressEngraving.Columns[10].HeaderText = "CylinderDate";
            grvProgressEngraving.Columns[11].HeaderText = "Cylinder Status";
        }

        private void BindDDL()
        {
            BindProgressReproStatusDDL();
            BindProgressCylinderStatusDDL();
            BindProgressReproStatusFilterDDL();
        }

        private void BindProgressReproStatusDDL()
        {
            List<TblProgressReproStatus> list = new List<TblProgressReproStatus>();
            list = ProgressReproStatusManager.SelectForDDL();
            ddlProgressReproStatus.DataSource = list;
            ddlProgressReproStatus.DataTextField = "ReproStatusName";
            ddlProgressReproStatus.DataValueField = "ReproStatusID";
            ddlProgressReproStatus.DataBind();
        }

        private void BindProgressCylinderStatusDDL()
        {
            List<TblProgressCylinderStatus> list = new List<TblProgressCylinderStatus>();
            list = ProgressCylinderStatusManager.SelectForDDL();
            ddlProgressCylinderStatus.DataSource = list;
            ddlProgressCylinderStatus.DataTextField = "CylinderStatusName";
            ddlProgressCylinderStatus.DataValueField = "CylinderStatusID";
            ddlProgressCylinderStatus.DataBind();
        }

        private void BindProgressReproStatusFilterDDL()
        {
            List<TblProgressReproStatus> list = new List<TblProgressReproStatus>();
            list = ProgressReproStatusManager.SelectForEngravingDDL();
            ddlReProStatus.DataSource = list;
            ddlReProStatus.DataTextField = "ReproStatusName";
            ddlReProStatus.DataValueField = "ReproStatusID";
            ddlReProStatus.DataBind();
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                DateTime? DeliveryB = (DateTime?)null, DeliveryE = (DateTime?)null, EngravingB = (DateTime?)null, EngravingE = (DateTime?)null;
                DateTime _DeliveryB = new DateTime(), _DeliveryE = new DateTime(), _EngravingB = new DateTime(), _EngravingE = new DateTime();
                if (DateTime.TryParseExact(txtDeliveryB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryB))
                    DeliveryB = _DeliveryB;
                if (DateTime.TryParseExact(txtDeliveryE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryE))
                    DeliveryE = _DeliveryE;
                if (DateTime.TryParseExact(txtEngravingB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingB))
                    EngravingB = _EngravingB;
                if (DateTime.TryParseExact(txtEngravingE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingE))
                    EngravingE = _EngravingE;

                int ReproStatusID = Convert.ToInt32(ddlReProStatus.SelectedValue);

                string JobNumber = null;
                if (!string.IsNullOrEmpty(txtJobNumber.Text.Trim()))
                {
                    JobNumber = txtJobNumber.Text.Trim();
                }

                DataTable dt = ProgressManager.SelectProgresseEngraving(DeliveryB, DeliveryE, EngravingB, EngravingE, ReproStatusID, CurrentPageIndex, grvProgressEngraving.PageSize, SortColumn, SortType, JobNumber);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvProgressEngraving.VirtualItemCount = totalRows;
                    grvProgressEngraving.DataSource = dt;
                    grvProgressEngraving.DataBind();
                    grvProgressEngraving.PageIndex = CurrentPageIndex;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvProgressEngraving_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateEngraving")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                hJobID.Value = ID.ToString();
                TblProgress obj = ProgressManager.SelectByID(ID);
                if (obj != null)
                {
                    txtDelivery.Text = obj.DeliveryDate != null ? ((DateTime)obj.DeliveryDate).ToString("dd/MM/yyyy") : string.Empty;
                    txtEngravingDate.Text = obj.EngravingDate != null ? ((DateTime)obj.EngravingDate).ToString("dd/MM/yyyy") : string.Empty;
                    //------------------------------------------------------------------------------------------------------
                    txtReproDate.Text = obj.ReproDate != null ? ((DateTime)obj.ReproDate).ToString("dd/MM/yyyy") : DateTime.Today.ToString("dd/MM/yyyy");
                    ddlProgressReproStatus.SelectedValue = obj.ReproStatusID != null ? obj.ReproStatusID.ToString() : "0";
                    ProgressReproStatusManager prReproManager = new ProgressReproStatusManager();
                    TblProgressReproStatus prReproObj = prReproManager.SelectByID(obj.ReproStatusID == null ? (short)0 : (short)obj.ReproStatusID);
                    if (prReproObj != null)
                        txtProgressReproStatusDesc.Text = prReproObj.Description;
                    else
                        txtProgressReproStatusDesc.Text = string.Empty;
                    //------------------------------------------------------------------------------------------------------
                    txtCylinderDate.Text = obj.CylinderDate != null ? ((DateTime)obj.CylinderDate).ToString("dd/MM/yyyy") : DateTime.Today.ToString("dd/MM/yyyy");
                    ddlProgressCylinderStatus.SelectedValue = obj.CylinderStatusID != null ? obj.CylinderStatusID.ToString() : "0";
                    ProgressCylinderStatusManager prCylManager = new ProgressCylinderStatusManager();
                    TblProgressCylinderStatus prCylObj = prCylManager.SelectByID(obj.CylinderStatusID == null ? (short)0 : (short)obj.CylinderStatusID);
                    if (prCylObj != null)
                        txtProgressCylinderStatusDesc.Text = prCylObj.Description;
                    else
                        txtProgressReproStatusDesc.Text = string.Empty;
                    //------------------------------------------------------------------------------------------------------
                    txtNote.Text = obj.Note;
                }
                else
                {
                    txtDelivery.Text = string.Empty;
                    txtEngravingDate.Text = string.Empty;
                    txtNote.Text = string.Empty;
                    ddlProgressReproStatus.SelectedIndex = 0;
                    ddlProgressCylinderStatus.SelectedIndex = 0;

                    TblJobSheet jObj = JobManager.SelectJobSheetByID(ID);
                    if (jObj != null)
                    {
                        txtDelivery.Text = string.Empty;
                        txtEngravingDate.Text = string.Empty;
                        txtReproDate.Text = jObj.ReproDate == null ? DateTime.Today.ToString("dd/MM/yyyy") : ((DateTime)jObj.ReproDate).ToString("dd/MM/yyyy");
                        txtProgressReproStatusDesc.Text = string.Empty;
                        txtCylinderDate.Text = jObj.CylinderDate == null ? DateTime.Today.ToString("dd/MM/yyyy") : ((DateTime)jObj.CylinderDate).ToString("dd/MM/yyyy");
                        txtProgressCylinderStatusDesc.Text = string.Empty;
                    }
                    else
                    {
                        txtReproDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        txtProgressReproStatusDesc.Text = string.Empty;
                        txtCylinderDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        txtProgressCylinderStatusDesc.Text = string.Empty;
                    }
                }
                lbMessage.Visible = false;
                string script = string.Format("{0}({1});", "OpenRepro", "");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", script, true);
            }
        }

        protected void grvProgressEngraving_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvProgressEngraving.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvProgressEngraving_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortType == "A")
            {
                SortType = "D";
            }
            else
            {
                SortType = "A";
            }

            int oldColumnIndex = SortColumn == null ? 0 : int.Parse(SortColumn);
            SortColumn = e.SortExpression;
            int columnIndex = int.Parse(e.SortExpression);
            grvProgressEngraving.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvProgressEngraving.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        private void SaveProgressData()
        {
            try
            {
                int ID = 0;
                if (int.TryParse(hJobID.Value, out ID))
                {
                    TblProgress obj = ProgressManager.SelectByID(ID);
                    DateTime? ReproDate = (DateTime?)null, CylinderDate = (DateTime?)null, DeliveryDate = (DateTime?)null, EngravingDate = (DateTime?)null;
                    DateTime _reproDate = new DateTime(), _cylinderDate = new DateTime(), _deliveryDate = new DateTime(), _engravingDate = new DateTime();
                    if (DateTime.TryParseExact(txtReproDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _reproDate))
                        ReproDate = _reproDate;
                    if (DateTime.TryParseExact(txtCylinderDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _cylinderDate))
                        CylinderDate = _cylinderDate;
                    if (DateTime.TryParseExact(txtDelivery.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _deliveryDate))
                        DeliveryDate = _deliveryDate;
                    if (DateTime.TryParseExact(txtEngravingDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _engravingDate))
                        EngravingDate = _engravingDate;
                    short? ProgressReproStatusID = (short?)null, ProgressCylinderStatusID = (short?)null;
                    short _progressReproStatusID = 0, _progressCylinderStatusID = 0;
                    if (short.TryParse(ddlProgressReproStatus.SelectedValue, out _progressReproStatusID))
                        ProgressReproStatusID = _progressReproStatusID != 0 ? _progressReproStatusID : (short?)null;
                    if (short.TryParse(ddlProgressCylinderStatus.SelectedValue, out _progressCylinderStatusID))
                        ProgressCylinderStatusID = _progressCylinderStatusID != 0 ? _progressCylinderStatusID : (short?)null;
                    if (obj != null)//Update
                    {
                        obj.DeliveryDate = DeliveryDate;
                        obj.EngravingDate = EngravingDate;
                        obj.ReproDate = ReproDate;
                        obj.ReproStatusID = ProgressReproStatusID;
                        obj.CylinderDate = CylinderDate;
                        obj.CylinderStatusID = ProgressCylinderStatusID;
                        obj.Note = txtNote.Text.Trim();
                        ProgressManager.Update(obj);
                    }
                    else
                    {
                        obj = new TblProgress();
                        obj.JobID = ID;
                        obj.DeliveryDate = DeliveryDate;
                        obj.EngravingDate = EngravingDate;
                        obj.ReproDate = ReproDate;
                        obj.ReproStatusID = ProgressReproStatusID;
                        obj.CylinderDate = CylinderDate;
                        obj.CylinderStatusID = ProgressCylinderStatusID;
                        obj.Note = txtNote.Text.Trim();
                        ProgressManager.Insert(obj);
                    }
                    lbMessage.Visible = true;
                    lbMessage.Text = ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            SaveProgressData();
        }

        protected void ddlProgressReproStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            short ReproStatusID = 0;
            if (short.TryParse(ddlProgressReproStatus.SelectedValue, out ReproStatusID))
            {
                ProgressReproStatusManager prManager = new ProgressReproStatusManager();
                TblProgressReproStatus obj = prManager.SelectByID(ReproStatusID);
                if (obj != null)
                    txtProgressReproStatusDesc.Text = obj.Description;
            }
        }

        protected void ddlProgressCylinderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            short CylStatusID = 0;
            if (short.TryParse(ddlProgressCylinderStatus.SelectedValue, out CylStatusID))
            {
                ProgressCylinderStatusManager prManager = new ProgressCylinderStatusManager();
                TblProgressCylinderStatus obj = prManager.SelectByID(CylStatusID);
                if (obj != null)
                    txtProgressCylinderStatusDesc.Text = obj.Description;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvProgressEngraving.PageIndex = 0;
            BindData();
        }

        private void CreateExcel(string fileName)
        {
            DateTime? DeliveryB = (DateTime?)null, DeliveryE = (DateTime?)null, EngravingB = (DateTime?)null, EngravingE = (DateTime?)null;
            DateTime _DeliveryB = new DateTime(), _DeliveryE = new DateTime(), _EngravingB = new DateTime(), _EngravingE = new DateTime();
            if (DateTime.TryParseExact(txtDeliveryB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryB))
                DeliveryB = _DeliveryB;
            if (DateTime.TryParseExact(txtDeliveryE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryE))
                DeliveryE = _DeliveryE;
            if (DateTime.TryParseExact(txtEngravingB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingB))
                EngravingB = _EngravingB;
            if (DateTime.TryParseExact(txtEngravingE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingE))
                EngravingE = _EngravingE;

            int ReproStatusID = Convert.ToInt32(ddlReProStatus.SelectedValue);

            string JobNumber = null;
            if (!string.IsNullOrEmpty(txtJobNumber.Text.Trim()))
            {
                JobNumber = txtJobNumber.Text.Trim();
            }

            //Parameters

            ReportParameter[] parameters = new ReportParameter[1];
            //CompanyName
            string CompanyName = string.Empty;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyName);
            if (setting != null)
                CompanyName = setting.SettingValue;
            parameters[0] = new ReportParameter("CompanyName", CompanyName);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable dt = ProgressManager.SelectProgresseEngraving(DeliveryB, DeliveryE, EngravingB, EngravingE, ReproStatusID, 0, 0, SortColumn, SortType, JobNumber);
            DataTable dtSource = dt;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/JobProgressEngraving_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ProgressEngravingSrc", dtSource));
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

        private void CreateEngraving(string fileName)
        {
            DateTime? DeliveryB = (DateTime?)null, DeliveryE = (DateTime?)null, EngravingB = (DateTime?)null, EngravingE = (DateTime?)null;
            DateTime _DeliveryB = new DateTime(), _DeliveryE = new DateTime(), _EngravingB = new DateTime(), _EngravingE = new DateTime();
            if (DateTime.TryParseExact(txtDeliveryB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryB))
                DeliveryB = _DeliveryB;
            if (DateTime.TryParseExact(txtDeliveryE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryE))
                DeliveryE = _DeliveryE;
            if (DateTime.TryParseExact(txtEngravingB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingB))
                EngravingB = _EngravingB;
            if (DateTime.TryParseExact(txtEngravingE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingE))
                EngravingE = _EngravingE;

            int ReproStatusID = Convert.ToInt32(ddlReProStatus.SelectedValue);


            //Parameters
            string CreatedBy = ApplicationContext.Current.User.UserName;
            string CreatedOnDate = DateTime.Now.ToString("dd/MM/yyyy");
            string CreatedOnTime = DateTime.Now.ToString("hh:mm tt");

            ReportParameter[] parameters = new ReportParameter[3];
            //CompanyName
            //string CompanyName = string.Empty;
            //TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyName);
            //if (setting != null)
            //    CompanyName = setting.SettingValue;
            //parameters[0] = new ReportParameter("CompanyName", CompanyName);
            parameters[0] = new ReportParameter("CreatedBy", CreatedBy);
            parameters[1] = new ReportParameter("CreatedOnDate", CreatedOnDate);
            parameters[2] = new ReportParameter("CreatedOnTime", CreatedOnTime);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            //DataTable dt = ProgressManager.SelectProgresseEngraving(DeliveryB, DeliveryE, EngravingB, EngravingE, ReproStatusID, 0, 0, SortColumn, SortType);
            DataTable dt = ProgressManager.SelectProgresseEngraving(DeliveryB, DeliveryE, EngravingB, EngravingE, ReproStatusID);
            DataTable dtSource = dt;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ProductionScheduleEngaving.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("EngravingSource", dtSource));
            viewer.LocalReport.SetParameters(parameters);

            //Chuyển sang Excel
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        private void CreateEmbossing(string fileName)
        {
            DateTime? DeliveryB = (DateTime?)null, DeliveryE = (DateTime?)null, EngravingB = (DateTime?)null, EngravingE = (DateTime?)null;
            DateTime _DeliveryB = new DateTime(), _DeliveryE = new DateTime(), _EngravingB = new DateTime(), _EngravingE = new DateTime();
            if (DateTime.TryParseExact(txtDeliveryB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryB))
                DeliveryB = _DeliveryB;
            if (DateTime.TryParseExact(txtDeliveryE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryE))
                DeliveryE = _DeliveryE;
            if (DateTime.TryParseExact(txtEngravingB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingB))
                EngravingB = _EngravingB;
            if (DateTime.TryParseExact(txtEngravingE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _EngravingE))
                EngravingE = _EngravingE;

            int ReproStatusID = Convert.ToInt32(ddlReProStatus.SelectedValue);


            //Parameters
            string CreatedBy = ApplicationContext.Current.User.UserName;
            string CreatedOnDate = DateTime.Now.ToString("dd/MM/yyyy");
            string CreatedOnTime = DateTime.Now.ToString("hh:mm tt");

            ReportParameter[] parameters = new ReportParameter[3];
            //CompanyName
            //string CompanyName = string.Empty;
            //TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyName);
            //if (setting != null)
            //    CompanyName = setting.SettingValue;
            //parameters[0] = new ReportParameter("CompanyName", CompanyName);
            parameters[0] = new ReportParameter("CreatedBy", CreatedBy);
            parameters[1] = new ReportParameter("CreatedOnDate", CreatedOnDate);
            parameters[2] = new ReportParameter("CreatedOnTime", CreatedOnTime);

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            //DataTable dt = ProgressManager.SelectProgresseEngraving(DeliveryB, DeliveryE, EngravingB, EngravingE, ReproStatusID, 0, 0, SortColumn, SortType);
            DataTable dt = ProgressManager.SelectProgresseEmbossing(DeliveryB, DeliveryE, EngravingB, EngravingE, ReproStatusID);
            DataTable dtSource = dt;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ProductionScheduleEmbossing.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("EmbossingSource", dtSource));
            viewer.LocalReport.SetParameters(parameters);

            //Chuyển sang Excel
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("JobProgressEngraving_{0}", today);
            CreateExcel(fileName);
        }

        protected void btnEngraving_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("ProductionSchedule_Engraving_{0}", today);
            CreateEngraving(fileName);
        }

        protected void btnEmbossing_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("ProductionSchedule_Embossing_{0}", today);
            CreateEmbossing(fileName);
        }
    }
}