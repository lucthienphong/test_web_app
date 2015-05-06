using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web;
using System.Data;
using System.IO;

namespace SweetSoft.APEM.Core
{
    public enum DateTimeFormatType
    {
        ShortFormat,
        LongFormat
    }

    public static class CultureHelper
    {
        public static CultureInfo ISO_CULTURE_FORMAT = new CultureInfo("ja-JP"); //Japan format like ISO format !!!
        public static string ISO_CODE = "ja-JP"; //Japan format like ISO format !!!
        public static string DEFAULT_LANGUAGE_CODE = "en-GB";
        public static string DEFAULT_PAGE_SIZE = "10";
        public static CultureInfo DEFAULT_CULTURE = new CultureInfo("en-GB");

        private static string AvailableValidationJSCacheKey = "AvailableValidationJSCacheKey";
        /// <summary>
        /// List of available language in validation engine JS
        /// </summary>
        public static List<string> AvailableValidationJS
        {
            get
            {
                List<string> avJS = AppCache.Get(AvailableValidationJSCacheKey) as List<string>;
                if (avJS == null)
                {
                    avJS = new List<string>();
                    string languageListFile = string.Format("{0}js\\languages\\languageList.xml", HttpContext.Current.Request.PhysicalApplicationPath);
                    DataSet ds = new DataSet();
                    if (File.Exists(languageListFile))
                        ds.ReadXml(languageListFile, XmlReadMode.Auto);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[0].Rows)
                            avJS.Add(r["code"].ToString());
                    }
                    AppCache.Remove(AvailableValidationJSCacheKey);
                    AppCache.Max(AvailableValidationJSCacheKey, avJS);
                }
                return avJS;
            }
        }
        /// <summary>
        /// Get date format string base on language code and format type (Long/Short)
        /// </summary>
        /// <param name="languageCode"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public static string GetDateFormat(string languageCode, DateTimeFormatType formatType)
        {
            CultureInfo ci = new CultureInfo(languageCode);
            switch (formatType)
            {
                case DateTimeFormatType.LongFormat:
                    return ci.DateTimeFormat.LongDatePattern;

                default:
                    return ci.DateTimeFormat.ShortDatePattern;
            }
        }

        /// <summary>
        /// Get time format string base on language code and format type (Long/Short)
        /// </summary>
        /// <param name="languageCode"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public static string GetTimeFormat(string languageCode, DateTimeFormatType formatType)
        {
            CultureInfo ci = new CultureInfo(languageCode);
            switch (formatType)
            {
                case DateTimeFormatType.LongFormat:
                    return ci.DateTimeFormat.LongTimePattern;

                default:
                    return ci.DateTimeFormat.ShortTimePattern;
            }
        }

        /// <summary>
        /// Get DateTime format string base on language code and format type (Long/Short)
        /// </summary>
        /// <param name="languageCode"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public static string GetDateTimeFormat(string languageCode, DateTimeFormatType formatType)
        {
            return string.Format("{0} {1}", GetDateFormat(languageCode, formatType), GetTimeFormat(languageCode, formatType));
        }

        public static CultureInfo GetCultureInfo(string languageCode)
        {
            return new CultureInfo(languageCode);
        }

        public static string GetDefaultShortFormatDate()
        {
            CultureInfo ci = DEFAULT_CULTURE;
            return ci.DateTimeFormat.ShortDatePattern;
        }

        public static  CultureInfo NumberCulture
        {
            get
            {
                CultureInfo info = new CultureInfo("vi-VN");
                info.NumberFormat.NumberDecimalSeparator = ",";
                info.NumberFormat.NumberGroupSeparator = ".";
                info.NumberFormat.CurrencyDecimalSeparator = ",";
                info.NumberFormat.CurrencyGroupSeparator = ".";
                info.NumberFormat.PercentDecimalSeparator = ",";
                info.NumberFormat.PercentGroupSeparator = ".";
                info.NumberFormat.NumberDecimalDigits = 3;
                info.NumberFormat.NumberGroupSizes = new int[] { 3 };
                return info;
            }
        }
    }
}
