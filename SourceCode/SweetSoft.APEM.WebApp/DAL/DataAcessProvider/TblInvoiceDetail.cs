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
	/// Strongly-typed collection for the TblInvoiceDetail class.
	/// </summary>
    [Serializable]
	public partial class TblInvoiceDetailCollection : ActiveList<TblInvoiceDetail, TblInvoiceDetailCollection>
	{	   
		public TblInvoiceDetailCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblInvoiceDetailCollection</returns>
		public TblInvoiceDetailCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblInvoiceDetail o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblInvoiceDetail table.
	/// </summary>
	[Serializable]
	public partial class TblInvoiceDetail : ActiveRecord<TblInvoiceDetail>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblInvoiceDetail()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblInvoiceDetail(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblInvoiceDetail(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblInvoiceDetail(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblInvoiceDetail", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarInvoiceID = new TableSchema.TableColumn(schema);
				colvarInvoiceID.ColumnName = "InvoiceID";
				colvarInvoiceID.DataType = DbType.Int32;
				colvarInvoiceID.MaxLength = 0;
				colvarInvoiceID.AutoIncrement = false;
				colvarInvoiceID.IsNullable = false;
				colvarInvoiceID.IsPrimaryKey = true;
				colvarInvoiceID.IsForeignKey = true;
				colvarInvoiceID.IsReadOnly = false;
				colvarInvoiceID.DefaultSetting = @"";
				
					colvarInvoiceID.ForeignKeyTableName = "tblInvoice";
				schema.Columns.Add(colvarInvoiceID);
				
				TableSchema.TableColumn colvarJobID = new TableSchema.TableColumn(schema);
				colvarJobID.ColumnName = "JobID";
				colvarJobID.DataType = DbType.Int32;
				colvarJobID.MaxLength = 0;
				colvarJobID.AutoIncrement = false;
				colvarJobID.IsNullable = false;
				colvarJobID.IsPrimaryKey = true;
				colvarJobID.IsForeignKey = true;
				colvarJobID.IsReadOnly = false;
				colvarJobID.DefaultSetting = @"";
				
					colvarJobID.ForeignKeyTableName = "tblJob";
				schema.Columns.Add(colvarJobID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblInvoiceDetail",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("InvoiceID")]
		[Bindable(true)]
		public int InvoiceID 
		{
			get { return GetColumnValue<int>(Columns.InvoiceID); }
			set { SetColumnValue(Columns.InvoiceID, value); }
		}
		  
		[XmlAttribute("JobID")]
		[Bindable(true)]
		public int JobID 
		{
			get { return GetColumnValue<int>(Columns.JobID); }
			set { SetColumnValue(Columns.JobID, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a TblInvoice ActiveRecord object related to this TblInvoiceDetail
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblInvoice TblInvoice
		{
			get { return SweetSoft.APEM.DataAccess.TblInvoice.FetchByID(this.InvoiceID); }
			set { SetColumnValue("InvoiceID", value.InvoiceID); }
		}
		
		
		/// <summary>
		/// Returns a TblJob ActiveRecord object related to this TblInvoiceDetail
		/// 
		/// </summary>
		public SweetSoft.APEM.DataAccess.TblJob TblJob
		{
			get { return SweetSoft.APEM.DataAccess.TblJob.FetchByID(this.JobID); }
			set { SetColumnValue("JobID", value.JobID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varInvoiceID,int varJobID)
		{
			TblInvoiceDetail item = new TblInvoiceDetail();
			
			item.InvoiceID = varInvoiceID;
			
			item.JobID = varJobID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varInvoiceID,int varJobID)
		{
			TblInvoiceDetail item = new TblInvoiceDetail();
			
				item.InvoiceID = varInvoiceID;
			
				item.JobID = varJobID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn InvoiceIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn JobIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string InvoiceID = @"InvoiceID";
			 public static string JobID = @"JobID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
