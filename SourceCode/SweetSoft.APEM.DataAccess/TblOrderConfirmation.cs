using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
// <auto-generated />
namespace SweetSoft.APEM.DataAccess
{
	/// <summary>
	/// Strongly-typed collection for the TblOrderConfirmation class.
	/// </summary>
    [Serializable]
	public partial class TblOrderConfirmationCollection : ActiveList<TblOrderConfirmation, TblOrderConfirmationCollection>
	{	   
		public TblOrderConfirmationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblOrderConfirmationCollection</returns>
		public TblOrderConfirmationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblOrderConfirmation o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the tblOrderConfirmation table.
	/// </summary>
	[Serializable]
	public partial class TblOrderConfirmation : ActiveRecord<TblOrderConfirmation>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblOrderConfirmation()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblOrderConfirmation(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblOrderConfirmation(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblOrderConfirmation(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("tblOrderConfirmation", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarJobID = new TableSchema.TableColumn(schema);
				colvarJobID.ColumnName = "JobID";
				colvarJobID.DataType = DbType.Int32;
				colvarJobID.MaxLength = 0;
				colvarJobID.AutoIncrement = false;
				colvarJobID.IsNullable = false;
				colvarJobID.IsPrimaryKey = true;
				colvarJobID.IsForeignKey = true;
				colvarJobID.IsReadOnly = false;
				colvarJobID.DefaultSetting = @"";
				
					colvarJobID.ForeignKeyTableName = "tblJob";
				schema.Columns.Add(colvarJobID);
				
				TableSchema.TableColumn colvarOCNumber = new TableSchema.TableColumn(schema);
				colvarOCNumber.ColumnName = "OCNumber";
				colvarOCNumber.DataType = DbType.AnsiString;
				colvarOCNumber.MaxLength = 50;
				colvarOCNumber.AutoIncrement = false;
				colvarOCNumber.IsNullable = false;
				colvarOCNumber.IsPrimaryKey = false;
				colvarOCNumber.IsForeignKey = false;
				colvarOCNumber.IsReadOnly = false;
				colvarOCNumber.DefaultSetting = @"";
				colvarOCNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOCNumber);
				
				TableSchema.TableColumn colvarCustomerPO1 = new TableSchema.TableColumn(schema);
				colvarCustomerPO1.ColumnName = "CustomerPO1";
				colvarCustomerPO1.DataType = DbType.String;
				colvarCustomerPO1.MaxLength = 50;
				colvarCustomerPO1.AutoIncrement = false;
				colvarCustomerPO1.IsNullable = true;
				colvarCustomerPO1.IsPrimaryKey = false;
				colvarCustomerPO1.IsForeignKey = false;
				colvarCustomerPO1.IsReadOnly = false;
				colvarCustomerPO1.DefaultSetting = @"";
				colvarCustomerPO1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerPO1);
				
				TableSchema.TableColumn colvarCustomerPO2 = new TableSchema.TableColumn(schema);
				colvarCustomerPO2.ColumnName = "CustomerPO2";
				colvarCustomerPO2.DataType = DbType.String;
				colvarCustomerPO2.MaxLength = 50;
				colvarCustomerPO2.AutoIncrement = false;
				colvarCustomerPO2.IsNullable = true;
				colvarCustomerPO2.IsPrimaryKey = false;
				colvarCustomerPO2.IsForeignKey = false;
				colvarCustomerPO2.IsReadOnly = false;
				colvarCustomerPO2.DefaultSetting = @"";
				colvarCustomerPO2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerPO2);
				
				TableSchema.TableColumn colvarContactPersonID = new TableSchema.TableColumn(schema);
				colvarContactPersonID.ColumnName = "ContactPersonID";
				colvarContactPersonID.DataType = DbType.Int32;
				colvarContactPersonID.MaxLength = 0;
				colvarContactPersonID.AutoIncrement = false;
				colvarContactPersonID.IsNullable = false;
				colvarContactPersonID.IsPrimaryKey = false;
				colvarContactPersonID.IsForeignKey = false;
				colvarContactPersonID.IsReadOnly = false;
				colvarContactPersonID.DefaultSetting = @"";
				colvarContactPersonID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactPersonID);
				
				TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
				colvarOrderDate.ColumnName = "OrderDate";
				colvarOrderDate.DataType = DbType.DateTime;
				colvarOrderDate.MaxLength = 0;
				colvarOrderDate.AutoIncrement = false;
				colvarOrderDate.IsNullable = false;
				colvarOrderDate.IsPrimaryKey = false;
				colvarOrderDate.IsForeignKey = false;
				colvarOrderDate.IsReadOnly = false;
				colvarOrderDate.DefaultSetting = @"";
				colvarOrderDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderDate);
				
