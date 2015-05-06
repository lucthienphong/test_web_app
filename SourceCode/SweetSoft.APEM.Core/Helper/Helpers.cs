using System;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;

namespace SweetSoft.APEM.Core.Helper
{
	/// <summary>
	/// Provides static helper methods to NotAClue.Web.BootstrapFriendlyControlAdapters controls. Singleton instance.
	/// </summary>
	public class Helpers
	{
		/// <summary>
		/// Private constructor forces singleton.
		/// </summary>
		private Helpers()
		{
		}

		public static int GetListItemIndex(ListControl control, ListItem item)
		{
			int index = control.Items.IndexOf(item);
			if (index == -1)
				throw new NullReferenceException("ListItem does not exist ListControl.");

			return index;
		}

		public static string GetListItemClientID(ListControl control, ListItem item)
		{
			if (control == null)
				throw new ArgumentNullException("Control can not be null.");
			
			int index = GetListItemIndex(control, item);

			return String.Format("{0}_{1}", control.ClientID, index.ToString());
		}

		public static string GetListItemUniqueID(ListControl control, ListItem item)
		{
			if (control == null)
				throw new ArgumentNullException("Control can not be null.");

			int index = GetListItemIndex(control, item);

			return String.Format("{0}${1}", control.UniqueID, index.ToString());
		}

		public static bool HeadContainsLinkHref(Page page, string href)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			foreach (Control control in page.Header.Controls)
			{
				if (control is HtmlLink && (control as HtmlLink).Href == href)
					return true;
			}

			return false;
		}

		public static void RegisterEmbeddedCSS(string css, Type type, Page page)
		{
			string filePath = page.ClientScript.GetWebResourceUrl(type, css);

			// if filePath is not empty, embedded CSS exists -- register it
			if (!String.IsNullOrEmpty(filePath))
			{
				if (!Helpers.HeadContainsLinkHref(page, filePath))
				{
					HtmlLink link = new HtmlLink();
					link.Href = page.ResolveUrl(filePath);
					link.Attributes["type"] = "text/css";
					link.Attributes["rel"] = "stylesheet";
					page.Header.Controls.Add(link);
				}
			}
		}

		public static void RegisterClientScript(string resource, Type type, Page page)
		{
			string filePath = page.ClientScript.GetWebResourceUrl(type, resource);

			// if filePath is empty, set to filename path
			if (String.IsNullOrEmpty(filePath))
			{
				string folderPath = WebConfigurationManager.AppSettings.Get("NotAClue.Web.BootstrapFriendlyControlAdapters-JavaScript-Path");
				if (String.IsNullOrEmpty(folderPath))
				{
					folderPath = "~/JavaScript";
				}
				filePath = folderPath.EndsWith("/") ? folderPath + resource : folderPath + "/" + resource;
			}

			if (!page.ClientScript.IsClientScriptIncludeRegistered(type, resource))
			{
				page.ClientScript.RegisterClientScriptInclude(type, resource, page.ResolveUrl(filePath));
			}
		}

		/// <summary>
		/// Gets the value of a non-public field of an object instance. Must have Reflection permission.
		/// </summary>
		/// <param name="container">The object whose field value will be returned.</param>
		/// <param name="fieldName">The name of the data field to get.</param>
		/// <remarks>Code initially provided by LonelyRollingStar.</remarks>
		public static object GetPrivateField(object container, string fieldName)
		{
			Type type = container.GetType();
			FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			return (fieldInfo == null ? null : fieldInfo.GetValue(container));
		}

        public static string GetPhysicalPath(string folderName, string fileName)
        {
            return string.Format("{0}{1}\\{2}", HttpContext.Current.Request.PhysicalApplicationPath, folderName, fileName);
        }

        public static string RandomString(int size)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
	}
}
