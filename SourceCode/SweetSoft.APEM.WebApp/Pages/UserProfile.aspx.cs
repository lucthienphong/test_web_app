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
    public partial class UserProfile : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "user_manager";
            }
        }


        TblUser user { get; set; }
        TblStaff staff { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
            if (user != null)
                staff = StaffManager.SelectByID(user.UserID);

            if (!IsPostBack)
            {
                BindUserData();
            }

        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            
        }

        private void BindUserData()
        {
            try
            {
                txtUsername.ReadOnly = true;
                txtFirstName.ReadOnly = true;
                txtLastName.ReadOnly = true;

                
                if (user != null)
                {
                    txtUsername.Text = user.UserName;
                    txtDisplayName.Text = user.DisplayName;


                    if (staff != null && staff.HasAccount)
                    {
                        txtStaffNo.Text = staff.StaffNo;
                        txtFirstName.Text = staff.FirstName;
                        txtLastName.Text = staff.LastName;
                        txtTelNumber.Text = staff.TelNumber;
                        txtEmail.Text = staff.Email;
                        txtAddress.Text = staff.Address;
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        public bool Authenticate(string userName, string passWord)
        {
            return Membership.ValidateUser(userName, passWord);
        }

        private void ValidationForm()
        {
            if (user != null && staff != null)
            {
                bool error = true;
                //kiem tra user
                if (!Authenticate(user.UserName, txtCurrentPassword.Text))
                {
                    lblCrrPass.Text = ResourceTextManager.GetApplicationText(ResourceText.INVALID_CURRENT_PASSWORD);
                    error = false;
                }

                //kiem tra mat khau moi
                if (!string.IsNullOrEmpty(txtNewPassword.Text) || !string.IsNullOrEmpty(txtRePassword.Text))
                {
                    if (txtNewPassword.Text.Trim().Length < 6)
                    {
                        lblErrorNewPass.Visible = true;
                        lblErrorNewPass.Text = ResourceTextManager.GetApplicationText(ResourceText.PASSWORD_LENGTH);
                        error = false;
                    }
                }
                if (string.Compare(txtNewPassword.Text, txtRePassword.Text) != 0)
                {
                    lblRePass.Visible = true;
                    lblRePass.Text = ResourceTextManager.GetApplicationText(ResourceText.PASSWORD_DOES_NOT_MATCH);
                    error = false;
                }

                //Kiem tra email
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    lblErrorNewPass.Visible = true;
                    lblErrorEmail.Text = ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE);
                    error = false;
                }
                else
                    if (UserManager.EmailExists(user.UserID, txtEmail.Text.Trim()))
                    {
                        error = false;
                        lblErrorNewPass.Visible = true;
                        lblErrorEmail.Text = ResourceTextManager.GetApplicationText(ResourceText.EMAIL_EXISTS);
                    }
                    else
                    {
                        if (!UserManager.IsValidEmail(txtEmail.Text))
                        {
                            error = false;
                            lblErrorNewPass.Visible = true;
                            lblErrorEmail.Text = ResourceTextManager.GetApplicationText(ResourceText.EMAIL_INVALID);
                        }
                    }

                /*luu du lieu*/
                if (error)
                {
                    lblCrrPass.Visible = false;
                    lblErrorNewPass.Visible = false;
                    lblRePass.Visible = false;
                    lblErrorEmail.Visible = false;

                    if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
                    {
                        UserManager.ChangePassword(user, txtCurrentPassword.Text, txtNewPassword.Text);

                        if (AllowSaveLogging)
                            SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.CHANGE_PASSWORD), FUNCTION_PAGE, ResourceTextManager.GetApplicationText(ResourceText.CHANGE_PASSWORD_SUCCESSFULLY));

                    }

                    user.Email = txtEmail.Text;
                    user.DisplayName = txtDisplayName.Text;
                    UserManager.UpdateWithoutPassword(user);

                    staff.TelNumber = txtTelNumber.Text;
                    staff.Email = txtEmail.Text;
                    staff.Address = txtAddress.Text;
                    StaffManager.Update(staff);
                    if (AllowSaveLogging)
                        SaveLogging(ResourceTextManager.GetApplicationText(ResourceText.UPDATE_STAFF), FUNCTION_PAGE, staff.ToJSONString());

                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE),
                        ResourceTextManager.GetApplicationText(ResourceText.USER_INFORMATION_UPDATE_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
            }
        }

      

        private void SaveUser()
        {
            try
            {
                if (string.IsNullOrEmpty(txtDisplayName.Text.Trim()))
                    AddErrorPrompt(txtDisplayName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                if (string.IsNullOrEmpty(txtTelNumber.Text.Trim()))
                    AddErrorPrompt(txtTelNumber.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                    AddErrorPrompt(txtEmail.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
               
                if (IsValid)
                {
                    ValidationForm();
                    BindUserData();
                }

                if (!IsValid)
                    ShowErrorPrompt();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

    
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindUserData();
        }
    }
}