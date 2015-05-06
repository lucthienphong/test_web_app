using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.Core.UI
{
    public class TextBoxExtension : TextBox
    {
        public enum TextBoxType
        {
            Text,
            Number,
            Mail,
            Currency,
            Float
        }

        #region Properties

        /// <summary>
        /// Định dạng validation cho control
        /// </summary>
        public TextBoxType ValidationType
        {
            get;
            set;
        }
        /// <summary>
        /// Câu thông báo khi có lỗi
        /// </summary>
        public string ValidationErrorMessage
        {
            get;
            set;
        }
        /// <summary>
        /// Vị trí hiển thị popup validation, mặc địh là topRight
        ///     + topRight
        ///     + topLeft
        ///     + bottomRight
        ///     + bottomLeft
        ///     + centerRight
        ///     + centerLeft
        ///     + các vị trí trên cộng với custom postion
        ///         vd: topRight: 10,20
        /// </summary>
        public string ValidationPostion
        {
            get;
            set;
        }

        public bool ShowDefaultValue
        {
            get;
            set;
        }
        /// <summary>
        /// Lấy giá trị của control
        /// </summary>

        public object Value
        {
            get
            {
                string value = this.Text;
                if (ValidationType.Equals(TextBoxType.Currency))
                {
                    int gx = 0;
                    value = value.Replace(".", string.Empty);
                    if (!int.TryParse(value, out gx))
                        gx = 0;
                    return gx;
                }
                return value;
            }
        }

        /// <summary>
        /// Yêu cầu nhập giá trị
        /// </summary>
        public bool RequiredValidate
        {
            get;
            set;
        }

        public bool EmailValidate
        {
            get;
            set;
        }

        public bool PhoneNumberValidate
        {
            get;
            set;
        }

        public bool LetterOnlyValidate
        {
            get;
            set;
        }

        public bool NumberOnlyValidate
        {
            get;
            set;
        }

        public bool LetterNumberOnlyValidate
        {
            get;
            set;
        }

        public int MaxSize
        {
            get;
            set;
        }

        public bool CurrencyFormat
        {
            get;
            set;
        }

        private bool cfv = true;
        public bool ConfirmValueChange
        {
            get { return cfv; }
            set { cfv = value; }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            //if (this.Width.IsEmpty)
            //{
            //    this.Width = 230;

            //}
            if (ShowDefaultValue)
            {
            }
            base.OnInit(e);
        }



        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            StringBuilder currentClass = new StringBuilder();
            string timeFormat = string.Empty;
            bool? showPopUp = null;
            string customClass = this.CssClass.ToString();

            #region TextBox type
            if (ValidationType != null)
            {
                switch (ValidationType)
                {
                    case TextBoxType.Currency:
                        currentClass.Append(string.Format("{0}", MaskInputType.Currency));
                        break;

                    case TextBoxType.Mail:
                        {
                            currentClass.Append(string.Format("{0}", MaskInputType.Email));
                        }
                        break;
                    case TextBoxType.Number:
                        {
                            currentClass.Append(string.Format("{0}", MaskInputType.Number));
                        }
                        break;
                    case TextBoxType.Float:
                        {
                            currentClass.Append(string.Format("{0}", MaskInputType.Float));
                        }
                        break;
                }
            }



            //showpopup :datetime

            #endregion

            #region custom validation
            string requiredValidate = RequiredValidate ? "required" : string.Empty;
            string maxSize = MaxSize > 0 ? string.Format("maxSize[{0}]", MaxSize) : string.Empty;
            string emailValidate = EmailValidate ? "custom[email]" : string.Empty;
            string phoneValidate = PhoneNumberValidate ? "custom[phone]" : string.Empty;
            string letterOnlyValidate = LetterOnlyValidate ? "custom[onlyLetterSp]" : string.Empty;
            string numberOnlyValidate = NumberOnlyValidate ? "custom[onlyNumberSp]" : string.Empty;
            string letterNumberOnlyValidate = LetterNumberOnlyValidate ? "custom[onlyLetterNumber]" : string.Empty;


            StringBuilder validate = new StringBuilder();

            if (!string.IsNullOrEmpty(requiredValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", requiredValidate) : requiredValidate);
            if (!string.IsNullOrEmpty(maxSize))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", maxSize) : maxSize);
            if (!string.IsNullOrEmpty(emailValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", emailValidate) : emailValidate);
            if (!string.IsNullOrEmpty(phoneValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", phoneValidate) : phoneValidate);
            if (!string.IsNullOrEmpty(letterOnlyValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", letterOnlyValidate) : letterOnlyValidate);
            if (!string.IsNullOrEmpty(numberOnlyValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", numberOnlyValidate) : numberOnlyValidate);
            if (!string.IsNullOrEmpty(letterNumberOnlyValidate))
                validate.Append(validate.Length > 0 ? string.Format(",{0}", letterNumberOnlyValidate) : letterNumberOnlyValidate);
            //if (!string.IsNullOrEmpty(ValidationGroup))
            //    validate.Append(validate.Length > 0 ? string.Format(",groupRequired[{0}]", ValidationGroup) : string.Format("groupRequired[{0}]", ValidationGroup));

            //validation
            if (validate.Length > 0)
            {
                currentClass.Append(string.Format(" validate[{0}]", validate.ToString()));
                if (!string.IsNullOrEmpty(ValidationErrorMessage))
                    writer.AddAttribute("data-errormessage", string.Format("<li>{0}</li>", ValidationErrorMessage));
            }
            // validation postion
            string validationPostion = ValidationPostion;
            if (string.IsNullOrEmpty(validationPostion))
                validationPostion = "topRight";
            writer.AddAttribute("data-prompt-position", validationPostion);
            #endregion

            if (ConfirmValueChange)
                writer.AddAttribute("cwl", "1");

            writer.AddAttribute("class", string.Format("input-large {0} {1}", currentClass.ToString().Trim(), customClass));

            if (!string.IsNullOrEmpty(ValidationGroup))
                writer.AddAttribute("validation-group", ValidationGroup);
            base.AddAttributesToRender(writer);
        }

        //private class
        class MaskInputType
        {
            public const string Email = "MaskEmail";
            public const string Currency = "MaskCurrency";
            public const string Number = "MaskNumber";
            public const string Float = "MaskFloat";
        }
    }
}
