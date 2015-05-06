using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

namespace SweetSoft.APEM.Core.UI
{
    public enum ButtonIcon
    {
        None,
        Add,
        Close,
        Back,
        Delete,
        Filter,
        Submit,
        Update,
        SignIn,
        Save,
        BarCode,
        Search,
        View,
        Share,
        Goto,
        GetBack,
        Print,
        Copy,
        TienOng,
        InsertPipe,
        Plus
    }

    public class ButtonExtension : Button
    {

        /// <summary>
        /// ShortCut của button
        /// </summary>
        public string ShortCut { get; set; }
        /// <summary>
        /// ID Control nhận focus tiếp theo
        /// </summary>
        public string NextElementID { get; set; }

        private void AddShortCut(string shortCut)
        {
        }

        public bool CheckValidationBeforeSubmit
        {
            set;
            get;
        }
        /// <summary>
        /// Icon cho button
        /// </summary>
        public ButtonIcon IconType
        {
            get;
            set;
        }

        public bool UseDefaultClass
        {
            set;
            get;
        }

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            StringBuilder currentClass = new StringBuilder();
            if (!string.IsNullOrEmpty(ShortCut))
                writer.AddAttribute("ShortCut", ShortCut);
            if (!string.IsNullOrEmpty(NextElementID))
                writer.AddAttribute("data-nextElement", NextElementID);

            string cssClass = this.CssClass;

            if (string.IsNullOrEmpty(ValidationGroup))
                this.OnClientClick += string.Format("ChangeValidationMode({0});", CheckValidationBeforeSubmit.ToString().ToLower());
            else
                // this.OnClientClick += string.Format("ChangeValidationMode({0});", CheckValidationBeforeSubmit.Value.ToString().ToLower());
                this.OnClientClick += string.Format("CheckValidWithButton({0},'{1}');", CheckValidationBeforeSubmit.ToString().ToLower(), ValidationGroup);

            if (!IconType.Equals(ButtonIcon.None))
            {
                cssClass += " btnIcon fa-2x";
                this.Text = string.Format("{0}  {1}", GetTextForIcon(IconType), this.Text);
            }

            writer.AddAttribute("class", string.Format("{0} {1} {2}", UseDefaultClass ? "btn" : string.Empty, currentClass.ToString().Trim(), cssClass));
            base.AddAttributesToRender(writer);
        }

        private string GetTextForIcon(ButtonIcon icon)
        {
            string result = string.Empty;
            switch (icon)
            {
                case ButtonIcon.Filter:
                    result = HttpUtility.HtmlDecode("&#xf0b0;");
                    break;
                case ButtonIcon.Close:
                    result = HttpUtility.HtmlDecode("&#xf00d;");
                    break;
                case ButtonIcon.Delete:
                    result = HttpUtility.HtmlDecode("&#xf014;");
                    break;
                case ButtonIcon.Update:
                    result = HttpUtility.HtmlDecode("&#xf044;");
                    break;
                case ButtonIcon.Add:
                    result = HttpUtility.HtmlDecode("&#xf0fe;");
                    break;
                case ButtonIcon.Submit:
                    result = HttpUtility.HtmlDecode("&#xf00c;");
                    break;
                case ButtonIcon.Back:
                    result = HttpUtility.HtmlDecode("&#xf08b;");
                    break;
                case ButtonIcon.SignIn:
                    result = HttpUtility.HtmlDecode("&#xf090;");
                    break;
                case ButtonIcon.Save:
                    result = HttpUtility.HtmlDecode("&#xf019;");
                    break;
                case ButtonIcon.BarCode:
                    result = HttpUtility.HtmlDecode("&#xf02a;");
                    break;
                case ButtonIcon.Search:
                    result = HttpUtility.HtmlDecode("&#xf002;");
                    break;
                case ButtonIcon.View:
                    result = HttpUtility.HtmlDecode("&#xf06e;");
                    break;
                case ButtonIcon.Share:
                    result = HttpUtility.HtmlDecode("&#xf064;");
                    break;
                case ButtonIcon.Goto:
                    result = HttpUtility.HtmlDecode("&#xf18e;");
                    break;
                case ButtonIcon.GetBack:
                    result = HttpUtility.HtmlDecode("&#xf190;");
                    break;
                case ButtonIcon.Print:
                    result = HttpUtility.HtmlDecode("&#xf02f;");
                    break;
                case ButtonIcon.Copy:
                    result = HttpUtility.HtmlDecode("&#xf0c5;");
                    break;
                case ButtonIcon.TienOng:
                    result = HttpUtility.HtmlDecode("&#xf0ad;");
                    break;
                case ButtonIcon.InsertPipe:
                    result = HttpUtility.HtmlDecode("&#xf03c;");
                    break;
                case ButtonIcon.Plus:
                    result = HttpUtility.HtmlDecode("&#xf146;");
                    break;
            }
            return result;
        }
    }
}
