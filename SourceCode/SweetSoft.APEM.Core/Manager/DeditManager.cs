using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class DebitManager
    {
        /// <summary>
        /// Tạo Job number
        /// 4 ký tự đầu format MMdd
        /// Ký tự thứ 5 là '/'
        /// 5 ký tự cuối là số thứ tự của năm
        /// </summary>
        /// <returns></returns>
        public static string CreateDebitNumber()
        {
            string _No = "1" + DateTime.Today.ToString("yy") + "5";
            string _MaxNumber = new Select(Aggregate.Max(TblDebit.DebitNoColumn))
                .From(TblDebit.Schema)
                .Where(TblDebit.DebitNoColumn).Like(_No + "%")
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

        /// <summary>
        /// Check if Debit number exists
        /// </summary>
        /// <param name="DebitID"></param>
        /// <param name="DebitNumber"></param>
        /// <returns></returns>
        public static bool DebitNumberExists(int DebitID, string DebitNumber)
        {
            return new Select().From(TblDebit.Schema)
                    .Where(TblDebit.DebitNoColumn).IsEqualTo(DebitNumber)
                    .And(TblDebit.DebitIDColumn).IsNotEqualTo(DebitID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Check if Debit exists
        /// </summary>
        /// <param name="DebitID"></param>
        /// <returns></returns>
        public static bool Exists(int DebitID)
        {
            return new Select().From(TblDebit.Schema)
                    .Where(TblDebit.DebitIDColumn).IsEqualTo(DebitID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Select by ID
        /// </summary>
        /// <param name="DebitID"></param>
        /// <returns></returns>
        public static TblDebit SelectByID(int DebitID)
        {
            return new Select().From(TblDebit.Schema)
                            .Where(TblDebit.DebitIDColumn).IsEqualTo(DebitID)
                            .ExecuteSingle<TblDebit>();
        }

        /// <summary>
        /// Select all Debit
        /// </summary>
        /// <param name="DebitNo"></param>
        /// <param name="Customer"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable SelectAll(string DebitNo, string Customer, DateTime? FromDate, DateTime? ToDate, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblDebitSelectAll(DebitNo, Customer, FromDate, ToDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        /// <summary>
        /// Add new Debit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblDebit Insert(TblDebit obj)
        {
            return new TblDebitController().Insert(obj);
        }

        /// <summary>
        /// Update Debit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblDebit Update(TblDebit obj)
        {
            return new TblDebitController().Update(obj);
        }

        /// <summary>
        /// Delete Debit
        /// </summary>
        /// <param name="DebitID"></param>
        /// <returns></returns>
        public static bool Delete(int DebitID)
        {
            //Delete detail
            new Delete().From(TblDebitDetail.Schema).Where(TblDebitDetail.DebitIDColumn).IsEqualTo(DebitID).Execute();
            //Delete Debit
            return new TblDebitController().Delete(DebitID);
        }

        //DETAIL
        #region Detail
        public static List<TblDebitDetail> SellectAllDetail(int DebitID)
        {
            List<TblDebitDetail> list = new List<TblDebitDetail>();
            list = new Select().From(TblDebitDetail.Schema)
                            .Where(TblDebitDetail.DebitIDColumn).IsEqualTo(DebitID)
                            .ExecuteAsCollection<TblDebitDetailCollection>()
                            .ToList();
            return list;
        }

        public static TblDebitDetail InsertDetail(TblDebitDetail obj)
        {
            return new TblDebitDetailController().Insert(obj);
        }

        public static TblDebitDetail UpdateDetail(TblDebitDetail obj)
        {
            return new TblDebitDetailController().Update(obj);
        }

        public static bool DeleteDetail(int DebitDetailID)
        {
            TblDebitDetail obj = (new Select().From(TblDebitDetail.Schema).Where(TblDebitDetail.DebitDetailIDColumn).IsEqualTo(DebitDetailID).ExecuteSingle<TblDebitDetail>());
            if (obj != null)
            {
                return new TblDebitDetailController().Delete(DebitDetailID);
            }
            return false;
        }
        #endregion
    }
}
