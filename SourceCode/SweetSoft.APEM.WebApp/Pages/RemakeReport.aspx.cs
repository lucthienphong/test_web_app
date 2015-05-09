using Microsoft.Reporting.WebForms;
using SweetSoft.APEM.Core;
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
using System.Drawing;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class RemakeReport : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "remake_report_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ViewState["PageID"] = (new Random()).Next().ToString();
                //BindDDL();
                //ApplyControlText();
                grvRemakeReport.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
                BindFillterDLL();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            //grvRemakeReport.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CUSTOMER_CODE);
            //grvRemakeReport.Columns[1].HeaderText = "JobNr";
            //grvRemakeReport.Columns[2].HeaderText = "Rev";
            //grvRemakeReport.Columns[3].HeaderText = "JobName";
            //grvRemakeReport.Columns[4].HeaderText = "Design";
            //grvRemakeReport.Columns[5].HeaderText = "Qty";
            //grvRemakeReport.Columns[6].HeaderText = "Delivery";
            //grvRemakeReport.Columns[7].HeaderText = "Engraving";
            //grvRemakeReport.Columns[8].HeaderText = "ReproDate";
            //grvRemakeReport.Columns[9].HeaderText = "Repro Status";
            //grvRemakeReport.Columns[10].HeaderText = "CylinderDate";
            //grvRemakeReport.Columns[11].HeaderText = "Cylinder Status";
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                DateTime? FromDate = (DateTime?)null, ToDate = (DateTime?)null;
                DateTime _FromDate = new DateTime(), _ToDate = new DateTime();
                if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _FromDate))
                    FromDate = _FromDate;
                if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ToDate))
                    ToDate = _ToDate;

                DataTable dt = ReportManager.RemakeReport(FromDate, ToDate);

                DataRow[] dtFillter = null;
                int fillterIndex = string.IsNullOrEmpty(ddlFillterReport.SelectedValue) ? 1 : Convert.ToInt32(ddlFillterReport.SelectedValue);

                switch (fillterIndex)
                {
                    case 1:
                        dtFillter = dt.Select("1 = 1");
                        break;
                    case 2:
                        dtFillter = dt.Select("CByS > 0 OR CByR > 0 OR CByP > 0 OR CByO > 0");
                        break;
                    case 3:
                        dtFillter = dt.Select("CByS = 0 AND CByR = 0 AND CByP = 0 AND CByO = 0");
                        break;
                }

                DataTable newDT = dt.Clone();

                if (dtFillter != null)
                {
                    int i = 1;
                    foreach (DataRow dr in dtFillter)
                    {
                        DataRow newR = newDT.NewRow();
                        newR.ItemArray = dr.ItemArray.Clone() as object[];
                        newR["RowNumber"] = i;
                        newDT.Rows.Add(newR);
                        i++;
                    }
                }
                else
                {
                    newDT = dt.Copy();
                }

                if (newDT.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    grvRemakeReport.VirtualItemCount = totalRows;
                    grvRemakeReport.DataSource = newDT;
                    grvRemakeReport.DataBind();
                    grvRemakeReport.PageIndex = CurrentPageIndex;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvRemakeReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grvRemakeReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            grvRemakeReport.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvRemakeReport_Sorting(object sender, GridViewSortEventArgs e)
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
            grvRemakeReport.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvRemakeReport.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            grvRemakeReport.PageIndex = 0;
            BindData();
        }

        private void CreateExcel(string fileName)
        {
            DateTime? FromDate = (DateTime?)null, ToDate = (DateTime?)null;
            DateTime _FromDate = new DateTime(), _ToDate = new DateTime();
            if (DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _FromDate))
                FromDate = _FromDate;
            if (DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _ToDate))
                ToDate = _ToDate;
            string DateInfo = string.Empty;
            if (FromDate != null && ToDate != null)
            {
                if (DateTime.Compare(_FromDate, _ToDate) == 0)
                    DateInfo = _FromDate.ToString("dd/MM/yyyy");
                else
                    DateInfo = string.Format("From date: {0} - To date: {1}", _FromDate.ToString("dd/MM/yyyy"), _ToDate.ToString("dd/MM/yyyy"));
            }
            else if (FromDate != null)
            {
                DateInfo = string.Format("From date: {0}", _FromDate.ToString("dd/MM/yyyy"));
            }
            else if (ToDate != null)
            {
                DateInfo = string.Format("To date: {0}", _ToDate.ToString("dd/MM/yyyy"));
            }
            //Parameters
            string CreatedBy = ApplicationContext.Current.User.UserName;
            string CreatedOnDate = DateTime.Now.ToString("dd/MM/yyyy");
            string CreatedOnTime = DateTime.Now.ToString("hh:mm tt");
            ReportParameter[] parameters = new ReportParameter[5];
            //CompanyName
            string CompanyName = string.Empty;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyName);
            if (setting != null)
                CompanyName = setting.SettingValue;
            parameters[0] = new ReportParameter("CompanyName", CompanyName);
            parameters[1] = new ReportParameter("DateInfo", DateInfo);
            parameters[2] = new ReportParameter("CreatedBy", CreatedBy);
            parameters[3] = new ReportParameter("CreatedOnDate", CreatedOnDate);
            parameters[4] = new ReportParameter("CreatedOnTime", CreatedOnTime);


            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            DataTable dtSource = ReportManager.RemakeReport(FromDate, ToDate);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Remake_rpt.rdlc");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource("RemakeDS", dtSource));
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string today = DateTime.Today.ToString("dd_MM_yyyy");
            string fileName = string.Format("RemakeReport_{0}", today);
            CreateExcel(fileName);
        }

        protected void grvRemakeReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                int valCByS = Convert.ToInt32(rowView["CByS"].ToString());
                int valCByR = Convert.ToInt32(rowView["CByR"].ToString());
                int valCByP = Convert.ToInt32(rowView["CByP"].ToString());
                int valCByO = Convert.ToInt32(rowView["CByO"].ToString());

                if (valCByS == 0 && valCByR == 0 && valCByP == 0 && valCByO == 0)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = Color.FromName("#e3e3e3");
                    }
                }
            }
        }

        protected void BindFillterDLL()
        {
            Dictionary<int, string> dt = new Dictionary<int, string>();
            dt.Add(1, "All");
            dt.Add(2, "Dept entered");
            dt.Add(3, "No dept entered");

            ddlFillterReport.DataSource = dt;
            ddlFillterReport.DataTextField = "Value";
            ddlFillterReport.DataValueField = "Key";
            ddlFillterReport.DataBind();
        }

    }
}