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
    /// Controller class for tblEngravingDetail
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblEngravingDetailController
    {
        // Preload our schema..
        TblEngravingDetail thisSchemaLoad = new TblEngravingDetail();
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
        public TblEngravingDetailCollection FetchAll()
        {
            TblEngravingDetailCollection coll = new TblEngravingDetailCollection();
            Query qry = new Query(TblEngravingDetail.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingDetailCollection FetchByID(object EngravingID)
        {
            TblEngravingDetailCollection coll = new TblEngravingDetailCollection().Where("EngravingID", EngravingID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblEngravingDetailCollection FetchByQuery(Query qry)
        {
            TblEngravingDetailCollection coll = new TblEngravingDetailCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object EngravingID)
        {
            return (TblEngravingDetail.Delete(EngravingID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object EngravingID)
        {
            return (TblEngravingDetail.Destroy(EngravingID) == 1);
        }



        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblEngravingDetail Insert(TblEngravingDetail obj)
        {
            obj.Save(UserName);
            return obj;
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblEngravingDetail Update(TblEngravingDetail obj)
        {
            obj.MarkOld();
            obj.IsLoaded = true;
            obj.Save(UserName);
            return obj;
        }
    }
}
