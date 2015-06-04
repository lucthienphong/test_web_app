using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;
using System.Data;
using System.Web.Security;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.Manager;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.UI;
using SweetSoft.APEM.Logs.DataAccess;


namespace SweetSoft.APEM.Core.Logs
{
    public class DataLogsViewObject
    {
        public int ID { get; set; }
        public int No { get; set; }
        public string DateAction { get; set; }
        public string UserName { get; set; }
        public string UserIP { get; set; }
        public string Action { get; set; }
        public string Object { get; set; }
        public string Contents { get; set; }
        public int TotalRow { get; set; }
    }
    public class DataLogsManager
    {
        public static TblAllDataLog UpdateLog(TblAllDataLog obj)
        {
            return new TblAllDataLogController().Update(obj);
        }

        public static TblAllDataLog InsertLog(TblAllDataLog obj)
        {
            return new TblAllDataLogController().Insert(obj);
        }

        public static void LogAction(TypeActionLogs type, string jsonString, string p_UserIP)
        {
            TblAllDataLog newLog = new TblAllDataLog();
            newLog.ActionDate = DateTime.Now;
            newLog.UserIP = p_UserIP;

            DataLogs newDataLogs = new DataLogs();
            newDataLogs.UserID = ApplicationContext.Current.User.UserID;
            newDataLogs.Action = Enum.GetName(typeof(TypeActionLogs), type);

            switch (type)
            {
                case TypeActionLogs.Data:
                    newDataLogs.DataObjLogs = JsonHelper.Deserialize<ObjLogs>(jsonString);
                    break;
                case TypeActionLogs.Authentication:
                    newDataLogs.DataAuthLogs = JsonHelper.Deserialize<AuthLogs>(jsonString);
                    break;
            }

            GetInforDataLogs(ref newLog, newDataLogs);

            newLog.ContentLogs = JsonHelper.Serialize<DataLogs>(newDataLogs);
            InsertLog(newLog);

        }

