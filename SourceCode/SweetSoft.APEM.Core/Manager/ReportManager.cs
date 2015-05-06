using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class JobDOStatus
    {
        public JobDOStatus(int statusID, string statusName)
        {
            StatusID = statusID;
            StatusName = statusName;
        }
        public int StatusID { set; get; }
        public string StatusName { set; get; }
    }

    public class JobOCStatus
    {
        public JobOCStatus(int statusID, string statusName)
        {
            StatusID = statusID;
            StatusName = statusName;
        }
        public int StatusID { set; get; }
        public string StatusName { set; get; }
    }

    public class JobInvoiceStatus
    {
        public JobInvoiceStatus(int statusID, string statusName)
        {
            StatusID = statusID;
            StatusName = statusName;
        }
        public int StatusID { set; get; }
        public string StatusName { set; get; }
    }

    public class ReportManager
    {
        public static List<JobOCStatus> SelectJobOCStatusForDDL()
        {
            List<JobOCStatus> list = new List<JobOCStatus>();
            //All order
            JobOCStatus obj = new JobOCStatus(0, "All orders");
            list.Add(obj);
            //Order with no Invoice yet
            obj = new JobOCStatus(1, "Order with no OC yet");
            //Order with Invoice completed
            list.Add(obj);
            obj = new JobOCStatus(2, "Order with OC completed");
            list.Add(obj);
            return list;
        }

        public static List<JobDOStatus> SelectJobDOStatusForDDL()
        {
            List<JobDOStatus> list = new List<JobDOStatus>();
            //All order
            JobDOStatus obj = new JobDOStatus(0, "All orders");
            list.Add(obj);
            //Order with no Invoice yet
            obj = new JobDOStatus(1, "Order with no DO yet");
            //Order with Invoice completed
            list.Add(obj);
            obj = new JobDOStatus(2, "Order with DO completed");
            list.Add(obj);
            return list;
        }

        public static List<JobInvoiceStatus> SelectJobInvoiceStatusForDDL()
        {
            List<JobInvoiceStatus> list = new List<JobInvoiceStatus>();
            //All order
            JobInvoiceStatus obj = new JobInvoiceStatus(0, "All orders");
            list.Add(obj);
            //Order with no Invoice yet
            obj = new JobInvoiceStatus(1, "Order with no Invoice yet");
            //Order with Invoice completed
            list.Add(obj);
            obj = new JobInvoiceStatus(2, "Order with Invoice completed");
            list.Add(obj);
            return list;
        }

        /// <summary>
        /// Remake Report
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public static DataTable RemakeReport(DateTime? FromDate, DateTime? ToDate)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.ReportRemakeReport(FromDate, ToDate).GetReader());
            return dt;
        }

        /// <summary>
        /// Sales Report
        /// </summary>
        /// <param name="ProductTypeID">tblJob.ProductTypeID</param>
        /// <param name="SaleID">tblJob.SaleRepID</param>
        /// <param name="CustomerID">tblJob.CustomerID</param>
        /// <param name="Type">0 - All Order; 1 - No Invoice Yet; 2 - Invoice Completed</param>
        /// <param name="BaseCurrencyID">Base Currency from System Configuration</param>
        /// <param name="FromDate">tblJob.CreatedOn</param>
        /// <param name="ToDate">tblJob.CreatedOn</param>
        /// <returns></returns>
        public static DataTable SaleReport(int ProductTypeID, int SaleID, int CustomerID, int Type, int BaseCurrencyID, DateTime? FromDate, DateTime? ToDate, DateTime? FromDateInvoice, DateTime? ToDateInvoice, int? PageIndex, int? PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.ReportSalesReport(ProductTypeID, SaleID, CustomerID, Type, BaseCurrencyID, FromDate, ToDate, FromDateInvoice, ToDateInvoice, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }
    }
}
