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
    public partial class UserList : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "user_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                grvUserList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvUserList.Columns[0].HeaderText = "Staff No";
            grvUserList.Columns[1].HeaderText = "Fullname";
            grvUserList.Columns[2].HeaderText = "Email";
            grvUserList.Columns[3].HeaderText = "Department";
            grvUserList.Columns[4].HeaderText = ResourceTextManager.GetApplicationText("Obsolete");
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                DataTable dt = StaffManager.SelectAll("", null, 0, CurrentPageIndex, grvUserList.PageSize, SortColumn, SortType);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvUserList.VirtualItemCount = totalRows;
                    grvUserList.DataSource = dt;
                    grvUserList.DataBind();
                    grvUserList.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            btnDelete.Visible = true;

            CurrentPageIndex = e.NewPageIndex;
            grvUserList.PageIndex = CurrentPageIndex;
            BindData();
        }

        protected void grvUserList_Sorting(object sender, GridViewSortEventArgs e)
        {
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
            grvUserList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvUserList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

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
            for (int i = 0; i < grvUserList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvUserList.Rows[i].FindControl("chkIsDelete");
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
                result.Value = "User_Delete";
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

        //Xóa dữ liệu được chọn
        private string removeSelectedRows(List<int> idList)
        {
            string cannotDeletedList = "";
            foreach (int ID in idList)
            {
                if (!StaffManager.IsBeingUsed(ID) && !UserManager.IsAdministrator(ID))
                {
                    StaffManager.Delete(ID);

                    //remove from notification list
                    RealtimeNotificationManager.RemoveUsersFromReceiveList(new List<int>() { ID });
                }
                else
                {
                    DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                    string roleName = dt.AsEnumerable().Where(x => x.Field<int>("StaffID") == ID).Select(x => x.Field<string>("FullName")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", roleName);
                }
            }
            if (!string.IsNullOrEmpty(cannotDeletedList))
            {
                cannotDeletedList = cannotDeletedList.Substring(0, cannotDeletedList.Length - 2);
            }
            return cannotDeletedList;
        }

        public override void ConfirmRequest(ModalConfirmResult e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Submit && e.Value != null)
                    {
                        if (e.Value.ToString().Equals("User_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<int> idList = new List<int>();
                            for (int i = 0; i < grvUserList.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)grvUserList.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    int ID = Convert.ToInt32(grvUserList.DataKeys[i].Value);
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
                                string message = string.Format("Cannot delete following staff(s) because data is being use: <br/>{0}", DataCannotDelete);
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

        protected void grvUserList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Pages/User.aspx?ID=" + ID.ToString());
            }
        }
    }
}