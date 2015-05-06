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
    /// Controller class for tblOtherCharges
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblOtherChargeController
    {
        // Preload our schema..
        TblOtherCharge thisSchemaLoad = new TblOtherCharge();
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
        public TblOtherChargeCollection FetchAll()
        {
            TblOtherChargeCollection coll = new TblOtherChargeCollection();
            Query qry = new Query(TblOtherCharge.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblOtherChargeCollection FetchByID(object OtherChargesID)
        {
            TblOtherChargeCollection coll = new TblOtherChargeCollection().Where("OtherChargesID", OtherChargesID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblOtherChargeCollection FetchByQuery(Query qry)
        {
            TblOtherChargeCollection coll = new TblOtherChargeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object OtherChargesID)
        {
            return (TblOtherCharge.Delete(OtherChargesID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object OtherChargesID)
        {
            return (TblOtherCharge.Destroy(OtherChargesID) == 1);
        }



        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblOtherCharge Insert(TblOtherCharge obj)
        {
            obj.Save(UserName);
            return obj;
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblOtherCharge Update(TblOtherCharge obj)
        {
            TblOtherCharge item = new TblOtherCharge();
            item.MarkOld();
            item.IsLoaded = true;

            item.OtherChargesID = obj.OtherChargesID;

            item.GLCode = obj.GLCode;

            item.Description = obj.Description;

            item.PricingID = obj.PricingID;

            item.Charge = obj.Charge;

            item.Quantity = obj.Quantity;

            item.JobID = obj.JobID;

            item.Save(UserName);
            return item;
        }
    }
}