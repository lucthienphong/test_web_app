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
	/// Strongly-typed collection for the TblCustomerQuotationAdditionalService class.
	/// </summary>
    [Serializable]
	public partial class TblCustomerQuotationAdditionalServiceCollection : ActiveList<TblCustomerQuotationAdditionalService, TblCustomerQuotationAdditionalServiceCollection>
	{	   
		public TblCustomerQuotationAdditionalServiceCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblCustomerQuotationAdditionalServiceCollection</returns>
		public TblCustomerQuotationAdditionalServiceCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblCustomerQuotationAdditionalService o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblCustomerQuotation_AdditionalService table.
	/// </summary>
	[Serializable]
	public partial class TblCustomerQuotationAdditionalService : ActiveRecord<TblCustomerQuotationAdditionalService>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblCustomerQuotationAdditionalService()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblCustomerQuotationAdditionalService(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblCustomerQuotationAdditionalService(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblCustomerQuotationAdditionalService(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblCustomerQuotation_AdditionalService", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "ID";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = true;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarCategory = new TableSchema.TableColumn(schema);
				colvarCategory.ColumnName = "Category";
				colvarCategory.DataType = DbType.String;
				colvarCategory.MaxLength = 50;
				colvarCategory.AutoIncrement = false;
				colvarCategory.IsNullable = true;
				colvarCategory.IsPrimaryKey = false;
				colvarCategory.IsForeignKey = false;
				colvarCategory.IsReadOnly = false;
				colvarCategory.DefaultSetting = @"";
				colvarCategory.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCategory);
				
				TableSchema.TableColumn colvarGLCode = new TableSchema.TableColumn(schema);
				colvarGLCode.ColumnName = "GLCode";
				colvarGLCode.DataType = DbType.String;
				colvarGLCode.MaxLength = 10;
				colvarGLCode.AutoIncrement = false;
				colvarGLCode.IsNullable = false;
				colvarGLCode.IsPrimaryKey = false;
				colvarGLCode.IsForeignKey = false;
				colvarGLCode.IsReadOnly = false;
				colvarGLCode.DefaultSetting = @"";
				colvarGLCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGLCode);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.String;
				colvarDescription.MaxLength = 200;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = false;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarPrice = new TableSchema.TableColumn(schema);
				colvarPrice.ColumnName = "Price";
				colvarPrice.DataType = DbType.Decimal;
				colvarPrice.MaxLength = 0;
				colvarPrice.AutoIncrement = false;
				colvarPrice.IsNullable = false;
				colvarPrice.IsPrimaryKey = false;
				colvarPrice.IsForeignKey = false;
				colvarPrice.IsReadOnly = false;
				colvarPrice.DefaultSetting = @"";
				colvarPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrice);
				
				TableSchema.TableColumn colvarCurrencyID = new TableSchema.TableColumn(schema);
				colvarCurrencyID.ColumnName = "CurrencyID";
				colvarCurrencyID.DataType = DbType.Int16;
				colvarCurrencyID.MaxLength = 0;
				colvarCurrencyID.AutoIncrement = false;
				colvarCurrencyID.IsNullable = false;
				colvarCurrencyID.IsPrimaryKey = false;
				colvarCurrencyID.IsForeignKey = false;
				colvarCurrencyID.IsReadOnly = false;
				colvarCurrencyID.DefaultSetting = @"";
				colvarCurrencyID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrencyID);
				
				TableSchema.TableColumn colvarCustomerID = new TableSchema.TableColumn(schema);
				colvarCustomerID.ColumnName = "CustomerID";
				colvarCustomerID.DataType = DbType.Int32;
				colvarCustomerID.MaxLength = 0;
				colvarCustomerID.AutoIncrement = false;
				colvarCustomerID.IsNullable = false;
				colvarCustomerID.IsPrimaryKey = false;
				colvarCustomerID.IsForeignKey = false;
				colvarCustomerID.IsReadOnly = false;
				colvarCustomerID.DefaultSetting = @"";
				colvarCustomerID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblCustomerQuotation_AdditionalService",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public int Id 
		{
			get { return GetColumnValue<int>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("Category")]
		[Bindable(true)]
		public string Category 
		{
			get { return GetColumnValue<string>(Columns.Category); }
			set { SetColumnValue(Columns.Category, value); }
		}
		  
		[XmlAttribute("GLCode")]
		[Bindable(true)]
		public string GLCode 
		{
			get { return GetColumnValue<string>(Columns.GLCode); }
			set { SetColumnValue(Columns.GLCode, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("Price")]
		[Bindable(true)]
		public decimal Price 
		{
			get { return GetColumnValue<decimal>(Columns.Price); }
			set { SetColumnValue(Columns.Price, value); }
		}
		  
		[XmlAttribute("CurrencyID")]
		[Bindable(true)]
		public short CurrencyID 
		{
			get { return GetColumnValue<short>(Columns.CurrencyID); }
			set { SetColumnValue(Columns.CurrencyID, value); }
		}
		  
		[XmlAttribute("CustomerID")]
		[Bindable(true)]
		public int CustomerID 
		{
			get { return GetColumnValue<int>(Columns.CustomerID); }
			set { SetColumnValue(Columns.CustomerID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varCategory,string varGLCode,string varDescription,decimal varPrice,short varCurrencyID,int varCustomerID)
		{
			TblCustomerQuotationAdditionalService item = new TblCustomerQuotationAdditionalService();
			
			item.Category = varCategory;
			
			item.GLCode = varGLCode;
			
			item.Description = varDescription;
			
			item.Price = varPrice;
			
			item.CurrencyID = varCurrencyID;
			
			item.CustomerID = varCustomerID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varCategory,string varGLCode,string varDescription,decimal varPrice,short varCurrencyID,int varCustomerID)
		{
			TblCustomerQuotationAdditionalService item = new TblCustomerQuotationAdditionalService();
			
				item.Id = varId;
			
				item.Category = varCategory;
			
				item.GLCode = varGLCode;
			
				item.Description = varDescription;
			
				item.Price = varPrice;
			
				item.CurrencyID = varCurrencyID;
			
				item.CustomerID = varCustomerID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CategoryColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn GLCodeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PriceColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyIDColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIDColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Category = @"Category";
			 public static string GLCode = @"GLCode";
			 public static string Description = @"Description";
			 public static string Price = @"Price";
			 public static string CurrencyID = @"CurrencyID";
			 public static string CustomerID = @"CustomerID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
