using Newtonsoft.Json;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class notification_Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string gid = Request.Form["gid"];
            if (string.IsNullOrEmpty(gid) == false)
            {
                string decode = string.Empty;
                try
                {
                    decode = SecurityHelper.Decrypt(gid);
                }
                catch (Exception ex)
                {

                }

                if (string.IsNullOrEmpty(decode) == false)
                {
                    TblNotification notifi = RealtimeNotificationManager.GetNotificationById(decode);
                    if (notifi != null)
                    {
                        Response.Write(JsonConvert.SerializeObject(new NotificationObject()
                        {
                            action = notifi.Actions,
                            content = notifi.Contents,
                            title = notifi.Title,
                            id = HttpUtility.UrlEncode(SweetSoft.APEM.Core.SecurityHelper.Encrypt(notifi.NotificationID.ToString())),
                            dismissevent = notifi.DismissEvent
                        }));
                    }
                }
                return;
            }

            string nid = Request.Form["nid"];
            if (string.IsNullOrEmpty(nid) == false)
            {
                string decode = string.Empty;
                try
                {
                    decode = SecurityHelper.Decrypt(nid);
                }
                catch (Exception ex)
                {

                }
                int notificationID = 0;
                int.TryParse(decode, out notificationID);
                if (notificationID > 0)
                {
                    bool isSucess = RealtimeNotificationManager.UpdateNotification(notificationID);
                    Response.Write(isSucess ? "1" : "0");
                }
                else
                    Response.Write("0");
            }
            else
            {
                TblNotificationCollection allNotifi = RealtimeNotificationManager.GetNotification();
                if (allNotifi != null && allNotifi.Count > 0)
                {
                    List<NotificationObject> lst = new List<NotificationObject>();
                    foreach (TblNotification item in allNotifi)
                    {
                        if (ValidateMessage(item))
                            lst.Add(new NotificationObject()
                            {
                                action = item.Actions,
                                content = item.Contents,
                                title = item.Title,
                                id = HttpUtility.UrlEncode(SweetSoft.APEM.Core.SecurityHelper.Encrypt(item.NotificationID.ToString())),
                                dismissevent = item.DismissEvent
                            });
                    }
                    Response.Write(JsonConvert.SerializeObject(lst));
                }
            }
        }

        static bool ValidateMessage(TblNotification item)
        {
            if (item.ReceiveIds != null && item.ReceiveIds.Length > 0)
            {
                NotificationType notificationType = (NotificationType)Enum.Parse(typeof(NotificationType), item.ReceiveType);
                List<int> lstId = new List<int>();
                string[] arrId = null;
                if (item.ReceiveIds.Contains('|'))
                {
                    string[] temp = item.ReceiveIds.Split('|');
                    if (temp.Length == 2)
                        arrId = temp[1].Split(',');
                }
                else
                    item.ReceiveIds.Split(',');
                if (arrId != null && arrId.Length > 0)
                {
                    int id;
                    foreach (string itemId in arrId)
                    {
                        if (int.TryParse(itemId, out id))
                            lstId.Add(id);
                    }
                }
                TblStaffCollection allStaff = null;
                switch (notificationType)
                {
                    case NotificationType.Deparment:
                        allStaff = RealtimeNotificationManager.GetStaffInDepartment(lstId);
                        break;
                    case NotificationType.Staff:
                        allStaff = RealtimeNotificationManager.GetStaffByIds(lstId);
                        break;
                }
                if (allStaff != null && allStaff.Count > 0)
                {
                    string username = ApplicationContext.Current.UserName;
                    foreach (TblStaff staff in allStaff)
                    {
                        if (staff.StaffID == ApplicationContext.Current.User.UserID)
                        {
                            if (item.DismissBy != null && item.DismissBy.Length > 0)
                            {
                                List<string> lst = item.DismissBy.Split(',').ToList();
                                if (lst != null && lst.Count > 0)
                                {
                                    for (int i = 0; i < lstId.Count; i++)
                                    {
                                        if (lst.Count > i && string.IsNullOrEmpty(lst[i]) == false
                                            && lst[i].ToLower() == username.ToLower())
                                            return false;
                                    }
                                }
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}