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
    /// Controller class for tblWorkFlowLine
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblWorkFlowLineController
    {
        // Preload our schema..
        TblWorkFlowLine thisSchemaLoad = new TblWorkFlowLine();
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
        public TblWorkFlowLineCollection FetchAll()
        {
            TblWorkFlowLineCollection coll = new TblWorkFlowLineCollection();
            Query qry = new Query(TblWorkFlowLine.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblWorkFlowLineCollection FetchByID(object Id)
        {
            TblWorkFlowLineCollection coll = new TblWorkFlowLineCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblWorkFlowLineCollection FetchByQuery(Query qry)
        {
            TblWorkFlowLineCollection coll = new TblWorkFlowLineCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (TblWorkFlowLine.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (TblWorkFlowLine.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblWorkFlowLine Insert(int? Node1ID, int? Node2ID, int? WorkFlowID, string LineType, int? MachineryProduceTypeID, string WorkFlowCode, int? WorkFlowIDInXML, int? WorkFlowToID, string CreatedBy, DateTime? CreatedOn, string ModifiedBy, DateTime? ModifiedOn)
	    {
		    TblWorkFlowLine item = new TblWorkFlowLine();
		    
            item.Node1ID = Node1ID;
            
            item.Node2ID = Node2ID;
            
            item.WorkFlowID = WorkFlowID;
            
            item.LineType = LineType;
            
            item.MachineryProduceTypeID = MachineryProduceTypeID;
            
            item.WorkFlowCode = WorkFlowCode;
            
            item.WorkFlowIDInXML = WorkFlowIDInXML;
            
            item.WorkFlowToID = WorkFlowToID;
            
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
        public TblWorkFlowLine Update(int Id, int? Node1ID, int? Node2ID, int? WorkFlowID, string LineType, int? MachineryProduceTypeID, string WorkFlowCode, int? WorkFlowIDInXML, int? WorkFlowToID, string CreatedBy, DateTime? CreatedOn, string ModifiedBy, DateTime? ModifiedOn)
	    {
		    TblWorkFlowLine item = new TblWorkFlowLine();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Node1ID = Node1ID;
				
			item.Node2ID = Node2ID;
				
			item.WorkFlowID = WorkFlowID;
				
			item.LineType = LineType;
				
			item.MachineryProduceTypeID = MachineryProduceTypeID;
				
			item.WorkFlowCode = WorkFlowCode;
				
			item.WorkFlowIDInXML = WorkFlowIDInXML;
				
			item.WorkFlowToID = WorkFlowToID;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
	        item.Save(UserName);

            return item;
	    }
    }
}
