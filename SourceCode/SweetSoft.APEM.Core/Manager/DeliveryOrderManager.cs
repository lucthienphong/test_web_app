using SubSonic;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace SweetSoft.APEM.Core.Manager
{
    public class DeliveryOrderPackingDimensionExtension : TblDeliveryOrderPackingDimension
    {
        //Phần bổ sung
        public TblDeliveryOrderPackingDimension Parent { get; set; }
        //Chuyển Parents -> Children
        public DeliveryOrderPackingDimensionExtension() { }
        public DeliveryOrderPackingDimensionExtension(TblDeliveryOrderPackingDimension parent)
        {
            Parent = parent;
            try
            {
                foreach (PropertyInfo prop in this.GetType().GetProperties())
                {
                    if (Parent.GetType().GetProperty(prop.Name) != null)
                        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
                }
            }
            catch
            {
            }
        }
        public int VitualID { set; get; }
        public string PackingDimensionName { set; get; }
    }

    public class DeliveryOrderManager
    {
        #region DeliveryOrder
        public static string CreateDONumber()
        {
            string _No = "1" + DateTime.Today.ToString("yy") + "2";
            string _MaxNumber = new Select(Aggregate.Max(TblDeliveryOrder.DONumberColumn))
                .From(TblDeliveryOrder.Schema)
                .Where(TblDeliveryOrder.DONumberColumn).Like(_No + "%")
                .ExecuteScalar<string>();
            _MaxNumber = _MaxNumber ?? "";
            if (_MaxNumber.Length > 0)
            {
                string _righ = (int.Parse(_MaxNumber.Substring(_MaxNumber.Length - 5, 5)) + 1).ToString();
                while (_righ.Length < 5)
                    _righ = "0" + _righ;
                _No += _righ;
            }
            else
                _No += "00001";
            return _No;
        }

        public static TblDeliveryOrder SelectDeliveryOrderByJobID(int jobID)
        {
            return new Select().From(TblDeliveryOrder.Schema).Where(TblDeliveryOrder.JobIDColumn).IsEqualTo(jobID).ExecuteSingle<TblDeliveryOrder>();
        }

        public static TblDeliveryOrderCollection SelectDeliveryOrderByCustomerID(int customerID)
        {
            List<int> exIDs = new List<int>();
            exIDs = new Select(TblInvoiceDetail.JobIDColumn).From(TblInvoiceDetail.Schema).ExecuteTypedList<int>();

            Select select = new Select();
            select.From(TblDeliveryOrder.Schema);
            select.InnerJoin(TblJob.JobIDColumn, TblDeliveryOrder.JobIDColumn);
            select.InnerJoin(TblOrderConfirmation.JobIDColumn, TblDeliveryOrder.JobIDColumn);
            select.Where(TblJob.CustomerIDColumn).IsEqualTo(customerID);
            select.And(TblJob.IsClosedColumn).IsEqualTo(0);
            select.And(TblJob.RevNumberColumn).IsEqualTo(0);
            if (exIDs != null && exIDs.Count > 0)
                select.And(TblDeliveryOrder.JobIDColumn).NotIn(exIDs);
            return select.ExecuteAsCollection<TblDeliveryOrderCollection>();
        }

        public static TblDeliveryOrder InsertDeliveryOrder(TblDeliveryOrder devO)
        {
            return new TblDeliveryOrderController().Insert(devO);
        }

        public static TblDeliveryOrder UpdateDeliveryOrder(TblDeliveryOrder devO)
        {
            return new TblDeliveryOrderController().Update(devO);
        }

        public static DataTable SelectDeliveryOrderByCustID(string Customer, string DONumber, string Job, DateTime? FromDate, DateTime? ToDate, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.SearchDeliveryOrderByCustomer(Customer, DONumber, Job, FromDate, ToDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public static bool Delete(int ID)
        {
            return new TblDeliveryOrderController().Delete(ID);
        }

        public static DataTable SelectDeliveryOrderForPrint(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblDeliverySelectSummaryForPrint(JobID).GetReader());
            return dt;
        }
        #endregion


        #region PackingDemension
        public static List<DeliveryOrderPackingDimensionExtension> SelectPackingDimensionDetail(int JobID)
        {
            List<DeliveryOrderPackingDimensionExtension> list = new List<DeliveryOrderPackingDimensionExtension>();
            //Lấy từ điển
            var dictionary = new Select().From(TblReference.Schema)
                                    .Where(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.PackingDimension)
                                    .ExecuteAsCollection<TblReferenceCollection>();
            //Lấy danh sách chi tiết
            TblDeliveryOrderPackingDimensionCollection coll = new TblDeliveryOrderPackingDimensionCollection();
            coll = new Select().From(TblDeliveryOrderPackingDimension.Schema)
                                .Where(TblDeliveryOrderPackingDimension.JobIDColumn).IsEqualTo(JobID)
                                .ExecuteAsCollection<TblDeliveryOrderPackingDimensionCollection>();
            Random rnd = new Random();
            int rndID = rnd.Next(-1000, -1);
            foreach (TblDeliveryOrderPackingDimension item in coll)
            {
                while (list.Where(x => x.VitualID == rndID).Count() > 0)
                    rndID = rnd.Next(-1000, -1);
                DeliveryOrderPackingDimensionExtension obj = new DeliveryOrderPackingDimensionExtension(item);
                obj.VitualID = rndID;
                obj.PackingDimensionName = dictionary.Where(x => x.ReferencesID == item.PackingDimensionID).Select(x => x.Name).FirstOrDefault();
                list.Add(obj);
            }
            return list;
        }

        public static TblDeliveryOrderPackingDimension SelectPackingDimensinByJobID_PackingDimensionID(int jobID, int packingDimesionID)
        {
            return new Select().From(TblDeliveryOrderPackingDimension.Schema)
                .Where(TblDeliveryOrderPackingDimension.JobIDColumn).IsEqualTo(jobID)
                .And(TblDeliveryOrderPackingDimension.PackingDimensionIDColumn).IsEqualTo(packingDimesionID).ExecuteSingle<TblDeliveryOrderPackingDimension>();
        }

        public static TblDeliveryOrderPackingDimension InsertPackingDimension(TblDeliveryOrderPackingDimension doPacking)
        {
            return new TblDeliveryOrderPackingDimensionController().Insert(doPacking);
        }

        public static TblDeliveryOrderPackingDimension UpdatePackingDimension(TblDeliveryOrderPackingDimension doPacking)
        {
            return new TblDeliveryOrderPackingDimensionController().Update(doPacking);
        }

        public static void DeletePackingDimension(int JobID, int PackingDimensionID)
        {
            new Delete().From(TblDeliveryOrderPackingDimension.Schema)
                    .Where(TblDeliveryOrderPackingDimension.JobIDColumn).IsEqualTo(JobID)
                    .And(TblDeliveryOrderPackingDimension.PackingDimensionIDColumn).IsEqualTo(PackingDimensionID)
                    .ExecuteScalar();
        }

        public static TblDeliveryOrderPackingDimensionCollection SelectPackingDimensinByJobID_PackingDimensionIDCollection(int p)
        {
            return new Select().From(TblDeliveryOrderPackingDimension.Schema)
                .Where(TblDeliveryOrderPackingDimension.JobIDColumn).IsEqualTo(p)
                .ExecuteAsCollection<TblDeliveryOrderPackingDimensionCollection>();
        }

        public static void DeleteDeliveryOrderPackingDimension(TblDeliveryOrderPackingDimension item)
        {
            new TblDeliveryOrderPackingDimensionController().Delete(item.PackingDimensionID, item.JobID);
        }
        #endregion


        /// Trunglc Add 23-04-2015
        /// 


        #region DeliveryOrderLockStatus

        ///Trunglc Add - 23-04-2015

        public static bool IsNewDeliveryOrder(int JobID)
        {
            return ObjectLockingManager.IsNewObjectLocking(JobID, ObjectLockingType.DO);
        }

        public static bool IsDeliveryOrderLocking(int JobID)
        {
            return ObjectLockingManager.IsObjectLocking(JobID, ObjectLockingType.DO);
        }

        public static void LockOrUnlockDeliveryOrder(int JobID, bool IsLock)
        {
            ObjectLockingManager.LockOrUnlockObjectLocking(JobID, ObjectLockingType.DO, IsLock);
        }

        public static void LockJobAndOC(int JobID)
        {
            JobManager.LockOrUnLockJob(JobID, true);
            OrderConfirmationManager.LockOrUnlockOrderConfirmation(JobID, true);
        }

        #endregion
    }
}
