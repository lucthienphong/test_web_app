using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace SweetSoft.APEM.Core.Manager
{
    public class InvoiceManager
    {

        public static TblInvoice Insert(TblInvoice obj)
        {
            return new TblInvoiceController().Insert(obj);
        }


        public static TblInvoice Update(TblInvoice obj)
        {
            return new TblInvoiceController().Update(obj);
        }

        public static bool Delete(int invoiceId)
        {
            new SubSonic.Delete().From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(invoiceId).Execute();
            return new TblInvoiceController().Delete(invoiceId);
        }

        public static TblInvoice SelectInvoiceByID_JobID(int InvoiceID, int JobID)
        {
            //return new Select().From(TblInvoice.Schema).Where
            return null;
        }

        public static TblInvoice SelectByID(int InvoiceID)
        {
            return new Select().From(TblInvoice.Schema).Where(TblInvoice.InvoiceIDColumn).IsEqualTo(InvoiceID).ExecuteSingle<TblInvoice>();
        }

        //Detail
        public static void InsertDetail(int invoiceId, int jobId)
        {
            //Update Job status
            TblJob obj = JobManager.SelectByID(jobId);
            if (obj != null)
            {
                obj.Status = JobStatus.Delivered.ToString();
                JobManager.Update(obj);
            }
            new TblInvoiceDetailController().Insert(invoiceId, jobId);
        }


        public static void UpdateDetail(int invoiceId, int jobId)
        {
            new TblInvoiceDetailController().Update(invoiceId, jobId);
        }

        public static TblInvoiceDetailCollection SelectInvoiceDetailByInvoiceId(int invoiceId)
        {
            return new SubSonic.Select().From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(invoiceId).ExecuteAsCollection<TblInvoiceDetailCollection>();
        }

        public static List<int> SelectListJobIDByInvoiceId(int invoiceId)
        {
            return new SubSonic.Select(TblInvoiceDetail.JobIDColumn).From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(invoiceId).ExecuteTypedList<int>();
        }

        public static List<int> SelectListJobIDFromInvoiceDetail()
        {
            return new SubSonic.Select(TblInvoiceDetail.JobIDColumn).From(TblInvoiceDetail.Schema).ExecuteTypedList<int>();
        }

        public static void DeleteDetailByInvoiceId(int invoiceId)
        {
            new SubSonic.Delete().From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(invoiceId).Execute();
        }

        public static void DeleteDetailByJobIdAndInvoiceId(int JobId, int invoiceId)
        {
            new SubSonic.Delete()
                .From(TblInvoiceDetail.Schema)
                .Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(invoiceId)
                .And(TblInvoiceDetail.JobIDColumn).IsEqualTo(JobId)
                .Execute();
        }

        public static DataTable InvoiceSelectAll(string Customer, string InvoiceNo, string Job, DateTime? FromDate, DateTime? ToDate, int? PageIndex, int? PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblInvoiceSelectAll(Customer, InvoiceNo, Job, FromDate, ToDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public static bool CheckExitJobInDetail(int jobID)
        {
            return new Select().From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.JobIDColumn).IsEqualTo(jobID).GetRecordCount() > 0 ? true : false;
        }

        public static bool CheckExitInvoiceNo(string invoiceNo)
        {
            return new Select().From(TblInvoice.Schema).Where(TblInvoice.InvoiceNoColumn).IsEqualTo(invoiceNo).GetRecordCount() > 0 ? true : false;
        }

        public static DataTable SelectForExport(string InvoiceIDs)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblInvoiceSelectForExport(InvoiceIDs).GetReader());
            return dt;
        }

        public static DataTable SelectDetailForExport(string InvoiceIDs)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblInvoiceSelectDetailForExport(InvoiceIDs).GetReader());
            return dt;
        }

        public static DataTable SelectInvoiceSummaryForPrint(int InvoiceID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblInvocieSelectSummary(InvoiceID).GetReader());
            return dt;
        }

        public static DataTable SelectInvoiceJobListForPrint(int InvoiceID)
        {
            DataTable dt = new DataTable();
            dt.Load(new Select(TblInvoiceDetail.JobIDColumn)
                        .From(TblInvoiceDetail.Schema)
                        .Where(TblInvoiceDetail.InvoiceIDColumn)
                        .IsEqualTo(InvoiceID)
                        .ExecuteReader());
            return dt;
        }

        public static DataTable SelectJobSummaryForPrint(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblInvoiceSelectJobSummary(JobID).GetReader());
            return dt;
        }

        public static DataTable SelectOtherChargesForPrint(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblInvoiceSelectJobOtherCharges(JobID).GetReader());
            return dt;
        }

        #region Trunglc

        // Trunglc Add 23-04-2015

        public static bool IsNewInvoice(int InvoiceID)
        {
            return ObjectLockingManager.IsNewObjectLocking(InvoiceID, ObjectLockingType.INVOICE);
        }

        public static bool IsInvoiceLocking(int InvoiceID)
        {
            return ObjectLockingManager.IsObjectLocking(InvoiceID, ObjectLockingType.INVOICE);
        }

        public static void LockOrUnLockInvoice(int InvoiceID, bool IsLock)
        {
            ObjectLockingManager.LockOrUnlockObjectLocking(InvoiceID, ObjectLockingType.INVOICE, IsLock);
        }

        public static void LockJobAndOCAndDO(int JobID)
        {
            JobManager.LockOrUnLockJob(JobID, true);
            OrderConfirmationManager.LockOrUnlockOrderConfirmation(JobID, true);
            //DeliveryOrderManager.LockOrUnlockDeliveryOrder(JobID, true);
        }

        #endregion
    }
}
