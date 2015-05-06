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
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class User : ModalBasePage
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
                BindDepartmentDDL();
                BindRoleList();
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                if (Request.QueryString["ID"] != null)
                {
                    BindUserData();
                }
                else
                {
                    ResetDataFields();
                }
            }
            else
            {
                ResetControlStyle();
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {

        }

        //Đổ dữ liệu vào dropdownlist Store
        private void BindDepartmentDDL()
        {
            TblDepartmentCollection sourcv = DepartmentManager.ListForDDL();
            ddlDepartment.DataSource = sourcv;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();
        }

        //Bind role list
        private void BindRoleList()
        {
            chkList.DataSource = RoleManager.RoleListForDLL();
            chkList.DataTextField = "RoleName";
            chkList.DataValueField = "RoleID";
            chkList.DataBind();
            ResetControlStyle();
        }

        private void BindCheckBoxList(int UserID)
        {
            List<int> RoleIDs = RoleManager.GetRoleIDsOfUser(UserID);
            foreach (ListItem li in chkList.Items)
            {
                int selectedID = int.Parse(li.Value);
                int exists = RoleIDs.FirstOrDefault(x => x == selectedID);
                if (exists != 0)
                    li.Selected = true;
            }
        }

        private void ResetControlStyle()
        {
            //checkboxlist
            foreach (ListItem li in chkList.Items)
            {
                li.Attributes.Add("class", "uniform");
            }
        }

        private void BindUserData()
        {
            try
            {
                int ID;
                if (int.TryParse(Request.QueryString["ID"], out ID))
                {
                    //Kiểm tra nhân viên còn tồn tại không
                    TblStaff obj = StaffManager.SelectByID(ID);
                    if (obj != null)
                    {
                        txtStaffNo.Text = obj.StaffNo;
                        txtFirstName.Text = obj.FirstName;
                        txtLastName.Text = obj.LastName;
                        txtTelNumber.Text = obj.TelNumber;
                        txtEmail.Text = obj.Email;
                        txtAddress.Text = obj.Address;
                        ddlDepartment.SelectedValue = obj.DepartmentID == null ? "0" : obj.DepartmentID.ToString();
                        chkObsolete.Checked = obj.IsObsolete;
                        chkHasAccount.Checked = obj.HasAccount;
                        BindCheckBoxList(obj.StaffID);
                        DisableAccount(!obj.HasAccount);
                        if (obj.HasAccount)
                        {
                            TblUser uObj = UserManager.SelectUserByID(ID);
                            if (uObj != null)
                            {
                                txtUsername.ReadOnly = true;
                                txtUsername.Text = uObj.UserName;
                                txtDisplayName.Text = uObj.DisplayName;
                            }
                        }
                        btnSave.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.SAVE);
                        Session[ViewState["PageID"] + "ID"] = obj.StaffID;
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_USER), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        ResetDataFields();
                    }
                }
                else
                {
                    ResetDataFields();
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void ResetDataFields()
        {
            //Staff
            txtStaffNo.Text = StaffManager.StaffNo();
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtTelNumber.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            ddlDepartment.SelectedIndex = 0;
            chkObsolete.Checked = false;
            chkHasAccount.Checked = true;

            //User
            txtUsername.ReadOnly = false;
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtRepass.Text = "";

            DisableAccount(false);

            Session[ViewState["PageID"] + "ID"] = null;
            btnSave.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.ADD);
        }

        private bool AdminRoleIsSelected()
        {
            try
            {
                return chkList.Items.FindByText("Administration").Selected;
            }
            catch
            {
                return false;
            }
        }

        private void SaveUser()
        {
            try
            {
                //-------BEGIN VALIDATION
                int ID = Session[ViewState["PageID"] + "ID"] == null ? 0 : int.Parse(Session[ViewState["PageID"] + "ID"].ToString());
                ///StaffNo
                if (string.IsNullOrEmpty(txtStaffNo.Text.Trim()))
                    AddErrorPrompt(txtStaffNo.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///FirstName
                if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                    AddErrorPrompt(txtFirstName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///LastName
                if (string.IsNullOrEmpty(txtLastName.Text.Trim()))
                    AddErrorPrompt(txtLastName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ///Email
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                    AddErrorPrompt(txtEmail.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else if (UserManager.EmailExists(ID, txtEmail.Text.Trim()))
                    AddErrorPrompt(txtEmail.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EMAIL_EXISTS));

                //Lấy thông tin acccount hiện tại
                TblUser user = UserManager.SelectUserByID(ID);
                if (chkHasAccount.Checked)
                {
                    ///UserName
                    if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
                        AddErrorPrompt(txtUsername.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                    else if (UserManager.UserNameExists(ID, txtUsername.Text.Trim()))
                        AddErrorPrompt(txtUsername.ClientID, ResourceTextManager.GetApplicationText(ResourceText.USER_NAME_EXISTS));

                    if (Session[ViewState["PageID"] + "ID"] == null)
                    {
                        ///Password
                        if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                            AddErrorPrompt(txtPassword.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                        ///RePassword
                        if (string.IsNullOrEmpty(txtRepass.Text.Trim()))
                            AddErrorPrompt(txtRepass.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                        if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && txtPassword.Text.Trim().Length < 6)
                            AddErrorPrompt(txtPassword.ClientID, ResourceTextManager.GetApplicationText(ResourceText.PASSWORD_LENGTH));
                    }


                    if (txtPassword.Text.Trim() != txtRepass.Text.Trim())
                    {
                        AddErrorPrompt(txtPassword.ClientID, ResourceTextManager.GetApplicationText(ResourceText.PASSWORD_DOES_NOT_MATCH));
                        AddErrorPrompt(txtRepass.ClientID, ResourceTextManager.GetApplicationText(ResourceText.PASSWORD_DOES_NOT_MATCH));
                    }
                }
                else
                {
                    //Nếu bỏ chọn hasAccount cho tài khoản admin thì thông báo lỗi
                    if (user != null)
                    {
                        if (user.UserName == "administrator")
                            AddErrorPrompt(txtUsername.ClientID, "You are not allowed to edit this user.");
                    }
                }

                ///Email
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                    AddErrorPrompt(txtEmail.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                else if (UserManager.EmailExists(ID, txtEmail.Text.Trim()))
                    AddErrorPrompt(txtEmail.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EMAIL_EXISTS));
                //-------END VALIDATION


                if (IsValid)
                {
                    //Kiểm tra xem quyền Admin có được chọn không và người thêm/sửa người dùng có phải admin không?
                    if (AdminRoleIsSelected() && !ApplicationContext.Current.IsAdministrator)
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CANNOT_ASSIGN_ADMIN_ROLE), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                        return;
                    }
                    if (user != null)
                    {
                        if (user.UserName == "administrator" && ApplicationContext.Current.UserName != "administrator")
                        {
                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CANNOT_EDIT_USER), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msg, null, false, false);
                            return;
                        }
                    }
                    //LƯU NGƯỜI DÙNG---------------------------------------------------------------
                    TblStaff obj = new TblStaff();
                    if (Session[ViewState["PageID"] + "ID"] != null)//Edit
                    {
                        //Kiểm tra quyền
                        if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                        {
                            MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msgRole, null, false, false);
                            return;
                        }


                        obj = StaffManager.SelectByID(int.Parse(Session[ViewState["PageID"] + "ID"].ToString()));
                        if (obj == null)//Nếu không tồn tại người dùng thì thông báo lỗi
                        {
                            MessageBox errMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_USER), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(errMsg, null, false, false);
                            BindUserData();
                            return;
                        }

                        //obj.StaffNo = txtStaffNo.Text.Trim();
                        obj.FirstName = txtFirstName.Text.Trim();
                        obj.LastName = txtLastName.Text.Trim();
                        obj.TelNumber = txtTelNumber.Text.Trim();
                        obj.Email = txtEmail.Text.Trim().Trim();
                        obj.Address = txtAddress.Text.Trim();
                        obj.DepartmentID = ddlDepartment.SelectedValue == "0" ? (short?)null : short.Parse(ddlDepartment.SelectedValue);
                        obj.IsObsolete = chkObsolete.Checked;
                        obj.HasAccount = chkHasAccount.Checked;

                        obj = StaffManager.Update(obj);

                        if (chkHasAccount.Checked)
                        {
                            TblUser uObj = new TblUser();
                            uObj = UserManager.SelectUserByID(obj.StaffID);
                            if (uObj != null)//Update
                            {
                                if (UserManager.IsAdministrator(uObj.UserName))//Nếu thông tin người dùng được sửa thuộc quyền quản trị
                                {
                                    //Nếu người sửa thông tin không thuộc quyền quản trị thì báo lỗi
                                    if (!ApplicationContext.Current.IsAdministrator)
                                    {
                                        MessageBox erMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CANNOT_EDIT_USER), MSGButton.OK, MSGIcon.Error);
                                        OpenMessageBox(erMsg, null, false, false);
                                        BindUserData();
                                        return;
                                    }
                                }

                                //uObj.UserName = txtUsername.Text.Trim();
                                //uObj.UserID = obj.StaffID;
                                uObj.DisplayName = txtDisplayName.Text;
                                uObj.Password = txtPassword.Text.Trim();
                                uObj.Email = txtEmail.Text;
                                uObj.IsObsolete = chkObsolete.Checked;

                                if (UserManager.Update(uObj, "") == true)//Lưu người dùng
                                {
                                    //LƯU NGƯỜI DÙNG---------------------------------------------------------------
                                    //THÊM QUYỀN-------------------------------------------------------------------
                                    //Xóa quyền người dùng
                                    RoleManager.RemoveUsersRoles(uObj.UserID, uObj.UserName);
                                    foreach (ListItem li in chkList.Items)
                                    {
                                        if (li.Selected)
                                        {
                                            int RoleID = int.Parse(li.Value);
                                            string RoleName = li.Text;
                                            RoleManager.AddUserToRole(uObj.UserID, uObj.UserName, RoleID, RoleName);
                                        }
                                    }

                                }
                            }
                            else//Insert
                            {
                                uObj = new TblUser();
                                uObj.UserID = obj.StaffID;
                                uObj.UserName = txtUsername.Text.Trim();
                                uObj.DisplayName = txtDisplayName.Text;
                                uObj.Password = txtPassword.Text.Trim();
                                uObj.Email = txtEmail.Text;
                                uObj.IsObsolete = chkObsolete.Checked;

                                MembershipCreateStatus createStatus = MembershipCreateStatus.Success;
                                UserManager.Insert(uObj, out uObj, out createStatus);
                                if (createStatus == MembershipCreateStatus.Success)//Lưu thành công
                                {
                                    //LƯU NGƯỜI DÙNG---------------------------------------------------------------
                                    //THÊM QUYỀN-------------------------------------------------------------------
                                    foreach (ListItem li in chkList.Items)
                                    {
                                        if (li.Selected)
                                        {
                                            int RoleID = int.Parse(li.Value);
                                            string RoleName = li.Text;
                                            RoleManager.AddUserToRole(uObj.UserID, uObj.UserName, RoleID, RoleName);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            UserManager.Delete(obj.StaffID);
                        }


                        Session[ViewState["PageID"] + "ID"] = obj.StaffID;
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.USER_SAVED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                    }
                    else//Add new
                    {
                        //Kiểm tra quyền
                        if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                        {
                            MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msgRole, null, false, false);
                            return;
                        }

                        obj = new TblStaff();
                        do
                        {
                            txtStaffNo.Text = StaffManager.StaffNo();
                            obj.StaffNo = txtStaffNo.Text.Trim();
                        } while (StaffManager.StaffNoExists(0, txtStaffNo.Text.Trim()));
                        obj.FirstName = txtFirstName.Text.Trim();
                        obj.LastName = txtLastName.Text.Trim();
                        obj.TelNumber = txtTelNumber.Text.Trim();
                        obj.Email = txtEmail.Text.Trim().Trim();
                        obj.Address = txtAddress.Text.Trim();
                        obj.DepartmentID = ddlDepartment.SelectedValue == "0" ? (short?)null : short.Parse(ddlDepartment.SelectedValue);
                        obj.IsObsolete = chkObsolete.Checked;
                        obj.HasAccount = chkHasAccount.Checked;

                        obj = StaffManager.Insert(obj);

                        if (obj != null)//Nếu thêm nhân viên thành công thì thêm tài khoản người dùng
                        {
                            if (chkHasAccount.Checked)
                            {
                                TblUser uObj = new TblUser();
                                uObj.UserID = obj.StaffID;
                                uObj.UserName = txtUsername.Text.Trim();
                                uObj.DisplayName = txtDisplayName.Text;
                                uObj.Password = txtPassword.Text.Trim();
                                uObj.Email = txtEmail.Text;
                                uObj.IsObsolete = chkObsolete.Checked;

                                MembershipCreateStatus createStatus = MembershipCreateStatus.Success;
                                UserManager.Insert(uObj, out uObj, out createStatus);

                                //add notification
                                RealtimeNotificationManager.AddUsersToReceiveList(new TblStaffCollection() { obj });

                                if (createStatus == MembershipCreateStatus.Success)//Lưu thành công
                                {
                                    //LƯU NGƯỜI DÙNG---------------------------------------------------------------
                                    //THÊM QUYỀN-------------------------------------------------------------------
                                    foreach (ListItem li in chkList.Items)
                                    {
                                        if (li.Selected)
                                        {
                                            int RoleID = int.Parse(li.Value);
                                            string RoleName = li.Text;
                                            RoleManager.AddUserToRole(uObj.UserID, uObj.UserName, RoleID, RoleName);
                                        }
                                    }
                                }
                            }
                        }


                        Session[ViewState["PageID"] + "ID"] = obj.StaffID;
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.USER_SAVED_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                    }
                }
                if (!IsValid)
                    ShowErrorPrompt();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void DisableAccount(bool Disable)
        {
            txtUsername.ReadOnly = Disable;
            txtDisplayName.ReadOnly = Disable;
            txtPassword.ReadOnly = Disable;
            txtRepass.ReadOnly = Disable;
            chkList.Enabled = !Disable;
        }

        protected void chkHasAccount_CheckedChanged(object sender, EventArgs e)
        {
            DisableAccount(!chkHasAccount.Checked);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/UserList.aspx");
        }
    }
}