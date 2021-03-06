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
	/// Strongly-typed collection for the TblUserRole class.
	/// </summary>
    [Serializable]
	public partial class TblUserRoleCollection : ActiveList<TblUserRole, TblUserRoleCollection>
	{	   
		public TblUserRoleCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblUserRoleCollection</returns>
		public TblUserRoleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblUserRole o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblUserRole table.
	/// </summary>
	[Serializable]
	public partial class TblUserRole : ActiveRecord<TblUserRole>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblUserRole()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblUserRole(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblUserRole(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblUserRole(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblUserRole", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarUserID = new TableSchema.TableColumn(schema);
				colvarUserID.ColumnName = "UserID";
				colvarUserID.DataType = DbType.Int32;
				colvarUserID.MaxLength = 0;
				colvarUserID.AutoIncrement = false;
				colvarUserID.IsNullable = false;
				colvarUserID.IsPrimaryKey = true;
				colvarUserID.IsForeignKey = true;
				colvarUserID.IsReadOnly = false;
				colvarUserID.DefaultSetting = @"";
				
					colvarUserID.ForeignKeyTableName = "tblUser";
				schema.Columns.Add(colvarUserID);
				
				TableSchema.TableColumn colvarRoleID = new TableSchema.TableColumn(schema);
				colvarRoleID.ColumnName = "RoleID";
				colvarRoleID.DataType = DbType.Int32;
				colvarRoleID.MaxLength = 0;
				colvarRoleID.AutoIncrement = false;
				colvarRoleID.IsNullable = false;
				colvarRoleID.IsPrimaryKey = true;
				colvarRoleID.IsForeignKey = true;
				colvarRoleID.IsReadOnly = false;
				colvarRoleID.DefaultSetting = @"";
				
					colvarRoleID.ForeignKeyTableName = "tblRole";
				schema.Columns.Add(colvarRoleID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblUserRole",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("UserID")]
		[Bindable(true)]
		public int UserID 
		{
			get { return GetColumnValue<int>(Columns.UserID); }
			set { SetColumnValue(Columns.UserID, value); }
		}
		  
		[XmlAttribute("RoleID")]
		[Bindable(true)]
		public int RoleID 
		{
			get { return GetColumnValue<int>(Columns.RoleID); }
			set { SetColumnValue(Columns.RoleID, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a TblRole ActiveRecord object related to this TblUserRole
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblRole TblRole
		{
			get { return SweetSoft.APEM.DataAccess.TblRole.FetchByID(this.RoleID); }
			set { SetColumnValue("RoleID", value.RoleID); }
		}
		
		
		/// <summary>
		/// Returns a TblUser ActiveRecord object related to this TblUserRole
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblUser TblUser
		{
			get { return SweetSoft.APEM.DataAccess.TblUser.FetchByID(this.UserID); }
			set { SetColumnValue("UserID", value.UserID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varUserID,int varRoleID)
		{
			TblUserRole item = new TblUserRole();
			
			item.UserID = varUserID;
			
			item.RoleID = varRoleID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varUserID,int varRoleID)
		{
			TblUserRole item = new TblUserRole();
			
				item.UserID = varUserID;
			
				item.RoleID = varRoleID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn UserIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn RoleIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string UserID = @"UserID";
			 public static string RoleID = @"RoleID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
