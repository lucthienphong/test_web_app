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
    public partial class ProgressRepro : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "progress_repro_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                ApplyControlText();
                grvProgressRepro.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvProgressRepro.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            grvProgressRepro.Columns[1].HeaderText = "JobNr";
            grvProgressRepro.Columns[2].HeaderText = "Rev";
            grvProgressRepro.Columns[3].HeaderText = "JobName";
            grvProgressRepro.Columns[4].HeaderText = "Design";
            grvProgressRepro.Columns[5].HeaderText = "Qty";
            grvProgressRepro.Columns[6].HeaderText = "Order Recieved";
            //grvProgressRepro.Columns[7].HeaderText = "Proof";
            grvProgressRepro.Columns[7].HeaderText = "ReproDate";
            grvProgressRepro.Columns[8].HeaderText = "Repro Status";
            grvProgressRepro.Columns[9].HeaderText = "CylinderDate";
            grvProgressRepro.Columns[10].HeaderText = "Cylinder Status";
        }

        private void BindDDL()
        {
            BindProgressReproStatusDDL();
            BindProgressReproStatusFilterDDL();
            BindProgressCylinderStatusDDL();
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

        private void BindProgressReproStatusFilterDDL()
        {
            List<TblProgressReproStatus> list = new List<TblProgressReproStatus>();
            list = ProgressReproStatusManager.SelectForReProDDL();
            ddlReProStatus.DataSource = list;
            ddlReProStatus.DataTextField = "ReproStatusName";
            ddlReProStatus.DataValueField = "ReproStatusID";
            ddlReProStatus.DataBind();
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

        public override void BindData()
        {
            try
            {
                string pJobNumber = null;
                int totalRows = 0;
                DateTime? OrderDateB = (DateTime?)null, OrderDateE = (DateTime?)null, ProofDateB = (DateTime?)null, ProofDateE = (DateTime?)null, ReProDateB = (DateTime?)null, ReProDateE = (DateTime?)null, CylinderDateB = (DateTime?)null, CylinderDateE = (DateTime?)null;
                DateTime _OrderDateB = new DateTime(), _OrderDateE =  new DateTime(), _ProofDateB = new DateTime(), _ProofDateE = new DateTime(), _ReProDateB = new DateTime(), _ReProDateE = new DateTime(), _CylinderDateB = new DateTime(), _CylinderDateE = new DateTime();
                //if (DateTime.TryParseExact(txtProofDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ProofDateB))
                //    ProofDateB = _ProofDateB;
                //if (DateTime.TryParseExact(txtProofDateE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ProofDateE))
                //    ProofDateE = _ProofDateE;
                if (DateTime.TryParseExact(txtReProDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ReProDateB))
                    ReProDateB = _ReProDateB;
                if (DateTime.TryParseExact(txtReProDateE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ReProDateE))
                    ReProDateE = _ReProDateE;
                if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateB))
                    CylinderDateB = _CylinderDateB;
                if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateE))
                    CylinderDateE = _CylinderDateE;
                if (!string.IsNullOrEmpty(txtJobNumber.Text.Trim())) 
                {
                    pJobNumber = txtJobNumber.Text.Trim();
                }

                int ReproStatusID = Convert.ToInt32(ddlReProStatus.SelectedValue);
                DataTable dt = ProgressManager.SelectProgressRepro(OrderDateB, OrderDateE, ProofDateB, ProofDateE, ReProDateB, ReProDateE, CylinderDateB, CylinderDateE, ReproStatusID, CurrentPageIndex, grvProgressRepro.PageSize, SortColumn, SortType, pJobNumber);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvProgressRepro.VirtualItemCount = totalRows;
                    grvProgressRepro.DataSource = dt;
                    grvProgressRepro.DataBind();
                    grvProgressRepro.PageIndex = CurrentPageIndex;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvProgressRepro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateRepro")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                hJobID.Value = ID.ToString();
                TblProgress obj = ProgressManager.SelectByID(ID);
                if (obj != null)
                {
                    //txtProof.Text = obj.ProofDate != null ? ((DateTime)obj.ProofDate).ToString("dd/MM/yyyy") : string.Empty;
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
                    TblJobSheet jObj = JobManager.SelectJobSheetByID(ID);
                    if (jObj != null)
                    {
                        //txtProof.Text = string.Empty;
                        txtReproDate.Text = jObj.ReproDate == null ? DateTime.Today.ToString("dd/MM/yyyy") : ((DateTime)jObj.ReproDate).ToString("dd/MM/yyyy");
                        txtProgressReproStatusDesc.Text = string.Empty;
                        ddlProgressReproStatus.SelectedIndex = 0;
                        txtCylinderDate.Text = jObj.CylinderDate == null ? DateTime.Today.ToString("dd/MM/yyyy") : ((DateTime)jObj.CylinderDate).ToString("dd/MM/yyyy");
                        txtProgressCylinderStatusDesc.Text = string.Empty;
                        ddlProgressCylinderStatus.SelectedIndex = 0;
                        txtNote.Text = string.Empty;
                    }
                    else
                    {
                        //txtProof.Text = string.Empty;
                        txtReproDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        ddlProgressReproStatus.SelectedIndex = 0;
                        txtProgressReproStatusDesc.Text = string.Empty;
                        txtCylinderDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        ddlProgressCylinderStatus.SelectedIndex = 0;
                        txtProgressCylinderStatusDesc.Text = string.Empty;
                        txtNote.Text = string.Empty;
                    }
                }
                lbMessage.Visible = false;
                string script = string.Format("{0}({1});", "OpenRepro", "");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", script, true);
            }
        }

        protected void grvProgressRepro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvProgressRepro.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvProgressRepro_Sorting(object sender, GridViewSortEventArgs e)
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
            grvProgressRepro.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvProgressRepro.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
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
                    DateTime? ReproDate = (DateTime?)null, CylinderDate = (DateTime?)null, ProofDate = (DateTime?)null;
                    DateTime _reproDate = new DateTime(), _cylinderDate = new DateTime(), _proofDate = new DateTime();
                    if (DateTime.TryParseExact(txtReproDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _reproDate))
                        ReproDate = _reproDate;
                    if (DateTime.TryParseExact(txtCylinderDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _cylinderDate))
                        CylinderDate = _cylinderDate;
                    //if (DateTime.TryParseExact(txtProof.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _proofDate))
                    //    ProofDate = _proofDate;
                    short? ProgressReproStatusID = (short?)null, ProgressCylinderStatusID = (short?)null;
                    short _progressReproStatusID = 0, _progressCylinderStatusID = 0;
                    if (short.TryParse(ddlProgressReproStatus.SelectedValue, out _progressReproStatusID))
                        ProgressReproStatusID = _progressReproStatusID != 0 ? _progressReproStatusID : (short?)null;
                    if (short.TryParse(ddlProgressCylinderStatus.SelectedValue, out _progressCylinderStatusID))
                        ProgressCylinderStatusID = _progressCylinderStatusID != 0 ? _progressCylinderStatusID : (short?)null;
                    if (obj != null)//Update
                    {
                        obj.ProofDate = ProofDate;
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
                        obj.ProofDate = ProofDate;
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
            grvProgressRepro.PageIndex = 0;
            BindData();
        }

        private void CreateExcel(string fileName)
        {
            DateTime? OrderDateB = (DateTime?)null, OrderDateE = (DateTime?)null, ProofDateB = (DateTime?)null, ProofDateE = (DateTime?)null, ReProDateB = (DateTime?)null, ReProDateE = (DateTime?)null, CylinderDateB = (DateTime?)null, CylinderDateE = (DateTime?)null;
            DateTime _OrderDateB = new DateTime(), _OrderDateE = new DateTime(), _ProofDateB = new DateTime(), _ProofDateE = new DateTime(), _ReProDateB = new DateTime(), _ReProDateE = new DateTime(), _CylinderDateB = new DateTime(), _CylinderDateE = new DateTime();
            //if (DateTime.TryParseExact(txtProofDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ProofDateB))
            //    ProofDateB = _ProofDateB;
            //if (DateTime.TryParseExact(txtProofDateE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ProofDateE))
            //    ProofDateE = _ProofDateE;
            if (DateTime.TryParseExact(txtReProDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ReProDateB))
                ReProDateB = _ReProDateB;
            if (DateTime.TryParseExact(txtReProDateE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ReProDateE))
                ReProDateE = _ReProDateE;
            if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateB))
                CylinderDateB = _CylinderDateB;
            if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateE))
                CylinderDateE = _CylinderDateE;

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

            DataTable dt = ProgressManager.SelectProgressRepro(OrderDateB, OrderDateE, ProofDateB, ProofDateE, ReProDateB, ReProDateE, CylinderDateB, CylinderDateE, ReproStatusID, 0, 0, SortColumn, SortType, JobNumber);
            DataTable dtSource = dt;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/JobProgressRepro_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ProgressReproSrc", dtSource));
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

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("JobProgressRepro_{0}", today);
            CreateExcel(fileName);
        }
    }
}