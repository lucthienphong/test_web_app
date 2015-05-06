using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
namespace APEMTimeline
{
    public partial class test : System.Web.UI.Page
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
                        _DataReturn.Columns.Add("Barcode", typeof(string));
                        _DataReturn.Columns.Add("Màu số", typeof(string));
                        _DataReturn.Columns.Add("Chu vi trục", typeof(decimal));
                        _DataReturn.Columns.Add("Chiều dài", typeof(decimal));
                    }
                }
                else
                    _DataReturn.Rows.Clear();

                return _DataReturn;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string data = Request.Form["Id"];

            //for test
            if (string.IsNullOrEmpty(data))
                data = Request.QueryString["Id"];

            if (!string.IsNullOrEmpty(data))
            {
                List<Tblaxisinfo> lst = TimelineManager.GetAxisInfoByOrderCode(data);
                if (lst != null)
                {
                    //Tblaxisinfo.Columns.Id, Tblaxisinfo.Columns.Barcode, Tblaxisinfo.Columns.ColorNo,
                    //Tblaxisinfo.Columns.SizeDiameter, Tblaxisinfo.Columns.SizeLength
                    DataTable dt = DataReturn;
                    foreach (Tblaxisinfo item in lst)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = item.Id.ToString();
                        dr[1] = item.Barcode;
                        dr[2] = item.ColorNo;
                        dr[3] = item.SizeDiameter ?? -1;
                        dr[4] = item.SizeLength ?? -1;
                        dt.Rows.Add(dr);
                    }

                    #region render data

                    GridView gv = new GridView();
                    gv.AutoGenerateColumns = false;
                    gv.CssClass = "responstable";
                    gv.ID = "gvtruc";
                    //gv.RowDataBound += gv_RowDataBound;

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = dt.Columns[1].Caption;
                    tfield.ItemTemplate = new AddTemplateToGridView(ListItemType.Item, dt.Columns[1].Caption, true, false);
                    gv.Columns.Add(tfield);

                    BoundField bfield = new BoundField();
                    bfield.HeaderText = dt.Columns[2].Caption;
                    bfield.DataField = dt.Columns[2].Caption;
                    gv.Columns.Add(bfield);

                    tfield = new TemplateField();
                    tfield.HeaderText = dt.Columns[3].Caption;
                    tfield.ItemTemplate = new AddTemplateToGridView(ListItemType.Item, dt.Columns[3].Caption, false, false);
                    gv.Columns.Add(tfield);

                    tfield = new TemplateField();
                    tfield.HeaderText = dt.Columns[4].Caption;
                    tfield.ItemTemplate = new AddTemplateToGridView(ListItemType.Item, dt.Columns[4].Caption, false, false);
                    gv.Columns.Add(tfield);

                    /*
                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = "Country";
                    gv.Columns.Add(tfield);
                    */

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