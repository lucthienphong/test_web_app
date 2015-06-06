using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using System.Data;

namespace SweetSoft.APEM.Core.Manager
{
    public class SettingNames
    {
        private static string SettingNamePrefix = "SweetSoft.APEM.Settings.{0}";

        //General settings
        public static string ApplicationTitle = string.Format(SettingNamePrefix, "ApplicationTitle");
        public static string DataGridItemsPerPage = string.Format(SettingNamePrefix, "DataGridItemsPerPage");
        public static string DataComboboxItemsPerPage = string.Format(SettingNamePrefix, "DataComboboxItemsPerPage");
        public static string MaxAllowCreatedColor = string.Format(SettingNamePrefix, "MaxAllowCreatedColor");
        public static string VAT = string.Format(SettingNamePrefix, "VAT");
        public static string AllowSaveLogging = string.Format(SettingNamePrefix, "AllowSaveLogging");
        public static string ReservationMaxTime = string.Format(SettingNamePrefix, "ReservationMaxTime");
        public static string BillID = string.Format(SettingNamePrefix, "BillID");
        public static string TableID = string.Format(SettingNamePrefix, "TableID");
        //Internal Announcement
        public static string InternalAnnouncement = string.Format(SettingNamePrefix, "InternalAnnouncement");

        //Contact info
        public static string CompanyName = string.Format(SettingNamePrefix, "CompanyName");
        public static string CompanyCode = string.Format(SettingNamePrefix, "CompanyCode");
        public static string CompanyAddress = string.Format(SettingNamePrefix, "CompanyAddress");
        public static string CompanyPhone = string.Format(SettingNamePrefix, "CompanyPhone");
        public static string CompanyFax = string.Format(SettingNamePrefix,"CompanyFax");
        public static string CompanyWebsite = string.Format(SettingNamePrefix, "CompanyWebsite");
        public static string CompanyGST = string.Format(SettingNamePrefix, "CompanyGST");
        public static string CompanyTIN = string.Format(SettingNamePrefix, "CompanyTIN");
        public static string CompanyEmail = string.Format(SettingNamePrefix, "CompanyEmail");
        public static string CompanyISDN = string.Format(SettingNamePrefix, "CompanyISDN");

        public static string BankAccountNumber = string.Format(SettingNamePrefix, "BankAccountNumber");
        public static string BankName = string.Format(SettingNamePrefix, "BankName");
        public static string BankAddress = string.Format(SettingNamePrefix, "BankAddress");
        public static string BankSwiftCode = string.Format(SettingNamePrefix, "BankSwiftCode");

        public static string ApplicationEmail = string.Format(SettingNamePrefix, "ApplicationEmail");
        public static string AdministratorEmail = string.Format(SettingNamePrefix, "AdministratorEmail");
        public static string BugReportEmail = string.Format(SettingNamePrefix, "BugReportEmail");

        //SMTP Settings
        public static string SmtpMailServerAddress = string.Format(SettingNamePrefix, "SmtpMailServerAddress");
        public static string SmtpPort = string.Format(SettingNamePrefix, "SmtpPort");
        public static string SmtpUsingSSL = string.Format(SettingNamePrefix, "SmtpUsingSSL");
        public static string SmtpSenderAccount = string.Format(SettingNamePrefix, "SmtpSenderAccount");
        public static string SmtpSenderPassword = string.Format(SettingNamePrefix, "SmtpSenderPassword");

        //General Setting
        public static string PiValueSetting = string.Format(SettingNamePrefix, "PiValueSetting");
        public static string SaleRepSetting = string.Format(SettingNamePrefix, "SaleRepSetting");
        public static string JobCoordinatorSetting = string.Format(SettingNamePrefix, "JobCoordinatorSetting");
        public static string BaseCountrySetting = string.Format(SettingNamePrefix, "BaseCountrySetting");
        public static string BaseCurrencySetting = string.Format(SettingNamePrefix, "BaseCurrencySetting");
        public static string DeReChromeSetting = string.Format(SettingNamePrefix, "DeReChromeSetting");
        public static string PricingMasterTemplateSetting = string.Format(SettingNamePrefix, "PricingMasterTemplateSetting");
        public static string DefaultTaxForOverseasSetting = string.Format(SettingNamePrefix, "DefaultTaxForOverseasSetting");
        public static string DefaultProductTypeSetting = string.Format(SettingNamePrefix, "DefaultProductTypeSetting");
        //Product Setting
        public static string InterestingProduct = string.Format(SettingNamePrefix, "InterestingProduct");
        public static string DepositingProduct = string.Format(SettingNamePrefix, "DepositingProduct");
        public static string SoldProduct = string.Format(SettingNamePrefix, "SoldProduct");

        //ShortDiscription
        public static string DescriptionTitle = string.Format(SettingNamePrefix, "DescriptionTitle");
        public static string DescriptionContent1 = string.Format(SettingNamePrefix, "DescriptionContent1");
        public static string DescriptionContent2 = string.Format(SettingNamePrefix, "DescriptionContent2");
        public static string DescriptionIcon = string.Format(SettingNamePrefix, "DescriptionIcon");

