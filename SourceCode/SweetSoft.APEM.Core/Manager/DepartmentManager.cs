using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class DepartmentManager
    {
        /// <summary>
        /// Kiểm tra đối tượng có tồn tại không
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static bool Exists(short ID, string Name)
        {
            return new Select().From(TblDepartment.Schema)
                .Where(TblDepartment.DepartmentIDColumn).IsNotEqualTo(ID)
                .And(TblDepartment.DepartmentNameColumn).IsEqualTo(Name).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra đối tượng có tồn tại không
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Exists(short ID)
        {
            return new Select().From(TblDepartment.Schema)
                .Where(TblDepartment.DepartmentIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra đối tượng có đang được sử dụng không?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool IsBeingUsed(short ID)
        {
            //Kiểm tra User
            bool IsUsedByStaff = new Select().From(TblStaff.Schema).Where(TblStaff.DepartmentIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;

            if (IsUsedByStaff)
                return true;
            return false;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblDepartment Insert(TblDepartment obj)
        {
            return new TblDepartmentController().Insert(obj);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblDepartment Update(TblDepartment obj)
        {
            return new TblDepartmentController().Update(obj);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static bool Delete(short ID)
        {
            return new TblDepartmentController().Delete(ID);
        }

        /// <summary>
        /// Select by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblDepartment SelectByID(short ID)
        {
            return new Select().From(TblDepartment.Schema)
                .Where(TblDepartment.DepartmentIDColumn).IsEqualTo(ID)
                .OrderAsc(TblDepartment.Columns.DepartmentName).ExecuteSingle<TblDepartment>();
        }

        /// <summary>
        /// Select list for DDL
        /// </summary>
        /// <returns></returns>
        public static TblDepartmentCollection ListForDDL()
        {
            Select select = new Select(TblDepartment.DepartmentIDColumn, TblDepartment.DepartmentNameColumn);
            select.From<TblDepartment>();
            select.Where(TblDepartment.IsObsoleteColumn).IsEqualTo(false);
            var Colls = select.ExecuteAsCollection<TblDepartmentCollection>();
            TblDepartment obj = new TblDepartment();
            obj.DepartmentID = 0;
            obj.DepartmentName = "--Select a department--";
            Colls.Insert(0, obj);
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
        public static DataTable SelectAll(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblDepartmentSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<short>("DepartmentID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["DepartmentID"] = randID;
                r["DepartmentName"] = "";
                r["ShowInWorkFlow"] = 1;
                r["ProcessType"] = string.Empty;
                r["ProcessTypeID"] = DBNull.Value;
                r["ProductType"] = string.Empty;
                r["ProductTypeID"] = string.Empty;
                //r["IsTimeline"] = true;
                r["TimelineOrder"] = 0;
                //r["Description"] = "";
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static TblDepartmentCollection GetDeptForWorkflow()
        {
            Select select = new Select();
            select.From(TblDepartment.Schema);
            select.Where(TblDepartment.ShowInWorkFlowColumn).IsEqualTo(true)
                .And(TblDepartment.IsObsoleteColumn).IsEqualTo(false);
            return select.ExecuteAsCollection<TblDepartmentCollection>();
        }

    }
}
