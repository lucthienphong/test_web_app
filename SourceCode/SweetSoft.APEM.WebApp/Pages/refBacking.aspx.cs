using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class refBacking : ModalBasePage
    {

        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "backing_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                gvBacking.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        private bool _addNew { get; set; }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            gvBacking.Columns[0].HeaderText = ResourceTextManager.GetApplicationText("BACKING_NAME");
            gvBacking.Columns[1].HeaderText = ResourceTextManager.GetApplicationText("Obsolete");
        }

        public override void BindData()
        {
            try
            {
                BackingManager cr = new BackingManager();
                int totalRows = 0;
                DataTable dt = cr.SelectAll("", null, CurrentPageIndex, gvBacking.PageSize, SortColumn, SortType, false);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvBacking.VirtualItemCount = totalRows;
                    gvBacking.DataSource = dt;
                    gvBacking.DataBind();
                    gvBacking.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void BindData(bool AddNew)
        {
            try
            {
                BackingManager cr = new BackingManager();
                int totalRows = 0;
                DataTable dt = cr.SelectAll("", null, CurrentPageIndex, gvBacking.PageSize, SortColumn, SortType, AddNew);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvBacking.VirtualItemCount = totalRows;
                    gvBacking.DataSource = dt;
                    gvBacking.DataBind();
                    gvBacking.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void gvBacking_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvBacking.EditIndex = -1;
            btnAdd.Visible = true;
            btnDelete.Visible = true;
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
            gvBacking.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvBacking.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void gvBacking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBacking.EditIndex = -1;
            btnAdd.Visible = true;
            btnDelete.Visible = true;

            CurrentPageIndex = e.NewPageIndex;
            gvBacking.PageIndex = CurrentPageIndex;
            BindData();
        }



        //ADD, EDIT, DELETE DEPARTMENT
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        private void BindGrid()
        {
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            gvBacking.DataSource = dt;
            gvBacking.DataBind();
        }

        /// <summary>
        /// Thêm dòng mới vào datagridview => Không sử dụng
        /// </summary>
        private void addRow()
        {
            //Get list of department
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            DataRow dr = dt.NewRow();

            Random rnd = new Random();
            short randID = 0;
            do
            {
                randID = (short)rnd.Next(-10000, -1);
            } while (dt.AsEnumerable().Where(x => x.Field<short>("BackingID") == randID).Count() > 0);

            dr["BackingID"] = randID;
            dr["BackingName"] = "";
            dr["IsObsolete"] = true;
            dr["RowsCount"] = 0;

            dt.Rows.InsertAt(dr, 0);

            //Update list
            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, short backingID, string backingName, bool isObsolete)
        {
            try
            {
                BackingManager s = new BackingManager();
                if (s.Exist(backingID, backingName))
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "A backing with this information already exists in the system!!", MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }

                TblBacking obj = new TblBacking();
                obj = s.SelectByID(backingID);
                if (obj != null)
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }
                    List<JsonData> lstData = new List<JsonData>();
                    if (obj.BackingName != backingName)
                    {
                        lstData.Add(new JsonData()
                        {
                            Title = "Backing Name",
                            Data = JsonConvert.SerializeObject(new Json()
                            {
                                OldValue = obj.BackingName,
                                NewValue = backingName
                            })
                        });
                    }
                    if (obj.IsObsolete != isObsolete)
                    {
                        lstData.Add(new JsonData()
                                {
                                    Title = "Obsolete",
                                    Data = JsonConvert.SerializeObject(new Json()
                                    {
                                        OldValue = obj.IsObsolete ? "True" : "False",
                                        NewValue = isObsolete ? "True" : "False"
                                    })
                                });
                    }
                    obj.BackingName = backingName;
                    obj.IsObsolete = isObsolete;
                    s.Update(obj);

                    //Lưu vào logging
                    LoggingActions("Backing",
                            LogsAction.Objects.Action.UPDATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(lstData));
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
                else
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }
                    obj = new TblBacking();
                    obj.BackingName = backingName;
                    obj.IsObsolete = isObsolete;
                    s.Insert(obj);

                    //Lưu vào logging
                    LoggingActions("Backing",
                            LogsAction.Objects.Action.CREATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Backing Name", Data = obj.BackingName} 
                            }));
                    LoggingManager.LogAction(ActivityLoggingHelper.INSERT, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
                if (obj != null)
                {
                    BindData();
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        //Xóa dữ liệu không hợp lệ
        private void removeInvalidRows()
        {
            //Get list of department
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];

            List<DataRow> dr = new List<DataRow>();
            foreach (DataRow r in dt.Rows)
            {
                if (string.IsNullOrEmpty(r["BackingName"].ToString()))
                    dr.Add(r);
            }
            foreach (DataRow r in dr)
                r.Delete();
            dt.AcceptChanges();

            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Xóa dữ liệu được chọn
        private string removeSelectedRows(List<short> idList)
        {
            string cannotDeletedList = "";
            BackingManager cr = new BackingManager();
            foreach (short ID in idList)
            {
                if (!cr.IsBeingUsed(ID))
                {
                    TblBacking obj = cr.SelectByID(ID);
                    if (obj != null)
                    {
                        cr.Delete(ID);

                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                    }

                }
                else
                {
                    DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                    string BackingName = dt.AsEnumerable().Where(x => x.Field<short>("BackingID") == ID).Select(x => x.Field<string>("BackingName")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", BackingName);
                }
            }
            if (!string.IsNullOrEmpty(cannotDeletedList))
            {
                cannotDeletedList = cannotDeletedList.Substring(0, cannotDeletedList.Length - 2);
            }
            return cannotDeletedList;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            removeInvalidRows();
            //addRow();
            gvBacking.EditIndex = 0;

            _addNew = true;
            BindData(true);

            btnAdd.Visible = false;
            btnDelete.Visible = false;

            btnCancel.Visible = true;
            btnCancel.Enabled = true;


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            btnCancel.Enabled = false;

            removeInvalidRows();
            gvBacking.EditIndex = -1;
            BindData();

            btnAdd.Visible = true;
            btnDelete.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Kiểm tra quyền
            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            bool HasDataToDelete = false;
            for (int i = 0; i < gvBacking.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvBacking.Rows[i].FindControl("chkIsDelete");
                if (chkIsDelete.Checked)//Nếu tồn tại dòng dữ liệu được check thì hiện thông báo xóa
                {
                    HasDataToDelete = true;
                    break;
                }
            }
            if (HasDataToDelete)
            {
                //Gọi message box
                ModalConfirmResult result = new ModalConfirmResult();
                result.Value = "Backing_Delete";
                CurrentConfirmResult = result;
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CONFIRM_TITLE), "Do you want to delete seleted rows?", MSGButton.DeleteCancel, MSGIcon.Error);
                OpenMessageBox(msg, result, false, false);
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Please choose data to delete.", MSGButton.OK, MSGIcon.Info);
                OpenMessageBox(msg, null, false, false);
            }
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        if (e.Value.ToString().Equals("Backing_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<short> idList = new List<short>();
                            List<JsonData> lstData = new List<JsonData>();

                            for (int i = 0; i < gvBacking.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)gvBacking.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    short ID = Convert.ToInt16(gvBacking.DataKeys[i].Value);
                                    TblBacking obj = new BackingManager().SelectByID(ID);
                                    if (obj != null)
                                    {
                                        lstData.Add(new JsonData() { Title = "Backing Name", Data = obj.BackingName });
                                    }
                                    idList.Add(ID);
                                }
                            }
                            string DataCannotDelete = removeSelectedRows(idList);
                            BindData();
                            if (string.IsNullOrEmpty(DataCannotDelete))
                            {
                                LoggingActions("Backing",
                                            LogsAction.Objects.Action.DELETE,
                                            LogsAction.Objects.Status.SUCCESS,
                                            JsonConvert.SerializeObject(lstData));
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data deleted susscessfully!", MSGButton.OK, MSGIcon.Success);
                                OpenMessageBox(msg, null, false, false);
                            }
                            else
                            {
                                string message = string.Format("Following data cannot delete because they are begin use: {0}", DataCannotDelete);
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Warning);
                                OpenMessageBox(msg, null, false, false);
                            }
                        }
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data is not found", MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        BindData();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void gvBacking_RowEditing(object sender, GridViewEditEventArgs e)
        {
            removeInvalidRows();
            gvBacking.EditIndex = e.NewEditIndex;
            BindGrid();

            btnAdd.Visible = false;
            btnDelete.Visible = false;

            btnCancel.Visible = true;
            btnCancel.Enabled = true;
        }

        protected void gvBacking_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            short ID = Convert.ToInt16(gvBacking.DataKeys[e.RowIndex].Value);
            TextBox txtBackingName = (TextBox)gvBacking.Rows[e.RowIndex].FindControl("txtBackingName");
            BackingManager cr = new BackingManager();
            if (string.IsNullOrEmpty(txtBackingName.Text))
            {
                AddErrorPrompt(txtBackingName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (cr.Exist(ID, txtBackingName.Text.Trim()))
            {
                AddErrorPrompt(txtBackingName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EXISTS));
            }

            if (IsValid)
            {
                CheckBox chkIsObsolete = (CheckBox)gvBacking.Rows[e.RowIndex].FindControl("chkIsObsolete");
                string BackingName = txtBackingName.Text;

                bool isObsolete = chkIsObsolete.Checked;
                updateRow(e.RowIndex, ID, BackingName, isObsolete);
                gvBacking.EditIndex = -1;
                BindData();

                btnAdd.Visible = true;
                btnDelete.Visible = true;
                btnCancel.Visible = false;
            }
            if (!IsValid)
            {
                BindGrid();
                ShowErrorPromptExtension();

            }


        }

        protected void gvBacking_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //removeInvalidRows();
            gvBacking.EditIndex = -1;
            BindData();

            btnAdd.Visible = true;
            btnDelete.Visible = true;

            btnCancel.Visible = false;
        }

        protected void gvBacking_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        #endregion

        protected void gvBacking_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (_addNew)
            {
                if (e.Row.RowIndex != 0)
                {
                    e.Row.Enabled = false;
                }
            }
        }
    }
}