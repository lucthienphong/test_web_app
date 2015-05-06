using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.Core;

namespace SweetSoft.CMS.Common.Utils
{
    /// <summary>
    /// Xml helper class
    /// </summary>
    public class XmlHelper
    {
        #region Menu and sitemap
        public static void BuildXmlMenuToFile(int roleId, string serverPath)
        {
            XmlDocument xmlMenuDoc = new XmlDocument();
            xmlMenuDoc.LoadXml(BuildMenuWithRoleToXml(roleId));
            string fileName = "UserMenu.config";
            string fileNamePath = string.Format("{0}/{1}", serverPath, fileName);
            if (File.Exists(fileNamePath))
                File.Delete(fileNamePath);
            XmlTextWriter xmlWriter = new XmlTextWriter(fileNamePath, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = 4;
            xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            xmlMenuDoc.WriteTo(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();
            AppCache.Remove("APEM_CACHE_MENU");
        }

        private static string BuildMenuWithRoleToXml(int roleId)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<Menus>");
            BuildXmlMenu(ref xml, string.Empty);
            xml.Append("</Menus>");
            return xml.ToString();
        }


        private static void BuildXmlMenu(ref StringBuilder xml, string parentId)
        {

            StringBuilder xmlMenu = new StringBuilder();

            Dictionary<int, string> rolesOfFunction = AppCache.Get("ROLES_OF_FUNCTION_CACHE_KEY") as Dictionary<int, string>;

            if (rolesOfFunction == null || rolesOfFunction.Count == 0) // if cache is clear then get
            {
                rolesOfFunction = RoleManager.GetUserRolesInFunctionLookup();

                AppCache.Remove("ROLES_OF_FUNCTION_CACHE_KEY");
                AppCache.Max("ROLES_OF_FUNCTION_CACHE_KEY", rolesOfFunction);
            }

            List<TblRole> roles = RoleManager.GetRoles().ToList<TblRole>();

            foreach (TblRole ur in roles)
            {
                string fc = string.Empty;
                TblRolePermission rolesCol = RoleManager.GetFunctionRoleByRoleID(ur.RoleID);
                
                if (rolesCol.Count > 0 && rolesCol != null)
                {
                    foreach (TblFunctionRole item in rolesCol)
                    {
                       fc += item.FunctionID + " , ";
                    }
                }
                xml.AppendFormat("<MenuGroup RoleId=\"{0}\" AllowRoles=\"{1}\">", ur.RoleID, fc);    
                //if (RoleManager.GetFunctionPages(fp.FunctionId).Count > 0) //check has child
                //    BuildXmlMenu(ref xml, fp.FunctionId); // build child

                //if (string.IsNullOrEmpty(fp.ParentId))
                //    xml.Append("</MenuGroup>");
                xml.Append("</MenuGroup>");
            }
        }



        #endregion

        #region Methods
        /// <summary>
        /// XML Encode
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncode(string s)
        {
            if (s == null)
                return null;
            s = Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAsIs(s);
        }

        /// <summary>
        /// XML Encode as is
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncodeAsIs(string s)
        {
            if (s == null)
                return null;
            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xwr = new XmlTextWriter(sw))
            {
                xwr.WriteString(s);
                String sTmp = sw.ToString();
                return sTmp;
            }
        }

        /// <summary>
        /// Encodes an attribute
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttribute(string s)
        {
            if (s == null)
                return null;
            s = Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAttributeAsIs(s);
        }

        /// <summary>
        /// Encodes an attribute as is
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttributeAsIs(string s)
        {
            return XmlEncodeAsIs(s).Replace("\"", "&quot;");
        }

        /// <summary>
        /// Decodes an attribute
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Decoded attribute</returns>
        public static string XmlDecode(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            return sb.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").ToString();
        }

        /// <summary>
        /// Serializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Serialized datetime</returns>
        public static string SerializeDateTime(DateTime dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlS.Serialize(sw, dateTime);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Deserialized datetime</returns>
        public static DateTime DeserializeDateTime(string dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            StringBuilder sb = new StringBuilder();
            using (StringReader sr = new StringReader(dateTime))
            {
                object test = xmlS.Deserialize(sr);
                return (DateTime)test;
            }
        }
        #endregion
    }
}
