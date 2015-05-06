using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Globalization;


namespace SweetSoft.APEM.Core.UI
{
    public class DateTimeExtension : TextBox
    {
        public enum TimeFormat
        {
            None,
            Time24h,
            Time12h
        }
        /// <summary>
        /// Hiển thị popup, mặc định là True
        /// </summary>
        public bool ShowPopUp
        {
            get;
            set;
        }

        public DateTime? CurrentDate
        {
            get
            {

                DateTime time;
                CultureInfo viVN = new CultureInfo("vi-VN");
                if (!DateTime.TryParse(this.Text, viVN, DateTimeStyles.None, out time))
                    time = DateTime.MinValue;
                if (time == DateTime.MinValue)
                    return null;

                return time;

            }
        }

        public bool DateFormatValidate
        {
            get;
            set;
        }

        public string ValidationPostion
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
        }

        public bool RequiredValidate
        {
            get;
            set;
        }

        public TimeFormat CurrentTimeFormat
        {
            get;
            set;
        }

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            StringBuilder currentClass = new StringBuilder();
            string timeFormat = string.Empty;
            bool? showPopUp = null;
            string customClass = this.CssClass.ToString();
            currentClass.Append("MaskDate");
            if (CurrentTimeFormat.Equals(TimeFormat.Time24h))
                timeFormat = "Full";
            else if (CurrentTimeFormat.Equals(TimeFormat.Time12h))
                timeFormat = "Half";
            else
                timeFormat = string.Empty;
            if (!showPopUp.HasValue)
                showPopUp = true;
            writer.AddAttribute("ShowPopUp", showPopUp.Value.ToString());
            if (!string.IsNullOrEmpty(timeFormat))
                writer.AddAttribute("TimeFormat", timeFormat);

            StringBuilder validate = new StringBuilder();
            string requiredValidate = RequiredValidate ? "required" : string.Empty;
            string dateValidate = DateFormatValidate ? "custom[date]" : string.Empty;

            if (!string.IsNullOrEmpty(requiredValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", requiredValidate) : requiredValidate);
            if (!string.IsNullOrEmpty(dateValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", dateValidate) : dateValidate);

            if (validate.Length > 0)
                currentClass.Append(string.Format(" validate[{0}]", validate.ToString()));
            // validation postion
            string validationPostion = ValidationPostion;
            if (string.IsNullOrEmpty(validationPostion))
                validationPostion = "topRight";
            writer.AddAttribute("data-prompt-position", validationPostion);


            writer.AddAttribute("class", string.Format("{0} {1}", currentClass.ToString().Trim(), customClass));
            base.AddAttributesToRender(writer);
        }
    }
}
