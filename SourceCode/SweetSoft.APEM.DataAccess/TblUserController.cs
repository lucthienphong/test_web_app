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
    /// Controller class for tblUser
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblUserController
    {
        // Preload our schema..
        TblUser thisSchemaLoad = new TblUser();
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
        public TblUserCollection FetchAll()
        {
            TblUserCollection coll = new TblUserCollection();
            Query qry = new Query(TblUser.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblUserCollection FetchByID(object UserID)
        {
            TblUserCollection coll = new TblUserCollection().Where("UserID", UserID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblUserCollection FetchByQuery(Query qry)
        {
            TblUserCollection coll = new TblUserCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object UserID)
        {
            return (TblUser.Delete(UserID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object UserID)
        {
            return (TblUser.Destroy(UserID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblUser Insert(TblUser obj)
	    {
            obj.Save(UserName);
            return obj;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblUser Update(TblUser obj)
	    {
            obj.MarkOld();
            obj.IsLoaded = true;
            obj.Save(UserName);
            return obj;
	    }
    }
}
