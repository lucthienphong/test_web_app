using SweetSoft.APEM.Core;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class NotificationDetailAll : ModalBasePage
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
                //BindData();
            }
        }

        void BindData()
        {

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

            NotificationSetting.Save();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/Notification.aspx");
        }
    }
}