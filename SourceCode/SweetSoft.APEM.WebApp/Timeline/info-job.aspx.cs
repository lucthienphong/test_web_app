using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using SweetSoft.APEM.DataAccess;
using System.Data;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Manager;
using System.Text;
using System.IO;

namespace SweetSoft.APEM.WebApp.Timeline
{
    public partial class info_job : Page
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
            string data = Request.Form["Id"];//job id

            //for test
            //if (string.IsNullOrEmpty(data))
            //    data = Request.QueryString["Id"];

            if (!string.IsNullOrEmpty(data))
            {
                int id = 0;
                int.TryParse(data, out id);
                if (id > 0)
                {
                    DataTable dt = DataReturn;

                    DataRow dr = dt.NewRow();

                    TblJobProcess jp = new SubSonic.Select().From(TblJobProcess.Schema)
                            .Where(TblJobProcess.JobIDColumn).IsEqualTo(id)
                            .ExecuteSingle<TblJobProcess>();

                    if (jp != null)
                    {
                        dr["Id"] = id;
                        DateTime dtTemp = jp.StartedOn ?? DateTime.MinValue;
                        string staffName = string.Empty;

                        #region create

                        TblUser us = UserManager.GetUserByUserName(jp.StartedBy);
                        if (us != null)
                            staffName = us.DisplayName;
                        else
                            staffName = "[No name]";

                        dr["Start by"] = staffName;
                        dr["Start on"] = dtTemp;

                        #endregion

                        #region finish

                        dtTemp = jp.FinishedOn ?? DateTime.MinValue;
                        if (string.IsNullOrEmpty(jp.FinishedBy) == false)
                        {
                            us = TimelineManager.GetUserByUserName(jp.FinishedBy);
                            if (us != null)
                                staffName = us.DisplayName;
                            else
                                staffName = "[No name]";
                        }

                        dr["Finish by"] = dtTemp == DateTime.MinValue ? string.Empty : staffName;
                        dr["Finish on"] = dtTemp;


                        #endregion

                        dt.Rows.Add(dr);
                    }
                    else
                    { }

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

                        gv.Columns[0].HeaderStyle.Width = Unit.Percentage(25);
                        gv.Columns[1].HeaderStyle.Width = Unit.Percentage(25);
                        gv.Columns[2].HeaderStyle.Width = Unit.Percentage(25);
                        gv.Columns[3].HeaderStyle.Width = Unit.Percentage(25);

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
            }

            Response.Write(string.Empty);
        }
    }
}