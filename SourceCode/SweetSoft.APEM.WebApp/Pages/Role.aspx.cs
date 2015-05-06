using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
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
    public partial class Role : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "role_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                grvRoleList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvRoleList.Columns[0].HeaderText = "Role";
            grvRoleList.Columns[1].HeaderText = "Description";
            grvRoleList.Columns[2].HeaderText = ResourceTextManager.GetApplicationText("Obsolete");
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                DataTable dt = RoleManager.SelectAll("", null, CurrentPageIndex, grvRoleList.PageSize, SortColumn, SortType, false);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvRoleList.VirtualItemCount = totalRows;
                    grvRoleList.DataSource = dt;
                    grvRoleList.DataBind();
                    grvRoleList.PageIndex = CurrentPageIndex;
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
                int totalRows = 0;
                DataTable dt = RoleManager.SelectAll("", null, CurrentPageIndex, grvRoleList.PageSize, SortColumn, SortType, AddNew);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvRoleList.VirtualItemCount = totalRows;
                    grvRoleList.DataSource = dt;
                    grvRoleList.DataBind();
                    grvRoleList.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvRoleList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grvRoleList_Sorting(object sender, GridViewSortEventArgs e)
        {
            grvRoleList.Columns[3].Visible = true;
            grvRoleList.EditIndex = -1;
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
            grvRoleList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvRoleList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void grvRoleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvRoleList.Columns[3].Visible = true;
            grvRoleList.EditIndex = -1;
            btnAdd.Visible = true;
            btnDelete.Visible = true;

            CurrentPageIndex = e.NewPageIndex;
            grvRoleList.PageIndex = CurrentPageIndex;
            BindData();
        }

        //ADD, EDIT, DELETE ROLE
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        private void BindGrid()
        {
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            grvRoleList.DataSource = dt;
            grvRoleList.DataBind();
        }

        /// <summary>
        /// Thêm dòng mới vào datagridview => Không sử dụng
        /// </summary>
        private void addRow()
        {
            //Get list of role
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            DataRow dr = dt.NewRow();

            Random rnd = new Random();
            int randID = 0;
            do
            {
                randID = (int)rnd.Next(-10000, -1);
            } while (dt.AsEnumerable().Where(x => x.Field<int>("RoleID") == randID).Count() > 0);

            dr["RoleID"] = randID;
            dr["RoleName"] = "";
            dr["Description"] = "";
            dr["IsObsolete"] = false;

            dt.Rows.InsertAt(dr, 0);

            //Update list
            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, int roleID, string roleName, string Description, bool isObsolete)
        {
            try
            {
                TblRole obj = new TblRole();
                obj = RoleManager.SelectByID(roleID);
                if (obj != null)
                {
                    //Kiểm tra quyền
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }

                    if (RoleManager.Exists(roleID, roleName))
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "This role exists!", MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        return;
                    }

                    if (obj.RoleName == "Administration" && !ApplicationContext.Current.IsAdministrator)
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CANNOT_EDIT_ADMIN_ROLE), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        return;
                    }

                    obj.RoleName = roleName;
                    obj.Description = Description;
                    obj.IsObsolete = isObsolete;
                    RoleManager.Update(obj);
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

                    if (RoleManager.Exists(roleName))
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.ROLE_EXISTS), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        return;
                    }

                    obj = new TblRole();
                    obj.RoleName = roleName;
                    obj.Description = Description;
                    obj.IsObsolete = isObsolete;
                    RoleManager.Insert(obj);
                }
                if (obj != null)
                {
                    BindData();
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
            }
            catch(Exception ex)
            {
                ProcessException(ex);
            }
        }

        //Xóa dữ liệu không hợp lệ
        private void removeInvalidRows()
        {
            //Get list of role
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];

            List<DataRow> dr = new List<DataRow>();
            foreach (DataRow r in dt.Rows)
            {
                if (string.IsNullOrEmpty(r["RoleName"].ToString()))
                    dr.Add(r);
            }
            foreach (DataRow r in dr)
                r.Delete();
            dt.AcceptChanges();

            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Xóa dữ liệu được chọn
        private string removeSelectedRows(List<int> idList)
        {
            string cannotDeletedList = "";
            foreach (int ID in idList)
            {
                if (!RoleManager.IsBeingUsed(ID) && !RoleManager.IsAdministration(ID))
                    RoleManager.Delete(ID);
                else
                {
                    DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                    string roleName = dt.AsEnumerable().Where(x => x.Field<int>("RoleID") == ID).Select(x => x.Field<string>("RoleName")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", roleName);
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
            grvRoleList.Columns[3].Visible = false;
            grvRoleList.EditIndex = 0;
            BindData(true);

            btnAdd.Visible = false;
            btnDelete.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //Kiểm tra quyền
                if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }

                bool HasDataToDelete = false;
                for (int i = 0; i < grvRoleList.Rows.Count; i++)
                {
                    CheckBox chkIsDelete = (CheckBox)grvRoleList.Rows[i].FindControl("chkIsDelete");
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
                    result.Value = "Role_Delete";
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
            catch (Exception ex)
            {
                ProcessException(ex);
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
                        if (e.Value.ToString().Equals("Role_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<int> idList = new List<int>();
                            for (int i = 0; i < grvRoleList.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)grvRoleList.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    int ID = Convert.ToInt32(grvRoleList.DataKeys[i].Value);
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
                                string message = string.Format("Cannot delete because data is being use: {0}", DataCannotDelete);
                                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), message, MSGButton.OK, MSGIcon.Warning);
                                OpenMessageBox(msg, null, false, false);
                            }
                        }
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_DIRECTION), MSGButton.OK, MSGIcon.Error);
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

        protected void grvRoleList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvRoleList.Columns[3].Visible = false;
            removeInvalidRows();
            grvRoleList.EditIndex = e.NewEditIndex;
            BindGrid();

            btnAdd.Visible = false;
            btnDelete.Visible = false;
        }

        protected void grvRoleList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(grvRoleList.DataKeys[e.RowIndex].Value);
            TextBox txtRoleName = (TextBox)grvRoleList.Rows[e.RowIndex].FindControl("txtRoleName");
            if (string.IsNullOrEmpty(txtRoleName.Text))
            {
                AddErrorPrompt(txtRoleName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else if (RoleManager.Exists(ID, txtRoleName.Text.Trim()))
            {
                AddErrorPrompt(txtRoleName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EXISTS));
            }
            if (IsValid)
            {
                TextBox txtDescription = (TextBox)grvRoleList.Rows[e.RowIndex].FindControl("txtDescription");
                CheckBox chkIsObsolete = (CheckBox)grvRoleList.Rows[e.RowIndex].FindControl("chkIsObsolete");
                string roleName = txtRoleName.Text;
                string description = txtDescription.Text.Trim();
                bool isObsolete = chkIsObsolete.Checked;
                updateRow(e.RowIndex, ID, roleName, description, isObsolete);
                grvRoleList.Columns[3].Visible = true;
                grvRoleList.EditIndex = -1;
                BindData();

                btnAdd.Visible = true;
                btnDelete.Visible = true;
            }
            if (!IsValid)
            {
                BindGrid();
                ShowErrorPrompt();
            }
        }

        protected void grvRoleList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //removeInvalidRows();
            grvRoleList.Columns[3].Visible = true;
            grvRoleList.EditIndex = -1;
            BindData();

            btnAdd.Visible = true;
            btnDelete.Visible = true;
        }

        protected void grvRoleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
        #endregion
}