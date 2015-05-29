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
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class RolePermission : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "role_permission_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                if (Request.QueryString["ID"] != null)
                {
                    BindData();
                }
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvRolePermisstion.Columns[0].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.FUNCTION_NAME);
            grvRolePermisstion.Columns[1].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.CHECK_ALL);
            grvRolePermisstion.Columns[2].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ALLOW_ADD);
            grvRolePermisstion.Columns[3].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ALLOW_EDIT);
            grvRolePermisstion.Columns[4].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ALLOW_DELETE);
            grvRolePermisstion.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ALLOW_UPDATE_STATUS);
            grvRolePermisstion.Columns[6].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ALLOW_OTHER);
            grvRolePermisstion.Columns[7].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.ALLOW_LOCK_UNLOCK);
        }

        public override void BindData()
        {
            int ID = 0;
            if (int.TryParse(Request.QueryString["ID"], out ID))
            {
                TblRole obj = RoleManager.SelectByID(ID);
                if (obj != null)
                {
                    lbRoleName.Text = string.Format("Role: {0}", obj.RoleName);
                    DataTable dt = RoleManager.RolePermissionSelectByRoleID(ID);
                    grvRolePermisstion.DataSource = dt;
                    grvRolePermisstion.DataBind();
                    Session[ViewState["PageID"] + "ID"] = ID;
                }
                else
                {

                }
            }
            else
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_ROLE), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
            }
        }

        protected void grvRolePermisstion_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < grvRolePermisstion.Rows.Count; i++)
            {
                string parentsID = grvRolePermisstion.DataKeys[i].Values[1].ToString();
                if (string.IsNullOrEmpty(parentsID))
                {
                    GridViewRow row = grvRolePermisstion.Rows[i];
                    row.Cells[0].ColumnSpan = 8;
                    row.Cells[1].Visible = false;
                    row.Cells[2].Visible = false;
                    row.Cells[3].Visible = false;
                    row.Cells[4].Visible = false;
                    row.Cells[5].Visible = false;
                    row.Cells[6].Visible = false;
                    row.Cells[7].Visible = false;
                }
            }
        }

        private void SaveData()
        {
            try
            {
                if (Session[ViewState["PageID"] + "ID"] != null)
                {
                    int RoleID = int.Parse(Session[ViewState["PageID"] + "ID"].ToString());
                    TblRole obj = RoleManager.SelectByID(RoleID);
                    if (obj != null)
                    {
                        //Kiểm tra quyền
                        if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                        {
                            MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(msgRole, null, false, false);
                            return;
                        }

                        if (obj.RoleName == "Administration" && !ApplicationContext.Current.IsAdministrator)
                        {
                            MessageBox eMsg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CANNOT_EDIT_ADMIN_ROLE), MSGButton.OK, MSGIcon.Error);
                            OpenMessageBox(eMsg, null, false, false);
                            return;
                        }
                        //Xóa các quyền cũ
                        List<JsonData> lstJSData = new List<JsonData>();

                        for (int i = 0; i < grvRolePermisstion.Rows.Count; i++)
                        {
                            string functionID = grvRolePermisstion.DataKeys[i].Values[0].ToString();
                            string parentID = grvRolePermisstion.DataKeys[i].Values[1].ToString();
                            GridViewRow row = grvRolePermisstion.Rows[i];
                            if (!string.IsNullOrEmpty(parentID))
                            {
                                CheckBox chkAll = (CheckBox)row.FindControl("chkCheckAll");
                                CheckBox chkAdd = (CheckBox)row.FindControl("chkAllowAdd");
                                CheckBox chkEdit = (CheckBox)row.FindControl("chkAllowEdit");
                                CheckBox chkDelete = (CheckBox)row.FindControl("chkAllowDelete");
                                CheckBox chkViewPrice = (CheckBox)row.FindControl("AllowUpdateStatus");
                                CheckBox chkOther = (CheckBox)row.FindControl("chkAllowOther");
                                CheckBox chkLockUnlock = (CheckBox)row.FindControl("chkAllowLockUnlock");
                                CheckBox chkUnlock = (CheckBox)row.FindControl("chkAllowUnlock");
                                CheckBox chkEditUnlock = (CheckBox)row.FindControl("chkAllowEditUnlock");

                                Label lblTitle = (Label)row.FindControl("lblTitle");

                                TblRolePermission frObj = RoleManager.TblRolePermissionSelectBy(RoleID, functionID);
                                if (frObj != null)
                                { 
                                    AddJsonDataLog(frObj.AllowAdd, chkAdd, lblTitle.Text, ref lstJSData);
                                    AddJsonDataLog(frObj.AllowEdit, chkEdit, lblTitle.Text, ref lstJSData);
                                    AddJsonDataLog(frObj.AllowDelete, chkDelete, lblTitle.Text, ref lstJSData);
                                    AddJsonDataLog(frObj.AllowUpdateStatus, chkViewPrice, lblTitle.Text, ref lstJSData);
                                    AddJsonDataLog(frObj.AllowOther, chkOther, lblTitle.Text, ref lstJSData);
                                    AddJsonDataLog(frObj.AllowLockUnlock, chkLockUnlock, lblTitle.Text, ref lstJSData);
                                    
                                    frObj.RoleID = RoleID;
                                    frObj.FunctionID = functionID;
                                    frObj.AllowAdd = chkAdd.Checked;
                                    frObj.AllowEdit = chkEdit.Checked;
                                    frObj.AllowDelete = chkDelete.Checked;
                                    frObj.AllowUpdateStatus = chkViewPrice.Checked;
                                    frObj.AllowOther = chkOther.Checked;
                                    frObj.AllowLockUnlock = chkLockUnlock.Checked;

                                    RoleManager.UpdateRolePermissions(frObj);
                                }
                                else
                                {
                                    frObj = new TblRolePermission();
                                    frObj.RoleID = RoleID;
                                    frObj.FunctionID = functionID;
                                    frObj.AllowAdd = chkAdd.Checked;
                                    frObj.AllowEdit = chkEdit.Checked;
                                    frObj.AllowDelete = chkDelete.Checked;
                                    frObj.AllowUpdateStatus = chkViewPrice.Checked;
                                    frObj.AllowOther = chkOther.Checked;
                                    frObj.AllowLockUnlock = chkLockUnlock.Checked;
                                    RoleManager.InsertRolePermissions(frObj);
                                }
                            }
                        }

                        LoggingActions("Role Permission",
                                        LogsAction.Objects.Action.UPDATE,
                                        LogsAction.Objects.Status.SUCCESS,
                                        JsonConvert.SerializeObject(lstJSData));

                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.ROLE_PERMISSIONS_SAVE_SUCCESSFULLY), MSGButton.OK, MSGIcon.Success);
                        OpenMessageBox(msg, null, false, false);
                    }
                    else
                    {
                        MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.CAN_NOT_FIND_ROLE), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msg, null, false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void AddJsonDataLog(bool oldValueObj, Control ctrlPage, string sTitle, ref List<JsonData> lstJSData)
        {
            bool newValue = ((CheckBox)ctrlPage).Checked;

            if (oldValueObj != newValue)
            {
                if (!lstJSData.Exists(item => item.Data.Equals(sTitle) == true))
                {
                    lstJSData.Add(new JsonData() { Title = "Role Name", Data = sTitle });
                }
                lstJSData.Add(new JsonData()
                {
                    Title = ((CheckBox)ctrlPage).ToolTip,
                    Data = JsonConvert.SerializeObject(new Json()
                    {
                        OldValue = oldValueObj ? "True" : "False",
                        NewValue = newValue ? "True" : "False"
                    })
                });
            }
        }
    }
}