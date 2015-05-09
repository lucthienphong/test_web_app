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
using System.ComponentModel;

namespace SweetSoft.APEM.Core.Manager
{
    public enum TypeOfCylinder
    {
        Shaftless,
        Shaft
    }

    public enum JobPrinting
    {
        Surface,
        Reverse
    }

    public enum JobStatus
    {
        [Description("Confirmed/Active")]
        Actived,
        [Description("Approved")]
        Approved,
        [Description("On Hold")]
        OnHold,
        [Description("Canceled (No Invoice)")]
        Canceled,
        [Description("Delivered / Invoiced")]
        Delivered
    }

    /**
     * Trunglc Add 22-04-2015
     */

    public enum JobLockStatus
    {
        [Description("Lock")]
        Lock,
        [Description("Unlock")]
        Unlock,
    }

    public enum JobInternalExternal
    {
        Internal,
        External
    }

    public enum TypeOfOrder
    {
        [Description("New")]
        New,
        [Description("Redo")]
        Redo,
        [Description("Revision")]
        Revision,
        [Description("Barrel Proof")]
        BarrelProof
    }

    public enum EngravingProtocol
    {
        None,
        EMG,
        DLS,
        Etching,
        Matching,
        CNC,
        Digilas
    }

    public class JobExtension : TblJob
    {
        //Phần bổ sung
        public TblJob Parent { get; set; }
        //Chuyển Parents -> Children
        public JobExtension() { }
        public JobExtension(TblJob parent)
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
        public string CustomerName { set; get; }
        public string CustomerCode { set; get; }
        public string BrandOwnerName { set; get; }
        public string BrandOwnerCode { set; get; }
        public string ShipToPartyName { set; get; }
        public string ShipToPartyCode { set; get; }
        public string ShipToPartyAddress { set; get; }
        public string RootJobNumber { set; get; }
    }

    public class ServiceJobDetailExtension : TblServiceJobDetail
    {
        //Phần bổ sung
        public TblServiceJobDetail Parent { get; set; }
        //Chuyển Parents -> Children
        public ServiceJobDetailExtension() { }
        public ServiceJobDetailExtension(TblServiceJobDetail parent)
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
        public int No { set; get; }
        public string PricingName { set; get; }
        public string CategoryName { set; get; }
    }

