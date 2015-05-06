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
	/// Strongly-typed collection for the TblTax class.
	/// </summary>
    [Serializable]
	public partial class TblTaxCollection : ActiveList<TblTax, TblTaxCollection>
	{	   
		public TblTaxCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblTaxCollection</returns>
		public TblTaxCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblTax o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblTax table.
	/// </summary>
	[Serializable]
	public partial class TblTax : ActiveRecord<TblTax>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblTax()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblTax(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblTax(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblTax(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblTax", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarTaxID = new TableSchema.TableColumn(schema);
				colvarTaxID.ColumnName = "TaxID";
				colvarTaxID.DataType = DbType.Int16;
				colvarTaxID.MaxLength = 0;
				colvarTaxID.AutoIncrement = true;
				colvarTaxID.IsNullable = false;
				colvarTaxID.IsPrimaryKey = true;
				colvarTaxID.IsForeignKey = false;
				colvarTaxID.IsReadOnly = false;
				colvarTaxID.DefaultSetting = @"";
				colvarTaxID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTaxID);
				
				TableSchema.TableColumn colvarTaxCode = new TableSchema.TableColumn(schema);
				colvarTaxCode.ColumnName = "TaxCode";
				colvarTaxCode.DataType = DbType.String;
				colvarTaxCode.MaxLength = 50;
				colvarTaxCode.AutoIncrement = false;
				colvarTaxCode.IsNullable = false;
				colvarTaxCode.IsPrimaryKey = false;
				colvarTaxCode.IsForeignKey = false;
				colvarTaxCode.IsReadOnly = false;
				
						colvarTaxCode.DefaultSetting = @"('')";
				colvarTaxCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTaxCode);
				
				TableSchema.TableColumn colvarTaxName = new TableSchema.TableColumn(schema);
				colvarTaxName.ColumnName = "TaxName";
				colvarTaxName.DataType = DbType.String;
				colvarTaxName.MaxLength = 100;
				colvarTaxName.AutoIncrement = false;
				colvarTaxName.IsNullable = false;
				colvarTaxName.IsPrimaryKey = false;
				colvarTaxName.IsForeignKey = false;
				colvarTaxName.IsReadOnly = false;
				colvarTaxName.DefaultSetting = @"";
				colvarTaxName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTaxName);
				
				TableSchema.TableColumn colvarTaxPercentage = new TableSchema.TableColumn(schema);
				colvarTaxPercentage.ColumnName = "TaxPercentage";
				colvarTaxPercentage.DataType = DbType.Double;
				colvarTaxPercentage.MaxLength = 0;
				colvarTaxPercentage.AutoIncrement = false;
				colvarTaxPercentage.IsNullable = false;
				colvarTaxPercentage.IsPrimaryKey = false;
				colvarTaxPercentage.IsForeignKey = false;
				colvarTaxPercentage.IsReadOnly = false;
				colvarTaxPercentage.DefaultSetting = @"";
				colvarTaxPercentage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTaxPercentage);
				
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
				DataService.Providers["DataAcessProvider"].AddSchema("tblTax",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("TaxID")]
		[Bindable(true)]
		public short TaxID 
		{
			get { return GetColumnValue<short>(Columns.TaxID); }
			set { SetColumnValue(Columns.TaxID, value); }
		}
		  
		[XmlAttribute("TaxCode")]
		[Bindable(true)]
		public string TaxCode 
		{
			get { return GetColumnValue<string>(Columns.TaxCode); }
			set { SetColumnValue(Columns.TaxCode, value); }
		}
		  
		[XmlAttribute("TaxName")]
		[Bindable(true)]
		public string TaxName 
		{
			get { return GetColumnValue<string>(Columns.TaxName); }
			set { SetColumnValue(Columns.TaxName, value); }
		}
		  
		[XmlAttribute("TaxPercentage")]
		[Bindable(true)]
		public double TaxPercentage 
		{
			get { return GetColumnValue<double>(Columns.TaxPercentage); }
			set { SetColumnValue(Columns.TaxPercentage, value); }
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
        
		
		private SweetSoft.APEM.DataAccess.TblCylinderCollection colTblCylinderRecords;
		public SweetSoft.APEM.DataAccess.TblCylinderCollection TblCylinderRecords()
		{
			if(colTblCylinderRecords == null)
			{
				colTblCylinderRecords = new SweetSoft.APEM.DataAccess.TblCylinderCollection().Where(TblCylinder.Columns.TaxID, TaxID).Load();
				colTblCylinderRecords.ListChanged += new ListChangedEventHandler(colTblCylinderRecords_ListChanged);
			}
			return colTblCylinderRecords;
		}
				
		void colTblCylinderRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblCylinderRecords[e.NewIndex].TaxID = TaxID;
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varTaxCode,string varTaxName,double varTaxPercentage,bool varIsObsolete)
		{
			TblTax item = new TblTax();
			
			item.TaxCode = varTaxCode;
			
			item.TaxName = varTaxName;
			
			item.TaxPercentage = varTaxPercentage;
			
			item.IsObsolete = varIsObsolete;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(short varTaxID,string varTaxCode,string varTaxName,double varTaxPercentage,bool varIsObsolete)
		{
			TblTax item = new TblTax();
			
				item.TaxID = varTaxID;
			
				item.TaxCode = varTaxCode;
			
				item.TaxName = varTaxName;
			
				item.TaxPercentage = varTaxPercentage;
			
				item.IsObsolete = varIsObsolete;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn TaxIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn TaxCodeColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TaxNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TaxPercentageColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IsObsoleteColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string TaxID = @"TaxID";
			 public static string TaxCode = @"TaxCode";
			 public static string TaxName = @"TaxName";
			 public static string TaxPercentage = @"TaxPercentage";
			 public static string IsObsolete = @"IsObsolete";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colTblCylinderRecords != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblCylinder item in colTblCylinderRecords)
                    {
                        if (item.TaxID == null ||item.TaxID != TaxID)
                        {
                            item.TaxID = TaxID;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colTblCylinderRecords != null)
                {
                    colTblCylinderRecords.SaveAll();
               }
		}
        #endregion
	}
}
