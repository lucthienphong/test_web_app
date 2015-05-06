using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using SubSonic;
using SubSonic.Utilities;
// <auto-generated />
namespace SweetSoft.APEM.DataAccess
{
    /// <summary>
    /// Controller class for TblNotificationSetting
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblNotificationSettingController
    {
        // Preload our schema..
        TblNotificationSetting thisSchemaLoad = new TblNotificationSetting();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
                if (userName.Length == 0)
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        userName = System.Web.HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        userName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                    }
                }
                return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TblNotificationSettingCollection FetchAll()
        {
            TblNotificationSettingCollection coll = new TblNotificationSettingCollection();
            Query qry = new Query(TblNotificationSetting.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblNotificationSettingCollection FetchByID(object SettingId)
        {
            TblNotificationSettingCollection coll = new TblNotificationSettingCollection().Where("SettingId", SettingId).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblNotificationSettingCollection FetchByQuery(Query qry)
        {
            TblNotificationSettingCollection coll = new TblNotificationSettingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SettingId)
        {
            return (TblNotificationSetting.Delete(SettingId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SettingId)
        {
            return (TblNotificationSetting.Destroy(SettingId) == 1);
        }



        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblNotificationSetting Insert(string Title, string Description,
            bool IsObsolete, string TriggerButton, string Actions
            , string PageId, string CommandType, string DismissEvent,
            string ReceiveIds, string ReceiveType,
           string CreatedBy, DateTime? CreatedOn,
           string ModifiedBy, DateTime? ModifiedOn)
        {
            TblNotificationSetting item = new TblNotificationSetting();

            item.Title = Title;

            item.Description = Description;

            item.TriggerButton = TriggerButton;

            item.IsObsolete = IsObsolete;

            item.Actions = Actions;

            item.PageId = PageId;

            item.CommandType = CommandType;

            item.DismissEvent = DismissEvent;

            item.ReceiveIds = ReceiveIds;
            
            item.ReceiveType = ReceiveType;

            item.CreatedBy = CreatedBy;
            item.CreatedOn = CreatedOn;
            item.ModifiedBy = ModifiedBy;
            item.ModifiedOn = ModifiedOn;

            item.Save(UserName);

            return item;
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblNotificationSetting Update(int SettingId, string Title, string Description,
            bool IsObsolete, string TriggerButton, string Actions
            , string PageId, string CommandType, string DismissEvent,
            string ReceiveIds, string ReceiveType,
           string CreatedBy, DateTime? CreatedOn,
           string ModifiedBy, DateTime? ModifiedOn)
        {
            TblNotificationSetting item = new TblNotificationSetting();
            item.MarkOld();
            item.IsLoaded = true;

            item.SettingId = SettingId;

            item.Title = Title;

            item.Description = Description;

            item.TriggerButton = TriggerButton;

            item.IsObsolete = IsObsolete;

            item.Actions = Actions;

            item.PageId = PageId;

            item.CommandType = CommandType;

            item.DismissEvent = DismissEvent;
            
            item.ReceiveIds = ReceiveIds;
            
            item.ReceiveType = ReceiveType;

            item.CreatedBy = CreatedBy;
            item.CreatedOn = CreatedOn;
            item.ModifiedBy = ModifiedBy;
            item.ModifiedOn = ModifiedOn;

            item.Save(UserName);

            return item;
        }
    }
}