using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class CreditManager
    {
        /// <summary>
        /// Tạo Job number
        /// 4 ký tự đầu format MMdd
        /// Ký tự thứ 5 là '/'
        /// 5 ký tự cuối là số thứ tự của năm
        /// </summary>
        /// <returns></returns>
        public static string CreateCreditNumber()
        {
            string _No = "1" + DateTime.Today.ToString("yy") + "4";
            string _MaxNumber = new Select(Aggregate.Max(TblCredit.CreditNoColumn))
                .From(TblCredit.Schema)
                .Where(TblCredit.CreditNoColumn).Like(_No + "%")
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
        /// Check if credit number exists
        /// </summary>
        /// <param name="CreditID"></param>
        /// <param name="CreditNumber"></param>
        /// <returns></returns>
        public static bool CreditNumberExists(int CreditID, string CreditNumber)
        {
            return new Select().From(TblCredit.Schema)
                    .Where(TblCredit.CreditNoColumn).IsEqualTo(CreditNumber)
                    .And(TblCredit.CreditIDColumn).IsNotEqualTo(CreditID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Check if credit exists
        /// </summary>
        /// <param name="CreditID"></param>
        /// <returns></returns>
        public static bool Exists(int CreditID)
        {
            return new Select().From(TblCredit.Schema)
                    .Where(TblCredit.CreditIDColumn).IsEqualTo(CreditID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Select by ID
        /// </summary>
        /// <param name="CreditID"></param>
        /// <returns></returns>
        public static TblCredit SelectByID(int CreditID)
        {
            return new Select().From(TblCredit.Schema)
                            .Where(TblCredit.CreditIDColumn).IsEqualTo(CreditID)
                            .ExecuteSingle<TblCredit>();
        }

        /// <summary>
        /// Select all credit
        /// </summary>
        /// <param name="CreditNo"></param>
        /// <param name="Customer"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable SelectAll(string CreditNo, string Customer, DateTime? FromDate, DateTime? ToDate, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCreditSelectAll(CreditNo, Customer, FromDate, ToDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        /// <summary>
        /// Add new credit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblCredit Insert(TblCredit obj)
        {
            return new TblCreditController().Insert(obj);
        }

        /// <summary>
        /// Update credit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblCredit Update(TblCredit obj)
        {
            return new TblCreditController().Update(obj);
        }

        /// <summary>
        /// Delete Credit
        /// </summary>
        /// <param name="CreditID"></param>
        /// <returns></returns>
        public static bool Delete(int CreditID)
        {
            //Delete detail
            new Delete().From(TblCreditDetail.Schema).Where(TblCreditDetail.CreditIDColumn).IsEqualTo(CreditID).Execute();
            //Delete Credit
            return new TblCreditController().Delete(CreditID);
        }

        //DETAIL
        #region Detail
        public static List<TblCreditDetail> SellectAllDetail(int CreditID)
        {
            List<TblCreditDetail> list = new List<TblCreditDetail>();
            list = new Select().From(TblCreditDetail.Schema)
                            .Where(TblCreditDetail.CreditIDColumn).IsEqualTo(CreditID)
                            .ExecuteAsCollection<TblCreditDetailCollection>()
                            .ToList();
            return list;
        }

        public static TblCreditDetail InsertDetail(TblCreditDetail obj)
        {
            return new TblCreditDetailController().Insert(obj);
        }

        public static TblCreditDetail UpdateDetail(TblCreditDetail obj)
        {
            return new TblCreditDetailController().Update(obj);
        }

        public static bool DeleteDetail(int CreditDetailID)
        {
            TblCreditDetail obj = (new Select().From(TblCreditDetail.Schema).Where(TblCreditDetail.CreditDetailIDColumn).IsEqualTo(CreditDetailID).ExecuteSingle<TblCreditDetail>());
            if (obj != null)
            {
                return new TblCreditDetailController().Delete(CreditDetailID);
            }
            return false;
        }
        #endregion
    }
}
