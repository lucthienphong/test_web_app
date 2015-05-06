using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SweetSoft.APEM.Core
{
    public class ParseHelper
    {
        /// <summary>
        /// Parse to decimal with default value
        /// </summary>
        /// <param name="parseValue">value to parse</param>
        /// <param name="defaultValue">default value</param>
        /// <param name="cultureInfo">CultureInfo</param>
        /// <returns></returns>
        public static decimal ToDecimalWithDefault(object parseValue, decimal defaultValue)
        {
            return ToDecimalWithDefault(parseValue, defaultValue, CultureInfo.CurrentCulture);
        }

        public static decimal ToDecimalWithDefault(object parseValue, decimal defaultValue, CultureInfo cultureInfo)
        {
            decimal u = -1;
            if (decimal.TryParse(parseValue.ToString(), NumberStyles.Any, cultureInfo, out u))
                return u;
            return defaultValue;
        }
        /// <summary>
        /// Parse to short with default value
        /// </summary>
        /// <param name="parseValue">value to parse</param>
        /// <param name="defaultValue">default value</param>
        /// <param name="cultureInfo">CultureInfo</param>
        /// <returns></returns>
        public static short ToShortWithDefault(object parseValue, short defaultValue)
        {
            return ToShortWithDefault(parseValue, defaultValue, CultureInfo.CurrentCulture);
        }


        public static short ToShortWithDefault(object parseValue, short defaultValue, CultureInfo cultureInfo)
        {
            short u = -1;
            if (short.TryParse(parseValue.ToString(), NumberStyles.Any, cultureInfo, out u))
                return u;
            return defaultValue;
        }
    }
}
