using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;
using System.Data;
using System.Web.Security;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SweetSoft.APEM.Core.Manager
{
    public enum SteelBase 
    { 
        New,
        Old
    }


    public class CylinderManager
    {
        public static string CreateCylinderNumber()
        {
            string _No = DateTime.Today.ToString("yyMM");
            string _MaxNumber = new Select(Aggregate.Max(TblPurchaseOrderCylinder.CylinderNoColumn))
                .From(TblPurchaseOrderCylinder.Schema)
                .Where(TblPurchaseOrderCylinder.CylinderNoColumn).Like(_No + "%")
                .ExecuteScalar<string>();
            _MaxNumber = _MaxNumber ?? "";
            if (_MaxNumber.Length > 0)
            {
                string _righ = (int.Parse(_MaxNumber.Substring(_MaxNumber.Length - 6, 6)) + 1).ToString();
                while (_righ.Length < 6)
                    _righ = "0" + _righ;
                _No += _righ;
            }
            else
                _No += "000001";
            return _No;
        }

        /// <summary>
        /// Get all job cylinder
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static DataTable SelectAll(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCylinderSelectAll(JobID).GetReader());
            return dt;
        }

        public static DataTable SelectCylinderSelectForOrderConfirmation(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCylinderSelectForOrderConfirmation(JobID).GetReader());
            return dt;
        }

        public static DataTable SelectCylinderSelectForDeliveryOrder(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCylinderSelectForDeliveryOrder(JobID).GetReader());
            return dt;
        }

        public static TblCylinderCollection SelectCylinderByJobID(int jobID)
        {
            return new Select()
                .From(TblCylinder.Schema)
                .Where(TblCylinder.JobIDColumn).IsEqualTo(jobID)
                .ExecuteAsCollection<TblCylinderCollection>();
        }

        public static TblCylinder SelectByID(int ID)
        {
            return new SubSonic.Select().From(TblCylinder.Schema).Where(TblCylinder.CylinderIDColumn).IsEqualTo(ID).ExecuteSingle<TblCylinder>();
        }

        /// <summary>
        /// Return collection of job cylinder
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static TblCylinderCollection SelectCollections(int JobID)
        {
            return new Select().From(TblCylinder.Schema).Where(TblCylinder.JobIDColumn).IsEqualTo(JobID).ExecuteAsCollection<TblCylinderCollection>();
        }

        /// <summary>
        /// Insert new value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblCylinder Insert(TblCylinder obj)
        {
            return new TblCylinderController().Insert(obj);
        }


        /// <summary>
        /// Update old value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblCylinder Update(TblCylinder obj)
        {
            return new TblCylinderController().Update(obj);
        }

        /// <summary>
        /// Delete old value
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Delete(int ID)
        {
            try
            {
                //Delete from tblPurchaseOrder
                PurchaseOrderManager.DeletePODetailByCylinderID(ID);
                //Delete from tblCylinder
                return new TblCylinderController().Delete(ID);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="IsOutsource">IsOutsource == false => Chỉ lấy giá trị new, ngược lại lấy tất cả các cylinder của job</param>
        /// <returns></returns>
        public static TblCylinderCollection GetCylindersByJobID(int JobID, bool IsOutsource)
        {
            TblCylinderCollection cylCollection = new TblCylinderCollection();
            SqlQuery query = new Select().From(TblCylinder.Schema)
                                .Where(TblCylinder.JobIDColumn)
                                .IsEqualTo(JobID);
            if (!IsOutsource)
                query = query.And(TblCylinder.SteelBaseColumn).IsEqualTo(1);
            cylCollection = query.ExecuteAsCollection<TblCylinderCollection>();
            return cylCollection;
        }

        public static DataTable SelectCylinderForDeliveryOrder(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCylinderSelectForDeliveryOrder(JobID).GetReader());
            return dt;
        }
    }
}