        public static List<DataLogsViewObject> SelectDataLogs(int PageIndex, int PageSize, 
                                                                DateTime? DateFrom, 
                                                                DateTime? DateTo, 
                                                                string UserName,
                                                                string UserIP,
                                                                string Action,
                                                                string ObjectName,
                                                                Sort sort)
        {
            List<DataLogsViewObject> view = new List<DataLogsViewObject>();
            var select = new Select().From(TblAllDataLog.Schema).Where(TblAllDataLog.IdColumn).IsNotNull();

            #region Add Conditions

            if(DateFrom != null)
            {
                select = select.And(TblAllDataLog.ActionDateColumn).IsGreaterThanOrEqualTo(DateTimeHelper.AddFirstTimeOfDay(DateFrom.Value));
            }
            if(DateTo != null)
            {
                select = select.And(TblAllDataLog.ActionDateColumn).IsLessThanOrEqualTo(DateTimeHelper.AddLastTimeOfDay(DateTo.Value));
            }

            if (!string.IsNullOrEmpty(UserName))
            {
                UserName = "%" + UserName + "%";
                select = select.And(TblAllDataLog.UserNameColumn).Like(UserName);
            }

            if (!string.IsNullOrEmpty(UserIP))
            {
                UserIP = "%" + UserIP + "%";
                select = select.And(TblAllDataLog.UserIPColumn).Like(UserIP);
            }

            if (!string.IsNullOrEmpty(Action))
            {
                Action = "%" + Action + "%";
                select = select.And(TblAllDataLog.ActionColumn).Like(Action);
            }

            if (!string.IsNullOrEmpty(ObjectName))
            {
                ObjectName = "%" + ObjectName + "%";
                select = select.And(TblAllDataLog.ObjectXColumn).Like(ObjectName);
            }

            #endregion

            #region Sort

            switch (sort.ColIndex)
            {
                case 1:
                    if (sort.Type == NumSortType.DESC)
                    {
                        select = select.OrderDesc(TblAllDataLog.Columns.ActionDate);
                    }
                    else if (sort.Type == NumSortType.ASC)
                    {
                        select = select.OrderAsc(TblAllDataLog.Columns.ActionDate);
                    }
                    break;
                case 2:
                    if (sort.Type == NumSortType.DESC)
                    {
                        select = select.OrderDesc(TblAllDataLog.Columns.UserName);
                    }
                    else if (sort.Type == NumSortType.ASC)
                    {
                        select = select.OrderAsc(TblAllDataLog.Columns.UserName);
                    }
                    break;
                case 3:
                    if (sort.Type == NumSortType.DESC)
                    {
                        select = select.OrderDesc(TblAllDataLog.Columns.UserIP);
                    }
                    else if (sort.Type == NumSortType.ASC)
                    {
                        select = select.OrderAsc(TblAllDataLog.Columns.UserIP);
                    }
                    break;
                case 4:
                    if (sort.Type == NumSortType.DESC)
                    {
                        select = select.OrderDesc(TblAllDataLog.Columns.Action);
                    }
                    else if (sort.Type == NumSortType.ASC)
                    {
                        select = select.OrderAsc(TblAllDataLog.Columns.Action);
                    }
                    break;
                case 5:
                    if (sort.Type == NumSortType.DESC)
                    {
                        select = select.OrderDesc(TblAllDataLog.Columns.ObjectX);
                    }
                    else if (sort.Type == NumSortType.ASC)
                    {
                        select = select.OrderAsc(TblAllDataLog.Columns.ObjectX);
                    }
                    break;
            }

            #endregion

            int TotalRow = select.GetRecordCount();
            select.Paged(PageIndex + 1, PageSize);
            List<TblAllDataLog> lstLogs = select.ExecuteTypedList<TblAllDataLog>();

            #region Create Data View

            int i_No = 1;
            foreach (TblAllDataLog log in lstLogs)
            {
                DataLogsViewObject item = new DataLogsViewObject();
                item.ID = log.Id;
                item.No = i_No;
                item.DateAction = log.ActionDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                item.UserIP = log.UserIP;
                item.TotalRow = TotalRow;

                DataLogs contentLogs = JsonHelper.Deserialize<DataLogs>(log.ContentLogs);
                item.UserName = log.UserName;

                if (contentLogs.Action == Enum.GetName(typeof(TypeActionLogs), TypeActionLogs.Data))
                {
                    item.Action = contentLogs.DataObjLogs.Action;
                    item.Object = contentLogs.DataObjLogs.TypeObject;
                    List<JsonData> lstJData = JsonConvert.DeserializeObject<List<JsonData>>(contentLogs.DataObjLogs.JsonDatas);

                    if (lstJData != null && lstJData.Count > 0)
                    {
                        string sContentLogs = string.Empty;
                        string sTemplateLog = "- Property: [<strong>{0}</strong>] has changed from: [old: <strong>{1}</strong>], [new: <strong>{2}</strong>]";
                        string sTemplateLogTable = "- Property: [<strong>{0}</strong>] has changed: <strong>{1}</strong>";
                        int jDataIndex = 0;
                        foreach (JsonData jData in lstJData)
                        {
                            try
                            {
                                Json jn = new Json();
                                jn = JsonConvert.DeserializeObject<Json>(jData.Data);

                                if (jn.Type == typeof(GridView).ToString() || jn.Type == typeof(GridviewExtension).ToString())
                                {
                                    sContentLogs += string.Format(sTemplateLogTable, jData.Title, "<a class='tblDetail' onclick='Detail(this)' href='javascript:' data-id=" + log.Id + " jdata-index=" + jDataIndex + ">Detail</a>") + "</br>";
                                }
                                else
                                {
                                    sContentLogs += string.Format(sTemplateLog, jData.Title, jn.OldValue, jn.NewValue) + "</br>";
                                }
                            }
                            catch (Exception e)
                            {
                                sContentLogs += string.Format("- {0}: <strong>{1}</strong>", jData.Title, jData.Data) + "</br>";
                            }
                            jDataIndex++;
                        }

                        item.Contents = sContentLogs;
                    }
                    else
                    {
                        item.Contents = "No Data changed";
                    }
                }
                else if (contentLogs.Action == Enum.GetName(typeof(TypeActionLogs), TypeActionLogs.Authentication))
                {
                    item.Action = contentLogs.DataAuthLogs.Action;
                    item.Object = "User";
                }

                view.Add(item);

                i_No++;
            }

            #endregion

            return view;
            //return GetRange(view, PageIndex, PageSize);
        }

