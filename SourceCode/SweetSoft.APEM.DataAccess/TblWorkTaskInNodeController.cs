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
    /// Controller class for tblWorkTaskInNode
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblWorkTaskInNodeController
    {
        // Preload our schema..
        TblWorkTaskInNode thisSchemaLoad = new TblWorkTaskInNode();
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
        public TblWorkTaskInNodeCollection FetchAll()
        {
            TblWorkTaskInNodeCollection coll = new TblWorkTaskInNodeCollection();
            Query qry = new Query(TblWorkTaskInNode.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblWorkTaskInNodeCollection FetchByID(object Id)
        {
            TblWorkTaskInNodeCollection coll = new TblWorkTaskInNodeCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblWorkTaskInNodeCollection FetchByQuery(Query qry)
        {
            TblWorkTaskInNodeCollection coll = new TblWorkTaskInNodeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (TblWorkTaskInNode.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (TblWorkTaskInNode.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblWorkTaskInNode Insert(int WorkTaskID, int NodeID)
	    {
		    TblWorkTaskInNode item = new TblWorkTaskInNode();
		    
            item.WorkTaskID = WorkTaskID;
            
            item.NodeID = NodeID;
	    
		    item.Save(UserName);

            return item;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblWorkTaskInNode Update(int Id, int WorkTaskID, int NodeID)
	    {
		    TblWorkTaskInNode item = new TblWorkTaskInNode();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.WorkTaskID = WorkTaskID;
				
			item.NodeID = NodeID;
				
	        item.Save(UserName);

            return item;
	    }
    }
}