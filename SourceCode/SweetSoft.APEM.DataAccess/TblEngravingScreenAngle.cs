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
	/// Strongly-typed collection for the TblEngravingScreenAngle class.
	/// </summary>
    [Serializable]
	public partial class TblEngravingScreenAngleCollection : ActiveList<TblEngravingScreenAngle, TblEngravingScreenAngleCollection>
	{	   
		public TblEngravingScreenAngleCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblEngravingScreenAngleCollection</returns>
		public TblEngravingScreenAngleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblEngravingScreenAngle o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblEngraving_ScreenAngle table.
	/// </summary>
	[Serializable]
	public partial class TblEngravingScreenAngle : ActiveRecord<TblEngravingScreenAngle>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblEngravingScreenAngle()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblEngravingScreenAngle(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblEngravingScreenAngle(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblEngravingScreenAngle(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblEngraving_ScreenAngle", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarScreen = new TableSchema.TableColumn(schema);
				colvarScreen.ColumnName = "Screen";
				colvarScreen.DataType = DbType.String;
				colvarScreen.MaxLength = 20;
				colvarScreen.AutoIncrement = false;
				colvarScreen.IsNullable = false;
				colvarScreen.IsPrimaryKey = true;
				colvarScreen.IsForeignKey = false;
				colvarScreen.IsReadOnly = false;
				colvarScreen.DefaultSetting = @"";
				colvarScreen.ForeignKeyTableName = "";
				schema.Columns.Add(colvarScreen);
				
				TableSchema.TableColumn colvarAngle = new TableSchema.TableColumn(schema);
				colvarAngle.ColumnName = "Angle";
				colvarAngle.DataType = DbType.Int32;
				colvarAngle.MaxLength = 0;
				colvarAngle.AutoIncrement = false;
				colvarAngle.IsNullable = false;
				colvarAngle.IsPrimaryKey = true;
				colvarAngle.IsForeignKey = false;
				colvarAngle.IsReadOnly = false;
				colvarAngle.DefaultSetting = @"";
				colvarAngle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAngle);
				
				TableSchema.TableColumn colvarSh = new TableSchema.TableColumn(schema);
				colvarSh.ColumnName = "SH";
				colvarSh.DataType = DbType.Int32;
				colvarSh.MaxLength = 0;
				colvarSh.AutoIncrement = false;
				colvarSh.IsNullable = true;
				colvarSh.IsPrimaryKey = false;
				colvarSh.IsForeignKey = false;
				colvarSh.IsReadOnly = false;
				colvarSh.DefaultSetting = @"";
				colvarSh.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSh);
				
				TableSchema.TableColumn colvarHl = new TableSchema.TableColumn(schema);
				colvarHl.ColumnName = "HL";
				colvarHl.DataType = DbType.Int32;
				colvarHl.MaxLength = 0;
				colvarHl.AutoIncrement = false;
				colvarHl.IsNullable = true;
				colvarHl.IsPrimaryKey = false;
				colvarHl.IsForeignKey = false;
				colvarHl.IsReadOnly = false;
				colvarHl.DefaultSetting = @"";
				colvarHl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHl);
				
				TableSchema.TableColumn colvarCh = new TableSchema.TableColumn(schema);
				colvarCh.ColumnName = "CH";
				colvarCh.DataType = DbType.Int32;
				colvarCh.MaxLength = 0;
				colvarCh.AutoIncrement = false;
				colvarCh.IsNullable = true;
				colvarCh.IsPrimaryKey = false;
				colvarCh.IsForeignKey = false;
				colvarCh.IsReadOnly = false;
				colvarCh.DefaultSetting = @"";
				colvarCh.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCh);
				
				TableSchema.TableColumn colvarMidtone = new TableSchema.TableColumn(schema);
				colvarMidtone.ColumnName = "Midtone";
				colvarMidtone.DataType = DbType.Int32;
				colvarMidtone.MaxLength = 0;
				colvarMidtone.AutoIncrement = false;
				colvarMidtone.IsNullable = true;
				colvarMidtone.IsPrimaryKey = false;
				colvarMidtone.IsForeignKey = false;
				colvarMidtone.IsReadOnly = false;
				colvarMidtone.DefaultSetting = @"";
				colvarMidtone.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMidtone);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblEngraving_ScreenAngle",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Screen")]
		[Bindable(true)]
		public string Screen 
		{
			get { return GetColumnValue<string>(Columns.Screen); }
			set { SetColumnValue(Columns.Screen, value); }
		}
		  
		[XmlAttribute("Angle")]
		[Bindable(true)]
		public int Angle 
		{
			get { return GetColumnValue<int>(Columns.Angle); }
			set { SetColumnValue(Columns.Angle, value); }
		}
		  
		[XmlAttribute("Sh")]
		[Bindable(true)]
		public int? Sh 
		{
			get { return GetColumnValue<int?>(Columns.Sh); }
			set { SetColumnValue(Columns.Sh, value); }
		}
		  
		[XmlAttribute("Hl")]
		[Bindable(true)]
		public int? Hl 
		{
			get { return GetColumnValue<int?>(Columns.Hl); }
			set { SetColumnValue(Columns.Hl, value); }
		}
		  
		[XmlAttribute("Ch")]
		[Bindable(true)]
		public int? Ch 
		{
			get { return GetColumnValue<int?>(Columns.Ch); }
			set { SetColumnValue(Columns.Ch, value); }
		}
		  
		[XmlAttribute("Midtone")]
		[Bindable(true)]
		public int? Midtone 
		{
			get { return GetColumnValue<int?>(Columns.Midtone); }
			set { SetColumnValue(Columns.Midtone, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varScreen,int varAngle,int? varSh,int? varHl,int? varCh,int? varMidtone)
		{
			TblEngravingScreenAngle item = new TblEngravingScreenAngle();
			
			item.Screen = varScreen;
			
			item.Angle = varAngle;
			
			item.Sh = varSh;
			
			item.Hl = varHl;
			
			item.Ch = varCh;
			
			item.Midtone = varMidtone;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varScreen,int varAngle,int? varSh,int? varHl,int? varCh,int? varMidtone)
		{
			TblEngravingScreenAngle item = new TblEngravingScreenAngle();
			
				item.Screen = varScreen;
			
				item.Angle = varAngle;
			
				item.Sh = varSh;
			
				item.Hl = varHl;
			
				item.Ch = varCh;
			
				item.Midtone = varMidtone;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ScreenColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn AngleColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ShColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn HlColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ChColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn MidtoneColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Screen = @"Screen";
			 public static string Angle = @"Angle";
			 public static string Sh = @"SH";
			 public static string Hl = @"HL";
			 public static string Ch = @"CH";
			 public static string Midtone = @"Midtone";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
