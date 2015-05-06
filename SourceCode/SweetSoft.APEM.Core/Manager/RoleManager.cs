using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;
using SweetSoft.APEM.Core.Security;
using System.Data;
using System.Web.Security;

namespace SweetSoft.APEM.Core
{
    public class RoleManager
    {
        //Lấy danh sách các quyền mà người dùng có thể truy cập
        public static List<string> GetFunctionIdsByUserName(string userName)
        {
            //if (string.IsNullOrEmpty(userName) || userName.Equals("anonymous", StringComparison.InvariantCultureIgnoreCase))
            //    return null;
            if (string.IsNullOrEmpty(userName))
                return null;

            List<string> functions = new List<string>();
            functions.Add("APEM"); //Default function

            string[] roles = System.Web.Security.Roles.GetRolesForUser(userName); //Get list role
            Dictionary<string, int> roleNameId = BuildLookupRoleId();

            //Get list sFunctionId                         
            foreach (string role in roles)
            {
                List<TblRolePermission> roleList = new SubSonic.Select()
                    .From(TblRolePermission.Schema.TableName)
                    .InnerJoin(TblRole.Schema)
                    .WhereExpression(TblRolePermission.Columns.AllowAdd).IsEqualTo(true)
                    .Or(TblRolePermission.Columns.AllowEdit).IsEqualTo(true)
                    .Or(TblRolePermission.Columns.AllowDelete).IsEqualTo(true)
                    .Or(TblRolePermission.Columns.AllowUpdateStatus).IsEqualTo(true)
                    .Or(TblRolePermission.Columns.AllowOther).IsEqualTo(true)
                    .Or(TblRolePermission.Columns.AllowLockUnlock).IsEqualTo(true)
                    .AndExpression(TblRolePermission.Columns.RoleID).IsEqualTo(roleNameId[role])
                    .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                    .ExecuteAsCollection<TblRolePermissionCollection>().ToList<TblRolePermission>();
                foreach (TblRolePermission r in roleList)
                {
                    if (!functions.Contains(r.FunctionID))
                        functions.Add(r.FunctionID);
                }
            }

            return functions;
        }

        //Kiểm tra quyền thêm
        public static bool AllowAdd(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                .And(TblRolePermission.AllowAddColumn).IsEqualTo(true)
                .GetRecordCount() > 0 ? true : false;
        }

        //Kiểm tra quyền sửa
        public static bool AllowEdit(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                .And(TblRolePermission.AllowEditColumn).IsEqualTo(true)
                .GetRecordCount() > 0 ? true : false;
        }

        //Kiểm tra quyền xóa
        public static bool AllowDelete(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                .And(TblRolePermission.AllowDeleteColumn).IsEqualTo(true)
                .GetRecordCount() > 0 ? true : false;
        }

        //Kiểm tra quyền cập nhật trạng thái
        public static bool AllowUpdateStatus(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                .And(TblRolePermission.AllowUpdateStatusColumn).IsEqualTo(true)
                .GetRecordCount() > 0 ? true : false;
        }

        //Kiểm tra quyền khác
        public static bool AllowOther(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                .And(TblRolePermission.AllowOtherColumn).IsEqualTo(true)
                .GetRecordCount() > 0 ? true : false;
        }


        public static TblFunctionCollection GetFunctions(string parentId)
        {
            return new SubSonic.Select().From(TblFunction.Schema.TableName)
                                        .Where(TblFunction.Columns.ParentID)
                                        .IsEqualTo(parentId)
                                        .OrderAsc(TblFunction.Columns.DisplayOrder)
                                        .ExecuteAsCollection<TblFunctionCollection>();
        }

        public static TblFunction GetFunctionRoleByRoleId(long roleId, string functionId)
        {
            TblFunctionCollection ret = new SubSonic.Select().From(TblRolePermission.Schema.TableName)
                                                                    .Where(TblRolePermission.Columns.RoleID)
                                                                    .IsEqualTo(roleId)
                                                                    .And(TblRolePermission.Columns.FunctionID)
                                                                    .IsEqualTo(functionId)
                                                                    .ExecuteAsCollection<TblFunctionCollection>();

            return (ret != null && ret.Count > 0) ? ret[0] : null;
        }


