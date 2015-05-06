using SweetSoft.APEM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Controls
{
    public partial class ContentFormat : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtNContentFormat.config.toolbar = new object[]
			{
				new object[] {
                    "Source",
                    "-",
                    "Bold", "Italic", "Underline", "Strike",
                    "-",
                    "Subscript", "Superscript",
                    "-",
                    "NumberedList", "BulletedList",
                    "-",
                    "Link", "Unlink",
                    "-",
                    "TextColor", "BGColor",
                },
                "/"
                ,
                new object[] {
                    "Cut", "Copy", "Paste", "PasteText", "PasteFromWord"
                },
                new object[] {
                    "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock"
                },
                new object[] {
                    "Styles", "Format", "Font", "FontSize"
                }
			};

            if (!IsPostBack)
            {
                txtUsernameTest.Value = ApplicationContext.Current.UserName;
                txtNContentFormat.Language = ApplicationContext.Current.CurrentLanguageCode.Substring(2).ToLower();
            }
        }

        protected void btnTestEmailContentFormat_Click(object sender, EventArgs e)
        {
            bool isNContentFormat = (sender as Button).ID == btnTestNContentFormat.ID;
            string formatEmail = isNContentFormat ? txtNContentFormat.Text.Trim() : string.Empty;
            Regex regExFormat = new Regex(@"\{(.*?)\}");
            MatchCollection matchColl = regExFormat.Matches(formatEmail);
            if (matchColl != null && matchColl.Count > 0)
            {
                List<string> formatColl = new List<string>();
                foreach (Match match in matchColl)
                {
                    if (!formatColl.Contains(match.Groups[0].Value.Replace(" ", "")))
                        formatColl.Add(match.Groups[0].Value.Replace(" ", ""));
                    formatEmail = formatEmail.Replace(match.Groups[0].Value,
                        match.Groups[0].Value.Replace(" ", ""));
                }

                //validate
                int numberMax = 2;//-->3 cause index start with 0
                //5 control for get value + string "List order products" + string (other mesage for insert content in ContactInfoDetail.ascx control)
                /*
                
                txtObjectIdTest.Value, 
                txtObjectNameTest.Value,
                txtUsernameTest.Value
                
                 */

                #region Const define template

                const string otherMessage = "";

                #endregion

                Dictionary<int, string> listPositionError = new Dictionary<int, string>();
                for (int i = 0; i < formatColl.Count; i++)
                {
                    int temp = 0;
                    if (int.TryParse(formatColl[i].Replace("{", "").Replace("}", ""), out temp))
                    {
                        if (temp > numberMax)
                        {
                            listPositionError.Add(i, formatColl[i]);
                        }
                    }
                    else
                        listPositionError.Add(i, formatColl[i]);
                }
                const string ErrorReplaceIndex = "ERROR_REPLACE_INDEX";
                if (listPositionError != null && listPositionError.Count > 0)
                {
                    foreach (KeyValuePair<int, string> item in listPositionError)
                    {
                        formatEmail = formatEmail.Replace(item.Value, ErrorReplaceIndex + item.Key);
                    }
                }
                StringBuilder sbTemp = new StringBuilder();

                try
                {
                    sbTemp.AppendFormat(formatEmail,
                    isNContentFormat ? txtObjectIdTest.Value : string.Empty,
                    isNContentFormat ? txtObjectNameTest.Value : string.Empty,
                    isNContentFormat ? txtUsernameTest.Value : string.Empty);
                }
                catch { }

                if (listPositionError != null && listPositionError.Count > 0)
                {
                    errorMessage.Text = string.Format("<span style='color:red'>Error count: {0}</span><br/>", listPositionError.Count);
                    foreach (KeyValuePair<int, string> item in listPositionError)
                    {
                        sbTemp = sbTemp.Replace(ErrorReplaceIndex + item.Key, String.Format("<span style='color:red'>{0}</span>", item.Value));
                    }
                }
                else
                    errorMessage.Text = string.Empty;

                if (isNContentFormat)
                    ltrTestResult.Text = sbTemp.ToString();
            }
            else
            {
                if (isNContentFormat)
                    ltrTestResult.Text = formatEmail;
            }
        }

        public string Text
        {
            get
            {
                return txtNContentFormat.Text;
            }
            set
            {
                txtNContentFormat.Text = value;
            }
        }
    }
}