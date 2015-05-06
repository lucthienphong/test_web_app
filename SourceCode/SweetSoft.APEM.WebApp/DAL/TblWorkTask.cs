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
	/// Strongly-typed collection for the TblWorkTask class.
	/// </summary>
    [Serializable]
	public partial class TblWorkTaskCollection : ActiveList<TblWorkTask, TblWorkTaskCollection>
	{	   
		public TblWorkTaskCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblWorkTaskCollection</returns>
		public TblWorkTaskCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblWorkTask o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblWorkTask table.
	/// </summary>
	[Serializable]
	public partial class TblWorkTask : ActiveRecord<TblWorkTask>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblWorkTask()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblWorkTask(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblWorkTask(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblWorkTask(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblWorkTask", TableType.Table, DataService.GetInstance("DataAcessProvider"));
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
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.String;
				colvarName.MaxLength = 100;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = true;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarDepartmentID = new TableSchema.TableColumn(schema);
				colvarDepartmentID.ColumnName = "DepartmentID";
				colvarDepartmentID.DataType = DbType.Int16;
				colvarDepartmentID.MaxLength = 0;
				colvarDepartmentID.AutoIncrement = false;
				colvarDepartmentID.IsNullable = true;
				colvarDepartmentID.IsPrimaryKey = false;
				colvarDepartmentID.IsForeignKey = true;
				colvarDepartmentID.IsReadOnly = false;
				colvarDepartmentID.DefaultSetting = @"";
				
					colvarDepartmentID.ForeignKeyTableName = "tblDepartment";
				schema.Columns.Add(colvarDepartmentID);
				
				TableSchema.TableColumn colvarConstant1 = new TableSchema.TableColumn(schema);
				colvarConstant1.ColumnName = "Constant_1";
				colvarConstant1.DataType = DbType.Double;
				colvarConstant1.MaxLength = 0;
				colvarConstant1.AutoIncrement = false;
				colvarConstant1.IsNullable = true;
				colvarConstant1.IsPrimaryKey = false;
				colvarConstant1.IsForeignKey = false;
				colvarConstant1.IsReadOnly = false;
				colvarConstant1.DefaultSetting = @"";
				colvarConstant1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConstant1);
				
				TableSchema.TableColumn colvarConstant2 = new TableSchema.TableColumn(schema);
				colvarConstant2.ColumnName = "Constant_2";
				colvarConstant2.DataType = DbType.Double;
				colvarConstant2.MaxLength = 0;
				colvarConstant2.AutoIncrement = false;
				colvarConstant2.IsNullable = true;
				colvarConstant2.IsPrimaryKey = false;
				colvarConstant2.IsForeignKey = false;
				colvarConstant2.IsReadOnly = false;
				colvarConstant2.DefaultSetting = @"";
				colvarConstant2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConstant2);
				
				TableSchema.TableColumn colvarConstant3 = new TableSchema.TableColumn(schema);
				colvarConstant3.ColumnName = "Constant_3";
				colvarConstant3.DataType = DbType.Double;
				colvarConstant3.MaxLength = 0;
				colvarConstant3.AutoIncrement = false;
				colvarConstant3.IsNullable = true;
				colvarConstant3.IsPrimaryKey = false;
				colvarConstant3.IsForeignKey = false;
				colvarConstant3.IsReadOnly = false;
				colvarConstant3.DefaultSetting = @"";
				colvarConstant3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConstant3);
				
				TableSchema.TableColumn colvarConstant4 = new TableSchema.TableColumn(schema);
				colvarConstant4.ColumnName = "Constant_4";
				colvarConstant4.DataType = DbType.Double;
				colvarConstant4.MaxLength = 0;
				colvarConstant4.AutoIncrement = false;
				colvarConstant4.IsNullable = true;
				colvarConstant4.IsPrimaryKey = false;
				colvarConstant4.IsForeignKey = false;
				colvarConstant4.IsReadOnly = false;
				colvarConstant4.DefaultSetting = @"";
				colvarConstant4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConstant4);
				
				TableSchema.TableColumn colvarIsPlating = new TableSchema.TableColumn(schema);
				colvarIsPlating.ColumnName = "IsPlating";
				colvarIsPlating.DataType = DbType.Byte;
				colvarIsPlating.MaxLength = 0;
				colvarIsPlating.AutoIncrement = false;
				colvarIsPlating.IsNullable = true;
				colvarIsPlating.IsPrimaryKey = false;
				colvarIsPlating.IsForeignKey = false;
				colvarIsPlating.IsReadOnly = false;
				colvarIsPlating.DefaultSetting = @"";
				colvarIsPlating.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPlating);
				
				TableSchema.TableColumn colvarCurrentState = new TableSchema.TableColumn(schema);
				colvarCurrentState.ColumnName = "CurrentState";
				colvarCurrentState.DataType = DbType.Byte;
				colvarCurrentState.MaxLength = 0;
				colvarCurrentState.AutoIncrement = false;
				colvarCurrentState.IsNullable = true;
				colvarCurrentState.IsPrimaryKey = false;
				colvarCurrentState.IsForeignKey = false;
				colvarCurrentState.IsReadOnly = false;
				colvarCurrentState.DefaultSetting = @"";
				colvarCurrentState.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrentState);
				
				TableSchema.TableColumn colvarShowInWorkFlow = new TableSchema.TableColumn(schema);
				colvarShowInWorkFlow.ColumnName = "ShowInWorkFlow";
				colvarShowInWorkFlow.DataType = DbType.Byte;
				colvarShowInWorkFlow.MaxLength = 0;
				colvarShowInWorkFlow.AutoIncrement = false;
				colvarShowInWorkFlow.IsNullable = true;
				colvarShowInWorkFlow.IsPrimaryKey = false;
				colvarShowInWorkFlow.IsForeignKey = false;
				colvarShowInWorkFlow.IsReadOnly = false;
				colvarShowInWorkFlow.DefaultSetting = @"";
				colvarShowInWorkFlow.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowInWorkFlow);
				
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
				DataService.Providers["DataAcessProvider"].AddSchema("tblWorkTask",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("DepartmentID")]
		[Bindable(true)]
		public short? DepartmentID 
		{
			get { return GetColumnValue<short?>(Columns.DepartmentID); }
			set { SetColumnValue(Columns.DepartmentID, value); }
		}
		  
		[XmlAttribute("Constant1")]
		[Bindable(true)]
		public double? Constant1 
		{
			get { return GetColumnValue<double?>(Columns.Constant1); }
			set { SetColumnValue(Columns.Constant1, value); }
		}
		  
		[XmlAttribute("Constant2")]
		[Bindable(true)]
		public double? Constant2 
		{
			get { return GetColumnValue<double?>(Columns.Constant2); }
			set { SetColumnValue(Columns.Constant2, value); }
		}
		  
		[XmlAttribute("Constant3")]
		[Bindable(true)]
		public double? Constant3 
		{
			get { return GetColumnValue<double?>(Columns.Constant3); }
			set { SetColumnValue(Columns.Constant3, value); }
		}
		  
		[XmlAttribute("Constant4")]
		[Bindable(true)]
		public double? Constant4 
		{
			get { return GetColumnValue<double?>(Columns.Constant4); }
			set { SetColumnValue(Columns.Constant4, value); }
		}
		  
		[XmlAttribute("IsPlating")]
		[Bindable(true)]
		public byte? IsPlating 
		{
			get { return GetColumnValue<byte?>(Columns.IsPlating); }
			set { SetColumnValue(Columns.IsPlating, value); }
		}
		  
		[XmlAttribute("CurrentState")]
		[Bindable(true)]
		public byte? CurrentState 
		{
			get { return GetColumnValue<byte?>(Columns.CurrentState); }
			set { SetColumnValue(Columns.CurrentState, value); }
		}
		  
		[XmlAttribute("ShowInWorkFlow")]
		[Bindable(true)]
		public byte? ShowInWorkFlow 
		{
			get { return GetColumnValue<byte?>(Columns.ShowInWorkFlow); }
			set { SetColumnValue(Columns.ShowInWorkFlow, value); }
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
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a TblDepartment ActiveRecord object related to this TblWorkTask
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblDepartment TblDepartment
		{
			get { return SweetSoft.APEM.DataAccess.TblDepartment.FetchByID(this.DepartmentID); }
			set { SetColumnValue("DepartmentID", value.DepartmentID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varName,short? varDepartmentID,double? varConstant1,double? varConstant2,double? varConstant3,double? varConstant4,byte? varIsPlating,byte? varCurrentState,byte? varShowInWorkFlow,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblWorkTask item = new TblWorkTask();
			
			item.Name = varName;
			
			item.DepartmentID = varDepartmentID;
			
			item.Constant1 = varConstant1;
			
			item.Constant2 = varConstant2;
			
			item.Constant3 = varConstant3;
			
			item.Constant4 = varConstant4;
			
			item.IsPlating = varIsPlating;
			
			item.CurrentState = varCurrentState;
			
			item.ShowInWorkFlow = varShowInWorkFlow;
			
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
		public static void Update(int varId,string varName,short? varDepartmentID,double? varConstant1,double? varConstant2,double? varConstant3,double? varConstant4,byte? varIsPlating,byte? varCurrentState,byte? varShowInWorkFlow,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblWorkTask item = new TblWorkTask();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.DepartmentID = varDepartmentID;
			
				item.Constant1 = varConstant1;
			
				item.Constant2 = varConstant2;
			
				item.Constant3 = varConstant3;
			
				item.Constant4 = varConstant4;
			
				item.IsPlating = varIsPlating;
			
				item.CurrentState = varCurrentState;
			
				item.ShowInWorkFlow = varShowInWorkFlow;
			
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
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DepartmentIDColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn Constant1Column
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn Constant2Column
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn Constant3Column
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn Constant4Column
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPlatingColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrentStateColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowInWorkFlowColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Name = @"Name";
			 public static string DepartmentID = @"DepartmentID";
			 public static string Constant1 = @"Constant_1";
			 public static string Constant2 = @"Constant_2";
			 public static string Constant3 = @"Constant_3";
			 public static string Constant4 = @"Constant_4";
			 public static string IsPlating = @"IsPlating";
			 public static string CurrentState = @"CurrentState";
			 public static string ShowInWorkFlow = @"ShowInWorkFlow";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
