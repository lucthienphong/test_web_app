using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using SweetSoft.APEM.WebApp.Timeline;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class JobTimeline : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "job_timeline_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadJob();
        }

        void LoadJob()
        {
            DataTable dtProduct = new DataTable();
            dtProduct.Columns.Add("Id", typeof(string));
            dtProduct.Columns.Add("Job number", typeof(string));
            dtProduct.Columns.Add("Barcode", typeof(string));
            dtProduct.Columns.Add("Job name", typeof(string));
            dtProduct.Columns.Add("Customer code", typeof(string));
            //dtProduct.Columns.Add("Customer name", typeof(string));

            TblJobCollection allJob = TimelineManager.GetJobList();
            if (allJob != null && allJob.Count > 0)
            {
                foreach (TblJob item in allJob)
                {
                    DataRow dr = dtProduct.NewRow();
                    dr[0] = item.JobID.ToString();
                    dr[1] = item.JobNumber;
                    dr[2] = item.JobBarcode;
                    dr[3] = item.JobName;
                    dr[4] = item.TblCustomer.Code;
                    //dr[5] = item.TblCustomer.Name;
                    dtProduct.Rows.Add(dr);
                }

                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    #region render data

                    GridView gv = new GridView();
                    gv.AutoGenerateColumns = false;
                    //old
                    //gv.UseAccessibleHeader = true;
                    //gv.ShowHeader = true;
                    //new
                    //gv.Width = new Unit("100%");
                    gv.CssClass = "table table-bordered table-striped table-hover";
                    gv.ShowHeader = false;
                    gv.ShowFooter = false;
                    gv.ID = "gvdonhang";
                    
                    StringBuilder sbHeader = new StringBuilder();
                    for (int i = 1; i < dtProduct.Columns.Count; i++)
                        sbHeader.AppendFormat("<th scope='col'>{0}</th>", dtProduct.Columns[i].Caption);
                    ltrHeader.Text = sbHeader.ToString();

                    ltrTotal.Text = string.Format("<td colspan='{0}'><span class='titletotal'>{1}</span><span class='counttotal'>{2}</span></td>",
                        (dtProduct.Columns.Count - 1),
                        "TOTAL" +
                        //ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.totaL)+
                        " : ", dtProduct.Rows.Count);

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = dtProduct.Columns[1].Caption;
                    tfield.ItemTemplate = new AddTemplateToGridView(ListItemType.Item, dtProduct.Columns[1].Caption);
                    gv.Columns.Add(tfield);

                    BoundField bfield = new BoundField();
                    bfield.HeaderText = dtProduct.Columns[2].Caption;
                    bfield.DataField = dtProduct.Columns[2].Caption;
                    gv.Columns.Add(bfield);

                    bfield = new BoundField();
                    bfield.HeaderText = dtProduct.Columns[3].Caption;
                    bfield.DataField = dtProduct.Columns[3].Caption;
                    gv.Columns.Add(bfield);

                    bfield = new BoundField();
                    bfield.HeaderText = dtProduct.Columns[4].Caption;
                    bfield.DataField = dtProduct.Columns[4].Caption;
                    gv.Columns.Add(bfield);

                    //bfield = new BoundField();
                    //bfield.HeaderText = dtProduct.Columns[5].Caption;
                    //bfield.DataField = dtProduct.Columns[5].Caption;
                    //gv.Columns.Add(bfield);

                    gv.DataSource = dtProduct;
                    gv.DataBind();
                    StringBuilder sbProduct = new StringBuilder();
                    StringWriter sw = new StringWriter(sbProduct);
                    HtmlTextWriter writer = new HtmlTextWriter(sw);
                    gv.RenderControl(writer);

                    ltrProduct.Text = "<td colspan='" +
                        (dtProduct.Columns.Count - 1) + "' style='padding:0'><div class='innerb'>" +
                        sbProduct.ToString().Substring(5, sbProduct.Length - 11) + "</div></td>";

                    #endregion
                }
            }
        }
    }
}