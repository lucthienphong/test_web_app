using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class JobQuotationDetailExtension : TblJobQuotationPricing
    {
        //Phần bổ sung
        public TblJobQuotationPricing Parent { get; set; }
        //Chuyển Parents -> Children
        public JobQuotationDetailExtension() { }
        public JobQuotationDetailExtension(TblJobQuotationPricing parent)
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
        public string CurrencyName { set; get; }
        public string UnitName { set; get; }
    }

    public class JobQuotationManager
    {
        //BEGIN Job QUOTATION
        public static TblJobQuotation SelectByID(int QuotationID)
        {
            return new Select().From(TblJobQuotation.Schema)
                                .Where(TblJobQuotation.QuotationIDColumn).IsEqualTo(QuotationID)
                                .ExecuteSingle<TblJobQuotation>();
        }

        public static TblJobQuotation SelectNewestQuotationByJobID(int JobID)
        {
            return new Select().Top("1")
                .From(TblJobQuotation.Schema).Where(TblJobQuotation.JobIDColumn).IsEqualTo(JobID)
                .OrderDesc(TblJobQuotation.Columns.RevNumber)
                .ExecuteSingle<TblJobQuotation>();
        }

        public static TblJobQuotation Insert(TblJobQuotation obj)
        {
            return new TblJobQuotationController().Insert(obj);
        }

        public static TblJobQuotation Update(TblJobQuotation obj)
        {
            return new TblJobQuotationController().Update(obj);
        }

        public static bool Delete(int JobID)
        {
            return new TblJobQuotationController().Delete(JobID);
        }
        //END Job QUOTATION

        //BEGIN Job QUOTATION DETAIl
        /// <summary>
        /// Select all detail
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns></returns>
        public static List<JobQuotationDetailExtension> SelectDetail(int QuotationID, int JobID)
        {
            TblJob jObj = JobManager.SelectByID(JobID);
            List<JobQuotationDetailExtension> list = new List<JobQuotationDetailExtension>();
            //Lấy danh sách Pricing
            TblCustomerQuotationDetailCollection pricingList = new Select().From(TblCustomerQuotationDetail.Schema)
                .Where(TblCustomerQuotationDetail.CustomerIdColumn).IsEqualTo(jObj != null ? jObj.CustomerID : 0)
                    .OrderAsc(TblPricing.Columns.PricingName)
                    .ExecuteAsCollection<TblCustomerQuotationDetailCollection>();
            //Lấy chi tiết Quotation
            var quotationDetail = new Select().From(TblJobQuotationPricing.Schema)
                    .Where(TblJobQuotationPricing.QuotationIDColumn).IsEqualTo(QuotationID)
                    .ExecuteAsCollection<TblJobQuotationPricingCollection>();
            //Hợp dữ liệu
            foreach (var p in pricingList)
            {
                TblCurrency crObj = new CurrencyManager().SelectByID(p.CurrencyID);
                JobQuotationDetailExtension obj = new JobQuotationDetailExtension();
                TblJobQuotationPricing qObj = quotationDetail.Where(x => x.PricingID == p.Id).FirstOrDefault();
                obj.QuotationID = QuotationID;
                obj.PricingID = p.Id;
                obj.PricingName = p.PricingName;
                obj.UnitName = p.UnitOfMeasure;
                obj.CurrencyName = crObj != null ? crObj.CurrencyName : string.Empty;
                obj.OldSteelBasePrice = qObj != null ? qObj.OldSteelBasePrice : p.OldSteelBase;
                obj.NewSteelBasePrice = qObj != null ? qObj.NewSteelBasePrice : p.NewSteelBase;
                list.Add(obj);
            }
            return list;
        }

        public static bool CheckExits(int jobId)
        {
            return new Select().From(TblJobQuotation.Schema).Where(TblJobQuotation.JobIDColumn).IsEqualTo(jobId).GetRecordCount() > 0 ? true : false;
        }

        public static TblJobQuotationPricing InsertDetail(TblJobQuotationPricing obj)
        {
            return new TblJobQuotationPricingController().Insert(obj);
        }

        public static TblJobQuotationPricing UpdateDetail(TblJobQuotationPricing obj)
        {
            return new TblJobQuotationPricingController().Update(obj);
        }

        public static bool CheckExitsByJobID(int QuotationID, short PricingID)
        {
            return new SubSonic.Select()
                .From(TblJobQuotationPricing.Schema)
                .Where(TblJobQuotationPricing.QuotationIDColumn).IsEqualTo(QuotationID)
                .And(TblJobQuotationPricing.PricingIDColumn).IsEqualTo(PricingID)
                .GetRecordCount() > 0 ? true : false;
        }

        public static TblJobQuotationPricing SelectDetailByJobIDAndPricingID(int QuotationID, int PricingID)
        {
            return new SubSonic.Select()
                .From(TblJobQuotationPricing.Schema)
                .Where(TblJobQuotationPricing.QuotationIDColumn).IsEqualTo(QuotationID)
                .And(TblJobQuotationPricing.PricingIDColumn).IsEqualTo(PricingID)
                .ExecuteSingle<TblJobQuotationPricing>();
        }

        public static TblJobQuotationCollection SelectAllQuotationByJobID(int JobID)
        {
            TblJobQuotationCollection list = new Select()
                                            .From(TblJobQuotation.Schema).Where(TblJobQuotation.JobIDColumn).IsEqualTo(JobID)
                                            .OrderDesc(TblJobQuotation.Columns.RevNumber)
                                            .ExecuteAsCollection<TblJobQuotationCollection>();
            if (list == null)
                list = new TblJobQuotationCollection();
            TblJobQuotation obj = new TblJobQuotation();
            obj.RevNumber = 0;
            obj.QuotationID = JobID;
            list.Add(obj);
            return list;
        }
        //Delete All Old Value
        public static bool DeleteDetail(int JobID)
        {
            return new TblJobQuotationPricingController().Delete(JobID);
        }
        //END Job QUOTATION DETAIL
    }
}
