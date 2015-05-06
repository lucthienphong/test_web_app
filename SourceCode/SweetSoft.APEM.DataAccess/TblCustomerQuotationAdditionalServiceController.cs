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
    /// Controller class for tblCustomerQuotation_AdditionalService
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblCustomerQuotationAdditionalServiceController
    {
        // Preload our schema..
        TblCustomerQuotationAdditionalService thisSchemaLoad = new TblCustomerQuotationAdditionalService();
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
        public TblCustomerQuotationAdditionalServiceCollection FetchAll()
        {
            TblCustomerQuotationAdditionalServiceCollection coll = new TblCustomerQuotationAdditionalServiceCollection();
            Query qry = new Query(TblCustomerQuotationAdditionalService.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCustomerQuotationAdditionalServiceCollection FetchByID(object Id)
        {
            TblCustomerQuotationAdditionalServiceCollection coll = new TblCustomerQuotationAdditionalServiceCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCustomerQuotationAdditionalServiceCollection FetchByQuery(Query qry)
        {
            TblCustomerQuotationAdditionalServiceCollection coll = new TblCustomerQuotationAdditionalServiceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (TblCustomerQuotationAdditionalService.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (TblCustomerQuotationAdditionalService.Destroy(Id) == 1);
        }



        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblCustomerQuotationAdditionalService Insert(TblCustomerQuotationAdditionalService obj)
        {
            obj.Save(UserName);
            return obj;
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblCustomerQuotationAdditionalService Update(TblCustomerQuotationAdditionalService obj)
        {
            TblCustomerQuotationAdditionalService item = new TblCustomerQuotationAdditionalService();
            item.MarkOld();
            item.IsLoaded = true;

            item.Id = obj.Id;

            item.Category = obj.Category;

            item.GLCode = obj.GLCode;

            item.Description = obj.Description;

            item.Price = obj.Price;

            item.CurrencyID = obj.CurrencyID;

            item.CustomerID = obj.CustomerID;

            item.Save(UserName);

            return item;
        }
    }
}
