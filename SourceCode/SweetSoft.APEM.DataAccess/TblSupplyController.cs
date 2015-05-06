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
    /// Controller class for tblSupply
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblSupplyController
    {
        // Preload our schema..
        TblSupply thisSchemaLoad = new TblSupply();
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
        public TblSupplyCollection FetchAll()
        {
            TblSupplyCollection coll = new TblSupplyCollection();
            Query qry = new Query(TblSupply.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblSupplyCollection FetchByID(object SupplyID)
        {
            TblSupplyCollection coll = new TblSupplyCollection().Where("SupplyID", SupplyID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblSupplyCollection FetchByQuery(Query qry)
        {
            TblSupplyCollection coll = new TblSupplyCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SupplyID)
        {
            return (TblSupply.Delete(SupplyID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SupplyID)
        {
            return (TblSupply.Destroy(SupplyID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblSupply Insert(TblSupply item)
	    {
		    item.Save(UserName);
            return item;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblSupply Update(TblSupply item)
	    {
	        item.MarkOld();
	        item.IsLoaded = true;
	        item.Save(UserName);
            return item;
	    }
    }
}
