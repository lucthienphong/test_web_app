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
        public static TblDataLog UpdateLog(TblDataLog obj)
        {
            return new TblDataLogController().Update(obj);
        }

        public static TblDataLog InsertLog(TblDataLog obj)
        {
            return new TblDataLogController().Insert(obj);
        }

        public static void LogAction(TypeActionLogs type, string jsonString, string p_UserIP)
        {
            TblDataLog newLog = new TblDataLog();
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

            newLog.ContentLogs = JsonHelper.Serialize<DataLogs>(newDataLogs);
            InsertLog(newLog);

        }

        public static List<DataLogsViewObject> SelectDataLogs(int PageIndex, int PageSize, DateTime? DateFrom, DateTime? DateTo)
        {
            List<DataLogsViewObject> view = new List<DataLogsViewObject>();
            var select = new Select().From(TblDataLog.Schema).Where(TblDataLog.IdColumn).IsNotNull();
            if(DateFrom != null)
            {
                select = select.And(TblDataLog.ActionDateColumn).IsGreaterThanOrEqualTo(DateTimeHelper.AddFirstTimeOfDay(DateFrom.Value));
            }
            if(DateTo != null)
            {
                select = select.And(TblDataLog.ActionDateColumn).IsLessThanOrEqualTo(DateTimeHelper.AddLastTimeOfDay(DateTo.Value));
            }
            select = select.OrderDesc(TblDataLog.Columns.ActionDate);
            List<TblDataLog> lstLogs = select.ExecuteTypedList<TblDataLog>();

            int i_No = 1;
            foreach (TblDataLog log in lstLogs)
            {
                DataLogsViewObject item = new DataLogsViewObject();
                item.ID = log.Id;
                item.No = i_No;
                item.DateAction = log.ActionDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                item.UserIP = log.UserIP;
                item.TotalRow = lstLogs.Count;

                DataLogs contentLogs = JsonHelper.Deserialize<DataLogs>(log.ContentLogs);
                TblUser user = UserManager.SelectUserByID(contentLogs.UserID);
                item.UserName = user == null ? null : user.UserName;

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

            return GetRange(view, PageIndex, PageSize);
        }

        public static TblDataLog SeclectDataLogByID(int LogID)
        {
            return new Select().From(TblDataLog.Schema)
                               .Where(TblDataLog.IdColumn).IsEqualTo(LogID)
                               .ExecuteSingle<TblDataLog>();
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
    }
}
