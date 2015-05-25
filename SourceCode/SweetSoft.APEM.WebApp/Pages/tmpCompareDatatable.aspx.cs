using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.Logs;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class tmpCompareDatatable : System.Web.UI.Page
    {
        protected int LogID
        {
            get
            {
                int _logID = 0;
                if (Session[ViewState["PageID"] + "ID"] != null)
                    int.TryParse(Session[ViewState["PageID"] + "ID"].ToString(), out _logID);
                else if (Request.QueryString["ID"] != null)
                    int.TryParse(Request.QueryString["ID"].ToString(), out _logID);
                return _logID;
            }
            set { }
        }

        protected int JDataIndex
        {
            get
            {
                int _jIndex = 0;
                if (Session[ViewState["PageID"] + "jIndex"] != null)
                    int.TryParse(Session[ViewState["PageID"] + "jIndex"].ToString(), out _jIndex);
                else if (Request.QueryString["jIndex"] != null)
                    int.TryParse(Request.QueryString["jIndex"].ToString(), out _jIndex);
                return _jIndex;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
            }
            if (Request.QueryString["ID"] != null)
            {
                BindData();
            }
        }
        private void BindData()
        {
            TblDataLog dtLogs = DataLogsManager.SeclectDataLogByID(LogID);
            DataLogs contentLogs = JsonHelper.Deserialize<DataLogs>(dtLogs.ContentLogs);
            List<JsonData> lstJData = JsonConvert.DeserializeObject<List<JsonData>>(contentLogs.DataObjLogs.JsonDatas);

            JsonData jData = lstJData[JDataIndex];
            {
                Json jn = new Json();
                jn = JsonConvert.DeserializeObject<Json>(jData.Data);

                grvOld.InnerHtml = jn.OldValue;
                grvNew.InnerHtml = jn.NewValue;
            }
        }
    }
}