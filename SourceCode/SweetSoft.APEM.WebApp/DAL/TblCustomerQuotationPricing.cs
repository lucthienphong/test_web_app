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
	/// Strongly-typed collection for the TblCustomerQuotationPricing class.
	/// </summary>
    [Serializable]
	public partial class TblCustomerQuotationPricingCollection : ActiveList<TblCustomerQuotationPricing, TblCustomerQuotationPricingCollection>
	{	   
		public TblCustomerQuotationPricingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblCustomerQuotationPricingCollection</returns>
		public TblCustomerQuotationPricingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblCustomerQuotationPricing o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblCustomerQuotation_Pricing table.
	/// </summary>
	[Serializable]
	public partial class TblCustomerQuotationPricing : ActiveRecord<TblCustomerQuotationPricing>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblCustomerQuotationPricing()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblCustomerQuotationPricing(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblCustomerQuotationPricing(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblCustomerQuotationPricing(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblCustomerQuotation_Pricing", TableType.Table, DataService.GetInstance("DataAcessProvider"));
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
				
					colvarCustomerID.ForeignKeyTableName = "tblCustomerQuotation";
				schema.Columns.Add(colvarCustomerID);
				
				TableSchema.TableColumn colvarPricingID = new TableSchema.TableColumn(schema);
				colvarPricingID.ColumnName = "PricingID";
				colvarPricingID.DataType = DbType.Int16;
				colvarPricingID.MaxLength = 0;
				colvarPricingID.AutoIncrement = false;
				colvarPricingID.IsNullable = false;
				colvarPricingID.IsPrimaryKey = true;
				colvarPricingID.IsForeignKey = true;
				colvarPricingID.IsReadOnly = false;
				colvarPricingID.DefaultSetting = @"";
				
					colvarPricingID.ForeignKeyTableName = "tblPricing";
				schema.Columns.Add(colvarPricingID);
				
				TableSchema.TableColumn colvarOldSteelBasePrice = new TableSchema.TableColumn(schema);
				colvarOldSteelBasePrice.ColumnName = "OldSteelBasePrice";
				colvarOldSteelBasePrice.DataType = DbType.Decimal;
				colvarOldSteelBasePrice.MaxLength = 0;
				colvarOldSteelBasePrice.AutoIncrement = false;
				colvarOldSteelBasePrice.IsNullable = false;
				colvarOldSteelBasePrice.IsPrimaryKey = false;
				colvarOldSteelBasePrice.IsForeignKey = false;
				colvarOldSteelBasePrice.IsReadOnly = false;
				colvarOldSteelBasePrice.DefaultSetting = @"";
				colvarOldSteelBasePrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOldSteelBasePrice);
				
				TableSchema.TableColumn colvarNewSteelBasePrice = new TableSchema.TableColumn(schema);
				colvarNewSteelBasePrice.ColumnName = "NewSteelBasePrice";
				colvarNewSteelBasePrice.DataType = DbType.Decimal;
				colvarNewSteelBasePrice.MaxLength = 0;
				colvarNewSteelBasePrice.AutoIncrement = false;
				colvarNewSteelBasePrice.IsNullable = false;
				colvarNewSteelBasePrice.IsPrimaryKey = false;
				colvarNewSteelBasePrice.IsForeignKey = false;
				colvarNewSteelBasePrice.IsReadOnly = false;
				colvarNewSteelBasePrice.DefaultSetting = @"";
				colvarNewSteelBasePrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNewSteelBasePrice);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblCustomerQuotation_Pricing",schema);
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
		  
		[XmlAttribute("PricingID")]
		[Bindable(true)]
		public short PricingID 
		{
			get { return GetColumnValue<short>(Columns.PricingID); }
			set { SetColumnValue(Columns.PricingID, value); }
		}
		  
		[XmlAttribute("OldSteelBasePrice")]
		[Bindable(true)]
		public decimal OldSteelBasePrice 
		{
			get { return GetColumnValue<decimal>(Columns.OldSteelBasePrice); }
			set { SetColumnValue(Columns.OldSteelBasePrice, value); }
		}
		  
		[XmlAttribute("NewSteelBasePrice")]
		[Bindable(true)]
		public decimal NewSteelBasePrice 
		{
			get { return GetColumnValue<decimal>(Columns.NewSteelBasePrice); }
			set { SetColumnValue(Columns.NewSteelBasePrice, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a TblCustomerQuotation ActiveRecord object related to this TblCustomerQuotationPricing
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblCustomerQuotation TblCustomerQuotation
		{
			get { return SweetSoft.APEM.DataAccess.TblCustomerQuotation.FetchByID(this.CustomerID); }
			set { SetColumnValue("CustomerID", value.CustomerID); }
		}
		
		
		/// <summary>
		/// Returns a TblPricing ActiveRecord object related to this TblCustomerQuotationPricing
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblPricing TblPricing
		{
			get { return SweetSoft.APEM.DataAccess.TblPricing.FetchByID(this.PricingID); }
			set { SetColumnValue("PricingID", value.PricingID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varCustomerID,short varPricingID,decimal varOldSteelBasePrice,decimal varNewSteelBasePrice)
		{
			TblCustomerQuotationPricing item = new TblCustomerQuotationPricing();
			
			item.CustomerID = varCustomerID;
			
			item.PricingID = varPricingID;
			
			item.OldSteelBasePrice = varOldSteelBasePrice;
			
			item.NewSteelBasePrice = varNewSteelBasePrice;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCustomerID,short varPricingID,decimal varOldSteelBasePrice,decimal varNewSteelBasePrice)
		{
			TblCustomerQuotationPricing item = new TblCustomerQuotationPricing();
			
				item.CustomerID = varCustomerID;
			
				item.PricingID = varPricingID;
			
				item.OldSteelBasePrice = varOldSteelBasePrice;
			
				item.NewSteelBasePrice = varNewSteelBasePrice;
			
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
        
        
        
        public static TableSchema.TableColumn PricingIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn OldSteelBasePriceColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn NewSteelBasePriceColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CustomerID = @"CustomerID";
			 public static string PricingID = @"PricingID";
			 public static string OldSteelBasePrice = @"OldSteelBasePrice";
			 public static string NewSteelBasePrice = @"NewSteelBasePrice";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}