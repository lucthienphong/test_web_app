using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.LoggingManager;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using SweetSoftCMS.ExtraControls.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class refCurrencyRateExchange : ModalBasePage
    {

        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "currency_exchange_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                ApplyControlText();
                gvCurrency.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);
                BindData();
            }
        }

        private bool _addNew { get; set; }

        //Thay đổi control text theo ngôn ngữ
        private void ApplyControlText()
        {
            gvCurrency.Columns[0].HeaderText = "Foreign Currency short name";//ResourceTextManager.GetApplicationText("CURRENCY_NAME");
            gvCurrency.Columns[1].HeaderText = "Foreign currency value";//ResourceTextManager.GetApplicationText("CURRENCY_VALUE");
            gvCurrency.Columns[2].HeaderText = "Base currency value";
            gvCurrency.Columns[3].HeaderText = ResourceTextManager.GetApplicationText("Obsolete");
        }

        public override void BindData()
        {
            try
            {
                CurrencyManager cr = new CurrencyManager();
                int totalRows = 0;
                DataTable dt = cr.SelectAll("", null, CurrentPageIndex, gvCurrency.PageSize, SortColumn, SortType, false);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvCurrency.VirtualItemCount = totalRows;
                    gvCurrency.DataSource = dt;
                    gvCurrency.DataBind();
                    gvCurrency.PageIndex = CurrentPageIndex;
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
                CurrencyManager cr = new CurrencyManager();
                int totalRows = 0;
                DataTable dt = cr.SelectAll("", null, CurrentPageIndex, gvCurrency.PageSize, SortColumn, SortType, AddNew);
                if (dt.Rows.Count == 0 && CurrentPageIndex != 0)
                {
                    CurrentPageIndex -= 1;
                    BindData();
                }
                else
                {
                    if (dt.Rows.Count > 0)
                        totalRows = (int)dt.Rows[0]["RowsCount"];
                    gvCurrency.VirtualItemCount = totalRows;
                    gvCurrency.DataSource = dt;
                    gvCurrency.DataBind();
                    gvCurrency.PageIndex = CurrentPageIndex;
                }
                Session[ViewState["PageID"] + "tableSource"] = dt;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void gvCurrency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string name = e.CommandName;
        }

        protected void gvCurrency_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvCurrency.EditIndex = -1;
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
            gvCurrency.Columns[columnIndex].HeaderStyle.CssClass = SortType == "A" ? "sorting_desc" : "sorting_asc";
            if (oldColumnIndex != columnIndex)
                gvCurrency.Columns[oldColumnIndex].HeaderStyle.CssClass = "sorting";
            BindData();
        }

        protected void gvCurrency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCurrency.EditIndex = -1;
            btnAdd.Visible = true;
            btnDelete.Visible = true;

            CurrentPageIndex = e.NewPageIndex;
            gvCurrency.PageIndex = CurrentPageIndex;
            BindData();
        }



        //ADD, EDIT, DELETE 
        #region EditData
        //Thêm dòng mới vào dữ liệu
        //Nạp dữ liệu vào lưới
        private void BindGrid()
        {
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            gvCurrency.DataSource = dt;
            gvCurrency.DataBind();
        }

        /// <summary>
        /// Thêm dòng mới vào datagridview => Không sử dụng
        /// </summary>
        private void addRow()
        {
            //Get list of 
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];
            DataRow dr = dt.NewRow();

            Random rnd = new Random();
            short randID = 0;
            do
            {
                randID = (short)rnd.Next(-10000, -1);
            } while (dt.AsEnumerable().Where(x => x.Field<short>("CurrencyID") == randID).Count() > 0);

            dr["CurrencyID"] = randID;
            dr["CurrencyName"] = "";
            dr["CurrencyValue"] = 1;
            dr["RMValue"] = 0;
            dr["IsObsolete"] = false;
            dr["RowsCount"] = 0;

            dt.Rows.InsertAt(dr, 0);

            //Update list
            Session[ViewState["PageID"] + "tableSource"] = dt;
        }

        //Cập nhật dòng dữ liệu
        private void updateRow(int rowIndex, short currencyID, string currencyName, decimal rmValue, decimal currencyValue, bool isObsolete)
        {
            try
            {
                CurrencyManager cr = new CurrencyManager();
                if (cr.Exist(currencyID, currencyName))
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), "A currency with this information already exists in the system!", MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                    return;
                }

                TblCurrency obj = new TblCurrency();
                obj = cr.SelectByID(currencyID);
                if (obj != null)
                {
                    if (!RoleManager.AllowAdd(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }
                    List<JsonData> lstData = new List<JsonData>();

                    if (obj.CurrencyName != currencyName)
                    {
                        lstData.Add(new JsonData()
                        {
                            Title = "Name",
                            Data = JsonConvert.SerializeObject(new Json()
                            {
                                OldValue = obj.CurrencyName,
                                NewValue = currencyName
                            })
                        });
                    }

                    if (obj.RMValue != rmValue)
                    {
                        lstData.Add(new JsonData()
                        {
                            Title = "RMValue",
                            Data = JsonConvert.SerializeObject(new Json()
                            {
                                OldValue = obj.RMValue.ToString(),
                                NewValue = rmValue.ToString()
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
                                OldValue = Convert.ToBoolean(obj.IsObsolete) ? "True" : "False",
                                NewValue = Convert.ToBoolean(isObsolete) ? "True" : "False"
                            })
                        });
                    }

                    obj.CurrencyName = currencyName;
                    obj.RMValue = rmValue;
                    obj.CurrencyValue = currencyValue;
                    obj.IsObsolete = isObsolete;
                    if (cr.Update(obj) != null) {
                        TblCurrencyChangedLog changeLog = new TblCurrencyChangedLog();
                        //changeLog.ChangedBy = ApplicationContext.Current.UserID;
                        changeLog.CurrencyID = obj.CurrencyID;
                        changeLog.DateChanged = DateTime.Now;
                        changeLog.NewValue = obj.CurrencyValue.ToString();
                        changeLog.NewName = obj.CurrencyName;
                        changeLog.NewRMValue = obj.RMValue.ToString();
                        changeLog.Status = "Updated";
                        cr.InsertChangeedLog(changeLog);
                    }


                    //Lưu vào logging
                    LoggingActions("Currency",
                            LogsAction.Objects.Action.UPDATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(lstData));
                    LoggingManager.LogAction(ActivityLoggingHelper.UPDATE, FUNCTION_PAGE_ID, obj.ToJSONString());
                }
                else
                {
                    if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                    {
                        MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                        OpenMessageBox(msgRole, null, false, false);
                        return;
                    }
                    obj = new TblCurrency();
                    obj.CurrencyName = currencyName;
                    obj.RMValue = rmValue;
                    obj.CurrencyValue = currencyValue;
                    obj.IsObsolete = isObsolete;
                    if (cr.Insert(obj)!=null)
                    {
                        TblCurrencyChangedLog changeLog = new TblCurrencyChangedLog();
                        //changeLog.ChangedBy = ApplicationContext.Current.UserID;
                        changeLog.CurrencyID = obj.CurrencyID;
                        changeLog.DateChanged = DateTime.Now;
                        changeLog.NewValue = obj.CurrencyValue.ToString();
                        changeLog.NewName = obj.CurrencyName;
                        changeLog.NewRMValue= obj.RMValue.ToString();
                        changeLog.Status = "New";
                        cr.InsertChangeedLog(changeLog);
                    }

                    //Lưu vào logging
                    LoggingActions("Currency",
                            LogsAction.Objects.Action.CREATE,
                            LogsAction.Objects.Status.SUCCESS,
                            JsonConvert.SerializeObject(new List<JsonData>() { 
                                new JsonData() { Title = "Name", Data = obj.CurrencyName} 
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
            //Get list of 
            DataTable dt = (DataTable)Session[ViewState["PageID"] + "tableSource"];

            List<DataRow> dr = new List<DataRow>();
            foreach (DataRow r in dt.Rows)
            {
                if (string.IsNullOrEmpty(r["CurrencyName"].ToString()))
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
            CurrencyManager cr = new CurrencyManager();
            foreach (short ID in idList)
            {
                if (!cr.IsBeingUsed(ID))
                {
                    TblCurrency obj = cr.SelectByID(ID);
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
                    string currencyName = dt.AsEnumerable().Where(x => x.Field<short>("CurrencyID") == ID).Select(x => x.Field<string>("CurrencyName")).FirstOrDefault();
                    cannotDeletedList += string.Format("{0}, ", currencyName);
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
            gvCurrency.EditIndex = 0;

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
            gvCurrency.EditIndex = -1;
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
            for (int i = 0; i < gvCurrency.Rows.Count; i++)
            {
                CheckBox chkIsDelete = (CheckBox)gvCurrency.Rows[i].FindControl("chkIsDelete");
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
                result.Value = "Currency_Delete";
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
                        if (e.Value.ToString().Equals("Currency_Delete"))
                        {
                            //Kiểm tra quyền
                            if (!RoleManager.AllowDelete(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                            {
                                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                                OpenMessageBox(msgRole, null, false, false);
                                return;
                            }

                            List<short> idList = new List<short>();
                            List<JsonData> lstData = new List<JsonData>();

                            for (int i = 0; i < gvCurrency.Rows.Count; i++)
                            {
                                CheckBox chkIsDelete = (CheckBox)gvCurrency.Rows[i].FindControl("chkIsDelete");
                                if (chkIsDelete.Checked)
                                {
                                    short ID = Convert.ToInt16(gvCurrency.DataKeys[i].Value);
                                    TblCurrency obj = new CurrencyManager().SelectByID(ID);
                                    if (obj != null)
                                    {
                                        lstData.Add(new JsonData() { Title = "Currency", Data = "[Name: " + obj.CurrencyName + "]" });
                                    }
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

        protected void gvCurrency_RowEditing(object sender, GridViewEditEventArgs e)
        {
            removeInvalidRows();
            gvCurrency.EditIndex = e.NewEditIndex;
            BindGrid();

            btnAdd.Visible = false;
            btnDelete.Visible = false;

            btnCancel.Visible = true;
            btnCancel.Enabled = true;
        }

        protected void gvCurrency_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            short ID = Convert.ToInt16(gvCurrency.DataKeys[e.RowIndex].Value);
            TextBox txtCurrencyName = (TextBox)gvCurrency.Rows[e.RowIndex].FindControl("txtCurrencyName");
            TextBox txtRMValue = (TextBox)gvCurrency.Rows[e.RowIndex].FindControl("txtRMValue");
            //TextBox txtCurrencyValue = (TextBox)gvCurrency.Rows[e.RowIndex].FindControl("txtCurrencyValue");
            CurrencyManager cr = new CurrencyManager();
            if (string.IsNullOrEmpty(txtCurrencyName.Text))
            {
                AddErrorPrompt(txtCurrencyName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            if (string.IsNullOrEmpty(txtRMValue.Text))
            {
                AddErrorPrompt(txtRMValue.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            }

            //if (string.IsNullOrEmpty(txtCurrencyValue.Text))
            //{
            //    AddErrorPrompt(txtCurrencyValue.ClientID, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
            //}

            if (cr.Exist(ID, txtCurrencyName.Text.Trim()))
            {
                AddErrorPrompt(txtCurrencyName.ClientID, ResourceTextManager.GetApplicationText(ResourceText.EXISTS));
            }

            if (IsValid)
            {
                CheckBox chkIsObsolete = (CheckBox)gvCurrency.Rows[e.RowIndex].FindControl("chkIsObsolete");
                string currencyName = txtCurrencyName.Text;
                decimal RMValue = decimal.Parse(txtRMValue.Text);
                decimal CurrencyValue = 1;//decimal.Parse(txtCurrencyValue.Text);
                bool isObsolete = chkIsObsolete.Checked;
                updateRow(e.RowIndex, ID, currencyName, RMValue, CurrencyValue, isObsolete);
                gvCurrency.EditIndex = -1;
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

        protected void gvCurrency_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //removeInvalidRows();
            gvCurrency.EditIndex = -1;
            BindData();

            btnAdd.Visible = true;
            btnDelete.Visible = true;

            btnCancel.Visible = false;
        }

        protected void gvCurrency_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion

        protected void gvCurrency_RowCreated(object sender, GridViewRowEventArgs e)
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