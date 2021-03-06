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
	/// Strongly-typed collection for the TblSystemSetting class.
	/// </summary>
    [Serializable]
	public partial class TblSystemSettingCollection : ActiveList<TblSystemSetting, TblSystemSettingCollection>
	{	   
		public TblSystemSettingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblSystemSettingCollection</returns>
		public TblSystemSettingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblSystemSetting o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblSystemSetting table.
	/// </summary>
	[Serializable]
	public partial class TblSystemSetting : ActiveRecord<TblSystemSetting>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblSystemSetting()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblSystemSetting(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblSystemSetting(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblSystemSetting(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblSystemSetting", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSettingID = new TableSchema.TableColumn(schema);
				colvarSettingID.ColumnName = "SettingID";
				colvarSettingID.DataType = DbType.Int32;
				colvarSettingID.MaxLength = 0;
				colvarSettingID.AutoIncrement = true;
				colvarSettingID.IsNullable = false;
				colvarSettingID.IsPrimaryKey = true;
				colvarSettingID.IsForeignKey = false;
				colvarSettingID.IsReadOnly = false;
				colvarSettingID.DefaultSetting = @"";
				colvarSettingID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSettingID);
				
				TableSchema.TableColumn colvarSettingName = new TableSchema.TableColumn(schema);
				colvarSettingName.ColumnName = "SettingName";
				colvarSettingName.DataType = DbType.AnsiString;
				colvarSettingName.MaxLength = 255;
				colvarSettingName.AutoIncrement = false;
				colvarSettingName.IsNullable = false;
				colvarSettingName.IsPrimaryKey = false;
				colvarSettingName.IsForeignKey = false;
				colvarSettingName.IsReadOnly = false;
				colvarSettingName.DefaultSetting = @"";
				colvarSettingName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSettingName);
				
				TableSchema.TableColumn colvarSettingType = new TableSchema.TableColumn(schema);
				colvarSettingType.ColumnName = "SettingType";
				colvarSettingType.DataType = DbType.AnsiString;
				colvarSettingType.MaxLength = 50;
				colvarSettingType.AutoIncrement = false;
				colvarSettingType.IsNullable = false;
				colvarSettingType.IsPrimaryKey = false;
				colvarSettingType.IsForeignKey = false;
				colvarSettingType.IsReadOnly = false;
				colvarSettingType.DefaultSetting = @"";
				colvarSettingType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSettingType);
				
				TableSchema.TableColumn colvarSettingValue = new TableSchema.TableColumn(schema);
				colvarSettingValue.ColumnName = "SettingValue";
				colvarSettingValue.DataType = DbType.String;
				colvarSettingValue.MaxLength = 1073741823;
				colvarSettingValue.AutoIncrement = false;
				colvarSettingValue.IsNullable = false;
				colvarSettingValue.IsPrimaryKey = false;
				colvarSettingValue.IsForeignKey = false;
				colvarSettingValue.IsReadOnly = false;
				colvarSettingValue.DefaultSetting = @"";
				colvarSettingValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSettingValue);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblSystemSetting",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SettingID")]
		[Bindable(true)]
		public int SettingID 
		{
			get { return GetColumnValue<int>(Columns.SettingID); }
			set { SetColumnValue(Columns.SettingID, value); }
		}
		  
		[XmlAttribute("SettingName")]
		[Bindable(true)]
		public string SettingName 
		{
			get { return GetColumnValue<string>(Columns.SettingName); }
			set { SetColumnValue(Columns.SettingName, value); }
		}
		  
		[XmlAttribute("SettingType")]
		[Bindable(true)]
		public string SettingType 
		{
			get { return GetColumnValue<string>(Columns.SettingType); }
			set { SetColumnValue(Columns.SettingType, value); }
		}
		  
		[XmlAttribute("SettingValue")]
		[Bindable(true)]
		public string SettingValue 
		{
			get { return GetColumnValue<string>(Columns.SettingValue); }
			set { SetColumnValue(Columns.SettingValue, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varSettingName,string varSettingType,string varSettingValue)
		{
			TblSystemSetting item = new TblSystemSetting();
			
			item.SettingName = varSettingName;
			
			item.SettingType = varSettingType;
			
			item.SettingValue = varSettingValue;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varSettingID,string varSettingName,string varSettingType,string varSettingValue)
		{
			TblSystemSetting item = new TblSystemSetting();
			
				item.SettingID = varSettingID;
			
				item.SettingName = varSettingName;
			
				item.SettingType = varSettingType;
			
				item.SettingValue = varSettingValue;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SettingIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SettingNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SettingTypeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn SettingValueColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SettingID = @"SettingID";
			 public static string SettingName = @"SettingName";
			 public static string SettingType = @"SettingType";
			 public static string SettingValue = @"SettingValue";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