        /// <summary>
        /// Key: FunctionId, Value: string: 'roles1,role2,roleN'
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetRolesInFunctionLookup()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<TblFunction> functionPages = new TblFunctionController().FetchAll().ToList<TblFunction>();
            List<TblRolePermission> functionRoles = new TblRolePermissionController().FetchAll().ToList<TblRolePermission>();
            Dictionary<int, string> roleIdName = BuildLookupRoleName();
            foreach (TblFunction fd in functionPages)
            {
                StringBuilder roles = new StringBuilder();
                for (int i = 0; i < functionRoles.Count; i++)
                {
                    if (fd.FunctionID.CompareTo(functionRoles[i].FunctionID) == 0)
                    {
                        if (roles.Length > 0)
                            roles.AppendFormat(",{0}", roleIdName[functionRoles[i].RoleID]);
                        else
                            roles.AppendFormat("{0}", roleIdName[functionRoles[i].RoleID]);
                    }
                }
                dic[fd.FunctionID] = roles.ToString();
            }
            return dic;
        }

        /*----------------------------- tblRole, tblUserRole -----------------------*/
        public static bool IsBeingUsed(int ID)
        {
            //Kiểm tra User
            bool IsUsedByUser = new Select().From(TblUserRole.Schema).Where(TblUserRole.RoleIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;

            if (IsUsedByUser)
                return true;
            return false;
        }

        /// <summary>
        /// Thêm quyền
        /// </summary>
        /// <param name="objRole"></param>
        /// <returns></returns>
        public static bool Insert(TblRole objRole)
        {
            bool isCreated = true;
            TblRole role = null;
            try
            {
                role = new TblRoleController().Insert(objRole);
            }
            catch
            {
                isCreated = false;
            }

            if (isCreated && role != null)
            {
                try
                {
                    System.Web.Security.Roles.CreateRole(objRole.RoleName);
                }
                catch
                {
                    new TblRoleController().Destroy(role.RoleID);
                    return false;
                }
            }

            return isCreated;
        }

        /// <summary>
        /// Cập nhật quyền
        /// </summary>
        /// <param name="objRole"></param>
        /// <returns></returns>
        public static TblRole Update(TblRole objRole)
        {
            return new TblRoleController().Update(objRole);
        }

        /// <summary>
        /// Xóa quyền
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Delete(int ID)
        {
            TblRole obj = SelectByID(ID);
            bool isDeleted = false;
            if (obj != null)
            {
                new TblRolePermissionController().Delete(ID);
                if (new TblRoleController().Delete(ID))
                {
                    isDeleted = System.Web.Security.Roles.DeleteRole(obj.RoleName);
                }
            }
            return isDeleted;
        }

        /// <summary>
        /// Lấy danh sách quyền
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
            dt.Load(SPs.TblRoleSelectAll(KeyWord, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());
            if (AddNew)
            {
                Random rnd = new Random();
                short randID = 0;
                do
                {
                    randID = (short)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<int>("RoleID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["RoleID"] = randID;
                r["RoleName"] = "";
                r["Description"] = "";
                r["IsObsolete"] = false;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        /// <summary>
        /// Kiểm tra quyền có phải quyền administration không?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool IsAdministration(int ID)
        {
            return new Select().From(TblRole.Schema).Where(TblRole.RoleNameColumn).IsEqualTo("Administration").And(TblRole.RoleIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }
        //Role Permissions------------------------------------------------------------------------
        /// <summary>
        /// Xóa quyền người dùng theo người dùng
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        public static void RemoveUsersRoles(int userID, string userName)
        {
            //Remove all current Roles of user:
            new SubSonic.Delete().From<TblUserRole>().Where(TblUserRole.UserIDColumn).IsEqualTo(userID).Execute();
            string[] RoleList = Roles.GetAllRoles();
            foreach (string roleName in RoleList)
            {
                if (Roles.IsUserInRole(userName, roleName))
                    Roles.RemoveUserFromRole(userName, roleName);
            }
        }

        /// <summary>
        /// Xóa quyền người dùng theo quyền
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="roleName"></param>
        public static void RemoveRolesUser(int roleID, string roleName)
        {
            //Remove all current Roles of user:
            new SubSonic.Delete().From<TblUserRole>().Where(TblUserRole.RoleIDColumn).IsEqualTo(roleID).Execute();
            string[] UserList = Roles.GetUsersInRole(roleName);
            foreach (string userName in UserList)
            {
                if (Roles.IsUserInRole(userName, roleName))
                    Roles.RemoveUserFromRole(userName, roleName);
            }
        }

        public static List<int> GetRoleIDsOfUser(int ID)
        {
            return new Select(TblUserRole.RoleIDColumn).From(TblUserRole.Schema).Where(TblUserRole.UserIDColumn).IsEqualTo(ID).ExecuteTypedList<int>();
        }

        public static bool AddUserToRole(int UserID, string UserName, int RoleID, string RoleName)
        {
            try
            {
                TblUserRole.Insert(UserID, RoleID);
                Roles.AddUserToRole(UserName, RoleName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Get all permission of Role
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public static DataTable RolePermissionSelectByRoleID(int RoleID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblRolePermissionsSelectByRoleID(RoleID).GetReader());
            return dt;
        }

        /// <summary>
        /// Kiểm tra quyền đã có trong bản phân quyền chưa?
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public static TblRolePermission TblRolePermissionSelectBy(int RoleID, string FunctionID)
        {
            return new Select().From(TblRolePermission.Schema).Where(TblRolePermission.RoleIDColumn).IsEqualTo(RoleID).And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID).ExecuteSingle<TblRolePermission>();
        }

        /// <summary>
        /// Thêm quyền
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblRolePermission InsertRolePermissions(TblRolePermission obj)
        {
            return new TblRolePermissionController().Insert(obj);
        }

        /// <summary>
        /// Cập nhật quyền
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblRolePermission UpdateRolePermissions(TblRolePermission obj)
        {
            return new TblRolePermissionController().Update(obj);
        }

        public static bool CheckRoleHasUser(long roleId)
        {
            bool hasUser = false;
            TblUserCollection userColl = GetUsersInRole(roleId);
            if (userColl != null && userColl.Count > 0)
                hasUser = true;
            return hasUser;
        }

        public static DataTable RoleListForDLL()
        {
            DataTable dt = new DataTable();
            dt.Load(new Select(TblRole.RoleNameColumn, TblRole.RoleIDColumn).From(TblRole.Schema).Where(TblRole.IsObsoleteColumn).IsEqualTo(false).OrderAsc(TblRole.Columns.RoleName).ExecuteReader());
            return dt;
        }

        public static TblRoleCollection GetAllRoles()
        {
            return new SubSonic.Select().From<TblRole>().OrderAsc(TblRole.Columns.RoleName)
                                .ExecuteAsCollection<TblRoleCollection>();
        }

        public static TblRoleCollection GetActiveRoles()
        {
            return new SubSonic.Select().From<TblRole>()
                                .Where(TblRole.Columns.IsObsolete).IsEqualTo(false)
                                .OrderAsc(TblRole.Columns.RoleName)
                                .ExecuteAsCollection<TblRoleCollection>();
        }

        public static TblRoleCollection SearchRole(List<string> searchFields,
                    Dictionary<string, Comparison> comparisons,
                    Dictionary<string, object> searchValues1)
        {
            Query searchQuery = new Query(TblRole.Schema.TableName);

            for (int i = 0; i < searchFields.Count; i++)
            {
                searchQuery.AND(searchFields[i], comparisons[searchFields[i]], searchValues1[searchFields[i]]);
            }

            return new TblRoleController().FetchByQuery(searchQuery);
        }

        public static TblRole SelectByID(int RoleId)
        {
            return new Select()
                .From(TblRole.Schema)
                .Where(TblRole.RoleIDColumn).IsEqualTo(RoleId)

                .ExecuteSingle<TblRole>();
        }

        public static TblRole GetRoleByRoleName(string roleName)
        {
            TblRoleCollection roleColl = new TblRoleController().FetchByQuery(new SubSonic.Query(TblRole.Schema.TableName)
                                                                                   .WHERE(TblRole.Columns.RoleName, roleName));
            TblRole role = (roleColl != null && roleColl.Count > 0) ? roleColl[0] : null;
            return role;
        }

        /// <summary>
        /// Kiểm tra tên quyền tồn tại theo tên
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public static bool Exists(string RoleName)
        {
            bool sign1 = new Select().From(TblRole.Schema).Where(TblRole.RoleNameColumn).IsEqualTo(RoleName).GetRecordCount() > 0 ? true : false;
            bool sign2 = System.Web.Security.Roles.RoleExists(RoleName);
            if (sign1 || sign2)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Kiểm tra tên quyền tồn tại theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Exists(int ID)
        {
            return new Select().From(TblRole.Schema).Where(TblRole.RoleIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        public static bool Exists(int ID, string Name)
        {
            return new Select().From(TblRole.Schema)
                .Where(TblRole.RoleIDColumn).IsNotEqualTo(ID)
                .And(TblRole.RoleNameColumn).IsEqualTo(Name).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Key: RoleName, Value: TblRole object
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Dictionary<string, TblRole> BuildLookupRoleForUser(int userId)
        {
            Dictionary<string, TblRole> dic = new Dictionary<string, TblRole>();
            List<TblRole> roles = GetUserRoles(userId);
            foreach (TblRole role in roles)
            {
                dic[role.RoleName] = role;
            }
            return dic;
        }

        /// <summary>
        /// Get List of TblRole by userId
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<TblRole> GetUserRoles(long userID)
        {
            TblUserRoleCollection arrUserRoles = new TblUserRoleController().FetchByQuery(new SubSonic.Query(Tables.TblUserRole).WHERE(TblUserRole.Columns.UserID, userID));
            List<TblRole> lstRole = new List<TblRole>();
            if (arrUserRoles != null && arrUserRoles.Count > 0)
            {
                foreach (TblUserRole usrRole in arrUserRoles)
                {
                    if (usrRole != null)
                        lstRole.Add(usrRole.TblRole);
                }
            }
            return lstRole;
        }

        /// <summary>
        /// Key: RoleId, Value: RoleName
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> BuildLookupRoleName()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            List<TblRole> roles = GetAllRoles().ToList<TblRole>();
            if (roles.Count > 0)
            {
                foreach (TblRole r in roles)
                {
                    dic[r.RoleID] = r.RoleName;
                }
            }
            return dic;
        }

        /// <summary>
        /// Key: RoleName, Value: RoleId
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> BuildLookupRoleId()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            List<TblRole> roles = GetAllRoles().ToList<TblRole>();
            if (roles.Count > 0)
            {
                foreach (TblRole r in roles)
                {
                    dic[r.RoleName] = r.RoleID;
                }
            }
            return dic;
        }

        public static bool GetBit(byte[] strMask, int iPos)
        {
            string strTemp = Encoding.UTF8.GetString(strMask);
            if (strTemp.Substring(iPos - 1, 1) == "1")
                return true;
            return false;
        }


        public static TblUserCollection GetUsersInRole(long RoleID)
        {
            return new SubSonic.Select().From<TblUser>()
                                        .Where(TblUser.UserIDColumn)
                                        .In(new SubSonic.Select(TblUserRole.UserIDColumn).From<TblUserRole>()
                                                                    .Where(TblUserRole.RoleIDColumn).IsEqualTo(RoleID))
                                        .OrderAsc(TblUser.Columns.UserName)
                                        .ExecuteAsCollection<TblUserCollection>();
        }

        public static TblUserCollection GetUsersInRoleByUserId(long RoleID, long userId)
        {
            return new SubSonic.Select().From<TblUser>()
                                        .Where(TblUser.UserIDColumn)
                                        .In(new SubSonic.Select(TblUserRole.UserIDColumn).From<TblUserRole>()
                                                                    .Where(TblUserRole.RoleIDColumn).IsEqualTo(RoleID))
                                        .And(TblUser.UserIDColumn).IsEqualTo(userId)
                                        .OrderAsc(TblUser.Columns.UserName)
                                        .ExecuteAsCollection<TblUserCollection>();
        }

        /// <summary>
        /// Delete table tbUserRole by userId
        /// </summary>
        /// <param name="userId"></param>
        public static void DeleteUserRoleByUserId(long userId)
        {
            new Delete().From<TblUserRole>().Where(TblUserRole.UserIDColumn).IsEqualTo(userId).Execute();
        }

        //public static List<FunctionPermission> GetCurrentUserFunctionPermission()
        //{
        //    List<FunctionPermission> FunctionPermissions = new List<FunctionPermission>();
        //    List<TblRole> currentRoles = RoleManager.GetUserRoles(ApplicationContext.Current.User.UserID);
        //    if (currentRoles != null && currentRoles.Count > 0)
        //    {
        //        foreach (TblRole currentRole in currentRoles)
        //        {
        //            if (currentRole != null && currentRole.IsActive.HasValue && currentRole.IsActive.Value)
        //                FunctionPermissions.Add(RoleManager.CreateFunctionPermissionForRole(currentRole));
        //        }
        //    }
        //    return FunctionPermissions;
        //}



        public static void DeleteFunctionRole(int RoleID)
        {
            new Delete().From(TblRolePermission.Schema).Where(TblRolePermission.RoleIDColumn).IsEqualTo(RoleID).ExecuteReader();
        }

        public static FunctionPermission CreateFunctionPermissionForRole(TblRole role)
        {
            FunctionPermission objPermission = new FunctionPermission();
            //objPermission.RoleId = role.RoleID;
            //objPermission.RoleName = role.RoleName;
            //objPermission.AllowDelete = role.AllowDelete.HasValue ? role.AllowDelete.Value : false;
            //objPermission.AllowUpdate = role.AllowUpdate.HasValue ? role.AllowUpdate.Value : false;

            return objPermission;
        }

        public static TblRolePermission GetFunctionRoleById(long roleFunctionId)
        {
            return TblRolePermission.FetchByID(roleFunctionId);
        }

        internal static TblRolePermissionCollection GetFunctionRole()
        {
            return new TblRolePermissionController().FetchAll();
        }

        internal static Dictionary<int, string> GetUserRolesInFunctionLookup()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            List<TblUserRole> userRoles = new TblUserRoleController().FetchAll().ToList<TblUserRole>();
            foreach (TblUserRole item in userRoles)
            {
                dic[item.RoleID] = item.UserID.ToString();
            }
            return dic;
        }

        internal static TblRoleCollection GetRoles()
        {
            return new TblRoleController().FetchAll();
        }

        internal static TblRolePermissionCollection GetFunctionRoleByRoleID(int IDRole)
        {
            return new SubSonic.Select().From<TblRolePermission>()
                .Where(TblRolePermission.RoleIDColumn).IsEqualTo(IDRole)
                .AndExpression(TblRolePermission.AllowAddColumn.ColumnName).IsEqualTo(true)
                .Or(TblRolePermission.AllowDeleteColumn).IsEqualTo(true)
                .Or(TblRolePermission.AllowEditColumn).IsEqualTo(true)
                .Or(TblRolePermission.AllowOtherColumn).IsEqualTo(true)
                .Or(TblRolePermission.AllowUpdateStatusColumn).IsEqualTo(true)
                .ExecuteAsCollection<TblRolePermissionCollection>();
        }

        public static TblUserRole GetRoleByUserID(int userID)
        {
            return new SubSonic.Select().From<TblUserRole>()
                .Where(TblUserRole.UserIDColumn).IsEqualTo(userID).ExecuteSingle<TblUserRole>();
        }

        // Trunglc Add - 23-04-2015

        public static bool AllowLock(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                .And(TblRolePermission.AllowLockUnlockColumn).IsEqualTo(true)
                .GetRecordCount() > 0 ? true : false;
        }

        public static bool AllowUnlock(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                .And(TblRolePermission.AllowLockUnlockColumn).IsEqualTo(true)
                .GetRecordCount() > 0 ? true : false;
        }

        public static bool AllowEditUnlock(string UserName, string FunctionID)
        {
            return new SubSonic.Select().From(TblUser.Schema)
                    .InnerJoin(TblUserRole.UserIDColumn, TblUser.UserIDColumn)
                    .InnerJoin(TblRole.RoleIDColumn, TblUserRole.RoleIDColumn)
                    .InnerJoin(TblRolePermission.RoleIDColumn, TblRole.RoleIDColumn)
                    .Where(TblUser.UserNameColumn).IsEqualTo(UserName)
                    .And(TblRole.IsObsoleteColumn).IsEqualTo(false)
                    .And(TblRolePermission.FunctionIDColumn).IsEqualTo(FunctionID)
                    .And(TblRolePermission.AllowUpdateStatusColumn).IsEqualTo(true)
                    .GetRecordCount() > 0 ? true : false;
        }
    }
}
