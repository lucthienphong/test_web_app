using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SweetSoft.APEM.WebApp.Common;
using SweetSoft.APEM.Core;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class WorkFlow : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "workflow_manager";
            }
        }

        static string CreatePassword(int length)
        {
            StringBuilder res = new StringBuilder();
            if (length > 0)
            {
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                Random rnd = new Random();
                while (0 < length--)
                {
                    res.Append(valid[rnd.Next(valid.Length)]);
                }
            }
            return res.ToString();
        }

        class EncryptPos
        {
            public int pos { get; set; }
            public string value { get; set; }
        }

        static string Encode(string data)
        {
            List<EncryptPos> arrEncrypt = new List<EncryptPos>();
            Random rnd = new Random();
            string newData = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                newData += data[i];
                if (arrEncrypt.Count < 10)
                {
                    int num = rnd.Next(0, 5);
                    string val = CreatePassword(num);
                    arrEncrypt.Add(new EncryptPos() { pos = i, value = val });
                    newData += val;
                }
            }

            return JsonConvert.SerializeObject(new { arrEncrypt = arrEncrypt, code = data, newCode = newData });
        }

        protected override void OnLoad(EventArgs e)
        {
            #region phan quyen - tam thoi ko dung
            /*
            List<Tblright> result = new List<Tblright>();
            if (ApplicationContext.Current.User != null)
            {
                result = RoleManager.GetRightByFunctionCodeAndUserID("ThietKeSanXuat",
                    ApplicationContext.Current.User.Id).ToList<Tblright>();

                if (result != null && result.Count > 0)
                {
                    bool isEnable = false;
                    bool isDeletable = false;
                    bool isResetable = false;
                    bool isAddable = false;
                    foreach (Tblright item in result)
                    {
                        if (isDeletable == true &&
                            isResetable == true &&
                            isAddable == true &&
                            isEnable == true)
                            break;

                        if (isEnable == false && item.Rightscode == "thiet_ke_workflow")
                            isEnable = true;
                        if (isResetable == false && item.Rightscode == "reset_workflow")
                            isResetable = true;
                        if (isAddable == false && item.Rightscode == "add_main_workflow")
                            isAddable = true;
                        if (isDeletable == false && item.Rightscode == "delete_workflow")
                            isDeletable = true;
                    }

                    if (isEnable)
                    {
                        StringBuilder sbRender = new StringBuilder();
                        sbRender.Append("<input type='hidden' ID='hdfAuthorize' value='" + Encode(SecurityHelper.Encrypt(isEnable.ToString())) + "' />");
                        sbRender.Append("<script type='text/javascript'>");
                        sbRender.Append("var add_main_workflow = " + isAddable.ToString().ToLower() + ";");
                        sbRender.Append("var reset_workflow = " + isResetable.ToString().ToLower() + ";");
                        sbRender.Append("var delete_workflow = " + isDeletable.ToString().ToLower() + ";");
                        sbRender.Append("</script>");
                        ltrAuthorize.Text = sbRender.ToString();
                    }
                }
            }
            */
            #endregion


            //full right
            StringBuilder sbRender = new StringBuilder();
            sbRender.Append("<input type='hidden' ID='hdfAuthorize' value='" + Encode(SecurityHelper.Encrypt(true.ToString())) + "' />");
            sbRender.Append("<script type='text/javascript'>");
            sbRender.Append("var add_main_workflow = " + true.ToString().ToLower() + ";");
            sbRender.Append("var reset_workflow = " + true.ToString().ToLower() + ";");
            sbRender.Append("var delete_workflow = " + true.ToString().ToLower() + ";");
            sbRender.Append("</script>");
            ltrAuthorize.Text = sbRender.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*
        public override string FUNCTION_PAGE
        {
            get
            {
                return FunctionPageName.THIET_KE_SAN_XUAT;
            }
        }
        */
    }
}