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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class Department : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "department_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                grvDepartmentList.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
            else
            {
                int EditIndex = grvDepartmentList.EditIndex;
                if(EditIndex >= 0)
                {
                    HtmlGenericControl div = grvDepartmentList.Rows[EditIndex].FindControl("divProductType") as HtmlGenericControl;
                    addCheckList(div, 0);
                }
            }
        }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            grvDepartmentList.Columns[0].HeaderText = "Department";
            grvDepartmentList.Columns[1].HeaderText = "Process Type";
            grvDepartmentList.Columns[2].HeaderText = "Product Type";
            grvDepartmentList.Columns[3].HeaderText = "Used in workflow";
            //grvDepartmentList.Columns[4].HeaderText = "Used in job timeline";
            grvDepartmentList.Columns[4].HeaderText = "Timeline display order";
            grvDepartmentList.Columns[5].HeaderText = ResourceTextManager.GetApplicationText(ResourceText.IS_OBSOLETE);
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                DataTable dt = DepartmentManager.SelectAll("", null, CurrentPageIndex, grvDepartmentList.PageSize, SortColumn, SortType, false);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvDepartmentList.VirtualItemCount = totalRows;
                    grvDepartmentList.DataSource = dt;
                    grvDepartmentList.DataBind();
                    grvDepartmentList.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch(Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void BindData(bool AddNew)
        {
            try
            {
                int totalRows = 0;
                DataTable dt = DepartmentManager.SelectAll("", null, CurrentPageIndex, grvDepartmentList.PageSize, SortColumn, SortType, AddNew);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    grvDepartmentList.VirtualItemCount = totalRows;
                    grvDepartmentList.DataSource = dt;
                    grvDepartmentList.DataBind();
                    grvDepartmentList.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void grvDepartmentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grvDepartmentList_Sorting(object sender, GridViewSortEventArgs e)
        {
            grvDepartmentList.EditIndex = -1;
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
            grvDepartmentList.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                grvDepartmentList.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void grvDepartmentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvDepartmentList.EditIndex = -1;
            btnAdd.Visible = true;
            btnDelete.Visible = true;

            CurrentPageIndex = e.NewPageIndex;
            grvDepartmentList.PageIndex = CurrentPageIndex;
            BindData();
        }

        
        //Make process type example
        private DataTable dtProcessType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(short));
            dt.Columns.Add("Code", typeof(string));
            for (int i = 0; i < 10; i++)
            {
                DataRow r = dt.NewRow();
                r["ID"] = i + 1;
                r["Code"] = "Code" + (i + 1).ToString();
                dt.Rows.Add(r);
            }
            return dt;
        }

        //Add dropdown vào control
        private void addCheckList(HtmlGenericControl divProductType, short DepartmentID)
        {
            divProductType.Controls.Clear();
            List<ProductTypeExtension> list = ReferenceTableManager.SelectProductTypeForCheckboxList(DepartmentID);
            if (list == null)
                list = new List<ProductTypeExtension>();

            foreach (ProductTypeExtension item in list)
            {
                //div col-md
                HtmlGenericControl colmd = new HtmlGenericControl("div");
                colmd.Attributes.Add("class", "col-md-6");
                //label
                HtmlGenericControl label = new HtmlGenericControl("label");
                //checkbox
                CheckBox chk = new CheckBox();
                chk.ID = item.ReferencesID.ToString();
                chk.Checked = item.IsChecked;
                chk.CssClass = "uniform";
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes.Add("style", "font-weight: normal;");
                span.InnerText = item.Code;
                //Add checkbox into label
                label.Controls.Add(chk);
                label.Controls.Add(span);
                //Add label into div col-md
                colmd.Controls.Add(label);
                //Add col-md-4 into panel
                divProductType.Controls.Add(colmd);
            }
        }

        protected void grvDepartmentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    short DepartmentID = Convert.ToInt16(grvDepartmentList.DataKeys[e.Row.RowIndex].Value);
                    //Process Type
                    DropDownList ddlProcessType = e.Row.FindControl("ddlProcessType") as DropDownList;
                    if (ddlProcessType != null)
                    {
                        ddlProcessType.DataSource = ReferenceTableManager.SelectProcessTypeForDDL(true);
                        ddlProcessType.DataTextField = "Code";
                        ddlProcessType.DataValueField = "ReferencesID";
                        ddlProcessType.DataBind();

                        TblDepartment obj = DepartmentManager.SelectByID(DepartmentID);
                        if (obj != null)
                            ddlProcessType.SelectedValue = obj.ProcessTypeID != null ? obj.ProcessTypeID.ToString() : "0";
                    }

                    //Product Type
                    //Find div control
                    HtmlGenericControl divProductType = e.Row.FindControl("divProductType") as HtmlGenericControl;
                    if (divProductType != null)
                    {
                        addCheckList(divProductType, DepartmentID);
                    }
                }
            }
            catch
            {
            }
        }

        //ADD, EDIT, DELETE DEPARTMENT
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        private void BindGrid()
        {
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            grvDepartmentList.DataSource = dt;
            grvDepartmentList.DataBind();
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
            } while (dt.AsEnumerable().Where(x => x.Field<short>("DepartmentID") == randID).Count() > 0);

            dr["DepartmentID"] = randID;
            dr["DepartmentName"] = "";
            dr["ShowInWorkFlow"] = true;
            dr["IsTimeline"] = true;
            dr["TimelineOrder"] = 0;
            dr["IsObsolete"] = false;
            dr["RowsCount"] = 0;

            dt.Rows.InsertAt(dr, 0);

            //Update list
            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        private List<CheckBox> FindCharacteristics(HtmlGenericControl divProductType)
        {
            List<CheckBox> chkList = new List<CheckBox>();
            Extensions.GetControlList<CheckBox>(divProductType.Controls, chkList);
            return chkList;
        }

        private string GetProductTypeID(HtmlGenericControl divProductType)
        {
            string ret = string.Empty;
            //Thêm lại các đặc điểm mới
            List<CheckBox> chkList = FindCharacteristics(divProductType);
            foreach (CheckBox chk in chkList)
            {
                try
                {
                    if (chk.Checked)
                    {
                        short chID = Convert.ToInt16(chk.ID);
                        ret += string.Format("--{0}--", chID);
                    }
                }
                catch { }
            }
            return ret;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, short departmentID, string departmentName, bool ShowInWorkFlow, short ProcessTypeID, string ProductTypeID, byte TimelineOrder, bool isObsolete)
        {
            try
            {
                if (DepartmentManager.Exists(departmentID, departmentName))
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "This department exists!", MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }

                TblDepartment obj = new TblDepartment();
                obj = DepartmentManager.SelectByID(departmentID);
                if (obj != null)
                {
                    obj.DepartmentName = departmentName;
                    obj.ShowInWorkFlow = Convert.ToByte(ShowInWorkFlow);
                    obj.TimelineOrder = TimelineOrder;
                    obj.ProcessTypeID = ProcessTypeID;
                    obj.ProductTypeID = ProductTypeID;
                    obj.IsObsolete = Convert.ToByte(isObsolete);
                    DepartmentManager.Update(obj);

                    //Lưu vào logging
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
                else
                {
                    obj = new TblDepartment();
                    obj.DepartmentName = departmentName;
                    obj.ShowInWorkFlow = Convert.ToByte(ShowInWorkFlow);
                    obj.TimelineOrder = TimelineOrder;
                    obj.ProcessTypeID = ProcessTypeID;
                    obj.ProductTypeID = ProductTypeID;
                    obj.IsObsolete = Convert.ToByte(isObsolete);
                    DepartmentManager.Insert(obj);

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
                if (string.IsNullOrEmpty(r["DepartmentName"].ToString()))
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
            foreach (short ID in idList)
            {
                if (!DepartmentManager.IsBeingUsed(ID))
                {
                    TblDepartment obj = DepartmentManager.SelectByID(ID);
                    if (obj != null)
                    {
                        DepartmentManager.Delete(ID);

                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                    }
                }
                else
                {
                    DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                    string departmentName = dt.AsEnumerable().Where(x => x.Field<short>("DepartmentID") == ID).Select(x => x.Field<string>("DepartmentName")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", departmentName);
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
            grvDepartmentList.EditIndex = 0;
            BindData(true);

            btnAdd.Visible = false;
            btnDelete.Visible = false;
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
            for (int i = 0; i < grvDepartmentList.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)grvDepartmentList.Rows[i].FindControl("chkIsDelete");
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
                result.Value = "Department_Delete";
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
                        if (e.Value.ToString().Equals("Department_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<short> idList = new List<short>();
                            for (int i = 0; i < grvDepartmentList.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)grvDepartmentList.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    short ID = Convert.ToInt16(grvDepartmentList.DataKeys[i].Value);
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

        protected void grvDepartmentList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            removeInvalidRows();
            grvDepartmentList.EditIndex = e.NewEditIndex;
            BindGrid();

            btnAdd.Visible = false;
            btnDelete.Visible = false;
        }

        protected void grvDepartmentList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            short ID = Convert.ToInt16(grvDepartmentList.DataKeys[e.RowIndex].Value);
            TextBox txtDepartmentName = (TextBox)grvDepartmentList.Rows[e.RowIndex].FindControl("txtDepartmentName");
            if (string.IsNullOrEmpty(txtDepartmentName.Text))
            {
                AddErrorPrompt(txtDepartmentName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }
            else if (DepartmentManager.Exists(ID, txtDepartmentName.Text.Trim()))
            {
                AddErrorPrompt(txtDepartmentName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EXISTS));
            }
            if (IsValid)
            {
                CheckBox chkIsObsolete = (CheckBox)grvDepartmentList.Rows[e.RowIndex].FindControl("chkIsObsolete");
                CheckBox chkUsedInWorkflow = (CheckBox)grvDepartmentList.Rows[e.RowIndex].FindControl("chkUsedInWorkflow");
                //CheckBox chkUsedInTimeline = (CheckBox)grvDepartmentList.Rows[e.RowIndex].FindControl("chkUsedInTimeline");
                DropDownList ddlProcessType = grvDepartmentList.Rows[e.RowIndex].FindControl("ddlProcessType") as DropDownList;
                HtmlGenericControl divProductType = grvDepartmentList.Rows[e.RowIndex].FindControl("divProductType") as HtmlGenericControl;
                //if (divProductType != null)
                //    addCheckList(divProductType);

                TextBox txtTimelineOrder = (TextBox)grvDepartmentList.Rows[e.RowIndex].FindControl("txtTimelineOrder");
                string departmentName = txtDepartmentName.Text;
                bool isObsolete = chkIsObsolete.Checked;
                bool ShowInWorkFlow = chkUsedInWorkflow.Checked;
                //bool IsTimeline = chkUsedInTimeline.Checked;
                short ProcessTypeID = ddlProcessType != null ? Convert.ToInt16(ddlProcessType.SelectedValue) : (short)0;
                string ProductTypeID = divProductType != null ? GetProductTypeID(divProductType) : string.Empty;
                byte TimelineOrder = 0;
                byte.TryParse(txtTimelineOrder.Text.Trim().Replace(",", ""), out TimelineOrder);
                updateRow(e.RowIndex, ID, departmentName, ShowInWorkFlow, ProcessTypeID, ProductTypeID, TimelineOrder, isObsolete);
                grvDepartmentList.EditIndex = -1;
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

        protected void grvDepartmentList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //removeInvalidRows();
            grvDepartmentList.EditIndex = -1;
            BindData();

            btnAdd.Visible = true;
            btnDelete.Visible = true;
        }

        protected void grvDepartmentList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion
    }
}