        public static TblAllDataLog SeclectDataLogByID(int LogID)
        {
            return new Select().From(TblAllDataLog.Schema)
                               .Where(TblAllDataLog.IdColumn).IsEqualTo(LogID)
                               .ExecuteSingle<TblAllDataLog>();
        }

        private static List<DataLogsViewObject> GetRange(List<DataLogsViewObject> source, int Index, int Size)
        {
            List<DataLogsViewObject> retLST = new List<DataLogsViewObject>();

            if (Size >= source.Count)
            {
                return source;
            }

            if (Size > source.Count - (Index * Size))
            {
                return source.GetRange(Index * Size, source.Count - (Index * Size));
            }
            else 
            {
                retLST = source.GetRange(Index * Size, Size);
            }

            return retLST;
        }

        private static void GetInforDataLogs(ref TblAllDataLog log, DataLogs contentLogs)
        {
            if (contentLogs.Action == Enum.GetName(typeof(TypeActionLogs), TypeActionLogs.Data))
            {
                TblUser user = UserManager.SelectUserByID(contentLogs.UserID);
                log.UserName = user == null ? null : user.UserName;
                log.Action = contentLogs.DataObjLogs.Action;
                log.ObjectX = contentLogs.DataObjLogs.TypeObject;
            }
            else if (contentLogs.Action == Enum.GetName(typeof(TypeActionLogs), TypeActionLogs.Authentication))
            {
                log.Action = contentLogs.DataAuthLogs.Action;
                log.ObjectX = "User";
            }
        }

        public static List<TblAllDataLog> SelectUserNameByKeyWord(string KeyWord)
        {
            KeyWord = "%" + KeyWord + "%";

            TblAllDataLogCollection list = new Select(TblAllDataLog.UserNameColumn).From(TblAllDataLog.Schema).Distinct()
                    .Where(TblAllDataLog.Columns.UserName).Like(KeyWord)
                    .And(TblAllDataLog.Columns.UserName).IsNotNull()
                    .OrderAsc(TblAllDataLog.Columns.UserName)
                    .ExecuteAsCollection<TblAllDataLogCollection>();

            return list.ToList<TblAllDataLog>();
        }

        public static List<TblAllDataLog> SelectUserIPByKeyWord(string KeyWord)
        {
            KeyWord = "%" + KeyWord + "%";

            TblAllDataLogCollection list = new Select(TblAllDataLog.UserIPColumn).From(TblAllDataLog.Schema).Distinct()
                    .Where(TblAllDataLog.Columns.UserIP).Like(KeyWord)
                    .And(TblAllDataLog.Columns.UserIP).IsNotNull()
                    .OrderAsc(TblAllDataLog.Columns.UserIP)
                    .ExecuteAsCollection<TblAllDataLogCollection>();

            return list.ToList<TblAllDataLog>();
        }

        public static List<TblAllDataLog> SelectActionByKeyWord(string KeyWord)
        {
            KeyWord = "%" + KeyWord + "%";

            TblAllDataLogCollection list = new Select(TblAllDataLog.ActionColumn).From(TblAllDataLog.Schema).Distinct()
                    .Where(TblAllDataLog.Columns.Action).Like(KeyWord)
                    .And(TblAllDataLog.Columns.Action).IsNotNull()
                    .OrderAsc(TblAllDataLog.Columns.Action)
                    .ExecuteAsCollection<TblAllDataLogCollection>();

            return list.ToList<TblAllDataLog>();
        }

        public static List<TblAllDataLog> SelectObjectByKeyWord(string KeyWord)
        {
            KeyWord = "%" + KeyWord + "%";

            TblAllDataLogCollection list = new Select(TblAllDataLog.ObjectXColumn).From(TblAllDataLog.Schema).Distinct()
                    .Where(TblAllDataLog.Columns.ObjectX).Like(KeyWord)
                    .And(TblAllDataLog.Columns.ObjectX).IsNotNull()
                    .OrderAsc(TblAllDataLog.Columns.ObjectX)
                    .ExecuteAsCollection<TblAllDataLogCollection>();

            return list.ToList<TblAllDataLog>();
        }
    }
}
