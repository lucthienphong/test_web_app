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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class ProgressDeReChrome : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "progress_derechrome_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                ApplyControlText();
                grvProgressDeReChrome.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvProgressDeReChrome.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            grvProgressDeReChrome.Columns[1].HeaderText = "JobNr";
            grvProgressDeReChrome.Columns[2].HeaderText = "Rev";
            grvProgressDeReChrome.Columns[3].HeaderText = "JobName";
            grvProgressDeReChrome.Columns[4].HeaderText = "Design";
            grvProgressDeReChrome.Columns[5].HeaderText = "Qty";
            grvProgressDeReChrome.Columns[6].HeaderText = "Delivery";
            grvProgressDeReChrome.Columns[7].HeaderText = "DeRe Date";
            grvProgressDeReChrome.Columns[8].HeaderText = "CylinderDate";
            grvProgressDeReChrome.Columns[9].HeaderText = "Cylinder Status";
        }

        private void BindDDL()
        {
            BindProgressCylinderStatusDDL();
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
                int totalRows = 0;
                DateTime? DeliveryB = (DateTime?)null, DeliveryE = (DateTime?)null, DeReDateB = (DateTime?)null, DeReDateE = (DateTime?)null, CylinderDateB = (DateTime?)null, CylinderDateE = (DateTime?)null;
                DateTime _DeliveryB = new DateTime(), _DeliveryE = new DateTime(), _DeReDateB = new DateTime(), _DeReDateE = new DateTime(), _CylinderDateB = new DateTime(), _CylinderDateE = new DateTime();
                if (DateTime.TryParseExact(txtDeliveryB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryB))
                    DeliveryB = _DeliveryB;
                if (DateTime.TryParseExact(txtDeliveryE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryE))
                    DeliveryE = _DeliveryE;
                if (DateTime.TryParseExact(txtDeReDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeReDateB))
                    DeReDateB = _DeReDateB;
                if (DateTime.TryParseExact(txtDeReDateE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeReDateE))
                    DeReDateE = _DeReDateE;
                if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateB))
                    CylinderDateB = _CylinderDateB;
                if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateE))
                    CylinderDateE = _CylinderDateE;

                string JobNumber = null;
                if (!string.IsNullOrEmpty(txtJobNumber.Text.Trim()))
                {
                    JobNumber = txtJobNumber.Text.Trim();
                }

                string CustomerName = null;
                if (!string.IsNullOrEmpty(txtCustomerName.Text.Trim()))
                {
                    CustomerName = txtCustomerName.Text.Trim();
                }

                DataTable dt = ProgressManager.SelectProgresseDeReChrome(DeliveryB, DeliveryE, DeReDateB, DeReDateE, CylinderDateB, CylinderDateE, CurrentPageIndex, grvProgressDeReChrome.PageSize, SortColumn, SortType, JobNumber, CustomerName);

                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvProgressDeReChrome.VirtualItemCount = totalRows;
                    grvProgressDeReChrome.DataSource = dt;
                    grvProgressDeReChrome.DataBind();
                    grvProgressDeReChrome.PageIndex = CurrentPageIndex;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvProgressDeReChrome_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateEngraving")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                hJobID.Value = ID.ToString();
                TblProgress obj = ProgressManager.SelectByID(ID);
                if (obj != null)
                {
                    txtDelivery.Text = obj.DeliveryDate != null ? ((DateTime)obj.DeliveryDate).ToString("dd/MM/yyyy") : string.Empty;
                    txtDeReDate.Text = obj.DeReDate != null ? ((DateTime)obj.DeReDate).ToString("dd/MM/yyyy") : string.Empty;
                    //------------------------------------------------------------------------------------------------------
                    txtCylinderDate.Text = obj.CylinderDate != null ? ((DateTime)obj.CylinderDate).ToString("dd/MM/yyyy") : DateTime.Today.ToString("dd/MM/yyyy");
                    ddlProgressCylinderStatus.SelectedValue = obj.CylinderStatusID != null ? obj.CylinderStatusID.ToString() : "0";
                    txtProgressCylinderStatusDesc.Text = string.Empty;
                }
                else
                {
                    txtDelivery.Text = string.Empty;
                    txtDeReDate.Text = string.Empty;
                    txtNote.Text = string.Empty;
                    ddlProgressCylinderStatus.SelectedIndex = 0;
                    TblJobSheet jObj = JobManager.SelectJobSheetByID(ID);
                    if (jObj != null)
                    {
                        txtCylinderDate.Text = jObj.CylinderDate == null ? DateTime.Today.ToString("dd/MM/yyyy") : ((DateTime)jObj.CylinderDate).ToString("dd/MM/yyyy");
                        txtProgressCylinderStatusDesc.Text = string.Empty;
                    }
                    else
                    {
                        txtCylinderDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        txtProgressCylinderStatusDesc.Text = string.Empty;
                    }
                }
                lbMessage.Visible = false;
                string script = string.Format("{0}({1});", "OpenRepro", "");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", script, true);
            }
        }

        protected void grvProgressDeReChrome_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvProgressDeReChrome.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvProgressDeReChrome_Sorting(object sender, GridViewSortEventArgs e)
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
            grvProgressDeReChrome.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvProgressDeReChrome.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
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
                    DateTime? CylinderDate = (DateTime?)null, DeliveryDate = (DateTime?)null, DeReDate = (DateTime?)null;
                    DateTime _cylinderDate = new DateTime(), _deliveryDate = new DateTime(), _deReDate = new DateTime();
                    if (DateTime.TryParseExact(txtCylinderDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _cylinderDate))
                        CylinderDate = _cylinderDate;
                    if (DateTime.TryParseExact(txtDelivery.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _deliveryDate))
                        DeliveryDate = _deliveryDate;
                    if (DateTime.TryParseExact(txtDeReDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _deReDate))
                        DeReDate = _deReDate;
                    short? ProgressCylinderStatusID = (short?)null;
                    short _progressCylinderStatusID = 0;
                    if (short.TryParse(ddlProgressCylinderStatus.SelectedValue, out _progressCylinderStatusID))
                        ProgressCylinderStatusID = _progressCylinderStatusID != 0 ? _progressCylinderStatusID : (short?)null;
                    if (obj != null)//Update
                    {
                        obj.DeliveryDate = DeliveryDate;
                        obj.DeReDate = DeReDate;
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
                        obj.DeReDate = DeReDate;
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

        [WebMethod]
        public static void UpdateJobSeq()
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvProgressDeReChrome.PageIndex = 0;
            BindData();
        }

        private void CreateExcel(string fileName)
        {
            DateTime? DeliveryB = (DateTime?)null, DeliveryE = (DateTime?)null, DeReDateB = (DateTime?)null, DeReDateE = (DateTime?)null, CylinderDateB = (DateTime?)null, CylinderDateE = (DateTime?)null;
            DateTime _DeliveryB = new DateTime(), _DeliveryE = new DateTime(), _DeReDateB = new DateTime(), _DeReDateE = new DateTime(), _CylinderDateB = new DateTime(), _CylinderDateE = new DateTime();
            if (DateTime.TryParseExact(txtDeliveryB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryB))
                DeliveryB = _DeliveryB;
            if (DateTime.TryParseExact(txtDeliveryE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryE))
                DeliveryE = _DeliveryE;
            if (DateTime.TryParseExact(txtDeReDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeReDateB))
                DeReDateB = _DeReDateB;
            if (DateTime.TryParseExact(txtDeReDateE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeReDateE))
                DeReDateE = _DeReDateE;
            if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateB))
                CylinderDateB = _CylinderDateB;
            if (DateTime.TryParseExact(txtCylDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _CylinderDateE))
                CylinderDateE = _CylinderDateE;

            string JobNumber = null;
            if (!string.IsNullOrEmpty(txtJobNumber.Text.Trim()))
            {
                JobNumber = txtJobNumber.Text.Trim();
            }

            string CustomerName = null;
            if (!string.IsNullOrEmpty(txtCustomerName.Text.Trim()))
            {
                CustomerName = txtCustomerName.Text.Trim();
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

            DataTable dt = ProgressManager.SelectProgresseDeReChrome(DeliveryB, DeliveryE, DeReDateB, DeReDateE, CylinderDateB, CylinderDateE, 0, 0, SortColumn, SortType, JobNumber, CustomerName);
            DataTable dtSource = dt;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/JobProgressDeReChrome_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ProgressDeReChromeSrc", dtSource));
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

        private void CreateDeRe(string fileName)
        {
            DateTime? DeliveryB = (DateTime?)null, DeliveryE = (DateTime?)null, DeReDateB = (DateTime?)null, DeReDateE = (DateTime?)null;
            DateTime _DeliveryB = new DateTime(), _DeliveryE = new DateTime(), _DeReDateB = new DateTime(), _DeReDateE = new DateTime();
            if (DateTime.TryParseExact(txtDeliveryB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryB))
                DeliveryB = _DeliveryB;
            if (DateTime.TryParseExact(txtDeliveryE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeliveryE))
                DeliveryE = _DeliveryE;
            if (DateTime.TryParseExact(txtDeReDateB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeReDateB))
                DeReDateB = _DeReDateB;
            if (DateTime.TryParseExact(txtDeReDateE.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _DeReDateE))
                DeReDateE = _DeReDateE;


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
            DataTable dt = ProgressManager.SelectProgresseDeReChrome(DeliveryB, DeliveryE, DeReDateB, DeReDateE);
            DataTable dtSource = dt;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ProductionShceduleDeReChrome.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("DeReSource", dtSource));
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
            string fileName = string.Format("JobProgressDeRe_{0}", today);
            CreateExcel(fileName);
        }

        protected void btnDeRe_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("ProductionSchedule_DeRe_{0}", today);
            CreateDeRe(fileName);
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
    }
}