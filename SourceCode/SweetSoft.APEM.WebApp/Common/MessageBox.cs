using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSoft.APEM.WebApp.Common
{
    public enum MSGButton
    {
        OK,
        YesNo,
        DeleteCancel,
        AcceptCancel,
        Ok_With_Reload
    }

    public enum MSGIcon
    {
        Success,
        Info,
        Warning,
        Error
    }

    public class MessageBox
    {
        public string MessageTitle
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public MSGButton MessageButton
        {
            set;
            get;
        }

        public MSGIcon MessageIcon
        {
            set;
            get;
        }

        public MessageBox()
        {

        }

        public MessageBox(string messageTitle, string message, MSGButton messageButton, MSGIcon messageIcon)
        {
            MessageTitle = messageTitle;
            Message = message;
            MessageButton = messageButton;
            MessageIcon = messageIcon;
        }
    }
}