using SubSonic;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public enum UnitOfMeasure
    {
        cm2,
        piece
    }

    public enum JobCategory
    {
        Simple,
        Medium,
        Complex
    }

    public class CustomerQuotationDetailExtension : TblCustomerQuotationDetail
    {
        //Phần bổ sung
        public TblCustomerQuotationDetail Parent { get; set; }
        //Chuyển Parents -> Children
        public CustomerQuotationDetailExtension() { }
        public CustomerQuotationDetailExtension(TblCustomerQuotationDetail parent)
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
        public string ProcessTypeName { set; get; }
        public string ProductTypeName { set; get; }
        public string CurrencyName { set; get; }
    }

    public class CustomerQuotationAdditionalServiceExtention : TblCustomerQuotationAdditionalService
    {
        //Phần bổ sung
        public TblCustomerQuotationAdditionalService Parent { get; set; }
        //Chuyển Parents -> Children
        public CustomerQuotationAdditionalServiceExtention() { }
        public CustomerQuotationAdditionalServiceExtention(TblCustomerQuotationAdditionalService parent)
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
        public string CurrencyName { set; get; }
        public string CategoryName { set; get; }
    }

    public class CustomerQuotationOtherChargesExtention : TblCustomerQuotationOtherCharge
    {
        //Phần bổ sung
        public TblCustomerQuotationOtherCharge Parent { get; set; }
        //Chuyển Parents -> Children
        public CustomerQuotationOtherChargesExtention() { }
        public CustomerQuotationOtherChargesExtention(TblCustomerQuotationOtherCharge parent)
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
        public string CurrencyName { set; get; }
    }

    public class CustomerQuotationManager
    {
        //BEGIN CUSTOMER QUOTATION
        public static string CreateQuotationNumber()
        {
            string _No = DateTime.Today.ToString("yyMM/");
            string _MaxNumber = new Select(Aggregate.Max(TblCustomerQuotation.QuotationNoColumn))
                .From(TblCustomerQuotation.Schema)
                .Where(TblCustomerQuotation.QuotationNoColumn).Like(_No + "%")
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
        /// Kiểm tra mã báo giá đã tồn tại chưa?
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="QuotationNo"></param>
        /// <returns></returns>
        public static bool Exists(int CustomerID, string QuotationNo)
        {
            return new Select().From(TblCustomerQuotation.Schema).Where(TblCustomerQuotation.CurrencyIDColumn).IsNotEqualTo(CustomerID).And(TblCustomerQuotation.QuotationNoColumn).IsEqualTo(QuotationNo).GetRecordCount() > 0 ? true : false;
        }

        public static TblCustomerQuotation SelectByID(int CustomerID)
        {
            return new Select().From(TblCustomerQuotation.Schema).Where(TblCustomerQuotation.CustomerIDColumn).IsEqualTo(CustomerID).ExecuteSingle<TblCustomerQuotation>();
        }

        
        public static TblCustomerQuotation Insert(TblCustomerQuotation obj)
        {
            return new TblCustomerQuotationController().Insert(obj);
        }

        public static TblCustomerQuotation Update(TblCustomerQuotation obj)
        {
            return new TblCustomerQuotationController().Update(obj);
        }

        public static bool Delete(int CustomerID)
        {
            return new TblCustomerQuotationController().Delete(CustomerID);
        }
        //END CUSTOMER QUOTATION

        #region Detail
        //BEGIN CUSTOMER QUOTATION DETAIl
        /// <summary>
        /// Select quotation detail by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblCustomerQuotationDetail SelectDetailByID(int ID)
        {
            return new Select().From(TblCustomerQuotationDetail.Schema)
                    .Where(TblCustomerQuotationDetail.IdColumn)
                    .IsEqualTo(ID).ExecuteSingle<TblCustomerQuotationDetail>();
        }

        /// <summary>
        /// Select all detail
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static List<CustomerQuotationDetailExtension> SelectDetail(int CustomerID)
        {
            List<CustomerQuotationDetailExtension> list = new List<CustomerQuotationDetailExtension>();
            //Lấy danh sách ProcessType
            var processTypeList = new Select().From(TblReference.Schema)
                    .Where(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.ProcessType)
                    .ExecuteAsCollection<TblReferenceCollection>();
            //Lấy danh sách ProductType
            var productTypeList = new Select().From(TblReference.Schema)
                    .Where(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.ProductType)
                    .ExecuteAsCollection<TblReferenceCollection>();
            //Lấy danh sách Currency
            var currencyList = new Select().From(TblCurrency.Schema)
                    .ExecuteAsCollection<TblCurrencyCollection>();
            //Lấy chi tiết Quotation
            var quotationDetail = new Select().From(TblCustomerQuotationDetail.Schema)
                    .Where(TblCustomerQuotationDetail.CustomerIdColumn).IsEqualTo(CustomerID)
                    .OrderAsc(TblCustomerQuotationDetail.Columns.PricingName)
                    .ExecuteAsCollection<TblCustomerQuotationDetailCollection>();
            //Hợp dữ liệu
            foreach (var q in quotationDetail)
            {
                CustomerQuotationDetailExtension obj = new CustomerQuotationDetailExtension();
                obj.Id = q.Id;
                obj.CustomerId = CustomerID;
                obj.ProcessTypeID = q.ProcessTypeID;
                obj.ProcessTypeName = processTypeList.Where(x => x.ReferencesID == q.ProcessTypeID).Select(x => x.Code).FirstOrDefault();
                obj.ProductTypeID = q.ProductTypeID;
                obj.ProductTypeName = productTypeList.Where(x => x.ReferencesID == q.ProductTypeID).Select(x => x.Code).FirstOrDefault();
                obj.PricingName = q.PricingName;
                obj.GLCode = q.GLCode;
                obj.Description = q.Description;
                obj.OldSteelBase = q.OldSteelBase;
                obj.NewSteelBase = q.NewSteelBase;
                obj.CurrencyID = q.CurrencyID;
                obj.CurrencyName = currencyList.Where(x => x.CurrencyID == q.CurrencyID).Select(x => x.CurrencyName).FirstOrDefault();
                obj.UnitOfMeasure = q.UnitOfMeasure;
                list.Add(obj);
            }
            return list;
        }

        public static List<CustomerQuotationDetailExtension> SelectPricingMasterTemplateDetail(int CustomerID)
        {
            List<CustomerQuotationDetailExtension> list = new List<CustomerQuotationDetailExtension>();
            list = SelectDetail(CustomerID);
            Random rnd = new Random();
            int rndID = rnd.Next(-1000, -1);
            foreach (CustomerQuotationDetailExtension item in list)
            {
                while(list.Where(x => x.Id == rndID).Count() > 0)
                    rndID = rnd.Next(-1000, -1);
                item.Id = rndID;
            }
            return list;
        }

        public static TblCustomerQuotationDetailCollection SelectAllDetail(int CustomerID)
        {
            return new Select().From(TblCustomerQuotationDetail.Schema)
                                .Where(TblCustomerQuotationDetail.CustomerIdColumn).IsEqualTo(CustomerID)
                                .ExecuteAsCollection<TblCustomerQuotationDetailCollection>();
        }

        public static TblCustomerQuotationDetail InsertDetail(TblCustomerQuotationDetail obj)
        {
            return new TblCustomerQuotationDetailController().Insert(obj);
        }

        public static TblCustomerQuotationDetail UpdateDetail(TblCustomerQuotationDetail obj)
        {
            return new TblCustomerQuotationDetailController().Update(obj);
        }

        public static TblCustomerQuotationDetail SelectCustomerQuotationPricingByCustomerIDAndPricingID(int pricingId, int customerID)
        {
            return new SubSonic.Select().From(TblCustomerQuotationDetail.Schema)
                            .Where(TblCustomerQuotationDetail.CustomerIdColumn).IsEqualTo(customerID)
                            .And(TblCustomerQuotationDetail.IdColumn).IsEqualTo(pricingId)
                            .ExecuteSingle<TblCustomerQuotationDetail>();
        }

        //Delete All Old Value
        public static bool DeleteDetail(int ID)
        {
            return new TblCustomerQuotationDetailController().Delete(ID);
        }

        public static List<UnitOfMeasure> SelectUnitOfMeasureForDDL()
        {
            List<UnitOfMeasure> list = new List<UnitOfMeasure>();
            list = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().ToList();
            return list;
        }

        public static List<TblCustomerQuotationDetail> SelectQuotationForDDL(int CustomerID, short CurrencyID, int ProductTypeID)
        {
            List<TblCustomerQuotationDetail> list = new List<TblCustomerQuotationDetail>();
            list = new Select().From(TblCustomerQuotationDetail.Schema)
                    .Where(TblCustomerQuotationDetail.CustomerIdColumn).IsEqualTo(CustomerID)
                    .And(TblCustomerQuotationDetail.CurrencyIDColumn).IsEqualTo(CurrencyID)
                    .And(TblCustomerQuotationDetail.ProductTypeIDColumn).IsEqualTo(ProductTypeID)
                    .ExecuteAsCollection<TblCustomerQuotationDetailCollection>().ToList<TblCustomerQuotationDetail>();
            if (list == null)
                list = new List<TblCustomerQuotationDetail>();
            TblCustomerQuotationDetail obj = new TblCustomerQuotationDetail();
            obj.Id = 0;
            obj.PricingName = "--Select pricing--";
            list.Insert(0, obj);
            return list;
        }
        //END CUSTOMER QUOTATION DETAIL
        #endregion Detail

        #region AdditionalService
        //BEGIN CUSTOMER QUOTATION ADDITIONAL SERVICES
        public static List<JobCategory> SelectJobCategoryForDDL()
        {
            List<JobCategory> list = new List<JobCategory>();
            list = Enum.GetValues(typeof(JobCategory)).Cast<JobCategory>().ToList();
            return list;
        }

        /// <summary>
        /// Select quotation detail by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblCustomerQuotationAdditionalService SelectAdditionalByID(int ID)
        {
            return new Select().From(TblCustomerQuotationAdditionalService.Schema)
                    .Where(TblCustomerQuotationAdditionalService.IdColumn)
                    .IsEqualTo(ID).ExecuteSingle<TblCustomerQuotationAdditionalService>();
        }

        /// <summary>
        /// Select all detail
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static List<CustomerQuotationAdditionalServiceExtention> SelectAdditional(int CustomerID)
        {
            List<CustomerQuotationAdditionalServiceExtention> list = new List<CustomerQuotationAdditionalServiceExtention>();
            //Lấy danh sách Currency
            var currencyList = new Select().From(TblCurrency.Schema)
                    .ExecuteAsCollection<TblCurrencyCollection>();
            //Lấy danh sách Category
            var categoryList = ReferenceTableManager.SelectJobCategoryForDDL();
            //Lấy chi tiết Quotation
            var quotationDetail = new Select().From(TblCustomerQuotationAdditionalService.Schema)
                    .Where(TblCustomerQuotationAdditionalService.CustomerIDColumn).IsEqualTo(CustomerID)
                    .OrderAsc(TblCustomerQuotationAdditionalService.Columns.Description)
                    .ExecuteAsCollection<TblCustomerQuotationAdditionalServiceCollection>();
            //Hợp dữ liệu
            foreach (var q in quotationDetail)
            {
                int JobCategory = 0;
                int.TryParse(q.Category, out JobCategory);
                CustomerQuotationAdditionalServiceExtention obj = new CustomerQuotationAdditionalServiceExtention(q);
                obj.CurrencyName = currencyList.Where(x => x.CurrencyID == q.CurrencyID).Select(x => x.CurrencyName).FirstOrDefault();
                obj.CategoryName = categoryList.Where(x => x.ReferencesID == JobCategory).Select(x => x.Name).FirstOrDefault();
                list.Add(obj);
            }
            return list;
        }

        public static List<CustomerQuotationAdditionalServiceExtention> SelectPricingMasterTemplateAdditional(int CustomerID)
        {
            List<CustomerQuotationAdditionalServiceExtention> list = new List<CustomerQuotationAdditionalServiceExtention>();
            list = SelectAdditional(CustomerID);
            Random rnd = new Random();
            int rndID = rnd.Next(-1000, -1);
            foreach (CustomerQuotationAdditionalServiceExtention item in list)
            {
                while (list.Where(x => x.Id == rndID).Count() > 0)
                    rndID = rnd.Next(-1000, -1);
                item.Id = rndID;
            }
            return list;
        }

        public static TblCustomerQuotationAdditionalServiceCollection SelectAllAdditional(int CustomerID)
        {
            return new Select().From(TblCustomerQuotationAdditionalService.Schema)
                                .Where(TblCustomerQuotationAdditionalService.CustomerIDColumn).IsEqualTo(CustomerID)
                                .ExecuteAsCollection<TblCustomerQuotationAdditionalServiceCollection>();
        }

        public static TblCustomerQuotationAdditionalService InsertAdditional(TblCustomerQuotationAdditionalService obj)
        {
            return new TblCustomerQuotationAdditionalServiceController().Insert(obj);
        }

        public static TblCustomerQuotationAdditionalService UpdateAdditional(TblCustomerQuotationAdditionalService obj)
        {
            return new TblCustomerQuotationAdditionalServiceController().Update(obj);
        }

        public static TblCustomerQuotationAdditionalService SelectCustomerQuotationAdditionalByCustomerIDAndPricingID(int PricingID, int CustomerID)
        {
            return new SubSonic.Select().From(TblCustomerQuotationAdditionalService.Schema)
                            .Where(TblCustomerQuotationAdditionalService.CustomerIDColumn).IsEqualTo(CustomerID)
                            .And(TblCustomerQuotationAdditionalService.IdColumn).IsEqualTo(PricingID)
                            .ExecuteSingle<TblCustomerQuotationAdditionalService>();
        }

        //Delete All Old Value
        public static bool DeleteAdditional(int ID)
        {
            return new TblCustomerQuotationAdditionalServiceController().Delete(ID);
        }

        public static List<TblCustomerQuotationAdditionalService> SelectQuotationAdditionalForDDL(int CustomerID, short CurrencyID)
        {
            List<TblCustomerQuotationAdditionalService> list = new List<TblCustomerQuotationAdditionalService>();
            list = new Select().From(TblCustomerQuotationAdditionalService.Schema)
                    .Where(TblCustomerQuotationAdditionalService.CustomerIDColumn).IsEqualTo(CustomerID)
                    .And(TblCustomerQuotationAdditionalService.CurrencyIDColumn).IsEqualTo(CurrencyID)
                    .ExecuteAsCollection<TblCustomerQuotationAdditionalServiceCollection>().ToList<TblCustomerQuotationAdditionalService>();
            if (list == null)
                list = new List<TblCustomerQuotationAdditionalService>();
            TblCustomerQuotationAdditionalService obj = new TblCustomerQuotationAdditionalService();
            obj.Id = 0;
            obj.Description = "--Additional Services--";
            list.Insert(0, obj);
            return list;
        }
        //END CUSTOMER QUOTATION ADDITIONAL SERVICES
        #endregion

        #region OtherCharges
        //BEGIN CUSTOMER QUOTATION OTHER CHARGES
        /// <summary>
        /// Select quotation detail by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblCustomerQuotationOtherCharge SelectOtherChargesByID(int ID)
        {
            return new Select().From(TblCustomerQuotationOtherCharge.Schema)
                    .Where(TblCustomerQuotationOtherCharge.IdColumn)
                    .IsEqualTo(ID).ExecuteSingle<TblCustomerQuotationOtherCharge>();
        }

        /// <summary>
        /// Select all detail
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static List<CustomerQuotationOtherChargesExtention> SelectOtherCharges(int CustomerID)
        {
            List<CustomerQuotationOtherChargesExtention> list = new List<CustomerQuotationOtherChargesExtention>();
            //Lấy danh sách Currency
            var currencyList = new Select().From(TblCurrency.Schema)
                    .ExecuteAsCollection<TblCurrencyCollection>();
            //Lấy chi tiết Quotation
            var quotationDetail = new Select().From(TblCustomerQuotationOtherCharge.Schema)
                    .Where(TblCustomerQuotationOtherCharge.CustomerIDColumn).IsEqualTo(CustomerID)
                    .OrderAsc(TblCustomerQuotationOtherCharge.Columns.Description)
                    .ExecuteAsCollection<TblCustomerQuotationOtherChargeCollection>();
            //Hợp dữ liệu
            foreach (var q in quotationDetail)
            {
                CustomerQuotationOtherChargesExtention obj = new CustomerQuotationOtherChargesExtention();
                obj.Id = q.Id;
                obj.CustomerID = CustomerID;
                obj.GLCode = q.GLCode;
                obj.Description = q.Description;
                obj.Price = q.Price;
                obj.CurrencyID = q.CurrencyID;
                obj.CurrencyName = currencyList.Where(x => x.CurrencyID == q.CurrencyID).Select(x => x.CurrencyName).FirstOrDefault();
                list.Add(obj);
            }
            return list;
        }

        public static List<CustomerQuotationOtherChargesExtention> SelectPricingMasterTemplateOtherCharges(int CustomerID)
        {
            List<CustomerQuotationOtherChargesExtention> list = new List<CustomerQuotationOtherChargesExtention>();
            list = SelectOtherCharges(CustomerID);
            Random rnd = new Random();
            int rndID = rnd.Next(-1000, -1);
            foreach (CustomerQuotationOtherChargesExtention item in list)
            {
                while (list.Where(x => x.Id == rndID).Count() > 0)
                    rndID = rnd.Next(-1000, -1);
                item.Id = rndID;
            }
            return list;
        }

        public static TblCustomerQuotationOtherChargeCollection SelectAllOtherCharges(int CustomerID)
        {
            return new Select().From(TblCustomerQuotationOtherCharge.Schema)
                                .Where(TblCustomerQuotationOtherCharge.CustomerIDColumn).IsEqualTo(CustomerID)
                                .ExecuteAsCollection<TblCustomerQuotationOtherChargeCollection>();
        }

        public static TblCustomerQuotationOtherCharge InsertOtherChagres(TblCustomerQuotationOtherCharge obj)
        {
            return new TblCustomerQuotationOtherChargeController().Insert(obj);
        }

        public static TblCustomerQuotationOtherCharge UpdateOtherChagres(TblCustomerQuotationOtherCharge obj)
        {
            return new TblCustomerQuotationOtherChargeController().Update(obj);
        }

        public static TblCustomerQuotationOtherCharge SelectCustomerQuotationOtherChargesByCustomerIDAndPricingID(int PricingID, int CustomerID)
        {
            return new SubSonic.Select().From(TblCustomerQuotationOtherCharge.Schema)
                            .Where(TblCustomerQuotationOtherCharge.CustomerIDColumn).IsEqualTo(CustomerID)
                            .And(TblCustomerQuotationOtherCharge.IdColumn).IsEqualTo(PricingID)
                            .ExecuteSingle<TblCustomerQuotationOtherCharge>();
        }

        //Delete All Old Value
        public static bool DeleteOtherCharges(int ID)
        {
            return new TblCustomerQuotationOtherChargeController().Delete(ID);
        }

        public static List<TblCustomerQuotationOtherCharge> SelectQuotationOtherChargesForDDL(int CustomerID, short CurrencyID)
        {
            List<TblCustomerQuotationOtherCharge> list = new List<TblCustomerQuotationOtherCharge>();
            list = new Select().From(TblCustomerQuotationOtherCharge.Schema)
                    .Where(TblCustomerQuotationOtherCharge.CustomerIDColumn).IsEqualTo(CustomerID)
                    .And(TblCustomerQuotationOtherCharge.CurrencyIDColumn).IsEqualTo(CurrencyID)
                    .ExecuteAsCollection<TblCustomerQuotationOtherChargeCollection>().ToList<TblCustomerQuotationOtherCharge>();
            if (list == null)
                list = new List<TblCustomerQuotationOtherCharge>();
            TblCustomerQuotationOtherCharge obj = new TblCustomerQuotationOtherCharge();
            obj.Id = 0;
            obj.Description = "--Other charges--";
            list.Insert(0, obj);
            return list;
        }
        //END CUSTOMER QUOTATION OTHER CHARGES
        #endregion
    }
}
