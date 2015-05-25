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
    public partial class MachineList : ModalBasePage
    {

        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "machine_manager";
            }
        }

        public override void BindData()
        {
            TblMachineCollection col = MachineManager.SelectAll();
            if (col != null && col.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Code");
                dt.Columns.Add("Name");
                dt.Columns.Add("Performance");
                dt.Columns.Add("Department");
                dt.Columns.Add("IsObsolete", typeof(Boolean));
                foreach (var item in col)
                {
                    TblDepartment dptment = DepartmentManager.SelectByID((short)item.DepartmentID);
                    string dptmentname = string.Empty;
                    if (dptment != null)
                    {
                        dptmentname = dptment.DepartmentName;
                    }

                    bool isObsolete = false;
                    isObsolete = item.IsObsolete != null && item.IsObsolete == 1 ? true : false;

                    dt.Rows.Add(item.Id, item.Code, item.Name, item.Performance, dptmentname, item.IsObsolete);
                }
                //dt.DefaultView.Sort = "Department ASC";
                dt.DefaultView.Sort = dt.Columns[int.Parse(SortColumn)] == null ? "Department ASC" : (dt.Columns[int.Parse(SortColumn)].ColumnName + " " + (SortType == "A" ? "ASC" : "DESC"));
                gvMachine.DataSource = dt;
                gvMachine.DataBind();
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
                        if (e.Value.ToString().Equals("MACHINE_DELETE"))
                        {
                            for (int i = 0; i < gvMachine.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)gvMachine.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    short ID = Convert.ToInt16(gvMachine.DataKeys[i].Value);
                                    MachineManager.Delete(ID);
                                }
                            }
                            BindData();
                            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "Data deleted susscessfully!", MSGButton.OK, MSGIcon.Success);
                            OpenMessageBox(msg, null, false, false);
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("MachineDetail.aspx");
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
            for (int i = 0; i < gvMachine.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvMachine.Rows[i].FindControl("chkIsDelete");
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
                result.Value = "MACHINE_DELETE";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void gvMachine_Sorting(object sender, GridViewSortEventArgs e)
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
            gvMachine.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvMachine.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }
    }
}