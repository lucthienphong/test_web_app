using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SweetSoft.APEM.WebApp.MasterPages;
using System.Web.UI;
using System.Web.Services;
using System.Text;
using System.Globalization;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.LoggingManager;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Core.Helper;
using System.Web.SessionState;
using SweetSoftCMS.ExtraControls.Controls;
using System.Data;
using SweetSoft.APEM.Core.UI;
using SweetSoft.APEM.Core.Logs;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace SweetSoft.APEM.WebApp.Common
{
    public class ModalBasePage : System.Web.UI.Page
    {
        #region Enum
        protected enum GridCommand
        {
            Update,
            Role,
            Delete
        }

        public enum ShortCutEvent
        {
            Buttonclick,
            Gridclick
        }
        #endregion

        #region Validation
        /// <summary>
        /// Validation
        /// </summary>
        /// <param name="e"></param>
        /// <summary>

        //OLD VALIDATION
        /// Thêm validation lỗi
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="message"></param>
        protected void AddErrorPrompt(string clientID, string message)
        {
            string msgValid = string.Format("{0}", message);
            int u = PromptControdClientIDs.FindIndex(delegate(string p) { return p.Equals(clientID); });
            if (u > -1)
            {
                if (!string.IsNullOrEmpty(PromptErrorMessages[u]))
                {
                    PromptErrorMessages[u] += msgValid;
                }
            }
            else
            {
                PromptControdClientIDs.Add(clientID);
                PromptErrorMessages.Add(msgValid);
            }
        }
        /// <summary>
        /// Hiển thị validation lỗi
        /// </summary>
        /// <returns></returns>
        protected bool ShowErrorPrompt()
        {
            StringBuilder promptScript = new StringBuilder();
            if (PromptControdClientIDs.Count > 0 && PromptControdClientIDs.Count == PromptErrorMessages.Count)
            {
                for (int i = 0; i < PromptControdClientIDs.Count; i++)
                    promptScript.AppendFormat("$('#{0}').validationEngine('showPrompt', '{1}', 'error', true); ", PromptControdClientIDs[i], PromptErrorMessages[i]);

                string script = promptScript.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RunScript", script, true);
                return true;
            }
            return false;
        }

        private List<string> m_PromptErrorMessages;
        public List<string> PromptErrorMessages
        {
            get
            {
                if (m_PromptErrorMessages == null)
                    m_PromptErrorMessages = new List<string>();
                return m_PromptErrorMessages;
            }
            set
            {
                m_PromptErrorMessages = value;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //NEW VALIDATION
        string GetClientScriptError(string id, string message)
        {
            return "ShowError('" + id + "','" +
                message.Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace("'", "\"") +
                "',true);";
        }

        /// <summary>
        /// Hiển thị validation lỗi
        /// </summary>
        /// <returns></returns>
        protected bool ShowErrorPromptExtension()
        {
            StringBuilder sbRenderError = new StringBuilder();
            if (PromptControdClientIDs.Count > 0 && PromptControdClientIDs.Count == PromptErrorMessages.Count)
            {
                for (int i = 0; i < PromptControdClientIDs.Count; i++)
                    sbRenderError.Append(GetClientScriptError(PromptControdClientIDs[i], PromptErrorMessages[i]));

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "errorform", sbRenderError.ToString(), true);
                return true;
            }
            return false;
        }

        #endregion

        #region Paging + Sorting
        //Pagination -- Phân trang theo GridviewAdapter
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
        }

        public virtual void BindData()
        {

        }

        //Current gridview pageindex
        public int CurrentPageIndex
        {
            get
            {
                if (ViewState["PageIndex"] == null)
                    ViewState["PageIndex"] = 0;
                return (int)ViewState["PageIndex"];
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }

        //Current sort type
        public string SortType
        {
            get
            {
                if (ViewState["SortType"] == null)
                {
                    ViewState["SortType"] = "A";
                }
                return ViewState["SortType"].ToString();
            }
            set
            {
                ViewState["SortType"] = value;
            }

        }

        //Current sort type
        public string SortColumn
        {
            get
            {
                if (ViewState["SortColumn"] == null)
                {
                    ViewState["SortColumn"] = "0";
                }
                return ViewState["SortColumn"].ToString();
            }
            set
            {
                ViewState["SortColumn"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Login
        /// </summary>
        public string PageTitle
        {
            get
            {
                return Page.Header.Title;
            }
            set
            {
                Header.Title = string.Format("{0} - {1}", value, SettingManager.GetSettingValue(SettingNames.ApplicationTitle));
            }
        }

        protected string SessionPrefix = ApplicationContext.SessionPrefix;
        protected HttpSessionState session;

        #region Properties
        /// <summary>
        /// Cho phép lưu history
        /// </summary>
        public bool AllowSaveLogging
        {
            get
            {
                return SettingManager.GetSettingValueBoolean(SettingNames.AllowSaveLogging);
            }
        }

        /// <summary>
        /// Danh sách các id control đang bị lỗi
        /// </summary>
        private List<WebControl> listValidationPromtControl;
        protected List<WebControl> CurrentValidationPromtControl
        {
            get
            {

                if (listValidationPromtControl == null)
                    listValidationPromtControl = new List<WebControl>();
                return listValidationPromtControl;
            }
        }
        /// <summary>
        /// Danh sách các thông báo lỗi trên từng control
        /// </summary>
        private List<string> listValidationPromtMessage;
        protected List<string> CurrentValidationPromtMessage
        {
            get
            {
                if (listValidationPromtMessage == null)
                    listValidationPromtMessage = new List<string>();
                return listValidationPromtMessage;
            }
        }
        /// <summary>
        /// Quyền cập nhật
        /// </summary>
        protected bool Updateable
        {
            get
            {
                //if (Viewable)
                //    return CurrentRoleFunction[FUNCTION_PAGE].IsUpdate;
                return false;
            }
        }
        /// <summary>
        /// Quyền tạo mới
        /// </summary>
        protected bool Createable
        {
            get
            {
                //if (Viewable)
                //    return CurrentRoleFunction[FUNCTION_PAGE].IsInsert;
                return false;
            }
        }
        /// <summary>
        /// Quyền xóa
        /// </summary>
        protected bool Deleteable
        {
            get
            {
                //if (Viewable)
                //    return CurrentRoleFunction[FUNCTION_PAGE].IsDelete;
                return false;
            }
        }
        /// <summary>
        /// Quyền xem
        /// </summary>
        protected bool Viewable
        {
            get
            {
                return false;// CurrentRoleFunction.ContainsKey(FUNCTION_PAGE);
            }
        }
        public virtual string FUNCTION_PAGE
        {
            get { return SecurityHelper.APPLICATION_NAME; }
        }
        /// <summary>
        /// Thiết lập số lượng mỗi lần lấy dữ liệu cho combobox
        /// </summary>
        protected int ComboboxLoadCount
        {
            get
            {
                // Lấy từ cấu hình
                return 20;
            }
        }

        public ModalConfirmResult CurrentConfirmResult
        {
            get
            {
                if (Session["CurrentConfirmResult"] != null)
                    return (ModalConfirmResult)Session["CurrentConfirmResult"];
                return null;
            }
            set
            {
                Session["CurrentConfirmResult"] = value;
            }
        }
        /// <summary>
        /// CultureInfo cho số
        /// </summary>
        protected CultureInfo NumberCulture
        {
            get
            {
                CultureInfo info = new CultureInfo("vi-VN");
                info.NumberFormat.NumberDecimalSeparator = ",";
                info.NumberFormat.NumberGroupSeparator = ".";
                info.NumberFormat.CurrencyDecimalSeparator = ",";
                info.NumberFormat.CurrencyGroupSeparator = ".";
                info.NumberFormat.PercentDecimalSeparator = ",";
                info.NumberFormat.PercentGroupSeparator = ".";
                info.NumberFormat.NumberGroupSizes = new int[] { 3 };
                return info;
            }
        }
        /// <summary>
        /// MasterPage của trang
        /// </summary>
        protected ModalMasterPage CURRENT_MASTERPAGES
        {
            get
            {
                try
                {
                    MasterPage masterPage = this.Master;
                    return (ModalMasterPage)masterPage;
                }
                catch (Exception) { }
                return null;
            }
        }
        /// <summary>
        /// Control nhận focus mặc định
        /// </summary>
        protected Control FocusControlDefault
        {
            get;
            set;
        }
        #endregion

        #region Function

        /// <summary>
        /// Đóng popup link tới trang web
        /// </summary>
        protected void CloseMasterWindowPage()
        {
            RunScript("CloseRadWindDow", string.Empty);
        }

        /// <summary>
        /// Hàm run script trong telerik script block từ code behind
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="param"></param>
        public void RunScript(string scriptName, string param)
        {
            string script = string.Format("{0}({1});", scriptName, param);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RunScript", script, true);

        }
        /// <summary>
        /// Hàm run script thường
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="param"></param>
        public void RunScriptC(string scriptName, string param)
        {
            string script = string.Format("{0}({1});", scriptName, param);
            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, typeof(string), "runScript",
                                       script, true);
        }
        /// <summary>
        /// Hàm kiểm tra giá trị các control có thay đổi khi đóng form
        /// </summary>
        /// <param name="checking"></param>
        protected void CheckedChangeValueWhenClose(bool checking)
        {
            RunScriptC("CheckedChangeValueWhenClose", checking.ToString().ToLower());
        }
        /// <summary>
        /// Bật chế độ validation
        /// </summary>
        protected void EnableValidation()
        {
            RunScript("ChangeValidation", "true");
        }
        /// <summary>
        /// Tắt chế độ validation
        /// </summary>
        protected void DisableValidation()
        {
            RunScript("ChangeValidation", "false");
        }
        /// <summary>
        /// Hàm thêm thông báo lỗi cho các control
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="message"></param>
        protected bool AddValidationPromt(WebControl control, string message)
        {
            // PromptControdClientIDs.Add(control.ID);
            CurrentValidationPromtControl.Add(control);
            CurrentValidationPromtMessage.Add(message);
            return true;
        }
        /// <summary>
        /// Hàm xóa thông báo lỗi cho các control
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        protected bool RemoveValidationPromt(string clientID)
        {
            int indx = CurrentValidationPromtControl.FindIndex(delegate(WebControl p) { return p.ClientID.Equals(clientID); });
            if (indx > -1)
            {
                CurrentValidationPromtControl.RemoveAt(indx);
                CurrentValidationPromtMessage.RemoveAt(indx);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Xóa tất cả thông báo lỗi cho các control
        /// </summary>
        /// <returns></returns>
        protected bool ClearValidationPromt()
        {
            CurrentValidationPromtControl.Clear();
            return true;
        }
        /// <summary>
        /// Hiển thị các validation trên các control
        /// </summary>
        /// <returns></returns>
        protected bool ShowValidationPromtError()
        {
            StringBuilder promptScript = new StringBuilder();
            if (CurrentValidationPromtControl.Count > 0 && (CurrentValidationPromtControl.Count == CurrentValidationPromtMessage.Count))
            {
                for (int i = 0; i < CurrentValidationPromtControl.Count; i++)
                {
                    WebControl control = CurrentValidationPromtControl[i];

                    //DEV:show inline message
                    //string promtPostion = "topRight";
                    //string currentPromtPostion = control.Attributes["data-prompt-position"];
                    //if (!string.IsNullOrEmpty(currentPromtPostion))
                    //    promtPostion = currentPromtPostion;
                    promptScript.AppendFormat("ShowPromtMessage('{0}','{1}');", control.ClientID, CurrentValidationPromtMessage[i]);
                }

                AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, typeof(string), "showError",
                                        promptScript.ToString(), true);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Hiển thị thông báo load thành công
        /// </summary>
        /// <param name="message"></param>
        public void LoadSuccessForm(string message)
        {
            string script = string.Format("loadSuccess(\"{0}\");", message);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "jsSuccessMessage", script, true);
        }

        public void CheckFunctionPermission(string userName)
        {
            //if (CurrentUserFunctions != null && CurrentUserFunctions.Count > 0)
            //{
            //    if (!CurrentUserFunctions.Contains(FUNCTION_PAGE_ID))
            //        Response.Redirect("AccessDenied.aspx");
            //}
            //else
            //{
            //    CurrentUserFunctions = RoleManager.GetFunctionIdsByUserName(userName);
            //}
            List<string> currentUserFunctions = RoleManager.GetFunctionIdsByUserName(userName);
            if (currentUserFunctions != null)
            {
                if (!currentUserFunctions.Contains(FUNCTION_PAGE_ID))
                    Response.Redirect("~/Pages/AccessDenied.aspx");
            }
            else
                Response.Redirect("~/Pages/AccessDenied.aspx");
        }

        public List<string> CurrentUserFunctions
        {
            get
            {
                if (session[SessionPrefix + "CurrentUserFunctions"] != null)
                    return (List<string>)session[SessionPrefix + "CurrentUserFunctions"];

                return null;
            }
            set
            {
                session[SessionPrefix + "CurrentUserFunctions"] = value;
            }
        }

        public virtual string FUNCTION_PAGE_ID
        {
            get
            {
                return "APEM";
            }
        }

        //public void RunScript(string scriptName, string param)
        //{
        //    string script = string.Format("{0}({1})", scriptName, param);
        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RunScript", script, true);
        //}
        public void SaveLogging(string activity, string description)
        {
            SaveLogging(activity, FUNCTION_PAGE, description);
        }
        /// <summary>
        /// Lưu lịch sử hoạt động
        /// </summary>
        /// <param name="log">Hoạt động</param>
        /// <param name="functionPage">tên trang</param>
        /// <param name="description">miêu tả</param>
        public void SaveLogging(string activity, string functionPage, string description)
        {
            LoggingManager.LogAction(activity, functionPage, description);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        protected void ProcessException(Exception ex)
        {
            SendErrorMail(ex.Message, ex.StackTrace);
        }

        public void SendErrorMail(string errorMessage, string stackTrace)
        {
            //Ghi vào logging
            LoggingManager.LogAction(ActivityLoggingHelper.ERROR, FUNCTION_PAGE_ID, errorMessage + "///////" + stackTrace);
            //Gửi mail
            string emailSender = "APEM System";
            string emailFromAddress = SettingManager.GetSettingValue(SettingNames.SmtpSenderAccount);
            string emailToAddress = SettingManager.GetSettingValue(SettingNames.BugReportEmail);
            string subject = string.Format("APEM System Error: {0}",
                errorMessage).Replace("\t", " ").Replace("\r", "").Replace("\n", " ");

            StringBuilder strMessage = new StringBuilder();
            strMessage.AppendLine(string.Format("There was a unexpected system error: {0}", errorMessage));
            strMessage.AppendLine();
            strMessage.AppendLine("-------------------Error Detail-------------------");
            strMessage.AppendLine();
            strMessage.AppendLine(string.Format("Server Name: {0}", Server.MachineName));
            strMessage.AppendLine(string.Format("Application Name: {0}", SecurityHelper.APPLICATION_NAME));
            strMessage.AppendLine(string.Format("User Name: {0}", ApplicationContext.Current.UserName));
            strMessage.AppendLine(string.Format("IP Address: {0}", ApplicationContext.Current.CurrentUserIp));
            strMessage.AppendLine(string.Format("Recorded Date: {0}", DateTime.Now.ToString()));
            strMessage.AppendLine(string.Format("URL: {0}", ApplicationContext.Current.CurrentUri.AbsoluteUri));
            strMessage.AppendLine(string.Format("Error Detail: {0}", errorMessage));
            strMessage.AppendLine("STACK TRACE:");
            strMessage.AppendLine(stackTrace);

            CommonHelper.SendSmtpMail(emailSender, emailFromAddress, emailToAddress, subject, strMessage.ToString(), false);
        }
        /// <summary>
        /// Kích hoạt click trên button
        /// </summary>
        /// <param name="buttonid">ID button</param>
        /// <param name="clientid">Id dạng client</param>
        protected void PerformanceButtonClick(string buttonid, bool clientid)
        {
            RunScriptC("PerformanceButtonClick", string.Format("'{0}',{1}", buttonid, clientid.ToString().ToLower()));
        }
        /// <summary>
        /// Lấy url đầy đủ cho url tương đối
        /// </summary>
        /// <param name="path">Url tương đối</param>
        /// <returns></returns>
        protected string GetServerPath(string url)
        {
            return Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/") + url;
        }
        /// <summary>
        /// Hàm lấy resource text
        /// </summary>
        /// <param name="filename">tên file</param>
        /// <param name="resourceKey">khóa</param>
        /// <returns></returns>
        private string GetResourceText(string filename, string resourceKey)
        {
            object value = GetGlobalResourceObject(filename, resourceKey);
            if (value != null)
                return value.ToString();
            return resourceKey;
        }
        /// <summary>
        /// Lấy resource text cho control
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected string GetRSText(string resourceKey)
        {
            return GetResourceText(string.Format("APEMResourcesText_{0}", ApplicationContext.Current.CurrentLanguageCode.Replace("-", "_")), resourceKey);
        }

        #region Trunglc Add - 15/05/2015

        private Dictionary<string, ValueObject> GetDataControlPage(bool isNow)
        {
            ContentPlaceHolder MainContent = Page.Master.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;

            Dictionary<string, ValueObject> dicxNow = new Dictionary<string, ValueObject>();
            foreach (Control ctrl in MainContent.Controls)
            {
                if (ctrl.GetType().Equals(typeof(ExtraInputMask)))
                {
                    ValueObject vo = new ValueObject();
                    vo.Label = ((ExtraInputMask)ctrl).ToolTip;
                    vo.Value = ((ExtraInputMask)ctrl).Suffix != null ? 
                                ((ExtraInputMask)ctrl).Text.Replace(((ExtraInputMask)ctrl).Suffix, string.Empty) :
                                ((ExtraInputMask)ctrl).Text;
                    vo.Type = typeof(ExtraInputMask).ToString();

                    dicxNow.Add(ctrl.ID, vo);
                }
                if (ctrl.GetType().Equals(typeof(DropDownList)))
                {
                    ValueObject vo = new ValueObject();
                    vo.Label = ((DropDownList)ctrl).ToolTip;
                    vo.Value = ((DropDownList)ctrl).SelectedItem.Text;
                    vo.Type = typeof(DropDownList).ToString();

                    dicxNow.Add(ctrl.ID, vo);
                }

                if (ctrl.GetType().Equals(typeof(GridView)) || ctrl.GetType().Equals(typeof(GridviewExtension)))
                {
                    string htmlData = string.Empty;

                    if (isNow)
                    {
                        if (Session[ViewState_PageID + "DataSource_" + ctrl.ID + "_Now"] != null)
                        {
                            htmlData = Session[ViewState_PageID + "DataSource_" + ctrl.ID + "_Now"] as string;
                        }
                    }
                    else
                    {
                        if (Session[ViewState_PageID + "DataSource_" + ctrl.ID + "_Before"] != null)
                        {
                            htmlData = Session[ViewState_PageID + "DataSource_" + ctrl.ID + "_Before"] as string;
                        }
                    }

                    ValueObject vo = new ValueObject();
                    vo.Label = ((GridView)ctrl).ToolTip;
                    vo.Value = htmlData;
                    vo.Type = typeof(GridView).ToString();

                    dicxNow.Add(ctrl.ID, vo);
                }

                if (ctrl.GetType().Equals(typeof(CheckBox)))
                {
                    ValueObject vo = new ValueObject();
                    vo.Label = ((CheckBox)ctrl).ToolTip;
                    vo.Value = ((CheckBox)ctrl).Checked;
                    vo.Type = typeof(CheckBox).ToString();

                    dicxNow.Add(ctrl.ID, vo);
                }
                if (ctrl.GetType().Equals(typeof(RadioButton)))
                {
                    ValueObject vo = new ValueObject();
                    vo.Label = ((RadioButton)ctrl).ToolTip;
                    vo.Value = ((RadioButton)ctrl).Checked;
                    vo.Type = typeof(RadioButton).ToString();

                    dicxNow.Add(ctrl.ID, vo);
                }
                if (ctrl.GetType().Equals(typeof(CustomExtraTextbox)))
                {
                    ValueObject vo = new ValueObject();
                    vo.Label = ((CustomExtraTextbox)ctrl).ToolTip;
                    vo.Value = ((CustomExtraTextbox)ctrl).Text;
                    vo.Type = typeof(CustomExtraTextbox).ToString();

                    dicxNow.Add(ctrl.ID, vo);
                }
                if (ctrl.GetType().Equals(typeof(TextBox)))
                {
                    ValueObject vo = new ValueObject();
                    vo.Label = ((TextBox)ctrl).ToolTip;
                    vo.Value = ((TextBox)ctrl).Text;
                    vo.Type = typeof(TextBox).ToString();

                    dicxNow.Add(ctrl.ID, vo);
                }
            }
            return dicxNow;
        }

        protected void SaveBaseDataBeforeEdit()
        {
            Dictionary<string, ValueObject> dicxNow = GetDataControlPage(false);
            Session[ViewState_PageID + "DataBeforeEdit"] = dicxNow;
        }

        protected Dictionary<string, ValueObject> GetDataAfterEdit()
        {
            Dictionary<string, ValueObject> dicxNow = GetDataControlPage(true);
            return dicxNow;
        }

        protected void LoggingActions(string typeObj, string action, string status, string objdata = null)
        {
            ObjLogs newObjLogs = new ObjLogs();
            newObjLogs.Action = action;
            newObjLogs.Status = status;
            newObjLogs.TypeObject = typeObj;

            if (action == LogsAction.Objects.Action.UPDATE)
            {
                Dictionary<string, ValueObject> dicxBefore = Session[ViewState_PageID + "DataBeforeEdit"] as Dictionary<string, ValueObject>;
                Dictionary<string, ValueObject> dicxNow = GetDataAfterEdit();
                List<JsonData> lstJsonData = JsonConvert.DeserializeObject<List<JsonData>>(objdata);
                List<JsonData> lstObjData = new List<JsonData>();
                lstObjData = GenerateListJsonDataLogs(dicxBefore, dicxNow);
                lstJsonData.AddRange(lstObjData);
                newObjLogs.JsonDatas = JsonConvert.SerializeObject(lstJsonData);
                Session[ViewState_PageID + "DataBeforeEdit"] = dicxNow;
            }
            else
            {
                newObjLogs.JsonDatas = objdata;
            }
            DataLogsManager.LogAction(TypeActionLogs.Data, JsonHelper.Serialize<ObjLogs>(newObjLogs), Request.UserHostAddress);
        }

        protected List<JsonData> GenerateListJsonDataLogs(Dictionary<string, ValueObject> dicBefore, Dictionary<string, ValueObject> dicAfter)
        {
            List<JsonData> retJsonData = new List<JsonData>();

            foreach (var item in dicAfter)
            {
                if (dicBefore != null)
                {
                    ValueObject voBefore = dicBefore[item.Key] as ValueObject;
                    ValueObject voAfter = item.Value as ValueObject;

                    if (voBefore.Value != null)
                    {
                        if (voAfter.Value != null)
                        {
                            if (!voBefore.Value.Equals(voAfter.Value))
                            {
                                if (voAfter.Type == typeof(GridView).ToString() || voAfter.Type == typeof(GridviewExtension).ToString())
                                {
                                    string sOldData = string.Empty;
                                    string sNewData = string.Empty;

                                    sOldData = Session[ViewState_PageID + "DataSource_" + item.Key + "_Before"] as string;
                                    sNewData = Session[ViewState_PageID + "DataSource_" + item.Key + "_Now"] as string;
                                    Session.Remove(ViewState_PageID + "DataSource_" + item.Key + "_Before");
                                    Session[ViewState_PageID + "DataSource_" + item.Key + "_Before"] = sNewData;

                                    string s_CompareOld = Regex.Replace(sOldData, @"\s+", String.Empty);
                                    string s_CompareNew = Regex.Replace(sNewData, @"\s+", String.Empty);

                                    if (!String.Equals(s_CompareOld, s_CompareNew, StringComparison.OrdinalIgnoreCase))
                                    {
                                        Json newJ = new Json();
                                        newJ.OldValue = sOldData;
                                        newJ.NewValue = sNewData;
                                        newJ.Type = voAfter.Type;

                                        JsonData newJData = new JsonData();
                                        newJData.Title = voBefore.Label;
                                        newJData.Data = JsonConvert.SerializeObject(newJ);
                                        retJsonData.Add(newJData);
                                    }
                                }
                                else
                                {

                                    Json newJ = new Json();
                                    newJ.OldValue = voBefore.Value.ToString();
                                    newJ.NewValue = voAfter.Value.ToString();
                                    newJ.Type = voAfter.Type;

                                    JsonData newJData = new JsonData();
                                    newJData.Title = voBefore.Label;
                                    newJData.Data = JsonConvert.SerializeObject(newJ);
                                    retJsonData.Add(newJData);
                                }
                            }
                        }
                        else
                        {
                            Json newJ = new Json();
                            newJ.OldValue = voBefore.Value.ToString();
                            newJ.NewValue = null;
                            newJ.Type = voAfter.Type;

                            JsonData newJData = new JsonData();
                            newJData.Title = voBefore.Label;
                            newJData.Data = JsonConvert.SerializeObject(newJ);

                            retJsonData.Add(newJData);
                        }
                    }
                    else
                    {
                        if (voAfter.Value != null)
                        {
                            Json newJ = new Json();
                            newJ.OldValue = null;
                            newJ.NewValue = voAfter.Value.ToString();
                            newJ.Type = voAfter.Type;

                            JsonData newJData = new JsonData();
                            newJData.Title = voBefore.Label;
                            newJData.Data = JsonConvert.SerializeObject(newJ);

                            retJsonData.Add(newJData);
                        }
                    }
                }
            }

            return retJsonData;
        }


        protected string ViewState_PageID
        {
            get
            {
                return ViewState["PageID"] != null ? ViewState["PageID"].ToString() : string.Empty;
            }
        }

        [WebMethod(EnableSession = true)]
        public static bool SaveDataTable(string key, string data, string PageID)
        {
            HttpContext.Current.Session[PageID + "DataSource_" + key] = data;
            return true;
        }

        #endregion

        #endregion

        #region User Role
        //public void CheckUserRolePermission(string userName)
        //{
        //    if (CurrentRoleFunction != null && CurrentRoleFunction.Count > 1)
        //    {
        //        if (!CurrentRoleFunction.ContainsKey(FUNCTION_PAGE) && !FUNCTION_PAGE.Equals(PageFunctionName.ACCESS_DENIED))
        //        {
        //            //if (Request.RawUrl.Contains("/pages/"))
        //            //    Response.Redirect("~/AccessDenied.aspx");
        //            Response.Redirect("AccessDenied.aspx");
        //        }
        //    }
        //    else
        //    {
        //        Dictionary<string, PtRoleFunction> roleFunction = null;
        //        PtFunctionCollection function = null;
        //        RoleManager.GetRoleFunctionByUserName(userName, ref function, ref roleFunction);
        //        CurrentFunction = function;
        //        CurrentRoleFunction = roleFunction;

        //    }
        //}
        /// <summary>
        /// Lấy chức năng người dùng theo code
        /// </summary>
        /// <param name="fCode">Mã code chức năng</param>
        /// <returns></returns>
        //protected PtFunction GetFunctionByCode(string fCode)
        //{
        //    if (CurrentFunction != null)
        //    {
        //        int idx = CurrentFunction.Find(PtFunction.Columns.Code, fCode);
        //        if (idx > -1)
        //            return CurrentFunction[idx];

        //    }
        //    return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fCode"></param>
        ///// <returns></returns>
        //protected PtRoleFunction GetRoleFunctionByFunctionCode(string fCode)
        //{
        //    if (CurrentRoleFunction != null)
        //    {
        //        return CurrentRoleFunction[fCode];
        //    }

        //    return null;
        //}
        #endregion

        #region MasterPage Functions
        /// <summary>
        /// Hàm thêm shortcut cho control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="shortcut"></param>
        /// <param name="shortCutEvent"></param>
        public void ControlShortCutAdd(Control control, string shortcut, ShortCutEvent shortCutEvent)
        {
            switch (shortCutEvent)
            {
                case ShortCutEvent.Buttonclick:
                    RunScriptC("AddButtonShortCut", string.Format("'{0}',false,'{1}'", control.ID, shortcut));
                    break;
            }
        }
        /// <summary>
        /// Kích hoạt postback cho ajaxPanel
        /// </summary>
        /// <param name="ajaxPanelClientID">ClientID của panel</param>
        /// <param name="command">tên lệnh khi postback</param>
        public void AjaxPanelTriggerUpdate(string ajaxPanelClientID, string uniqueId, string command)
        {
            RunScriptC("AjaxPanelTriggerUpdate", string.Format("'{0}','{1}','{2}'", ajaxPanelClientID, uniqueId, command));
            //RunScript("AjaxPanelTriggerUpdate", string.Format("'{0}','{1}','{2}'", ajaxPanelClientID, uniqueId, command));
        }
        /// <summary>
        /// Hàm mở 1 window theo client id
        /// </summary>
        /// <param name="windowClientId"></param>
        protected void OpenWindow(string windowClientId)
        {
            OpenWindow(windowClientId, string.Empty);
        }

        protected void OpenWindow(string windowClientId, string title)
        {
            RunScript("OpenRadWindow", string.Format("'{0}','{1}'", windowClientId, title));
        }
        /// <summary>
        /// Hàm đóng window đang mở 
        /// </summary>
        public void CloseWindow()
        {
            RunScript("CloseRadWindDow", string.Empty);
        }
        /// <summary>
        /// Hàm đóng window theo id
        /// </summary>
        /// <param name="idWindow"></param>
        public void CloseWindow(string idWindow)
        {
            RunScript("CloseRadWindowByID", string.Format("'{0}'", idWindow));
        }
        /// <summary>
        /// Mở dialog xác nhận yêu cầu
        /// </summary>
        /// <param name="message">Thông báo</param>
        /// <param name="result">kết quả</param>
        /// <param name="isClosePostBack">Đóng dialog từ client (không postback)</param>
        public virtual void OpenMessageBox(MessageBox message, ModalConfirmResult result, bool isClosePostBack, bool showmodal)
        {
            if (CURRENT_MASTERPAGES != null)
            {
                CURRENT_MASTERPAGES.OpenMessageBox(message, result, isClosePostBack, showmodal);
            }
        }

        public virtual void CloseMessageBox()
        {
            if (CURRENT_MASTERPAGES != null)
            {
                CURRENT_MASTERPAGES.CloseMessageBox();
            }
        }

        #endregion

        #region Inherit Functions
        /// <summary>
        /// Hàm cập nhật các control cần xử lý Ajax
        /// </summary>
        protected virtual void AjaxControlsPerformance() { }
        /// <summary>
        /// Hàm cập nhật shortcut các control
        /// </summary>
        protected virtual void ShortCutControlsPerformance() { }
        /// <summary>
        /// Hàm trả về từ dialog confirm
        /// </summary>
        /// <param name="e"></param>
        public virtual void ConfirmRequest(ModalConfirmResult e)
        {
            if (e.FireInControl)
            {
                ModalBasePage modalBasepage = FindControl(e.ControlName) as ModalBasePage;
                if (modalBasepage != null)
                {
                    modalBasepage.ConfirmRequest(e);
                    return;
                }
            }
        }
        /// <summary>
        /// Hàm trả về từ message box
        /// </summary>
        public virtual void MessageBoxClose() { }
        /// <summary>
        /// Hàm tập trung resource text trong trang
        /// </summary>
        protected virtual void PageResourceText() { }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            session = ApplicationContext.Current.Session;
            CheckFunctionPermission(UserManager.GetLoggedOnUsername());
            base.OnInit(e);
            PageResourceText();

        }

        #region Event
        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            AjaxControlsPerformance();
            if (!IsPostBack)
            {
                //RunScriptC("ClosedFormLoading", string.Empty);
                ShortCutControlsPerformance();
                RegisterScriptInputMask();
            }

        }

        //ReFormat NumbericTexbox - Khoa's Development
        void RegisterScriptInputMask()
        {
            ClientScriptManager cs = this.ClientScript;

            // Define the resource name and type.
            Type thisT = typeof(ExtraInputMask);

            // Check to see if the startup script is already registered.
            if (!cs.IsStartupScriptRegistered(thisT, "SweetSoftCMS.ExtraControls.setInputMaskState"))
            {
                string thisvalidate = "SweetSoftCMS.ExtraControls.scripts.jquery.inputmask.bundle.min.js";
                string template = "<script type='text/javascript' src='{0}'></script>";
                // Get a ClientScriptManager reference from the Page class.
                String str = string.Format(template, cs.GetWebResourceUrl(thisT, thisvalidate));
                // Register the client resource with the page.
                cs.RegisterStartupScript(thisT, "SweetSoftCMS.ExtraControls.setInputMaskState", str, false);
            }
        }

        protected static void MarkAsSaved()
        {
            Page page = (System.Web.UI.Page)System.Web.HttpContext.Current.Handler;
            if (page != null && page.Master != null)
            {
                Literal ltrHasSaved = page.Master.FindControl("ltrHasSaved") as Literal;
                if (ltrHasSaved != null)
                    ltrHasSaved.Text = "<input type='hidden' id='hdfHasSaved' value='1' />";
            }
        }

        #endregion

        #region Validation

        private List<string> m_PromptControdClientIDs;
        public List<string> PromptControdClientIDs
        {
            get
            {
                if (m_PromptControdClientIDs == null)
                    m_PromptControdClientIDs = new List<string>();
                return m_PromptControdClientIDs;
            }
            set
            {
                m_PromptControdClientIDs = value;
            }
        }

        /// <summary>
        /// Tình trạng lỗi
        /// </summary>
        protected bool IsValid
        {
            get
            {
                return PromptControdClientIDs.Count == 0;
            }
        }

        #endregion      

        #region Web Methob

        [WebMethod]
        public static string GetCustomerData(string Keyword)
        {
            List<TblCustomer> result = new List<TblCustomer>();
            result = CustomerManager.SelectByKeyword(Keyword);
            string ret = new JavaScriptSerializer().Serialize(result);
            return ret;
        }

        #endregion
    }

    public class ModalConfirmResult
    {
        private bool submit = false;
        private bool fireInControl = false;
        private object value = null;
        private string controlName = string.Empty;
        private string commandName = string.Empty;
        public ModalConfirmResult() { }
        public bool Submit
        {
            get { return this.submit; }
            set { this.submit = value; }
        }

        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public bool FireInControl
        {
            get { return this.fireInControl; }
            set { this.fireInControl = value; }
        }

        public string ControlName
        {
            get { return this.controlName; }
            set { this.controlName = value; }
        }

        public string CommandName
        {
            get { return this.commandName; }
            set { this.commandName = value; }
        }
    }
}
