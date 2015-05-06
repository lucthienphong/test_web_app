using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Controls
{
    public partial class NotificationSettingAll : System.Web.UI.UserControl
    {
        public CommandType CommandType
        {
            get;
            set;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            txtNContentFormat.config.toolbar = txtOtherMessage.config.toolbar = new object[]
			{
				new object[] {
                    "Source",
                    "-",
                    "Bold", "Italic", "Underline", "Strike",
                    "-",
                    "Subscript", "Superscript",
                    "-",
                    "NumberedList", "BulletedList",
                    "-",
                    "Link", "Unlink",
                    "-",
                    "TextColor", "BGColor",
                },
                "/"
                ,
                new object[] {
                    "Cut", "Copy", "Paste", "PasteText", "PasteFromWord"
                },
                new object[] {
                    "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock"
                },
                new object[] {
                    "Styles", "Format", "Font", "FontSize"
                }
			};

            if (!IsPostBack)
            {
                BindData();
            }
        }

        void BindData()
        {
            txtNContentFormat.Language = txtOtherMessage.Language = ApplicationContext.Current.CurrentLanguageCode.Substring(2).ToLower();

            List<NotificationPage> lst = RealtimeNotificationManager.GetAllAvaliblePage();
            if (lst != null && lst.Count > 0)
            {
                ddlAvailablePage.Items.Clear();
                ddlAvailablePage.Items.Add(new ListItem("Select a page", ""));
                ddlAvailablePage.Items.Add(new ListItem("Open current page", "-2"));
                foreach (NotificationPage item in lst)
                {
                    ddlAvailablePage.Items.Add(new ListItem(item.url, item.id));
                }
                ddlAvailablePage.Items.Add(new ListItem("Custom page", "-1"));
            }

            if (CommandType == CommandType.Delete)
                chkOpenPage.Checked = false;

            string id = CommonHelper.QueryString("Id");
            if (string.IsNullOrEmpty(id) == false)
            {
                NotificationPage page = RealtimeNotificationManager.GetNotificationPageById(id);
                if (page != null)
                {
                    TblNotificationSetting notisetting = RealtimeNotificationManager.GetNotificationSettingByPageIdAndCommandType(id, CommandType);
                    if (notisetting != null)
                    {
                        txtID.Value = notisetting.PageId.ToString();
                        txtNContentFormat.Text = notisetting.Description;
                        txtNTitle.Value = notisetting.Title;
                        txtNButtonText.Value = notisetting.TriggerButton != null && notisetting.TriggerButton.Length > 0 ? notisetting.TriggerButton :
                            ResourceTextManager.GetApplicationText(ResourceText.VIEW_DETAIL);
                        chkIsObsolete.Checked = notisetting.IsObsolete;

                        if (notisetting.DismissEvent != null && notisetting.DismissEvent.Length > 0)
                        {
                            if (ddlDismissEvent.Items.FindByValue(notisetting.DismissEvent) != null)
                                ddlDismissEvent.Items.FindByValue(notisetting.DismissEvent).Selected = true;
                        }

                        #region RegionName

                        if (notisetting.Actions.Length > 0)
                        {
                            string encrypt = SecurityHelper.Encrypt("|");
                            string join = SecurityHelper.Encrypt(",");
                            string[] arr = notisetting.Actions.Split(new string[] { join }, StringSplitOptions.None);
                            if (arr != null && arr.Length > 0)
                            {
                                #region open page

                                string[] openPage = arr[0].Split(new string[] { encrypt }, StringSplitOptions.None);
                                if (openPage != null && openPage.Length > 0)
                                {
                                    if (ddlAvailablePage.Items.FindByValue("-2") != null)
                                        ddlAvailablePage.Items.FindByValue("-2").Selected = true;

                                    if (openPage.Length > 2 && openPage[2].Length > 0)
                                        txtNTitleWindow.Value = openPage[2];
                                    else
                                        txtNTitleWindow.Value = ResourceTextManager.GetApplicationText(ResourceText.JOB_NOTIFICATION_IFRAME_TITLE);
                                }

                                #endregion

                                if (arr.Length == 2)
                                {
                                    chkOpenMesage.Checked = true;
                                    string[] message = arr[1].Split(new string[] { encrypt }, StringSplitOptions.None);
                                    if (message != null && message.Length > 0)
                                        txtOtherMessage.Text = message[1];
                                }
                            }
                        }
                        else
                        {
                            //set default
                            txtNTitleWindow.Value = ResourceTextManager.GetApplicationText(ResourceText.JOB_NOTIFICATION_IFRAME_TITLE);
                            ddlAvailablePage.SelectedIndex = 1;
                        }
                        #endregion

                        if (notisetting.ReceiveIds != null && notisetting.ReceiveIds.Length > 0)
                            SelectReceivers1.DataReceive = notisetting.ReceiveIds;
                    }
                }
            }
        }

        public void Save()
        {
            string commandType = CommandType.ToString();
            if (string.IsNullOrEmpty(commandType))
                return;

            //string noHTML = Regex.Replace(ContentFormat1.Text, @"<[^>]+>|&nbsp;", "").Trim();
            //string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");

            string newTest = HtmlHelper.UnHtml(txtNContentFormat.Text);
            if (string.IsNullOrEmpty(newTest.Trim()) || string.IsNullOrEmpty(txtNTitle.Value.Trim()))
                return;

            string id = CommonHelper.QueryString("Id");

            TblNotificationSetting notiSetting = null;

            string encrypt = SecurityHelper.Encrypt("|");
            string join = SecurityHelper.Encrypt(",");
            bool isExists = false;
            TblNotificationSettingCollection all = RealtimeNotificationManager.GetNotificationSettingByPageId(id);
            if (all != null && all.Count > 0)
            {
                isExists = true;
                notiSetting = all[0];
            }
            else
            {
                notiSetting = new TblNotificationSetting();
                notiSetting.PageId = RealtimeNotificationManager.CreateNotificationNumber();
                notiSetting.CreatedBy = ApplicationContext.Current.UserName;
                notiSetting.CreatedOn = DateTime.Now;
            }

            List<object> lst = SelectReceivers1.GetDataReceiver();
            if (lst != null && lst.Count == 2)
            {
            }
            else
                return;

            notiSetting.ReceiveIds = lst[1].ToString() == NotificationType.Staff.ToString() ?
                lst[0].ToString() : string.Join(",", (lst[0] as List<int>).Select(x => x.ToString()).ToArray());
            notiSetting.ReceiveType = lst[1].ToString();

            notiSetting.CommandType = commandType;
            notiSetting.Description = txtNContentFormat.Text.Trim();
            List<string> lstActions = new List<string>();
            if (chkOpenPage.Checked && string.IsNullOrEmpty(ddlAvailablePage.Value) == false)
            {
                //open particular page
                string openPage = ddlAvailablePage.Value;
                if (openPage == "-1")
                    openPage = txtOtherUrl.Value.Trim();

                //open extra message
                lstActions.Add(chkOpenPage.Value + encrypt + openPage +
                    encrypt + (txtNTitleWindow.Value.Length > 0 ? txtNTitleWindow.Value : "[Global]"));
                if (chkOpenMesage.Checked)
                    lstActions.Add(chkOpenMesage.Value + encrypt + txtOtherMessage.Text.Trim());

                notiSetting.TriggerButton = txtNButtonText.Value != null && txtNButtonText.Value.Length > 0 ? txtNButtonText.Value : "Open";

            }
            notiSetting.Actions = string.Join(join, lstActions.ToArray());
            notiSetting.Title = txtNTitle.Value.Trim();
            notiSetting.IsObsolete = chkIsObsolete.Checked;
            notiSetting.DismissEvent = ddlDismissEvent.Value;

            if (isExists)
            {
                notiSetting = RealtimeNotificationManager.UpdateNotificationSetting(notiSetting);
                RealtimeNotificationManager.UpdateReceiveIds(notiSetting.PageId,
                    notiSetting.CommandType, notiSetting.ReceiveIds, notiSetting.Title, notiSetting.Description);
            }
            else
            {
                notiSetting = RealtimeNotificationManager.InsertNotificationSetting(notiSetting);
                TblNotification noti = new TblNotification();
                noti.Actions = notiSetting.Actions;
                noti.CommandType = notiSetting.CommandType;
                noti.Contents = notiSetting.Description;
                noti.CreatedBy = notiSetting.CreatedBy;
                noti.CreatedOn = notiSetting.CreatedOn.Value;
                noti.DateDismiss = string.Empty;
                noti.DismissBy = string.Empty;
                noti.DismissEvent = notiSetting.DismissEvent;
                noti.IsObsolete = false;
                noti.PageId = notiSetting.PageId;
                noti.ReceiveIds = notiSetting.ReceiveIds;
                noti.ReceiveType = notiSetting.ReceiveType;
                noti.Title = notiSetting.Title;
                RealtimeNotificationManager.Insert(noti);
            }

            ltrDelete.Text = "<input type='hidden' id='hdfdone' value='1' />";
        }

        protected void btnNDelete_Click(object sender, EventArgs e)
        {
            ltrDelete.Text = string.Empty;
            try
            {
                string id = CommonHelper.QueryString("Id");
                if (string.IsNullOrEmpty(id) == false)
                {
                    bool isDeleted = RealtimeNotificationManager.DeleteNotificationSettingByPageIdAndCommandType(id, CommandType);
                    if (isDeleted)
                    {
                        ltrDelete.Text = "<input type='hidden' id='hdfdone' value='1' />";
                        txtNTitle.Value = txtOtherMessage.Text = txtOtherUrl.Value = txtNContentFormat.Text = string.Empty;
                        chkIsObsolete.Checked = chkOpenMesage.Checked = chkOpenPage.Checked = false;
                        if (ddlAvailablePage.Items.Count > 0)
                            ddlAvailablePage.SelectedIndex = 0;
                        if (ddlDismissEvent.Items.Count > 0)
                            ddlDismissEvent.SelectedIndex = 0;

                        txtNButtonText.Value = "Open";
                        SelectReceivers1.UncheckAllDepartment();
                    }
                    else
                        ltrDelete.Text = "<input type='hidden' id='hdfdone' value='' />";
                }
            }
            catch (Exception ex)
            {

            }

        }

    }
}