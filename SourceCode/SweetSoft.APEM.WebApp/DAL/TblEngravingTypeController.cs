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
    /// Controller class for tblEngravingType
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblEngravingTypeController
    {
        // Preload our schema..
        TblEngravingType thisSchemaLoad = new TblEngravingType();
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
        public TblEngravingTypeCollection FetchAll()
        {
            TblEngravingTypeCollection coll = new TblEngravingTypeCollection();
            Query qry = new Query(TblEngravingType.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingTypeCollection FetchByID(object EngravingTypeID)
        {
            TblEngravingTypeCollection coll = new TblEngravingTypeCollection().Where("EngravingTypeID", EngravingTypeID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingTypeCollection FetchByQuery(Query qry)
        {
            TblEngravingTypeCollection coll = new TblEngravingTypeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object EngravingTypeID)
        {
            return (TblEngravingType.Delete(EngravingTypeID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object EngravingTypeID)
        {
            return (TblEngravingType.Destroy(EngravingTypeID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string EngravingTypeName,bool IsObsolete)
	    {
		    TblEngravingType item = new TblEngravingType();
		    
            item.EngravingTypeName = EngravingTypeName;
            
            item.IsObsolete = IsObsolete;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(short EngravingTypeID,string EngravingTypeName,bool IsObsolete)
	    {
		    TblEngravingType item = new TblEngravingType();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.EngravingTypeID = EngravingTypeID;
				
			item.EngravingTypeName = EngravingTypeName;
				
			item.IsObsolete = IsObsolete;
				
	        item.Save(UserName);
	    }
    }
}
