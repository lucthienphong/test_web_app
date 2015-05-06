using SubSonic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.Core.Manager
{
    public enum OrderLockingType
    {
        Job,
        OC,
        DO,
        Invoice
    }

    public class OrderLockingManager
    {
        /// <summary>
        /// Kiểm tra Job/DO/OC/Invoice có bị khóa không?
        /// </summary>
        /// <param name="ID">Job/DO/OC/Invoice ID</param>
        /// <param name="type">Loại order</param>
        /// <returns></returns>
        public static bool CheckLockingStatus(int ID, OrderLockingType Type)
        {
            return new Select().From(TblOrderLocking.Schema)
                            .Where(TblOrderLocking.IdColumn).IsEqualTo(ID)
                            .And(TblOrderLocking.TypeColumn).IsEqualTo(Type.ToString())
                            .GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Lock order
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type"></param>
        public static void LockOrder(int ID, OrderLockingType Type)
        {
            if (!CheckLockingStatus(ID, Type))
                new TblOrderLockingController().Insert(ID, Type.ToString());
        }

        public static void LockJob(int ID)
        {
            LockOrder(ID, OrderLockingType.Job);
        }

        public static void LockOC(int ID)
        {
            LockJob(ID);
            LockOrder(ID, OrderLockingType.OC);
        }

        public static void LockDO(int ID)
        {
            LockOC(ID);
            LockOrder(ID, OrderLockingType.DO);
        }

        public static void LockInvoice(int ID)
        {
            TblInvoiceDetailCollection list = new Select().From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(ID).ExecuteAsCollection<TblInvoiceDetailCollection>();
            foreach (var item in list)
            {
                LockDO(item.JobID);
            }
            LockOrder(ID, OrderLockingType.Invoice);
        }

        /// <summary>
        /// Bỏ khóa Job/OC/DO/Invoice
        /// </summary>
        /// <param name="ID">Job/OC/DO/Invoice ID</param>
        /// <param name="Type">Job/OC/DO/Invoice</param>
        public static void UnlockOrder(int ID, OrderLockingType Type)
        {
            if (CheckLockingStatus(ID, Type))
                new TblOrderLockingController().Delete(ID, Type.ToString());
        }

        public static void UnlockJob(int ID)
        {
            UnlockOrder(ID, OrderLockingType.Job);
        }

        public static void UnlockOC(int ID)
        {
            UnlockOrder(ID, OrderLockingType.OC);
        }

        public static void UnlockDO(int ID)
        {
            UnlockOrder(ID, OrderLockingType.DO);
        }

        public static void UnlockInvoice(int ID)
        {
            UnlockOrder(ID, OrderLockingType.Invoice);
        }
    }
}
