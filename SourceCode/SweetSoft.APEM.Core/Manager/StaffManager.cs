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
    public enum DepartmentSetting
    {
        All,
        Sale,
        JobCoordinator
    }

    public class TblStaffExeption : TblStaff
    {
        //Phần bổ sung
        public TblStaff Parent { get; set; }
        //Chuyển Parents -> Children
        public TblStaffExeption() { }
        public TblStaffExeption(TblStaff parent)
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
        public string FullName { set; get; }
    }

    public class StaffManager
    {
        public static string StaffNo()
        {
            string _No = "ID-";
            string _MaxNumber = new Select(Aggregate.Max(TblStaff.StaffNoColumn)).From(TblStaff.Schema).ExecuteScalar<string>();
            _MaxNumber = _MaxNumber ?? "";
            if (_MaxNumber.Length > 0)
            {
                string _righ = (int.Parse(_MaxNumber.Substring(_MaxNumber.Length - 4, 4)) + 1).ToString();
                while (_righ.Length < 4)
                    _righ = "0" + _righ;
                _No += _righ;

            }
            else
                _No += "0001";
            return _No;
        }

        /// <summary>
        /// Kiểm tra email nhân viên có tồn tại không?
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool StaffEmailExists(int ID, string Email)
        {
            return new Select().From(TblStaff.Schema)
                .Where(TblStaff.StaffIDColumn).IsNotEqualTo(ID)
                .And(TblStaff.Columns.Email).IsEqualTo(Email).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra mã nhân viên có tồn tại không?
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="StaffNo"></param>
        /// <returns></returns>
        public static bool StaffNoExists(int ID, string StaffNo)
        {
            return new Select().From(TblStaff.Schema)
                .Where(TblStaff.StaffIDColumn).IsNotEqualTo(ID)
                .And(TblStaff.Columns.StaffNo).IsEqualTo(StaffNo).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra nhân viên có tồn tại không?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Exists(int ID)
        {
            return new Select().From(TblStaff.Schema)
                .Where(TblStaff.StaffIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra nhân viên có được sử dụng không?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool IsBeingUsed(int ID)
        {
            //Kiểm tra User
            bool IsUsedByUser = new Select().From(TblUser.Schema).Where(TblUser.UserIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;

            if (IsUsedByUser)
                return true;
            return false;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblStaff Insert(TblStaff obj)
        {
            return new TblStaffController().Insert(obj);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblStaff Update(TblStaff obj)
        {
            return new TblStaffController().Update(obj);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static bool Delete(int ID)
        {
            TblStaff obj = (new Select().From(TblStaff.Schema).Where(TblStaff.StaffIDColumn).IsEqualTo(ID).ExecuteSingle<TblStaff>());
            if (obj != null)
            {
                bool userDeleted = UserManager.Delete(ID);
                bool staffDeleted = new TblStaffController().Delete(ID);
                if (userDeleted && staffDeleted)
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Lấy họ tên nhân viên theo tên đăng nhập
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static string GetStaffFullNameByUserName(string UserName)
        {

            TblStaff sObj = new Select().From(TblStaff.Schema)
                                    .InnerJoin(TblUser.UserIDColumn, TblStaff.StaffIDColumn)
                                    .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                                    .ExecuteSingle<TblStaff>();
            string FullName = sObj.FirstName + " " + sObj.LastName;
            return FullName;
        }

        /// <summary>
        /// Select by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblStaff SelectByID(int ID)
        {
            return new Select().From(TblStaff.Schema).Where(TblStaff.StaffIDColumn).IsEqualTo(ID).ExecuteSingle<TblStaff>();
        }

        /// <summary>
        /// Select list for DDL
        /// </summary>
        /// <returns></returns>
        public static List<TblStaffExeption> ListForDDL(DepartmentSetting Department)
        {
            Select select = new Select(TblStaff.StaffIDColumn, TblStaff.StaffNoColumn, TblStaff.LastNameColumn, TblStaff.FirstNameColumn, TblStaff.EmailColumn);
            select.From<TblStaff>();
            select.Where(TblStaff.IsObsoleteColumn).IsEqualTo(false);
            TblSystemSetting setting = new TblSystemSetting();
            short DepartmentID = 0;
            switch (Department)
            {
                case DepartmentSetting.All:
                    break;
                case DepartmentSetting.Sale:
                    setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SaleRepSetting);
                    DepartmentID = setting != null ? Convert.ToInt16(setting.SettingValue) : (short)0;
                    select.And(TblStaff.DepartmentIDColumn).IsEqualTo(DepartmentID);
                    break;
                case DepartmentSetting.JobCoordinator:
                    setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.JobCoordinatorSetting);
                    DepartmentID = setting != null ? Convert.ToInt16(setting.SettingValue) : (short)0;
                    select.And(TblStaff.DepartmentIDColumn).IsEqualTo(DepartmentID);
                    break;
            }


            select.OrderAsc(TblStaff.Columns.StaffNo);
            var Colls = select.ExecuteAsCollection<TblStaffCollection>();
            List<TblStaffExeption> listStaff = new List<TblStaffExeption>();
            TblStaffExeption sObj = new TblStaffExeption();
            sObj.FullName = "--Select a staff--";
            sObj.StaffID = 0;
            listStaff.Insert(0, sObj);

            foreach (var item in Colls)
            {
                sObj = new TblStaffExeption(item);
                sObj.FullName = string.Format("{0}--{1} {2}", item.StaffNo, item.FirstName, item.LastName);
                listStaff.Add(sObj);
            }

            return listStaff;
        }

        /// <summary>
        /// Get all staff
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="IsActive"></param>
        /// <param name="DepartmentID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable SelectAll(string KeyWord, bool? IsActive, short DepartmentID, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblStaffSelectAll(KeyWord, IsActive, DepartmentID, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }
    }
}
