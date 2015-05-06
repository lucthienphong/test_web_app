using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System.IO;
using System.Text.RegularExpressions;
namespace SweetSoft.APEM.WebApp.Timeline
{
    public partial class job_list : Page
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
                        _DataReturn.Columns.Add("Job name", typeof(string));
                        _DataReturn.Columns.Add("Customer code", typeof(string));
                        _DataReturn.Columns.Add("Customer name", typeof(string));
                    }
                }
                else
                    _DataReturn.Rows.Clear();

                return _DataReturn;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string data = Request.Form["act"];

            //for test
            if (string.IsNullOrEmpty(data))
                data = Request.QueryString["act"];

            if (!string.IsNullOrEmpty(data) && data == "reloadjob")
            {
                int rowCount;
                Response.Write(DataHelper.LoadJob(out rowCount));
                return;
            }
            Response.Write(string.Empty);
        }
    }
}