        //Email template
        public static string EmailTemplate1 = string.Format(SettingNamePrefix, "EmailTemplate1");
        public static string EmailTemplate2 = string.Format(SettingNamePrefix, "EmailTemplate2");

        //Contract
        public static string GroupSeller = string.Format(SettingNamePrefix, "GroupSeller");
        public static string ContractDeadline = string.Format(SettingNamePrefix, "ContractDeadLine");
        public static string ContractDeposit = string.Format(SettingNamePrefix, "ContractDeposit");
        public static string ContractProcessed = string.Format(SettingNamePrefix, "ContractProcessed");
        public static string ContractClosed = string.Format(SettingNamePrefix, "ContractClosed");
        public static string ContractRegisted = string.Format(SettingNamePrefix, "ContractRegisted");
        public static string SMSSendDays = string.Format(SettingNamePrefix, "SMSSendDays");

        //SMS
        public static string SMSAcount = string.Format(SettingNamePrefix, "SMSAcount");
        public static string SMSCodeAPI = string.Format(SettingNamePrefix, "SMSCodeAPI");
        public static string SMSBrandName = string.Format(SettingNamePrefix, "SMSBrandName");
        public static string SMSContent = string.Format(SettingNamePrefix, "SMSContent");


        
    }
    public class SettingManager
    {
            private static TblSystemSettingCollection m_CurrentSettings = null;
        private const string APP_SETTINGS_CACHEKEY = "APP_SETTINGS_CACHEKEY";

        public static TblSystemSettingCollection ApplicationSettings
        {
            get
            {
                m_CurrentSettings = AppCache.Get(APP_SETTINGS_CACHEKEY) as TblSystemSettingCollection;
                if (m_CurrentSettings == null || m_CurrentSettings.Count == 1)
                {
                    m_CurrentSettings = SettingManager.GetAllSettings();
                    AppCache.Remove(APP_SETTINGS_CACHEKEY);
                    AppCache.Max(APP_SETTINGS_CACHEKEY, m_CurrentSettings);
                }
                return m_CurrentSettings;
            }
            set
            {
                m_CurrentSettings = value;
                UpdateSettingsInCache(m_CurrentSettings);
            }
        }
        public static void UpdateSettingsInCache(TblSystemSettingCollection settings)
        {
            AppCache.Remove(APP_SETTINGS_CACHEKEY);
            if (settings == null)
                settings = GetAllSettings();

            AppCache.Max(APP_SETTINGS_CACHEKEY, settings);
        }

        /// <summary>
        /// Add new setting
        /// </summary>
        /// <param name="setting"></param>
        public static void InsertSetting(string settingName, string settingType, string settingValue)
        {
            new TblSystemSettingController().Insert(settingName, settingType, settingValue);
        }

        /// <summary>
        /// Update setting
        /// </summary>
        /// <param name="setting"></param>
        public static void UpdateSetting(int settingId, string settingName, string settingType, string settingValue)
        {
            new TblSystemSettingController().Update(settingId, settingName, settingType, settingValue);
        }

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        public static TblSystemSettingCollection GetAllSettings()
        {
            return new TblSystemSettingController().FetchAll();
        }

        /// <summary>
        /// Get setting by Setting Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TblSystemSetting GetSettingByName(string name)
        {
            TblSystemSettingCollection settings = new SubSonic.Select().From<TblSystemSetting>()
                                                    .Where(TblSystemSetting.SettingNameColumn)
                                                    .IsEqualTo(name)
                                                    .ExecuteAsCollection<TblSystemSettingCollection>();

            return settings != null && settings.Count > 0 ? settings[0] : null;
        }

        /// <summary>
        /// Gets a boolean value of a setting
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting value</returns>
        public static bool GetSettingValueBoolean(string name)
        {
            return GetSettingValueBoolean(name, true);
        }

        /// <summary>
        /// Gets a boolean value of a setting
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <param name="DefaultValue">The default value</param>
        /// <returns>The setting value</returns>
        public static bool GetSettingValueBoolean(string name, bool defaultValue)
        {
            string value = GetSettingValue(name);
            bool ret = defaultValue;
            if (!string.IsNullOrEmpty(value))
                bool.TryParse(value, out ret);
            return ret;
        }

        /// <summary>
        /// Gets a setting int value
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting int value</returns>
        public static int GetSettingValueInt(string name, int defaultValue)
        {
            int intValue = 0;
            int.TryParse(GetSettingValue(name), out intValue);

            return intValue == 0 ? defaultValue : intValue;
        }

        /// <summary>
        /// Gets a setting byte value
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting byte value</returns>
        public static byte GetSettingValueByte(string name, byte defaultValue)
        {
            byte byteValue = 0;
            byte.TryParse(GetSettingValue(name), out byteValue);

            return byteValue == 0 ? defaultValue : byteValue;
        }

        /// <summary>
        /// Gets a setting value
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting value</returns>
        public static string GetSettingValue(string Name)
        {
            TblSystemSetting objSetting = ApplicationSettings.FindBySettingName(Name);            
            return (objSetting != null) ? objSetting.SettingValue : string.Empty;
        }
    }
}
