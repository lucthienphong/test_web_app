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

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class refProgressCylinderStatus : ModalBasePage
    {

        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "progress_cylinder_status_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                gvProgressCylinderStatus.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        private bool _addNew { get; set; }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            gvProgressCylinderStatus.Columns[0].HeaderText = ResourceTextManager.GetApplicationText("PROGRESS_CYLINDER_STATUS_NAME");
            gvProgressCylinderStatus.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.DESCRIPTION);
            gvProgressCylinderStatus.Columns[2].HeaderText = ResourceTextManager.GetApplicationText("Obsolete");
        }

        public override void BindData()
        {
            try
            {
                ProgressCylinderStatusManager cr = new ProgressCylinderStatusManager();
                int totalRows = 0;
                DataTable dt = cr.SelectAll("", null, CurrentPageIndex, gvProgressCylinderStatus.PageSize, SortColumn, SortType, false);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvProgressCylinderStatus.VirtualItemCount = totalRows;
                    gvProgressCylinderStatus.DataSource = dt;
                    gvProgressCylinderStatus.DataBind();
                    gvProgressCylinderStatus.PageIndex = CurrentPageIndex;
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
                ProgressCylinderStatusManager cr = new ProgressCylinderStatusManager();
                int totalRows = 0;
                DataTable dt = cr.SelectAll("", null, CurrentPageIndex, gvProgressCylinderStatus.PageSize, SortColumn, SortType, AddNew);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvProgressCylinderStatus.VirtualItemCount = totalRows;
                    gvProgressCylinderStatus.DataSource = dt;
                    gvProgressCylinderStatus.DataBind();
                    gvProgressCylinderStatus.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void gvProgressCylinderStatus_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvProgressCylinderStatus.EditIndex = -1;
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
            gvProgressCylinderStatus.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvProgressCylinderStatus.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void gvProgressCylinderStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProgressCylinderStatus.EditIndex = -1;
            btnAdd.Visible = true;
            btnDelete.Visible = true;

            CurrentPageIndex = e.NewPageIndex;
            gvProgressCylinderStatus.PageIndex = CurrentPageIndex;
            BindData();
        }



        //ADD, EDIT, DELETE DEPARTMENT
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        private void BindGrid()
        {
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            gvProgressCylinderStatus.DataSource = dt;
            gvProgressCylinderStatus.DataBind();
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
            } while (dt.AsEnumerable().Where(x => x.Field<short>("CylinderStatusID") == randID).Count() > 0);

            dr["CylinderStatusID"] = randID;
            dr["CylinderStatusName"] = "";
            dr["IsObsolete"] = true;
            dr["RowsCount"] = 0;

            dt.Rows.InsertAt(dr, 0);

            //Update list
            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, short StatusID, string CylinderStatusName, string Description, bool isObsolete)
        {
            try
            {
                ProgressCylinderStatusManager s = new ProgressCylinderStatusManager();
                if (s.Exist(StatusID, CylinderStatusName))
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "A status with this information already exists in the system!", MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }

                TblProgressCylinderStatus obj = new TblProgressCylinderStatus();
                obj = s.SelectByID(StatusID);
                if (obj != null)
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    } 
                    obj.CylinderStatusName = CylinderStatusName;
                    obj.Description = Description;
                    obj.IsObsolete = isObsolete;
                    s.Update(obj);

                    //Lưu vào logging
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
                    obj = new TblProgressCylinderStatus();
                    obj.CylinderStatusName = CylinderStatusName;
                    obj.Description = Description;
                    obj.IsObsolete = isObsolete;
                    s.Insert(obj);

                    //Lưu vào logging
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
                if (string.IsNullOrEmpty(r["CylinderStatusName"].ToString()))
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
            ProgressCylinderStatusManager cr = new ProgressCylinderStatusManager();
            foreach (short ID in idList)
            {
                if (!cr.IsBeingUsed(ID))
                {
                    TblProgressCylinderStatus obj = cr.SelectByID(ID);
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
                    string CylinderStatusName = dt.AsEnumerable().Where(x => x.Field<short>("CylinderStatusID") == ID).Select(x => x.Field<string>("CylinderStatusName")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", CylinderStatusName);
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
            gvProgressCylinderStatus.EditIndex = 0;

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
            gvProgressCylinderStatus.EditIndex = -1;
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
            for (int i = 0; i < gvProgressCylinderStatus.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvProgressCylinderStatus.Rows[i].FindControl("chkIsDelete");
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
                result.Value = "ProgressStatus_Delete";
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
                        if (e.Value.ToString().Equals("ProgressStatus_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<short> idList = new List<short>();
                            for (int i = 0; i < gvProgressCylinderStatus.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)gvProgressCylinderStatus.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    short ID = Convert.ToInt16(gvProgressCylinderStatus.DataKeys[i].Value);
                                    idList.Add(ID);
                                }
                            }
                            string DataCannotDelete = removeSelectedRows(idList);
                            BindData();
                            if (string.IsNullOrEmpty(DataCannotDelete))
                            {
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

        protected void gvProgressCylinderStatus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            removeInvalidRows();
            gvProgressCylinderStatus.EditIndex = e.NewEditIndex;
            BindGrid();
            
            btnAdd.Visible = false;
            btnDelete.Visible = false;

            btnCancel.Visible = true;
            btnCancel.Enabled = true;
        }

        protected void gvProgressCylinderStatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            short ID = Convert.ToInt16(gvProgressCylinderStatus.DataKeys[e.RowIndex].Value);
            TextBox txtCylinderStatusName = (TextBox)gvProgressCylinderStatus.Rows[e.RowIndex].FindControl("txtCylinderStatusName");
            ProgressCylinderStatusManager cr = new ProgressCylinderStatusManager();
            if (string.IsNullOrEmpty(txtCylinderStatusName.Text))
            {
                AddErrorPrompt(txtCylinderStatusName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            if (cr.Exist(ID, txtCylinderStatusName.Text.Trim()))
            {
                AddErrorPrompt(txtCylinderStatusName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EXISTS));
            }

            if (IsValid)
            {
                TextBox txtDescription = (TextBox)gvProgressCylinderStatus.Rows[e.RowIndex].FindControl("txtDescription");
                CheckBox chkIsObsolete = (CheckBox)gvProgressCylinderStatus.Rows[e.RowIndex].FindControl("chkIsObsolete");
                string CylinderStatusName = txtCylinderStatusName.Text;

                string Description = txtDescription.Text.Trim();
                bool isObsolete = chkIsObsolete.Checked;
                updateRow(e.RowIndex, ID, CylinderStatusName, Description, isObsolete);
                gvProgressCylinderStatus.EditIndex = -1;
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

        protected void gvProgressCylinderStatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //removeInvalidRows();
            gvProgressCylinderStatus.EditIndex = -1;
            BindData();

            btnAdd.Visible = true;
            btnDelete.Visible = true;

            btnCancel.Visible = false;
        }

        protected void gvProgressCylinderStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion

        protected void gvProgressCylinderStatus_RowCreated(object sender, GridViewRowEventArgs e)
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