using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public enum CylinderStatusAction
    {
        [Description("None")]
        None,
        [Description("De/Re Chrome")]
        DeReChrome,
        [Description("Repro")]
        Repro
    }

    /// <summary>
    /// TblCurrency
    /// TblSupply
    /// TblTax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReference<T>
    {
        T Insert(T obj);
        T Update(T obj);
        T SelectByID(short id);
        bool IsBeingUsed(short id);
        bool Delete(short id);
        bool Exist(short id);
        bool Exist(short id, string name);
        DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew);
    }

    public class CurrencyManager : IReference<TblCurrency>
    {
        public TblCurrency Insert(TblCurrency obj)
        {
            return new TblCurrencyController().Insert(obj);
        }

        public TblCurrency Update(TblCurrency obj)
        {
            return new TblCurrencyController().Update(obj);
        }

        public bool Delete(short id)
        {
            return new TblCurrencyController().Delete(id);
        }

        public TblCurrency SelectByID(short id)
        {
            return new Select().From(TblCurrency.Schema)
              .Where(TblCurrency.Columns.CurrencyID).IsEqualTo(id)
              .OrderAsc(TblCurrency.Columns.CurrencyName).ExecuteSingle<TblCurrency>();
        }

        public bool Exist(short id)
        {
            return new Select().From(TblCurrency.Schema)
              .Where(TblCurrency.CurrencyIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblCurrency.Schema)
                .Where(TblCurrency.CurrencyIDColumn).IsNotEqualTo(id)
                .And(TblCurrency.CurrencyNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public bool IsBeingUsed(short ID)
        {
            //Kiểm tra TblOrder
            bool isOrdered = new Select().From(TblOrderConfirmation.Schema).Where(TblOrderConfirmation.CurrencyIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;

            if (isOrdered)
                return true;
            return false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCurrencySelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("CurrencyID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["CurrencyID"] = randID;
                r["CurrencyName"] = "";
                r["RMValue"] = 0;
                r["CurrencyValue"] = 0;
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public void InsertChangeedLog(TblCurrencyChangedLog changeLog)
        {
            new TblCurrencyChangedLogController().Insert(changeLog);
        }

        public DataTable SelectCurrencyLogByCurrencyID(int currencyID,DateTime? SearchDate,int PageIndex,int PageSize,string SortColumn,string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCurrencyChangegLodSelectAll(currencyID,SearchDate, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public TblCurrencyCollection SelectAllForDDL()
        {
           return new SubSonic.Select().From(TblCurrency.Schema)
                            .Where(TblCurrency.IsObsoleteColumn).IsEqualTo(false)
                            .OrderAsc(TblCurrency.Columns.CurrencyName)
                            .ExecuteAsCollection<TblCurrencyCollection>();
        }

        public decimal SelectExchangeRateByCurrencyID(short CurrencyID, DateTime EndDate)
        {
            decimal RMValue = 0;
            TblCurrencyChangedLog obj = new Select().Top("1")
                                                .From(TblCurrencyChangedLog.Schema)
                                                .Where(TblCurrencyChangedLog.CurrencyIDColumn).IsEqualTo(CurrencyID)
                                                .And(TblCurrencyChangedLog.DateChangedColumn).IsGreaterThan(EndDate)
                                                .OrderDesc(TblCurrencyChangedLog.Columns.DateChanged)
                                                .ExecuteSingle<TblCurrencyChangedLog>();
            if (obj != null)//Nếu tìm thấy lịch sử thì tìm giá trị gần nhất
            {
                RMValue = obj != null ? Convert.ToDecimal(obj.NewRMValue) : 0;
            }
            else//Nếu không tìm thấy lịch sử thì lấy giá trị hiện tại
            {
                TblCurrency cObj = SelectByID(CurrencyID);
                RMValue = cObj != null ? cObj.RMValue : 0;
            }
            return RMValue;
        }
    }

    public class SupplyManager : IReference<TblSupply>
    {
        public TblSupply Insert(TblSupply obj)
        {
            return new TblSupplyController().Insert(obj);
        }

        public TblSupply Update(TblSupply obj)
        {
            return new TblSupplyController().Update(obj);
        }

        public TblSupply SelectByID(short id)
        {
            return new Select().From(TblSupply.Schema)
              .Where(TblSupply.Columns.SupplyID).IsEqualTo(id)
              .OrderAsc(TblSupply.Columns.SupplyName).ExecuteSingle<TblSupply>();
        }

        public bool IsBeingUsed(short id)
        {
            bool IsUsedByJobSheet = new Select().From(TblJobSheet.Schema).Where(TblJobSheet.SupplyIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            if (IsUsedByJobSheet)
                return true;
            return false;
        }

        public bool Delete(short id)
        {
            return new TblSupplyController().Delete(id);
        }

        public bool Exist(short id)
        {
            return new Select().From(TblSupply.Schema)
             .Where(TblSupply.SupplyIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblSupply.Schema)
              .Where(TblSupply.SupplyIDColumn).IsNotEqualTo(id)
              .And(TblSupply.SupplyNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblSupplySelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("SupplyID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["SupplyID"] = randID;
                r["SupplyName"] = "";
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static List<TblSupply> SelectForDDL()
        {
            List<TblSupply> list = new List<TblSupply>();
            list = new Select().From(TblSupply.Schema)
                            .Where(TblSupply.IsObsoleteColumn).IsEqualTo(false)
                            .OrderAsc(TblSupply.Columns.SupplyName).ExecuteAsCollection<TblSupplyCollection>().ToList<TblSupply>();
            TblSupply obj = new TblSupply();
            obj.SupplyID = 0;
            obj.SupplyName = "--Select supply--";
            obj.IsObsolete = false;
            list.Insert(0, obj);
            return list;
        }
    }

    public class TaxManager : IReference<TblTax>
    {
        public TblTax Insert(TblTax obj)
        {
            return new TblTaxController().Insert(obj);
        }

        public TblTax Update(TblTax obj)
        {
            return new TblTaxController().Update(obj);
        }

        public TblTax SelectByID(short id)
        {
            return new Select().From(TblTax.Schema)
              .Where(TblTax.Columns.TaxID).IsEqualTo(id)
              .OrderAsc(TblTax.Columns.TaxName).ExecuteSingle<TblTax>();
        }

        public bool IsBeingUsed(short id)
        {
            bool IsUsedByCylinder = new Select().From(TblCylinder.Schema)
                .Where(TblCylinder.TaxIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            if (IsUsedByCylinder)
                return true;
            return false;
        }

        public bool Delete(short id)
        {
            return new TblTaxController().Delete(id);
        }

        public bool Exist(short id)
        {
            return new Select().From(TblTax.Schema)
             .Where(TblTax.TaxIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblTax.Schema)
              .Where(TblTax.TaxIDColumn).IsNotEqualTo(id)
              .And(TblTax.TaxNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public bool ExistTaxCode(short id, string taxCode)
        {
            return new Select().From(TblTax.Schema)
              .Where(TblTax.TaxIDColumn).IsNotEqualTo(id)
              .And(TblTax.TaxCodeColumn).IsEqualTo(taxCode).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblTaxSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("TaxID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["TaxID"] = randID;
                r["TaxName"] = "";
                r["TaxCode"] = "";
                r["TaxPercentage"] = 0;
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        /// <summary>
        /// Select tax for dropdownlist
        /// </summary>
        /// <param name="SelectAll">True - Select all; Fasle - Default Overseas Tax</param>
        /// <returns></returns>
        public List<TblTax> SelectAllForDDL(bool SelectAll)
        {
            List<TblTax> list = new List<TblTax>();
            if (SelectAll)//Nếu select all = true thì lấy tất cả
            {
                list = new SubSonic.Select().From(TblTax.Schema).Where(TblTax.IsObsoleteColumn).IsEqualTo(false).ExecuteAsCollection<TblTaxCollection>().ToList();
            }
            else//Ngược lại chỉ lấy thuế mặc định của overseas
            {
                TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.DefaultTaxForOverseasSetting);
                short TaxID = setting != null ? Convert.ToInt16(setting.SettingValue) : (short)0;
                TblTax obj = new TaxManager().SelectByID(TaxID);
                if (obj != null)
                    list.Add(obj);
            }

            TblTax newObj = new TblTax();
            newObj.TaxID = 0;
            newObj.TaxCode = "-----";
            newObj.TaxName = "--Select tax--";
            newObj.TaxPercentage = 0;
            list.Insert(0, newObj);
            return list;
        }

    }

    public class SupplierManager
    {
        public TblSupplier Insert(TblSupplier obj)
        {
            return new TblSupplierController().Insert(obj);
        }

        public TblSupplier Update(TblSupplier obj)
        {
            return new TblSupplierController().Update(obj);
        }

        public TblSupplier SelectByID(int id)
        {
            return new Select().From(TblSupplier.Schema)
              .Where(TblSupplier.Columns.SupplierID).IsEqualTo(id)
              .OrderAsc(TblSupplier.Columns.Name).ExecuteSingle<TblSupplier>();
        }

        public TblSupplierCollection GetAllSupplier(bool isObsolete) {
            return new Select().From(TblSupplier.Schema).Where(TblSupplier.IsObsoleteColumn).IsEqualTo(isObsolete).ExecuteAsCollection<TblSupplierCollection>();
        }

        public bool IsBeingUsed(int id)
        {
            bool IsUsedByPurchaseOrder = new Select().From(TblPurchaseOrder.Schema)
                .Where(TblPurchaseOrder.SupplierIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            if (IsUsedByPurchaseOrder)
                return true;
            return false;
        }

        public bool Delete(int id)
        {
            return new TblSupplierController().Delete(id);
        }

        public bool Exist(int id)
        {
            return new Select().From(TblSupplier.Schema)
             .Where(TblSupplier.SupplierIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(int id, string name)
        {
            return new Select().From(TblSupplier.Schema)
              .Where(TblSupplier.SupplierIDColumn).IsNotEqualTo(id)
              .And(TblSupplier.NameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblSupplierSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<int>("SupplierID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["SupplierID"] = randID;
                r["Name"] = "";
                r["Address"] = "";
                r["Tel"] = "";
                r["Fax"] = "";
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        /// <summary>
        /// Select supplier for ddl
        /// </summary>
        /// <returns></returns>
        public static List<TblSupplier> SelectSupllierForDDL()
        {
            List<TblSupplier> list = new List<TblSupplier>();
            list = new Select().From(TblSupplier.Schema).Where(TblSupplier.IsObsoleteColumn).IsEqualTo(false).OrderAsc(TblSupplier.Columns.Name).ExecuteAsCollection<TblSupplierCollection>().ToList();
            TblSupplier obj = new TblSupplier();
            obj.Name = "--Select supplier--";
            obj.SupplierID = 0;
            list.Insert(0, obj);
            return list;
        }
    }

    public class CylinderStatusManager : IReference<TblCylinderStatus>
    {
        public TblCylinderStatus Insert(TblCylinderStatus obj)
        {
            return new TblCylinderStatusController().Insert(obj);
        }

        public TblCylinderStatus Update(TblCylinderStatus obj)
        {
            return new TblCylinderStatusController().Update(obj);
        }

        public TblCylinderStatus SelectByID(short id)
        {
            return new Select().From(TblCylinderStatus.Schema)
              .Where(TblCylinderStatus.Columns.CylinderStatusID).IsEqualTo(id)
              .OrderAsc(TblCylinderStatus.Columns.CylinderStatusName).ExecuteSingle<TblCylinderStatus>();
        }

        public bool IsBeingUsed(short id)
        {
            bool IsUsedByPurchaseOrder = new Select().From(TblCylinder.Schema)
                .Where(TblCylinder.CylinderStatusIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            if (IsUsedByPurchaseOrder)
                return true;
            return false;
        }

        public bool Delete(short id)
        {
            return new TblCylinderStatusController().Delete(id);
        }

        public bool Exist(short id)
        {
            return new Select().From(TblCylinderStatus.Schema)
             .Where(TblCylinderStatus.CylinderStatusIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblCylinderStatus.Schema)
              .Where(TblCylinderStatus.CylinderStatusIDColumn).IsNotEqualTo(id)
              .And(TblCylinderStatus.CylinderStatusNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCylinderStatusSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("CylinderStatusID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["CylinderStatusID"] = randID;
                r["CylinderStatusName"] = "";
                r["Action"] = "";
                r["Invoice"] = 0;
                r["Physical"] = 0;
                r["IsObsolete"] = 0;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static List<CylinderStatusAction> SelectActionsForDDL()
        {
            List<CylinderStatusAction> list = new List<CylinderStatusAction>();
            list = Enum.GetValues(typeof(CylinderStatusAction)).Cast<CylinderStatusAction>().ToList();
            return list;
        }

        public static TblCylinderStatusCollection SelectForDDL()
        {
            return new Select().From(TblCylinderStatus.Schema)
                                .Where(TblCylinderStatus.IsObsoleteColumn).IsEqualTo(false)
                                .OrderAsc(TblCylinderStatus.Columns.CylinderStatusName).ExecuteAsCollection<TblCylinderStatusCollection>();
        }

        public static TblCylinderStatus SelectCylinderStatusByID(short p)
        {
            return new Select().From(TblCylinderStatus.Schema).Where(TblCylinderStatus.CylinderStatusIDColumn).IsEqualTo(p).ExecuteSingle<TblCylinderStatus>();
        }
    }

    public class BackingManager : IReference<TblBacking>
    {
        public TblBacking Insert(TblBacking obj)
        {
            return new TblBackingController().Insert(obj);
        }

        public TblBacking Update(TblBacking obj)
        {
            return new TblBackingController().Update(obj);
        }

        public TblBacking SelectByID(short id)
        {
            return new Select().From(TblBacking.Schema)
              .Where(TblBacking.Columns.BackingID).IsEqualTo(id)
              .OrderAsc(TblBacking.Columns.BackingName).ExecuteSingle<TblBacking>();
        }

        public bool IsBeingUsed(short id)
        {
            bool IsUsedByJobSheet = new Select().From(TblJobSheet.Schema)
                .Where(TblJobSheet.BackingIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            if (IsUsedByJobSheet)
                return true;
            return false;
        }

        public bool Delete(short id)
        {
            return new TblBackingController().Delete(id);
        }

        public bool Exist(short id)
        {
            return new Select().From(TblBacking.Schema)
             .Where(TblBacking.BackingIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblBacking.Schema)
              .Where(TblBacking.BackingIDColumn).IsNotEqualTo(id)
              .And(TblBacking.BackingNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblBackingSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("BackingID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["BackingID"] = randID;
                r["BackingName"] = "";
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static List<TblBacking> SelectForDDL()
        {
            List<TblBacking> list = new List<TblBacking>();
            list = new Select().From(TblBacking.Schema)
                            .Where(TblBacking.IsObsoleteColumn).IsEqualTo(false)
                            .OrderAsc(TblBacking.Columns.BackingName).ExecuteAsCollection<TblBackingCollection>().ToList<TblBacking>();
            TblBacking obj = new TblBacking();
            obj.BackingID = 0;
            obj.BackingName = "--Select backing--";
            obj.IsObsolete = false;
            list.Insert(0, obj);
            return list;
        }
    }

    public class PricingManager : IReference<TblPricing>
    {
        public TblPricing Insert(TblPricing obj)
        {
            return new TblPricingController().Insert(obj);
        }

        public TblPricing Update(TblPricing obj)
        {
            return new TblPricingController().Update(obj);
        }

        public TblPricing SelectByID(short id)
        {
            return new Select().From(TblPricing.Schema)
              .Where(TblPricing.Columns.PricingID).IsEqualTo(id)
              .OrderAsc(TblPricing.Columns.PricingName).ExecuteSingle<TblPricing>();
        }

        public bool IsBeingUsed(short id)
        {
            bool IsUsedByJobSheet = new Select().From(TblCylinder.Schema)
                .Where(TblCylinder.PricingIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            bool IsUsedByQuotation = new Select().From(TblCustomerQuotationPricing.Schema)
                .Where(TblCustomerQuotationPricing.PricingIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
            if (IsUsedByQuotation || IsUsedByJobSheet)
                return true;
            return false;
        }

        public bool Delete(short id)
        {
            return new TblPricingController().Delete(id);
        }

        public bool Exist(short id)
        {
            return new Select().From(TblPricing.Schema)
             .Where(TblPricing.PricingIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblPricing.Schema)
              .Where(TblPricing.PricingIDColumn).IsNotEqualTo(id)
              .And(TblPricing.PricingNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblPricingSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("PricingID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["PricingID"] = randID;
                r["PricingName"] = "";
                r["IsObsolete"] = false;
                r["ForTobaccoCustomers"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static TblPricingCollection SelectForDDL()
        {
            return new Select().From(TblPricing.Schema)
                        .Where(TblPricing.IsObsoleteColumn).IsEqualTo(false)
                        .OrderAsc(TblPricing.Columns.PricingName).ExecuteAsCollection<TblPricingCollection>();
        }

        public static TblPricingCollection SelectAll()
        {
            return new TblPricingController().FetchAll();
        }

        public static bool CheckForTobaccoCustomers(short pricingID)
        {
            return new SubSonic.Select().From(TblPricing.Schema).Where(TblPricing.PricingIDColumn).IsEqualTo(pricingID).And(TblPricing.ForTobaccoCustomersColumn).IsEqualTo(true).GetRecordCount() > 0 ? true : false;
        }

        public static TblPricing SelectByPricingID(short p)
        {
            return new Select(TblPricing.PricingNameColumn).From(TblPricing.Schema).Where(TblPricing.PricingIDColumn).IsEqualTo(p).ExecuteSingle<TblPricing>();
        }
    }

    public class ProgressReproStatusManager : IReference<TblProgressReproStatus>
    {
        public TblProgressReproStatus Insert(TblProgressReproStatus obj)
        {
            return new TblProgressReproStatusController().Insert(obj);
        }

        public TblProgressReproStatus Update(TblProgressReproStatus obj)
        {
            return new TblProgressReproStatusController().Update(obj);
        }

        public TblProgressReproStatus SelectByID(short id)
        {
            return new Select().From(TblProgressReproStatus.Schema)
              .Where(TblProgressReproStatus.Columns.ReproStatusID).IsEqualTo(id)
              .OrderAsc(TblProgressReproStatus.Columns.ReproStatusName).ExecuteSingle<TblProgressReproStatus>();
        }

        public bool IsBeingUsed(short id)
        {
            bool IsUsedByJobSheet = new Select().From(TblProgress.Schema)
                .Where(TblProgress.ReproStatusIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            if (IsUsedByJobSheet)
                return true;
            return false;
        }

        public bool Delete(short id)
        {
            return new TblProgressReproStatusController().Delete(id);
        }

        public bool Exist(short id)
        {
            return new Select().From(TblProgressReproStatus.Schema)
             .Where(TblProgressReproStatus.ReproStatusIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblProgressReproStatus.Schema)
              .Where(TblProgressReproStatus.ReproStatusIDColumn).IsNotEqualTo(id)
              .And(TblProgressReproStatus.ReproStatusNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblReproStatusSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("ReproStatusID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["ReproStatusID"] = randID;
                r["ReproStatusName"] = "";
                r["Description"] = string.Empty;
                r["GoBackToRepro"] = false;
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static List<TblProgressReproStatus> SelectForDDL()
        {
            List<TblProgressReproStatus> list = new List<TblProgressReproStatus>();
            list = new Select().From(TblProgressReproStatus.Schema)
                        .Where(TblProgressReproStatus.IsObsoleteColumn).IsEqualTo(false)
                        .OrderAsc(TblProgressReproStatus.Columns.ReproStatusName)
                        .ExecuteAsCollection<TblProgressReproStatusCollection>()
                        .ToList<TblProgressReproStatus>();
            TblProgressReproStatus obj = new TblProgressReproStatus();
            obj.ReproStatusID = 0;
            obj.ReproStatusName = "--Select status--";
            obj.Description = string.Empty;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblProgressReproStatus> SelectForReProDDL()
        {
            List<TblProgressReproStatus> list = new List<TblProgressReproStatus>();
            list = new Select().From(TblProgressReproStatus.Schema)
                        .Where(TblProgressReproStatus.IsObsoleteColumn).IsEqualTo(false)
                        .And(TblProgressReproStatus.GoBackToReproColumn).IsEqualTo(true)
                        .OrderAsc(TblProgressReproStatus.Columns.ReproStatusName)
                        .ExecuteAsCollection<TblProgressReproStatusCollection>()
                        .ToList<TblProgressReproStatus>();
            TblProgressReproStatus obj = new TblProgressReproStatus();
            obj.ReproStatusID = 0;
            obj.ReproStatusName = "--Select status--";
            obj.Description = string.Empty;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblProgressReproStatus> SelectForEngravingDDL()
        {
            List<TblProgressReproStatus> list = new List<TblProgressReproStatus>();
            list = new Select().From(TblProgressReproStatus.Schema)
                        .Where(TblProgressReproStatus.IsObsoleteColumn).IsEqualTo(false)
                        .And(TblProgressReproStatus.GoBackToReproColumn).IsEqualTo(false)
                        .OrderAsc(TblProgressReproStatus.Columns.ReproStatusName)
                        .ExecuteAsCollection<TblProgressReproStatusCollection>()
                        .ToList<TblProgressReproStatus>();
            TblProgressReproStatus obj = new TblProgressReproStatus();
            obj.ReproStatusID = 0;
            obj.ReproStatusName = "--Select status--";
            obj.Description = string.Empty;
            list.Insert(0, obj);
            return list;
        }
    }

    public class ProgressCylinderStatusManager : IReference<TblProgressCylinderStatus>
    {
        public TblProgressCylinderStatus Insert(TblProgressCylinderStatus obj)
        {
            return new TblProgressCylinderStatusController().Insert(obj);
        }

        public TblProgressCylinderStatus Update(TblProgressCylinderStatus obj)
        {
            return new TblProgressCylinderStatusController().Update(obj);
        }

        public TblProgressCylinderStatus SelectByID(short id)
        {
            return new Select().From(TblProgressCylinderStatus.Schema)
              .Where(TblProgressCylinderStatus.Columns.CylinderStatusID).IsEqualTo(id)
              .OrderAsc(TblProgressCylinderStatus.Columns.CylinderStatusName).ExecuteSingle<TblProgressCylinderStatus>();
        }

        public bool IsBeingUsed(short id)
        {
            bool IsUsedByJobSheet = new Select().From(TblJobSheet.Schema)
                .Where(TblProgress.CylinderStatusIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;

            if (IsUsedByJobSheet)
                return true;
            return false;
        }

        public bool Delete(short id)
        {
            return new TblProgressCylinderStatusController().Delete(id);
        }

        public bool Exist(short id)
        {
            return new Select().From(TblProgressCylinderStatus.Schema)
             .Where(TblProgressCylinderStatus.CylinderStatusIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public bool Exist(short id, string name)
        {
            return new Select().From(TblProgressCylinderStatus.Schema)
              .Where(TblProgressCylinderStatus.CylinderStatusIDColumn).IsNotEqualTo(id)
              .And(TblProgressCylinderStatus.CylinderStatusNameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }

        public DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblProgressCylinderStatusSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("CylinderStatusID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["CylinderStatusID"] = randID;
                r["CylinderStatusName"] = "";
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static List<TblProgressCylinderStatus> SelectForDDL()
        {
            List<TblProgressCylinderStatus> list = new List<TblProgressCylinderStatus>();
            list = new Select().From(TblProgressCylinderStatus.Schema)
                        .Where(TblProgressCylinderStatus.IsObsoleteColumn).IsEqualTo(false)
                        .OrderAsc(TblProgressCylinderStatus.Columns.CylinderStatusName)
                        .ExecuteAsCollection<TblProgressCylinderStatusCollection>()
                        .ToList<TblProgressCylinderStatus>();
            TblProgressCylinderStatus obj = new TblProgressCylinderStatus();
            obj.CylinderStatusID = 0;
            obj.CylinderStatusName = "--Select a status--";
            obj.Description = string.Empty;
            list.Insert(0, obj);
            return list;
        }
    }
}
