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
    /// Controller class for tblEngravingEtching
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblEngravingEtchingController
    {
        // Preload our schema..
        TblEngravingEtching thisSchemaLoad = new TblEngravingEtching();
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
        public TblEngravingEtchingCollection FetchAll()
        {
            TblEngravingEtchingCollection coll = new TblEngravingEtchingCollection();
            Query qry = new Query(TblEngravingEtching.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingEtchingCollection FetchByID(object EngravingID)
        {
            TblEngravingEtchingCollection coll = new TblEngravingEtchingCollection().Where("EngravingID", EngravingID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingEtchingCollection FetchByQuery(Query qry)
        {
            TblEngravingEtchingCollection coll = new TblEngravingEtchingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object EngravingID)
        {
            return (TblEngravingEtching.Delete(EngravingID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object EngravingID)
        {
            return (TblEngravingEtching.Destroy(EngravingID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblEngravingEtching Insert(TblEngravingEtching obj)
	    {
            obj.Save(UserName);
            return obj;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblEngravingEtching Update(TblEngravingEtching obj)
	    {
            obj.MarkOld();
            obj.IsLoaded = true;
            obj.Save(UserName);
            return obj;
	    }
    }
}
