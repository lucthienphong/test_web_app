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
namespace SweetSoft.APEM.DataAccessLog
{
	/// <summary>
	/// Strongly-typed collection for the TblAllDataLog class.
	/// </summary>
    [Serializable]
	public partial class TblAllDataLogCollection : ActiveList<TblAllDataLog, TblAllDataLogCollection>
	{	   
		public TblAllDataLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblAllDataLogCollection</returns>
		public TblAllDataLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblAllDataLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblAllDataLogs table.
	/// </summary>
	[Serializable]
	public partial class TblAllDataLog : ActiveRecord<TblAllDataLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblAllDataLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblAllDataLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblAllDataLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblAllDataLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblAllDataLogs", TableType.Table, DataService.GetInstance("DataAcessProviderLog"));
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
				
				TableSchema.TableColumn colvarContentLogs = new TableSchema.TableColumn(schema);
				colvarContentLogs.ColumnName = "ContentLogs";
				colvarContentLogs.DataType = DbType.String;
				colvarContentLogs.MaxLength = 1073741823;
				colvarContentLogs.AutoIncrement = false;
				colvarContentLogs.IsNullable = true;
				colvarContentLogs.IsPrimaryKey = false;
				colvarContentLogs.IsForeignKey = false;
				colvarContentLogs.IsReadOnly = false;
				colvarContentLogs.DefaultSetting = @"";
				colvarContentLogs.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContentLogs);
				
				TableSchema.TableColumn colvarActionDate = new TableSchema.TableColumn(schema);
				colvarActionDate.ColumnName = "ActionDate";
				colvarActionDate.DataType = DbType.DateTime;
				colvarActionDate.MaxLength = 0;
				colvarActionDate.AutoIncrement = false;
				colvarActionDate.IsNullable = true;
				colvarActionDate.IsPrimaryKey = false;
				colvarActionDate.IsForeignKey = false;
				colvarActionDate.IsReadOnly = false;
				colvarActionDate.DefaultSetting = @"";
				colvarActionDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarActionDate);
				
				TableSchema.TableColumn colvarUserIP = new TableSchema.TableColumn(schema);
				colvarUserIP.ColumnName = "UserIP";
				colvarUserIP.DataType = DbType.String;
				colvarUserIP.MaxLength = 50;
				colvarUserIP.AutoIncrement = false;
				colvarUserIP.IsNullable = true;
				colvarUserIP.IsPrimaryKey = false;
				colvarUserIP.IsForeignKey = false;
				colvarUserIP.IsReadOnly = false;
				colvarUserIP.DefaultSetting = @"";
				colvarUserIP.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserIP);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProviderLog"].AddSchema("tblAllDataLogs",schema);
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
		  
		[XmlAttribute("ContentLogs")]
		[Bindable(true)]
		public string ContentLogs 
		{
			get { return GetColumnValue<string>(Columns.ContentLogs); }
			set { SetColumnValue(Columns.ContentLogs, value); }
		}
		  
		[XmlAttribute("ActionDate")]
		[Bindable(true)]
		public DateTime? ActionDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ActionDate); }
			set { SetColumnValue(Columns.ActionDate, value); }
		}
		  
		[XmlAttribute("UserIP")]
		[Bindable(true)]
		public string UserIP 
		{
			get { return GetColumnValue<string>(Columns.UserIP); }
			set { SetColumnValue(Columns.UserIP, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varContentLogs,DateTime? varActionDate,string varUserIP)
		{
			TblAllDataLog item = new TblAllDataLog();
			
			item.ContentLogs = varContentLogs;
			
			item.ActionDate = varActionDate;
			
			item.UserIP = varUserIP;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varContentLogs,DateTime? varActionDate,string varUserIP)
		{
			TblAllDataLog item = new TblAllDataLog();
			
				item.Id = varId;
			
				item.ContentLogs = varContentLogs;
			
				item.ActionDate = varActionDate;
			
				item.UserIP = varUserIP;
			
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
        
        
        
        public static TableSchema.TableColumn ContentLogsColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ActionDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIPColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string ContentLogs = @"ContentLogs";
			 public static string ActionDate = @"ActionDate";
			 public static string UserIP = @"UserIP";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
