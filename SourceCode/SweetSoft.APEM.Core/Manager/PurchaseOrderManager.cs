using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

namespace SweetSoft.APEM.Core.Manager
{
    public class PurchaseOrderManager
    {
        #region Purchase Order

        public static TblPurchaseOrder InsertPurchaseOrder(TblPurchaseOrder purOrder)
        {
            return new TblPurchaseOrderController().Insert(purOrder);
        }

        public static TblPurchaseOrder SelectPurchaseOrder(string purID)
        {
            return new Select().From(TblPurchaseOrder.Schema).Where(TblPurchaseOrder.PurchaseOrderIDColumn).IsEqualTo(purID).ExecuteSingle<TblPurchaseOrder>();
        }

        public static object UpdatePurchaseOrder(TblPurchaseOrder purOrder)
        {
            return new TblPurchaseOrderController().Update(purOrder);
        }

        public static bool DeletePurchaseOrder(int p)
        {
            return new TblPurchaseOrderController().Delete(p);
        }

        public static TblPurchaseOrder SelectPurchaseOrderByOrderNumber(string p)
        {
            return new Select().From(TblPurchaseOrder.Schema).Where(TblPurchaseOrder.OrderNumberColumn).IsEqualTo(p).ExecuteSingle<TblPurchaseOrder>();
        }

        #endregion Purchase Order

        #region Purchase Order_Cylinder

        public static TblPurchaseOrderCylinder SelectPurchaseOrderByJobId_PurchaseID(int cyID, int purID)
        {
            return new Select().From(TblPurchaseOrderCylinder.Schema).Where(TblPurchaseOrderCylinder.PurchaseOrderIDColumn).IsEqualTo(purID)
                .And(TblPurchaseOrderCylinder.CylinderIDColumn).IsEqualTo(cyID).ExecuteSingle<TblPurchaseOrderCylinder>();
        }

        public static TblPurchaseOrderCylinder InsertPurchase_CylinderOrder(TblPurchaseOrderCylinder pc)
        {
            return new TblPurchaseOrderCylinderController().Insert(pc);
        }

        public static TblPurchaseOrderCylinder UpdatePurchase_Cylinder(TblPurchaseOrderCylinder pc)
        {
            return new TblPurchaseOrderCylinderController().Update(pc);
        }

        public static bool DeletePurchaseOrderCylinder(TblPurchaseOrderCylinder purOderCyl)
        {
            return new TblPurchaseOrderCylinderController().Delete(purOderCyl.Id);
        }

        /// <summary>
        /// Delete all detail of PO
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool DeleteAllPurchaseOrderCylinder(int PurchaseOrderID)
        {
            try
            {
                //Lấy thông tin các cylinder trong PO
                SqlQuery query = new Select(TblPurchaseOrderCylinder.CylinderIDColumn)
                                        .From(TblPurchaseOrderCylinder.Schema)
                                        .Where(TblPurchaseOrderCylinder.PurchaseOrderIDColumn).IsEqualTo(PurchaseOrderID);
                TblCylinderCollection cylColl = new Select().From(TblCylinder.Schema)
                                                    .Where(TblCylinder.CylinderIDColumn).In(query)
                                                    .ExecuteAsCollection<TblCylinderCollection>();
                foreach (TblCylinder cObj in cylColl)
                {
                    cObj.CylinderNo = string.Empty;
                    CylinderManager.Update(cObj);
                }

                new Delete().From(TblPurchaseOrderCylinder.Schema).Where(TblPurchaseOrderCylinder.PurchaseOrderIDColumn).IsEqualTo(PurchaseOrderID).Execute();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Dùng để xóa Detail khi xóa 1 cylinder trong job
        /// </summary>
        /// <param name="CylinderID"></param>
        /// <returns></returns>
        public static void DeletePODetailByCylinderID(int CylinderID)
        {
            TblPurchaseOrderCylinder dObj = new Select().From(TblPurchaseOrderCylinder.Schema)
                                                .Where(TblPurchaseOrderCylinder.CylinderIDColumn).IsEqualTo(CylinderID)
                                                .ExecuteSingle<TblPurchaseOrderCylinder>();
            if (dObj != null)
            {
                new TblPurchaseOrderCylinderController().Delete(dObj.Id);
            }
        }

        public static TblPurchaseOrderCylinderCollection SelectPurchaseCylinderOrderByPurchaseID(int p)
        {
            return new Select().From(TblPurchaseOrderCylinder.Schema).Where(TblPurchaseOrderCylinder.PurchaseOrderIDColumn).IsEqualTo(p).ExecuteAsCollection<TblPurchaseOrderCylinderCollection>();
        }

        public static DataTable GetPurchaseOrdersBy(string Customer, string JobNumber, string PurchaseOrder, DateTime? FromDate, DateTime? ToDate, int? PageIndex, int? PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.SearchPurchaseOrderByCustomer(Customer, JobNumber, PurchaseOrder, FromDate, ToDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        #endregion Purchase Order_Cylinder

        public static TblPurchaseOrderCollection SelectPurchaseOrderByJobID(int JobID)
        {
            return new Select().From(TblPurchaseOrder.Schema).Where(TblPurchaseOrder.JobIDColumn).IsEqualTo(JobID).ExecuteAsCollection<TblPurchaseOrderCollection>();
        }
    }    
}
