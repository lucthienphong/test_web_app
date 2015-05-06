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
    /// Controller class for tblPurchaseOrder
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblPurchaseOrderController
    {
        // Preload our schema..
        TblPurchaseOrder thisSchemaLoad = new TblPurchaseOrder();
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
        public TblPurchaseOrderCollection FetchAll()
        {
            TblPurchaseOrderCollection coll = new TblPurchaseOrderCollection();
            Query qry = new Query(TblPurchaseOrder.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblPurchaseOrderCollection FetchByID(object PurchaseOrderID)
        {
            TblPurchaseOrderCollection coll = new TblPurchaseOrderCollection().Where("PurchaseOrderID", PurchaseOrderID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblPurchaseOrderCollection FetchByQuery(Query qry)
        {
            TblPurchaseOrderCollection coll = new TblPurchaseOrderCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PurchaseOrderID)
        {
            return (TblPurchaseOrder.Delete(PurchaseOrderID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PurchaseOrderID)
        {
            return (TblPurchaseOrder.Destroy(PurchaseOrderID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblPurchaseOrder Insert(TblPurchaseOrder obj)
	    {
            obj.Save(UserName);
            return obj;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblPurchaseOrder Update(TblPurchaseOrder obj)
	    {
		    TblPurchaseOrder item = new TblPurchaseOrder();
	        item.MarkOld();
	        item.IsLoaded = true;

            item.PurchaseOrderID = obj.PurchaseOrderID;

            item.SupplierID = obj.SupplierID;

            item.OrderDate = obj.OrderDate;

            item.OrderNumber = obj.OrderNumber;

            item.RequiredDeliveryDate = obj.RequiredDeliveryDate;

            item.CylinderType = obj.CylinderType;

            item.Remark = obj.Remark;

            item.ContactName = obj.ContactName;

            item.ContactPhone = obj.ContactPhone;

            item.ContactEmail = obj.ContactEmail;

            item.CurrencyID = obj.CurrencyID;

            item.TotalNumberOfCylinders = obj.TotalNumberOfCylinders;

            item.IsUrgent = obj.IsUrgent;

            item.JobID = obj.JobID;

            item.CreatedBy = obj.CreatedBy;

            item.CreatedOn = obj.CreatedOn;

            item.ModifiedBy = obj.ModifiedBy;

            item.ModifiedOn = obj.ModifiedOn;
				
	        item.Save(UserName);
            return item;
	    }
    }
}