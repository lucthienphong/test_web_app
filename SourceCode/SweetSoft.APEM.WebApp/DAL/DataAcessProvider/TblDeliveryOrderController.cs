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
    /// Controller class for tblDeliveryOrder
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblDeliveryOrderController
    {
        // Preload our schema..
        TblDeliveryOrder thisSchemaLoad = new TblDeliveryOrder();
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
        public TblDeliveryOrderCollection FetchAll()
        {
            TblDeliveryOrderCollection coll = new TblDeliveryOrderCollection();
            Query qry = new Query(TblDeliveryOrder.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDeliveryOrderCollection FetchByID(object JobID)
        {
            TblDeliveryOrderCollection coll = new TblDeliveryOrderCollection().Where("JobID", JobID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDeliveryOrderCollection FetchByQuery(Query qry)
        {
            TblDeliveryOrderCollection coll = new TblDeliveryOrderCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object JobID)
        {
            return (TblDeliveryOrder.Delete(JobID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object JobID)
        {
            return (TblDeliveryOrder.Destroy(JobID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int JobID,string DONumber,string CustomerPO1,string CustomerPO2,int ContactPersonID,DateTime OrderDate,string OtherItem,int? PackingID,string GrossWeigth,string NetWeight,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    TblDeliveryOrder item = new TblDeliveryOrder();
		    
            item.JobID = JobID;
            
            item.DONumber = DONumber;
            
            item.CustomerPO1 = CustomerPO1;
            
            item.CustomerPO2 = CustomerPO2;
            
            item.ContactPersonID = ContactPersonID;
            
            item.OrderDate = OrderDate;
            
            item.OtherItem = OtherItem;
            
            item.PackingID = PackingID;
            
            item.GrossWeigth = GrossWeigth;
            
            item.NetWeight = NetWeight;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int JobID,string DONumber,string CustomerPO1,string CustomerPO2,int ContactPersonID,DateTime OrderDate,string OtherItem,int? PackingID,string GrossWeigth,string NetWeight,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    TblDeliveryOrder item = new TblDeliveryOrder();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.JobID = JobID;
				
			item.DONumber = DONumber;
				
			item.CustomerPO1 = CustomerPO1;
				
			item.CustomerPO2 = CustomerPO2;
				
			item.ContactPersonID = ContactPersonID;
				
			item.OrderDate = OrderDate;
				
			item.OtherItem = OtherItem;
				
			item.PackingID = PackingID;
				
			item.GrossWeigth = GrossWeigth;
				
			item.NetWeight = NetWeight;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
	        item.Save(UserName);
	    }
    }
}
