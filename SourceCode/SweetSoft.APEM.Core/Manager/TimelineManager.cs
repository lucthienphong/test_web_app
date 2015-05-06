using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;
using System.Data;
using System.ComponentModel;

namespace SweetSoft.APEM.Core.Manager
{
    public class TimelineManager
    {
        #region Timeline Order

        /// <summary>
        /// Hàm lấy dang sách công việc
        /// </summary>
        /// <returns></returns>
        public static TblJobCollection GetJobList()
        {
            Select select = new Select();
            select.From(TblJob.Schema).Where(TblJob.CustomerIDColumn).IsNotNull()
                .And(TblJob.IsClosedColumn).IsEqualTo(0)
                .And(TblJob.IsServiceJobColumn).IsEqualTo(0);

            select.OrderDesc(TblJob.Columns.CreatedOn);
            return select.ExecuteAsCollection<TblJobCollection>();
        }

        public static DataTable ToDataTable<T>(IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// Hàm lấy dang sách cylinder
        /// </summary>
        /// <returns></returns>
        public static List<CylinderTimelineObject> GetCylinderList()
        {
            return GetCylinderList(string.Empty);
        }

        /// <summary>
        /// Hàm lấy dang sách cylinder
        /// </summary>
        /// <returns></returns>
        public static List<CylinderTimelineObject> GetCylinderList(string departmentId)
        {
            List<CylinderTimelineObject> lst = new List<CylinderTimelineObject>();
            string template = @"select DISTINCT j.JobNumber, j.JobName, c.CylinderNo, c.CylinderBarcode,c.CylinderID 
	from tblCylinderProcessing cpr INNER JOIN
		tblCylinder c ON cpr.CylinderID = c.CylinderID INNER JOIN
		tblJob j ON c.JobID = j.JobID
	where cpr.CylinderID not in ( select cp.CylinderID from tblCylinderProcessing cp
	where (cp.DepartmentID = (select top 1 ISNULL(wEnd.DepartmentID,0)
								from tblWorkFlowNode wEnd /*INNER JOIN
									tblWorkFlowNode wNEnd ON wEnd.WorkFlowListFromConnection = wNEnd.ID*/
								where LEN(wEnd.WorkFlowListToConnection) = 0) and FinishedOn IS NOT NULL))
	";
            if (string.IsNullOrEmpty(departmentId) == false)
                template += " and DepartmentID = " + departmentId;

            using (var reader = new InlineQuery().ExecuteReader(template))
            {
                while (reader.Read())
                {
                    lst.Add(new CylinderTimelineObject()
                    {
                        Id = reader["CylinderId"] != null ? reader["CylinderId"].ToString() : string.Empty,
                        CylinderBarcode = reader["CylinderBarcode"] != null ? reader["CylinderBarcode"].ToString() : string.Empty,
                        CylinderNo = reader["CylinderNo"] != null ? reader["CylinderNo"].ToString() : string.Empty,
                        JobName = reader["JobName"] != null ? reader["JobName"].ToString() : string.Empty,
                        JobNumber = reader["JobNumber"] != null ? reader["JobNumber"].ToString() : string.Empty
                    });
                }
            }
            return lst;
        }

        public static TblDepartmentCollection GetDepartmentForTimeline()
        {
            return new SubSonic.Select().From(TblDepartment.Schema)
                .And(TblDepartment.IsObsoleteColumn).IsEqualTo(false)
                .OrderDesc(new string[] { TblDepartment.TimelineOrderColumn.ColumnName })
                .ExecuteAsCollection<TblDepartmentCollection>();
        }

        /*
        /// <summary>
        /// Hàm lấy danh sách lịch sử thao tác theo mã công việc
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public static TblJobProcessingCollection GetHistoryByDeptCode(object jobId)
        {
            return new SubSonic.Select()
              .From(TblJobProcessing.Schema).Where(TblJobProcessing.JobIDColumn).IsEqualTo(jobId)
              .ExecuteAsCollection<TblJobProcessingCollection>();
        }

        /// <summary>
        /// Hàm lấy danh sách lịch sử thao tác theo mã công việc và mã phòng ban
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public static TblJobProcessingCollection GetHistoryByDeptCode(object jobId, string departmentId)
        {
            return new SubSonic.Select()
              .From(TblJobProcessing.Schema).Where(TblJobProcessing.JobIDColumn).IsEqualTo(jobId)
              .And(TblJobProcessing.DepartmentIDColumn).IsEqualTo(departmentId)
              .ExecuteAsCollection<TblJobProcessingCollection>();
        }
        */

        public static TblUser GetUserByUserName(string username)
        {
            TblUserCollection allFound = new SubSonic.Select()
                .From(TblUser.Schema).Where(TblUser.UserNameColumn).IsEqualTo(username)
                .ExecuteAsCollection<TblUserCollection>();
            if (allFound != null && allFound.Count > 0)
            {
                if (allFound.Count == 1)
                    return allFound[0];

                foreach (TblUser tblUser in allFound)
                {
                    TblStaff staff = new SubSonic.Select()
                    .From(TblStaff.Schema).Where(TblStaff.EmailColumn)
                    .IsEqualTo(tblUser.Email).ExecuteSingle<TblStaff>();
                    if (staff != null)
                        return tblUser;
                }
            }
            return null;
        }

        #endregion

    }

    public class CylinderTimelineObject
    {
        public string Id { get; set; }
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string CylinderNo { get; set; }
        public string CylinderBarcode { get; set; }
    }
}
