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
	/// Strongly-typed collection for the TblOtherCharge class.
	/// </summary>
    [Serializable]
	public partial class TblOtherChargeCollection : ActiveList<TblOtherCharge, TblOtherChargeCollection>
	{	   
		public TblOtherChargeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblOtherChargeCollection</returns>
		public TblOtherChargeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblOtherCharge o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblOtherCharges table.
	/// </summary>
	[Serializable]
	public partial class TblOtherCharge : ActiveRecord<TblOtherCharge>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblOtherCharge()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblOtherCharge(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblOtherCharge(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblOtherCharge(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblOtherCharges", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarOtherChargesID = new TableSchema.TableColumn(schema);
				colvarOtherChargesID.ColumnName = "OtherChargesID";
				colvarOtherChargesID.DataType = DbType.Int32;
				colvarOtherChargesID.MaxLength = 0;
				colvarOtherChargesID.AutoIncrement = true;
				colvarOtherChargesID.IsNullable = false;
				colvarOtherChargesID.IsPrimaryKey = true;
				colvarOtherChargesID.IsForeignKey = false;
				colvarOtherChargesID.IsReadOnly = false;
				colvarOtherChargesID.DefaultSetting = @"";
				colvarOtherChargesID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOtherChargesID);
				
				TableSchema.TableColumn colvarGLCode = new TableSchema.TableColumn(schema);
				colvarGLCode.ColumnName = "GLCode";
				colvarGLCode.DataType = DbType.String;
				colvarGLCode.MaxLength = 10;
				colvarGLCode.AutoIncrement = false;
				colvarGLCode.IsNullable = true;
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
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarCharge = new TableSchema.TableColumn(schema);
				colvarCharge.ColumnName = "Charge";
				colvarCharge.DataType = DbType.Decimal;
				colvarCharge.MaxLength = 0;
				colvarCharge.AutoIncrement = false;
				colvarCharge.IsNullable = true;
				colvarCharge.IsPrimaryKey = false;
				colvarCharge.IsForeignKey = false;
				colvarCharge.IsReadOnly = false;
				colvarCharge.DefaultSetting = @"";
				colvarCharge.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCharge);
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "Quantity";
				colvarQuantity.DataType = DbType.Int32;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = true;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantity);
				
				TableSchema.TableColumn colvarJobID = new TableSchema.TableColumn(schema);
				colvarJobID.ColumnName = "JobID";
				colvarJobID.DataType = DbType.Int32;
				colvarJobID.MaxLength = 0;
				colvarJobID.AutoIncrement = false;
				colvarJobID.IsNullable = true;
				colvarJobID.IsPrimaryKey = false;
				colvarJobID.IsForeignKey = false;
				colvarJobID.IsReadOnly = false;
				colvarJobID.DefaultSetting = @"";
				colvarJobID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarJobID);
				
				TableSchema.TableColumn colvarPricingID = new TableSchema.TableColumn(schema);
				colvarPricingID.ColumnName = "PricingID";
				colvarPricingID.DataType = DbType.Int32;
				colvarPricingID.MaxLength = 0;
				colvarPricingID.AutoIncrement = false;
				colvarPricingID.IsNullable = true;
				colvarPricingID.IsPrimaryKey = false;
				colvarPricingID.IsForeignKey = false;
				colvarPricingID.IsReadOnly = false;
				colvarPricingID.DefaultSetting = @"";
				colvarPricingID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPricingID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblOtherCharges",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("OtherChargesID")]
		[Bindable(true)]
		public int OtherChargesID 
		{
			get { return GetColumnValue<int>(Columns.OtherChargesID); }
			set { SetColumnValue(Columns.OtherChargesID, value); }
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
		  
		[XmlAttribute("Charge")]
		[Bindable(true)]
		public decimal? Charge 
		{
			get { return GetColumnValue<decimal?>(Columns.Charge); }
			set { SetColumnValue(Columns.Charge, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public int? Quantity 
		{
			get { return GetColumnValue<int?>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("JobID")]
		[Bindable(true)]
		public int? JobID 
		{
			get { return GetColumnValue<int?>(Columns.JobID); }
			set { SetColumnValue(Columns.JobID, value); }
		}
		  
		[XmlAttribute("PricingID")]
		[Bindable(true)]
		public int? PricingID 
		{
			get { return GetColumnValue<int?>(Columns.PricingID); }
			set { SetColumnValue(Columns.PricingID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varGLCode,string varDescription,decimal? varCharge,int? varQuantity,int? varJobID,int? varPricingID)
		{
			TblOtherCharge item = new TblOtherCharge();
			
			item.GLCode = varGLCode;
			
			item.Description = varDescription;
			
			item.Charge = varCharge;
			
			item.Quantity = varQuantity;
			
			item.JobID = varJobID;
			
			item.PricingID = varPricingID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varOtherChargesID,string varGLCode,string varDescription,decimal? varCharge,int? varQuantity,int? varJobID,int? varPricingID)
		{
			TblOtherCharge item = new TblOtherCharge();
			
				item.OtherChargesID = varOtherChargesID;
			
				item.GLCode = varGLCode;
			
				item.Description = varDescription;
			
				item.Charge = varCharge;
			
				item.Quantity = varQuantity;
			
				item.JobID = varJobID;
			
				item.PricingID = varPricingID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn OtherChargesIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn GLCodeColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ChargeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn JobIDColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn PricingIDColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string OtherChargesID = @"OtherChargesID";
			 public static string GLCode = @"GLCode";
			 public static string Description = @"Description";
			 public static string Charge = @"Charge";
			 public static string Quantity = @"Quantity";
			 public static string JobID = @"JobID";
			 public static string PricingID = @"PricingID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
