using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace SweetSoft.APEM.Core
{
    public static class ExtensionHelper
    {

        #region DataTable, DataSet
        /// <summary>
        /// Định nghĩa thêm cho DataTable phương thức ToCollection
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="dt">DataTable chủ</param>
        /// <returns></returns>
        public static List<T> ToCollection<T>(this DataTable dt)
        {
            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    try
                    {

                        DataColumn d = dc.Find(c => (c.ColumnName.ToLower().Replace("_", string.Empty) == pc.Name.ToLower()));
                        if (d != null)
                        {
                            string value = item[d.ColumnName].ToString();
                            switch (d.DataType.FullName)
                            {
                                case "System.UInt16":
                                case "System.Int16":
                                    Int16 value16 = Int16.Parse(value);
                                    pc.SetValue(cn, value16, null);
                                    break;
                                case "System.UInt32":
                                case "System.Int32":
                                    Int32 value32 = Int32.Parse(value);
                                    pc.SetValue(cn, value32, null);
                                    break;
                                case "System.Decimal":
                                case "System.Double":
                                    Decimal valued = Decimal.Parse(value);
                                    pc.SetValue(cn, valued, null);
                                    break;
                                case "System.Boolean":
                                    bool b = false;
                                    Boolean.TryParse(value, out b);
                                    pc.SetValue(cn, b, null);
                                    break;
                                case "System.DateTime":
                                    DateTime date = DateTime.MinValue;
                                    DateTime.TryParse(value, out date);
                                    pc.SetValue(cn, date, null);
                                    break;
                                default:
                                    pc.SetValue(cn, value, null);
                                    break;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }
        /// <summary>
        /// Định nghĩa thêm cho DataSet phương thức ToCollection
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="ds">DataSet chủ</param>
        /// <returns></returns>
        public static List<T> ToCollection<T>(this DataSet ds)
        {
            List<T> lst = new System.Collections.Generic.List<T>();
            if (ds != null && ds.Tables.Count > 0)
            {
                lst = ds.Tables[0].ToCollection<T>();
            }
            return lst;
        }
        #endregion

        #region Decimal
        /// <summary>
        /// Lấy chuỗi xuất giá trị theo format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringWithLanguage(this decimal value)
        {
            CultureInfo cul = new CultureInfo("vi-VN");

            if (value % 1 == 0)
                return value.ToString("N0", cul);
            return value.ToString("N3", cul);
        }

        /// <summary>
        /// Làm tròn giá trị
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modValue"></param>
        /// <param name="INC"></param>
        public static decimal RoundValue(this decimal value, int modValue, bool INC)
        {
            long phannguyen = (long)(value / 1000);
            int phandu = (int)(value % 1000);
            if (phandu >= modValue && INC)
                value = phannguyen + 1;
            else
                value = phannguyen;
            value *= 1000;
            return value;
        }
        #endregion
    }
}
