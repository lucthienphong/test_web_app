using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Controls
{
    public partial class SelectReceivers : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TblDepartmentCollection allDeparment =
                    new SubSonic.Select().From(TblDepartment.Schema)
                    .ExecuteAsCollection<TblDepartmentCollection>();

                if (allDeparment != null && allDeparment.Count > 0)
                {
                    chkDepartment.Items.Clear();
                    //chkDepartment.Items.Add(new ListItem("None", ""));
                    //chkDepartment.Items.Add(new ListItem("All department", "all"));
                    foreach (TblDepartment item in allDeparment)
                        chkDepartment.Items.Add(new ListItem(item.DepartmentName, item.DepartmentID.ToString()));
                }

                SetData();
            }
        }

        #region prop

        public string DataReceive
        {
            get
            {
                return ViewState["DataReceive"] == null ? string.Empty : (string)ViewState["DataReceive"];
            }
            set
            {
                ViewState["DataReceive"] = value;
            }
        }

        List<int> AllOldDept
        {
            get
            {
                if (ViewState["AllOldDept"] != null)
                    return ViewState["AllOldDept"] as List<int>;
                return null;
            }
            set
            {
                ViewState["AllOldDept"] = value;
            }
        }

        List<string> AllUserIds
        {
            get
            {
                if (ViewState["AllUserIds"] != null)
                    return ViewState["AllUserIds"] as List<string>;
                return null;
            }
            set
            {
                ViewState["AllUserIds"] = value;
            }
        }

        List<string> AllExceptUserIds
        {
            get
            {
                if (ViewState["AllExceptUserIds"] != null)
                    return ViewState["AllExceptUserIds"] as List<string>;
                return null;
            }
            set
            {
                ViewState["AllExceptUserIds"] = value;
            }
        }

        protected DataTable Data
        {
            get
            {
                return Session["SweetSoft.APEM.WebApp-SelectReceivers" + UniqueID] as DataTable;
            }
            set
            {
                Session["SweetSoft.APEM.WebApp-SelectReceivers" + UniqueID] = value;
            }
        }

        //Current gridview pageindex
        public int CurrentPageIndex
        {
            get
            {
                if (ViewState["PageIndex"] == null)
                    ViewState["PageIndex"] = 0;
                return (int)ViewState["PageIndex"];
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }

        //Current sort type
        public string SortType
        {
            get
            {
                if (ViewState["SortType"] == null)
                {
                    ViewState["SortType"] = "A";
                }
                return ViewState["SortType"].ToString();
            }
            set
            {
                ViewState["SortType"] = value;
            }

        }

        //Current sort type
        public string SortColumn
        {
            get
            {
                if (ViewState["SortColumn"] == null)
                {
                    ViewState["SortColumn"] = "0";
                }
                return ViewState["SortColumn"].ToString();
            }
            set
            {
                ViewState["SortColumn"] = value;
            }
        }

        #endregion

        void SetDefaultAllCheck()
        {
            chkselall.Checked = true;
            List<int> lstDept = new List<int>();
            foreach (ListItem item in chkDepartment.Items)
            {
                item.Selected = true;
                lstDept.Add(int.Parse(item.Value));
            }
            BindData();

            CheckItemGridview();
        }

        private void SetData()
        {
            string _dataReceive = DataReceive;
            if (_dataReceive == null || string.IsNullOrEmpty(_dataReceive))
            {
                SetDefaultAllCheck();
                return;
            }

            if (_dataReceive.Contains("|"))
            {
                string[] arr = _dataReceive.Split('|');
                if (arr.Length > 1)
                {
                    string[] arrDepart = arr[0].Split(',');
                    if (arr[1].Length > 0)
                    {
                        List<string> lst = AllUserIds;
                        if (lst == null)
                            lst = arr[1].Split(',').ToList();
                        else
                            lst.AddRange(arr[1].Split(',').ToList());

                        AllUserIds = lst;
                    }

                    if (arrDepart != null && arrDepart.Length > 0)
                    {
                        foreach (string item in arrDepart)
                        {
                            if (chkDepartment.Items.FindByValue(item) != null)
                                chkDepartment.Items.FindByValue(item).Selected = true;
                        }

                        btnSelect_Click(null, new EventArgs());
                    }
                    else
                    {
                        SetDefaultAllCheck();
                    }
                }
            }
            /*
        else
        {
            string[] arrId = _dataReceive.Split(',');
            if (arrId != null && arrId.Length > 0)
            {
                foreach (string item in arrId)
                {
                    if (chkDepartment.Items.FindByValue(item) != null)
                        chkDepartment.Items.FindByValue(item).Selected = true;
                }
            }
            else
            {
                SetDefaultAllCheck();
            }
        }
             */
        }

        public void UncheckAllDepartment()
        {
            if (chkDepartment.Items != null && chkDepartment.Items.Count > 0)
            {
                foreach (ListItem item in chkDepartment.Items)
                    item.Selected = false;
            }
        }

        void CheckItemGridview()
        {
            List<string> allUserIds = AllUserIds;
            if (allUserIds != null && allUserIds.Count > 0)
            {
                List<string> allExceptUserIds = AllExceptUserIds;
                var dataDiff = allExceptUserIds != null ? allUserIds.Except(allExceptUserIds) : allUserIds;

                if (grvUserList.Rows.Count > 0)
                {
                    foreach (GridViewRow item in grvUserList.Rows)
                    {
                        HtmlInputCheckBox chkSelect = item.FindControl("chkSelect") as HtmlInputCheckBox;
                        if (chkSelect != null)
                        {
                            var found = dataDiff.Where(x => x == chkSelect.Value);
                            if (found != null && found.Count() > 0)
                                chkSelect.Checked = true;
                        }
                    }
                }
            }

        }

        void BindData()
        {
            try
            {
                List<int> lst = GetSelectedDepartment();
                if (lst != null && lst.Count > 0)
                {
                    DataTable allUser = RealtimeNotificationManager.GetUsersByDepartmentIDs(lst);
                    if (allUser != null && allUser.Rows.Count > 0)
                    {
                        List<int> allOldDept = AllOldDept;
                        var isDiff = allOldDept == null;
                        if (isDiff == false)
                            isDiff = (lst.Count() != allOldDept.Count()) || lst.Except(allOldDept).Any();
                        if (isDiff)
                        {
                            List<int> lstSave = RealtimeNotificationManager.GetAllStaffIdsByDepartmentIds(lst);
                            if (lstSave != null && lstSave.Count > 0)
                            {
                                var allUserIds = AllUserIds;
                                List<int> ignoreId = null;

                                var idChecked = allOldDept != null ? (lst.Count > allOldDept.Count ? lst.Except(allOldDept) : allOldDept.Except(lst)) : null;

                                if (idChecked != null && idChecked.Count() > 0)
                                {
                                    DataRow[] drSelected = allUser.Select(String.Format("departmentid='{0}'", idChecked.First()));
                                    if (drSelected != null && drSelected.Length > 0)
                                    {
                                        ignoreId = new List<int>();
                                        int idTemp = 0;
                                        foreach (DataRow dr in drSelected)
                                        {
                                            int.TryParse(dr["StaffID"].ToString(), out idTemp);
                                            if (idTemp > 0)
                                                ignoreId.Add(idTemp);
                                        }
                                    }
                                    if (ignoreId != null && ignoreId.Count > 0)
                                        allUserIds.AddRange(ignoreId.Select(x => x.ToString()));
                                }

                                if (allUserIds != null && lstSave.Count > allUserIds.Count)
                                {
                                    var dataEx = lstSave.Except(allUserIds.Select(x => int.Parse(x)));
                                    if (dataEx != null && dataEx.Any())
                                        AllExceptUserIds = dataEx.Select(x => x.ToString()).ToList();
                                }

                                AllUserIds = lstSave.Select(x => x.ToString()).ToList();
                            }
                            AllOldDept = lst;
                        }

                        grvUserList.VirtualItemCount = allUser.Rows.Count;

                        string sortType = SortType;

                        int oldColumnIndex = SortColumn == null ? 0 : int.Parse(SortColumn);
                        int columnIndex = int.Parse(SortColumn);
                        grvUserList.Columns[columnIndex].HeaderStyle.CssClass = sortType == "A" ? "sorting_desc" : "sorting_asc";
                        if (oldColumnIndex != columnIndex)
                            grvUserList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

                        string exp = string.Empty;
                        switch (columnIndex)
                        {
                            case 0:
                                exp = "StaffID";
                                break;
                            case 1:
                                exp = "FirstName";
                                break;
                            case 2:
                                exp = "LastName";
                                break;
                            case 3:
                                exp = "Email";
                                break;
                            default:
                                break;
                        }

                        //Sort the data.
                        allUser.DefaultView.Sort = String.Format("{0} {1}", exp, (sortType == "A" ? "ASC" : "DESC"));


                        grvUserList.DataSource = allUser;
                        grvUserList.DataBind();
                        //grvUserList.PageIndex = CurrentPageIndex;
                    }

                    Data = allUser;
                }
                else
                {
                    grvUserList.DataSource = null;
                    grvUserList.DataBind();
                    CurrentPageIndex = 0;
                    AllUserIds = null;
                    AllExceptUserIds = null;
                    AllOldDept = null;
                    grvUserList.PageIndex = CurrentPageIndex;
                }
            }
            catch (Exception ex)
            {
            }
        }

        void SaveExceptUser()
        {
            if (grvUserList.Rows.Count > 0)
            {
                List<string> allExceptUserIds = AllExceptUserIds;
                if (allExceptUserIds == null)
                    allExceptUserIds = new List<string>();

                foreach (GridViewRow item in grvUserList.Rows)
                {
                    using (HtmlInputCheckBox chkSelect = item.FindControl("chkSelect") as HtmlInputCheckBox)
                    {
                        if (chkSelect != null)
                        {
                            if (chkSelect.Checked == false)
                            {
                                if (allExceptUserIds.Contains(chkSelect.Value) == false)
                                    allExceptUserIds.Add(chkSelect.Value);
                            }
                            else
                            {
                                if (allExceptUserIds.Contains(chkSelect.Value))
                                    allExceptUserIds.Remove(chkSelect.Value);
                            }
                        }
                    }
                }

                if (allExceptUserIds != null && allExceptUserIds.Count > 0)
                    AllExceptUserIds = allExceptUserIds;
            }
        }

        protected void grvUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveExceptUser();
            CurrentPageIndex = e.NewPageIndex;
            grvUserList.PageIndex = CurrentPageIndex;
            BindData();
            CheckItemGridview();
        }

        protected void grvUserList_Sorting(object sender, GridViewSortEventArgs e)
        {
            SaveExceptUser();
            DataTable dt = Data;
            if (dt != null && dt.Rows.Count > 0)
            {
                string sortType = SortType;

                if (sortType == "A")
                {
                    SortType = "D";
                }
                else
                {
                    SortType = "A";
                }

                //int oldColumnIndex = SortColumn == null ? 0 : int.Parse(SortColumn);
                //SortColumn = e.SortExpression;
                //int columnIndex = int.Parse(e.SortExpression);
                //grvUserList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
                //if (oldColumnIndex != columnIndex)
                //    grvUserList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

                int oldColumnIndex = SortColumn == null ? 0 : int.Parse(SortColumn);
                SortColumn = e.SortExpression.Length > 0 ? e.SortExpression : "0";
                int columnIndex = int.Parse(SortColumn);
                grvUserList.Columns[columnIndex].HeaderStyle.CssClass = sortType == "A" ? "sorting_desc" : "sorting_asc";
                if (oldColumnIndex != columnIndex)
                    grvUserList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";

                string exp = string.Empty;
                switch (columnIndex)
                {
                    case 0:
                        exp = "StaffID";
                        break;
                    case 1:
                        exp = "FirstName";
                        break;
                    case 2:
                        exp = "LastName";
                        break;
                    case 3:
                        exp = "Email";
                        break;
                    default:
                        break;
                }

                //Sort the data.
                dt.DefaultView.Sort = String.Format("{0} {1}", exp, (sortType == "A" ? "ASC" : "DESC"));

                grvUserList.DataSource = dt;
                grvUserList.DataBind();
                CheckItemGridview();
            }
        }

        public List<object> GetDataReceiver()
        {
            List<object> lstReturn = new List<object>();
            List<int> lst = AllOldDept;
            if (lst != null && lst.Count > 0)
            {
                if (grvUserList.Visible && grvUserList.Rows.Count > 0)
                {
                    SaveExceptUser();
                    List<string> allUserIds = AllUserIds;
                    List<string> allExceptUserIds = AllExceptUserIds;
                    var dataDiff = allExceptUserIds != null ? allUserIds.Except(allExceptUserIds) : allUserIds;
                    lstReturn.Add(String.Format("{0}|{1}", 
                        string.Join(",", lst.Select(x => x.ToString()).ToArray()), 
                        string.Join(",", dataDiff.Select(x => x.ToString()).ToArray())));
                    lstReturn.Add(NotificationType.Staff);
                }
            }
            return lstReturn;
        }

        List<int> GetSelectedDepartment()
        {
            List<int> lst = new List<int>();
            if (chkDepartment.Items != null && chkDepartment.Items.Count > 0)
            {
                int id = 0;
                foreach (ListItem item in chkDepartment.Items)
                {
                    if (item.Selected)
                    {
                        int.TryParse(item.Value, out id);
                        if (id > 0 && lst.Contains(id) == false)
                            lst.Add(id);
                    }
                }
            }
            return lst;
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            SaveExceptUser();
            grvUserList.Visible = true;
            BindData();
            CheckItemGridview();
        }
    }
}