using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SweetSoft.APEM.DataAccess;
using System.Collections;
using System.Collections.Specialized;
using System.Web.SessionState;
using SweetSoft.APEM.Core.Security;
using System.IO;
using SweetSoft.APEM.Core.Manager;
//using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.Core
{
    /// <summary>
    /// The ApplicationContext represents common properties and settings used through out of a Request. All data stored
    /// in the context will be cleared at the end of the request
    /// 
    /// This object should be safe to use outside of a web request, but querystring and other values should be prepopulated
    /// 
    /// Each CS thread must create an instance of the ApplicationContext using one of the Three Create overloads. In some cases, 
    /// the CreateEmptyContext method may be used, but it is NOT recommended.
    /// </summary>
    public sealed class ApplicationContext
    {
        #region Private Containers

        //Generally expect 10 or less items
        private HybridDictionary m_Items = new HybridDictionary();
        private NameValueCollection m_QueryString = null;
        private string m_SiteUrl = null;
        private Uri m_CurrentUri;
        private string m_RawUrl;
        HttpContext m_HttpContext = null;

        #endregion

        #region Core Properties
        /// <summary>
        /// Simulates Context.Items and provides a per request/instance storage bag
        /// </summary>
        public IDictionary Items
        {
            get { return m_Items; }
        }

        /// <summary>
        /// Provides direct access to the .Items property
        /// </summary>
        public object this[string key]
        {
            get
            {
                return this.Items[key];
            }
            set
            {
                this.Items[key] = value;
            }
        }

        /// <summary>
        /// Allows access to QueryString values
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return m_QueryString; }
        }

        /// <summary>
        /// Quick check to see if we have a valid web reqeust. Returns false if HttpContext == null
        /// </summary>
        public bool IsWebRequest
        {
            get { return this.Context != null; }
        }

        public HttpContext Context
        {
            get
            {
                return m_HttpContext;
            }
        }

        public string SiteUrl
        {
            get { return m_SiteUrl; }
        }

        public Uri CurrentUri
        {
            get
            {
                if (m_CurrentUri == null)
                    m_CurrentUri = new Uri("http://localhost/");

                return m_CurrentUri;

            }
            set { m_CurrentUri = value; }
        }

        private string _hostPath = null;
        /// <summary>
        /// 
        /// </summary>
        public string HostPath
        {
            get
            {
                if (_hostPath == null)
                {
                    string portInfo = CurrentUri.Port == 80 ? string.Empty : ":" + CurrentUri.Port.ToString();
                    _hostPath = string.Format("{0}://{1}{2}", CurrentUri.Scheme, CurrentUri.Host, portInfo);
                }
                return _hostPath;
            }
        }

        #endregion

        #region Initialize  and contructors

        public void ClearSession(string sessionName)
        {
            Session[SessionPrefix + sessionName] = null;
        }
        /// <summary>
        /// Create/Instatiate items that will vary based on where this object 
        /// is first created
        /// 
        /// We could wire up Path, encoding, etc as necessary
        /// </summary>
        private void Initialize(NameValueCollection qs, Uri uri, string rawUrl, string siteUrl)
        {
            m_QueryString = qs;
            m_SiteUrl = siteUrl;
            m_CurrentUri = uri;
            m_RawUrl = rawUrl;
        }

        /// <summary>
        /// cntr called when no HttpContext is available
        /// </summary>
        private ApplicationContext(Uri uri, string siteUrl)
        {
            Initialize(new NameValueCollection(), uri, uri.ToString(), siteUrl);
        }

        /// <summary>
        /// cnst called when HttpContext is avaiable
        /// </summary>
        /// <param name="context"></param>
        private ApplicationContext(HttpContext context, bool includeQueryString)
        {
            this.m_HttpContext = context;

            if (includeQueryString)
            {
                Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url, context.Request.RawUrl, GetSiteUrl());
            }
            else
            {
                Initialize(null, context.Request.Url, context.Request.RawUrl, GetSiteUrl());
            }
        }

        #endregion

        #region State

        private static readonly string dataKey = "ApplicationContextStore";

        [ThreadStatic]
        private static ApplicationContext currentContext = null;

        /// <summary>
        /// Returns the current instance of the CMSContext from the ThreadData Slot. If one is not found and a valid HttpContext can be found,
        /// it will be used. Otherwise, an exception will be thrown. 
        /// </summary>
        public static ApplicationContext Current
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;

                if (httpContext != null)
                {
                    if (httpContext.Items.Contains(dataKey))
                        return httpContext.Items[dataKey] as ApplicationContext;
                    else
                    {
                        ApplicationContext context = new ApplicationContext(httpContext, true);
                        SaveContextToStore(context);
                        return context;
                    }
                }

                if (currentContext == null)
                    throw new Exception("No ApplicationContext exists in the Current Application. AutoCreate fails since HttpContext.Current is not accessible.");
                return currentContext;
            }
        }

        private static void SaveContextToStore(ApplicationContext context)
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

        /// <summary>
        /// Remove current context out of memmory
        /// </summary>
        public static void Unload()
        {
            currentContext = null;
        }
        #endregion

        #region Session context
        public const string SessionPrefix = "APEM";
        public HttpSessionState Session
        {
            get
            {
                return Context.Session;
            }
        }
        #endregion

        #region URL context

        public string MapPath(string path)
        {
            if (Context != null)
                return Context.Server.MapPath(path);
            else
                return PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
        }

        public string PhysicalPath(string path)
        {
            return string.Concat(RootPath().TrimEnd(Path.DirectorySeparatorChar), Path.DirectorySeparatorChar.ToString(), path.TrimStart(Path.DirectorySeparatorChar));
        }

        private string m_RootPath;
        private string RootPath()
        {
            if (m_RootPath == null)
            {
                m_RootPath = AppDomain.CurrentDomain.BaseDirectory;
                string dirSep = Path.DirectorySeparatorChar.ToString();

                m_RootPath = m_RootPath.Replace("/", dirSep);
            }
            return m_RootPath;
        }

        private string GetSiteUrl()
        {
            string hostName = Context.Request.Url.Host.Replace("www.", string.Empty);
            string applicationPath = Context.Request.ApplicationPath;

            if (applicationPath.EndsWith("/"))
                applicationPath = applicationPath.Remove(applicationPath.Length - 1, 1);

            return hostName + applicationPath;

        }
        #endregion

        #region Language Context

        private const string CURRENT_LANGUAGE_CODE = "SWEETSOFT_CURRENT_LANGUAGE_CODE";

        private string m_CurrentLanguageCode;
        /// <summary>
        /// Get current language code
        /// </summary>
        public string CurrentLanguageCode
        {
            get
            {
                if (string.IsNullOrEmpty(m_CurrentLanguageCode))
                {   //Try to get from cookie
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_CODE];
                    if (cookie != null)
                        m_CurrentLanguageCode = cookie.Value;
                    else
                        m_CurrentLanguageCode = CultureHelper.DEFAULT_LANGUAGE_CODE;
                }

                return m_CurrentLanguageCode;
            }
            set
            {
                m_CurrentLanguageCode = value;
                WriteLanguageIdToCookie(m_CurrentLanguageCode);
            }
        }

        private void WriteLanguageIdToCookie(string languaugeCode)
        {
            if (HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_CODE] != null)
            {
                HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_CODE].Value = languaugeCode;
                HttpContext.Current.Response.Cookies.Set(HttpContext.Current.Request.Cookies[CURRENT_LANGUAGE_CODE]);
            }
            else
                HttpContext.Current.Response.Cookies.Set(new HttpCookie(CURRENT_LANGUAGE_CODE, languaugeCode));

            //Cookie will be expired in 7 days
            HttpContext.Current.Response.Cookies[CURRENT_LANGUAGE_CODE].Expires = DateTime.Now.AddDays(7);

        }

        #endregion

        #region Gridview page size

        private const string CURRENT_PAGE_SIZE = "SWEETSOFT_PAGE_SIZE";

        private string m_CurrentPageSize;
        /// <summary>
        /// Get current language code
        /// </summary>
        public string CurrentPageSize
        {
            get
            {
                if (string.IsNullOrEmpty(m_CurrentPageSize))
                {   //Try to get from cookie
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[CURRENT_PAGE_SIZE];
                    if (cookie != null)
                        m_CurrentPageSize = cookie.Value;
                    else
                        m_CurrentPageSize = CultureHelper.DEFAULT_PAGE_SIZE;
                }

                return m_CurrentPageSize;
            }
            set
            {
                m_CurrentPageSize = value;
                WritePageSizeToCookie(m_CurrentPageSize);
            }
        }

        private void WritePageSizeToCookie(string pageSize)
        {
            if (HttpContext.Current.Request.Cookies[CURRENT_PAGE_SIZE] != null)
            {
                HttpContext.Current.Request.Cookies[CURRENT_PAGE_SIZE].Value = pageSize;
                HttpContext.Current.Response.Cookies.Set(HttpContext.Current.Request.Cookies[CURRENT_PAGE_SIZE]);
            }
            else
                HttpContext.Current.Response.Cookies.Set(new HttpCookie(CURRENT_PAGE_SIZE, pageSize));

            //Cookie will be expired in 7 days
            HttpContext.Current.Response.Cookies[CURRENT_PAGE_SIZE].Expires = DateTime.Now.AddDays(7);

        }

        #endregion

        #region APEM Data

        /// <summary>
        /// Return the current logged in UserName. This is read-only, value has bee set by the Membership Provider
        /// </summary>
        public string UserName
        {
            get
            {
                // if (!this.IsWebRequest || this.Context.User == null || this.Context.User.Identity.Name == string.Empty)
                if (Session[SessionPrefix + "CurrentUserName"] == null)
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                        return "Anonymous";
                    else
                        return HttpContext.Current.User.Identity.Name;
                }

                return Session[SessionPrefix + "CurrentUserName"].ToString();
            }
            set
            {
                Session[SessionPrefix + "CurrentUserName"] = value;
            }
        }
        /// <summary>
        /// Return the current logged in UserID. This is read-only, value has bee set by the Membership Provider
        /// </summary>
        public int UserID
        {
            get
            {
                // if (!this.IsWebRequest || this.Context.User == null || this.Context.User.Identity.Name == string.Empty)
                if (Session[SessionPrefix + "CurrentUserID"] == null)
                    return 0;

                return (int)Session[SessionPrefix + "CurrentUserID"];
            }
            set
            {
                Session[SessionPrefix + "CurrentUserID"] = value;
            }
        }

        public bool IsLogin
        {
            get
            {
                return this.User != null;
            }
        }

        /// <summary>
        /// Return the current logged in User. This user value to be anonymous if no user is logged in.
        /// This value can be set if necessary
        /// </summary>
        public TblUser User
        {
            get
            {
                TblUser m_CurrentUser = null;
                if (Session[SessionPrefix + "CurrentUser"] != null)
                    m_CurrentUser = (TblUser)Session[SessionPrefix + "CurrentUser"];

                if (m_CurrentUser == null)
                {
                    m_CurrentUser = UserManager.GetUserByName(UserName);
                    Session[SessionPrefix + "CurrentUser"] = m_CurrentUser;
                }

                return m_CurrentUser;
            }
        }

        public string CurrentUserIp
        {
            get
            {
                if (Session[SessionPrefix + "CurrentUserIp"] != null)
                    return Session[SessionPrefix + "CurrentUserIp"] as string;
                return string.Empty;
            }
            set
            {
                Session[SessionPrefix + "CurrentUserIp"] = value;
            }
        }

        public bool IsAdministrator
        {
            get
            {
                return ApplicationContext.Current.User.UserName == "administrator" || System.Web.Security.Roles.IsUserInRole(ApplicationContext.Current.User.UserName, "Administration");
            }
        }

        private Dictionary<string, FunctionPermission> m_UserFunctionPermission;
        public Dictionary<string, FunctionPermission> UserFunctionPermission
        {
            get
            {
                if (Session[SessionPrefix + "UserFunctionPermission"] != null)
                    m_UserFunctionPermission = (Dictionary<string, FunctionPermission>)Session[SessionPrefix + "UserFunctionPermission"];

                if (m_UserFunctionPermission == null)
                {
                    m_UserFunctionPermission = new Dictionary<string, FunctionPermission>();// RoleManager.GetFunctionPermissionByUserId(User.Id);
                    Session[SessionPrefix + "UserFunctionPermission"] = m_UserFunctionPermission;
                }

                return m_UserFunctionPermission;
            }
        }

        public void ReloadUser()
        {
            if (!string.IsNullOrEmpty(UserName))
                Session[SessionPrefix + "CurrentUser"] = UserManager.GetUserByName(UserName);
        }

        #endregion
    }
}
