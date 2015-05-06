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
	/// Strongly-typed collection for the TblEngravingType class.
	/// </summary>
    [Serializable]
	public partial class TblEngravingTypeCollection : ActiveList<TblEngravingType, TblEngravingTypeCollection>
	{	   
		public TblEngravingTypeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblEngravingTypeCollection</returns>
		public TblEngravingTypeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblEngravingType o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblEngravingType table.
	/// </summary>
	[Serializable]
	public partial class TblEngravingType : ActiveRecord<TblEngravingType>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblEngravingType()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblEngravingType(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblEngravingType(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblEngravingType(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblEngravingType", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarEngravingTypeID = new TableSchema.TableColumn(schema);
				colvarEngravingTypeID.ColumnName = "EngravingTypeID";
				colvarEngravingTypeID.DataType = DbType.Int16;
				colvarEngravingTypeID.MaxLength = 0;
				colvarEngravingTypeID.AutoIncrement = true;
				colvarEngravingTypeID.IsNullable = false;
				colvarEngravingTypeID.IsPrimaryKey = true;
				colvarEngravingTypeID.IsForeignKey = false;
				colvarEngravingTypeID.IsReadOnly = false;
				colvarEngravingTypeID.DefaultSetting = @"";
				colvarEngravingTypeID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEngravingTypeID);
				
				TableSchema.TableColumn colvarEngravingTypeName = new TableSchema.TableColumn(schema);
				colvarEngravingTypeName.ColumnName = "EngravingTypeName";
				colvarEngravingTypeName.DataType = DbType.String;
				colvarEngravingTypeName.MaxLength = 100;
				colvarEngravingTypeName.AutoIncrement = false;
				colvarEngravingTypeName.IsNullable = false;
				colvarEngravingTypeName.IsPrimaryKey = false;
				colvarEngravingTypeName.IsForeignKey = false;
				colvarEngravingTypeName.IsReadOnly = false;
				colvarEngravingTypeName.DefaultSetting = @"";
				colvarEngravingTypeName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEngravingTypeName);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblEngravingType",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("EngravingTypeID")]
		[Bindable(true)]
		public short EngravingTypeID 
		{
			get { return GetColumnValue<short>(Columns.EngravingTypeID); }
			set { SetColumnValue(Columns.EngravingTypeID, value); }
		}
		  
		[XmlAttribute("EngravingTypeName")]
		[Bindable(true)]
		public string EngravingTypeName 
		{
			get { return GetColumnValue<string>(Columns.EngravingTypeName); }
			set { SetColumnValue(Columns.EngravingTypeName, value); }
		}
		  
		[XmlAttribute("IsObsolete")]
		[Bindable(true)]
		public bool IsObsolete 
		{
			get { return GetColumnValue<bool>(Columns.IsObsolete); }
			set { SetColumnValue(Columns.IsObsolete, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private SweetSoft.APEM.DataAccess.TblEngravingDetailCollection colTblEngravingDetailRecords;
		public SweetSoft.APEM.DataAccess.TblEngravingDetailCollection TblEngravingDetailRecords()
		{
			if(colTblEngravingDetailRecords == null)
			{
				colTblEngravingDetailRecords = new SweetSoft.APEM.DataAccess.TblEngravingDetailCollection().Where(TblEngravingDetail.Columns.EngravingTypeID, EngravingTypeID).Load();
				colTblEngravingDetailRecords.ListChanged += new ListChangedEventHandler(colTblEngravingDetailRecords_ListChanged);
			}
			return colTblEngravingDetailRecords;
		}
				
		void colTblEngravingDetailRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblEngravingDetailRecords[e.NewIndex].EngravingTypeID = EngravingTypeID;
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varEngravingTypeName,bool varIsObsolete)
		{
			TblEngravingType item = new TblEngravingType();
			
			item.EngravingTypeName = varEngravingTypeName;
			
			item.IsObsolete = varIsObsolete;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(short varEngravingTypeID,string varEngravingTypeName,bool varIsObsolete)
		{
			TblEngravingType item = new TblEngravingType();
			
				item.EngravingTypeID = varEngravingTypeID;
			
				item.EngravingTypeName = varEngravingTypeName;
			
				item.IsObsolete = varIsObsolete;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn EngravingTypeIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn EngravingTypeNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IsObsoleteColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string EngravingTypeID = @"EngravingTypeID";
			 public static string EngravingTypeName = @"EngravingTypeName";
			 public static string IsObsolete = @"IsObsolete";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colTblEngravingDetailRecords != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblEngravingDetail item in colTblEngravingDetailRecords)
                    {
                        if (item.EngravingTypeID != EngravingTypeID)
                        {
                            item.EngravingTypeID = EngravingTypeID;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colTblEngravingDetailRecords != null)
                {
                    colTblEngravingDetailRecords.SaveAll();
               }
		}
        #endregion
	}
}