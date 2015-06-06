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
using SweetSoftCMS.ExtraControls.Controls;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class refCellShape : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "cell_shape_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                gvReference.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        private bool _addNew { get; set; }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            gvReference.Columns[0].HeaderText = "Code";//ResourceTextManager.GetApplicationText("BACKING_NAME");
            gvReference.Columns[1].HeaderText = "Cell Shape";
            gvReference.Columns[2].HeaderText = ResourceTextManager.GetApplicationText("Obsolete");
        }

        public override void BindData()
        {
            try
            {
                int totalRows = 0;
                DataTable dt = ReferenceTableManager.SelectAll("", ReferenceTypeHelper.CellShape, -1, CurrentPageIndex, gvReference.PageSize, SortColumn, SortType, false);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvReference.VirtualItemCount = totalRows;
                    gvReference.DataSource = dt;
                    gvReference.DataBind();
                    gvReference.PageIndex = CurrentPageIndex;
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
                DataTable dt = ReferenceTableManager.SelectAll("", ReferenceTypeHelper.CellShape, -1, CurrentPageIndex, gvReference.PageSize, SortColumn, SortType, AddNew);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvReference.VirtualItemCount = totalRows;
                    gvReference.DataSource = dt;
                    gvReference.DataBind();
                    gvReference.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void gvReference_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvReference.EditIndex = -1;
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
            gvReference.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvReference.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void gvReference_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReference.EditIndex = -1;
            btnAdd.Visible = true;
            btnDelete.Visible = true;

            CurrentPageIndex = e.NewPageIndex;
            gvReference.PageIndex = CurrentPageIndex;
            BindData();
        }

        //ADD, EDIT, DELETE DEPARTMENT
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        private void BindGrid()
        {
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            gvReference.DataSource = dt;
            gvReference.DataBind();
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
            int randID = 0;
            do
            {
                randID = (int)rnd.Next(-10000, -1);
            } while (dt.AsEnumerable().Where(x => x.Field<int>("ReferencesID") == randID).Count() > 0);

            dr["ReferencesID"] = randID;
            dr["Code"] = "";
            dr["Name"] = "";
            dr["Type"] = ReferenceTypeHelper.CellShape;
            dr["IsObsolete"] = 0;
            dr["RowsCount"] = 0;

            dt.Rows.InsertAt(dr, 0);

            //Update list
            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, int ReferencesID, string countryCode, string countryName, byte type, byte isObsolete)
        {
            try
            {
                if (ReferenceTableManager.ExistName(ReferencesID, countryName, ReferenceTypeHelper.CellShape))
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "A cell shape with this information already exists in the system!!", MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }
                if (ReferenceTableManager.ExistCode(ReferencesID, countryCode, ReferenceTypeHelper.CellShape))
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "A cell shape with this information already exists in the system!!", MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }

                TblReference obj = new TblReference();
                obj = ReferenceTableManager.SelectByID(ReferencesID);
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

                    if (obj.Code != countryCode)
                    {
                        lstData.Add(new JsonData()
                        {
                            Title = "Code",
                            Data = JsonConvert.SerializeObject(new Json()
                            {
                                OldValue = obj.Code,
                                NewValue = countryCode
                            })
                        });
                    }
                    if (obj.Name != countryName)
                    {
                        lstData.Add(new JsonData()
                        {
                            Title = "Name",
                            Data = JsonConvert.SerializeObject(new Json()
                            {
                                OldValue = obj.Name,
                                NewValue = countryName
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
                                OldValue = ConverByteToBool(obj.IsObsolete) ? "True" : "False",
                                NewValue = ConverByteToBool(isObsolete) ? "True" : "False"
                            })
                        });
                    }

                    obj.Name = countryName;
                    obj.Code = countryCode;
                    obj.IsObsolete = isObsolete;
                    obj.Type = type;
                    ReferenceTableManager.Update(obj);

                    //Lưu vào logging
                    LoggingActions("Cell Shape",
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
                    obj = new TblReference();
                    obj.Name = countryName;
                    obj.Type = type;
                    obj.Code = countryCode;
                    obj.IsObsolete = isObsolete;
                    ReferenceTableManager.Insert(obj);

                    //Lưu vào logging
                    LoggingActions("Cell Shape",
                            LogsAction.Objects.Action.CREATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Code", Data = obj.Code} ,
                                new JsonData() { Title = "Name", Data = obj.Name} 
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
            if (dr != null && dr.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    if (string.IsNullOrEmpty(r["Name"].ToString()) || string.IsNullOrEmpty(r["Code"].ToString()))
                        dr.Add(r);
                }
                foreach (DataRow r in dr)
                    r.Delete();
            }
            dt.AcceptChanges();
            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Xóa dữ liệu được chọn
        private string removeSelectedRows(List<int> idList)
        {


            string cannotDeletedList = "";
            foreach (int ID in idList)
            {
                //TblReference obj = ReferenceTableManager.SelectByID(ID);
                //if (obj != null)
                //{
                //    ReferenceTableManager.Delete(ID);
                //    //Lưu vào logging
                //    LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                //}

                if (!ReferenceTableManager.IsBeingUsed(ID))
                {
                    TblReference obj = ReferenceTableManager.SelectByID(ID);
                    if (obj != null)
                    {
                        ReferenceTableManager.Delete(ID);
                        //Lưu vào logging
                        LoggingManager.LogAction(ActivityLoggingHelper.DELETE, FUNCTION_PAGE_ID, obj.ToJSONString());
                    }

                }
                else
                {
                    DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
                    string countryName = dt.AsEnumerable().Where(x => x.Field<int>("ReferencesID") == ID).Select(x => x.Field<string>("Name")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", countryName);
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
            gvReference.EditIndex = 0;

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
            gvReference.EditIndex = -1;
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
            for (int i = 0; i < gvReference.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvReference.Rows[i].FindControl("chkIsDelete");
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

                            List<int> idList = new List<int>();
                            List<JsonData> lstData = new List<JsonData>();

                            for (int i = 0; i < gvReference.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)gvReference.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    int ID = Convert.ToInt16(gvReference.DataKeys[i].Value);
                                    TblReference obj = ReferenceTableManager.SelectByID(ID);
                                    if (obj != null)
                                    {
                                        lstData.Add(new JsonData() { Title = "Cell Shape", Data = "[Code: " + obj.Code + "] ; " + "[Name: " + obj.Name + "]" });
                                    }
                                    idList.Add(ID);
                                }
                            }
                            string DataCannotDelete = removeSelectedRows(idList);
                            BindData();
                            if (string.IsNullOrEmpty(DataCannotDelete))
                            {
                                LoggingActions("Cell Shape",
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

        protected void gvReference_RowEditing(object sender, GridViewEditEventArgs e)
        {
            removeInvalidRows();
            gvReference.EditIndex = e.NewEditIndex;
            BindGrid();

            btnAdd.Visible = false;
            btnDelete.Visible = false;

            btnCancel.Visible = true;
            btnCancel.Enabled = true;
        }

        protected void gvReference_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = Convert.ToInt32(gvReference.DataKeys[e.RowIndex].Value);
            CustomExtraTextbox txtCode = (CustomExtraTextbox)gvReference.Rows[e.RowIndex].FindControl("txtCode");
            CustomExtraTextbox txtName = (CustomExtraTextbox)gvReference.Rows[e.RowIndex].FindControl("txtName");
            CheckBox chkIsObsolete = (CheckBox)gvReference.Rows[e.RowIndex].FindControl("chkIsObsolete");

            string code = string.Empty;
            string name = string.Empty;
            byte isObsolete = 0;

            if (txtCode != null)
            {
                if (string.IsNullOrEmpty(txtCode.Text))
                {
                    AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
                code = txtCode.Text.Trim();
            }
            if (txtName != null)
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                }
                name = txtName.Text.Trim();
            }
            if (chkIsObsolete != null)
                isObsolete = chkIsObsolete.Checked ? (byte)1 : (byte)0;

            if (ReferenceTableManager.ExistCode(ID, code, ReferenceTypeHelper.CellShape))
            {
                AddErrorPrompt(txtCode.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EXISTS));
            }
            if (ReferenceTableManager.ExistName(ID, name, ReferenceTypeHelper.CellShape))
            {
                AddErrorPrompt(txtName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EXISTS));
            }

            if (IsValid)
            {
                updateRow(e.RowIndex, ID, code, name, ReferenceTypeHelper.CellShape, isObsolete);
                gvReference.EditIndex = -1;
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

        protected void gvReference_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //removeInvalidRows();
            gvReference.EditIndex = -1;
            BindData();

            btnAdd.Visible = true;
            btnDelete.Visible = true;

            btnCancel.Visible = false;
        }

        protected void gvReference_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion

        protected void gvReference_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (_addNew)
            {
                if (e.Row.RowIndex != 0)
                {
                    e.Row.Enabled = false;
                }
            }
        }

        protected bool ConverByteToBool(object value)
        {
            bool _value = false;
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                byte num = 0;
                byte.TryParse(value.ToString(), out num);
                if (num == 0)
                    _value = false;
                else
                    _value = true;
            }
            return _value;
        }
    }
}