				TableSchema.TableColumn colvarTaxID = new TableSchema.TableColumn(schema);
				colvarTaxID.ColumnName = "TaxID";
				colvarTaxID.DataType = DbType.Int16;
				colvarTaxID.MaxLength = 0;
				colvarTaxID.AutoIncrement = false;
				colvarTaxID.IsNullable = true;
				colvarTaxID.IsPrimaryKey = false;
				colvarTaxID.IsForeignKey = false;
				colvarTaxID.IsReadOnly = false;
				colvarTaxID.DefaultSetting = @"";
				colvarTaxID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTaxID);
				
				TableSchema.TableColumn colvarTaxPercentage = new TableSchema.TableColumn(schema);
				colvarTaxPercentage.ColumnName = "TaxPercentage";
				colvarTaxPercentage.DataType = DbType.Double;
				colvarTaxPercentage.MaxLength = 0;
				colvarTaxPercentage.AutoIncrement = false;
				colvarTaxPercentage.IsNullable = true;
				colvarTaxPercentage.IsPrimaryKey = false;
				colvarTaxPercentage.IsForeignKey = false;
				colvarTaxPercentage.IsReadOnly = false;
				colvarTaxPercentage.DefaultSetting = @"";
				colvarTaxPercentage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTaxPercentage);
				
				TableSchema.TableColumn colvarCurrencyID = new TableSchema.TableColumn(schema);
				colvarCurrencyID.ColumnName = "CurrencyID";
				colvarCurrencyID.DataType = DbType.Int16;
				colvarCurrencyID.MaxLength = 0;
				colvarCurrencyID.AutoIncrement = false;
				colvarCurrencyID.IsNullable = false;
				colvarCurrencyID.IsPrimaryKey = false;
				colvarCurrencyID.IsForeignKey = true;
				colvarCurrencyID.IsReadOnly = false;
				colvarCurrencyID.DefaultSetting = @"";
				
					colvarCurrencyID.ForeignKeyTableName = "tblCurrency";
				schema.Columns.Add(colvarCurrencyID);
				
				TableSchema.TableColumn colvarRMValue = new TableSchema.TableColumn(schema);
				colvarRMValue.ColumnName = "RMValue";
				colvarRMValue.DataType = DbType.Decimal;
				colvarRMValue.MaxLength = 0;
				colvarRMValue.AutoIncrement = false;
				colvarRMValue.IsNullable = false;
				colvarRMValue.IsPrimaryKey = false;
				colvarRMValue.IsForeignKey = false;
				colvarRMValue.IsReadOnly = false;
				colvarRMValue.DefaultSetting = @"";
				colvarRMValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRMValue);
				
				TableSchema.TableColumn colvarCurrencyValue = new TableSchema.TableColumn(schema);
				colvarCurrencyValue.ColumnName = "CurrencyValue";
				colvarCurrencyValue.DataType = DbType.Decimal;
				colvarCurrencyValue.MaxLength = 0;
				colvarCurrencyValue.AutoIncrement = false;
				colvarCurrencyValue.IsNullable = false;
				colvarCurrencyValue.IsPrimaryKey = false;
				colvarCurrencyValue.IsForeignKey = false;
				colvarCurrencyValue.IsReadOnly = false;
				colvarCurrencyValue.DefaultSetting = @"";
				colvarCurrencyValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrencyValue);
				
				TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
				colvarDiscount.ColumnName = "Discount";
				colvarDiscount.DataType = DbType.Double;
				colvarDiscount.MaxLength = 0;
				colvarDiscount.AutoIncrement = false;
				colvarDiscount.IsNullable = true;
				colvarDiscount.IsPrimaryKey = false;
				colvarDiscount.IsForeignKey = false;
				colvarDiscount.IsReadOnly = false;
				colvarDiscount.DefaultSetting = @"";
				colvarDiscount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscount);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = 2000;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarRemarkScreen = new TableSchema.TableColumn(schema);
				colvarRemarkScreen.ColumnName = "RemarkScreen";
				colvarRemarkScreen.DataType = DbType.String;
				colvarRemarkScreen.MaxLength = 2000;
				colvarRemarkScreen.AutoIncrement = false;
				colvarRemarkScreen.IsNullable = true;
				colvarRemarkScreen.IsPrimaryKey = false;
				colvarRemarkScreen.IsForeignKey = false;
				colvarRemarkScreen.IsReadOnly = false;
				colvarRemarkScreen.DefaultSetting = @"";
				colvarRemarkScreen.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarkScreen);
				
				TableSchema.TableColumn colvarDeliveryTerm = new TableSchema.TableColumn(schema);
				colvarDeliveryTerm.ColumnName = "DeliveryTerm";
				colvarDeliveryTerm.DataType = DbType.String;
				colvarDeliveryTerm.MaxLength = 100;
				colvarDeliveryTerm.AutoIncrement = false;
				colvarDeliveryTerm.IsNullable = true;
				colvarDeliveryTerm.IsPrimaryKey = false;
				colvarDeliveryTerm.IsForeignKey = false;
				colvarDeliveryTerm.IsReadOnly = false;
				colvarDeliveryTerm.DefaultSetting = @"";
				colvarDeliveryTerm.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeliveryTerm);
				
				TableSchema.TableColumn colvarPaymentTerm = new TableSchema.TableColumn(schema);
				colvarPaymentTerm.ColumnName = "PaymentTerm";
				colvarPaymentTerm.DataType = DbType.String;
				colvarPaymentTerm.MaxLength = 100;
				colvarPaymentTerm.AutoIncrement = false;
				colvarPaymentTerm.IsNullable = true;
				colvarPaymentTerm.IsPrimaryKey = false;
				colvarPaymentTerm.IsForeignKey = false;
				colvarPaymentTerm.IsReadOnly = false;
				colvarPaymentTerm.DefaultSetting = @"";
				colvarPaymentTerm.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPaymentTerm);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.String;
				colvarCreatedBy.MaxLength = 100;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.String;
				colvarModifiedBy.MaxLength = 100;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = true;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
				TableSchema.TableColumn colvarTotalPrice = new TableSchema.TableColumn(schema);
				colvarTotalPrice.ColumnName = "TotalPrice";
				colvarTotalPrice.DataType = DbType.Decimal;
				colvarTotalPrice.MaxLength = 0;
				colvarTotalPrice.AutoIncrement = false;
				colvarTotalPrice.IsNullable = true;
				colvarTotalPrice.IsPrimaryKey = false;
				colvarTotalPrice.IsForeignKey = false;
				colvarTotalPrice.IsReadOnly = false;
				colvarTotalPrice.DefaultSetting = @"";
				colvarTotalPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalPrice);
				
				TableSchema.TableColumn colvarInvoiceCurrency = new TableSchema.TableColumn(schema);
				colvarInvoiceCurrency.ColumnName = "InvoiceCurrency";
				colvarInvoiceCurrency.DataType = DbType.Boolean;
				colvarInvoiceCurrency.MaxLength = 0;
				colvarInvoiceCurrency.AutoIncrement = false;
				colvarInvoiceCurrency.IsNullable = false;
				colvarInvoiceCurrency.IsPrimaryKey = false;
				colvarInvoiceCurrency.IsForeignKey = false;
				colvarInvoiceCurrency.IsReadOnly = false;
				
						colvarInvoiceCurrency.DefaultSetting = @"((0))";
				colvarInvoiceCurrency.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInvoiceCurrency);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblOrderConfirmation",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("JobID")]
		[Bindable(true)]
		public int JobID 
		{
			get { return GetColumnValue<int>(Columns.JobID); }
			set { SetColumnValue(Columns.JobID, value); }
		}
		  
		[XmlAttribute("OCNumber")]
		[Bindable(true)]
		public string OCNumber 
		{
			get { return GetColumnValue<string>(Columns.OCNumber); }
			set { SetColumnValue(Columns.OCNumber, value); }
		}
		  
		[XmlAttribute("CustomerPO1")]
		[Bindable(true)]
		public string CustomerPO1 
		{
			get { return GetColumnValue<string>(Columns.CustomerPO1); }
			set { SetColumnValue(Columns.CustomerPO1, value); }
		}
		  
		[XmlAttribute("CustomerPO2")]
		[Bindable(true)]
		public string CustomerPO2 
		{
			get { return GetColumnValue<string>(Columns.CustomerPO2); }
			set { SetColumnValue(Columns.CustomerPO2, value); }
		}
		  
		[XmlAttribute("ContactPersonID")]
		[Bindable(true)]
		public int ContactPersonID 
		{
			get { return GetColumnValue<int>(Columns.ContactPersonID); }
			set { SetColumnValue(Columns.ContactPersonID, value); }
		}
		  
		[XmlAttribute("OrderDate")]
		[Bindable(true)]
		public DateTime OrderDate 
		{
			get { return GetColumnValue<DateTime>(Columns.OrderDate); }
			set { SetColumnValue(Columns.OrderDate, value); }
		}
		  
		[XmlAttribute("TaxID")]
		[Bindable(true)]
		public short? TaxID 
		{
			get { return GetColumnValue<short?>(Columns.TaxID); }
			set { SetColumnValue(Columns.TaxID, value); }
		}
		  
		[XmlAttribute("TaxPercentage")]
		[Bindable(true)]
		public double? TaxPercentage 
		{
			get { return GetColumnValue<double?>(Columns.TaxPercentage); }
			set { SetColumnValue(Columns.TaxPercentage, value); }
		}
		  
		[XmlAttribute("CurrencyID")]
		[Bindable(true)]
		public short CurrencyID 
		{
			get { return GetColumnValue<short>(Columns.CurrencyID); }
			set { SetColumnValue(Columns.CurrencyID, value); }
		}
		  
		[XmlAttribute("RMValue")]
		[Bindable(true)]
		public decimal RMValue 
		{
			get { return GetColumnValue<decimal>(Columns.RMValue); }
			set { SetColumnValue(Columns.RMValue, value); }
		}
		  
		[XmlAttribute("CurrencyValue")]
		[Bindable(true)]
		public decimal CurrencyValue 
		{
			get { return GetColumnValue<decimal>(Columns.CurrencyValue); }
			set { SetColumnValue(Columns.CurrencyValue, value); }
		}
		  
		[XmlAttribute("Discount")]
		[Bindable(true)]
		public double? Discount 
		{
			get { return GetColumnValue<double?>(Columns.Discount); }
			set { SetColumnValue(Columns.Discount, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("RemarkScreen")]
		[Bindable(true)]
		public string RemarkScreen 
		{
			get { return GetColumnValue<string>(Columns.RemarkScreen); }
			set { SetColumnValue(Columns.RemarkScreen, value); }
		}
		  
		[XmlAttribute("DeliveryTerm")]
		[Bindable(true)]
		public string DeliveryTerm 
		{
			get { return GetColumnValue<string>(Columns.DeliveryTerm); }
			set { SetColumnValue(Columns.DeliveryTerm, value); }
		}
		  
		[XmlAttribute("PaymentTerm")]
		[Bindable(true)]
		public string PaymentTerm 
		{
			get { return GetColumnValue<string>(Columns.PaymentTerm); }
			set { SetColumnValue(Columns.PaymentTerm, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("TotalPrice")]
		[Bindable(true)]
		public decimal? TotalPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.TotalPrice); }
			set { SetColumnValue(Columns.TotalPrice, value); }
		}
		  
		[XmlAttribute("InvoiceCurrency")]
		[Bindable(true)]
		public bool InvoiceCurrency 
		{
			get { return GetColumnValue<bool>(Columns.InvoiceCurrency); }
			set { SetColumnValue(Columns.InvoiceCurrency, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private SweetSoft.APEM.DataAccess.TblOtherChargeCollection colTblOtherCharges;
		public SweetSoft.APEM.DataAccess.TblOtherChargeCollection TblOtherCharges()
		{
			if(colTblOtherCharges == null)
			{
				colTblOtherCharges = new SweetSoft.APEM.DataAccess.TblOtherChargeCollection().Where(TblOtherCharge.Columns.JobID, JobID).Load();
				colTblOtherCharges.ListChanged += new ListChangedEventHandler(colTblOtherCharges_ListChanged);
			}
			return colTblOtherCharges;
		}
				
		void colTblOtherCharges_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblOtherCharges[e.NewIndex].JobID = JobID;
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a TblCurrency ActiveRecord object related to this TblOrderConfirmation
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblCurrency TblCurrency
		{
			get { return SweetSoft.APEM.DataAccess.TblCurrency.FetchByID(this.CurrencyID); }
			set { SetColumnValue("CurrencyID", value.CurrencyID); }
		}
		
		
		/// <summary>
		/// Returns a TblJob ActiveRecord object related to this TblOrderConfirmation
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblJob TblJob
		{
			get { return SweetSoft.APEM.DataAccess.TblJob.FetchByID(this.JobID); }
			set { SetColumnValue("JobID", value.JobID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varJobID,string varOCNumber,string varCustomerPO1,string varCustomerPO2,int varContactPersonID,DateTime varOrderDate,short? varTaxID,double? varTaxPercentage,short varCurrencyID,decimal varRMValue,decimal varCurrencyValue,double? varDiscount,string varRemark,string varRemarkScreen,string varDeliveryTerm,string varPaymentTerm,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,decimal? varTotalPrice,bool varInvoiceCurrency)
		{
			TblOrderConfirmation item = new TblOrderConfirmation();
			
			item.JobID = varJobID;
			
			item.OCNumber = varOCNumber;
			
			item.CustomerPO1 = varCustomerPO1;
			
			item.CustomerPO2 = varCustomerPO2;
			
			item.ContactPersonID = varContactPersonID;
			
			item.OrderDate = varOrderDate;
			
			item.TaxID = varTaxID;
			
			item.TaxPercentage = varTaxPercentage;
			
			item.CurrencyID = varCurrencyID;
			
			item.RMValue = varRMValue;
			
			item.CurrencyValue = varCurrencyValue;
			
			item.Discount = varDiscount;
			
			item.Remark = varRemark;
			
			item.RemarkScreen = varRemarkScreen;
			
			item.DeliveryTerm = varDeliveryTerm;
			
			item.PaymentTerm = varPaymentTerm;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.TotalPrice = varTotalPrice;
			
			item.InvoiceCurrency = varInvoiceCurrency;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varJobID,string varOCNumber,string varCustomerPO1,string varCustomerPO2,int varContactPersonID,DateTime varOrderDate,short? varTaxID,double? varTaxPercentage,short varCurrencyID,decimal varRMValue,decimal varCurrencyValue,double? varDiscount,string varRemark,string varRemarkScreen,string varDeliveryTerm,string varPaymentTerm,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,decimal? varTotalPrice,bool varInvoiceCurrency)
		{
			TblOrderConfirmation item = new TblOrderConfirmation();
			
				item.JobID = varJobID;
			
				item.OCNumber = varOCNumber;
			
				item.CustomerPO1 = varCustomerPO1;
			
				item.CustomerPO2 = varCustomerPO2;
			
				item.ContactPersonID = varContactPersonID;
			
				item.OrderDate = varOrderDate;
			
				item.TaxID = varTaxID;
			
				item.TaxPercentage = varTaxPercentage;
			
				item.CurrencyID = varCurrencyID;
			
				item.RMValue = varRMValue;
			
				item.CurrencyValue = varCurrencyValue;
			
				item.Discount = varDiscount;
			
				item.Remark = varRemark;
			
				item.RemarkScreen = varRemarkScreen;
			
				item.DeliveryTerm = varDeliveryTerm;
			
				item.PaymentTerm = varPaymentTerm;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.TotalPrice = varTotalPrice;
			
				item.InvoiceCurrency = varInvoiceCurrency;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn JobIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn OCNumberColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerPO1Column
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerPO2Column
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactPersonIDColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDateColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TaxIDColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TaxPercentageColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyIDColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn RMValueColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyValueColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkScreenColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn DeliveryTermColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn PaymentTermColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalPriceColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn InvoiceCurrencyColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string JobID = @"JobID";
			 public static string OCNumber = @"OCNumber";
			 public static string CustomerPO1 = @"CustomerPO1";
			 public static string CustomerPO2 = @"CustomerPO2";
			 public static string ContactPersonID = @"ContactPersonID";
			 public static string OrderDate = @"OrderDate";
			 public static string TaxID = @"TaxID";
			 public static string TaxPercentage = @"TaxPercentage";
			 public static string CurrencyID = @"CurrencyID";
			 public static string RMValue = @"RMValue";
			 public static string CurrencyValue = @"CurrencyValue";
			 public static string Discount = @"Discount";
			 public static string Remark = @"Remark";
			 public static string RemarkScreen = @"RemarkScreen";
			 public static string DeliveryTerm = @"DeliveryTerm";
			 public static string PaymentTerm = @"PaymentTerm";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string TotalPrice = @"TotalPrice";
			 public static string InvoiceCurrency = @"InvoiceCurrency";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colTblOtherCharges != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblOtherCharge item in colTblOtherCharges)
                    {
                        if (item.JobID == null ||item.JobID != JobID)
                        {
                            item.JobID = JobID;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colTblOtherCharges != null)
                {
                    colTblOtherCharges.SaveAll();
               }
		}
        #endregion
	}
}