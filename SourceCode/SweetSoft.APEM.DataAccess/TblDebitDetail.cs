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
	/// Strongly-typed collection for the TblDebitDetail class.
	/// </summary>
    [Serializable]
	public partial class TblDebitDetailCollection : ActiveList<TblDebitDetail, TblDebitDetailCollection>
	{	   
		public TblDebitDetailCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblDebitDetailCollection</returns>
		public TblDebitDetailCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblDebitDetail o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblDebitDetail table.
	/// </summary>
	[Serializable]
	public partial class TblDebitDetail : ActiveRecord<TblDebitDetail>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblDebitDetail()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblDebitDetail(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblDebitDetail(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblDebitDetail(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblDebitDetail", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarDebitDetailID = new TableSchema.TableColumn(schema);
				colvarDebitDetailID.ColumnName = "DebitDetailID";
				colvarDebitDetailID.DataType = DbType.Int32;
				colvarDebitDetailID.MaxLength = 0;
				colvarDebitDetailID.AutoIncrement = true;
				colvarDebitDetailID.IsNullable = false;
				colvarDebitDetailID.IsPrimaryKey = true;
				colvarDebitDetailID.IsForeignKey = false;
				colvarDebitDetailID.IsReadOnly = false;
				colvarDebitDetailID.DefaultSetting = @"";
				colvarDebitDetailID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDebitDetailID);
				
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
				
				TableSchema.TableColumn colvarDebitID = new TableSchema.TableColumn(schema);
				colvarDebitID.ColumnName = "DebitID";
				colvarDebitID.DataType = DbType.Int32;
				colvarDebitID.MaxLength = 0;
				colvarDebitID.AutoIncrement = false;
				colvarDebitID.IsNullable = false;
				colvarDebitID.IsPrimaryKey = false;
				colvarDebitID.IsForeignKey = false;
				colvarDebitID.IsReadOnly = false;
				colvarDebitID.DefaultSetting = @"";
				colvarDebitID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDebitID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblDebitDetail",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("DebitDetailID")]
		[Bindable(true)]
		public int DebitDetailID 
		{
			get { return GetColumnValue<int>(Columns.DebitDetailID); }
			set { SetColumnValue(Columns.DebitDetailID, value); }
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
		  
		[XmlAttribute("DebitID")]
		[Bindable(true)]
		public int DebitID 
		{
			get { return GetColumnValue<int>(Columns.DebitID); }
			set { SetColumnValue(Columns.DebitID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varJobOrderNo,string varDescription,int varQuantity,decimal varUnitPrice,int varDebitID)
		{
			TblDebitDetail item = new TblDebitDetail();
			
			item.JobOrderNo = varJobOrderNo;
			
			item.Description = varDescription;
			
			item.Quantity = varQuantity;
			
			item.UnitPrice = varUnitPrice;
			
			item.DebitID = varDebitID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varDebitDetailID,string varJobOrderNo,string varDescription,int varQuantity,decimal varUnitPrice,int varDebitID)
		{
			TblDebitDetail item = new TblDebitDetail();
			
				item.DebitDetailID = varDebitDetailID;
			
				item.JobOrderNo = varJobOrderNo;
			
				item.Description = varDescription;
			
				item.Quantity = varQuantity;
			
				item.UnitPrice = varUnitPrice;
			
				item.DebitID = varDebitID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn DebitDetailIDColumn
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
        
        
        
        public static TableSchema.TableColumn DebitIDColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string DebitDetailID = @"DebitDetailID";
			 public static string JobOrderNo = @"JobOrderNo";
			 public static string Description = @"Description";
			 public static string Quantity = @"Quantity";
			 public static string UnitPrice = @"UnitPrice";
			 public static string DebitID = @"DebitID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
