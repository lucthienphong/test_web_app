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
	/// Strongly-typed collection for the TblNotification class.
	/// </summary>
    [Serializable]
	public partial class TblNotificationCollection : ActiveList<TblNotification, TblNotificationCollection>
	{	   
		public TblNotificationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblNotificationCollection</returns>
		public TblNotificationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblNotification o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblNotification table.
	/// </summary>
	[Serializable]
	public partial class TblNotification : ActiveRecord<TblNotification>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblNotification()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblNotification(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblNotification(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblNotification(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblNotification", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarNotificationID = new TableSchema.TableColumn(schema);
				colvarNotificationID.ColumnName = "NotificationID";
				colvarNotificationID.DataType = DbType.Int32;
				colvarNotificationID.MaxLength = 0;
				colvarNotificationID.AutoIncrement = true;
				colvarNotificationID.IsNullable = false;
				colvarNotificationID.IsPrimaryKey = true;
				colvarNotificationID.IsForeignKey = false;
				colvarNotificationID.IsReadOnly = false;
				colvarNotificationID.DefaultSetting = @"";
				colvarNotificationID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNotificationID);
				
				TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
				colvarTitle.ColumnName = "Title";
				colvarTitle.DataType = DbType.String;
				colvarTitle.MaxLength = 200;
				colvarTitle.AutoIncrement = false;
				colvarTitle.IsNullable = true;
				colvarTitle.IsPrimaryKey = false;
				colvarTitle.IsForeignKey = false;
				colvarTitle.IsReadOnly = false;
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarContents = new TableSchema.TableColumn(schema);
				colvarContents.ColumnName = "Contents";
				colvarContents.DataType = DbType.String;
				colvarContents.MaxLength = -1;
				colvarContents.AutoIncrement = false;
				colvarContents.IsNullable = true;
				colvarContents.IsPrimaryKey = false;
				colvarContents.IsForeignKey = false;
				colvarContents.IsReadOnly = false;
				colvarContents.DefaultSetting = @"";
				colvarContents.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContents);
				
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
				
				TableSchema.TableColumn colvarReceiveIds = new TableSchema.TableColumn(schema);
				colvarReceiveIds.ColumnName = "ReceiveIds";
				colvarReceiveIds.DataType = DbType.String;
				colvarReceiveIds.MaxLength = -1;
				colvarReceiveIds.AutoIncrement = false;
				colvarReceiveIds.IsNullable = true;
				colvarReceiveIds.IsPrimaryKey = false;
				colvarReceiveIds.IsForeignKey = false;
				colvarReceiveIds.IsReadOnly = false;
				colvarReceiveIds.DefaultSetting = @"";
				colvarReceiveIds.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiveIds);
				
				TableSchema.TableColumn colvarReceiveType = new TableSchema.TableColumn(schema);
				colvarReceiveType.ColumnName = "ReceiveType";
				colvarReceiveType.DataType = DbType.String;
				colvarReceiveType.MaxLength = 50;
				colvarReceiveType.AutoIncrement = false;
				colvarReceiveType.IsNullable = true;
				colvarReceiveType.IsPrimaryKey = false;
				colvarReceiveType.IsForeignKey = false;
				colvarReceiveType.IsReadOnly = false;
				colvarReceiveType.DefaultSetting = @"";
				colvarReceiveType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiveType);
				
				TableSchema.TableColumn colvarActions = new TableSchema.TableColumn(schema);
				colvarActions.ColumnName = "Actions";
				colvarActions.DataType = DbType.String;
				colvarActions.MaxLength = -1;
				colvarActions.AutoIncrement = false;
				colvarActions.IsNullable = true;
				colvarActions.IsPrimaryKey = false;
				colvarActions.IsForeignKey = false;
				colvarActions.IsReadOnly = false;
				colvarActions.DefaultSetting = @"";
				colvarActions.ForeignKeyTableName = "";
				schema.Columns.Add(colvarActions);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.String;
				colvarCreatedBy.MaxLength = 100;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = false;
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
				colvarCreatedOn.IsNullable = false;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarDateDismiss = new TableSchema.TableColumn(schema);
				colvarDateDismiss.ColumnName = "DateDismiss";
				colvarDateDismiss.DataType = DbType.String;
				colvarDateDismiss.MaxLength = -1;
				colvarDateDismiss.AutoIncrement = false;
				colvarDateDismiss.IsNullable = true;
				colvarDateDismiss.IsPrimaryKey = false;
				colvarDateDismiss.IsForeignKey = false;
				colvarDateDismiss.IsReadOnly = false;
				colvarDateDismiss.DefaultSetting = @"";
				colvarDateDismiss.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateDismiss);
				
				TableSchema.TableColumn colvarDismissBy = new TableSchema.TableColumn(schema);
				colvarDismissBy.ColumnName = "DismissBy";
				colvarDismissBy.DataType = DbType.String;
				colvarDismissBy.MaxLength = -1;
				colvarDismissBy.AutoIncrement = false;
				colvarDismissBy.IsNullable = true;
				colvarDismissBy.IsPrimaryKey = false;
				colvarDismissBy.IsForeignKey = false;
				colvarDismissBy.IsReadOnly = false;
				colvarDismissBy.DefaultSetting = @"";
				colvarDismissBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDismissBy);
				
				TableSchema.TableColumn colvarDismissEvent = new TableSchema.TableColumn(schema);
				colvarDismissEvent.ColumnName = "DismissEvent";
				colvarDismissEvent.DataType = DbType.String;
				colvarDismissEvent.MaxLength = 50;
				colvarDismissEvent.AutoIncrement = false;
				colvarDismissEvent.IsNullable = true;
				colvarDismissEvent.IsPrimaryKey = false;
				colvarDismissEvent.IsForeignKey = false;
				colvarDismissEvent.IsReadOnly = false;
				colvarDismissEvent.DefaultSetting = @"";
				colvarDismissEvent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDismissEvent);
				
				TableSchema.TableColumn colvarPageId = new TableSchema.TableColumn(schema);
				colvarPageId.ColumnName = "PageId";
				colvarPageId.DataType = DbType.String;
				colvarPageId.MaxLength = 250;
				colvarPageId.AutoIncrement = false;
				colvarPageId.IsNullable = true;
				colvarPageId.IsPrimaryKey = false;
				colvarPageId.IsForeignKey = false;
				colvarPageId.IsReadOnly = false;
				colvarPageId.DefaultSetting = @"";
				colvarPageId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPageId);
				
				TableSchema.TableColumn colvarCommandType = new TableSchema.TableColumn(schema);
				colvarCommandType.ColumnName = "CommandType";
				colvarCommandType.DataType = DbType.String;
				colvarCommandType.MaxLength = 50;
				colvarCommandType.AutoIncrement = false;
				colvarCommandType.IsNullable = true;
				colvarCommandType.IsPrimaryKey = false;
				colvarCommandType.IsForeignKey = false;
				colvarCommandType.IsReadOnly = false;
				colvarCommandType.DefaultSetting = @"";
				colvarCommandType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommandType);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblNotification",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("NotificationID")]
		[Bindable(true)]
		public int NotificationID 
		{
			get { return GetColumnValue<int>(Columns.NotificationID); }
			set { SetColumnValue(Columns.NotificationID, value); }
		}
		  
		[XmlAttribute("Title")]
		[Bindable(true)]
		public string Title 
		{
			get { return GetColumnValue<string>(Columns.Title); }
			set { SetColumnValue(Columns.Title, value); }
		}
		  
		[XmlAttribute("Contents")]
		[Bindable(true)]
		public string Contents 
		{
			get { return GetColumnValue<string>(Columns.Contents); }
			set { SetColumnValue(Columns.Contents, value); }
		}
		  
		[XmlAttribute("IsObsolete")]
		[Bindable(true)]
		public bool IsObsolete 
		{
			get { return GetColumnValue<bool>(Columns.IsObsolete); }
			set { SetColumnValue(Columns.IsObsolete, value); }
		}
		  
		[XmlAttribute("ReceiveIds")]
		[Bindable(true)]
		public string ReceiveIds 
		{
			get { return GetColumnValue<string>(Columns.ReceiveIds); }
			set { SetColumnValue(Columns.ReceiveIds, value); }
		}
		  
		[XmlAttribute("ReceiveType")]
		[Bindable(true)]
		public string ReceiveType 
		{
			get { return GetColumnValue<string>(Columns.ReceiveType); }
			set { SetColumnValue(Columns.ReceiveType, value); }
		}
		  
		[XmlAttribute("Actions")]
		[Bindable(true)]
		public string Actions 
		{
			get { return GetColumnValue<string>(Columns.Actions); }
			set { SetColumnValue(Columns.Actions, value); }
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
		public DateTime CreatedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("DateDismiss")]
		[Bindable(true)]
		public string DateDismiss 
		{
			get { return GetColumnValue<string>(Columns.DateDismiss); }
			set { SetColumnValue(Columns.DateDismiss, value); }
		}
		  
		[XmlAttribute("DismissBy")]
		[Bindable(true)]
		public string DismissBy 
		{
			get { return GetColumnValue<string>(Columns.DismissBy); }
			set { SetColumnValue(Columns.DismissBy, value); }
		}
		  
		[XmlAttribute("DismissEvent")]
		[Bindable(true)]
		public string DismissEvent 
		{
			get { return GetColumnValue<string>(Columns.DismissEvent); }
			set { SetColumnValue(Columns.DismissEvent, value); }
		}
		  
		[XmlAttribute("PageId")]
		[Bindable(true)]
		public string PageId 
		{
			get { return GetColumnValue<string>(Columns.PageId); }
			set { SetColumnValue(Columns.PageId, value); }
		}
		  
		[XmlAttribute("CommandType")]
		[Bindable(true)]
		public string CommandType 
		{
			get { return GetColumnValue<string>(Columns.CommandType); }
			set { SetColumnValue(Columns.CommandType, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varTitle,string varContents,bool varIsObsolete,string varReceiveIds,string varReceiveType,string varActions,string varCreatedBy,DateTime varCreatedOn,string varDateDismiss,string varDismissBy,string varDismissEvent,string varPageId,string varCommandType)
		{
			TblNotification item = new TblNotification();
			
			item.Title = varTitle;
			
			item.Contents = varContents;
			
			item.IsObsolete = varIsObsolete;
			
			item.ReceiveIds = varReceiveIds;
			
			item.ReceiveType = varReceiveType;
			
			item.Actions = varActions;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.DateDismiss = varDateDismiss;
			
			item.DismissBy = varDismissBy;
			
			item.DismissEvent = varDismissEvent;
			
			item.PageId = varPageId;
			
			item.CommandType = varCommandType;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varNotificationID,string varTitle,string varContents,bool varIsObsolete,string varReceiveIds,string varReceiveType,string varActions,string varCreatedBy,DateTime varCreatedOn,string varDateDismiss,string varDismissBy,string varDismissEvent,string varPageId,string varCommandType)
		{
			TblNotification item = new TblNotification();
			
				item.NotificationID = varNotificationID;
			
				item.Title = varTitle;
			
				item.Contents = varContents;
			
				item.IsObsolete = varIsObsolete;
			
				item.ReceiveIds = varReceiveIds;
			
				item.ReceiveType = varReceiveType;
			
				item.Actions = varActions;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.DateDismiss = varDateDismiss;
			
				item.DismissBy = varDismissBy;
			
				item.DismissEvent = varDismissEvent;
			
				item.PageId = varPageId;
			
				item.CommandType = varCommandType;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn NotificationIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ContentsColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn IsObsoleteColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiveIdsColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiveTypeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ActionsColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DateDismissColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DismissByColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn DismissEventColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn PageIdColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn CommandTypeColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string NotificationID = @"NotificationID";
			 public static string Title = @"Title";
			 public static string Contents = @"Contents";
			 public static string IsObsolete = @"IsObsolete";
			 public static string ReceiveIds = @"ReceiveIds";
			 public static string ReceiveType = @"ReceiveType";
			 public static string Actions = @"Actions";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string DateDismiss = @"DateDismiss";
			 public static string DismissBy = @"DismissBy";
			 public static string DismissEvent = @"DismissEvent";
			 public static string PageId = @"PageId";
			 public static string CommandType = @"CommandType";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