    public class JobManager
    {
        /// <summary>
        /// Check if job has PO, OC, DO,...
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool IsBeingUsed(int ID)
        {
            string JobNumber = new Select(TblJob.JobNumberColumn).From(TblJob.Schema).Where(TblJob.JobIDColumn).IsEqualTo(ID).ExecuteScalar<string>();
            bool HasRevision = new Select().From(TblJob.Schema).Where(TblJob.JobNumberColumn).IsEqualTo(JobNumber).And(TblJob.JobIDColumn).IsNotEqualTo(ID).GetRecordCount() > 0 ? true : false;
            bool HasChildren = new Select().From(TblJob.Schema).Where(TblJob.RootJobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
            bool HasPurchaseOrder = new Select().From(TblPurchaseOrder.Schema).Where(TblPurchaseOrder.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
            bool HasOrderConfirmation = new Select().From(TblOrderConfirmation.Schema).Where(TblOrderConfirmation.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
            bool HasDeliveryOrder = new Select().From(TblDeliveryOrder.Schema).Where(TblDeliveryOrder.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
            bool HasInvoice = new Select().From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
            //bool HasEngraving = new Select().From(TblEngraving.Schema).Where(TblEngraving.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
            if (!HasPurchaseOrder && !HasChildren && !HasOrderConfirmation && !HasDeliveryOrder && !HasInvoice)
            {
                return false;
            }
            else return true;
        }

        public static double PiNumber()
        {
            double pi = 0;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PiValueSetting);
            if (setting != null)
            {
                if (!double.TryParse(setting.SettingValue, out pi))
                    pi = 3.1416;
            }
            else
                pi = 3.1416;
            return pi;
        }

        /// <summary>
        /// Return OC Number By JobID
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static string JobHasOC(int JobID)
        {
            string OCNumber = string.Empty;
            OCNumber = new Select(TblOrderConfirmation.OCNumberColumn).From(TblOrderConfirmation.Schema)
                                .Where(TblOrderConfirmation.JobIDColumn).IsEqualTo(JobID)
                                .ExecuteScalar<string>();
            if (string.IsNullOrEmpty(OCNumber))
                OCNumber = string.Empty;
            return OCNumber;
        }

        /// <summary>
        /// Return DO Number By JobID
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static string JobHasDO(int JobID)
        {
            string DONumber = string.Empty;
            DONumber = new Select(TblDeliveryOrder.DONumberColumn).From(TblDeliveryOrder.Schema)
                                .Where(TblDeliveryOrder.JobIDColumn).IsEqualTo(JobID)
                                .ExecuteScalar<string>();
            if (string.IsNullOrEmpty(DONumber))
                DONumber = string.Empty;
            return DONumber;
        }

        /// <summary>
        /// Return Invoice Number By JobID
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static string JobHasInvoice(int JobID)
        {
            string InvoiceNumber = string.Empty;
            TblInvoiceDetail dObj = new Select().From(TblInvoiceDetail.Schema)
                                        .Where(TblInvoiceDetail.JobIDColumn).IsEqualTo(JobID)
                                        .ExecuteSingle<TblInvoiceDetail>();
            if (dObj != null)
            {
                TblInvoice obj = InvoiceManager.SelectByID(dObj.InvoiceID);
                InvoiceNumber = obj != null ? obj.InvoiceNo : string.Empty;
            }
            return InvoiceNumber;
        }

        public static string IsBeingUsedFor(int ID)
        {
            string Result = string.Empty;
            TblJob obj = SelectByID(ID); ;
            string HasRevision = new Select().From(TblJob.Schema)
                                    .Where(TblJob.JobNumberColumn).IsEqualTo(obj.JobNumber)
                                    .And(TblJob.JobIDColumn).IsNotEqualTo(ID)
                                    .And(TblJob.RevNumberColumn).IsGreaterThan(obj.RevNumber)
                                    .GetRecordCount() > 0 ? "<p style='text-align:left; margin-bottom:0;'>- This job has made a <strong>Revision</strong></p>" : string.Empty;
            if (!string.IsNullOrEmpty(HasRevision))
                Result += HasRevision;
            string HasChildren = new Select().From(TblJob.Schema).Where(TblJob.RootJobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? "<p style='text-align:left; margin-bottom:0;'>- This job is root job of another job" : string.Empty;
            if (!string.IsNullOrEmpty(HasChildren))
                Result += HasChildren;
            string HasPurchaseOrder = new Select().From(TblPurchaseOrder.Schema).Where(TblPurchaseOrder.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? "<p style='text-align:left; margin-bottom:0;'>- There exists a <strong>Purchase Order</strong> has been created for this job</p>" : string.Empty;
            if (!string.IsNullOrEmpty(HasPurchaseOrder))
                Result += HasPurchaseOrder;
            string HasOrderConfirmation = new Select().From(TblOrderConfirmation.Schema).Where(TblOrderConfirmation.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? "<p style='text-align:left; margin-bottom:0;'>- There is a <strong>Order Confirmation</strong> has been created for this job</p>" : string.Empty;
            if (!string.IsNullOrEmpty(HasOrderConfirmation))
                Result += HasOrderConfirmation;
            string HasDeliveryOrder = new Select().From(TblDeliveryOrder.Schema).Where(TblDeliveryOrder.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? "<p style='text-align:left; margin-bottom:0;'>- There is a <strong>Delivery Order</strong> has been created for this job</p>" : string.Empty;
            if (!string.IsNullOrEmpty(HasDeliveryOrder))
                Result += HasDeliveryOrder;
            string HasInvoice = new Select().From(TblInvoiceDetail.Schema).Where(TblInvoiceDetail.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? "<p style='text-align:left; margin-bottom:0;'>- There is a <strong>Invoice</strong> has been created for this job</p>" : string.Empty;
            if (!string.IsNullOrEmpty(HasInvoice))
                Result += HasInvoice;
            //string HasEngraving = new Select().From(TblEngraving.Schema).Where(TblEngraving.JobIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? "<p style='text-align:left; margin-bottom:0;'>- There is an <strong>Engraving Infomation</strong> has been created for this job</p>" : string.Empty;
            //if (!string.IsNullOrEmpty(HasInvoice))
            //    Result += HasInvoice;
            return Result;
        }

        /// <summary>
        /// Tạo Job number
        /// 4 ký tự đầu format MMdd
        /// Ký tự thứ 5 là '/'
        /// 5 ký tự cuối là số thứ tự của năm
        /// </summary>
        /// <returns></returns>
        public static string CreateJobNumber()
        {
            string _No = DateTime.Today.ToString("yyMM/");
            string _MaxNumber = new Select(Aggregate.Max(TblJob.JobNumberColumn))
                .From(TblJob.Schema)
                //.Where(TblJob.JobNumberColumn).Like(_No + "%")
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
        /// Create job barcode
        /// </summary>
        /// <returns></returns>
        public static string CreateJobBarcode()
        {
            string _No = "";
            StoredProcedure s = SPs.TblJobCreateJobBarcode(_No);
            s.Execute();
            foreach (object objOutput in s.OutputValues)
            {
                _No = objOutput.ToString();
            }
            return _No;
        }

        /// <summary>
        /// Check if job number exists
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="JobNumber"></param>
        /// <returns></returns>
        public static bool JobNumberExists(int JobID, string JobNumber)
        {
            return new Select().From(TblJob.Schema)
                    .Where(TblJob.JobNumberColumn).IsEqualTo(JobNumber)
                    .And(TblJob.JobIDColumn).IsNotEqualTo(JobID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Check if job barcode exists
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="JobBarcode"></param>
        /// <returns></returns>
        public static bool JobBarcodeExists(int JobID, string JobBarcode)
        {
            return new Select().From(TblJob.Schema)
                    .Where(TblJob.JobBarcodeColumn).IsEqualTo(JobBarcode)
                    .And(TblJob.JobIDColumn).IsNotEqualTo(JobID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Get Revision number
        /// </summary>
        /// <param name="JobNumber"></param>
        /// <returns></returns>
        public static int JobRevisionNumber(string JobNumber)
        {
            int MaxNumber = new Select(Aggregate.Max(TblJob.RevNumberColumn))
                .From(TblJob.Schema)
                .Where(TblJob.JobNumberColumn).IsEqualTo(JobNumber)
                .ExecuteScalar<int>();
            return MaxNumber + 1;
        }

        /// <summary>
        /// Get Root Job Rev Number
        /// </summary>
        /// <param name="RootJobID"></param>
        /// <returns></returns>
        public static int JobRootRevNumber(int RootJobID)
        {
            int MaxNumber = new Select(Aggregate.Max(TblJob.RootJobRevNumberColumn))
                .From(TblJob.Schema)
                .Where(TblJob.RootJobIDColumn).IsEqualTo(RootJobID)
                .ExecuteScalar<int>();
            return MaxNumber + 1;
        }

        /// <summary>
        /// Select Job information by ID
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static JobExtension SelectByID(int JobID)
        {
            TblJob jObj = new Select().From(TblJob.Schema)
                            .Where(TblJob.JobIDColumn).IsEqualTo(JobID)
                            .ExecuteSingle<TblJob>();
            JobExtension obj = new JobExtension(jObj);
            if (jObj != null)
            {
                //Customer
                TblCustomer cObj = CustomerManager.SelectByID(jObj.CustomerID);
                obj.CustomerCode = cObj.Code;
                obj.CustomerName = cObj.Name;
                //BrandOwner
                TblCustomer boObj = CustomerManager.SelectByID((jObj.BrandOwner ?? 0));
                obj.BrandOwnerCode = boObj == null ? string.Empty : boObj.Code;
                obj.BrandOwnerName = boObj == null ? string.Empty : boObj.Name;
                //ShipToParty
                TblCustomer sObj = CustomerManager.SelectByID((jObj.ShipToParty ?? 0));
                obj.ShipToPartyCode = sObj == null ? string.Empty : sObj.Code;
                obj.ShipToPartyName = sObj == null ? string.Empty : sObj.Name;
                obj.ShipToPartyAddress = sObj == null ? string.Empty : sObj.Address;
                //RootJob
                obj.RootJobNumber = new Select(TblJob.JobNumberColumn)
                                            .From(TblJob.Schema).Where(TblJob.JobIDColumn)
                                            .IsEqualTo(jObj.RootJobID).ExecuteScalar<string>();
            }
            else
                obj = null;
            return obj;
        }

        /// <summary>
        /// Select job sheet by ID
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static TblJobSheet SelectJobSheetByID(int JobID)
        {
            return new Select().From(TblJobSheet.Schema).Where(TblJobSheet.JobIDColumn).IsEqualTo(JobID).ExecuteSingle<TblJobSheet>();
        }

        /// <summary>
        /// Get Revision History
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static DataTable SelectRevionHistoryByID(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblJobSelectRevisionHistory(JobID).GetReader());
            return dt;
        }

        /// <summary>
        /// Add new job
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblJob Insert(TblJob obj)
        {
            return new TblJobController().Insert(obj);
        }

        /// <summary>
        /// Add new Job sheet
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblJobSheet InsertJobSheet(TblJobSheet obj)
        {
            return new TblJobSheetController().Insert(obj);
        }

        /// <summary>
        /// Update Job
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblJob Update(TblJob obj)
        {
            return new TblJobController().Update(obj);
        }

        /// <summary>
        /// Update Job sheet
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblJobSheet UpdateJobSheet(TblJobSheet obj)
        {
            return new TblJobSheetController().Update(obj);
        }

        /// <summary>
        /// Delete Job
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static bool Delete(int JobID)
        {
            //Get Job infomation
            TblJob obj = SelectByID(JobID);

            //Delete JobQuotation
            //new Delete().From(TblJobQuotation.Schema).Where(TblJobQuotation.JobIDColumn).IsEqualTo(JobID).Execute();
            //new Delete().From(TblJobQuotationPricing.Schema).Where(TblJobQuotationPricing.JobIDColumn).IsEqualTo(JobID).Execute();

            //Delete PurchaseOrder
            TblPurchaseOrderCollection poList = new Select().From(TblPurchaseOrder.Schema).Where(TblPurchaseOrder.JobIDColumn).IsEqualTo(JobID).ExecuteAsCollection<TblPurchaseOrderCollection>();
            foreach (TblPurchaseOrder p in poList)
            {
                new Delete().From(TblPurchaseOrderCylinder.Schema).Where(TblPurchaseOrderCylinder.PurchaseOrderIDColumn).IsEqualTo(p.PurchaseOrderID).Execute();
                new Delete().From(TblPurchaseOrder.Schema).Where(TblPurchaseOrder.JobIDColumn).IsEqualTo(JobID).Execute();
            }

            //Delete ServiceJob
            new Delete().From(TblServiceJobDetail.Schema).Where(TblServiceJobDetail.JobIDColumn).IsEqualTo(JobID).Execute();

            //Delete DeliveryOrder
            new Delete().From(TblDeliveryOrder.Schema).Where(TblDeliveryOrder.JobIDColumn).IsEqualTo(JobID).Execute();

            //Delete OrderConfirmation
            new Delete().From(TblOrderConfirmation.Schema).Where(TblOrderConfirmation.JobIDColumn).IsEqualTo(JobID).Execute();

            //Delete Engraving
            new Delete().From(TblEngravingDetail.Schema).Where(TblEngravingDetail.JobIDColumn).IsEqualTo(JobID).Execute();
            new Delete().From(TblEngraving.Schema).Where(TblEngraving.JobIDColumn).IsEqualTo(JobID).Execute();

            //Delete Progress
            new Delete().From(TblProgress.Schema).Where(TblProgress.JobIDColumn).IsEqualTo(JobID).Execute();

            //Delete Jobsheet, Cylinder, OtherCharges
            new Delete().From(TblJobSheet.Schema).Where(TblJobSheet.JobIDColumn).IsEqualTo(JobID).Execute();
            new Delete().From(TblCylinder.Schema).Where(TblCylinder.JobIDColumn).IsEqualTo(JobID).Execute();
            new Delete().From(TblServiceJobDetail.Schema).Where(TblServiceJobDetail.JobIDColumn).IsEqualTo(JobID).Execute();
            new Delete().From(TblOtherCharge.Schema).Where(TblOtherCharge.JobIDColumn).IsEqualTo(JobID).Execute();

            //Delete Processing
            new Delete().From(TblJobProcess.Schema).Where(TblJobProcess.JobIDColumn).IsEqualTo(JobID).Execute();

            //Update closed status for early job
            TblJob prevJob = new Select().Top("1").From(TblJob.Schema)
                                .Where(TblJob.JobNumberColumn).IsEqualTo(obj.JobNumber)
                                .And(TblJob.RevNumberColumn).IsLessThan(obj.RevNumber)
                                .OrderDesc(TblJob.Columns.RevNumber).ExecuteSingle<TblJob>();
            if (prevJob != null)
            {
                prevJob.IsClosed = 0;
                Update(prevJob);
            }
            //Delete Job
            return new TblJobController().Delete(JobID);
        }

        /// <summary>
        /// Delete Job sheet
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static bool DeleteJobSheet(int JobID)
        {
            return new TblJobSheetController().Delete(JobID);
        }

        /// <summary>
        /// Return Job Internal/External
        /// </summary>
        /// <returns></returns>
        public static List<JobInternalExternal> SelectJobInternalExternalForDDL()
        {
            List<JobInternalExternal> list = new List<JobInternalExternal>();
            list = Enum.GetValues(typeof(JobInternalExternal)).Cast<JobInternalExternal>().ToList();
            return list;
        }

        /// <summary>
        /// Return Engraving Protocol list
        /// </summary>
        /// <returns></returns>
        public static List<EngravingProtocol> SelectEngravingProtocolForDDL()
        {
            List<EngravingProtocol> list = new List<EngravingProtocol>();
            list = Enum.GetValues(typeof(EngravingProtocol)).Cast<EngravingProtocol>().ToList();
            return list;
        }

        /// <summary>
        /// Return Type of Order
        /// </summary>
        /// <returns></returns>
        public static List<TypeOfOrder> SelectTypeOfOrderForDDL()
        {
            List<TypeOfOrder> list = new List<TypeOfOrder>();
            list = Enum.GetValues(typeof(TypeOfOrder)).Cast<TypeOfOrder>().ToList();
            return list;
        }

        /// <summary>
        /// Return job status
        /// </summary>
        /// <returns></returns>
        public static List<JobStatus> SelectJobStatusForDDL()
        {
            List<JobStatus> list = new List<JobStatus>();
            list = Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>().ToList();
            return list;
        }

        /// <summary>
        /// Return Jobsheet Type of Cylinder
        /// </summary>
        /// <returns></returns>
        public static List<JobPrinting> SelectPrintingForDDL()
        {
            List<JobPrinting> list = new List<JobPrinting>();
            list = Enum.GetValues(typeof(JobPrinting)).Cast<JobPrinting>().ToList();
            return list;
        }

        /// <summary>
        /// Return Jobsheet Printing
        /// </summary>
        /// <returns></returns>
        public static List<TypeOfCylinder> SelectTypeOfCylinderForDDL()
        {
            List<TypeOfCylinder> list = new List<TypeOfCylinder>();
            list = Enum.GetValues(typeof(TypeOfCylinder)).Cast<TypeOfCylinder>().ToList();
            return list;
        }


        /// <summary>
        /// Get all rev number by each job
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static DataTable SelectRevNumberForDDL(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(int));
            if (JobID == 0)
            {
                dt.Rows.Add(0, 0);
            }
            else
            {
                DataTable currentList = new DataTable();
                currentList.Load(SPs.TblJobSelectRevisionHistory(JobID).GetReader());
                foreach (DataRow r in currentList.Rows)
                {
                    dt.Rows.Add((int)r["JobID"], (int)r["RevNumber"]);
                }
            }
            return dt;
        }

        /// <summary>
        /// Close all old job and get closed job id
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static int CloseOldJob(int JobID)
        {
            int ClosedID = 0;
            DataTable currentList = new DataTable();
            currentList.Load(SPs.TblJobSelectRevisionHistory(JobID).GetReader());
            foreach (DataRow r in currentList.Rows)
            {
                int ID = (int)r["JobID"];
                bool IsClosed = Convert.ToBoolean(r["IsClosed"]);
                if (!IsClosed)
                {
                    new Update(TblJob.Schema).Set(TblJob.IsClosedColumn).EqualTo(1).Where(TblJob.JobIDColumn).IsEqualTo(ID).Execute();
                    ClosedID = ID;
                }
            }
            return ClosedID;
        }

        /// <summary>
        /// Load all job by conditions
        /// </summary>
        /// <param name="Customer"></param>
        /// <param name="JobBarcode"></param>
        /// <param name="JobNumber"></param>
        /// <param name="JobInfo"></param>
        /// <param name="SaleRepID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable SelectAll(string Customer, string JobBarcode, string JobNumber, string JobInfo, string CusCylID, int SaleRepID, DateTime? FromDate, DateTime? ToDate, int OCStatusID, int DOStatusID, int InvoiceStatusID, bool IsServiceJob, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblJobSelectAll(Customer, JobBarcode, JobNumber, JobInfo, CusCylID, SaleRepID, FromDate, ToDate, OCStatusID, DOStatusID, InvoiceStatusID, IsServiceJob, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public static List<string> SelectJobNumberByCustomerID(int custID, bool SelectServiceJob, bool HasNoOC, bool IsActived = false)
        {
            SqlQuery select = new Select();
            select = select.From(TblJob.Schema);
            select = select.Where(TblJob.Columns.CustomerID).IsEqualTo(custID);
            if (!SelectServiceJob)
                select = select.And(TblJob.IsServiceJobColumn).IsEqualTo(0);
            if (HasNoOC)
            {
                SqlQuery JobHasOC = new Select(TblOrderConfirmation.JobIDColumn).From(TblOrderConfirmation.Schema)
                                                            .InnerJoin(TblJob.JobIDColumn, TblOrderConfirmation.JobIDColumn)
                                                            .Where(TblJob.CustomerIDColumn).IsEqualTo(custID);
                select = select.And(TblJob.JobIDColumn).NotIn(JobHasOC);
            }

            if (IsActived)
            {
                select = select.And(TblJob.StatusColumn).IsEqualTo("Actived");
            }

            var jobs = select.ExecuteTypedList<TblJob>();

            var _jobs = jobs.GroupBy(q => q.JobNumber, (key, g) => new { JobNumber = key });

            List<string> jobString = new List<string>();

            if (_jobs.Count() == 0)
                jobString.Add("N/A");

            foreach (var item in _jobs)
            {
                jobString.Add(item.JobNumber);
            }
            return jobString;
        }

        public static Dictionary<string, string> SelectRevNumberByJobNumber(string JobNumber, byte Type, bool IsActived = false)
        {
            SqlQuery select = new Select();
            SqlQuery exists = new SqlQuery();
            //Type == 1 -- Order confirmation
            if (Type == 0)
                exists = new Select(TblPurchaseOrder.JobIDColumn).From(TblPurchaseOrder.Schema)
                                    .Where(TblPurchaseOrder.JobIDColumn).IsEqualTo(0);
            else if (Type == 1)
                exists = new Select(TblJob.JobIDColumn).From(TblOrderConfirmation.Schema)
                                    .InnerJoin(TblJob.JobIDColumn, TblOrderConfirmation.JobIDColumn)
                                    .Where(TblJob.JobNumberColumn).IsEqualTo(JobNumber);
            select = select.From(TblJob.Schema);
            select = select.Where(TblJob.Columns.JobNumber).IsEqualTo(JobNumber);
            select = select.And(TblJob.JobIDColumn).NotIn(exists);
            if (IsActived)
            {
                select = select.And(TblJob.StatusColumn).IsEqualTo("Actived");
            }
            select = select.OrderDesc(TblJob.Columns.RevNumber);

            var jobs = select.ExecuteTypedList<TblJob>();

            Dictionary<string, string> jobList = new Dictionary<string, string>();
            if (jobs.Count == 0)
                jobList.Add("0", "N/A");
            foreach (var item in jobs)
            {
                jobList.Add(item.JobID.ToString(), item.RevNumber.ToString());
            }
            return jobList;
        }

        public static Dictionary<string, int> SelectRevNumberByJobNumberForInvoice(string JobNumber, List<int> ExJobIDs, List<int> JobIDs)
        {
            Select select = new Select();
            select.From(TblJob.Schema);
            select.Where(TblJob.Columns.JobNumber).IsEqualTo(JobNumber);
            if (ExJobIDs != null && ExJobIDs.Count > 0)
                select.And(TblJob.JobIDColumn).NotIn(ExJobIDs);
            select.And(TblJob.Columns.JobID).In(JobIDs);

            var jobs = select.ExecuteTypedList<TblJob>();
            Dictionary<string, int> jobList = new Dictionary<string, int>();

            foreach (var item in jobs)
            {
                jobList.Add(item.JobID.ToString(), item.RevNumber);
            }
            return jobList;
        }

        /// <summary>
        /// Select all for report
        /// </summary>
        /// <param name="Customer"></param>
        /// <param name="JobBarcode"></param>
        /// <param name="JobNumber"></param>
        /// <param name="JobInfo"></param>
        /// <param name="SaleRepID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable SelectForReport(string Customer, string JobBarcode, string JobNumber, string JobInfo, int SaleRepID, DateTime? FromDate, DateTime? ToDate, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblJobSelectForReport(Customer, JobBarcode, JobNumber, JobInfo, SaleRepID, FromDate, ToDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public static TblJobCollection SelectJobByCustID(int custID)
        {
            return new Select().From(TblJob.Schema).Where(TblJob.CustomerIDColumn).IsEqualTo(custID).ExecuteAsCollection<TblJobCollection>();
        }

        public static TblJobCollection SelectJobByInvoiceID(int InvoiceID)
        {
            return new Select().From(TblJob.Schema)
                        .InnerJoin(TblInvoiceDetail.JobIDColumn, TblJob.JobIDColumn)
                        .Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(InvoiceID)
                        .ExecuteAsCollection<TblJobCollection>();
        }

        public static List<TblJob> SelectJobByKeyWord(string Keyword)
        {
            Keyword = "%" + Keyword + "%";
            List<TblJob> list = new List<TblJob>();
            var result = new Select().From(TblJob.Schema)
                    .Where(TblJob.Columns.JobName).Like(Keyword)
                    .Or(TblJob.Columns.Design).Like(Keyword)
                    .OrderAsc(TblJob.Columns.JobName)
                    .ExecuteAsCollection<TblJobCollection>();

            foreach (var item in result)
            {
                list.Add(item);
            }
            return list;
        }


        //Service job detail
        public static List<ServiceJobDetailExtension> SelectServiceJobDetailByID(int ID)
        {
            List<ServiceJobDetailExtension> list = new List<ServiceJobDetailExtension>();
            TblJob obj = SelectByID(ID);
            int CustomerID = obj != null ? obj.CustomerID : 0;
            //Lấy danh sách pricing
            TblCustomerQuotationAdditionalServiceCollection dictionary = new Select().From(TblCustomerQuotationAdditionalService.Schema)
                                                                    .Where(TblCustomerQuotationAdditionalService.CustomerIDColumn).IsEqualTo(CustomerID)
                                                                    .ExecuteAsCollection<TblCustomerQuotationAdditionalServiceCollection>();
            //Lấy danh sách Category
            var category = ReferenceTableManager.SelectJobCategoryForDDL();
            TblServiceJobDetailCollection colls = new SubSonic.Select()
                .From(TblServiceJobDetail.Schema)
                .Where(TblServiceJobDetail.JobIDColumn).IsEqualTo(ID).OrderAsc(TblServiceJobDetail.WorkOrderNumberColumn.ColumnName, TblServiceJobDetail.ProductIDColumn.ColumnName)
                .ExecuteAsCollection<TblServiceJobDetailCollection>();

            int i = 0;

            foreach (TblServiceJobDetail sObj in colls)
            {
                i += 1;
                TblCustomerQuotationAdditionalService pricing = dictionary.Where(x => x.Id == sObj.PricingID).FirstOrDefault();
                int CategoryID = 0;
                if (pricing != null)
                    int.TryParse(pricing.Category, out CategoryID);
                TblReference categoryObj = ReferenceTableManager.SelectByID(CategoryID);
                ServiceJobDetailExtension item = new ServiceJobDetailExtension(sObj);
                item.No = i;
                item.PricingName = dictionary.Where(x => x.Id == item.PricingID).Select(x => x.Description).FirstOrDefault();
                item.CategoryName = categoryObj != null ? categoryObj.Name : string.Empty;
                list.Add(item);
            }
            return list;
        }


        public static List<int> SelectListServiceJobIdByJobID(int jobID)
        {
            return new Select(TblServiceJobDetail.ServiceJobIDColumn).
                From(TblServiceJobDetail.Schema).Where(TblServiceJobDetail.JobIDColumn)
                .IsEqualTo(jobID)
                .ExecuteTypedList<int>();
        }

        public static TblServiceJobDetail SelectServiceJobDetailById(int ID)
        {
            return new Select().From(TblServiceJobDetail.Schema)
                .Where(TblServiceJobDetail.ServiceJobIDColumn).IsEqualTo(ID)
                .ExecuteSingle<TblServiceJobDetail>();
        }

        public static TblServiceJobDetail InsertServiceJobDetail(TblServiceJobDetail obj)
        {
            return new TblServiceJobDetailController().Insert(obj);
        }

        public static TblServiceJobDetail UpdateServiceJobDetail(TblServiceJobDetail obj)
        {
            return new TblServiceJobDetailController().Update(obj);
        }

        public static bool DeleteServiceJobDetail(int ID)
        {
            return new TblServiceJobDetailController().Destroy(ID);
        }

        public static bool DeleteServiceJob(int JobID)
        {
            try
            {
                new SubSonic.Delete().From(TblServiceJobDetail.Schema).Where(TblServiceJobDetail.JobIDColumn).IsEqualTo(JobID).Execute();
                return Delete(JobID);
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteServiceJobDetailByJobId(int jobId)
        {
            try
            {
                new SubSonic.Delete().From(TblServiceJobDetail.Schema).Where(TblServiceJobDetail.JobIDColumn).IsEqualTo(jobId).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static DataTable GetJobForPrinting(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.JobPrintingDetail(JobID).GetReader());
            return dt;
        }

        #region Engraving
        /// <summary>
        /// Select detail for engraving
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static DataTable tblEngravingDetail_SelectAll(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblEngravingDetailSelectAll(JobID).GetReader());
            return dt;
        }
        #endregion

        public static DataTable SP_TblEtchingDetail_SelectAll(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblEtchingDetailSelectAll(JobID).GetReader());
            return dt;
        }

        #region Trunglc Add 22-04-2015

        /// <summary>
        /// Check status of Job is Lock or Unlock
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns>
        /// True if Job is Lock
        /// Flale if Job is Unlock
        /// </returns>
        public static bool IsJobLocking(int JobID)
        {
            return ObjectLockingManager.IsObjectLocking(JobID, ObjectLockingType.JOB);
        }

        public static bool IsNewJob(int JobID)
        {
            return ObjectLockingManager.IsNewObjectLocking(JobID, ObjectLockingType.JOB);
        }

        public static void LockOrUnLockJob(int JobID, bool IsLock)
        {
            ObjectLockingManager.LockOrUnlockObjectLocking(JobID, ObjectLockingType.JOB, IsLock);
        }

        #endregion

        // Trung add 06-05-2015

        public static bool IsExistInvoiceCreatedByJobID(int JobID)
        {
            return new SubSonic.Select().From(TblInvoiceDetail.Schema)
                                        .Where(TblInvoiceDetail.JobIDColumn).IsEqualTo(JobID)
                                        .GetRecordCount() > 0 ? true : false;
        }

        // End 06-05-2015
       
    }
}
