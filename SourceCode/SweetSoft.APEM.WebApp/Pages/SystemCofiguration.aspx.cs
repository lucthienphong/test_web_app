using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Helper;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class SystemCofiguration : ModalBasePage
    {
        public override string FUNCTION_PAGE_ID
        {
            get
            {
                return "system_configuration_manager";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyControlResourceText();
            if (!IsPostBack)
            {
                ViewState["PageID"] = (new Random()).Next().ToString();
                BindDDL();
                LoadData();
            }
        }

        private void ApplyControlResourceText()
        {
            btnTestEmail.Text = ResourceTextManager.GetApplicationText(ResourceText.SMTP_SEND_TEST_EMAIL);
        }

        private void BindDDL()
        {
            BindDepartmentDDL();
            BindCountryDDL();
            BindTaxDDL();
            BindCurrency();
            BindCustomer();
        }

        private void BindCustomer()
        {
            List<TblCustomer> list = CustomerManager.SelectCustomerForDDL();
            ddlCustomer.DataSource = list;
            ddlCustomer.DataTextField = "Name";
            ddlCustomer.DataValueField = "CustomerID";
            ddlCustomer.DataBind();
        }

        private void BindCountryDDL()
        {
            ddlBaseCountry.Items.Clear();
            ddlBaseCountry.Items.Add(new ListItem("--Select country--", "0"));
            TblReferenceCollection coll = ReferenceTableManager.SelectReferenceByType(ReferenceTypeHelper.Country);
            if (coll != null && coll.Count > 0)
            {
                foreach (var item in coll)
                    ddlBaseCountry.Items.Add(new ListItem(item.Name, item.ReferencesID.ToString()));
            }
        }

        private void BindCurrency()
        {
            TblCurrencyCollection list = new CurrencyManager().SelectAllForDDL();
            ddlBaseCurrency.DataSource = list;
            ddlBaseCurrency.DataTextField = "CurrencyName";
            ddlBaseCurrency.DataValueField = "CurrencyID";
            ddlBaseCurrency.DataBind();
        }

        private void BindTaxDDL()
        {
            ddlTax.DataSource = new TaxManager().SelectAllForDDL(true);
            ddlTax.DataTextField = "TaxName";
            ddlTax.DataValueField = "TaxID";
            ddlTax.DataBind();
        }

        private void BindDepartmentDDL()
        {
            TblDepartmentCollection colls = DepartmentManager.ListForDDL();
            ddlSaleRep.DataSource = colls;
            ddlSaleRep.DataTextField = "DepartmentName";
            ddlSaleRep.DataValueField = "DepartmentID";
            ddlSaleRep.DataBind();

            ddlJobCoordinator.DataSource = colls;
            ddlJobCoordinator.DataTextField = "DepartmentName";
            ddlJobCoordinator.DataValueField = "DepartmentID";
            ddlJobCoordinator.DataBind();
        }

        //Bind Data
        private void LoadData()
        {
            LoadContactInfo();
            LoadSMTPConfiguration();
            LoadGeneralSettings();
            //LoadDisplayConfiguration();
            //LoadPaymentSetting();
            //LoadProductStatus();
            //LoadOrderStatus();
            //LoadEmailTemplate();
            //LoadContractInfo();
            //LoadSMSInfor();
            //LoadBrowserDownload();
            // LoadShortDescription();
        }

        private void LoadContactInfo()
        {
            //Company Name
            TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyName);
            if (setting != null)
                txtCompanyName.Text = setting.SettingValue;
            //Company Code
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyCode);
            if (setting != null)
                txtCompanyCode.Text = setting.SettingValue;
            //Company Address
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyAddress);
            if (setting != null)
                txtCompanyAddress.Text = setting.SettingValue;
            //Company Phone
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyPhone);
            if (setting != null)
                txtPhoneNumber.Text = setting.SettingValue;
            //Company Fax
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyFax);
            if (setting != null)
                txtFax.Text = setting.SettingValue;
            //Company Website
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyWebsite);
            if (setting != null)
                txtWebsite.Text = setting.SettingValue;
            //Bank account number
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankAccountNumber);
            if (setting != null)
                txtAccountNumber.Text = setting.SettingValue;
            //Bank name
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankName);
            if (setting != null)
                txtBankName.Text = setting.SettingValue;
            //Bank address
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankAddress);
            if (setting != null)
                txtBankAddress.Text = setting.SettingValue;
            //Bank swift code
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankSwiftCode);
            if (setting != null)
                txtSwiftCode.Text = setting.SettingValue;
            //Bug reports email
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BugReportEmail);
            if (setting != null)
                txtBugReportEmail.Text = setting.SettingValue;
            //GST No
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyGST);
            if (setting != null)
                txtGSTNo.Text = setting.SettingValue;
            //TIN No
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyTIN);
            if (setting != null)
                txtTINNo.Text = setting.SettingValue;
            //Email
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyEmail);
            if (setting != null)
                txtEmail.Text = setting.SettingValue;
            //ISDN
            setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyISDN);
            if (setting != null)
                txtISDN.Text = setting.SettingValue;
        }

        //Bind SMTP Configuration
        private void LoadSMTPConfiguration()
        {
            try
            {
                //Load Mail root
                TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpMailServerAddress);
                if (setting != null)
                    txtSmtpMailServerAddress.Text = setting.SettingValue;
                //Load SMTP Port
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpPort);
                if (setting != null)
                    txtSmtpPort.Text = setting.SettingValue;
                //Load SSL
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpUsingSSL);
                if (setting != null)
                    chkSmtpUsingSSL.Checked = setting.SettingValue.ToUpper().Equals("TRUE");
                //Load Mail Sender
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpSenderAccount);
                if (setting != null)
                    txtSmtpSenderAccount.Text = setting.SettingValue;
                //Load PassWord
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpSenderPassword);
                if (setting != null)
                    txtSmtpSenderPassword.Text = setting.SettingValue;
            }
            catch (Exception e)
            {
                ProcessException(e);
            }
        }

        //Bind Settings
        private void LoadGeneralSettings()
        {
            try
            {
                //Load Sale dep
                TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SaleRepSetting);
                if (setting != null)
                    ddlSaleRep.SelectedValue = setting.SettingValue;
                else
                    ddlSaleRep.SelectedIndex = 0;

                //Load Pi value
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PiValueSetting);
                if (setting != null)
                {
                    txtPiValue.Text = setting.SettingValue;
                }
                else
                    txtPiValue.Text = "3.1416";

                //Load Base Country
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCountrySetting);
                if (setting != null)
                {
                    if (ddlBaseCountry.Items.FindByValue(setting.SettingValue) != null)
                        ddlBaseCountry.Items.FindByValue(setting.SettingValue).Selected = true;
                }
                else
                    ddlBaseCountry.SelectedIndex = 0;

                //Load default tax for overseas
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.DefaultTaxForOverseasSetting);
                if (setting != null)
                {
                    if (ddlTax.Items.FindByValue(setting.SettingValue) != null)
                        ddlTax.Items.FindByValue(setting.SettingValue).Selected = true;
                }
                else
                    ddlTax.SelectedIndex = 0;

                //Load Base Currency
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCurrencySetting);
                if (setting != null)
                {
                    if (ddlBaseCurrency.Items.FindByValue(setting.SettingValue) != null)
                        ddlBaseCurrency.Items.FindByValue(setting.SettingValue).Selected = true;
                }
                else
                    ddlBaseCurrency.SelectedIndex = 0;

                //Load Job Coordiantor dep
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.JobCoordinatorSetting);
                if (setting != null)
                    ddlJobCoordinator.SelectedValue = setting.SettingValue;
                else
                    ddlJobCoordinator.SelectedIndex = 0;

                //Load Customer master template
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PricingMasterTemplateSetting);
                if(setting != null)
                    ddlCustomer.SelectedValue = setting.SettingValue;
                else
                    ddlCustomer.SelectedIndex = 0; 
            }
            catch (Exception e)
            {
                ProcessException(e);
            }
        }

        protected void btnTestEmail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestingEmail.Text.Trim()))
            {
                AddValidationPromt(txtTestingEmail, ResourceTextManager.GetApplicationText(ResourceText.VALID_REQUIRE));
                ShowValidationPromtError();
                return;
            }

            try
            {
                string emailSender = "APEM System";
                string emailFromAddress = SettingManager.GetSettingValue(SettingNames.SmtpSenderAccount);
                string emailToAddress = txtTestingEmail.Text.Trim();
                string subject = string.Format("Test mail sent by [{0}] on APEM Administration", ApplicationContext.Current.UserName);
                // Message body content
                StringBuilder strMessage = new StringBuilder();
                strMessage.AppendLine(string.Format("This is a test mail sent by [{0}] on APEM Administration", ApplicationContext.Current.UserName));
                strMessage.AppendLine("The SMTP setting is correct!");
                strMessage.AppendLine();
                strMessage.AppendLine(string.Format("(This email has been sent from {0})", ApplicationContext.Current.SiteUrl));
                strMessage.AppendLine();
                CommonHelper.SendSmtpMail(emailSender, emailFromAddress, emailToAddress, subject, strMessage.ToString(), false);
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), string.Format("{0} {1}", ResourceTextManager.GetApplicationText(ResourceText.SMTP_TEST_EMAIL_SEND_SUCCESS), txtTestingEmail.Text), MSGButton.OK, MSGIcon.Success);
                OpenMessageBox(msg, null, false, false);
            }
            catch (Exception exc)
            {
                MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.SMTP_FAIL_SEND_TEST_EMAIL), MSGButton.OK, MSGIcon.Error);
                OpenMessageBox(msg, null, false, false);
                ProcessException(exc);
            }
        }

        //--------------------------------------SAVE DATA-----------------------------------------------------------------------------------------
        //Save contact infomation
        private bool SaveContactInfo()
        {
            try
            {
                //Company Name
                TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyName);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtCompanyName.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyName, typeof(String).ToString(), txtCompanyName.Text.Trim());
                //Company Code
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyCode);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtCompanyCode.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyCode, typeof(String).ToString(), txtCompanyCode.Text.Trim());
                //Company Address
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyAddress);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtCompanyAddress.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyAddress, typeof(String).ToString(), txtCompanyAddress.Text.Trim());
                //Company Phone
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyPhone);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtPhoneNumber.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyPhone, typeof(String).ToString(), txtPhoneNumber.Text.Trim());
                //Company Fax
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyFax);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtFax.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyFax, typeof(String).ToString(), txtFax.Text.Trim());
                //Company Website
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyWebsite);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtWebsite.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyWebsite, typeof(String).ToString(), txtWebsite.Text.Trim());
                //GST No
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyGST);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtGSTNo.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyGST, typeof(String).ToString(), txtGSTNo.Text.Trim());
                //TIN No
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyTIN);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtTINNo.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyTIN, typeof(String).ToString(), txtTINNo.Text.Trim());
                //EMail
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyEmail);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtEmail.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyEmail, typeof(String).ToString(), txtEmail.Text.Trim());
                //ISDN
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.CompanyISDN);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtISDN.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.CompanyISDN, typeof(String).ToString(), txtISDN.Text.Trim());
                //Bank account number
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankAccountNumber);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtAccountNumber.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.BankAccountNumber, typeof(String).ToString(), txtAccountNumber.Text.Trim());
                //Bank Name
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankName);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtBankName.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.BankName, typeof(String).ToString(), txtBankName.Text.Trim());
                //Bank Address
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankAddress);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtBankAddress.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.BankAddress, typeof(String).ToString(), txtBankAddress.Text.Trim());
                //Bank Swift Code
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BankSwiftCode);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtSwiftCode.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.BankSwiftCode, typeof(String).ToString(), txtSwiftCode.Text.Trim());
                //Bug report email
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BugReportEmail);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtBugReportEmail.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.BugReportEmail, typeof(String).ToString(), txtBugReportEmail.Text.Trim());

                return true;
            }
            catch (Exception e)
            {
                ProcessException(e);
                return false;
            }
        }

        //Save SMTP
        private bool SaveSMTPConfiguration()
        {
            try
            {
                //SmtpMailServerAddress
                TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpMailServerAddress);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtSmtpMailServerAddress.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.SmtpMailServerAddress, typeof(String).ToString(), txtSmtpMailServerAddress.Text.Trim());
                //SMTP Port
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpPort);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtSmtpPort.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.SmtpPort, typeof(String).ToString(), txtSmtpPort.Text.Trim());
                //SSl
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpUsingSSL);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, chkSmtpUsingSSL.Checked.ToString());
                else
                    SettingManager.InsertSetting(SettingNames.SmtpUsingSSL, typeof(String).ToString(), chkSmtpUsingSSL.Checked.ToString());
                //Email Sender
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpSenderAccount);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtSmtpSenderAccount.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.SmtpSenderAccount, typeof(String).ToString(), txtSmtpSenderAccount.Text.Trim());
                // PassWord
                if (!string.IsNullOrEmpty(txtSmtpSenderPassword.Text.Trim()))
                {
                    setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SmtpSenderPassword);
                    if (setting != null)
                        SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtSmtpSenderPassword.Text.Trim());
                    else
                        SettingManager.InsertSetting(SettingNames.SmtpSenderPassword, typeof(String).ToString(), txtSmtpSenderPassword.Text.Trim());
                }
                return true;
            }
            catch (Exception e)
            {
                ProcessException(e);
            }
            return false;
        }

        //Save general setting
        private bool SaveGeneralSetting()
        {
            try
            {
                //SaleRep
                TblSystemSetting setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.SaleRepSetting);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, ddlSaleRep.SelectedValue);
                else
                    SettingManager.InsertSetting(SettingNames.SaleRepSetting, typeof(String).ToString(), ddlSaleRep.SelectedValue);

                //PiValue
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PiValueSetting);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, txtPiValue.Text.Trim());
                else
                    SettingManager.InsertSetting(SettingNames.PiValueSetting, typeof(String).ToString(), txtPiValue.Text.Trim());

                //BaseCountry
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCountrySetting);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, ddlBaseCountry.SelectedValue);
                else
                    SettingManager.InsertSetting(SettingNames.BaseCountrySetting, typeof(String).ToString(), ddlBaseCountry.SelectedValue);

                //DefaultTax
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.DefaultTaxForOverseasSetting);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, ddlTax.SelectedValue);
                else
                    SettingManager.InsertSetting(SettingNames.DefaultTaxForOverseasSetting, typeof(String).ToString(), ddlTax.SelectedValue);

                //BaseCurrency
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.BaseCurrencySetting);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, ddlBaseCurrency.SelectedValue);
                else
                    SettingManager.InsertSetting(SettingNames.BaseCurrencySetting, typeof(String).ToString(), ddlBaseCurrency.SelectedValue);

                //JobCoordinator
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.JobCoordinatorSetting);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, ddlJobCoordinator.SelectedValue);
                else
                    SettingManager.InsertSetting(SettingNames.JobCoordinatorSetting, typeof(String).ToString(), ddlJobCoordinator.SelectedValue);

                //Pricing master template
                setting = SettingManager.ApplicationSettings.FindBySettingName(SettingNames.PricingMasterTemplateSetting);
                if (setting != null)
                    SettingManager.UpdateSetting(setting.SettingID, setting.SettingName, setting.SettingType, ddlCustomer.SelectedValue);
                else
                    SettingManager.InsertSetting(SettingNames.PricingMasterTemplateSetting, typeof(String).ToString(), ddlCustomer.SelectedValue);

                return true;
            }
            catch (Exception e)
            {
                ProcessException(e);
            }
            return false;
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //Kiểm tra quyền
                if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, FUNCTION_PAGE_ID))
                {
                    MessageBox msgRole = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msgRole, null, false, false);
                    return;
                }

                bool isSuccess = false;
                isSuccess = SaveContactInfo();
                isSuccess = SaveSMTPConfiguration();
                isSuccess = SaveGeneralSetting();
                //isSuccess = SavePaymentSetting();
                //isSuccess = SaveProductStatus();
                //isSuccess = SaveEmailTemplate();
                //isSuccess = SaveDisplayConfiguration();
                //isSuccess = SaveContractInfo();
                //isSuccess = SaveContractStatus();
                //isSuccess = SaveSMSInfo();
                //isSuccess = SaveBrowser();
                //isSuccess = SaveShortDescription();
                if (isSuccess)
                {
                    AppCache.Clear();
                    LoadData();
                    SaveLogging(ActivityLoggingHelper.UPDATE, "Update system configuration.");
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_SAVED_SUCCESS), MSGButton.OK, MSGIcon.Success);
                    OpenMessageBox(msg, null, false, false);
                }
                else
                {
                    MessageBox msg = new MessageBox(ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE), ResourceTextManager.GetApplicationText(ResourceText.DATA_CAN_NOT_SAVED), MSGButton.OK, MSGIcon.Error);
                    OpenMessageBox(msg, null, false, false);
                }
            }
            catch(Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void btnCancel_ServerClick(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}