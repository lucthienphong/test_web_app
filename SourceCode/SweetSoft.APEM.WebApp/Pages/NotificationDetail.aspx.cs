using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class NotificationDetail : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "notification_setting";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        void BindData()
        {
            string id = CommonHelper.QueryString("Id");
            if (string.IsNullOrEmpty(id) == false)
            {
                NotificationPage page = RealtimeNotificationManager.GetNotificationPageById(id);
                if (page != null)
                {
                    txtId.Text = page.id;
                    txtTitle.Text = page.title;
                }
            }
        }

        protected void btnNSave_Click(object sender, EventArgs e)
        {
            //Kiểm tra quyền
            if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
            {
                MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE),
                    ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), 
                    MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msgRole, null, false, false);
                return;
            }

            string id = CommonHelper.QueryString("Id");
            if (string.IsNullOrEmpty(id) == false)
            {
                NotificationPage page = RealtimeNotificationManager.GetNotificationPageById(id);
                if (page != null)
                {
                    TblFunction function = new SubSonic.Select().From(TblFunction.Schema)
                        .Where(TblFunction.FunctionIDColumn).IsEqualTo(page.functionid)
                        .ExecuteSingle<TblFunction>();
                    if (function != null)
                    {
                        if (function.Title != txtTitle.Text.Trim())
                        {
                            function.Title = txtTitle.Text.Trim();
                            new TblFunctionController().Update(function.FunctionID,
                                function.ParentID, function.Title, function.Description,
                                function.DisplayOrder);
                        }
                    }
                }
            }
            NotificationSettingInsert.Save();
            NotificationSettingUpdate.Save();
            NotificationSettingDelete.Save();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/Notification.aspx");
        }
    }
}