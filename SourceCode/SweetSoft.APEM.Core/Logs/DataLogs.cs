using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Logs
{
    public enum NumSortType
    {
        ASC,
        DESC
    }
    public class Sort
    {
        public int ColIndex = 1;
        public NumSortType Type = NumSortType.ASC;
    }

    public enum TypeActionLogs
    {
        [Description("Authentication")]
        Authentication,
        [Description("Data")]
        Data
    }

    public class Json
    {
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Type { get; set; }
    }

    public class JsonData
    {
        public string Title { get; set; }
        public string Data { get; set; }
    }

    public class LogsAction
    {
        public class Auth
        {
            public class Action
            {
                public const string LOGIN = "LOGIN";
                public const string FORGOT_PASSWORD = "FORGOT PASSWORD";
                public const string CHANGE_PASSWORD = "CHANGE PASSWORD";
                public const string LOGOUT = "LOGOUT";
            }
            public class Status
            {
                public const string SUCCESS = "SUCCESS";
                public const string FAIL = "FAIL";
                public const string ERROR = "ERROR";
            }
        }

        public class Objects
        {
            public class Action
            {
                public const string CREATE = "CREATE";
                public const string UPDATE = "UPDATE";
                public const string DELETE = "DELETE";
                public const string LOCK = "LOCK";
                public const string UNLOCK = "UNLOCK";
                public const string CREATE_REVISION = "CREATE REVISION";
                public const string GET_COPY = "GET COPY";
            }
            public class Status
            {
                public const string SUCCESS = "SUCCESS";
                public const string FAIL = "FAIL";
                public const string ERROR = "ERROR";
            }
        }
    }

    public class AuthLogs
    {
        private string _action;
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
    }

    public class ObjLogs
    {
        private string _action;
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _typeObject;
        public string TypeObject
        {
            get { return _typeObject; }
            set { _typeObject = value; }
        }

        private string _jsonDatas;
        public string JsonDatas
        {
            get { return _jsonDatas; }
            set { _jsonDatas = value; }
        }
    }

    public class DataLogs
    {
        private string _action;
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        private int _userID;
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private AuthLogs _dataAuthLogs = null;
        public AuthLogs DataAuthLogs
        {
            get { return _dataAuthLogs; }
            set
            {
                if (value != null)
                {
                    _dataAuthLogs = new AuthLogs();
                    _dataAuthLogs.Action = value.Action;
                    _dataAuthLogs.Status = value.Status;
                    _dataAuthLogs.UserId = value.UserId;
                    _dataAuthLogs.Username = value.Username;
                }
                else
                {
                    _dataAuthLogs = null;
                }
            }
        }

        private ObjLogs _dataObjLogs = null;
        public ObjLogs DataObjLogs
        {
            get { return _dataObjLogs; }
            set
            {
                if (value != null)
                {
                    _dataObjLogs = new ObjLogs();
                    _dataObjLogs.Action = value.Action;
                    _dataObjLogs.Status = value.Status;
                    _dataObjLogs.TypeObject = value.TypeObject;
                    _dataObjLogs.JsonDatas = value.JsonDatas;
                }
                else
                {
                    _dataObjLogs = null;
                }
            }
        }

    }
}
