using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class CustomerManager
    {
        /// <summary>
        /// Kiểm tra mã khách hàng tồn tại chưa
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static bool Exists(int ID, string Code)
        {
            return new Select().From(TblCustomer.Schema)
                .Where(TblCustomer.CustomerIDColumn).IsNotEqualTo(ID)
                .And(TblCustomer.CodeColumn).IsEqualTo(Code).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra khách hàng có tồn tại không
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Exists(int ID)
        {
            return new Select().From(TblCustomer.Schema)
                .Where(TblCustomer.CustomerIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        public static bool IsBeingUsed(int ID)
        {
            //Kiểm tra Contact
            bool IsUsedByContact = false;
            List<TblContact> cList = new List<TblContact>();
            cList = ContactManager.SelectAll(ID, false);
            foreach(TblContact c in cList)
            {
                if (ContactManager.IsBeingUsed(c.ContactID))
                {
                    IsUsedByContact = true;
                    break;
                }
            }

            //Kiểm tra Pricing Master
            int CustomerID = 0;
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PricingMasterTemplateSetting);
            if (setting != null)
            {
                CustomerID = Convert.ToInt32(setting.SettingValue);
                if (CustomerID == ID)
                    return true;
            }

            //Kiểm tra Job
            bool IsUsedByJob = new Select().From(TblJob.Schema).Where(TblJob.CustomerIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;

            if (IsUsedByContact || IsUsedByJob)
                return true;
            return false;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblCustomer Insert(TblCustomer obj)
        {
            return new TblCustomerController().Insert(obj);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblCustomer Update(TblCustomer obj)
        {
            return new TblCustomerController().Update(obj);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static bool Delete(int ID)
        {
            //Xóa trong CustomerQuotationPricing
            new Delete().From(TblCustomerQuotationPricing.Schema)
                .Where(TblCustomerQuotationPricing.CustomerIDColumn).IsEqualTo(ID).Execute();
            //Xóa trong CustomerQuotation
            new Delete().From(TblCustomerQuotation.Schema)
                .Where(TblCustomerQuotation.CustomerIDColumn).IsEqualTo(ID).Execute();
            //Xóa trong contact
            new Delete().From(TblContact.Schema)
                .Where(TblContact.CustomerIDColumn).IsEqualTo(ID).Execute();
            return new TblCustomerController().Delete(ID);   
        }

        /// <summary>
        /// Select by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblCustomer SelectByID(int ID)
        {
            return new Select().From(TblCustomer.Schema)
                .Where(TblCustomer.CustomerIDColumn).IsEqualTo(ID)
                .OrderAsc(TblCustomer.Columns.Name).ExecuteSingle<TblCustomer>();
        }

        /// <summary>
        /// Select list for DDL
        /// </summary>
        /// <returns></returns>
        public static TblCustomerCollection ListForDDL()
        {
            Select select = new Select(TblCustomer.CustomerIDColumn, TblCustomer.NameColumn);
            select.From<TblCustomer>();
            select.Where(TblCustomer.IsObsoleteColumn).IsEqualTo(false);
            var Colls = select.ExecuteAsCollection<TblCustomerCollection>();
            return Colls;
        }

        /// <summary>
        /// Get all department
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="IsActive"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCustomerSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public static List<TblCustomer> SelectByKeyword(string Keyword)
        {
            Keyword = "%" + Keyword + "%";
            List<TblCustomer> list = new List<TblCustomer>();
            list = new Select().From(TblCustomer.Schema)
                    .Where(TblCustomer.Columns.Name).Like(Keyword)
                    .Or(TblCustomer.Columns.Code).Like(Keyword)
                    .AndExpression(TblCustomer.Columns.IsObsolete).IsEqualTo(false)
                    .OrderAsc(TblCustomer.Columns.Code)
                    .ExecuteAsCollection<TblCustomerCollection>().ToList();
            return list;
        }

        public static List<TblCustomer> SelectBrandOwnerByKeyword(string Keyword)
        {
            Keyword = "%" + Keyword + "%";
            List<TblCustomer> list = new List<TblCustomer>();
            var result = new Select().From(TblCustomer.Schema)
                //.Where(TblCustomer.IsObsoleteColumn).IsEqualTo(false)
                    .Where(TblCustomer.Columns.Name).Like(Keyword)
                    .Or(TblCustomer.Columns.Code).Like(Keyword)
                    .AndExpression(TblCustomer.Columns.IsObsolete).IsEqualTo(false)
                    .And(TblCustomer.IsBrandColumn).IsEqualTo(true)
                    .OrderAsc(TblCustomer.Columns.Code)
                    .ExecuteAsCollection<TblCustomerCollection>();
            foreach (var item in result)
            {
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Select all customer for report
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="IsActive"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable SelectAllForReport(string KeyWord, bool? IsActive, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblCustomerSelectForReport(KeyWord, IsActive, SortColumn, SortType).GetReader());
            return dt;
        }

        public static bool EmailExists(int ID, string Email,short isBrand)
        {
            return new Select()
                .From(TblCustomer.Schema)
                .Where(TblCustomer.EmailColumn).IsEqualTo(Email)
                .And(TblCustomer.IsBrandColumn).IsEqualTo(isBrand)
                .And(TblCustomer.CustomerIDColumn).IsNotEqualTo(ID)
                .GetRecordCount() > 0 ? true : false;
        }

        public static List<TblCustomer> SelectCustomerForDDL()
        {
            List<TblCustomer> list = new List<TblCustomer>();
            list = new Select().From(TblCustomer.Schema)
                                .Where(TblCustomer.IsObsoleteColumn).IsEqualTo(false)
                                .OrderAsc(TblCustomer.Columns.Name)
                                .ExecuteAsCollection<TblCustomerCollection>().ToList();
            TblCustomer obj = new TblCustomer();
            obj.Name = "--Select customer--";
            obj.CustomerID = 0;
            list.Insert(0, obj);
            return list;
        }
    }

    public enum TypeCustomerHelper { 
        Customer = 0,
        Brand = 1
    }
}
