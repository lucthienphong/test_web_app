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
	/// Strongly-typed collection for the TblCodeDictionary class.
	/// </summary>
    [Serializable]
	public partial class TblCodeDictionaryCollection : ActiveList<TblCodeDictionary, TblCodeDictionaryCollection>
	{	   
		public TblCodeDictionaryCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblCodeDictionaryCollection</returns>
		public TblCodeDictionaryCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblCodeDictionary o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblCodeDictionary table.
	/// </summary>
	[Serializable]
	public partial class TblCodeDictionary : ActiveRecord<TblCodeDictionary>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblCodeDictionary()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblCodeDictionary(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblCodeDictionary(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblCodeDictionary(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblCodeDictionary", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCodeID = new TableSchema.TableColumn(schema);
				colvarCodeID.ColumnName = "CodeID";
				colvarCodeID.DataType = DbType.Int32;
				colvarCodeID.MaxLength = 0;
				colvarCodeID.AutoIncrement = true;
				colvarCodeID.IsNullable = false;
				colvarCodeID.IsPrimaryKey = true;
				colvarCodeID.IsForeignKey = false;
				colvarCodeID.IsReadOnly = false;
				colvarCodeID.DefaultSetting = @"";
				colvarCodeID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCodeID);
				
				TableSchema.TableColumn colvarCode = new TableSchema.TableColumn(schema);
				colvarCode.ColumnName = "Code";
				colvarCode.DataType = DbType.String;
				colvarCode.MaxLength = 10;
				colvarCode.AutoIncrement = false;
				colvarCode.IsNullable = false;
				colvarCode.IsPrimaryKey = false;
				colvarCode.IsForeignKey = false;
				colvarCode.IsReadOnly = false;
				colvarCode.DefaultSetting = @"";
				colvarCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCode);
				
				TableSchema.TableColumn colvarIsUsed = new TableSchema.TableColumn(schema);
				colvarIsUsed.ColumnName = "IsUsed";
				colvarIsUsed.DataType = DbType.Boolean;
				colvarIsUsed.MaxLength = 0;
				colvarIsUsed.AutoIncrement = false;
				colvarIsUsed.IsNullable = false;
				colvarIsUsed.IsPrimaryKey = false;
				colvarIsUsed.IsForeignKey = false;
				colvarIsUsed.IsReadOnly = false;
				colvarIsUsed.DefaultSetting = @"";
				colvarIsUsed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsUsed);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblCodeDictionary",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CodeID")]
		[Bindable(true)]
		public int CodeID 
		{
			get { return GetColumnValue<int>(Columns.CodeID); }
			set { SetColumnValue(Columns.CodeID, value); }
		}
		  
		[XmlAttribute("Code")]
		[Bindable(true)]
		public string Code 
		{
			get { return GetColumnValue<string>(Columns.Code); }
			set { SetColumnValue(Columns.Code, value); }
		}
		  
		[XmlAttribute("IsUsed")]
		[Bindable(true)]
		public bool IsUsed 
		{
			get { return GetColumnValue<bool>(Columns.IsUsed); }
			set { SetColumnValue(Columns.IsUsed, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varCode,bool varIsUsed)
		{
			TblCodeDictionary item = new TblCodeDictionary();
			
			item.Code = varCode;
			
			item.IsUsed = varIsUsed;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCodeID,string varCode,bool varIsUsed)
		{
			TblCodeDictionary item = new TblCodeDictionary();
			
				item.CodeID = varCodeID;
			
				item.Code = varCode;
			
				item.IsUsed = varIsUsed;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CodeIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CodeColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IsUsedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CodeID = @"CodeID";
			 public static string Code = @"Code";
			 public static string IsUsed = @"IsUsed";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}