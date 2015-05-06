using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Web;
using SweetSoft.APEM.Core;

namespace SweetSoft.APEM.Core
{
    public enum APEMTextProvider
    {
        MenuArea,
        ApplicationArea
    }

    public class ResourceTextManager
    {
        private const string idCol = "id";
        private const string textCol = "txt";

        public ResourceTextManager()
        {
        }

        public ResourceTextManager(APEMTextProvider textProvider, string languageCode)
        {
            m_TextProvider = textProvider;
            m_LanguageCode = languageCode;
        }

        private APEMTextProvider m_TextProvider;
        /// <summary>
        /// Specify which area of text: MenuArea, ApplicationArea
        /// </summary>
        public APEMTextProvider TextProvider
        {
            get { return m_TextProvider; }
            set { m_TextProvider = value; }
        }

        private string m_LanguageCode;
        public string LanguageCode
        {
            get { return m_LanguageCode; }
            set { m_LanguageCode = value; }
        }

        public DataSet TextDataSet
        {
            get
            {
                DataSet textDs = AppCache.Get(TextDataSetCacheKey) as DataSet;
                if (textDs == null)
                {
                    textDs = new DataSet();
                    if (File.Exists(TextXmlFile))
                    {
                        textDs.ReadXml(TextXmlFile, XmlReadMode.Auto);
                        UpdateTextDataSetCache(textDs);
                    }
                }
                return textDs;
            }
        }

        public ResourceTextManager DefaultResourceTextManager
        {
            get
            {
                return new ResourceTextManager(this.TextProvider, CultureHelper.DEFAULT_LANGUAGE_CODE);
            }
        }

        private string TextXmlFile
        {
            get
            {
                if (!string.IsNullOrEmpty(LanguageCode))
                {
                    string filePrefix = string.Empty;
                    switch (TextProvider)
                    {
                        case APEMTextProvider.MenuArea:
                            filePrefix = "MenuText";
                            break;
                        case APEMTextProvider.ApplicationArea:
                            filePrefix = "ApplicationText";
                            break;
                        default:
                            filePrefix = "ApplicationText";
                            break;
                    }

                    return string.Format("{0}App_Data\\{1}.{2}.xml",
                                    HttpContext.Current.Request.PhysicalApplicationPath, filePrefix, LanguageCode);
                }
                else throw new Exception("No language is specified.");
            }
        }

        private string TextDictionaryCacheKey
        {
            get
            {
                return string.Format("TEXT_DICTIONARY_{0}", TextProvider);
            }
        }

        private string TextDataSetCacheKey
        {
            get
            {
                return string.Format("TEXT_DATASET_{0}_{1}", TextProvider, LanguageCode);
            }
        }

        /// <summary>
        /// Key: LanguageCode, Value: Dictionary[TextID, TextValue]
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> TextDictionary
        {
            get
            {
                Dictionary<string, Dictionary<string, string>> textDic = AppCache.Get(TextDictionaryCacheKey) as Dictionary<string, Dictionary<string, string>>;
                if (textDic == null)
                    textDic = new Dictionary<string, Dictionary<string, string>>();
                if (textDic.ContainsKey(LanguageCode))
                    return textDic;
                else
                {
                    Dictionary<string, string> textData = new Dictionary<string, string>();
                    DataTable dtLanguage = null;
                    if (TextDataSet != null && TextDataSet.Tables.Count > 0)
                        dtLanguage = TextDataSet.Tables[0];
                    else if (DefaultResourceTextManager.TextDataSet.Tables.Count > 0) //Text for this language is not found. Get English as default
                        dtLanguage = DefaultResourceTextManager.TextDataSet.Tables[0];

                    if (dtLanguage != null && dtLanguage.Rows.Count > 0)
                    {
                        foreach (DataRow text in dtLanguage.Rows)
                            textData[(string)text[idCol]] = (string)text[textCol];

                        textDic[LanguageCode] = textData;
                        UpdateTextDictionaryCache(textDic);
                    }
                }
                return textDic;
            }
        }

