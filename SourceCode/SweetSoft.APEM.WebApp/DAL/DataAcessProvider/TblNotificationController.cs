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
    /// Controller class for tblNotification
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblNotificationController
    {
        // Preload our schema..
        TblNotification thisSchemaLoad = new TblNotification();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TblNotificationCollection FetchAll()
        {
            TblNotificationCollection coll = new TblNotificationCollection();
            Query qry = new Query(TblNotification.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblNotificationCollection FetchByID(object NotificationID)
        {
            TblNotificationCollection coll = new TblNotificationCollection().Where("NotificationID", NotificationID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblNotificationCollection FetchByQuery(Query qry)
        {
            TblNotificationCollection coll = new TblNotificationCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object NotificationID)
        {
            return (TblNotification.Delete(NotificationID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object NotificationID)
        {
            return (TblNotification.Destroy(NotificationID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Title,string Contents,bool IsObsolete,string ReceiveIds,string ReceiveType,string Actions,string CreatedBy,DateTime CreatedOn,string DateDismiss,string DismissBy,string DismissEvent,string PageId,string CommandType)
	    {
		    TblNotification item = new TblNotification();
		    
            item.Title = Title;
            
            item.Contents = Contents;
            
            item.IsObsolete = IsObsolete;
            
            item.ReceiveIds = ReceiveIds;
            
            item.ReceiveType = ReceiveType;
            
            item.Actions = Actions;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.DateDismiss = DateDismiss;
            
            item.DismissBy = DismissBy;
            
            item.DismissEvent = DismissEvent;
            
            item.PageId = PageId;
            
            item.CommandType = CommandType;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int NotificationID,string Title,string Contents,bool IsObsolete,string ReceiveIds,string ReceiveType,string Actions,string CreatedBy,DateTime CreatedOn,string DateDismiss,string DismissBy,string DismissEvent,string PageId,string CommandType)
	    {
		    TblNotification item = new TblNotification();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.NotificationID = NotificationID;
				
			item.Title = Title;
				
			item.Contents = Contents;
				
			item.IsObsolete = IsObsolete;
				
			item.ReceiveIds = ReceiveIds;
				
			item.ReceiveType = ReceiveType;
				
			item.Actions = Actions;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.DateDismiss = DateDismiss;
				
			item.DismissBy = DismissBy;
				
			item.DismissEvent = DismissEvent;
				
			item.PageId = PageId;
				
			item.CommandType = CommandType;
				
	        item.Save(UserName);
	    }
    }
}
