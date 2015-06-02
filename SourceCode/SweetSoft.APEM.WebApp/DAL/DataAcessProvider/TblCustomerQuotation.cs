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
	/// Strongly-typed collection for the TblCustomerQuotation class.
	/// </summary>
    [Serializable]
	public partial class TblCustomerQuotationCollection : ActiveList<TblCustomerQuotation, TblCustomerQuotationCollection>
	{	   
		public TblCustomerQuotationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblCustomerQuotationCollection</returns>
		public TblCustomerQuotationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblCustomerQuotation o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblCustomerQuotation table.
	/// </summary>
	[Serializable]
	public partial class TblCustomerQuotation : ActiveRecord<TblCustomerQuotation>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblCustomerQuotation()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblCustomerQuotation(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblCustomerQuotation(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblCustomerQuotation(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblCustomerQuotation", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCustomerID = new TableSchema.TableColumn(schema);
				colvarCustomerID.ColumnName = "CustomerID";
				colvarCustomerID.DataType = DbType.Int32;
				colvarCustomerID.MaxLength = 0;
				colvarCustomerID.AutoIncrement = false;
				colvarCustomerID.IsNullable = false;
				colvarCustomerID.IsPrimaryKey = true;
				colvarCustomerID.IsForeignKey = true;
				colvarCustomerID.IsReadOnly = false;
				colvarCustomerID.DefaultSetting = @"";
				
					colvarCustomerID.ForeignKeyTableName = "tblCustomer";
				schema.Columns.Add(colvarCustomerID);
				
				TableSchema.TableColumn colvarQuotationNo = new TableSchema.TableColumn(schema);
				colvarQuotationNo.ColumnName = "QuotationNo";
				colvarQuotationNo.DataType = DbType.String;
				colvarQuotationNo.MaxLength = 50;
				colvarQuotationNo.AutoIncrement = false;
				colvarQuotationNo.IsNullable = true;
				colvarQuotationNo.IsPrimaryKey = false;
				colvarQuotationNo.IsForeignKey = false;
				colvarQuotationNo.IsReadOnly = false;
				colvarQuotationNo.DefaultSetting = @"";
				colvarQuotationNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuotationNo);
				
				TableSchema.TableColumn colvarDateOfQuotation = new TableSchema.TableColumn(schema);
				colvarDateOfQuotation.ColumnName = "DateOfQuotation";
				colvarDateOfQuotation.DataType = DbType.DateTime;
				colvarDateOfQuotation.MaxLength = 0;
				colvarDateOfQuotation.AutoIncrement = false;
				colvarDateOfQuotation.IsNullable = false;
				colvarDateOfQuotation.IsPrimaryKey = false;
				colvarDateOfQuotation.IsForeignKey = false;
				colvarDateOfQuotation.IsReadOnly = false;
				colvarDateOfQuotation.DefaultSetting = @"";
				colvarDateOfQuotation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateOfQuotation);
				
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
				
				TableSchema.TableColumn colvarContactSignation = new TableSchema.TableColumn(schema);
				colvarContactSignation.ColumnName = "ContactSignation";
				colvarContactSignation.DataType = DbType.String;
				colvarContactSignation.MaxLength = 150;
				colvarContactSignation.AutoIncrement = false;
				colvarContactSignation.IsNullable = false;
				colvarContactSignation.IsPrimaryKey = false;
				colvarContactSignation.IsForeignKey = false;
				colvarContactSignation.IsReadOnly = false;
				colvarContactSignation.DefaultSetting = @"";
				colvarContactSignation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactSignation);
				
				TableSchema.TableColumn colvarIrisProof = new TableSchema.TableColumn(schema);
				colvarIrisProof.ColumnName = "IrisProof";
				colvarIrisProof.DataType = DbType.Decimal;
				colvarIrisProof.MaxLength = 0;
				colvarIrisProof.AutoIncrement = false;
				colvarIrisProof.IsNullable = false;
				colvarIrisProof.IsPrimaryKey = false;
				colvarIrisProof.IsForeignKey = false;
				colvarIrisProof.IsReadOnly = false;
				colvarIrisProof.DefaultSetting = @"";
				colvarIrisProof.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIrisProof);
				
				TableSchema.TableColumn colvarUnitID = new TableSchema.TableColumn(schema);
				colvarUnitID.ColumnName = "UnitID";
				colvarUnitID.DataType = DbType.AnsiString;
				colvarUnitID.MaxLength = 50;
				colvarUnitID.AutoIncrement = false;
				colvarUnitID.IsNullable = true;
				colvarUnitID.IsPrimaryKey = false;
				colvarUnitID.IsForeignKey = false;
				colvarUnitID.IsReadOnly = false;
				colvarUnitID.DefaultSetting = @"";
				colvarUnitID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUnitID);
				
				TableSchema.TableColumn colvarCurrencyID = new TableSchema.TableColumn(schema);
				colvarCurrencyID.ColumnName = "CurrencyID";
				colvarCurrencyID.DataType = DbType.Int16;
				colvarCurrencyID.MaxLength = 0;
				colvarCurrencyID.AutoIncrement = false;
				colvarCurrencyID.IsNullable = true;
				colvarCurrencyID.IsPrimaryKey = false;
				colvarCurrencyID.IsForeignKey = false;
				colvarCurrencyID.IsReadOnly = false;
				colvarCurrencyID.DefaultSetting = @"";
				colvarCurrencyID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrencyID);
				
				TableSchema.TableColumn colvarQuotationText = new TableSchema.TableColumn(schema);
				colvarQuotationText.ColumnName = "QuotationText";
				colvarQuotationText.DataType = DbType.String;
				colvarQuotationText.MaxLength = 1000;
				colvarQuotationText.AutoIncrement = false;
				colvarQuotationText.IsNullable = false;
				colvarQuotationText.IsPrimaryKey = false;
				colvarQuotationText.IsForeignKey = false;
				colvarQuotationText.IsReadOnly = false;
				colvarQuotationText.DefaultSetting = @"";
				colvarQuotationText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuotationText);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblCustomerQuotation",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CustomerID")]
		[Bindable(true)]
		public int CustomerID 
		{
			get { return GetColumnValue<int>(Columns.CustomerID); }
			set { SetColumnValue(Columns.CustomerID, value); }
		}
		  
		[XmlAttribute("QuotationNo")]
		[Bindable(true)]
		public string QuotationNo 
		{
			get { return GetColumnValue<string>(Columns.QuotationNo); }
			set { SetColumnValue(Columns.QuotationNo, value); }
		}
		  
		[XmlAttribute("DateOfQuotation")]
		[Bindable(true)]
		public DateTime DateOfQuotation 
		{
			get { return GetColumnValue<DateTime>(Columns.DateOfQuotation); }
			set { SetColumnValue(Columns.DateOfQuotation, value); }
		}
		  
		[XmlAttribute("ContactPersonID")]
		[Bindable(true)]
		public int ContactPersonID 
		{
			get { return GetColumnValue<int>(Columns.ContactPersonID); }
			set { SetColumnValue(Columns.ContactPersonID, value); }
		}
		  
		[XmlAttribute("ContactSignation")]
		[Bindable(true)]
		public string ContactSignation 
		{
			get { return GetColumnValue<string>(Columns.ContactSignation); }
			set { SetColumnValue(Columns.ContactSignation, value); }
		}
		  
		[XmlAttribute("IrisProof")]
		[Bindable(true)]
		public decimal IrisProof 
		{
			get { return GetColumnValue<decimal>(Columns.IrisProof); }
			set { SetColumnValue(Columns.IrisProof, value); }
		}
		  
		[XmlAttribute("UnitID")]
		[Bindable(true)]
		public string UnitID 
		{
			get { return GetColumnValue<string>(Columns.UnitID); }
			set { SetColumnValue(Columns.UnitID, value); }
		}
		  
		[XmlAttribute("CurrencyID")]
		[Bindable(true)]
		public short? CurrencyID 
		{
			get { return GetColumnValue<short?>(Columns.CurrencyID); }
			set { SetColumnValue(Columns.CurrencyID, value); }
		}
		  
		[XmlAttribute("QuotationText")]
		[Bindable(true)]
		public string QuotationText 
		{
			get { return GetColumnValue<string>(Columns.QuotationText); }
			set { SetColumnValue(Columns.QuotationText, value); }
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
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private SweetSoft.APEM.DataAccess.TblCustomerQuotationPricingCollection colTblCustomerQuotationPricingRecords;
		public SweetSoft.APEM.DataAccess.TblCustomerQuotationPricingCollection TblCustomerQuotationPricingRecords()
		{
			if(colTblCustomerQuotationPricingRecords == null)
			{
				colTblCustomerQuotationPricingRecords = new SweetSoft.APEM.DataAccess.TblCustomerQuotationPricingCollection().Where(TblCustomerQuotationPricing.Columns.CustomerID, CustomerID).Load();
				colTblCustomerQuotationPricingRecords.ListChanged += new ListChangedEventHandler(colTblCustomerQuotationPricingRecords_ListChanged);
			}
			return colTblCustomerQuotationPricingRecords;
		}
				
		void colTblCustomerQuotationPricingRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblCustomerQuotationPricingRecords[e.NewIndex].CustomerID = CustomerID;
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a TblCustomer ActiveRecord object related to this TblCustomerQuotation
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblCustomer TblCustomer
		{
			get { return SweetSoft.APEM.DataAccess.TblCustomer.FetchByID(this.CustomerID); }
			set { SetColumnValue("CustomerID", value.CustomerID); }
		}
		
		
		#endregion
		
		
		
		#region Many To Many Helpers
		
		 
		public SweetSoft.APEM.DataAccess.TblPricingCollection GetTblPricingCollection() { return TblCustomerQuotation.GetTblPricingCollection(this.CustomerID); }
		public static SweetSoft.APEM.DataAccess.TblPricingCollection GetTblPricingCollection(int varCustomerID)
		{
		    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT * FROM [dbo].[tblPricing] INNER JOIN [tblCustomerQuotation_Pricing] ON [tblPricing].[PricingID] = [tblCustomerQuotation_Pricing].[PricingID] WHERE [tblCustomerQuotation_Pricing].[CustomerID] = @CustomerID", TblCustomerQuotation.Schema.Provider.Name);
			cmd.AddParameter("@CustomerID", varCustomerID, DbType.Int32);
			IDataReader rdr = SubSonic.DataService.GetReader(cmd);
			TblPricingCollection coll = new TblPricingCollection();
			coll.LoadAndCloseReader(rdr);
			return coll;
		}
		
		public static void SaveTblPricingMap(int varCustomerID, TblPricingCollection items)
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[CustomerID] = @CustomerID", TblCustomerQuotation.Schema.Provider.Name);
			cmdDel.AddParameter("@CustomerID", varCustomerID, DbType.Int32);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (TblPricing item in items)
			{
				TblCustomerQuotationPricing varTblCustomerQuotationPricing = new TblCustomerQuotationPricing();
				varTblCustomerQuotationPricing.SetColumnValue("CustomerID", varCustomerID);
				varTblCustomerQuotationPricing.SetColumnValue("PricingID", item.GetPrimaryKeyValue());
				varTblCustomerQuotationPricing.Save();
			}
		}
		public static void SaveTblPricingMap(int varCustomerID, System.Web.UI.WebControls.ListItemCollection itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[CustomerID] = @CustomerID", TblCustomerQuotation.Schema.Provider.Name);
			cmdDel.AddParameter("@CustomerID", varCustomerID, DbType.Int32);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (System.Web.UI.WebControls.ListItem l in itemList) 
			{
				if (l.Selected) 
				{
					TblCustomerQuotationPricing varTblCustomerQuotationPricing = new TblCustomerQuotationPricing();
					varTblCustomerQuotationPricing.SetColumnValue("CustomerID", varCustomerID);
					varTblCustomerQuotationPricing.SetColumnValue("PricingID", l.Value);
					varTblCustomerQuotationPricing.Save();
				}
			}
		}
		public static void SaveTblPricingMap(int varCustomerID , short[] itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[CustomerID] = @CustomerID", TblCustomerQuotation.Schema.Provider.Name);
			cmdDel.AddParameter("@CustomerID", varCustomerID, DbType.Int32);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (short item in itemList) 
			{
				TblCustomerQuotationPricing varTblCustomerQuotationPricing = new TblCustomerQuotationPricing();
				varTblCustomerQuotationPricing.SetColumnValue("CustomerID", varCustomerID);
				varTblCustomerQuotationPricing.SetColumnValue("PricingID", item);
				varTblCustomerQuotationPricing.Save();
			}
		}
		
		public static void DeleteTblPricingMap(int varCustomerID) 
		{
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[CustomerID] = @CustomerID", TblCustomerQuotation.Schema.Provider.Name);
			cmdDel.AddParameter("@CustomerID", varCustomerID, DbType.Int32);
			DataService.ExecuteQuery(cmdDel);
		}
		
		#endregion
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varCustomerID,string varQuotationNo,DateTime varDateOfQuotation,int varContactPersonID,string varContactSignation,decimal varIrisProof,string varUnitID,short? varCurrencyID,string varQuotationText,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblCustomerQuotation item = new TblCustomerQuotation();
			
			item.CustomerID = varCustomerID;
			
			item.QuotationNo = varQuotationNo;
			
			item.DateOfQuotation = varDateOfQuotation;
			
			item.ContactPersonID = varContactPersonID;
			
			item.ContactSignation = varContactSignation;
			
			item.IrisProof = varIrisProof;
			
			item.UnitID = varUnitID;
			
			item.CurrencyID = varCurrencyID;
			
			item.QuotationText = varQuotationText;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCustomerID,string varQuotationNo,DateTime varDateOfQuotation,int varContactPersonID,string varContactSignation,decimal varIrisProof,string varUnitID,short? varCurrencyID,string varQuotationText,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblCustomerQuotation item = new TblCustomerQuotation();
			
				item.CustomerID = varCustomerID;
			
				item.QuotationNo = varQuotationNo;
			
				item.DateOfQuotation = varDateOfQuotation;
			
				item.ContactPersonID = varContactPersonID;
			
				item.ContactSignation = varContactSignation;
			
				item.IrisProof = varIrisProof;
			
				item.UnitID = varUnitID;
			
				item.CurrencyID = varCurrencyID;
			
				item.QuotationText = varQuotationText;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CustomerIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn QuotationNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DateOfQuotationColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactPersonIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactSignationColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IrisProofColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn UnitIDColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyIDColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn QuotationTextColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CustomerID = @"CustomerID";
			 public static string QuotationNo = @"QuotationNo";
			 public static string DateOfQuotation = @"DateOfQuotation";
			 public static string ContactPersonID = @"ContactPersonID";
			 public static string ContactSignation = @"ContactSignation";
			 public static string IrisProof = @"IrisProof";
			 public static string UnitID = @"UnitID";
			 public static string CurrencyID = @"CurrencyID";
			 public static string QuotationText = @"QuotationText";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colTblCustomerQuotationPricingRecords != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblCustomerQuotationPricing item in colTblCustomerQuotationPricingRecords)
                    {
                        if (item.CustomerID != CustomerID)
                        {
                            item.CustomerID = CustomerID;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colTblCustomerQuotationPricingRecords != null)
                {
                    colTblCustomerQuotationPricingRecords.SaveAll();
               }
		}
        #endregion
	}
}