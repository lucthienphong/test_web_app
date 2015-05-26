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
    /// Controller class for tblDataLogs
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblDataLogController
    {
        // Preload our schema..
        TblDataLog thisSchemaLoad = new TblDataLog();
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
        public TblDataLogCollection FetchAll()
        {
            TblDataLogCollection coll = new TblDataLogCollection();
            Query qry = new Query(TblDataLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDataLogCollection FetchByID(object Id)
        {
            TblDataLogCollection coll = new TblDataLogCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDataLogCollection FetchByQuery(Query qry)
        {
            TblDataLogCollection coll = new TblDataLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (TblDataLog.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (TblDataLog.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblDataLog Insert(TblDataLog obj)
	    {
		    TblDataLog item = new TblDataLog();

            item.ContentLogs = obj.ContentLogs;

            item.ActionDate = obj.ActionDate;

            item.UserIP = obj.UserIP;
	    
		    item.Save(UserName);

            return item;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblDataLog Update(TblDataLog obj)
	    {
		    TblDataLog item = new TblDataLog();
	        item.MarkOld();
	        item.IsLoaded = true;

            item.Id = obj.Id;

            item.ContentLogs = obj.ContentLogs;

            item.ActionDate = obj.ActionDate == null ? DateTime.Now : obj.ActionDate;

            item.UserIP = obj.UserIP;
				
	        item.Save(UserName);

            return item;
	    }
    }
}