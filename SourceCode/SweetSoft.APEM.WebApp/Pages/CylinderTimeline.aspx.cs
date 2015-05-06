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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class CylinderTimeline : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "cylinder_timeline_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCylinder();
                LoadDept();
            }
        }

        void LoadDept()
        {
            TblDepartmentCollection allDept = TimelineManager.GetDepartmentForTimeline();
            if (allDept != null && allDept.Count > 0)
            {
                ddlDepartment.Items.Add(new ListItem("All department", ""));

                foreach (TblDepartment item in allDept)
                {
                    ddlDepartment.Items.Add(new ListItem(item.DepartmentName, item.DepartmentID.ToString()));
                }
            }
        }

        void LoadCylinder()
        {
            string id = ddlDepartment.SelectedValue;
            List<CylinderTimelineObject> allCylinder = TimelineManager.GetCylinderList(id);
            if (allCylinder != null && allCylinder.Count > 0)
            {
                DataTable dtCylinder = TimelineManager.ToDataTable<CylinderTimelineObject>(allCylinder);

                if (dtCylinder != null && dtCylinder.Rows.Count > 0)
                {
                    dtCylinder.Columns[1].Caption = "Jobnumber";
                    dtCylinder.Columns[2].Caption = "Job name";
                    dtCylinder.Columns[3].Caption = "Cylinder no";
                    dtCylinder.Columns[4].Caption = "Cylinder barcode";

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
                    gv.Width = Unit.Percentage(100);

                    StringBuilder sbHeader = new StringBuilder();
                    for (int i = 1; i < dtCylinder.Columns.Count; i++)
                        sbHeader.AppendFormat("<th scope='col'>{0}</th>", dtCylinder.Columns[i].Caption);
                    ltrHeader.Text = sbHeader.ToString();

                    ltrTotal.Text = string.Format("<td colspan='{0}'><span class='titletotal'>{1}</span><span class='counttotal'>{2}</span></td>",
                        (dtCylinder.Columns.Count),
                        //"TOTAL" +
                        ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TOTAL) +
                        " ", dtCylinder.Rows.Count);

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = "Job number";
                    tfield.ItemTemplate = new AddTemplateToGridView(ListItemType.Item, dtCylinder.Columns[1].Caption);
                    gv.Columns.Add(tfield);

                    BoundField bfield = new BoundField();
                    bfield.HeaderText = dtCylinder.Columns[2].Caption;
                    bfield.DataField = dtCylinder.Columns[2].ColumnName;
                    gv.Columns.Add(bfield);

                    bfield = new BoundField();
                    bfield.HeaderText = dtCylinder.Columns[3].Caption;
                    bfield.DataField = dtCylinder.Columns[3].ColumnName;
                    gv.Columns.Add(bfield);

                    bfield = new BoundField();
                    bfield.HeaderText = dtCylinder.Columns[4].Caption;
                    bfield.DataField = dtCylinder.Columns[4].ColumnName;
                    gv.Columns.Add(bfield);

                    gv.DataSource = dtCylinder;
                    gv.DataBind();
                    StringBuilder sbProduct = new StringBuilder();
                    StringWriter sw = new StringWriter(sbProduct);
                    HtmlTextWriter writer = new HtmlTextWriter(sw);
                    gv.RenderControl(writer);

                    ltrProduct.Text = "<td colspan='4' style='padding:0'><div class='innerb'>" +
                        sbProduct.ToString().Substring(5, sbProduct.Length - 11) + "</div></td>";

                    #endregion
                }
            }
            else
                ltrProduct.Text = "<td colspan='4' style='padding:0'><div class='innerb'>" +
                         "</div></td>";
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCylinder();
            upMain.Update();
        }
    }
}