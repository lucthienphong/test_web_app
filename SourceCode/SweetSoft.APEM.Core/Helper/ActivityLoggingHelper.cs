using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Helper
{
    public class ActivityLoggingHelper
    {

        public const string LOGIN = "login";
        public const string LOGOUT = "logout";
        public const string DELETE = "delete";
        public const string UPDATE = "update";
        public const string INSERT = "insert";
        public const string FORGOT_PASSWORD = "forgotpassword";
        public const string CHANGE_PASSWORD = "changepassword";
        public const string ERROR = "error";
    }

    public class MessageHelper {
        public const string CREATED = "c";
        public const string RECEIVED = "r";

        public const string CODE_PIPE = "CODE_PIPE";
        public const string CODE_GRAPHIC = "CODE_GRAPHIC";
    }

    public class ConstantHelper {
        public const string UPLOAD_TEMP = @"~\Uploads\temp";
    }
}
