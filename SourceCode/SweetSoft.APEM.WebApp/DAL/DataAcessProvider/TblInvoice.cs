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
	/// Strongly-typed collection for the TblInvoice class.
	/// </summary>
    [Serializable]
	public partial class TblInvoiceCollection : ActiveList<TblInvoice, TblInvoiceCollection>
	{	   
		public TblInvoiceCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblInvoiceCollection</returns>
		public TblInvoiceCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblInvoice o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblInvoice table.
	/// </summary>
	[Serializable]
	public partial class TblInvoice : ActiveRecord<TblInvoice>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblInvoice()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblInvoice(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblInvoice(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblInvoice(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblInvoice", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarInvoiceID = new TableSchema.TableColumn(schema);
				colvarInvoiceID.ColumnName = "InvoiceID";
				colvarInvoiceID.DataType = DbType.Int32;
				colvarInvoiceID.MaxLength = 0;
				colvarInvoiceID.AutoIncrement = true;
				colvarInvoiceID.IsNullable = false;
				colvarInvoiceID.IsPrimaryKey = true;
				colvarInvoiceID.IsForeignKey = false;
				colvarInvoiceID.IsReadOnly = false;
				colvarInvoiceID.DefaultSetting = @"";
				colvarInvoiceID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInvoiceID);
				
				TableSchema.TableColumn colvarInvoiceNo = new TableSchema.TableColumn(schema);
				colvarInvoiceNo.ColumnName = "InvoiceNo";
				colvarInvoiceNo.DataType = DbType.String;
				colvarInvoiceNo.MaxLength = 50;
				colvarInvoiceNo.AutoIncrement = false;
				colvarInvoiceNo.IsNullable = false;
				colvarInvoiceNo.IsPrimaryKey = false;
				colvarInvoiceNo.IsForeignKey = false;
				colvarInvoiceNo.IsReadOnly = false;
				colvarInvoiceNo.DefaultSetting = @"";
				colvarInvoiceNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInvoiceNo);
				
				TableSchema.TableColumn colvarInvoiceDate = new TableSchema.TableColumn(schema);
				colvarInvoiceDate.ColumnName = "InvoiceDate";
				colvarInvoiceDate.DataType = DbType.DateTime;
				colvarInvoiceDate.MaxLength = 0;
				colvarInvoiceDate.AutoIncrement = false;
				colvarInvoiceDate.IsNullable = false;
				colvarInvoiceDate.IsPrimaryKey = false;
				colvarInvoiceDate.IsForeignKey = false;
				colvarInvoiceDate.IsReadOnly = false;
				colvarInvoiceDate.DefaultSetting = @"";
				colvarInvoiceDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInvoiceDate);
				
				TableSchema.TableColumn colvarCustomerID = new TableSchema.TableColumn(schema);
				colvarCustomerID.ColumnName = "CustomerID";
				colvarCustomerID.DataType = DbType.Int32;
				colvarCustomerID.MaxLength = 0;
				colvarCustomerID.AutoIncrement = false;
				colvarCustomerID.IsNullable = false;
				colvarCustomerID.IsPrimaryKey = false;
				colvarCustomerID.IsForeignKey = false;
				colvarCustomerID.IsReadOnly = false;
				colvarCustomerID.DefaultSetting = @"";
				colvarCustomerID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerID);
				
				TableSchema.TableColumn colvarContactID = new TableSchema.TableColumn(schema);
				colvarContactID.ColumnName = "ContactID";
				colvarContactID.DataType = DbType.Int32;
				colvarContactID.MaxLength = 0;
				colvarContactID.AutoIncrement = false;
				colvarContactID.IsNullable = false;
				colvarContactID.IsPrimaryKey = false;
				colvarContactID.IsForeignKey = false;
				colvarContactID.IsReadOnly = false;
				colvarContactID.DefaultSetting = @"";
				colvarContactID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactID);
				
				TableSchema.TableColumn colvarPONumber = new TableSchema.TableColumn(schema);
				colvarPONumber.ColumnName = "PONumber";
				colvarPONumber.DataType = DbType.String;
				colvarPONumber.MaxLength = 50;
				colvarPONumber.AutoIncrement = false;
				colvarPONumber.IsNullable = false;
				colvarPONumber.IsPrimaryKey = false;
				colvarPONumber.IsForeignKey = false;
				colvarPONumber.IsReadOnly = false;
				colvarPONumber.DefaultSetting = @"";
				colvarPONumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPONumber);
				
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
				
				TableSchema.TableColumn colvarRMValue = new TableSchema.TableColumn(schema);
				colvarRMValue.ColumnName = "RMValue";
				colvarRMValue.DataType = DbType.Decimal;
				colvarRMValue.MaxLength = 0;
				colvarRMValue.AutoIncrement = false;
				colvarRMValue.IsNullable = true;
				colvarRMValue.IsPrimaryKey = false;
				colvarRMValue.IsForeignKey = false;
				colvarRMValue.IsReadOnly = false;
				colvarRMValue.DefaultSetting = @"";
				colvarRMValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRMValue);
				
				TableSchema.TableColumn colvarCurrencyValue = new TableSchema.TableColumn(schema);
				colvarCurrencyValue.ColumnName = "CurrencyValue";
				colvarCurrencyValue.DataType = DbType.Decimal;
				colvarCurrencyValue.MaxLength = 0;
				colvarCurrencyValue.AutoIncrement = false;
				colvarCurrencyValue.IsNullable = true;
				colvarCurrencyValue.IsPrimaryKey = false;
				colvarCurrencyValue.IsForeignKey = false;
				colvarCurrencyValue.IsReadOnly = false;
				colvarCurrencyValue.DefaultSetting = @"";
				colvarCurrencyValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrencyValue);
				
				TableSchema.TableColumn colvarTaxID = new TableSchema.TableColumn(schema);
				colvarTaxID.ColumnName = "TaxID";
				colvarTaxID.DataType = DbType.Int16;
				colvarTaxID.MaxLength = 0;
				colvarTaxID.AutoIncrement = false;
				colvarTaxID.IsNullable = true;
				colvarTaxID.IsPrimaryKey = false;
				colvarTaxID.IsForeignKey = false;
				colvarTaxID.IsReadOnly = false;
				colvarTaxID.DefaultSetting = @"";
				colvarTaxID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTaxID);
				
				TableSchema.TableColumn colvarIrisQty = new TableSchema.TableColumn(schema);
				colvarIrisQty.ColumnName = "IrisQty";
				colvarIrisQty.DataType = DbType.Int32;
				colvarIrisQty.MaxLength = 0;
				colvarIrisQty.AutoIncrement = false;
				colvarIrisQty.IsNullable = true;
				colvarIrisQty.IsPrimaryKey = false;
				colvarIrisQty.IsForeignKey = false;
				colvarIrisQty.IsReadOnly = false;
				colvarIrisQty.DefaultSetting = @"";
				colvarIrisQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIrisQty);
				
				TableSchema.TableColumn colvarIrisPrice = new TableSchema.TableColumn(schema);
				colvarIrisPrice.ColumnName = "IrisPrice";
				colvarIrisPrice.DataType = DbType.Decimal;
				colvarIrisPrice.MaxLength = 0;
				colvarIrisPrice.AutoIncrement = false;
				colvarIrisPrice.IsNullable = true;
				colvarIrisPrice.IsPrimaryKey = false;
				colvarIrisPrice.IsForeignKey = false;
				colvarIrisPrice.IsReadOnly = false;
				colvarIrisPrice.DefaultSetting = @"";
				colvarIrisPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIrisPrice);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = 2000;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarRemarkScreen = new TableSchema.TableColumn(schema);
				colvarRemarkScreen.ColumnName = "RemarkScreen";
				colvarRemarkScreen.DataType = DbType.String;
				colvarRemarkScreen.MaxLength = 500;
				colvarRemarkScreen.AutoIncrement = false;
				colvarRemarkScreen.IsNullable = true;
				colvarRemarkScreen.IsPrimaryKey = false;
				colvarRemarkScreen.IsForeignKey = false;
				colvarRemarkScreen.IsReadOnly = false;
				colvarRemarkScreen.DefaultSetting = @"";
				colvarRemarkScreen.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarkScreen);
				
				TableSchema.TableColumn colvarPaymentTern = new TableSchema.TableColumn(schema);
				colvarPaymentTern.ColumnName = "PaymentTern";
				colvarPaymentTern.DataType = DbType.String;
				colvarPaymentTern.MaxLength = 200;
				colvarPaymentTern.AutoIncrement = false;
				colvarPaymentTern.IsNullable = true;
				colvarPaymentTern.IsPrimaryKey = false;
				colvarPaymentTern.IsForeignKey = false;
				colvarPaymentTern.IsReadOnly = false;
				colvarPaymentTern.DefaultSetting = @"";
				colvarPaymentTern.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPaymentTern);
				
				TableSchema.TableColumn colvarTotalPrice = new TableSchema.TableColumn(schema);
				colvarTotalPrice.ColumnName = "TotalPrice";
				colvarTotalPrice.DataType = DbType.Decimal;
				colvarTotalPrice.MaxLength = 0;
				colvarTotalPrice.AutoIncrement = false;
				colvarTotalPrice.IsNullable = true;
				colvarTotalPrice.IsPrimaryKey = false;
				colvarTotalPrice.IsForeignKey = false;
				colvarTotalPrice.IsReadOnly = false;
				colvarTotalPrice.DefaultSetting = @"";
				colvarTotalPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalPrice);
				
				TableSchema.TableColumn colvarNetTotal = new TableSchema.TableColumn(schema);
				colvarNetTotal.ColumnName = "NetTotal";
				colvarNetTotal.DataType = DbType.Decimal;
				colvarNetTotal.MaxLength = 0;
				colvarNetTotal.AutoIncrement = false;
				colvarNetTotal.IsNullable = true;
				colvarNetTotal.IsPrimaryKey = false;
				colvarNetTotal.IsForeignKey = false;
				colvarNetTotal.IsReadOnly = false;
				colvarNetTotal.DefaultSetting = @"";
				colvarNetTotal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetTotal);
				
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
				DataService.Providers["DataAcessProvider"].AddSchema("tblInvoice",schema);
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
		  
		[XmlAttribute("InvoiceNo")]
		[Bindable(true)]
		public string InvoiceNo 
		{
			get { return GetColumnValue<string>(Columns.InvoiceNo); }
			set { SetColumnValue(Columns.InvoiceNo, value); }
		}
		  
		[XmlAttribute("InvoiceDate")]
		[Bindable(true)]
		public DateTime InvoiceDate 
		{
			get { return GetColumnValue<DateTime>(Columns.InvoiceDate); }
			set { SetColumnValue(Columns.InvoiceDate, value); }
		}
		  
		[XmlAttribute("CustomerID")]
		[Bindable(true)]
		public int CustomerID 
		{
			get { return GetColumnValue<int>(Columns.CustomerID); }
			set { SetColumnValue(Columns.CustomerID, value); }
		}
		  
		[XmlAttribute("ContactID")]
		[Bindable(true)]
		public int ContactID 
		{
			get { return GetColumnValue<int>(Columns.ContactID); }
			set { SetColumnValue(Columns.ContactID, value); }
		}
		  
		[XmlAttribute("PONumber")]
		[Bindable(true)]
		public string PONumber 
		{
			get { return GetColumnValue<string>(Columns.PONumber); }
			set { SetColumnValue(Columns.PONumber, value); }
		}
		  
		[XmlAttribute("CurrencyID")]
		[Bindable(true)]
		public short CurrencyID 
		{
			get { return GetColumnValue<short>(Columns.CurrencyID); }
			set { SetColumnValue(Columns.CurrencyID, value); }
		}
		  
		[XmlAttribute("RMValue")]
		[Bindable(true)]
		public decimal? RMValue 
		{
			get { return GetColumnValue<decimal?>(Columns.RMValue); }
			set { SetColumnValue(Columns.RMValue, value); }
		}
		  
		[XmlAttribute("CurrencyValue")]
		[Bindable(true)]
		public decimal? CurrencyValue 
		{
			get { return GetColumnValue<decimal?>(Columns.CurrencyValue); }
			set { SetColumnValue(Columns.CurrencyValue, value); }
		}
		  
		[XmlAttribute("TaxID")]
		[Bindable(true)]
		public short? TaxID 
		{
			get { return GetColumnValue<short?>(Columns.TaxID); }
			set { SetColumnValue(Columns.TaxID, value); }
		}
		  
		[XmlAttribute("IrisQty")]
		[Bindable(true)]
		public int? IrisQty 
		{
			get { return GetColumnValue<int?>(Columns.IrisQty); }
			set { SetColumnValue(Columns.IrisQty, value); }
		}
		  
		[XmlAttribute("IrisPrice")]
		[Bindable(true)]
		public decimal? IrisPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.IrisPrice); }
			set { SetColumnValue(Columns.IrisPrice, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("RemarkScreen")]
		[Bindable(true)]
		public string RemarkScreen 
		{
			get { return GetColumnValue<string>(Columns.RemarkScreen); }
			set { SetColumnValue(Columns.RemarkScreen, value); }
		}
		  
		[XmlAttribute("PaymentTern")]
		[Bindable(true)]
		public string PaymentTern 
		{
			get { return GetColumnValue<string>(Columns.PaymentTern); }
			set { SetColumnValue(Columns.PaymentTern, value); }
		}
		  
		[XmlAttribute("TotalPrice")]
		[Bindable(true)]
		public decimal? TotalPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.TotalPrice); }
			set { SetColumnValue(Columns.TotalPrice, value); }
		}
		  
		[XmlAttribute("NetTotal")]
		[Bindable(true)]
		public decimal? NetTotal 
		{
			get { return GetColumnValue<decimal?>(Columns.NetTotal); }
			set { SetColumnValue(Columns.NetTotal, value); }
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
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private SweetSoft.APEM.DataAccess.TblInvoiceDetailCollection colTblInvoiceDetailRecords;
		public SweetSoft.APEM.DataAccess.TblInvoiceDetailCollection TblInvoiceDetailRecords()
		{
			if(colTblInvoiceDetailRecords == null)
			{
				colTblInvoiceDetailRecords = new SweetSoft.APEM.DataAccess.TblInvoiceDetailCollection().Where(TblInvoiceDetail.Columns.InvoiceID, InvoiceID).Load();
				colTblInvoiceDetailRecords.ListChanged += new ListChangedEventHandler(colTblInvoiceDetailRecords_ListChanged);
			}
			return colTblInvoiceDetailRecords;
		}
				
		void colTblInvoiceDetailRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colTblInvoiceDetailRecords[e.NewIndex].InvoiceID = InvoiceID;
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		#region Many To Many Helpers
		
		 
		public SweetSoft.APEM.DataAccess.TblJobCollection GetTblJobCollection() { return TblInvoice.GetTblJobCollection(this.InvoiceID); }
		public static SweetSoft.APEM.DataAccess.TblJobCollection GetTblJobCollection(int varInvoiceID)
		{
		    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT * FROM [dbo].[tblJob] INNER JOIN [tblInvoiceDetail] ON [tblJob].[JobID] = [tblInvoiceDetail].[JobID] WHERE [tblInvoiceDetail].[InvoiceID] = @InvoiceID", TblInvoice.Schema.Provider.Name);
			cmd.AddParameter("@InvoiceID", varInvoiceID, DbType.Int32);
			IDataReader rdr = SubSonic.DataService.GetReader(cmd);
			TblJobCollection coll = new TblJobCollection();
			coll.LoadAndCloseReader(rdr);
			return coll;
		}
		
		public static void SaveTblJobMap(int varInvoiceID, TblJobCollection items)
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblInvoiceDetail] WHERE [tblInvoiceDetail].[InvoiceID] = @InvoiceID", TblInvoice.Schema.Provider.Name);
			cmdDel.AddParameter("@InvoiceID", varInvoiceID, DbType.Int32);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (TblJob item in items)
			{
				TblInvoiceDetail varTblInvoiceDetail = new TblInvoiceDetail();
				varTblInvoiceDetail.SetColumnValue("InvoiceID", varInvoiceID);
				varTblInvoiceDetail.SetColumnValue("JobID", item.GetPrimaryKeyValue());
				varTblInvoiceDetail.Save();
			}
		}
		public static void SaveTblJobMap(int varInvoiceID, System.Web.UI.WebControls.ListItemCollection itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblInvoiceDetail] WHERE [tblInvoiceDetail].[InvoiceID] = @InvoiceID", TblInvoice.Schema.Provider.Name);
			cmdDel.AddParameter("@InvoiceID", varInvoiceID, DbType.Int32);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (System.Web.UI.WebControls.ListItem l in itemList) 
			{
				if (l.Selected) 
				{
					TblInvoiceDetail varTblInvoiceDetail = new TblInvoiceDetail();
					varTblInvoiceDetail.SetColumnValue("InvoiceID", varInvoiceID);
					varTblInvoiceDetail.SetColumnValue("JobID", l.Value);
					varTblInvoiceDetail.Save();
				}
			}
		}
		public static void SaveTblJobMap(int varInvoiceID , int[] itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblInvoiceDetail] WHERE [tblInvoiceDetail].[InvoiceID] = @InvoiceID", TblInvoice.Schema.Provider.Name);
			cmdDel.AddParameter("@InvoiceID", varInvoiceID, DbType.Int32);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (int item in itemList) 
			{
				TblInvoiceDetail varTblInvoiceDetail = new TblInvoiceDetail();
				varTblInvoiceDetail.SetColumnValue("InvoiceID", varInvoiceID);
				varTblInvoiceDetail.SetColumnValue("JobID", item);
				varTblInvoiceDetail.Save();
			}
		}
		
		public static void DeleteTblJobMap(int varInvoiceID) 
		{
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [tblInvoiceDetail] WHERE [tblInvoiceDetail].[InvoiceID] = @InvoiceID", TblInvoice.Schema.Provider.Name);
			cmdDel.AddParameter("@InvoiceID", varInvoiceID, DbType.Int32);
			DataService.ExecuteQuery(cmdDel);
		}
		
		#endregion
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varInvoiceNo,DateTime varInvoiceDate,int varCustomerID,int varContactID,string varPONumber,short varCurrencyID,decimal? varRMValue,decimal? varCurrencyValue,short? varTaxID,int? varIrisQty,decimal? varIrisPrice,string varRemark,string varRemarkScreen,string varPaymentTern,decimal? varTotalPrice,decimal? varNetTotal,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblInvoice item = new TblInvoice();
			
			item.InvoiceNo = varInvoiceNo;
			
			item.InvoiceDate = varInvoiceDate;
			
			item.CustomerID = varCustomerID;
			
			item.ContactID = varContactID;
			
			item.PONumber = varPONumber;
			
			item.CurrencyID = varCurrencyID;
			
			item.RMValue = varRMValue;
			
			item.CurrencyValue = varCurrencyValue;
			
			item.TaxID = varTaxID;
			
			item.IrisQty = varIrisQty;
			
			item.IrisPrice = varIrisPrice;
			
			item.Remark = varRemark;
			
			item.RemarkScreen = varRemarkScreen;
			
			item.PaymentTern = varPaymentTern;
			
			item.TotalPrice = varTotalPrice;
			
			item.NetTotal = varNetTotal;
			
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
		public static void Update(int varInvoiceID,string varInvoiceNo,DateTime varInvoiceDate,int varCustomerID,int varContactID,string varPONumber,short varCurrencyID,decimal? varRMValue,decimal? varCurrencyValue,short? varTaxID,int? varIrisQty,decimal? varIrisPrice,string varRemark,string varRemarkScreen,string varPaymentTern,decimal? varTotalPrice,decimal? varNetTotal,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			TblInvoice item = new TblInvoice();
			
				item.InvoiceID = varInvoiceID;
			
				item.InvoiceNo = varInvoiceNo;
			
				item.InvoiceDate = varInvoiceDate;
			
				item.CustomerID = varCustomerID;
			
				item.ContactID = varContactID;
			
				item.PONumber = varPONumber;
			
				item.CurrencyID = varCurrencyID;
			
				item.RMValue = varRMValue;
			
				item.CurrencyValue = varCurrencyValue;
			
				item.TaxID = varTaxID;
			
				item.IrisQty = varIrisQty;
			
				item.IrisPrice = varIrisPrice;
			
				item.Remark = varRemark;
			
				item.RemarkScreen = varRemarkScreen;
			
				item.PaymentTern = varPaymentTern;
			
				item.TotalPrice = varTotalPrice;
			
				item.NetTotal = varNetTotal;
			
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
        
        
        public static TableSchema.TableColumn InvoiceIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn InvoiceNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn InvoiceDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactIDColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PONumberColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyIDColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn RMValueColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyValueColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn TaxIDColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn IrisQtyColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn IrisPriceColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkScreenColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn PaymentTernColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalPriceColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn NetTotalColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string InvoiceID = @"InvoiceID";
			 public static string InvoiceNo = @"InvoiceNo";
			 public static string InvoiceDate = @"InvoiceDate";
			 public static string CustomerID = @"CustomerID";
			 public static string ContactID = @"ContactID";
			 public static string PONumber = @"PONumber";
			 public static string CurrencyID = @"CurrencyID";
			 public static string RMValue = @"RMValue";
			 public static string CurrencyValue = @"CurrencyValue";
			 public static string TaxID = @"TaxID";
			 public static string IrisQty = @"IrisQty";
			 public static string IrisPrice = @"IrisPrice";
			 public static string Remark = @"Remark";
			 public static string RemarkScreen = @"RemarkScreen";
			 public static string PaymentTern = @"PaymentTern";
			 public static string TotalPrice = @"TotalPrice";
			 public static string NetTotal = @"NetTotal";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colTblInvoiceDetailRecords != null)
                {
                    foreach (SweetSoft.APEM.DataAccess.TblInvoiceDetail item in colTblInvoiceDetailRecords)
                    {
                        if (item.InvoiceID != InvoiceID)
                        {
                            item.InvoiceID = InvoiceID;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colTblInvoiceDetailRecords != null)
                {
                    colTblInvoiceDetailRecords.SaveAll();
               }
		}
        #endregion
	}
}
