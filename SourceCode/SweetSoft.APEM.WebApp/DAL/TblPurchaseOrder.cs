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
	/// Strongly-typed collection for the TblPurchaseOrder class.
	/// </summary>
    [Serializable]
	public partial class TblPurchaseOrderCollection : ActiveList<TblPurchaseOrder, TblPurchaseOrderCollection>
	{	   
		public TblPurchaseOrderCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblPurchaseOrderCollection</returns>
		public TblPurchaseOrderCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblPurchaseOrder o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblPurchaseOrder table.
	/// </summary>
	[Serializable]
	public partial class TblPurchaseOrder : ActiveRecord<TblPurchaseOrder>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblPurchaseOrder()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblPurchaseOrder(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblPurchaseOrder(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblPurchaseOrder(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblPurchaseOrder", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPurchaseOrderID = new TableSchema.TableColumn(schema);
				colvarPurchaseOrderID.ColumnName = "PurchaseOrderID";
				colvarPurchaseOrderID.DataType = DbType.Int32;
				colvarPurchaseOrderID.MaxLength = 0;
				colvarPurchaseOrderID.AutoIncrement = true;
				colvarPurchaseOrderID.IsNullable = false;
				colvarPurchaseOrderID.IsPrimaryKey = true;
				colvarPurchaseOrderID.IsForeignKey = false;
				colvarPurchaseOrderID.IsReadOnly = false;
				colvarPurchaseOrderID.DefaultSetting = @"";
				colvarPurchaseOrderID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPurchaseOrderID);
				
				TableSchema.TableColumn colvarSupplierID = new TableSchema.TableColumn(schema);
				colvarSupplierID.ColumnName = "SupplierID";
				colvarSupplierID.DataType = DbType.Int32;
				colvarSupplierID.MaxLength = 0;
				colvarSupplierID.AutoIncrement = false;
				colvarSupplierID.IsNullable = false;
				colvarSupplierID.IsPrimaryKey = false;
				colvarSupplierID.IsForeignKey = true;
				colvarSupplierID.IsReadOnly = false;
				colvarSupplierID.DefaultSetting = @"";
				
					colvarSupplierID.ForeignKeyTableName = "tblSupplier";
				schema.Columns.Add(colvarSupplierID);
				
				TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
				colvarOrderDate.ColumnName = "OrderDate";
				colvarOrderDate.DataType = DbType.DateTime;
				colvarOrderDate.MaxLength = 0;
				colvarOrderDate.AutoIncrement = false;
				colvarOrderDate.IsNullable = false;
				colvarOrderDate.IsPrimaryKey = false;
				colvarOrderDate.IsForeignKey = false;
				colvarOrderDate.IsReadOnly = false;
				colvarOrderDate.DefaultSetting = @"";
				colvarOrderDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderDate);
				
				TableSchema.TableColumn colvarOrderNumber = new TableSchema.TableColumn(schema);
				colvarOrderNumber.ColumnName = "OrderNumber";
				colvarOrderNumber.DataType = DbType.String;
				colvarOrderNumber.MaxLength = 50;
				colvarOrderNumber.AutoIncrement = false;
				colvarOrderNumber.IsNullable = false;
				colvarOrderNumber.IsPrimaryKey = false;
				colvarOrderNumber.IsForeignKey = false;
				colvarOrderNumber.IsReadOnly = false;
				colvarOrderNumber.DefaultSetting = @"";
				colvarOrderNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderNumber);
				
				TableSchema.TableColumn colvarRequiredDeliveryDate = new TableSchema.TableColumn(schema);
				colvarRequiredDeliveryDate.ColumnName = "RequiredDeliveryDate";
				colvarRequiredDeliveryDate.DataType = DbType.DateTime;
				colvarRequiredDeliveryDate.MaxLength = 0;
				colvarRequiredDeliveryDate.AutoIncrement = false;
				colvarRequiredDeliveryDate.IsNullable = true;
				colvarRequiredDeliveryDate.IsPrimaryKey = false;
				colvarRequiredDeliveryDate.IsForeignKey = false;
				colvarRequiredDeliveryDate.IsReadOnly = false;
				colvarRequiredDeliveryDate.DefaultSetting = @"";
				colvarRequiredDeliveryDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRequiredDeliveryDate);
				
				TableSchema.TableColumn colvarCylinderType = new TableSchema.TableColumn(schema);
				colvarCylinderType.ColumnName = "CylinderType";
				colvarCylinderType.DataType = DbType.String;
				colvarCylinderType.MaxLength = 50;
				colvarCylinderType.AutoIncrement = false;
				colvarCylinderType.IsNullable = false;
				colvarCylinderType.IsPrimaryKey = false;
				colvarCylinderType.IsForeignKey = false;
				colvarCylinderType.IsReadOnly = false;
				colvarCylinderType.DefaultSetting = @"";
				colvarCylinderType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCylinderType);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = false;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarContactName = new TableSchema.TableColumn(schema);
				colvarContactName.ColumnName = "ContactName";
				colvarContactName.DataType = DbType.String;
				colvarContactName.MaxLength = 200;
				colvarContactName.AutoIncrement = false;
				colvarContactName.IsNullable = false;
				colvarContactName.IsPrimaryKey = false;
				colvarContactName.IsForeignKey = false;
				colvarContactName.IsReadOnly = false;
				colvarContactName.DefaultSetting = @"";
				colvarContactName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactName);
				
				TableSchema.TableColumn colvarContactPhone = new TableSchema.TableColumn(schema);
				colvarContactPhone.ColumnName = "ContactPhone";
				colvarContactPhone.DataType = DbType.String;
				colvarContactPhone.MaxLength = 50;
				colvarContactPhone.AutoIncrement = false;
				colvarContactPhone.IsNullable = false;
				colvarContactPhone.IsPrimaryKey = false;
				colvarContactPhone.IsForeignKey = false;
				colvarContactPhone.IsReadOnly = false;
				colvarContactPhone.DefaultSetting = @"";
				colvarContactPhone.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactPhone);
				
				TableSchema.TableColumn colvarContactEmail = new TableSchema.TableColumn(schema);
				colvarContactEmail.ColumnName = "ContactEmail";
				colvarContactEmail.DataType = DbType.String;
				colvarContactEmail.MaxLength = 200;
				colvarContactEmail.AutoIncrement = false;
				colvarContactEmail.IsNullable = false;
				colvarContactEmail.IsPrimaryKey = false;
				colvarContactEmail.IsForeignKey = false;
				colvarContactEmail.IsReadOnly = false;
				colvarContactEmail.DefaultSetting = @"";
				colvarContactEmail.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactEmail);
				
				TableSchema.TableColumn colvarCurrencyID = new TableSchema.TableColumn(schema);
				colvarCurrencyID.ColumnName = "CurrencyID";
				colvarCurrencyID.DataType = DbType.Int16;
				colvarCurrencyID.MaxLength = 0;
				colvarCurrencyID.AutoIncrement = false;
				colvarCurrencyID.IsNullable = false;
				colvarCurrencyID.IsPrimaryKey = false;
				colvarCurrencyID.IsForeignKey = false;
				colvarCurrencyID.IsReadOnly = false;
				colvarCurrencyID.DefaultSetting = @"";
				colvarCurrencyID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrencyID);
				
				TableSchema.TableColumn colvarTotalNumberOfCylinders = new TableSchema.TableColumn(schema);
				colvarTotalNumberOfCylinders.ColumnName = "TotalNumberOfCylinders";
				colvarTotalNumberOfCylinders.DataType = DbType.Int32;
				colvarTotalNumberOfCylinders.MaxLength = 0;
				colvarTotalNumberOfCylinders.AutoIncrement = false;
				colvarTotalNumberOfCylinders.IsNullable = true;
				colvarTotalNumberOfCylinders.IsPrimaryKey = false;
				colvarTotalNumberOfCylinders.IsForeignKey = false;
				colvarTotalNumberOfCylinders.IsReadOnly = false;
				colvarTotalNumberOfCylinders.DefaultSetting = @"";
				colvarTotalNumberOfCylinders.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalNumberOfCylinders);
				
				TableSchema.TableColumn colvarIsUrgent = new TableSchema.TableColumn(schema);
				colvarIsUrgent.ColumnName = "IsUrgent";
				colvarIsUrgent.DataType = DbType.Byte;
				colvarIsUrgent.MaxLength = 0;
				colvarIsUrgent.AutoIncrement = false;
				colvarIsUrgent.IsNullable = false;
				colvarIsUrgent.IsPrimaryKey = false;
				colvarIsUrgent.IsForeignKey = false;
				colvarIsUrgent.IsReadOnly = false;
				colvarIsUrgent.DefaultSetting = @"";
				colvarIsUrgent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsUrgent);
				
				TableSchema.TableColumn colvarJobID = new TableSchema.TableColumn(schema);
				colvarJobID.ColumnName = "JobID";
				colvarJobID.DataType = DbType.Int32;
				colvarJobID.MaxLength = 0;
				colvarJobID.AutoIncrement = false;
				colvarJobID.IsNullable = false;
				colvarJobID.IsPrimaryKey = false;
				colvarJobID.IsForeignKey = true;
				colvarJobID.IsReadOnly = false;
				colvarJobID.DefaultSetting = @"";
				
					colvarJobID.ForeignKeyTableName = "tblJob";
				schema.Columns.Add(colvarJobID);
				
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
				DataService.Providers["DataAcessProvider"].AddSchema("tblPurchaseOrder",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PurchaseOrderID")]
		[Bindable(true)]
		public int PurchaseOrderID 
		{
			get { return GetColumnValue<int>(Columns.PurchaseOrderID); }
			set { SetColumnValue(Columns.PurchaseOrderID, value); }
		}
		  
		[XmlAttribute("SupplierID")]
		[Bindable(true)]
		public int SupplierID 
		{
			get { return GetColumnValue<int>(Columns.SupplierID); }
			set { SetColumnValue(Columns.SupplierID, value); }
		}
		  
		[XmlAttribute("OrderDate")]
		[Bindable(true)]
		public DateTime OrderDate 
		{
			get { return GetColumnValue<DateTime>(Columns.OrderDate); }
			set { SetColumnValue(Columns.OrderDate, value); }
		}
		  
		[XmlAttribute("OrderNumber")]
		[Bindable(true)]
		public string OrderNumber 
		{
			get { return GetColumnValue<string>(Columns.OrderNumber); }
			set { SetColumnValue(Columns.OrderNumber, value); }
		}
		  
		[XmlAttribute("RequiredDeliveryDate")]
		[Bindable(true)]
		public DateTime? RequiredDeliveryDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.RequiredDeliveryDate); }
			set { SetColumnValue(Columns.RequiredDeliveryDate, value); }
		}
		  
		[XmlAttribute("CylinderType")]
		[Bindable(true)]
		public string CylinderType 
		{
			get { return GetColumnValue<string>(Columns.CylinderType); }
			set { SetColumnValue(Columns.CylinderType, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("ContactName")]
		[Bindable(true)]
		public string ContactName 
		{
			get { return GetColumnValue<string>(Columns.ContactName); }
			set { SetColumnValue(Columns.ContactName, value); }
		}
		  
		[XmlAttribute("ContactPhone")]
		[Bindable(true)]
		public string ContactPhone 
		{
			get { return GetColumnValue<string>(Columns.ContactPhone); }
			set { SetColumnValue(Columns.ContactPhone, value); }
		}
		  
		[XmlAttribute("ContactEmail")]
		[Bindable(true)]
		public string ContactEmail 
		{
			get { return GetColumnValue<string>(Columns.ContactEmail); }
			set { SetColumnValue(Columns.ContactEmail, value); }
		}
		  
		[XmlAttribute("CurrencyID")]
		[Bindable(true)]
		public short CurrencyID 
		{
			get { return GetColumnValue<short>(Columns.CurrencyID); }
			set { SetColumnValue(Columns.CurrencyID, value); }
		}
		  
		[XmlAttribute("TotalNumberOfCylinders")]
		[Bindable(true)]
		public int? TotalNumberOfCylinders 
		{
			get { return GetColumnValue<int?>(Columns.TotalNumberOfCylinders); }
			set { SetColumnValue(Columns.TotalNumberOfCylinders, value); }
		}
		  
		[XmlAttribute("IsUrgent")]
		[Bindable(true)]
		public byte IsUrgent 
		{
			get { return GetColumnValue<byte>(Columns.IsUrgent); }
			set { SetColumnValue(Columns.IsUrgent, value); }
		}
		  
		[XmlAttribute("JobID")]
		[Bindable(true)]
		public int JobID 
		{
			get { return GetColumnValue<int>(Columns.JobID); }
			set { SetColumnValue(Columns.JobID, value); }
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
		/// Returns a TblJob ActiveRecord object related to this TblPurchaseOrder
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblJob TblJob
		{
			get { return SweetSoft.APEM.DataAccess.TblJob.FetchByID(this.JobID); }
			set { SetColumnValue("JobID", value.JobID); }
		}
		
		
		/// <summary>
		/// Returns a TblSupplier ActiveRecord object related to this TblPurchaseOrder
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblSupplier TblSupplier
		{
			get { return SweetSoft.APEM.DataAccess.TblSupplier.FetchByID(this.SupplierID); }
			set { SetColumnValue("SupplierID", value.SupplierID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varSupplierID,DateTime varOrderDate,string varOrderNumber,DateTime? varRequiredDeliveryDate,string varCylinderType,string varRemark,string varContactName,string varContactPhone,string varContactEmail,short varCurrencyID,int? varTotalNumberOfCylinders,byte varIsUrgent,int varJobID,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblPurchaseOrder item = new TblPurchaseOrder();
			
			item.SupplierID = varSupplierID;
			
			item.OrderDate = varOrderDate;
			
			item.OrderNumber = varOrderNumber;
			
			item.RequiredDeliveryDate = varRequiredDeliveryDate;
			
			item.CylinderType = varCylinderType;
			
			item.Remark = varRemark;
			
			item.ContactName = varContactName;
			
			item.ContactPhone = varContactPhone;
			
			item.ContactEmail = varContactEmail;
			
			item.CurrencyID = varCurrencyID;
			
			item.TotalNumberOfCylinders = varTotalNumberOfCylinders;
			
			item.IsUrgent = varIsUrgent;
			
			item.JobID = varJobID;
			
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
		public static void Update(int varPurchaseOrderID,int varSupplierID,DateTime varOrderDate,string varOrderNumber,DateTime? varRequiredDeliveryDate,string varCylinderType,string varRemark,string varContactName,string varContactPhone,string varContactEmail,short varCurrencyID,int? varTotalNumberOfCylinders,byte varIsUrgent,int varJobID,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblPurchaseOrder item = new TblPurchaseOrder();
			
				item.PurchaseOrderID = varPurchaseOrderID;
			
				item.SupplierID = varSupplierID;
			
				item.OrderDate = varOrderDate;
			
				item.OrderNumber = varOrderNumber;
			
				item.RequiredDeliveryDate = varRequiredDeliveryDate;
			
				item.CylinderType = varCylinderType;
			
				item.Remark = varRemark;
			
				item.ContactName = varContactName;
			
				item.ContactPhone = varContactPhone;
			
				item.ContactEmail = varContactEmail;
			
				item.CurrencyID = varCurrencyID;
			
				item.TotalNumberOfCylinders = varTotalNumberOfCylinders;
			
				item.IsUrgent = varIsUrgent;
			
				item.JobID = varJobID;
			
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
        
        
        public static TableSchema.TableColumn PurchaseOrderIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SupplierIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderNumberColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn RequiredDeliveryDateColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CylinderTypeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactNameColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactPhoneColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactEmailColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyIDColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalNumberOfCylindersColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn IsUrgentColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn JobIDColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PurchaseOrderID = @"PurchaseOrderID";
			 public static string SupplierID = @"SupplierID";
			 public static string OrderDate = @"OrderDate";
			 public static string OrderNumber = @"OrderNumber";
			 public static string RequiredDeliveryDate = @"RequiredDeliveryDate";
			 public static string CylinderType = @"CylinderType";
			 public static string Remark = @"Remark";
			 public static string ContactName = @"ContactName";
			 public static string ContactPhone = @"ContactPhone";
			 public static string ContactEmail = @"ContactEmail";
			 public static string CurrencyID = @"CurrencyID";
			 public static string TotalNumberOfCylinders = @"TotalNumberOfCylinders";
			 public static string IsUrgent = @"IsUrgent";
			 public static string JobID = @"JobID";
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
