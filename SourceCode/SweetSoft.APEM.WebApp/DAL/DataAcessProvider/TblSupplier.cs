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
	/// Strongly-typed collection for the TblSupplier class.
	/// </summary>
    [Serializable]
	public partial class TblSupplierCollection : ActiveList<TblSupplier, TblSupplierCollection>
	{	   
		public TblSupplierCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblSupplierCollection</returns>
		public TblSupplierCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblSupplier o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblSupplier table.
	/// </summary>
	[Serializable]
	public partial class TblSupplier : ActiveRecord<TblSupplier>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblSupplier()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblSupplier(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblSupplier(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblSupplier(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblSupplier", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSupplierID = new TableSchema.TableColumn(schema);
				colvarSupplierID.ColumnName = "SupplierID";
				colvarSupplierID.DataType = DbType.Int32;
				colvarSupplierID.MaxLength = 0;
				colvarSupplierID.AutoIncrement = true;
				colvarSupplierID.IsNullable = false;
				colvarSupplierID.IsPrimaryKey = true;
				colvarSupplierID.IsForeignKey = false;
				colvarSupplierID.IsReadOnly = false;
				colvarSupplierID.DefaultSetting = @"";
				colvarSupplierID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSupplierID);
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.String;
				colvarName.MaxLength = 200;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarAddress = new TableSchema.TableColumn(schema);
				colvarAddress.ColumnName = "Address";
				colvarAddress.DataType = DbType.String;
				colvarAddress.MaxLength = 200;
				colvarAddress.AutoIncrement = false;
				colvarAddress.IsNullable = false;
				colvarAddress.IsPrimaryKey = false;
				colvarAddress.IsForeignKey = false;
				colvarAddress.IsReadOnly = false;
				colvarAddress.DefaultSetting = @"";
				colvarAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddress);
				
				TableSchema.TableColumn colvarTel = new TableSchema.TableColumn(schema);
				colvarTel.ColumnName = "Tel";
				colvarTel.DataType = DbType.AnsiString;
				colvarTel.MaxLength = 20;
				colvarTel.AutoIncrement = false;
				colvarTel.IsNullable = false;
				colvarTel.IsPrimaryKey = false;
				colvarTel.IsForeignKey = false;
				colvarTel.IsReadOnly = false;
				colvarTel.DefaultSetting = @"";
				colvarTel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTel);
				
				TableSchema.TableColumn colvarFax = new TableSchema.TableColumn(schema);
				colvarFax.ColumnName = "Fax";
				colvarFax.DataType = DbType.AnsiString;
				colvarFax.MaxLength = 20;
				colvarFax.AutoIncrement = false;
				colvarFax.IsNullable = false;
				colvarFax.IsPrimaryKey = false;
				colvarFax.IsForeignKey = false;
				colvarFax.IsReadOnly = false;
				colvarFax.DefaultSetting = @"";
				colvarFax.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFax);
				
				TableSchema.TableColumn colvarIsObsolete = new TableSchema.TableColumn(schema);
				colvarIsObsolete.ColumnName = "IsObsolete";
				colvarIsObsolete.DataType = DbType.Boolean;
				colvarIsObsolete.MaxLength = 0;
				colvarIsObsolete.AutoIncrement = false;
				colvarIsObsolete.IsNullable = false;
				colvarIsObsolete.IsPrimaryKey = false;
				colvarIsObsolete.IsForeignKey = false;
				colvarIsObsolete.IsReadOnly = false;
				colvarIsObsolete.DefaultSetting = @"";
				colvarIsObsolete.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsObsolete);
				
				TableSchema.TableColumn colvarContactPerson = new TableSchema.TableColumn(schema);
				colvarContactPerson.ColumnName = "ContactPerson";
				colvarContactPerson.DataType = DbType.String;
				colvarContactPerson.MaxLength = 100;
				colvarContactPerson.AutoIncrement = false;
				colvarContactPerson.IsNullable = true;
				colvarContactPerson.IsPrimaryKey = false;
				colvarContactPerson.IsForeignKey = false;
				colvarContactPerson.IsReadOnly = false;
				colvarContactPerson.DefaultSetting = @"";
				colvarContactPerson.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactPerson);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblSupplier",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SupplierID")]
		[Bindable(true)]
		public int SupplierID 
		{
			get { return GetColumnValue<int>(Columns.SupplierID); }
			set { SetColumnValue(Columns.SupplierID, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Address")]
		[Bindable(true)]
		public string Address 
		{
			get { return GetColumnValue<string>(Columns.Address); }
			set { SetColumnValue(Columns.Address, value); }
		}
		  
		[XmlAttribute("Tel")]
		[Bindable(true)]
		public string Tel 
		{
			get { return GetColumnValue<string>(Columns.Tel); }
			set { SetColumnValue(Columns.Tel, value); }
		}
		  
		[XmlAttribute("Fax")]
		[Bindable(true)]
		public string Fax 
		{
			get { return GetColumnValue<string>(Columns.Fax); }
			set { SetColumnValue(Columns.Fax, value); }
		}
		  
		[XmlAttribute("IsObsolete")]
		[Bindable(true)]
		public bool IsObsolete 
		{
			get { return GetColumnValue<bool>(Columns.IsObsolete); }
			set { SetColumnValue(Columns.IsObsolete, value); }
		}
		  
		[XmlAttribute("ContactPerson")]
		[Bindable(true)]
		public string ContactPerson 
		{
			get { return GetColumnValue<string>(Columns.ContactPerson); }
			set { SetColumnValue(Columns.ContactPerson, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varName,string varAddress,string varTel,string varFax,bool varIsObsolete,string varContactPerson)
		{
			TblSupplier item = new TblSupplier();
			
			item.Name = varName;
			
			item.Address = varAddress;
			
			item.Tel = varTel;
			
			item.Fax = varFax;
			
			item.IsObsolete = varIsObsolete;
			
			item.ContactPerson = varContactPerson;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varSupplierID,string varName,string varAddress,string varTel,string varFax,bool varIsObsolete,string varContactPerson)
		{
			TblSupplier item = new TblSupplier();
			
				item.SupplierID = varSupplierID;
			
				item.Name = varName;
			
				item.Address = varAddress;
			
				item.Tel = varTel;
			
				item.Fax = varFax;
			
				item.IsObsolete = varIsObsolete;
			
				item.ContactPerson = varContactPerson;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SupplierIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn AddressColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TelColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn FaxColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IsObsoleteColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactPersonColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SupplierID = @"SupplierID";
			 public static string Name = @"Name";
			 public static string Address = @"Address";
			 public static string Tel = @"Tel";
			 public static string Fax = @"Fax";
			 public static string IsObsolete = @"IsObsolete";
			 public static string ContactPerson = @"ContactPerson";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
