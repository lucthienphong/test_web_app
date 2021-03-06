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
	/// Strongly-typed collection for the TblCreditDetail class.
	/// </summary>
    [Serializable]
	public partial class TblCreditDetailCollection : ActiveList<TblCreditDetail, TblCreditDetailCollection>
	{	   
		public TblCreditDetailCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblCreditDetailCollection</returns>
		public TblCreditDetailCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblCreditDetail o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblCreditDetail table.
	/// </summary>
	[Serializable]
	public partial class TblCreditDetail : ActiveRecord<TblCreditDetail>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblCreditDetail()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblCreditDetail(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblCreditDetail(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblCreditDetail(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblCreditDetail", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCreditDetailID = new TableSchema.TableColumn(schema);
				colvarCreditDetailID.ColumnName = "CreditDetailID";
				colvarCreditDetailID.DataType = DbType.Int32;
				colvarCreditDetailID.MaxLength = 0;
				colvarCreditDetailID.AutoIncrement = true;
				colvarCreditDetailID.IsNullable = false;
				colvarCreditDetailID.IsPrimaryKey = true;
				colvarCreditDetailID.IsForeignKey = false;
				colvarCreditDetailID.IsReadOnly = false;
				colvarCreditDetailID.DefaultSetting = @"";
				colvarCreditDetailID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreditDetailID);
				
				TableSchema.TableColumn colvarJobOrderNo = new TableSchema.TableColumn(schema);
				colvarJobOrderNo.ColumnName = "JobOrderNo";
				colvarJobOrderNo.DataType = DbType.String;
				colvarJobOrderNo.MaxLength = 50;
				colvarJobOrderNo.AutoIncrement = false;
				colvarJobOrderNo.IsNullable = false;
				colvarJobOrderNo.IsPrimaryKey = false;
				colvarJobOrderNo.IsForeignKey = false;
				colvarJobOrderNo.IsReadOnly = false;
				colvarJobOrderNo.DefaultSetting = @"";
				colvarJobOrderNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarJobOrderNo);
				
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
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "Quantity";
				colvarQuantity.DataType = DbType.Int32;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = false;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantity);
				
				TableSchema.TableColumn colvarUnitPrice = new TableSchema.TableColumn(schema);
				colvarUnitPrice.ColumnName = "UnitPrice";
				colvarUnitPrice.DataType = DbType.Decimal;
				colvarUnitPrice.MaxLength = 0;
				colvarUnitPrice.AutoIncrement = false;
				colvarUnitPrice.IsNullable = false;
				colvarUnitPrice.IsPrimaryKey = false;
				colvarUnitPrice.IsForeignKey = false;
				colvarUnitPrice.IsReadOnly = false;
				colvarUnitPrice.DefaultSetting = @"";
				colvarUnitPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUnitPrice);
				
				TableSchema.TableColumn colvarCreditID = new TableSchema.TableColumn(schema);
				colvarCreditID.ColumnName = "CreditID";
				colvarCreditID.DataType = DbType.Int32;
				colvarCreditID.MaxLength = 0;
				colvarCreditID.AutoIncrement = false;
				colvarCreditID.IsNullable = false;
				colvarCreditID.IsPrimaryKey = false;
				colvarCreditID.IsForeignKey = false;
				colvarCreditID.IsReadOnly = false;
				colvarCreditID.DefaultSetting = @"";
				colvarCreditID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreditID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblCreditDetail",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CreditDetailID")]
		[Bindable(true)]
		public int CreditDetailID 
		{
			get { return GetColumnValue<int>(Columns.CreditDetailID); }
			set { SetColumnValue(Columns.CreditDetailID, value); }
		}
		  
		[XmlAttribute("JobOrderNo")]
		[Bindable(true)]
		public string JobOrderNo 
		{
			get { return GetColumnValue<string>(Columns.JobOrderNo); }
			set { SetColumnValue(Columns.JobOrderNo, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public int Quantity 
		{
			get { return GetColumnValue<int>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("UnitPrice")]
		[Bindable(true)]
		public decimal UnitPrice 
		{
			get { return GetColumnValue<decimal>(Columns.UnitPrice); }
			set { SetColumnValue(Columns.UnitPrice, value); }
		}
		  
		[XmlAttribute("CreditID")]
		[Bindable(true)]
		public int CreditID 
		{
			get { return GetColumnValue<int>(Columns.CreditID); }
			set { SetColumnValue(Columns.CreditID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varJobOrderNo,string varDescription,int varQuantity,decimal varUnitPrice,int varCreditID)
		{
			TblCreditDetail item = new TblCreditDetail();
			
			item.JobOrderNo = varJobOrderNo;
			
			item.Description = varDescription;
			
			item.Quantity = varQuantity;
			
			item.UnitPrice = varUnitPrice;
			
			item.CreditID = varCreditID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCreditDetailID,string varJobOrderNo,string varDescription,int varQuantity,decimal varUnitPrice,int varCreditID)
		{
			TblCreditDetail item = new TblCreditDetail();
			
				item.CreditDetailID = varCreditDetailID;
			
				item.JobOrderNo = varJobOrderNo;
			
				item.Description = varDescription;
			
				item.Quantity = varQuantity;
			
				item.UnitPrice = varUnitPrice;
			
				item.CreditID = varCreditID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CreditDetailIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn JobOrderNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UnitPriceColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreditIDColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CreditDetailID = @"CreditDetailID";
			 public static string JobOrderNo = @"JobOrderNo";
			 public static string Description = @"Description";
			 public static string Quantity = @"Quantity";
			 public static string UnitPrice = @"UnitPrice";
			 public static string CreditID = @"CreditID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
