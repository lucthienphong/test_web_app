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
	/// Strongly-typed collection for the TblPricing class.
	/// </summary>
    [Serializable]
	public partial class TblPricingCollection : ActiveList<TblPricing, TblPricingCollection>
	{	   
		public TblPricingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblPricingCollection</returns>
		public TblPricingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblPricing o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblPricing table.
	/// </summary>
	[Serializable]
	public partial class TblPricing : ActiveRecord<TblPricing>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblPricing()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblPricing(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblPricing(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblPricing(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblPricing", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPricingID = new TableSchema.TableColumn(schema);
				colvarPricingID.ColumnName = "PricingID";
				colvarPricingID.DataType = DbType.Int16;
				colvarPricingID.MaxLength = 0;
				colvarPricingID.AutoIncrement = true;
				colvarPricingID.IsNullable = false;
				colvarPricingID.IsPrimaryKey = true;
				colvarPricingID.IsForeignKey = false;
				colvarPricingID.IsReadOnly = false;
				colvarPricingID.DefaultSetting = @"";
				colvarPricingID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPricingID);
				
				TableSchema.TableColumn colvarPricingName = new TableSchema.TableColumn(schema);
				colvarPricingName.ColumnName = "PricingName";
				colvarPricingName.DataType = DbType.String;
				colvarPricingName.MaxLength = 50;
				colvarPricingName.AutoIncrement = false;
				colvarPricingName.IsNullable = false;
				colvarPricingName.IsPrimaryKey = false;
				colvarPricingName.IsForeignKey = false;
				colvarPricingName.IsReadOnly = false;
				colvarPricingName.DefaultSetting = @"";
				colvarPricingName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPricingName);
				
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
				
				TableSchema.TableColumn colvarForTobaccoCustomers = new TableSchema.TableColumn(schema);
				colvarForTobaccoCustomers.ColumnName = "ForTobaccoCustomers";
				colvarForTobaccoCustomers.DataType = DbType.Boolean;
				colvarForTobaccoCustomers.MaxLength = 0;
				colvarForTobaccoCustomers.AutoIncrement = false;
				colvarForTobaccoCustomers.IsNullable = false;
				colvarForTobaccoCustomers.IsPrimaryKey = false;
				colvarForTobaccoCustomers.IsForeignKey = false;
				colvarForTobaccoCustomers.IsReadOnly = false;
				
						colvarForTobaccoCustomers.DefaultSetting = @"((1))";
				colvarForTobaccoCustomers.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForTobaccoCustomers);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblPricing",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PricingID")]
		[Bindable(true)]
		public short PricingID 
		{
			get { return GetColumnValue<short>(Columns.PricingID); }
			set { SetColumnValue(Columns.PricingID, value); }
		}
		  
		[XmlAttribute("PricingName")]
		[Bindable(true)]
		public string PricingName 
		{
			get { return GetColumnValue<string>(Columns.PricingName); }
			set { SetColumnValue(Columns.PricingName, value); }
		}
		  
		[XmlAttribute("IsObsolete")]
		[Bindable(true)]
		public bool IsObsolete 
		{
			get { return GetColumnValue<bool>(Columns.IsObsolete); }
			set { SetColumnValue(Columns.IsObsolete, value); }
		}
		  
		[XmlAttribute("ForTobaccoCustomers")]
		[Bindable(true)]
		public bool ForTobaccoCustomers 
		{
			get { return GetColumnValue<bool>(Columns.ForTobaccoCustomers); }
			set { SetColumnValue(Columns.ForTobaccoCustomers, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private SweetSoft.APEM.DataAccess.TblCustomerQuotationPricingCollection colTblCustomerQuotationPricingRecords;
		public SweetSoft.APEM.DataAccess.TblCustomerQuotationPricingCollection TblCustomerQuotationPricingRecords()
		{
			if(colTblCustomerQuotationPricingRecords == null)
			{
				colTblCustomerQuotationPricingRecords = new SweetSoft.APEM.DataAccess.TblCustomerQuotationPricingCollection().Where(TblCustomerQuotationPricing.Columns.PricingID, PricingID).Load();
				colTblCustomerQuotationPricingRecords.ListChanged += new ListChangedEventHandler(colTblCustomerQuotationPricingRecords_ListChanged);
			}
			return colTblCustomerQuotationPricingRecords;
		}
				
		void colTblCustomerQuotationPricingRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblCustomerQuotationPricingRecords[e.NewIndex].PricingID = PricingID;
            }
		}
		private SweetSoft.APEM.DataAccess.TblCylinderCollection colTblCylinderRecords;
		public SweetSoft.APEM.DataAccess.TblCylinderCollection TblCylinderRecords()
		{
			if(colTblCylinderRecords == null)
			{
				colTblCylinderRecords = new SweetSoft.APEM.DataAccess.TblCylinderCollection().Where(TblCylinder.Columns.PricingID, PricingID).Load();
				colTblCylinderRecords.ListChanged += new ListChangedEventHandler(colTblCylinderRecords_ListChanged);
			}
			return colTblCylinderRecords;
		}
				
		void colTblCylinderRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblCylinderRecords[e.NewIndex].PricingID = PricingID;
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		#region Many To Many Helpers
		
		 
		public SweetSoft.APEM.DataAccess.TblCustomerQuotationCollection GetTblCustomerQuotationCollection() { return TblPricing.GetTblCustomerQuotationCollection(this.PricingID); }
		public static SweetSoft.APEM.DataAccess.TblCustomerQuotationCollection GetTblCustomerQuotationCollection(short varPricingID)
		{
		    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT * FROM [dbo].[tblCustomerQuotation] INNER JOIN [tblCustomerQuotation_Pricing] ON [tblCustomerQuotation].[CustomerID] = [tblCustomerQuotation_Pricing].[CustomerID] WHERE [tblCustomerQuotation_Pricing].[PricingID] = @PricingID", TblPricing.Schema.Provider.Name);
			cmd.AddParameter("@PricingID", varPricingID, DbType.Int16);
			IDataReader rdr = SubSonic.DataService.GetReader(cmd);
			TblCustomerQuotationCollection coll = new TblCustomerQuotationCollection();
			coll.LoadAndCloseReader(rdr);
			return coll;
		}
		
		public static void SaveTblCustomerQuotationMap(short varPricingID, TblCustomerQuotationCollection items)
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[PricingID] = @PricingID", TblPricing.Schema.Provider.Name);
			cmdDel.AddParameter("@PricingID", varPricingID, DbType.Int16);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (TblCustomerQuotation item in items)
			{
				TblCustomerQuotationPricing varTblCustomerQuotationPricing = new TblCustomerQuotationPricing();
				varTblCustomerQuotationPricing.SetColumnValue("PricingID", varPricingID);
				varTblCustomerQuotationPricing.SetColumnValue("CustomerID", item.GetPrimaryKeyValue());
				varTblCustomerQuotationPricing.Save();
			}
		}
		public static void SaveTblCustomerQuotationMap(short varPricingID, System.Web.UI.WebControls.ListItemCollection itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[PricingID] = @PricingID", TblPricing.Schema.Provider.Name);
			cmdDel.AddParameter("@PricingID", varPricingID, DbType.Int16);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (System.Web.UI.WebControls.ListItem l in itemList) 
			{
				if (l.Selected) 
				{
					TblCustomerQuotationPricing varTblCustomerQuotationPricing = new TblCustomerQuotationPricing();
					varTblCustomerQuotationPricing.SetColumnValue("PricingID", varPricingID);
					varTblCustomerQuotationPricing.SetColumnValue("CustomerID", l.Value);
					varTblCustomerQuotationPricing.Save();
				}
			}
		}
		public static void SaveTblCustomerQuotationMap(short varPricingID , int[] itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[PricingID] = @PricingID", TblPricing.Schema.Provider.Name);
			cmdDel.AddParameter("@PricingID", varPricingID, DbType.Int16);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (int item in itemList) 
			{
				TblCustomerQuotationPricing varTblCustomerQuotationPricing = new TblCustomerQuotationPricing();
				varTblCustomerQuotationPricing.SetColumnValue("PricingID", varPricingID);
				varTblCustomerQuotationPricing.SetColumnValue("CustomerID", item);
				varTblCustomerQuotationPricing.Save();
			}
		}
		
		public static void DeleteTblCustomerQuotationMap(short varPricingID) 
		{
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblCustomerQuotation_Pricing] WHERE [tblCustomerQuotation_Pricing].[PricingID] = @PricingID", TblPricing.Schema.Provider.Name);
			cmdDel.AddParameter("@PricingID", varPricingID, DbType.Int16);
			DataService.ExecuteQuery(cmdDel);
		}
		
		#endregion
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varPricingName,bool varIsObsolete,bool varForTobaccoCustomers)
		{
			TblPricing item = new TblPricing();
			
			item.PricingName = varPricingName;
			
			item.IsObsolete = varIsObsolete;
			
			item.ForTobaccoCustomers = varForTobaccoCustomers;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(short varPricingID,string varPricingName,bool varIsObsolete,bool varForTobaccoCustomers)
		{
			TblPricing item = new TblPricing();
			
				item.PricingID = varPricingID;
			
				item.PricingName = varPricingName;
			
				item.IsObsolete = varIsObsolete;
			
				item.ForTobaccoCustomers = varForTobaccoCustomers;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PricingIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PricingNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IsObsoleteColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ForTobaccoCustomersColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PricingID = @"PricingID";
			 public static string PricingName = @"PricingName";
			 public static string IsObsolete = @"IsObsolete";
			 public static string ForTobaccoCustomers = @"ForTobaccoCustomers";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colTblCustomerQuotationPricingRecords != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblCustomerQuotationPricing item in colTblCustomerQuotationPricingRecords)
                    {
                        if (item.PricingID != PricingID)
                        {
                            item.PricingID = PricingID;
                        }
                    }
               }
		
                if (colTblCylinderRecords != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblCylinder item in colTblCylinderRecords)
                    {
                        if (item.PricingID != PricingID)
                        {
                            item.PricingID = PricingID;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colTblCustomerQuotationPricingRecords != null)
                {
                    colTblCustomerQuotationPricingRecords.SaveAll();
               }
		
                if (colTblCylinderRecords != null)
                {
                    colTblCylinderRecords.SaveAll();
               }
		}
        #endregion
	}
}
