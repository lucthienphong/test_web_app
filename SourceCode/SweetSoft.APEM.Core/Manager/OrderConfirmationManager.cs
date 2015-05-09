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
    public class OtherChargesExtension : TblOtherCharge
    {
        //Phần bổ sung
        public TblOtherCharge Parent { get; set; }
        //Chuyển Parents -> Children
        public OtherChargesExtension() { }
        public OtherChargesExtension(TblOtherCharge parent)
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
        public string PricingName { set; get; }
    }

    public class OrderConfirmationManager
    {
        /// <summary>
        /// Get base country ID
        /// </summary>
        /// <returns></returns>
        public static int BaseCountryID()
        {
            //Lấy thông tin base country trong setting
            TblSystemSetting setting  = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCountrySetting);
            int BCountryID = setting != null ? Convert.ToInt32(setting.SettingValue) : 0;
            return BCountryID;
        }


        public static bool HaveGST(int JobID)
        {
            bool yesno = false;//Mặc định là không được gán thuế
            TblJob obj = JobManager.SelectByID(JobID);
            if(obj != null)//Nếu tìm thấy job
            {
                if (obj.IsServiceJob == 1)//Nếu là service job thì cho phép chọn thuế
                    yesno = true;
                else//Nếu là job thường thì kiểm tra
                {
                    int ShipTopartyID = obj.ShipToParty != null ? (int)obj.ShipToParty : 0;//Lấy mã shiptoparty
                    TblCustomer stpObj = CustomerManager.SelectByID(ShipTopartyID);//Lấy thông tin ShipToParty
                    if (stpObj != null)//Nếu tìm thấy ship to party
                    {
                        int baseCountryID = BaseCountryID();//Lấy mã base country
                        if (baseCountryID == stpObj.CountryID && baseCountryID != 0)//Nếu mã base country != 0 và basecountry = stpCountry thì cho phép chọn thuế
                            yesno = true;
                    }
                }
            }
            return yesno;
        }


        public static TblOrderConfirmation Insert(TblOrderConfirmation obj)
        {
            return new TblOrderConfirmationController().Insert(obj);
        }

        public static TblOrderConfirmation SelectByID(int JobID)
        {
            return new Select().From(TblOrderConfirmation.Schema).Where(TblOrderConfirmation.JobIDColumn).IsEqualTo(JobID).ExecuteSingle<TblOrderConfirmation>();
        }

        public static TblOrderConfirmation Update(TblOrderConfirmation obj)
        {
            return new TblOrderConfirmationController().Update(obj);
        }

        public static bool Delete(int ID)
        {
            //Delete Other Charges
            //new Delete().From(TblOtherCharge.Schema).Where(TblOtherCharge.JobIDColumn).IsEqualTo(ID).Execute();
            //Delete Job Quotation
            TblJobQuotationCollection quoteColl = new Select().From(TblJobQuotation.Schema)
                                                    .Where(TblJobQuotation.JobIDColumn).IsEqualTo(ID)
                                                    .ExecuteAsCollection<TblJobQuotationCollection>();
            foreach (TblJobQuotation item in quoteColl)
            {
                new Delete().From(TblJobQuotationPricing.Schema).Where(TblJobQuotationPricing.QuotationIDColumn).IsEqualTo(item.QuotationID).Execute();
                new Delete().From(TblJobQuotation.Schema).Where(TblJobQuotation.QuotationIDColumn).IsEqualTo(item.QuotationID).Execute();
            }
            //Delete OC
            return new TblOrderConfirmationController().Destroy(ID);
        }

        //Other charges
        public static List<OtherChargesExtension> SelectOtherChargeByJobID(int jobID)
        {
            //Lấy thông tin Job
            TblJob jObj = JobManager.SelectByID(jobID);
            //Khởi tạo danh sách
            List<OtherChargesExtension> list = new List<OtherChargesExtension>();
            //Lấy từ điển
            TblCustomerQuotationOtherChargeCollection dictionary = new Select().From(TblCustomerQuotationOtherCharge.Schema)
                .Where(TblCustomerQuotationOtherCharge.CustomerIDColumn).IsEqualTo(jObj != null ? jObj.CustomerID : 0)
                .ExecuteAsCollection<TblCustomerQuotationOtherChargeCollection>();
            TblOtherChargeCollection coll = new SubSonic.Select().From(TblOtherCharge.Schema)
                                                .Where(TblOtherCharge.JobIDColumn).IsEqualTo(jobID)
                                                .ExecuteAsCollection<TblOtherChargeCollection>();
            foreach (TblOtherCharge item in coll)
            {
                OtherChargesExtension obj = new OtherChargesExtension(item);
                obj.PricingName = dictionary.Where(x => x.Id == item.PricingID).Select(x => x.Description).FirstOrDefault();
                list.Add(obj);
            }

            return list;
        }

        public static List<int> SelectListOtherChargeIDByJobID(int jobID)
        {
            return new Select(TblOtherCharge.OtherChargesIDColumn).
                From(TblOtherCharge.Schema).Where(TblOtherCharge.JobIDColumn)
                .IsEqualTo(jobID)
                .ExecuteTypedList<int>();
        }

        public static TblOtherCharge SelectOtherChargeByID(int ID)
        {
            return new SubSonic.Select()
                .From(TblOtherCharge.Schema)
                .Where(TblOtherCharge.OtherChargesIDColumn).IsEqualTo(ID)
                .ExecuteSingle<TblOtherCharge>();
        }

        public static TblOtherCharge InsertOtherCharge(TblOtherCharge obj)
        {
            return new TblOtherChargeController().Insert(obj);
        }

        public static object UpdateOtherCharge(TblOtherCharge obj)
        {
            return new TblOtherChargeController().Update(obj);
        }

        public static bool DeleteOtherCharge(int ID)
        {
           return new TblOtherChargeController().Destroy(ID);
        }

        public static DataTable ConfirmOrderSelectAll(string Customer, string Job, string OCNumber, DateTime? BeginDate, DateTime? EndDate, int? PageIndex, int? PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblConfirmOrderSelectAll(Customer, Job, OCNumber, BeginDate, EndDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public static void DeleteOtherChargeByJobID(int jobID)
        {
            new SubSonic.Delete().From(TblOtherCharge.Schema).Where(TblOtherCharge.JobIDColumn).IsEqualTo(jobID).Execute();
        }

        public static List<int> SelectListJobIdInOrderConfirmationByCustomerID(int customerID)
        {
           Select select = new Select(TblOrderConfirmation.JobIDColumn);
           select.From(TblOrderConfirmation.Schema);
           select.InnerJoin(TblJob.JobIDColumn, TblOrderConfirmation.JobIDColumn);
           select.Where(TblJob.CustomerIDColumn).IsEqualTo(customerID);
           return select.ExecuteTypedList<int>();
        }

        public static List<TblOrderConfirmation> SelectOrderConfirmationByCustomerID(int CustomerID)
        {
            List<TblOrderConfirmation> list = new List<TblOrderConfirmation>();
            //Tìm các phiếu đã lập DO
            SqlQuery exists = new Select(TblDeliveryOrder.JobIDColumn)
                            .From(TblDeliveryOrder.Schema)
                            .InnerJoin(TblJob.JobIDColumn, TblDeliveryOrder.JobIDColumn)
                            .Where(TblJob.CustomerIDColumn).IsEqualTo(CustomerID);

            list = new Select()
                        .From(TblOrderConfirmation.Schema)
                        .InnerJoin(TblJob.JobIDColumn, TblOrderConfirmation.JobIDColumn)
                        .InnerJoin(TblCustomer.CustomerIDColumn, TblJob.CustomerIDColumn)
                        .Where(TblCustomer.CustomerIDColumn).IsEqualTo(CustomerID)
                        .And(TblOrderConfirmation.JobIDColumn).NotIn(exists)
                        .ExecuteAsCollection<TblOrderConfirmationCollection>().ToList();
            if (list.Count == 0)
            {
                TblOrderConfirmation obj = new TblOrderConfirmation();
                obj.JobID = 0;
                obj.OCNumber = "N/A";
                list.Insert(0, obj);
            }
            return list;
        }

        public static void ResetTotalPriceForOC(int JobID)
        {
            decimal Total = 0;
            TblOrderConfirmation obj = SelectByID(JobID);
            if (obj != null)
            {
                //Select all cylinder prices
                DataTable dtCylinder = new DataTable();
                dtCylinder = CylinderManager.SelectCylinderSelectForOrderConfirmation(JobID);
                if (dtCylinder != null)
                {
                    foreach (DataRow r in dtCylinder.Rows)
                    {
                        Total += Math.Round(Convert.ToDecimal(r["TotalPrice"].ToString()), 2);
                    }
                }

                //Select all service job
                List<ServiceJobDetailExtension> listServiceJob = JobManager.SelectServiceJobDetailByID(JobID);
                if (listServiceJob.Count > 0)
                {
                    Total += listServiceJob.Sum(x => x.WorkOrderValues);
                }

                //Select all other charges
                List<OtherChargesExtension> listOtherCharges = OrderConfirmationManager.SelectOtherChargeByJobID(JobID);
                if (listOtherCharges.Count > 0)
                {
                    Total += listOtherCharges.Sum(x => (decimal)x.Charge  * (decimal)x.Quantity);
                }

                //Total = (Total * (1 - (decimal)obj.Discount / 100)) * (1 + (decimal)obj.TaxPercentage / 100);

                obj.TotalPrice = Total;
                Update(obj);
            }
        }

        public static void ResetTotalPriceForInvoice(int JobID)
        {
            decimal Total = 0, NetTotal = 0;
            //Lấy thông tin chi tiết invoice
            TblInvoiceDetail dObj = new Select().From(TblInvoiceDetail.Schema)
                                           .Where(TblInvoiceDetail.JobIDColumn).IsEqualTo(JobID)
                                           .ExecuteSingle<TblInvoiceDetail>();
            if (dObj != null)//Lấy thông tin invoice
            {
                TblInvoice obj = InvoiceManager.SelectByID(dObj.InvoiceID);
                if (obj != null)
                {
                    //Lấy danh sách order confirmation
                    TblOrderConfirmationCollection collOC = new Select().From(TblOrderConfirmation.Schema)
                                                                        .InnerJoin(TblInvoiceDetail.JobIDColumn, TblOrderConfirmation.JobIDColumn)
                                                                        .Where(TblInvoiceDetail.InvoiceIDColumn).IsEqualTo(obj.InvoiceID)
                                                                        .ExecuteAsCollection<TblOrderConfirmationCollection>();
                    if (collOC != null)
                    {
                        Total = collOC.Sum(x => (decimal)x.TotalPrice * (1 - (decimal)x.Discount/100));
                        NetTotal = collOC.Sum(x => ((decimal)x.TotalPrice * (1 - (decimal)x.Discount/100)) * (1 + (decimal)x.TaxPercentage / 100));
                        obj.TotalPrice = Total;
                        obj.NetTotal = NetTotal;
                        InvoiceManager.Update(obj);
                    }
                }
            }
        }

        #region Trunglc Add - 23-04-2015

        public static bool IsNewOrderConfirmation(int JobID)
        {
            return ObjectLockingManager.IsNewObjectLocking(JobID, ObjectLockingType.OC);
        }

        public static bool IsOrderConfirmationLocking(int JobID) 
        {
            return ObjectLockingManager.IsObjectLocking(JobID, ObjectLockingType.OC);
        }

        public static void LockOrUnlockOrderConfirmation(int JobID, bool IsLock)
        {
            ObjectLockingManager.LockOrUnlockObjectLocking(JobID, ObjectLockingType.OC, IsLock);
        }

        public static void LockJob(int JobID)
        {
            JobManager.LockOrUnLockJob(JobID, true);
        }

        #endregion
    }
}
