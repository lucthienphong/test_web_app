using SweetSoft.APEM.Core;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.MasterPages
{
    public partial class ModalMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Mở dialog xác nhận yêu cầu
        /// </summary>
        /// <param name="message">Thông báo</param>
        /// <param name="result">Kết quả</param>
        /// <param name="ispostback">Đóng form từ client (không postback)</param>
        public void OpenMessageBox(MessageBox msg, ModalConfirmResult result, bool isClosePostBack, bool showmodal)
        {
            lbTitle.Text = msg.MessageTitle;
            lbMessage.Text = msg.Message;
            switch (msg.MessageButton)
            {
                case Common.MSGButton.OK:
                    btnAccept.Visible = false;
                    btnCloseMessage.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.CLOSE);
                    break;
                case Common.MSGButton.Ok_With_Reload:
                    btnAccept.Visible = true;
                    btnAccept.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.CLOSE);
                    btnAccept.CssClass = "btn btn-primary";
                    btnCloseMessage.Visible = false;
                    break;
                case Common.MSGButton.YesNo:
                    btnAccept.Visible = true;
                    btnAccept.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.YES);
                    btnAccept.CssClass = "btn btn-primary";
                    btnCloseMessage.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.NO);
                    break;
                case Common.MSGButton.AcceptCancel:
                    btnAccept.Visible = true;
                    btnAccept.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.ACCEPT);
                    btnAccept.CssClass = "btn btn-warning";
                    btnCloseMessage.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.CANCEL);
                    break;
                case Common.MSGButton.DeleteCancel:
                    btnAccept.Visible = true;
                    btnAccept.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.DELETE);
                    btnAccept.CssClass = "btn btn-danger";
                    btnCloseMessage.Text = ResourceTextManager.GetApplicationText(ResourceTextIDsFrontEnd.CANCEL);
                    break;
            }
            switch (msg.MessageIcon)
            {
                case MSGIcon.Error:
                    MessageHeader.Attributes.Add("class", "modal-header alert alert-danger");
                    break;
                case MSGIcon.Info:
                    MessageHeader.Attributes.Add("class", "modal-header alert alert-info");
                    break;
                case MSGIcon.Success:
                    MessageHeader.Attributes.Add("class", "modal-header alert alert-success");
                    break;
                case MSGIcon.Warning:
                    btnAccept.CssClass = "btn btn-warning";
                    MessageHeader.Attributes.Add("class", "modal-header alert alert-warning");
                    break;
            }
            RunScript("showMessageBox", "'#MessageModel'");
        }
        /// <summary>
        /// Hàm đóng confirm dialog
        /// </summary>
        public void CloseMessageBox()
        {
            RunScript("closeMessageBox", "'#MessageModel'");
        }

        private void RunScript(string scriptName, string param)
        {
            string script = string.Format("{0}({1});", scriptName, param);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RunScript", script, true);
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            ModalConfirmResult result = CurrentConfirmResult;
            if (result == null)
                result = new ModalConfirmResult();
            result.Submit = true;
            if (CURRENT_PAGE != null)
            {
                CURRENT_PAGE.ConfirmRequest(result);
                CurrentConfirmResult = null;
            }
        }

        protected ModalBasePage CURRENT_PAGE
        {
            get
            {
                try
                {
                    if (this.Page is ModalBasePage)
                        return (ModalBasePage)this.Page;
                }
                catch (Exception) { }
                return null;
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
    }
}