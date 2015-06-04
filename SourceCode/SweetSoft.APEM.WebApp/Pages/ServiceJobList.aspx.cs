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
using SweetSoft.APEM.Core.Logs;


namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class ServiceJobList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "service_job_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                BindDDL();
                grvJobList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvJobList.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER);
            grvJobList.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NR);
            grvJobList.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.JOB_NAME);
            grvJobList.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESIGN);
            grvJobList.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DATE_CREATED);
        }

        private void BindOCStatusDDL()
        {
            List<JobOCStatus> list = ReportManager.SelectJobOCStatusForDDL();
            ddlOCStatus.DataSource = list;
            ddlOCStatus.DataTextField = "StatusName";
            ddlOCStatus.DataValueField = "StatusID";
            ddlOCStatus.DataBind();
        }

        private void BindDOStatusDDL()
        {
            List<JobDOStatus> list = ReportManager.SelectJobDOStatusForDDL();
            ddlDOStatus.DataSource = list;
            ddlDOStatus.DataTextField = "StatusName";
            ddlDOStatus.DataValueField = "StatusID";
            ddlDOStatus.DataBind();
        }

        private void BindInvoiceStatusDDL()
        {
            List<JobInvoiceStatus> list = ReportManager.SelectJobInvoiceStatusForDDL();
            ddlInvoiceStatus.DataSource = list;
            ddlInvoiceStatus.DataTextField = "StatusName";
            ddlInvoiceStatus.DataValueField = "StatusID";
            ddlInvoiceStatus.DataBind();
        }

        private void BindDDL()
        {
            BindOCStatusDDL();
            BindDOStatusDDL();
            BindInvoiceStatusDDL();
        }
      
        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                string Customer = txtCustomer.Text.Trim();
                DateTime? FromDate = (DateTime?)null;
                DateTime? ToDate = (DateTime?)null;
                DateTime _fromDate = new DateTime();
                DateTime _toDate = new DateTime();
                if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate))
                    FromDate = _fromDate;
                if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate))
                    ToDate = _toDate;
                string Number = txtJobNumber.Text.Trim();
                string Info = txtJobInfo.Text.Trim();

                int JobOCStatusID = int.Parse(ddlOCStatus.SelectedValue);
                int JobDOStatusID = int.Parse(ddlDOStatus.SelectedValue);
                int JobInvoiceStatusID = int.Parse(ddlInvoiceStatus.SelectedValue);

                string s_JobStatus = string.Join(",", Enum.GetNames(typeof(JobStatus)));

                DataTable dt = JobManager.SelectAll(Customer, string.Empty, Number, Info, string.Empty, 0, FromDate, ToDate, JobOCStatusID, JobDOStatusID, JobInvoiceStatusID, true, grvJobList.PageIndex, grvJobList.PageSize, SortColumn, SortType, s_JobStatus);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvJobList.VirtualItemCount = totalRows;
                    grvJobList.DataSource = dt;
                    grvJobList.DataBind();
                    grvJobList.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvJobList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvJobList.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvJobList_Sorting(object sender, GridViewSortEventArgs e)
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
            grvJobList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvJobList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            #region Trunglc Add
            // Trunglc Add - 27-04-2015
            for (int i = 0; i < grvJobList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvJobList.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)
                {
                    int ID = Convert.ToInt32(grvJobList.DataKeys[i].Value);

                    bool IsNewJob = JobManager.IsNewJob(ID);
                    bool IsLocking = JobManager.IsJobLocking(ID);

                    if (!IsNewJob)
                    {
                        if (IsLocking || !RoleManager.AllowEditUnlock(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                        {
                            MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msgRole, null, false, false);
                            return;
                        }
                    }

                }
            }

            #endregion

            //Kiểm tra quyền
            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            bool HasDataToDelete = false;
            for (int i = 0; i < grvJobList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvJobList.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)//Nếu tồn tại dòng dữ liệu được check thì hiện thông báo xóa
                {
                    HasDataToDelete = true;
                    break;
                }
            }
            if (HasDataToDelete)
            {
                //Gọi message box
                ModalConfirmResult result = new ModalConfirmResult();
                result.Value = "Job_Delete";
                CurrentConfirmResult = result;
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Do you want to delete seleted rows?", MSGButton.DeleteCancel, MSGIcon.Error);
                OpenMessageBox(msg, result, false, false);
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.SELECT_DATA_TO_DELETE), MSGButton.OK, MSGIcon.Info);
                OpenMessageBox(msg, null, false, false);
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
                        if (e.Value.ToString().Equals("Job_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<int> idList = new List<int>();
                            List<JsonData> lstData = new List<JsonData>();

                            for (int i = 0; i < grvJobList.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)grvJobList.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    int ID = Convert.ToInt32(grvJobList.DataKeys[i].Value);
                                    TblJob obj = JobManager.SelectByID(ID);
                                    if (obj != null)
                                    {
                                        lstData.Add(new JsonData() { Title = "Job Number", Data = obj.JobNumber });
                                    }
                                    idList.Add(ID);
                                }
                            }
                            string DataCannotDelete = removeSelectedRows(idList);
                            BindData();
                            if (string.IsNullOrEmpty(DataCannotDelete))
                            {
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data deleted susscessfully!", MSGButton.OK, MSGIcon.Success);
                                OpenMessageBox(msg, null, false, false);
                            }
                            else
                            {
                                string message = string.Format("{0}:<br/>{1}", ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CANNOT_DELETE), DataCannotDelete);
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Warning);
                                OpenMessageBox(msg, null, false, false);
                            }
                        }
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.INVALID_VALUE), MSGButton.OK, MSGIcon.Error);
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

        //Xóa dữ liệu được chọn
        private string removeSelectedRows(List<int> idList)
        {
            string cannotDeletedList = "";
            foreach (int ID in idList)
            {
                if (!JobManager.IsBeingUsed(ID))
                {
                    TblJob obj = JobManager.SelectByID(ID);
                    if (obj != null && obj.IsServiceJob == 1)
                    {
                        if (!Convert.ToBoolean(obj.IsClosed))
                            JobManager.Delete(obj.JobID);

                        //Lưu vào logging
                        if (AllowSaveLogging)
                            LoggingManager.LogAction(ResourceTextManager.GetApplicationText(ResourceText.DELETE_SERVICE_JOB), FUNCTION_PAGE_ID, obj.ToJSONString());
                    }
                }
                else
                {
                    DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                    string JobNo = dt.AsEnumerable().Where(x => x.Field<int>("JobID") == ID).Select(x => x.Field<string>("JobNumber")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", JobNo);
                }
            }
            if (!string.IsNullOrEmpty(cannotDeletedList))
            {
                cannotDeletedList = cannotDeletedList.Substring(0, cannotDeletedList.Length - 2);
            }
            return cannotDeletedList;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvJobList.PageIndex = 0;
            BindData();
        }


        //------------------------------------------------------------------------------------------------------------------------------------------------
        //EXPORT EXCEL
        public static string FixBase64ForImage(string Image)
        {
            return System.Web.HttpUtility.HtmlDecode(Image);
        }

        private static string ConvertImageToBase64(string base64String)
        {

            string dataImg64 = base64String.Replace("data:image/png;base64,", "");
            byte[] imageBytes = Convert.FromBase64String(FixBase64ForImage(dataImg64));
            return Convert.ToBase64String(imageBytes);

        }

        private void CreateExcel(string fileName)
        {
            string Customer = txtCustomer.Text.Trim();
            DateTime? FromDate = (DateTime?)null;
            DateTime? ToDate = (DateTime?)null;
            DateTime _fromDate = new DateTime();
            DateTime _toDate = new DateTime();
            if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate))
                FromDate = _fromDate;
            if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate))
                ToDate = _toDate;
            string Number = txtJobNumber.Text.Trim();
            string Info = txtJobInfo.Text.Trim();
           
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

            DataTable dt = JobManager.SelectForReport(Customer, string.Empty, Number, Info, 0, FromDate, ToDate, grvJobList.PageIndex, grvJobList.PageSize, SortColumn, SortType);
            dt.Columns["JobBarcodeImage"].ReadOnly = false;
            foreach (DataRow r in dt.Rows)
            {
                string Base64Image = r["JobBarcodeImage"].ToString();
                r["JobBarcodeImage"] = ConvertImageToBase64(Base64Image);
                r.AcceptChanges();
            }
            DataTable dtSource = dt;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/JobList_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("JobListSrc", dtSource));
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
            string fileName = string.Format("JobList_{0}", today);
            CreateExcel(fileName);
        }

        protected void grvJobList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Pages/ServiceJob.aspx?ID=" + ID.ToString());
            }
        }
    }
}