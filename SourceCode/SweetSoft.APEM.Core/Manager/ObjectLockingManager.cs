using System;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
namespace SweetSoft.APEM.Core.Manager
{
    public class ObjectLockingType
    {
        public static string JOB = "JOB";
        public static string OC = "OC";
        public static string DO = "DO";
        public static string INVOICE = "INVOICE";
    }
    public class ObjectLockingManager
    {
        public static bool IsNewObjectLocking(int ID, string Type)
        {
            return new Select().From(TblObjectLocking.Schema)
                               .Where(TblObjectLocking.IdColumn).IsEqualTo(ID)
                               .And(TblObjectLocking.TypeColumn).IsEqualTo(Type)
                               .GetRecordCount() > 0 ? false : true;
        }

        public static bool IsObjectLocking(int ID, string Type)
        {
            return new Select().From(TblObjectLocking.Schema)
                               .Where(TblObjectLocking.IdColumn).IsEqualTo(ID)
                               .And(TblObjectLocking.TypeColumn).IsEqualTo(Type)
                               .And(TblObjectLocking.LockingColumn).IsEqualTo(true)
                               .GetRecordCount() > 0 ? true : false;
        }

        public static TblObjectLocking SelectByIDAndType(int ID, string Type)
        {
            return new Select().From(TblObjectLocking.Schema)
                               .Where(TblObjectLocking.IdColumn).IsEqualTo(ID)
                               .And(TblObjectLocking.TypeColumn).IsEqualTo(Type)
                               .ExecuteSingle<TblObjectLocking>();
        }

        public static TblObjectLocking Update(TblObjectLocking obj)
        {
            return new TblObjectLockingController().Update(obj);
        }

        public static TblObjectLocking Insert(TblObjectLocking obj)
        {
            return new TblObjectLockingController().Insert(obj);
        }

        public static bool Exists(int ID, string Type)
        {
            TblObjectLocking objObjectLocking = ObjectLockingManager.SelectByIDAndType(ID, Type);
            if (objObjectLocking != null)
                return true;
            else
                return false;
        }

        public static void LockOrUnlockObjectLocking(int ID, string Type, bool IsLock)
        {
            TblObjectLocking objObjectLocking = ObjectLockingManager.SelectByIDAndType(ID, Type);
            if (objObjectLocking != null)
            {
                objObjectLocking.Locking = IsLock;
                ObjectLockingManager.Update(objObjectLocking);
            }
            else
            {
                TblObjectLocking newObjectLocking = new TblObjectLocking();
                newObjectLocking.Id = ID;
                newObjectLocking.Type = Type;
                newObjectLocking.Locking = IsLock;
                ObjectLockingManager.Insert(newObjectLocking);
            }
        }

    }
}
