using Newtonsoft.Json;
using SubSonic;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace SweetSoft.APEM.Core.Manager
{
    /// <summary>
    /// Summary description for RealtimeNotificationManager
    /// </summary>
    public class RealtimeNotificationManager
    {
        public static string keyName = "APE-No";

        #region TblNotification

        public static TblStaffCollection GetStaffInDepartment(List<int> departmentIds)
        {
            if (departmentIds != null && departmentIds.Count > 0)
            {
                return new Select().From(TblStaff.Schema).Where(TblStaff.DepartmentIDColumn)
                    .In(departmentIds).ExecuteAsCollection<TblStaffCollection>();
            }
            return null;
        }

        public static TblStaffCollection GetStaffByIds(List<int> staffIds)
        {
            if (staffIds != null && staffIds.Count > 0)
            {
                return new Select().From(TblStaff.Schema).Where(TblStaff.StaffIDColumn)
                    .In(staffIds).ExecuteAsCollection<TblStaffCollection>();
            }
            else
                return null;
        }

        public static TblStaffCollection GetStaffByDepartmentIDs(List<int> lstDepartmentIds)
        {
            if (lstDepartmentIds != null && lstDepartmentIds.Count > 0)
                return new Select().From(TblStaff.Schema).Where(TblStaff.DepartmentIDColumn)
                          .In(lstDepartmentIds).ExecuteAsCollection<TblStaffCollection>();
            else
                return null;
        }

        public static System.Data.DataTable GetUsersByDepartmentIDs(List<int> lstDepartmentIds)
        {
            if (lstDepartmentIds != null && lstDepartmentIds.Count > 0)
            {
                SqlQuery q = new Select().From(TblStaff.Schema)
                          .Where(TblStaff.DepartmentIDColumn).In(lstDepartmentIds)
                          .And(TblStaff.HasAccountColumn).IsEqualTo(true)
                          .And(TblUser.IsObsoleteColumn.ColumnName).IsEqualTo(false);
                System.Data.DataSet ds = q.ExecuteDataSet();
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
            }
            return null;
        }

        public static TblNotificationCollection GetNotificationByPageIdAndCommandType(string pageId, string commandType)
        {
            return GetNotificationByPageIdAndCommandType(pageId, commandType, false);
        }

        public static TblNotificationCollection GetNotificationByPageIdAndCommandType(string pageId,
            string commandType, bool? isObsolete)
        {
            SqlQuery q = new Select().From(TblNotification.Schema)
                 .Where(TblNotification.PageIdColumn).IsEqualTo(pageId)
                 .And(TblNotification.CommandTypeColumn).IsEqualTo(commandType);

            if (isObsolete.HasValue)
                q.And(TblNotification.IsObsoleteColumn).IsEqualTo(isObsolete);

            return q.ExecuteAsCollection<TblNotificationCollection>();
        }

        /*
        public static TblNotification InsertNotification(string Title, NotificationType notificationType,
            List<int> lstId, string message, string action, string pageId)
        {
            return new TblNotificationController().Insert(Title, string.Empty, message, false, 
                string.Join(",", lstId.Select(x => x.ToString()).ToArray()),
                action,
                 ApplicationContext.Current.UserName, DateTime.Now, string.Empty, string.Empty, pageId,notificationType.ToString());
        }
        */

        /*
        public static TblNotification Update(TblNotification notification)
        {
            return new TblNotificationController().Update(notification.NotificationID,
                notification.Title, notification.Summary, notification.Contents, notification.IsObsolete,
           notification.ReceiveIds, notification.Actions, notification.CreatedBy, notification.CreatedOn
                , notification.DateDismiss, notification.DismissBy, notification.PageId);
        }
        */
        public static TblNotification Update(TblNotification notification)
        {
            return new TblNotificationController().Update(notification.NotificationID,
                notification.Title, notification.DismissEvent, notification.Contents, notification.IsObsolete,
             notification.ReceiveIds, notification.Actions, notification.CreatedBy, notification.CreatedOn
                , notification.DateDismiss, notification.DismissBy, notification.PageId,
                notification.ReceiveType, notification.CommandType);
        }

        public static TblNotification Insert(TblNotification notification)
        {
            return new TblNotificationController().Insert(
                notification.Title, notification.DismissEvent, notification.Contents, notification.IsObsolete,
             notification.ReceiveIds, notification.Actions, notification.CreatedBy, notification.CreatedOn
                , notification.DateDismiss, notification.DismissBy, notification.PageId,
                notification.ReceiveType, notification.CommandType);
        }

        public static bool Delete(object id)
        {
            try
            {
                return new TblNotificationController().Delete(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool DeleteNotificationByPageId(string pageId)
        {
            try
            {
                int isSucess = new Delete().From(TblNotification.Schema)
                       .Where(TblNotification.PageIdColumn).IsEqualTo(pageId)
                       .Execute();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static TblNotification GetNotificationById(object Id)
        {
            TblNotificationCollection all = new TblNotificationController().FetchByID(Id);
            if (all != null && all.Count > 0)
                return all[0];
            else
                return null;
        }

        public static TblNotificationCollection GetNotificationByPageId(string pageId, bool? IsObsolete)
        {
            SqlQuery q = new Select().From(TblNotification.Schema)
                .Where(TblNotification.PageIdColumn).IsEqualTo(pageId);
            if (IsObsolete.HasValue)
                q.And(TblNotification.IsObsoleteColumn).IsEqualTo(IsObsolete);
            return q.ExecuteAsCollection<TblNotificationCollection>();
        }

        public static TblNotificationCollection GetNotification()
        {
            return new Select().From(TblNotification.Schema)
                    .Where(TblNotification.IsObsoleteColumn).IsEqualTo(false)
                    .OrderDesc(TblNotification.CreatedOnColumn.ColumnName)
                    .ExecuteAsCollection<TblNotificationCollection>();
        }

        public static bool UpdateNotification()
        {
            int id = CommonHelper.QueryStringInt("NotificationId");
            if (id > 0)
                return UpdateNotification(id);
            else
                return false;
        }

        public static bool UpdateNotification(int id, TblStaff staff, string userName)
        {
            return UpdateNotification(id, (int)staff.DepartmentID, staff.StaffID, userName, false);
        }

        public static bool UpdateNotification(int id, int staffId)
        {
            TblStaff staff = StaffManager.SelectByID(staffId);
            if (staff != null && staff.HasAccount == true)
                return UpdateNotification(id, staff, ApplicationContext.Current.UserName);
            return false;
        }

        public static bool UpdateNotification(int id)
        {
            TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
            if (curentUser != null)
                return UpdateNotification(id, curentUser.TblStaff, curentUser.UserName);
            return false;
        }

        public static bool UpdateNotification(TblNotification noti, int deparmentId, int staffId)
        {
            return UpdateNotification(noti, deparmentId, staffId,
                ApplicationContext.Current.UserName, false);
        }

        public static bool UpdateNotification(TblNotification noti, int deparmentId, int staffId, bool isMarkAsUnread)
        {
            return UpdateNotification(noti, deparmentId, staffId,
                ApplicationContext.Current.UserName, isMarkAsUnread);
        }

        public static bool UpdateNotification(TblNotification noti, int deparmentId,
            int staffId, string userName, bool isMarkAsUnread)
        {
            if (noti != null)
            {
                string data = noti.ReceiveIds;
                if (data != null && data.Length > 0)
                {
                    string[] typeandid = data.Split('|');
                    NotificationType notificationType = (NotificationType)Enum.Parse(typeof(NotificationType), noti.ReceiveType);
                    string[] arrId;

                    if (typeandid.Length == 1)
                        arrId = typeandid[0].Split(',');
                    else
                        arrId = typeandid[1].Split(',');

                    if (arrId != null && arrId.Length > 0)
                    {
                        #region add or update

                        List<string> dataDateDismiss = null;
                        if (noti.DateDismiss != null && noti.DateDismiss.Length > 0)
                        {
                            dataDateDismiss = noti.DateDismiss.Split(',').ToList();
                            if (dataDateDismiss.Count < arrId.Length)
                            {
                                for (int i = 0, j = arrId.Length - dataDateDismiss.Count; i < j; i++)
                                    dataDateDismiss.Add(string.Empty);
                            }
                        }
                        else
                            dataDateDismiss = new List<string>();

                        List<string> dataDismissBy = null;
                        if (noti.DismissBy != null && noti.DismissBy.Length > 0)
                        {
                            dataDismissBy = noti.DismissBy.Split(',').ToList();
                            if (dataDismissBy.Count < arrId.Length)
                            {
                                for (int i = 0, j = arrId.Length - dataDismissBy.Count; i < j; i++)
                                    dataDismissBy.Add(string.Empty);
                            }
                        }
                        else
                            dataDismissBy = new List<string>();

                        if (dataDismissBy.Count == 0)
                        {
                            #region add

                            for (int i = 0; i < arrId.Length; i++)
                            {
                                if (isMarkAsUnread)
                                {
                                    dataDateDismiss.Add(string.Empty);
                                    dataDismissBy.Add(string.Empty);
                                }
                                else
                                {
                                    switch (notificationType)
                                    {
                                        case NotificationType.Deparment:
                                            if (deparmentId.ToString() == arrId[i])
                                            {
                                                dataDateDismiss.Add(DateTime.Now.ToString());
                                                dataDismissBy.Add(userName);
                                            }
                                            else
                                            {
                                                dataDateDismiss.Add(string.Empty);
                                                dataDismissBy.Add(string.Empty);
                                            }
                                            break;
                                        case NotificationType.Staff:
                                            if (staffId.ToString() == arrId[i])
                                            {
                                                dataDateDismiss.Add(DateTime.Now.ToString());
                                                dataDismissBy.Add(userName);
                                            }
                                            else
                                            {
                                                dataDateDismiss.Add(string.Empty);
                                                dataDismissBy.Add(string.Empty);
                                            }
                                            break;
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region update

                            bool isFound = false;
                            for (int i = 0; i < arrId.Length; i++)
                            {
                                if (isFound)
                                    break;
                                switch (notificationType)
                                {
                                    case NotificationType.Deparment:
                                        if (deparmentId.ToString() == arrId[i])
                                        {
                                            if (isMarkAsUnread)
                                            {
                                                dataDateDismiss[i] = string.Empty;
                                                dataDismissBy[i] = string.Empty;
                                            }
                                            else
                                            {
                                                dataDateDismiss[i] = DateTime.Now.ToString();
                                                dataDismissBy[i] = userName;
                                            }
                                            isFound = true;
                                        }
                                        break;
                                    case NotificationType.Staff:
                                        if (staffId.ToString() == arrId[i])
                                        {
                                            if (isMarkAsUnread)
                                            {
                                                dataDateDismiss[i] = string.Empty;
                                                dataDismissBy[i] = string.Empty;
                                            }
                                            else
                                            {
                                                dataDateDismiss[i] = DateTime.Now.ToString();
                                                dataDismissBy[i] = userName;
                                            }
                                            isFound = true;
                                        }
                                        break;
                                }
                            }

                            #endregion
                        }

                        noti.DateDismiss = string.Join(",", dataDateDismiss.Select(s => "" + s).ToArray());
                        noti.DismissBy = string.Join(",", dataDismissBy.Select(s => "" + s).ToArray());

                        #endregion

                        //check if all is dismiss
                        bool isDismissAll = true;
                        for (int i = 0; i < dataDismissBy.Count; i++)
                        {
                            if (string.IsNullOrEmpty(dataDismissBy[i]))
                            {
                                isDismissAll = false;
                                break;
                            }
                        }
                        noti.IsObsolete = isDismissAll;
                    }

                    RealtimeNotificationManager.Update(noti);
                }
                return true;
            }
            return false;
        }

        public static bool UpdateNotification(int id, int deparmentId,
            int staffId, string userName, bool isMarkAsUnread)
        {
            //int id = CommonHelper.QueryStringInt("NotificationId");
            if (id < 1)
                return false;

            return UpdateNotification(RealtimeNotificationManager.GetNotificationById(id),
                deparmentId, staffId, userName, isMarkAsUnread);
        }

        public static void MarkAsRead(List<int> lstNotificationId, int departmentId, int staffId)
        {
            if (lstNotificationId == null || lstNotificationId.Count == 0)
                return;

            TblNotificationCollection allNoti = new Select().From(TblNotification.Schema)
            .Where(TblNotification.NotificationIDColumn).In(lstNotificationId)
            .ExecuteAsCollection<TblNotificationCollection>();
            if (allNoti != null && allNoti.Count > 0)
            {
                foreach (TblNotification item in allNoti)
                {
                    if (string.IsNullOrEmpty(item.ReceiveIds) == false)
                    {
                        UpdateNotification(item, departmentId, staffId);
                    }
                }
            }
        }

        public static void MarkAsRead(List<int> lstNotificationId)
        {
            TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
            if (curentUser != null)
            {
                MarkAsRead(lstNotificationId, (int)curentUser.TblStaff.DepartmentID,
                   curentUser.TblStaff.StaffID);
            }
        }

        public static void MarkAsUnRead(List<int> lstNotificationId)
        {
            TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
            if (curentUser != null)
            {
                MarkAsUnRead(lstNotificationId, (int)curentUser.TblStaff.DepartmentID,
                   curentUser.TblStaff.StaffID);
            }
        }

        public static void MarkAsUnRead(List<int> lstNotificationId, int departmentId, int staffId)
        {
            if (lstNotificationId == null || lstNotificationId.Count == 0)
                return;

            TblNotificationCollection allNoti = new Select().From(TblNotification.Schema)
            .Where(TblNotification.NotificationIDColumn).In(lstNotificationId)
            .ExecuteAsCollection<TblNotificationCollection>();
            if (allNoti != null && allNoti.Count > 0)
            {
                foreach (TblNotification item in allNoti)
                {
                    if (string.IsNullOrEmpty(item.ReceiveIds) == false)
                    {
                        UpdateNotification(item, departmentId, staffId, true);
                    }
                }
            }
        }

        public static List<int> GetAllDepartmentId()
        {
            return new Select(TblDepartment.DepartmentIDColumn).From(TblDepartment.Schema)
                    .ExecuteTypedList<int>();
        }

        public static List<int> GetAllStaffIdsByDepartmentIds(List<int> lstDepartmentIds)
        {
            SqlQuery q = new Select(TblUser.UserIDColumn).From(TblUser.Schema)
               .LeftOuterJoin(TblStaff.Schema).Where(TblUser.UserIDColumn)
                     .In(new Select(TblStaff.StaffIDColumn).From(TblStaff.Schema)
                     .Where(TblStaff.DepartmentIDColumn).In(lstDepartmentIds))
                     .And(TblUser.IsObsoleteColumn).IsEqualTo(false);

            return q.ExecuteTypedList<int>();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static string CreateNotificationNumber()
        {
            string _No = keyName + DateTime.Today.ToString("yy");

            string _MaxNumber = new Select(Aggregate.Max(TblNotificationSetting.PageIdColumn))
               .From(TblNotificationSetting.Schema)
               .Where(TblNotificationSetting.PageIdColumn).Like(_No + "%")
               .ExecuteScalar<string>();

            _MaxNumber = _MaxNumber ?? "";
            if (_MaxNumber.Length > 0)
                _No += (int.Parse(_MaxNumber.Substring(_No.Length)) + 1).ToString("d5");
            else
                _No += "00001";
            return _No;
        }

        static Dictionary<string, Type> AllTypeInWebNamespace
        {
            get
            {
                object val = HttpContext.Current.Session["TestSerialize-AllTypeInWebNamespace"];
                if (val == null)
                {
                    Assembly dataAccess = Assembly.Load("SweetSoft.APEM.WebApp");
                    if (dataAccess != null)
                    {
                        Type[] allType = dataAccess.GetTypes();
                        if (allType != null && allType.Length > 0)
                        {
                            Dictionary<string, Type> dic = new Dictionary<string, Type>();
                            List<string> lstIgnore = new List<string>() { "WorkFlow", "Login", "Notification", "NotificationSetting" };
                            foreach (Type item in allType)
                            {
                                if (item.Namespace == "SweetSoft.APEM.WebApp.Pages"
                                    && item.FullName.Contains("<") == false
                                    && item.FullName.Contains("`") == false
                                    && lstIgnore.Contains(item.Name) == false)
                                    dic.Add(item.FullName, item);
                            }
                            val = dic;
                            HttpContext.Current.Session["TestSerialize-AllTypeInWebNamespace"] = dic;
                        }
                    }
                }
                return val as Dictionary<string, Type>;
            }
        }

        readonly static List<string> lstIgnore = new List<string>() { 
            "WorkFlow", "Login", "Notification", "NotificationDetailAll",
            "CurrencyChangedLog", "SystemCofiguration", "RolePermission", "Role",
            "NotificationDetail"
        };

        public static List<NotificationPage> GetAllSupportNotificationPage(Dictionary<string, Type> allTypeInWebNamespace)
        {
            List<NotificationPage> lst = new List<NotificationPage>();

            if (allTypeInWebNamespace != null && allTypeInWebNamespace.Count > 0)
            {
                PropertyInfo pi = null;
                string title = string.Empty;

                TblFunctionCollection allFucntion = new Select()
                    .From(TblFunction.Schema).ExecuteAsCollection<TblFunctionCollection>();

                string id = string.Empty;
                foreach (KeyValuePair<string, Type> keyValuePairString in allTypeInWebNamespace)
                {
                    try
                    {
                        id = keyValuePairString.Key.Substring(keyValuePairString.Key.LastIndexOf(".") + 1);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    title = string.Empty;
                    pi = keyValuePairString.Value.GetProperty("FUNCTION_PAGE_ID");
                    if (pi != null)
                    {
                        object obj = Activator.CreateInstance(keyValuePairString.Value);
                        var val = pi.GetValue(obj, null);
                        if (val != null)
                        {
                            if (allFucntion != null && allFucntion.Count > 0)
                            {
                                var found = allFucntion.Where(x => x.FunctionID == val.ToString());
                                if (found != null && found.Count() > 0)
                                    title = found.First().Title;
                            }
                        }
                        if (string.IsNullOrEmpty(title))
                            title = id;

                        lst.Add(new NotificationPage()
                        {
                            functionid = val != null ? val.ToString() : string.Empty,
                            id = id,
                            title = title,
                            url = id + ".aspx"
                        });
                    }
                }
            }
            return lst;
        }

        /*old
        public static List<NotificationPage> GetAllSupportNotificationPage()
        {
            Dictionary<string, Type> allTypeInWebNamespace = AllTypeInWebNamespace;
            List<NotificationPage> lst = GetAllSupportNotificationPage(allTypeInWebNamespace);
            TblNotificationSettingCollection allGlobal = GetGlobalNotificationSetting();
            if (allGlobal != null && allGlobal.Count > 0)
            {
                foreach (TblNotificationSetting tblNotificationSetting in allGlobal)
                {
                    lst.Add(new NotificationPage()
                    {
                        functionid = "",
                        id = tblNotificationSetting.PageId,
                        title = tblNotificationSetting.Title,
                        url = ""
                    });
                }
            }
            return lst;
        }
        */

        //new
        public static List<NotificationPage> GetAllSupportNotificationPage()
        {
            List<NotificationPage> lst = new List<NotificationPage>();
            TblNotificationSettingCollection allGlobal = GetGlobalNotificationSetting();
            if (allGlobal != null && allGlobal.Count > 0)
            {
                foreach (TblNotificationSetting tblNotificationSetting in allGlobal)
                {
                    lst.Add(new NotificationPage()
                    {
                        functionid = "",
                        id = tblNotificationSetting.PageId,
                        title = tblNotificationSetting.Title,
                        url = "",
                        createdby = tblNotificationSetting.CreatedBy,
                        createdon = tblNotificationSetting.CreatedOn ?? DateTime.Now
                    });
                }
            }
            return lst;
        }

        public static NotificationPage GetNotificationPageById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            if (id.StartsWith(keyName))
            {
                TblNotificationSetting noti = new Select().From(TblNotificationSetting.Schema)
                    .Where(TblNotificationSetting.PageIdColumn).IsEqualTo(id)
                    .ExecuteSingle<TblNotificationSetting>();
                if (noti != null)
                    return new NotificationPage() { functionid = "", id = id, title = noti.Title, url = "" };
            }
            else
            {
                Dictionary<string, Type> allTypeInWebNamespace = AllTypeInWebNamespace;
                List<NotificationPage> lst = GetAllSupportNotificationPage(allTypeInWebNamespace);

                if (lst != null && lst.Count > 0)
                {
                    var found = lst.Where(x => x.id.ToLower() == id.ToLower());
                    if (found != null && found.Count() > 0)
                        return found.First();
                }
            }
            return null;
        }

        public static List<NotificationPage> GetAllAvaliblePage()
        {
            string[] fileColl = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Pages"), "*.aspx");
            if (fileColl != null && fileColl.Length > 0)
            {
                Assembly webappPage = Assembly.Load("SweetSoft.APEM.WebApp");
                if (webappPage != null)
                {
                    Type[] allType = webappPage.GetTypes();
                    if (allType != null && allType.Length > 0)
                    {
                        Dictionary<string, Type> dic = new Dictionary<string, Type>();
                        foreach (Type item in allType)
                        {
                            if (item.Namespace == "SweetSoft.APEM.WebApp.Pages"
                                && item.FullName.Contains("<") == false
                                && item.FullName.Contains("`") == false)
                                dic.Add(item.FullName, item);
                        }

                        #region MyRegion

                        if (dic != null && dic.Count > 0)
                        {
                            Type ModalBasePage = Type.GetType("SweetSoft.APEM.WebApp.Common.ModalBasePage,SweetSoft.APEM.WebApp");

                            var found = dic.Where(x => x.Value.IsSubclassOf(ModalBasePage));

                            if (found != null && found.Count() > 0)
                            {
                                Dictionary<string, Type> lst = new Dictionary<string, Type>();
                                List<string> lstIgnore = new List<string>() { "WorkFlow", "Login", 
                                    "Notification", "JobNotification", 
                                    "JobTimeline", "CylinderTimeline",
                                    "NotificationSetting" };
                                foreach (string item in fileColl)
                                {
                                    string className = Path.GetFileNameWithoutExtension(item);
                                    if (lstIgnore.Contains(className))
                                        continue;

                                    var hasInherit = found.Where(x => x.Key.EndsWith(className));
                                    if (hasInherit != null && hasInherit.Count() > 0)
                                    {
                                        if (lst.ContainsKey(hasInherit.First().Key) == false)
                                            lst.Add(hasInherit.First().Key, hasInherit.First().Value);
                                    }
                                }

                                if (lst != null && lst.Count > 0)
                                {
                                    return RealtimeNotificationManager.GetAllSupportNotificationPage(lst);
                                }
                            }
                        }

                        #endregion
                    }
                }
            }
            return null;
        }

        #endregion

        #region TblNotificationSetting

        public static TblNotificationSetting GetNotificationSettingByPageIdAndCommandType(string pageId, CommandType commandType)
        {
            TblNotificationSettingCollection all = new Select().From(TblNotificationSetting.Schema)
                .Where(TblNotificationSetting.PageIdColumn).IsEqualTo(pageId)
                .And(TblNotificationSetting.CommandTypeColumn).IsEqualTo(commandType.ToString())
                .ExecuteAsCollection<TblNotificationSettingCollection>();
            if (all != null && all.Count > 0)
                return all[0];
            else
                return null;
        }

        public static TblNotificationSettingCollection GetNotificationSettingByPageId(string pageId)
        {
            if (string.IsNullOrEmpty(pageId))
                return null;

            return new Select().From(TblNotificationSetting.Schema)
                .Where(TblNotificationSetting.PageIdColumn).IsEqualTo(pageId)
                .ExecuteAsCollection<TblNotificationSettingCollection>();
        }

        public static TblNotificationSettingCollection GetGlobalNotificationSetting()
        {
            return new Select().From(TblNotificationSetting.Schema)
                .Where(TblNotificationSetting.PageIdColumn).Like(keyName + "%")
                .ExecuteAsCollection<TblNotificationSettingCollection>();
        }

        public static TblNotificationSetting InsertNotificationSetting(TblNotificationSetting notiSetting)
        {
            return new TblNotificationSettingController().Insert(notiSetting.Title, notiSetting.Description,
                notiSetting.IsObsolete, notiSetting.TriggerButton, notiSetting.Actions, notiSetting.PageId,
                notiSetting.CommandType, notiSetting.DismissEvent, notiSetting.ReceiveIds,
                notiSetting.ReceiveType, notiSetting.CreatedBy, notiSetting.CreatedOn,
                notiSetting.ModifiedBy, notiSetting.ModifiedOn);
        }

        public static TblNotificationSetting UpdateNotificationSetting(TblNotificationSetting notiSetting)
        {
            return new TblNotificationSettingController().Update(notiSetting.SettingId, notiSetting.Title, notiSetting.Description,
                notiSetting.IsObsolete, notiSetting.TriggerButton, notiSetting.Actions,
                notiSetting.PageId, notiSetting.CommandType, notiSetting.DismissEvent,
                notiSetting.ReceiveIds, notiSetting.ReceiveType, notiSetting.CreatedBy, notiSetting.CreatedOn,
                notiSetting.ModifiedBy, notiSetting.ModifiedOn);
        }

        public static bool DeleteNotificationSetting(object id)
        {
            try
            {
                return new TblNotificationSettingController().Delete(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteNotificationSettingByPageId(string pageId)
        {
            try
            {
                if (DeleteNotificationByPageId(pageId))
                {
                    int isSucess = new Delete().From(TblNotificationSetting.Schema)
                          .Where(TblNotificationSetting.PageIdColumn).IsEqualTo(pageId)
                          .Execute();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteNotificationSettingByPageIdAndCommandType(string pageId, CommandType commandType)
        {
            try
            {
                int isSucess = new Delete().From(TblNotificationSetting.Schema)
                       .Where(TblNotificationSetting.PageIdColumn).IsEqualTo(pageId)
                       .And(TblNotificationSetting.CommandTypeColumn).IsEqualTo(commandType.ToString())
                       .Execute();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion



        static IEnumerable<T> EnumToList<T>() where T : struct
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>();

            foreach (T val in enumValArray)
                enumValList.Add(val);

            return enumValList;
        }

        public static TblNotification CreateOrDismissNotification(string jsonDictionary, string commandType,
            string currentUserName, string functionPageId, TblNotificationSetting currentSetting)
        {
            return CreateOrDismissNotification(jsonDictionary, commandType, currentUserName,
                functionPageId, currentSetting, false, string.Empty, string.Empty);
        }

        public static TblNotification CreateOrDismissNotification(string jsonDictionary, string commandType,
            string currentUserName, string functionPageId, TblNotificationSetting currentSetting,
            bool forceCreate, string contentMessageFormat, string pageId)
        {
            System.Web.UI.Page page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;
            if (page != null)
            {
                if (string.IsNullOrEmpty(pageId))
                    pageId = page.GetType().BaseType.Name;

                #region save notification

                try
                {
                    string id = string.Empty;
                    string name = string.Empty;

                    string pageSettingId = pageId;

                    //fix some page
                    if (pageId == "JobList")
                        pageSettingId = "Job";

                    #region get data

                    if (string.IsNullOrEmpty(jsonDictionary) == false)
                    {
                        List<KeyValuePair<string, object>> dic = JsonConvert.DeserializeObject<List<KeyValuePair<string, object>>>(jsonDictionary);
                        if (dic != null && dic.Count > 0)
                        {
                            foreach (KeyValuePair<string, object> keyValuePairString in dic)
                            {
                                //fix some page
                                if (pageId == "Job" || pageId == "JobList")
                                {
                                    name = dic.Where(x => x.Key == TblJob.JobNumberColumn.ColumnName).First().Value.ToString();
                                }

                                #region RegionName

                                if (string.IsNullOrEmpty(id))
                                {
                                    if (keyValuePairString.Key.ToLower().Contains("id"))
                                    {
                                        id = keyValuePairString.Value.ToString();
                                    }
                                }

                                if (string.IsNullOrEmpty(name))
                                {
                                    if (keyValuePairString.Key.ToLower().Contains("firstname") &&
                                        keyValuePairString.Key.ToLower().Contains("lastname"))
                                    {
                                        Dictionary<string, object> temp = dic.ToDictionary(x => x.Key, x => x.Value);
                                        name = temp["FirstName"] + " " + temp["LastName"];
                                    }
                                }

                                if (string.IsNullOrEmpty(name))
                                {
                                    if (keyValuePairString.Key.ToLower().Contains("fullname"))
                                    {
                                        name = keyValuePairString.Value.ToString();
                                    }
                                }

                                if (string.IsNullOrEmpty(name))
                                {
                                    if (keyValuePairString.Key.ToLower().Contains("name"))
                                    {
                                        name = keyValuePairString.Value.ToString();
                                    }
                                }

                                if (string.IsNullOrEmpty(name))
                                {
                                    if (keyValuePairString.Key.ToLower().Contains("description"))
                                    {
                                        name = keyValuePairString.Value.ToString();
                                    }
                                }

                                #endregion

                                if (string.IsNullOrEmpty(id) == false && string.IsNullOrEmpty(name) == false)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    #endregion

                    IEnumerable<CommandType> lst = EnumToList<CommandType>();
                    if (lst != null && lst.Count() > 0)
                    {
                        TblNotificationSetting notiSetting = null;
                        TblNotification noti = null;
                        string laterCheck = "-1";
                        bool isExist = false;

                        if (currentSetting == null)
                        {
                            foreach (CommandType item in lst)
                            {
                                if (item.ToString().ToLower() == commandType.ToLower())
                                {
                                    notiSetting = RealtimeNotificationManager.GetNotificationSettingByPageIdAndCommandType(pageSettingId, item);
                                    if (notiSetting != null)
                                    {
                                        if (forceCreate) { }
                                        else
                                        {
                                            TblNotificationCollection allNoti = RealtimeNotificationManager.GetNotificationByPageIdAndCommandType(pageSettingId, item.ToString());

                                            //validate for duplicate
                                            if (allNoti != null && allNoti.Count > 0)
                                            {
                                                string content = string.Format(notiSetting.Description, id, name, currentUserName);
                                                foreach (TblNotification itemNoti in allNoti)
                                                {
                                                    if (itemNoti.Contents == content)
                                                    {
                                                        isExist = true;
                                                        noti = itemNoti;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    laterCheck = item.ToString();
                                    break;
                                }
                            }
                        }

                        else
                        {
                            notiSetting = currentSetting;
                            TblNotificationCollection all = RealtimeNotificationManager.GetNotificationByPageId(currentSetting.PageId, false);
                            if (all != null && all.Count > 0)
                                noti = all[0];
                        }

                        if (notiSetting == null)
                            return null;

                        if (noti == null)
                        {
                            noti = new TblNotification();
                            noti.CreatedBy = ApplicationContext.Current.UserName;
                            noti.CreatedOn = DateTime.Now;
                        }
                        else
                            isExist = true;

                        #region RegionName

                        if (notiSetting.Actions.Length > 0)
                        {
                            StringBuilder sbAction = new StringBuilder();
                            string encrypt = SecurityHelper.Encrypt("|");
                            string join = SecurityHelper.Encrypt(",");
                            string[] arr = notiSetting.Actions.Split(new string[] { join }, StringSplitOptions.None);
                            if (arr != null && arr.Length > 0)
                            {
                                string[] openPage = arr[0].Split(new string[] { encrypt }, StringSplitOptions.None);
                                if (openPage != null && openPage.Length > 0)
                                {
                                    string pathFile = openPage[1];
                                    if (pathFile == "-2")
                                        pathFile = HttpContext.Current.Request.Url.PathAndQuery;
                                    else
                                        pathFile += ".aspx";
                                    //string uas = CommonHelper.QueryString("uas");
                                    if (notiSetting.DismissEvent != null && notiSetting.DismissEvent.Length > 0)
                                    {
                                        if (pathFile.Contains("uas=") == false)
                                        {
                                            if (pathFile.Contains("?"))
                                                pathFile += "&uas=" + notiSetting.DismissEvent;
                                            else
                                                pathFile += "?uas=" + notiSetting.DismissEvent;
                                        }
                                    }

                                    if (commandType.ToLower() == "insert" && pathFile.ToLower().Contains("id=") == false)
                                    {
                                        if (pathFile.Contains("?"))
                                            pathFile += "&ID=" + id;
                                        else
                                            pathFile += "?ID=" + id;
                                    }

                                    sbAction.Append(String.Format("{0}('{1}','{2}');", openPage[0], openPage[2].Replace("'", "\""), pathFile));
                                }

                                if (arr.Length > 1)
                                {
                                    string[] messageData = arr[1].Split(new string[] { encrypt }, StringSplitOptions.None);
                                    if (messageData != null && messageData.Length > 0)
                                    {
                                        sbAction.Append("setTimeout(function(){" + messageData[0] + "('','" + messageData[1] + "');" + "},500);");
                                    }
                                }
                                noti.Actions = sbAction.ToString();
                            }
                        }

                        #endregion

                        string deptName = string.Empty;
                        if (commandType == CommandType.ScanBarcode.ToString())
                        {
                            TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
                            if (curentUser.TblStaff.TblDepartment != null)
                                deptName = curentUser.TblStaff.TblDepartment.DepartmentName;
                        }

                        noti.CommandType = notiSetting.CommandType.ToLower();
                        noti.Contents = string.Format(string.IsNullOrEmpty(contentMessageFormat) ?
                            notiSetting.Description : contentMessageFormat
                            , id, name, currentUserName, deptName);

                        noti.DismissEvent = notiSetting.DismissEvent;
                        noti.IsObsolete = false;
                        noti.PageId = notiSetting.PageId;

                        noti.ReceiveIds = notiSetting.ReceiveIds;

                        noti.ReceiveType = notiSetting.ReceiveType;
                        noti.Title = notiSetting.Title;

                        if (isExist == false)
                            return RealtimeNotificationManager.Insert(noti);
                        else
                            RealtimeNotificationManager.Update(noti);

                        if (isExist == true)
                        {
                            string uas = CommonHelper.QueryString("uas");
                            if (string.IsNullOrEmpty(uas) == false && uas.ToLower() == laterCheck.ToLower())
                            {
                                RealtimeNotificationManager.UpdateNotification(noti.NotificationID);
                            }
                            else
                            {
                                //RealtimeNotificationManager.UpdateNotification(noti.NotificationID);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                #endregion
            }
            return null;
        }

        public static void RemoveUsersFromReceiveList(List<int> idStaff)
        {
            if (idStaff != null && idStaff.Count > 0)
            {
                TblStaffCollection allSelected = new Select().From(TblStaff.Schema)
                    .Where(TblStaff.StaffIDColumn).In(idStaff)
                    .ExecuteAsCollection<TblStaffCollection>();
                if (allSelected != null && allSelected.Count > 0)
                    RemoveUsersFromReceiveList(allSelected);
            }
        }

        public static void RemoveUsersFromReceiveList(TblNotification tblNotification, int departmentId, int staffId)
        {
            if (string.IsNullOrEmpty(tblNotification.ReceiveIds))
            { }
            else
            {
                string[] oldArrId = null;
                string[] oldReceive = tblNotification.ReceiveIds.Split('|');
                if (oldReceive.Length == 1)
                {
                    oldArrId = oldReceive[0].Split(',');
                }
                else
                    oldArrId = oldReceive[1].Split(',');

                if (oldArrId != null && oldArrId.Length > 0)
                {
                    IEnumerable<string> found = null;
                    if (oldReceive.Length == 1)
                    {
                        found = oldArrId.Where(x => x == departmentId.ToString());
                        if (found != null && found.Any())
                        {
                            List<string> lst = oldArrId.ToList();
                            lst.Remove(departmentId.ToString());
                            UpdateReceiveIds(tblNotification, string.Join(",", lst.ToArray()), null, null);
                        }
                    }
                    else
                    {
                        found = oldArrId.Where(x => x == staffId.ToString());
                        if (found != null && found.Any())
                        {
                            List<string> lst = oldArrId.ToList();
                            lst.Remove(staffId.ToString());
                            UpdateReceiveIds(tblNotification,
                                String.Format("{0}|{1}", oldReceive[0], string.Join(",", lst.ToArray())), null, null);
                        }
                    }
                }
            }
        }

        public static void RemoveUsersFromReceiveList(TblStaffCollection lstStaff)
        {
            if (lstStaff != null && lstStaff.Count > 0)
            {
                foreach (TblStaff staff in lstStaff)
                {
                    if (staff.DepartmentID.HasValue)
                    {
                        TblNotificationCollection allNotifi = GetNotificationForDeparment(staff.DepartmentID.Value, false);
                        if (allNotifi != null && allNotifi.Count > 0)
                        {
                            foreach (TblNotification tblNotification in allNotifi)
                                RemoveUsersFromReceiveList(tblNotification, (int)staff.DepartmentID, staff.StaffID);
                        }
                    }
                }
            }
        }

        public static void RemoveCurrentUsersFromReceiveList(List<int> lstNotificationId)
        {
            TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
            if (curentUser != null)
                RemoveUsersFromReceiveList(new TblStaffCollection() { curentUser.TblStaff }, lstNotificationId);
        }

        public static void RemoveUsersFromReceiveList(TblStaff staff, List<int> lstNotificationId)
        {
            RemoveUsersFromReceiveList(new TblStaffCollection() { staff }, lstNotificationId);
        }

        public static void RemoveUsersFromReceiveList(TblStaffCollection lstStaff, List<int> lstNotificationId)
        {
            if (lstStaff == null || lstStaff.Count == 0)
                return;

            if (lstNotificationId == null || lstNotificationId.Count == 0)
            {
                RemoveUsersFromReceiveList(lstStaff);
                return;
            }

            TblNotificationCollection allNoti = new Select().From(TblNotification.Schema)
            .Where(TblNotification.NotificationIDColumn).In(lstNotificationId)
            .ExecuteAsCollection<TblNotificationCollection>();
            if (allNoti != null && allNoti.Count > 0)
            {
                foreach (TblNotification item in allNoti)
                {
                    if (string.IsNullOrEmpty(item.ReceiveIds) == false)
                    {
                        foreach (TblStaff staff in lstStaff)
                            RemoveUsersFromReceiveList(item, (int)staff.DepartmentID, staff.StaffID);
                    }
                }
            }
        }

        public static void AddUsersToReceiveList(List<int> idStaff)
        {
            if (idStaff != null && idStaff.Count > 0)
            {
                TblStaffCollection allSelected = new Select().From(TblStaff.Schema)
                    .Where(TblStaff.StaffIDColumn).In(idStaff)
                    .ExecuteAsCollection<TblStaffCollection>();
                if (allSelected != null && allSelected.Count > 0)
                    AddUsersToReceiveList(allSelected);
            }
        }

        public static void AddUsersToReceiveList(TblStaffCollection lstStaff)
        {
            if (lstStaff != null && lstStaff.Count > 0)
            {
                foreach (TblStaff staff in lstStaff)
                {
                    if (staff.DepartmentID.HasValue)
                    {
                        TblNotificationCollection allNotifi = GetNotificationForDeparment(staff.DepartmentID.Value);
                        if (allNotifi != null && allNotifi.Count > 0)
                        {
                            foreach (TblNotification tblNotification in allNotifi)
                            {
                                if (string.IsNullOrEmpty(tblNotification.ReceiveIds))
                                { }
                                else
                                {
                                    string[] oldArrId = null;
                                    string[] oldReceive = tblNotification.ReceiveIds.Split('|');
                                    if (oldReceive.Length == 1)
                                    {
                                        oldArrId = oldReceive[0].Split(',');
                                    }
                                    else
                                        oldArrId = oldReceive[1].Split(',');

                                    if (oldArrId != null && oldArrId.Length > 0)
                                    {
                                        IEnumerable<string> found = null;
                                        if (oldReceive.Length == 1)
                                        {
                                            found = oldArrId.Where(x => x == staff.DepartmentID.ToString());
                                            if (found != null && found.Any())
                                            { }
                                            else
                                                UpdateReceiveIds(tblNotification,
                                                    String.Format("{0},{1}", tblNotification.ReceiveIds, staff.DepartmentID), null, null);
                                        }
                                        else
                                        {
                                            found = oldArrId.Where(x => x == staff.StaffID.ToString());
                                            if (found != null && found.Any())
                                            { }
                                            else
                                            {
                                                string[] temp = oldReceive[0].Split(',');
                                                var foundDept = temp.Where(x => x == staff.DepartmentID.Value.ToString());
                                                if (foundDept != null && foundDept.Any())
                                                { }
                                                else
                                                    oldReceive[0] += "," + staff.DepartmentID.Value;
                                                UpdateReceiveIds(tblNotification,
                                                    oldReceive[0] + "|" + oldReceive[1] + "," + staff.StaffID, null, null);
                                            }
                                        }
                                    }
                                    else
                                    { }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static TblNotificationCollection GetNotificationForDeparment(int departmentId)
        {
            return GetNotificationForDeparment(null, departmentId, null);
        }

        public static TblNotificationCollection GetNotificationForDeparment(int departmentId, bool? isObsolete)
        {
            return GetNotificationForDeparment(null, departmentId, isObsolete);
        }

        public static TblNotificationCollection GetNotificationForDeparment(string title, int departmentId)
        {
            return GetNotificationForDeparment(title, departmentId, null);
        }

        public static TblNotificationCollection GetNotificationForDeparment(string title, int departmentId, bool? isObsolete)
        {
            string receiveIdscolumn = TblNotification.ReceiveIdsColumn.ColumnName;
            string template = "select * from " + TblNotification.Schema + " where (" +
receiveIdscolumn + " like '" + departmentId + "|%' or " +
" SUBSTRING(" + receiveIdscolumn + ",1, CHARINDEX('|', "
              + receiveIdscolumn + ") - 1) like '%," + departmentId + "' or" +
" SUBSTRING(" + receiveIdscolumn + ",1, CHARINDEX('|', "
              + receiveIdscolumn + ") - 1) like '" + departmentId + ",%' or" +
" SUBSTRING(" + receiveIdscolumn + ",1, CHARINDEX('|', " +
receiveIdscolumn + ") - 1) like '%," + departmentId + ",%') ";

            if (isObsolete.HasValue)
                template += " and IsObsolete=" + (isObsolete.Value ? "1" : "0");
            if (string.IsNullOrEmpty(title) == false)
                template += String.Format(" and title like '%{0}%'", title);

            return new InlineQuery()
                .ExecuteAsCollection<TblNotificationCollection>(template);
        }

        public static TblNotificationCollection GetNotificationForCurrentStaff(bool? isObsolete)
        {
            return GetNotificationForCurrentStaff(null, isObsolete);
        }

        public static TblNotificationCollection GetNotificationForCurrentStaff(string title, bool? isObsolete)
        {
            TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
            if (curentUser != null)
                return GetNotificationForStaff((int)curentUser.TblStaff.DepartmentID,
                    curentUser.TblStaff.StaffID, title, isObsolete);
            else
                return null;
        }

        public static TblNotificationCollection GetNotificationForStaff(int departmentId, int staffId, string title, bool? isObsolete)
        {
            string receiveIdscolumn = TblNotification.ReceiveIdsColumn.ColumnName;
            string template = " select * from " + TblNotification.Schema
                + " where (" + receiveIdscolumn + " like '%|" + staffId + "%' or " +
             " SUBSTRING(" + receiveIdscolumn + ", CHARINDEX('|', " + receiveIdscolumn + ")+1,LEN(" +
             receiveIdscolumn + ")- CHARINDEX('|', " + receiveIdscolumn + "))  like '%," + staffId + "' or " +
             " SUBSTRING(" + receiveIdscolumn + ", CHARINDEX('|', " + receiveIdscolumn + ")+1,LEN(" +
             receiveIdscolumn + ")- CHARINDEX('|', " + receiveIdscolumn + "))  like '" + staffId + ",%' or " +
             " SUBSTRING(" + receiveIdscolumn + ", CHARINDEX('|', " + receiveIdscolumn + ")+1,LEN(" +
             receiveIdscolumn + ")- CHARINDEX('|', " + receiveIdscolumn + "))  like '%," + staffId + ",%' )  ";

            if (isObsolete.HasValue)
                template += " and IsObsolete=" + (isObsolete.Value ? "1" : "0");
            if (string.IsNullOrEmpty(title) == false)
                template += " and title like '%" + title + "%'";

            TblNotificationCollection all = new InlineQuery()
                .ExecuteAsCollection<TblNotificationCollection>(template);
            if (all != null && all.Count > 0)
            {
                //kiểm tra đã đọc tin nhắn chưa
                List<string> lst = null;
                foreach (TblNotification tblNotification in all)
                {
                    //chưa ai đọc
                    if (string.IsNullOrEmpty(tblNotification.DateDismiss))
                        continue;

                    string type = tblNotification.ReceiveType;

                    if (type == NotificationType.Deparment.ToString())
                    {
                        if (string.IsNullOrEmpty(tblNotification.ReceiveIds) == false)
                            lst = tblNotification.ReceiveIds.Split(',').ToList();
                        else
                            lst = null;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(tblNotification.ReceiveIds) == false)
                            lst = tblNotification.ReceiveIds.Split('|')[1].Split(',').ToList();
                        else
                            lst = null;
                    }

                    if (lst != null && lst.Count > 0)
                    {
                        List<string> dataDateDismiss = tblNotification.DateDismiss.Split(',').ToList();
                        //List<string> dataDismissBy = tblNotification.DismissBy.Split(',').ToList();
                        for (int i = 0; i < lst.Count; i++)
                        {
                            if (type == NotificationType.Deparment.ToString())
                            {
                                if (departmentId > 0 && lst[i] == departmentId.ToString())
                                {
                                    if (dataDateDismiss.Count > i && string.IsNullOrEmpty(dataDateDismiss[i]) == false)
                                        tblNotification.IsObsolete = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (lst[i] == staffId.ToString())
                                {
                                    if (dataDateDismiss.Count > i && string.IsNullOrEmpty(dataDateDismiss[i]) == false)
                                        tblNotification.IsObsolete = true;
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            return all;
        }

        public static void UpdateReceiveIds(string pageId, string commandType, string receiveIds
            , string title, string content)
        {
            TblNotificationCollection allCurrentNoti = RealtimeNotificationManager
                .GetNotificationByPageIdAndCommandType(pageId, commandType, null);
            if (allCurrentNoti != null && allCurrentNoti.Count > 0)
            {
                foreach (TblNotification noti in allCurrentNoti)
                    UpdateReceiveIds(noti, receiveIds, title, content);
            }
        }

        static void UpdateReceiveIds(TblNotification noti, string receiveIds, string title, string contents)
        {
            if (noti.ReceiveIds != receiveIds)
            {
                //save old
                string[] oldArrId = null;
                string[] oldReceive = noti.ReceiveIds.Split('|');
                if (oldReceive.Length == 1)
                {
                    oldArrId = oldReceive[0].Split(',');
                }
                else
                    oldArrId = oldReceive[1].Split(',');

                Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
                List<string> dataDateDismiss = noti.DateDismiss != null ? noti.DateDismiss.Split(',').ToList() : new List<string>();
                List<string> dataDismissBy = noti.DismissBy != null ? noti.DismissBy.Split(',').ToList() : new List<string>();
                if (oldArrId != null && oldArrId.Length > 0)
                {
                    for (int i = 0; i < oldArrId.Length; i++)
                    {
                        if (dataDateDismiss.Count > i && string.IsNullOrEmpty(dataDateDismiss[i]) == false)
                            dic.Add(oldArrId[i], new string[] { dataDateDismiss[i], dataDismissBy[i] });
                    }
                }

                //update receive
                noti.ReceiveIds = receiveIds;
                noti.IsObsolete = false;

                if (dic.Count > 0)
                {
                    dataDateDismiss = new List<string>();
                    dataDismissBy = new List<string>();
                    string[] newArrId = null;
                    string[] newReceive = receiveIds.Split('|');
                    if (newReceive.Length == 1)
                        newArrId = newReceive[0].Split(',');
                    else
                        newArrId = newReceive[1].Split(',');

                    if (newArrId != null && newArrId.Length > 0)
                    {
                        for (int i = 0; i < newArrId.Length; i++)
                        {
                            if (dic.ContainsKey(newArrId[i]))
                            {
                                dataDateDismiss.Add(dic[newArrId[i]][0]);
                                dataDismissBy.Add(dic[newArrId[i]][1]);
                            }
                            else
                            {
                                dataDateDismiss.Add(string.Empty);
                                dataDismissBy.Add(string.Empty);
                            }
                        }

                        noti.DateDismiss = string.Join(",", dataDateDismiss.Select(s => "" + s).ToArray());
                        noti.DismissBy = string.Join(",", dataDismissBy.Select(s => "" + s).ToArray());
                    }
                }

                if (title != null)
                    noti.Title = title;

                if (contents != null)
                    noti.Contents = contents;

                RealtimeNotificationManager.Update(noti);
            }
            else
            {
                if (title != null && noti.Title != title)
                    noti.Title = title;

                if (contents != null && noti.Contents != contents)
                    noti.Contents = contents;

                RealtimeNotificationManager.Update(noti);
            }
        }
    }

    public enum NotificationType
    {
        Deparment,
        Staff
    }

    public enum CommandType
    {
        All,
        Insert,
        Delete,
        Update,
        ScanBarcode
    }

    public class NotificationObject
    {
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string action { get; set; }
        public string dismissevent { get; set; }
    }

    public class NotificationPage
    {
        public string id { get; set; }
        public string title { get; set; }
        public string functionid { get; set; }
        public string url { get; set; }
        public DateTime createdon { get; set; }
        public string createdby { get; set; }
    }
}