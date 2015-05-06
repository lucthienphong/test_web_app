using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;
using System.Data;
using System.Web.Security;
using System.Globalization;
using System.Text.RegularExpressions;


namespace SweetSoft.APEM.Core.Manager
{
    public class UserManager
    {
        public static string UserNumber()
        {
            string _No = "NV";
            ////string _MaxNumber = new Select(Aggregate.Max(TblUser.UserNoColumn)).From(TblUser.Schema).ExecuteScalar<string>();
            ////_MaxNumber = _MaxNumber ?? "";
            ////if (_MaxNumber.Length > 0)
            ////{
            ////    string _righ = (int.Parse(_MaxNumber.Substring(_MaxNumber.Length - 3, 3)) + 1).ToString();
            ////    while (_righ.Length < 4)
            ////        _righ = "0" + _righ;
            ////    _No += _righ;

            ////}
            ////else
            ////    _No += "0001";
            return _No;
        }

        /// <summary>
        /// Kiểm tra người dùng có tồn tại không?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Exists(int ID)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.UserIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra tên đăng nhập đã được sử dụng
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool UserNameExists(int ID, string UserName)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.UserNameColumn).IsEqualTo(UserName).And(TblUser.UserIDColumn).IsNotEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra email đã được sử dụng
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool EmailExists(int ID, string Email)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.EmailColumn).IsEqualTo(Email).And(TblUser.UserIDColumn).IsNotEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        /// <summary>
        /// Kiểm tra định dạng email
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(strIn);
                return address.Address == strIn;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra người dùng có phải là admin không?
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool IsAdministrator(int ID)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.UserNameColumn).IsEqualTo("administrator").And(TblUser.UserIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
        }

        public static bool IsAdministrator(string UserName)
        {
            return UserName == "administrator" || System.Web.Security.Roles.IsUserInRole(UserName, "Administration");
        }

        public static TblUser GetUserByName(string username)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.UserNameColumn).IsEqualTo(username).ExecuteSingle<TblUser>();
        }

        public static TblUser GetUserById(object id)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.UserIDColumn).IsEqualTo(id).ExecuteSingle<TblUser>();
        }

        public static bool ChangePassword(TblUser user, string oldPass, string newPass)
        {
            bool isChanged = false;

            MembershipUser membershipUser = Membership.GetUser(user.UserName);
            if (string.IsNullOrEmpty(oldPass) && ApplicationContext.Current.IsAdministrator)
                oldPass = membershipUser.ResetPassword();

            isChanged = membershipUser.ChangePassword(oldPass, newPass);
            if (isChanged)
            {
                UpdatePassword(user.UserID, newPass);
            }

            return isChanged;
        }

        public static void UpdatePassword(int userId, string newPass)
        {
            UpdateQuery query = new UpdateQuery(TblUser.Schema.TableName);
            query.QueryType = QueryType.Update;
            query.AddUpdateSetting(TblUser.Columns.Password, SecurityHelper.Encrypt(newPass));
            query.WHERE(TblUser.Columns.UserID, Comparison.Equals, userId);
            query.Execute();
        }

        public static string ResetPassword(string userName)
        {
            MembershipUser memberuser = Membership.GetUser(userName);
            return memberuser.ResetPassword();
        }

        /// <summary>
        /// Thêm người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void Insert(TblUser user, out TblUser retUser, out MembershipCreateStatus status)
        {
            int createdUserId = 0;
            status = MembershipCreateStatus.Success;
            try
            {
                string password = user.Password;
                user.Password = SecurityHelper.Encrypt(user.Password);
                retUser = new TblUserController().Insert(user);

                if (retUser != null)
                {
                    createdUserId = retUser.UserID;

                    // user.Password is not Encrypted, this will be Encrypted by Membership Provider
                    Membership.CreateUser(retUser.UserName, password, retUser.Email, "Name of the system?", "SweetSoft-APEM", !retUser.IsObsolete, out status);

                    //if (status == MembershipCreateStatus.Success)
                    //    AddRoleForUser(createdUserId, retUser.Username, roleIDs);
                    //else
                    //    new TblUserController().Destroy(retUser.Id);
                }
            }
            catch
            {
                //Delete user info in TblUser if error.
                if (createdUserId > 0)
                    new TblUserController().Destroy(createdUserId);
                status = MembershipCreateStatus.ProviderError;
                retUser = null;
            }
        }

        /// <summary>
        /// Cập nhật người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Update(TblUser user, string oldPassword)
        {
            //try
            {
                //Update in TblUser
                new TblUserController().Update(user);

                //Update in membership
                MembershipUser membershipUser = Membership.GetUser(user.UserName);

                if (!user.IsObsolete && membershipUser.IsLockedOut)
                    membershipUser.UnlockUser();

                if (!string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(user.Password))
                {
                    membershipUser.ChangePassword(oldPassword, user.Password);
                }

                //reset password
                if (string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(user.Password))
                {
                    ChangePassword(user, oldPassword, user.Password);
                }

                if (membershipUser.Email.CompareTo(user.Email) != 0)
                    membershipUser.Email = user.Email;

                if (user.IsObsolete)
                    membershipUser.IsApproved = false;
                else if (!membershipUser.IsApproved)
                    membershipUser.IsApproved = true;

                Membership.UpdateUser(membershipUser);

                //Update roles for user                 
                //AddRoleForUser(user.UserId, user.UserName, roleIDs);

                return true;
            }
            //catch
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// Xóa người dùng
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Delete(int ID)
        {
            TblUser obj = SelectUserByID(ID);
            if (obj != null)
            {
                int retUR = new SubSonic.Delete().From<TblUserRole>().Where(TblUserRole.UserIDColumn).IsEqualTo(obj.UserID).Execute();
                int ret = new SubSonic.Delete().From<TblUser>().Where(TblUser.UserNameColumn).IsEqualTo(obj.UserName).Execute();
                bool memberDeleted = Membership.DeleteUser(obj.UserName);
                return (retUR > 0 && ret > 0 && memberDeleted) ? true : false;
            }
            return false;
        }

        public static bool CheckUsernameExits(string username)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.UserNameColumn).IsEqualTo(username).ExecuteSingle<TblUser>() != null;
        }

        public static bool CheckEmailExits(string email, string username)
        {
            TblUserCollection userColl = new SubSonic.Select().From<TblUser>()
                                                  .Where(TblUser.EmailColumn).IsEqualTo(email)
                                                  .And(TblUser.UserNameColumn).IsNotEqualTo(username)
                                                  .ExecuteAsCollection<TblUserCollection>();

            return (userColl != null && userColl.Count > 0) ? true : false;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static TblUser GetUserByUserName(string userName)
        {

            TblUserCollection userCollection = new TblUserController().FetchByQuery(new SubSonic.Query(TblUser.Schema.TableName).WHERE(TblUser.Columns.UserName, userName));
            TblUser user = (userCollection != null && userCollection.Count > 0) ? userCollection[0] : null;

            if (user != null)
            {
                MembershipUser member = Membership.GetUser(user.UserName);
                if (member != null)
                    user.IsObsolete = member.IsLockedOut && !user.IsObsolete;
            }

            return user;
        }

        /// <summary>
        /// Get logged on user name
        /// </summary>
        /// <returns></returns>
        public static string GetLoggedOnUsername()
        {
            return ApplicationContext.Current.UserName;
        }

        public static TblUser SelectUserByID(int ID)
        {
            return new Select().From(TblUser.Schema).Where(TblUser.UserIDColumn).IsEqualTo(ID).ExecuteSingle<TblUser>();
        }

        /// <summary>
        /// Lấy danh sách người dùng
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortColumn"></param>
        /// <param name="SortType"></param>
        /// <returns></returns>
        public static DataTable UserList(string KeyWord, bool? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType)
        {
            DataTable dt = new DataTable();
            //dt.Load(SPs.TblUserSelectAll(KeyWord, IsActive, Language, PageIndex, PageSize, SortColumn, SortType).GetReader());
            return dt;
        }

        public static void UpdateWithoutPassword(TblUser user)
        {
            new TblUserController().Update(user);
        }
    }
}
