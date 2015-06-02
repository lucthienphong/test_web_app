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
    /// Controller class for tblPurchaseOrder_Cylinder
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblPurchaseOrderCylinderController
    {
        // Preload our schema..
        TblPurchaseOrderCylinder thisSchemaLoad = new TblPurchaseOrderCylinder();
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
        public TblPurchaseOrderCylinderCollection FetchAll()
        {
            TblPurchaseOrderCylinderCollection coll = new TblPurchaseOrderCylinderCollection();
            Query qry = new Query(TblPurchaseOrderCylinder.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblPurchaseOrderCylinderCollection FetchByID(object Id)
        {
            TblPurchaseOrderCylinderCollection coll = new TblPurchaseOrderCylinderCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblPurchaseOrderCylinderCollection FetchByQuery(Query qry)
        {
            TblPurchaseOrderCylinderCollection coll = new TblPurchaseOrderCylinderCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (TblPurchaseOrderCylinder.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (TblPurchaseOrderCylinder.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int PurchaseOrderID,int? CylinderID,decimal UnitPrice,int Quantity,string CylinderNo,double? Circumference,double? FaceWidth,int? JobID,string Unit,string Status,string DONumber,DateTime? DeliveryDate)
	    {
		    TblPurchaseOrderCylinder item = new TblPurchaseOrderCylinder();
		    
            item.PurchaseOrderID = PurchaseOrderID;
            
            item.CylinderID = CylinderID;
            
            item.UnitPrice = UnitPrice;
            
            item.Quantity = Quantity;
            
            item.CylinderNo = CylinderNo;
            
            item.Circumference = Circumference;
            
            item.FaceWidth = FaceWidth;
            
            item.JobID = JobID;
            
            item.Unit = Unit;
            
            item.Status = Status;
            
            item.DONumber = DONumber;
            
            item.DeliveryDate = DeliveryDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PurchaseOrderID,int? CylinderID,decimal UnitPrice,int Quantity,int Id,string CylinderNo,double? Circumference,double? FaceWidth,int? JobID,string Unit,string Status,string DONumber,DateTime? DeliveryDate)
	    {
		    TblPurchaseOrderCylinder item = new TblPurchaseOrderCylinder();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PurchaseOrderID = PurchaseOrderID;
				
			item.CylinderID = CylinderID;
				
			item.UnitPrice = UnitPrice;
				
			item.Quantity = Quantity;
				
			item.Id = Id;
				
			item.CylinderNo = CylinderNo;
				
			item.Circumference = Circumference;
				
			item.FaceWidth = FaceWidth;
				
			item.JobID = JobID;
				
			item.Unit = Unit;
				
			item.Status = Status;
				
			item.DONumber = DONumber;
				
			item.DeliveryDate = DeliveryDate;
				
	        item.Save(UserName);
	    }
    }
}
