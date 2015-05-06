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
    /// Controller class for tblEngraving_ScreenAngle
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblEngravingScreenAngleController
    {
        // Preload our schema..
        TblEngravingScreenAngle thisSchemaLoad = new TblEngravingScreenAngle();
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
        public TblEngravingScreenAngleCollection FetchAll()
        {
            TblEngravingScreenAngleCollection coll = new TblEngravingScreenAngleCollection();
            Query qry = new Query(TblEngravingScreenAngle.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingScreenAngleCollection FetchByID(object Screen)
        {
            TblEngravingScreenAngleCollection coll = new TblEngravingScreenAngleCollection().Where("Screen", Screen).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingScreenAngleCollection FetchByQuery(Query qry)
        {
            TblEngravingScreenAngleCollection coll = new TblEngravingScreenAngleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Screen)
        {
            return (TblEngravingScreenAngle.Delete(Screen) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Screen)
        {
            return (TblEngravingScreenAngle.Destroy(Screen) == 1);
        }
        
        
        
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(string Screen,int Angle)
        {
            Query qry = new Query(TblEngravingScreenAngle.Schema);
            qry.QueryType = QueryType.Delete;
            qry.AddWhere("Screen", Screen).AND("Angle", Angle);
            qry.Execute();
            return (true);
        }        
       
    	
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblEngravingScreenAngle Insert(TblEngravingScreenAngle item)
	    {
		    item.Save(UserName);
            return item;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblEngravingScreenAngle Update(TblEngravingScreenAngle item)
	    {
	        item.MarkOld();
	        item.IsLoaded = true;
	        item.Save(UserName);
            return item;
	    }
    }
}
