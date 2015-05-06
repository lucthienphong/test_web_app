using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.WebApp.Timeline
{
    /// <summary>
    /// Summary description for DataHelper
    /// </summary>
    public class DataHelper
    {
        public DataHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        static DataTable Default_DataReturn;
        public static DataTable DefaultDataReturn
        {
            get
            {
                if (Default_DataReturn == null)
                {
                    Default_DataReturn = new DataTable();
                    if (Default_DataReturn.Columns.Count == 0)
                    {
                        Default_DataReturn.Columns.Add("Id", typeof(string));
                        Default_DataReturn.Columns.Add("Job number", typeof(string));
                        Default_DataReturn.Columns.Add("Barcode", typeof(string));
                        Default_DataReturn.Columns.Add("Job name", typeof(string));
                        Default_DataReturn.Columns.Add("Customer code", typeof(string));
                        //Default_DataReturn.Columns.Add("Customer name", typeof(string));
                    }
                }
                else
                    Default_DataReturn.Rows.Clear();

                return Default_DataReturn;
            }
        }

        public static string LoadJob(out int rowCount)
        {
            StringBuilder sbProduct = new StringBuilder();

            DataTable dtProduct = DefaultDataReturn;
            dtProduct.Rows.Clear();
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
                    gv.Width = new Unit("100%");
                    gv.ShowHeader = false;
                    gv.ShowFooter = false;
                    //gv.RowStyle.CssClass = "even";
                    //gv.AlternatingRowStyle.CssClass = "odd";
                    //old
                    //gv.CssClass = "responstable";
                    gv.ID = "gvdonhang";
                    //old
                    //gv.RowDataBound += gv_RowDataBound;
                    //new

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
                    //bfield.DataFormatString = "{0:dd/MM/yyyy}";
                    //bfield.NullDisplayText = "empty";
                    gv.Columns.Add(bfield);

                    bfield = new BoundField();
                    bfield.HeaderText = dtProduct.Columns[4].Caption;
                    bfield.DataField = dtProduct.Columns[4].Caption;
                    gv.Columns.Add(bfield);

                    //bfield = new BoundField();
                    //bfield.HeaderText = dtProduct.Columns[5].Caption;
                    //bfield.DataField = dtProduct.Columns[5].Caption;
                    //gv.Columns.Add(bfield);

                    /*
                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = "Country";
                    gv.Columns.Add(tfield);
                    */
                    gv.Attributes.Add("datacount", dtProduct.Rows.Count.ToString());
                    gv.DataSource = dtProduct;
                    gv.DataBind();
                    StringWriter sw = new StringWriter(sbProduct);
                    HtmlTextWriter writer = new HtmlTextWriter(sw);
                    gv.RenderControl(writer);

                    #endregion
                }
            }
            rowCount = dtProduct.Rows.Count;
            return sbProduct.ToString();
        }
    }
}