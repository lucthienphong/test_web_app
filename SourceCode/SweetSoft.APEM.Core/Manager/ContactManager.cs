using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class ContactManager
    {
        public static bool IsBeingUsed(int ID)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static List<TblContact> SelectAll(int CustomerID, bool AddNew)
        {
            List<TblContact> list = new List<TblContact>();
            TblContactCollection colls = new Select().From(TblContact.Schema).Where(TblContact.CustomerIDColumn).IsEqualTo(CustomerID).ExecuteAsCollection<TblContactCollection>();
            foreach (TblContact c in colls)
                list.Add(c);
            if (AddNew)
            {
                TblContact obj = new TblContact();
                obj.ContactID = -1;
                obj.ContactName = "";
                obj.Honorific = "";
                obj.Designation = "";
                obj.Email = "";
                obj.Tel = "";
                obj.CustomerID = CustomerID;
                list.Insert(0, obj);
            }
            return list;
        }

        /// <summary>
        /// Lấy danh sách contact theo khách hàng
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static List<TblContact> SelectAll(int CustomerID)
        {
            List<TblContact> list = new List<TblContact>();
            TblContactCollection colls = new Select().From(TblContact.Schema)
                .Where(TblContact.CustomerIDColumn).IsEqualTo(CustomerID)
                .OrderAsc(TblContact.Columns.ContactName)
                .ExecuteAsCollection<TblContactCollection>();
            foreach (TblContact c in colls)
                list.Add(c);
            TblContact obj = new TblContact();
            obj.ContactID = 0;
            obj.ContactName = "--Select a contact--";
            obj.Honorific = "";
            obj.Designation = "";
            obj.CustomerID = CustomerID;
            list.Insert(0, obj);

            return list;
        }


        public static List<TblContact> SelectAllByCustomerID(int CustomerID)
        {
            List<TblContact> list = new List<TblContact>();
            list = new Select()
                .From(TblContact.Schema)
                .Where(TblContact.CustomerIDColumn).IsEqualTo(CustomerID)
                .ExecuteAsCollection<TblContactCollection>().ToList();
            if (list.Count == 0)
            {
                TblContact obj = new TblContact();
                obj.ContactID = 0;
                obj.ContactName = "--Select contact person--";
                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// Select by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblContact SelectByID(int ID)
        {
            return new Select().From(TblContact.Schema).Where(TblContact.ContactIDColumn).IsEqualTo(ID).ExecuteSingle<TblContact>();
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblContact Insert(TblContact obj)
        {
            return new TblContactController().Insert(obj);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblContact Update(TblContact obj)
        {
            return new TblContactController().Update(obj);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static bool Delete(int ID)
        {
            TblContact obj = (new Select().From(TblContact.Schema).Where(TblContact.ContactIDColumn).IsEqualTo(ID).ExecuteSingle<TblContact>());
            if (obj != null)
            {
                return new TblContactController().Delete(ID);
            }
            return false;
        }

        public static TblContactCollection GetAllContact()
        {
            return new Select().From(TblContact.Schema).ExecuteAsCollection<TblContactCollection>();
        }
    }
}
