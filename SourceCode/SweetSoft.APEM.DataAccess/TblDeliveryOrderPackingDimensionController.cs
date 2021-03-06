using SubSonic;
using System;
using System.ComponentModel;

// <auto-generated />
namespace SweetSoft.APEM.DataAccess
{
    /// <summary>
    /// Controller class for TblDeliveryOrder_PackingDimension
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblDeliveryOrderPackingDimensionController
    {
        // Preload our schema..
        private TblDeliveryOrderPackingDimension thisSchemaLoad = new TblDeliveryOrderPackingDimension();

        private string userName = String.Empty;

        protected string UserName
        {
            get
            {
                if (userName.Length == 0)
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        userName = System.Web.HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        userName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                    }
                }
                return userName;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TblDeliveryOrderPackingDimensionCollection FetchAll()
        {
            TblDeliveryOrderPackingDimensionCollection coll = new TblDeliveryOrderPackingDimensionCollection();
            Query qry = new Query(TblDeliveryOrderPackingDimension.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDeliveryOrderPackingDimensionCollection FetchByID(object PackingDimensionID)
        {
            TblDeliveryOrderPackingDimensionCollection coll = new TblDeliveryOrderPackingDimensionCollection().Where("PackingDimensionID", PackingDimensionID).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDeliveryOrderPackingDimensionCollection FetchByQuery(Query qry)
        {
            TblDeliveryOrderPackingDimensionCollection coll = new TblDeliveryOrderPackingDimensionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PackingDimensionID)
        {
            return (TblDeliveryOrderPackingDimension.Delete(PackingDimensionID) == 1);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PackingDimensionID)
        {
            return (TblDeliveryOrderPackingDimension.Destroy(PackingDimensionID) == 1);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(int PackingDimensionID, int JobID)
        {
            Query qry = new Query(TblDeliveryOrderPackingDimension.Schema);
            qry.QueryType = QueryType.Delete;
            qry.AddWhere("PackingDimensionID", PackingDimensionID).AND("JobID", JobID);
            qry.Execute();
            return (true);
        }

        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblDeliveryOrderPackingDimension Insert(TblDeliveryOrderPackingDimension item)
        {
            item.Save(UserName);
            return item;
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblDeliveryOrderPackingDimension Update(TblDeliveryOrderPackingDimension item)
        {
            item.MarkOld();
            item.IsLoaded = true;
            item.Save(UserName);
            return item;
        }
    }
}