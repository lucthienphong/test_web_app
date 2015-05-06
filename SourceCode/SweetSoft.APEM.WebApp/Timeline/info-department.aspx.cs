using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.WebApp.Timeline
{
    public partial class info_department : Page
    {
        static DataTable _DataReturn;
        static DataTable DataReturn
        {
            get
            {
                if (_DataReturn == null)
                {
                    _DataReturn = new DataTable();
                    if (_DataReturn.Columns.Count == 0)
                    {
                        _DataReturn.Columns.Add("Id", typeof(string));
                        /*vie
                        _DataReturn.Columns.Add("Người bắt đầu", typeof(string));
                        _DataReturn.Columns.Add("Ngày bắt đầu", typeof(DateTime));
                        _DataReturn.Columns.Add("Người kết thúc", typeof(string));
                        _DataReturn.Columns.Add("Ngày kết thúc", typeof(DateTime));
                        */
                        //eng
                        _DataReturn.Columns.Add("Start by", typeof(string));
                        _DataReturn.Columns.Add("Start on", typeof(DateTime));
                        _DataReturn.Columns.Add("Finish by", typeof(string));
                        _DataReturn.Columns.Add("Finish on", typeof(DateTime));
                    }
                }
                else
                    _DataReturn.Rows.Clear();

                return _DataReturn;
            }
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                if (dr != null)
                {
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string data = Request.Form["pb"];//code
            string id = Request.Form["Id"];//order

            //for test
            if (string.IsNullOrEmpty(data))
                data = Request.QueryString["pb"];

            if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(id))
            {
                DataTable dt = DataReturn;

                DataRow dr = dt.NewRow();
                TblJobProcessingCollection lst = TimelineManager.GetHistoryByDeptCode(id, data);
                if (lst != null && lst.Count > 0)
                {

                    dr["Id"] = id;

                    #region create

                    var foundCreate = lst.Where(x => x.CreatedOn.HasValue);
                    string staffName = string.Empty;
                    DateTime dtTemp = DateTime.MinValue;
                    if (foundCreate != null && foundCreate.Count() > 0)
                    {
                        TblUser us = TimelineManager.GetUserByUserName(foundCreate.First().CreatedBy);
                        if (us != null)
                            staffName = us.DisplayName;
                        else
                            staffName = "[No name]";
                        dtTemp = foundCreate.First().CreatedOn.Value;
                    }

                    dr["Start by"] = staffName;
                    dr["Start on"] = dtTemp;

                    #endregion

                    #region finish

                    var foundFinish = lst.Where(x => x.FinishedOn.HasValue);
                    staffName = string.Empty;
                    dtTemp = DateTime.MinValue;
                    if (foundFinish != null && foundFinish.Count() > 0)
                    {
                        TblUser us = TimelineManager.GetUserByUserName(foundFinish.First().FinishedBy);
                        if (us != null)
                            staffName = us.DisplayName;
                        else
                            staffName = "[No name]";
                        dtTemp = foundFinish.First().FinishedOn.Value;
                    }

                    dr["Finish by"] = staffName;
                    dr["Finish on"] = dtTemp;

                    #endregion
                }
                else
                { }
                dt.Rows.Add(dr);

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region render data

                    GridView gv = new GridView();
                    gv.AutoGenerateColumns = false;
                    gv.CssClass = "table table-bordered table-striped";
                    gv.ID = "gvnoidung";
                    //gv.RowDataBound += gv_RowDataBound;

                    BoundField bfield = new BoundField();
                    bfield.HeaderText = dt.Columns[1].Caption;
                    bfield.DataField = dt.Columns[1].Caption;
                    gv.Columns.Add(bfield);

                    bfield = new BoundField();
                    bfield.HeaderText = dt.Columns[2].Caption;
                    bfield.DataField = dt.Columns[2].Caption;
                    gv.Columns.Add(bfield);

                    bfield = new BoundField();
                    bfield.HeaderText = dt.Columns[3].Caption;
                    bfield.DataField = dt.Columns[3].Caption;
                    gv.Columns.Add(bfield);

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = dt.Columns[4].Caption;
                    tfield.ItemTemplate = new AddTemplateToGridView(ListItemType.Item, dt.Columns[4].Caption);
                    gv.Columns.Add(tfield);


                    gv.Columns[gv.Columns.Count - 1].ItemStyle.CssClass = "last";
                    gv.Columns[0].ItemStyle.CssClass = "first";
                    gv.DataSource = dt;
                    gv.DataBind();
                    StringBuilder sbReturn = new StringBuilder();
                    StringWriter sw = new StringWriter(sbReturn);
                    HtmlTextWriter writer = new HtmlTextWriter(sw);
                    gv.RenderControl(writer);

                    Response.Write(sbReturn.ToString());
                    return;

                    #endregion
                }
            }
            Response.Write(string.Empty);
        }
    }
}