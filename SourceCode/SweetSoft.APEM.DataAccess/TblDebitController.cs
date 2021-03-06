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
    /// Controller class for tblDebit
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblDebitController
    {
        // Preload our schema..
        TblDebit thisSchemaLoad = new TblDebit();
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
        public TblDebitCollection FetchAll()
        {
            TblDebitCollection coll = new TblDebitCollection();
            Query qry = new Query(TblDebit.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDebitCollection FetchByID(object DebitID)
        {
            TblDebitCollection coll = new TblDebitCollection().Where("DebitID", DebitID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDebitCollection FetchByQuery(Query qry)
        {
            TblDebitCollection coll = new TblDebitCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object DebitID)
        {
            return (TblDebit.Delete(DebitID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object DebitID)
        {
            return (TblDebit.Destroy(DebitID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblDebit Insert(TblDebit obj)
	    {
            obj.Save(UserName);
            return obj;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblDebit Update(TblDebit obj)
	    {
		    TblDebit item = new TblDebit();
	        item.MarkOld();
	        item.IsLoaded = true;

            item.DebitID = obj.DebitID;

            item.DebitNo = obj.DebitNo;

            item.DebitDate = obj.DebitDate;

            item.CustomerID = obj.CustomerID;

            item.CurrencyID = obj.CurrencyID;

            item.TermsOfPayment = obj.TermsOfPayment;

            item.TermsOfDelivery = obj.TermsOfDelivery;

            item.Remark = obj.Remark;

            item.Total = obj.Total;

            item.CreatedBy = obj.CreatedBy;

            item.CreatedOn = obj.CreatedOn;

            item.ModifiedBy = obj.ModifiedBy;

            item.ModifiedOn = obj.ModifiedOn;

            item.TaxID = obj.TaxID;
				
	        item.Save(UserName);

            return item;
	    }
    }
}
