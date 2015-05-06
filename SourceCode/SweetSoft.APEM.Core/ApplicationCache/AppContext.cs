using SweetSoft.CMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace SweetSoft.APEM.Core.ApplicationCache
{
    public sealed class AppContext
    {

        HttpContext _httpContext = null;

        private void Initialize(NameValueCollection qs, Uri uri, string rawUrl, string siteUrl)
        {
            _queryString = qs;
            _siteUrl = siteUrl;
            _currentUri = uri;
            _rawUrl = rawUrl;

        }

        public Uri CurrentUri
        {
            get
            {
                if (_currentUri == null)
                    _currentUri = new Uri("http://localhost/");

                return _currentUri;

            }
            set { _currentUri = value; }
        }


        [ThreadStatic]
        private static AppContext currentContext = null;

        private static readonly string dataKey = "AppContextStore";
        private HttpContext httpContext;
        private bool p;

        public AppContext(HttpContext context, bool includeQS)
        {
            this._httpContext = context;

            if (includeQS)
            {
                Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url, context.Request.RawUrl, GetSiteUrl());
            }
            else
            {
                Initialize(null, context.Request.Url, context.Request.RawUrl, GetSiteUrl());
            }
        }

        private string GetSiteUrl()
        {
            string hostName = _httpContext.Request.Url.Host.Replace("www.", string.Empty);
            string applicationPath = _httpContext.Request.ApplicationPath;

            if (applicationPath.EndsWith("/"))
                applicationPath = applicationPath.Remove(applicationPath.Length - 1, 1);

            return hostName + applicationPath;
        }

        public static AppContext Current
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;

                if (httpContext != null)
                {
                    if (httpContext.Items.Contains(dataKey))
                        return httpContext.Items[dataKey] as AppContext;
                    else
                    {
                        AppContext context = new AppContext(httpContext, true);
                        SaveContextToStore(context);
                        return context;
                    }
                }

                if (currentContext == null)
                    throw new Exception("No AppContext exists in the Current Application. AutoCreate fails since HttpContext.Current is not accessible.");
                return currentContext;
            }
        }

        private static void SaveContextToStore(AppContext context)
        {
            if (context.IsWebRequest)
            {
                context.Context.Items[dataKey] = context;
            }
            else
            {
                currentContext = context;
            }
        }

        public NameValueCollection _queryString = null;

        public string _siteUrl = null;

        public Uri _currentUri;

        public string _rawUrl;

        private int _languageId = 0; //default language (vietnamese)

        public bool IsWebRequest
        {
            get
            {
                return this.Context != null;
            }
        }

        public HttpContext Context
        {
            get
            {
                return _httpContext;
            }
        }

        /// <summary>
        /// get current languageid 
        /// </summary>
        public int CurrentLanguageId
        {
            get
            {
                return GetCurrentLanguage();
            }
            set
            {
                _languageId = value;
                WriteLanguageIdToCookie(_languageId);
            }
        }

        private int GetCurrentLanguage()
        {
            if (_languageId != 0)
                return _languageId;

            if (_languageId == 0)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_ID];
                if (cookie != null)
                {
                    int.TryParse(cookie.Value, out _languageId);
                }
                else //Nếu không có cookies
                {
                    string[] userLanguages = HttpContext.Current.Request.UserLanguages; //Lấy ngôn ngữ trình duyệt
                    if (userLanguages != null && userLanguages.Length > 0)
                        _languageId = LanguageHelper.GetLanguageCodeByCultureName(userLanguages[0].Trim());
                }
            }

            return _languageId == 0 ? LanguageHelper.English : _languageId; //if languageid = 0 return English language(2)
        }

        private void WriteLanguageIdToCookie(int languaugeId)
        {
            if (HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_ID] != null)
            {
                HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_ID].Value = languaugeId.ToString();
                HttpContext.Current.Response.Cookies.Set(HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_ID]);
            }
            else
                HttpContext.Current.Response.Cookies.Set(new HttpCookie(CURRENT_LANGUAGE_ID, languaugeId.ToString()));


            HttpContext.Current.Response.Cookies[CURRENT_LANGUAGE_ID].Expires = DateTime.Now.AddDays(1);

        }

        private const string CURRENT_LANGUAGE_ID = "SWEETSOFT_CURRENT_LANGUAGE_ID";
    }
}
