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
    /// Controller class for tblCodeDictionary
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblCodeDictionaryController
    {
        // Preload our schema..
        TblCodeDictionary thisSchemaLoad = new TblCodeDictionary();
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
        public TblCodeDictionaryCollection FetchAll()
        {
            TblCodeDictionaryCollection coll = new TblCodeDictionaryCollection();
            Query qry = new Query(TblCodeDictionary.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCodeDictionaryCollection FetchByID(object CodeID)
        {
            TblCodeDictionaryCollection coll = new TblCodeDictionaryCollection().Where("CodeID", CodeID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCodeDictionaryCollection FetchByQuery(Query qry)
        {
            TblCodeDictionaryCollection coll = new TblCodeDictionaryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CodeID)
        {
            return (TblCodeDictionary.Delete(CodeID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CodeID)
        {
            return (TblCodeDictionary.Destroy(CodeID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Code,bool IsUsed)
	    {
		    TblCodeDictionary item = new TblCodeDictionary();
		    
            item.Code = Code;
            
            item.IsUsed = IsUsed;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CodeID,string Code,bool IsUsed)
	    {
		    TblCodeDictionary item = new TblCodeDictionary();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CodeID = CodeID;
				
			item.Code = Code;
				
			item.IsUsed = IsUsed;
				
	        item.Save(UserName);
	    }
    }
}
