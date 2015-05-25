using SweetSoft.APEM.Core;
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
    public partial class MachineDetail : ModalBasePage
    {
        private int machineID;
        public int MachineID
        {
            get
            {
                int a = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"], out a);
                }
                return a;
            }
            set { machineID = value; }
        }

        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "machine_manager";
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
                            #region permission
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            } 
                            #endregion
                            TblMachine machine = MachineManager.SelectMachineByID(MachineID);
                            if (machine != null)
                            {
                                MachineManager.Delete(machine.Id);
                                btnCancel_Click(null, null);
                            }
                        }
                        if (e.Value.ToString().Equals("ADD_SUCCESS"))
                        {
                            Response.Redirect("~/Pages/MachineList.aspx", false);
                        }
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
            #region validation
            if (string.IsNullOrEmpty(txtMachineName.Text))
            {
                AddErrorPrompt(txtMachineName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (string.IsNullOrEmpty(txtMachineCode.Text))
            {
                AddErrorPrompt(txtMachineCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (string.IsNullOrEmpty(txtPerformance.Text))
            {
                AddErrorPrompt(txtPerformance.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (string.IsNullOrEmpty(txtMainternance.Text))
            {
                AddErrorPrompt(txtMainternance.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (string.IsNullOrEmpty(txtManufacturer.Text))
            {
                AddErrorPrompt(txtManufacturer.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (string.IsNullOrEmpty(txtProductYear.Text))
            {
                AddErrorPrompt(txtProductYear.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (string.IsNullOrEmpty(ddlDepartment.SelectedValue))
            {
                AddErrorPrompt(ddlDepartment.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (!IsValid)
            {
                ShowErrorPrompt();
                return;
            }
            #endregion
            byte _isAbsolete = chkIsObsolete.Checked == true ? (byte)1 : (byte)0;
            if (MachineID == 0)
            {
                TblMachine machine = new TblMachine()
                {
                    Name = txtMachineName.Text,
                    Code = txtMachineCode.Text,
                    Performance = txtPerformance.Text,
                    Maintenance = txtMainternance.Text,
                    Manufacturer = txtManufacturer.Text,
                    ProduceYear = int.Parse(txtProductYear.Text),
                    DepartmentID = short.Parse(ddlDepartment.SelectedValue),
                    IsObsolete = _isAbsolete
                };
                MachineManager.Insert(machine);

                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.Ok_With_Reload, MSGIcon.Success);
                ModalConfirmResult result = new ModalConfirmResult();
                result.Value = "ADD_SUCCESS";
                CurrentConfirmResult = result;
                OpenMessageBox(msg, result, false, false);
            }
            else
            {
                TblMachine machine = MachineManager.SelectMachineByID(MachineID);
                if (machine != null)
                {
                    machine.Name = txtMachineName.Text;
                    machine.Code = txtMachineCode.Text;
                    machine.Performance = txtPerformance.Text;
                    machine.Maintenance = txtMainternance.Text;
                    machine.Manufacturer = txtManufacturer.Text;
                    machine.ProduceYear = int.Parse(txtProductYear.Text);
                    machine.DepartmentID = short.Parse(ddlDepartment.SelectedValue);
                    machine.IsObsolete = _isAbsolete;

                    MachineManager.Update(machine);
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MachineList.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ModalConfirmResult result = new ModalConfirmResult();
            result.Value = "MACHINE_DELETE";
            CurrentConfirmResult = result;

            MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE),
                ResourceTextManager.GetApplicationText(ResourceText.DO_YOU_REALLY_WAN_TO_DELETE_THIS_MACHINE), MSGButton.DeleteCancel, MSGIcon.Warning);
            OpenMessageBox(msg, result, false, false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (MachineID == 0)
                {
                    LoadDepartment();
                    btnDelete.Visible = false;
                }
                else
                {
                    LoadMachine();
                    btnDelete.Visible = true;
                }
            }
        }

        private void LoadDepartment()
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new ListItem(ResourceTextManager.GetApplicationText(ResourceText.CHOOSE_DEPARTMENT), string.Empty));
            DataTable dt = DepartmentManager.SelectAll("", null, 0, 1000, "0", "A", false);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string departmentName = dt.Rows[i]["DepartmentName"].ToString();
                string departID = dt.Rows[i]["DepartmentID"].ToString();
                ddlDepartment.Items.Add(new ListItem(departmentName, departID));
            }
        }

        private void LoadMachine()
        {
            LoadDepartment();
            TblMachine machine = MachineManager.SelectMachineByID(MachineID);
            if (machine != null)
            {
                txtMachineName.Text = machine.Name;
                txtMachineCode.Text = machine.Code;
                txtPerformance.Text = machine.Performance;
                txtMainternance.Text = machine.Maintenance;
                txtManufacturer.Text = machine.Manufacturer;
                txtProductYear.Text = machine.ProduceYear.ToString();
                ddlDepartment.SelectedValue = machine.DepartmentID.ToString();
                chkIsObsolete.Checked = machine.IsObsolete == 1 ? true : false;
            }

        }
    }
}