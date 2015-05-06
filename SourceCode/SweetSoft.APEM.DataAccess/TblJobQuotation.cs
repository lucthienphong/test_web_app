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
	/// Strongly-typed collection for the TblJobQuotation class.
	/// </summary>
    [Serializable]
	public partial class TblJobQuotationCollection : ActiveList<TblJobQuotation, TblJobQuotationCollection>
	{	   
		public TblJobQuotationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblJobQuotationCollection</returns>
		public TblJobQuotationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblJobQuotation o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblJobQuotation table.
	/// </summary>
	[Serializable]
	public partial class TblJobQuotation : ActiveRecord<TblJobQuotation>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblJobQuotation()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblJobQuotation(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblJobQuotation(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblJobQuotation(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblJobQuotation", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarQuotationID = new TableSchema.TableColumn(schema);
				colvarQuotationID.ColumnName = "QuotationID";
				colvarQuotationID.DataType = DbType.Int32;
				colvarQuotationID.MaxLength = 0;
				colvarQuotationID.AutoIncrement = true;
				colvarQuotationID.IsNullable = false;
				colvarQuotationID.IsPrimaryKey = true;
				colvarQuotationID.IsForeignKey = false;
				colvarQuotationID.IsReadOnly = false;
				colvarQuotationID.DefaultSetting = @"";
				colvarQuotationID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuotationID);
				
				TableSchema.TableColumn colvarJobID = new TableSchema.TableColumn(schema);
				colvarJobID.ColumnName = "JobID";
				colvarJobID.DataType = DbType.Int32;
				colvarJobID.MaxLength = 0;
				colvarJobID.AutoIncrement = false;
				colvarJobID.IsNullable = false;
				colvarJobID.IsPrimaryKey = false;
				colvarJobID.IsForeignKey = false;
				colvarJobID.IsReadOnly = false;
				colvarJobID.DefaultSetting = @"";
				colvarJobID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarJobID);
				
				TableSchema.TableColumn colvarRevNumber = new TableSchema.TableColumn(schema);
				colvarRevNumber.ColumnName = "RevNumber";
				colvarRevNumber.DataType = DbType.Int32;
				colvarRevNumber.MaxLength = 0;
				colvarRevNumber.AutoIncrement = false;
				colvarRevNumber.IsNullable = false;
				colvarRevNumber.IsPrimaryKey = false;
				colvarRevNumber.IsForeignKey = false;
				colvarRevNumber.IsReadOnly = false;
				colvarRevNumber.DefaultSetting = @"";
				colvarRevNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRevNumber);
				
				TableSchema.TableColumn colvarQuotationNo = new TableSchema.TableColumn(schema);
				colvarQuotationNo.ColumnName = "QuotationNo";
				colvarQuotationNo.DataType = DbType.String;
				colvarQuotationNo.MaxLength = 50;
				colvarQuotationNo.AutoIncrement = false;
				colvarQuotationNo.IsNullable = true;
				colvarQuotationNo.IsPrimaryKey = false;
				colvarQuotationNo.IsForeignKey = false;
				colvarQuotationNo.IsReadOnly = false;
				colvarQuotationNo.DefaultSetting = @"";
				colvarQuotationNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuotationNo);
				
				TableSchema.TableColumn colvarQuotationDate = new TableSchema.TableColumn(schema);
				colvarQuotationDate.ColumnName = "QuotationDate";
				colvarQuotationDate.DataType = DbType.DateTime;
				colvarQuotationDate.MaxLength = 0;
				colvarQuotationDate.AutoIncrement = false;
				colvarQuotationDate.IsNullable = true;
				colvarQuotationDate.IsPrimaryKey = false;
				colvarQuotationDate.IsForeignKey = false;
				colvarQuotationDate.IsReadOnly = false;
				colvarQuotationDate.DefaultSetting = @"";
				colvarQuotationDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuotationDate);
				
				TableSchema.TableColumn colvarQuotationText = new TableSchema.TableColumn(schema);
				colvarQuotationText.ColumnName = "QuotationText";
				colvarQuotationText.DataType = DbType.String;
				colvarQuotationText.MaxLength = 1000;
				colvarQuotationText.AutoIncrement = false;
				colvarQuotationText.IsNullable = true;
				colvarQuotationText.IsPrimaryKey = false;
				colvarQuotationText.IsForeignKey = false;
				colvarQuotationText.IsReadOnly = false;
				colvarQuotationText.DefaultSetting = @"";
				colvarQuotationText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuotationText);
				
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
				DataService.Providers["DataAcessProvider"].AddSchema("tblJobQuotation",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("QuotationID")]
		[Bindable(true)]
		public int QuotationID 
		{
			get { return GetColumnValue<int>(Columns.QuotationID); }
			set { SetColumnValue(Columns.QuotationID, value); }
		}
		  
		[XmlAttribute("JobID")]
		[Bindable(true)]
		public int JobID 
		{
			get { return GetColumnValue<int>(Columns.JobID); }
			set { SetColumnValue(Columns.JobID, value); }
		}
		  
		[XmlAttribute("RevNumber")]
		[Bindable(true)]
		public int? RevNumber 
		{
			get { return GetColumnValue<int?>(Columns.RevNumber); }
			set { SetColumnValue(Columns.RevNumber, value); }
		}
		  
		[XmlAttribute("QuotationNo")]
		[Bindable(true)]
		public string QuotationNo 
		{
			get { return GetColumnValue<string>(Columns.QuotationNo); }
			set { SetColumnValue(Columns.QuotationNo, value); }
		}
		  
		[XmlAttribute("QuotationDate")]
		[Bindable(true)]
		public DateTime? QuotationDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.QuotationDate); }
			set { SetColumnValue(Columns.QuotationDate, value); }
		}
		  
		[XmlAttribute("QuotationText")]
		[Bindable(true)]
		public string QuotationText 
		{
			get { return GetColumnValue<string>(Columns.QuotationText); }
			set { SetColumnValue(Columns.QuotationText, value); }
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
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varJobID,int? varRevNumber,string varQuotationNo,DateTime? varQuotationDate,string varQuotationText,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblJobQuotation item = new TblJobQuotation();
			
			item.JobID = varJobID;
			
			item.RevNumber = varRevNumber;
			
			item.QuotationNo = varQuotationNo;
			
			item.QuotationDate = varQuotationDate;
			
			item.QuotationText = varQuotationText;
			
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
		public static void Update(int varQuotationID,int varJobID,int? varRevNumber,string varQuotationNo,DateTime? varQuotationDate,string varQuotationText,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblJobQuotation item = new TblJobQuotation();
			
				item.QuotationID = varQuotationID;
			
				item.JobID = varJobID;
			
				item.RevNumber = varRevNumber;
			
				item.QuotationNo = varQuotationNo;
			
				item.QuotationDate = varQuotationDate;
			
				item.QuotationText = varQuotationText;
			
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
        
        
        public static TableSchema.TableColumn QuotationIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn JobIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn RevNumberColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn QuotationNoColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn QuotationDateColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn QuotationTextColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string QuotationID = @"QuotationID";
			 public static string JobID = @"JobID";
			 public static string RevNumber = @"RevNumber";
			 public static string QuotationNo = @"QuotationNo";
			 public static string QuotationDate = @"QuotationDate";
			 public static string QuotationText = @"QuotationText";
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
