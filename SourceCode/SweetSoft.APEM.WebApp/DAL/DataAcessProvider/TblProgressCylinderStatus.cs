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
	/// Strongly-typed collection for the TblProgressCylinderStatus class.
	/// </summary>
    [Serializable]
	public partial class TblProgressCylinderStatusCollection : ActiveList<TblProgressCylinderStatus, TblProgressCylinderStatusCollection>
	{	   
		public TblProgressCylinderStatusCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblProgressCylinderStatusCollection</returns>
		public TblProgressCylinderStatusCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblProgressCylinderStatus o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblProgressCylinderStatus table.
	/// </summary>
	[Serializable]
	public partial class TblProgressCylinderStatus : ActiveRecord<TblProgressCylinderStatus>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblProgressCylinderStatus()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblProgressCylinderStatus(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblProgressCylinderStatus(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblProgressCylinderStatus(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblProgressCylinderStatus", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCylinderStatusID = new TableSchema.TableColumn(schema);
				colvarCylinderStatusID.ColumnName = "CylinderStatusID";
				colvarCylinderStatusID.DataType = DbType.Int16;
				colvarCylinderStatusID.MaxLength = 0;
				colvarCylinderStatusID.AutoIncrement = true;
				colvarCylinderStatusID.IsNullable = false;
				colvarCylinderStatusID.IsPrimaryKey = true;
				colvarCylinderStatusID.IsForeignKey = false;
				colvarCylinderStatusID.IsReadOnly = false;
				colvarCylinderStatusID.DefaultSetting = @"";
				colvarCylinderStatusID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCylinderStatusID);
				
				TableSchema.TableColumn colvarCylinderStatusName = new TableSchema.TableColumn(schema);
				colvarCylinderStatusName.ColumnName = "CylinderStatusName";
				colvarCylinderStatusName.DataType = DbType.String;
				colvarCylinderStatusName.MaxLength = 100;
				colvarCylinderStatusName.AutoIncrement = false;
				colvarCylinderStatusName.IsNullable = false;
				colvarCylinderStatusName.IsPrimaryKey = false;
				colvarCylinderStatusName.IsForeignKey = false;
				colvarCylinderStatusName.IsReadOnly = false;
				colvarCylinderStatusName.DefaultSetting = @"";
				colvarCylinderStatusName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCylinderStatusName);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.String;
				colvarDescription.MaxLength = 100;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
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
				DataService.Providers["DataAcessProvider"].AddSchema("tblProgressCylinderStatus",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CylinderStatusID")]
		[Bindable(true)]
		public short CylinderStatusID 
		{
			get { return GetColumnValue<short>(Columns.CylinderStatusID); }
			set { SetColumnValue(Columns.CylinderStatusID, value); }
		}
		  
		[XmlAttribute("CylinderStatusName")]
		[Bindable(true)]
		public string CylinderStatusName 
		{
			get { return GetColumnValue<string>(Columns.CylinderStatusName); }
			set { SetColumnValue(Columns.CylinderStatusName, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
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
        
		
		private SweetSoft.APEM.DataAccess.TblProgressCollection colTblProgressRecords;
		public SweetSoft.APEM.DataAccess.TblProgressCollection TblProgressRecords()
		{
			if(colTblProgressRecords == null)
			{
				colTblProgressRecords = new SweetSoft.APEM.DataAccess.TblProgressCollection().Where(TblProgress.Columns.CylinderStatusID, CylinderStatusID).Load();
				colTblProgressRecords.ListChanged += new ListChangedEventHandler(colTblProgressRecords_ListChanged);
			}
			return colTblProgressRecords;
		}
				
		void colTblProgressRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblProgressRecords[e.NewIndex].CylinderStatusID = CylinderStatusID;
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varCylinderStatusName,string varDescription,bool varIsObsolete)
		{
			TblProgressCylinderStatus item = new TblProgressCylinderStatus();
			
			item.CylinderStatusName = varCylinderStatusName;
			
			item.Description = varDescription;
			
			item.IsObsolete = varIsObsolete;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(short varCylinderStatusID,string varCylinderStatusName,string varDescription,bool varIsObsolete)
		{
			TblProgressCylinderStatus item = new TblProgressCylinderStatus();
			
				item.CylinderStatusID = varCylinderStatusID;
			
				item.CylinderStatusName = varCylinderStatusName;
			
				item.Description = varDescription;
			
				item.IsObsolete = varIsObsolete;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CylinderStatusIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CylinderStatusNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn IsObsoleteColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CylinderStatusID = @"CylinderStatusID";
			 public static string CylinderStatusName = @"CylinderStatusName";
			 public static string Description = @"Description";
			 public static string IsObsolete = @"IsObsolete";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colTblProgressRecords != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblProgress item in colTblProgressRecords)
                    {
                        if (item.CylinderStatusID == null ||item.CylinderStatusID != CylinderStatusID)
                        {
                            item.CylinderStatusID = CylinderStatusID;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colTblProgressRecords != null)
                {
                    colTblProgressRecords.SaveAll();
               }
		}
        #endregion
	}
}