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
    /// Controller class for tblDeliveryOrderLockStatus
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblDeliveryOrderLockStatusController
    {
        // Preload our schema..
        TblDeliveryOrderLockStatus thisSchemaLoad = new TblDeliveryOrderLockStatus();
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
        public TblDeliveryOrderLockStatusCollection FetchAll()
        {
            TblDeliveryOrderLockStatusCollection coll = new TblDeliveryOrderLockStatusCollection();
            Query qry = new Query(TblDeliveryOrderLockStatus.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDeliveryOrderLockStatusCollection FetchByID(object JobID)
        {
            TblDeliveryOrderLockStatusCollection coll = new TblDeliveryOrderLockStatusCollection().Where("JobID", JobID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDeliveryOrderLockStatusCollection FetchByQuery(Query qry)
        {
            TblDeliveryOrderLockStatusCollection coll = new TblDeliveryOrderLockStatusCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object JobID)
        {
            return (TblDeliveryOrderLockStatus.Delete(JobID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object JobID)
        {
            return (TblDeliveryOrderLockStatus.Destroy(JobID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int JobID,bool LockX)
	    {
		    TblDeliveryOrderLockStatus item = new TblDeliveryOrderLockStatus();
		    
            item.JobID = JobID;
            
            item.LockX = LockX;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int JobID,bool LockX)
	    {
		    TblDeliveryOrderLockStatus item = new TblDeliveryOrderLockStatus();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.JobID = JobID;
				
			item.LockX = LockX;
				
	        item.Save(UserName);
	    }
    }
}