        /// <summary>
        /// Update text to xml file
        /// </summary>
        /// <param name="ds"></param>
        public void UpdateText(DataSet ds)
        {
            StreamWriter sWriter = new StreamWriter(TextXmlFile, false, Encoding.UTF8);
            ds.WriteXml(sWriter);
            sWriter.Close();
            AppCache.Remove(TextDataSetCacheKey);
            AppCache.Remove(TextDictionaryCacheKey);
        }

        /// <summary>
        /// Get list of text to bind to grid view
        /// </summary>
        /// <returns></returns>
        public List<TextCarrier> GetTextList()
        {
            List<TextCarrier> texts = new List<TextCarrier>();
            DataSet dsDefaultLanguage = DefaultResourceTextManager.TextDataSet;
            if (dsDefaultLanguage != null && dsDefaultLanguage.Tables.Count > 0)
            {
                foreach (DataRow row in dsDefaultLanguage.Tables[0].Rows)
                {
                    TextCarrier tc = new TextCarrier();
                    tc.Id = Convert.ToString(row[idCol]);
                    Dictionary<string, string> currentTextDic = TextDictionary[LanguageCode];
                    if (currentTextDic.ContainsKey(tc.Id))
                        tc.Txt = currentTextDic[tc.Id];
                    else
                        tc.Txt = Convert.ToString(row[textCol]);

                    texts.Add(tc);
                }
            }
            return texts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetCurrentTextDictionary()
        {
            return TextDictionary[LanguageCode];
        }

        /// <summary>
        /// Get text base on language code property of ResourceTextManager
        /// </summary>
        /// <param name="textId"></param>
        /// <returns></returns>
        public string GetText(string textId)
        {
            return TextDictionary[LanguageCode].ContainsKey(textId) ? TextDictionary[LanguageCode][textId] : textId;
        }

        /// <summary>
        /// Get menu text
        /// </summary>
        /// <param name="textId"></param>
        /// <returns></returns>
        public static string GetMenuText(string textId)
        {
            return new ResourceTextManager(APEMTextProvider.MenuArea, ApplicationContext.Current.CurrentLanguageCode).GetText(textId);
        }

        public static string GetApplicationText(string textId)
        {
            return new ResourceTextManager(APEMTextProvider.ApplicationArea, ApplicationContext.Current.CurrentLanguageCode).GetText(textId);
        }

        /// <summary>
        /// Get text base on specified languageCode
        /// </summary>
        /// <param name="languageCode"></param>
        /// <param name="textId"></param>
        /// <returns></returns>
        public string GetText(string languageCode, string textId)
        {
            LanguageCode = languageCode;
            return TextDictionary[LanguageCode].ContainsKey(textId) ? TextDictionary[LanguageCode][textId] : textId;
        }


        public void DeleteTextData()
        {
            if (File.Exists(TextXmlFile))
                File.Delete(TextXmlFile);
        }

        private void UpdateTextDictionaryCache(object data)
        {
            AppCache.Remove(TextDictionaryCacheKey);
            AppCache.Max(TextDictionaryCacheKey, data);
        }

        private void UpdateTextDataSetCache(object data)
        {
            AppCache.Remove(TextDataSetCacheKey);
            AppCache.Max(TextDataSetCacheKey, data);
        }

        #region DataTable helpers

        public static DataSet CreateDataSet()
        {
            return new DataSet("Texts");
        }

        public static DataTable CreateDataTable()
        {
            DataTable dt = new DataTable("Text");

            DataColumn col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = idCol;
            dt.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = textCol;
            dt.Columns.Add(col);

            return dt;
        }

        public static void AddDataToTable(DataTable dt, string textID, string text)
        {
            DataRow dRow = dt.NewRow();
            dRow[idCol] = textID;
            dRow[textCol] = text;

            dt.Rows.Add(dRow);
        }
        #endregion
    }
}
