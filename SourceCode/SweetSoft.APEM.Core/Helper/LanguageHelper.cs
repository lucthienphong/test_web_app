using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.ApplicationCache;


namespace SweetSoft.CMS.Common
{
    public static class LanguageHelper
    {
        public static int English = 1;
        public static int Vietnamese = 2;

        private const string LANGUAGE_CODE_CACHE = "LANGUAGE_CODE_CACHE";
        private const string LANGUAGE_NAME_CACHE = "LANGUAGE_NAME_CACHE";
        /// <summary>
        /// Key: Language value (English:1, Vietnamese: 2)
        /// Value: string of lagguage name
        /// </summary>
        public static Dictionary<int, string> LanguageName
        {
            get
            {
                int langId = AppContext.Current.CurrentLanguageId;
                Dictionary<int, string> dic = AppCache.Get(LANGUAGE_NAME_CACHE + langId) as Dictionary<int, string>;
                if (dic == null)
                {
                        dic = new Dictionary<int, string>();
                        //dic[English] = UITextsReader.GetBackEndResourceText(AdminResourceKeys.ENGLISH);
                        //dic[Vietnamese] = UITextsReader.GetBackEndResourceText(AdminResourceKeys.VIETNAMESE);

                        dic[English] = "Tiếng Anh";
                        dic[Vietnamese] = "Việt Nam";
                   
                    AppCache.Max(LANGUAGE_NAME_CACHE + AppContext.Current.CurrentLanguageId, dic);
                }
                return dic;
            }
        }

        /// <summary>
        /// Key: Language code (English:1, Vietnamese: 2)
        /// Value: string of language code (English: en-US, Vietnamese: vi-VN)
        /// </summary>
        public static Dictionary<int, string> LanguageCode
        {
            get
            {
                Dictionary<int, string> dic = AppCache.Get(LANGUAGE_CODE_CACHE) as Dictionary<int, string>;
                if (dic == null)
                {
                    dic = new Dictionary<int, string>();
                    dic[English] = "en-US";
                    dic[Vietnamese] = "vi-VN";
                    AppCache.Max(LANGUAGE_CODE_CACHE, dic);
                }
                return dic;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strLanguageCode"></param>
        /// <returns>Language code (English:1, Vietnamese: 2,Laos: 176,Campuchia = 177,Myanmar = 99)</returns>
        public static int GetLanguageCodeByCultureName(string strLanguageCode)
        {
            switch (strLanguageCode.ToLower())
            {
                case "en-us":
                case "en":
                    return English;

                case "vi":
                case "vn":
                case "vi-vn":
                    return Vietnamese;
                default:
                    return 0;
            }
        }


        public static int CurrentLanguageCode
        {
            get
            {
                switch (CultureInfo.CurrentUICulture.Name)
                {
                    case "en-US":
                        return 1;
                    case "vi-VN":
                        return 2;
                    default:
                        return 2;
                }
            }
        }

    }
}
