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
    /// Strongly-typed collection for the TblJobProcessing class.
    /// </summary>
    [Serializable]
    public partial class TblJobProcessingCollection : ActiveList<TblJobProcessing, TblJobProcessingCollection>
    {
        public TblJobProcessingCollection() { }

        /// <summary>
        /// Filters an existing collection based on the set criteria. This is an in-memory filter
        /// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblJobProcessingCollection</returns>
        public TblJobProcessingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblJobProcessing o = this[i];
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
    /// This is an ActiveRecord class which wraps the TblJobProcessing table.
    /// </summary>
    [Serializable]
    public partial class TblJobProcessing : ActiveRecord<TblJobProcessing>, IActiveRecord
    {
        #region .ctors and Default Settings

        public TblJobProcessing()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        private void InitSetDefaults() { SetDefaults(); }

        public TblJobProcessing(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public TblJobProcessing(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public TblJobProcessing(string columnName, object columnValue)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByParam(columnName, columnValue);
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
            if (!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("TblJobProcessing", TableType.Table, DataService.GetInstance("DataAcessProvider"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns

                TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
                colvarId.ColumnName = "Id";
                colvarId.DataType = DbType.Int32;
                colvarId.MaxLength = 0;
                colvarId.AutoIncrement = true;
                colvarId.IsNullable = false;
                colvarId.IsPrimaryKey = true;
                colvarId.IsForeignKey = false;
                colvarId.IsReadOnly = false;
                colvarId.DefaultSetting = @"";
                colvarId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarId);

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

                TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
                colvarDescription.ColumnName = "Description";
                colvarDescription.DataType = DbType.String;
                colvarDescription.MaxLength = 1000;
                colvarDescription.AutoIncrement = false;
                colvarDescription.IsNullable = true;
                colvarDescription.IsPrimaryKey = false;
                colvarDescription.IsForeignKey = false;
                colvarDescription.IsReadOnly = false;
                colvarDescription.DefaultSetting = @"";
                colvarDescription.ForeignKeyTableName = "";
                schema.Columns.Add(colvarDescription);

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

                TableSchema.TableColumn colvarFinishedBy = new TableSchema.TableColumn(schema);
                colvarFinishedBy.ColumnName = "FinishedBy";
                colvarFinishedBy.DataType = DbType.String;
                colvarFinishedBy.MaxLength = 100;
                colvarFinishedBy.AutoIncrement = false;
                colvarFinishedBy.IsNullable = true;
                colvarFinishedBy.IsPrimaryKey = false;
                colvarFinishedBy.IsForeignKey = false;
                colvarFinishedBy.IsReadOnly = false;
                colvarFinishedBy.DefaultSetting = @"";
                colvarFinishedBy.ForeignKeyTableName = "";
                schema.Columns.Add(colvarFinishedBy);

                TableSchema.TableColumn colvarFinishedOn = new TableSchema.TableColumn(schema);
                colvarFinishedOn.ColumnName = "FinishedOn";
                colvarFinishedOn.DataType = DbType.DateTime;
                colvarFinishedOn.MaxLength = 0;
                colvarFinishedOn.AutoIncrement = false;
                colvarFinishedOn.IsNullable = true;
                colvarFinishedOn.IsPrimaryKey = false;
                colvarFinishedOn.IsForeignKey = false;
                colvarFinishedOn.IsReadOnly = false;
                colvarFinishedOn.DefaultSetting = @"";
                colvarFinishedOn.ForeignKeyTableName = "";
                schema.Columns.Add(colvarFinishedOn);

                TableSchema.TableColumn colvarDepartmentID = new TableSchema.TableColumn(schema);
                colvarDepartmentID.ColumnName = "DepartmentID";
                colvarDepartmentID.DataType = DbType.Int32;
                colvarDepartmentID.MaxLength = 0;
                colvarDepartmentID.AutoIncrement = false;
                colvarDepartmentID.IsNullable = false;
                colvarDepartmentID.IsPrimaryKey = false;
                colvarDepartmentID.IsForeignKey = false;
                colvarDepartmentID.IsReadOnly = false;
                colvarDepartmentID.DefaultSetting = @"";
                colvarDepartmentID.ForeignKeyTableName = "tblDepartment";
                schema.Columns.Add(colvarDepartmentID);

                TableSchema.TableColumn colvarJobStatus = new TableSchema.TableColumn(schema);
                colvarJobStatus.ColumnName = "JobStatus";
                colvarJobStatus.DataType = DbType.String;
                colvarJobStatus.MaxLength = 50;
                colvarJobStatus.AutoIncrement = false;
                colvarJobStatus.IsNullable = true;
                colvarJobStatus.IsPrimaryKey = false;
                colvarJobStatus.IsForeignKey = false;
                colvarJobStatus.IsReadOnly = false;
                colvarJobStatus.DefaultSetting = @"";
                colvarJobStatus.ForeignKeyTableName = "";
                schema.Columns.Add(colvarJobStatus);

                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["DataAcessProvider"].AddSchema("TblJobProcessing", schema);
            }
        }
        #endregion

        #region Props

        [XmlAttribute("Id")]
        [Bindable(true)]
        public int Id
        {
            get { return GetColumnValue<int>(Columns.Id); }
            set { SetColumnValue(Columns.Id, value); }
        }

        [XmlAttribute("JobID")]
        [Bindable(true)]
        public int JobID
        {
            get { return GetColumnValue<int>(Columns.JobID); }
            set { SetColumnValue(Columns.JobID, value); }
        }

        [XmlAttribute("Description")]
        [Bindable(true)]
        public string Description
        {
            get { return GetColumnValue<string>(Columns.Description); }
            set { SetColumnValue(Columns.Description, value); }
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

        [XmlAttribute("FinishedBy")]
        [Bindable(true)]
        public string FinishedBy
        {
            get { return GetColumnValue<string>(Columns.FinishedBy); }
            set { SetColumnValue(Columns.FinishedBy, value); }
        }

        [XmlAttribute("FinishdOn")]
        [Bindable(true)]
        public DateTime? FinishedOn
        {
            get { return GetColumnValue<DateTime?>(Columns.FinishedOn); }
            set { SetColumnValue(Columns.FinishedOn, value); }
        }

        [XmlAttribute("DepartmentID")]
        [Bindable(true)]
        public int? DepartmentID
        {
            get { return GetColumnValue<int?>(Columns.DepartmentID); }
            set { SetColumnValue(Columns.DepartmentID, value); }
        }

        [XmlAttribute("JobStatus")]
        [Bindable(true)]
        public string JobStatus
        {
            get { return GetColumnValue<string>(Columns.JobStatus); }
            set { SetColumnValue(Columns.JobStatus, value); }
        }
        
        #endregion

        #region ForeignKey Properties

        /// <summary>
        /// Returns a TblJob ActiveRecord object related to this TblJobProcessing
        /// 
        /// </summary>
        public SweetSoft.APEM.DataAccess.TblJob TblJob
        {
            get { return SweetSoft.APEM.DataAccess.TblJob.FetchByID(this.JobID); }
            set { SetColumnValue("JobID", value.JobID); }
        }

        /// <summary>
        /// Returns a TblCustomerQuotation ActiveRecord object related to this TblCustomerQuotationPricing
        /// 
        /// </summary>
        public SweetSoft.APEM.DataAccess.TblDepartment TblDepartment
        {
            get { return SweetSoft.APEM.DataAccess.TblDepartment.FetchByID(this.DepartmentID); }
            set { SetColumnValue("DepartmentID", value.DepartmentID); }
        }


        #endregion

        #region ObjectDataSource support


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static TblJobProcessing Insert(int varJobID, string varDescription,
            string varCreatedBy, DateTime? varCreatedOn,
            string varFinishedBy, DateTime? varFinishedOn, int? varDepartmentID,
            string varJobStatus)
        {
            TblJobProcessing item = new TblJobProcessing();

            item.Description = varDescription;

            item.JobID = varJobID;

            item.CreatedBy = varCreatedBy;

            item.CreatedOn = varCreatedOn;

            item.FinishedBy = varFinishedBy;

            item.FinishedOn = varFinishedOn;

            item.DepartmentID = varDepartmentID;

            item.JobStatus = varJobStatus;


            if (System.Web.HttpContext.Current != null)
                item.Save(System.Web.HttpContext.Current.User.Identity.Name);
            else
                item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);

            return item;
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        public static TblJobProcessing Update(int varId, int varJobID,
            string varDescription,
            string varCreatedBy, DateTime? varCreatedOn,
            string varFinishedBy, DateTime? varFinishedOn, int? varDepartmentID,
            string varJobStatus)
        {
            TblJobProcessing item = new TblJobProcessing();

            item.Id = varId;

            item.Description = varDescription;

            item.JobID = varJobID;

            item.CreatedBy = varCreatedBy;

            item.CreatedOn = varCreatedOn;

            item.FinishedBy = varFinishedBy;

            item.FinishedOn = varFinishedOn;

            item.DepartmentID = varDepartmentID;

            item.JobStatus = varJobStatus;


            item.IsNew = false;
            if (System.Web.HttpContext.Current != null)
                item.Save(System.Web.HttpContext.Current.User.Identity.Name);
            else
                item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);

            return item;
        }

        #endregion

        #region Typed Columns


        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }



        public static TableSchema.TableColumn JobIDColumn
        {
            get { return Schema.Columns[1]; }
        }



        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[2]; }
        }



        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[3]; }
        }



        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[4]; }
        }




        public static TableSchema.TableColumn FinishedByColumn
        {
            get { return Schema.Columns[5]; }
        }



        public static TableSchema.TableColumn FinishedOnColumn
        {
            get { return Schema.Columns[6]; }
        }

        

        public static TableSchema.TableColumn DepartmentIDColumn
        {
            get { return Schema.Columns[7]; }
        }


        public static TableSchema.TableColumn JobStatusColumn
        {
            get { return Schema.Columns[8]; }
        }

        

        #endregion

        #region Columns Struct
        public struct Columns
        {
            public static string Id = @"Id";
            public static string JobID = @"JobID";
            public static string Description = @"Description";
            public static string CreatedBy = @"CreatedBy";
            public static string CreatedOn = @"CreatedOn";
            public static string FinishedBy = @"FinishedBy";
            public static string FinishedOn = @"FinishedOn";
            public static string DepartmentID = @"DepartmentID";
            public static string JobStatus = @"JobStatus";
        }
        #endregion

        #region Update PK Collections

        #endregion

        #region Deep Save

        #endregion
    }
}
