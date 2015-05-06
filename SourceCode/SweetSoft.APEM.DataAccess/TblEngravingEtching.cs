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
	/// Strongly-typed collection for the TblEngravingEtching class.
	/// </summary>
    [Serializable]
	public partial class TblEngravingEtchingCollection : ActiveList<TblEngravingEtching, TblEngravingEtchingCollection>
	{	   
		public TblEngravingEtchingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TblEngravingEtchingCollection</returns>
		public TblEngravingEtchingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TblEngravingEtching o = this[i];
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
	/// This is an ActiveRecord class which wraps the tblEngravingEtching table.
	/// </summary>
	[Serializable]
	public partial class TblEngravingEtching : ActiveRecord<TblEngravingEtching>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TblEngravingEtching()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TblEngravingEtching(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TblEngravingEtching(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TblEngravingEtching(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("tblEngravingEtching", TableType.Table, DataService.GetInstance("DataAcessProvider"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarEngravingID = new TableSchema.TableColumn(schema);
				colvarEngravingID.ColumnName = "EngravingID";
				colvarEngravingID.DataType = DbType.Int32;
				colvarEngravingID.MaxLength = 0;
				colvarEngravingID.AutoIncrement = true;
				colvarEngravingID.IsNullable = false;
				colvarEngravingID.IsPrimaryKey = true;
				colvarEngravingID.IsForeignKey = false;
				colvarEngravingID.IsReadOnly = false;
				colvarEngravingID.DefaultSetting = @"";
				colvarEngravingID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEngravingID);
				
				TableSchema.TableColumn colvarCylinderID = new TableSchema.TableColumn(schema);
				colvarCylinderID.ColumnName = "CylinderID";
				colvarCylinderID.DataType = DbType.Int32;
				colvarCylinderID.MaxLength = 0;
				colvarCylinderID.AutoIncrement = false;
				colvarCylinderID.IsNullable = false;
				colvarCylinderID.IsPrimaryKey = false;
				colvarCylinderID.IsForeignKey = false;
				colvarCylinderID.IsReadOnly = false;
				colvarCylinderID.DefaultSetting = @"";
				colvarCylinderID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCylinderID);
				
				TableSchema.TableColumn colvarSequence = new TableSchema.TableColumn(schema);
				colvarSequence.ColumnName = "Sequence";
				colvarSequence.DataType = DbType.Int32;
				colvarSequence.MaxLength = 0;
				colvarSequence.AutoIncrement = false;
				colvarSequence.IsNullable = false;
				colvarSequence.IsPrimaryKey = false;
				colvarSequence.IsForeignKey = false;
				colvarSequence.IsReadOnly = false;
				colvarSequence.DefaultSetting = @"";
				colvarSequence.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSequence);
				
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
				
				TableSchema.TableColumn colvarScreenLpi = new TableSchema.TableColumn(schema);
				colvarScreenLpi.ColumnName = "ScreenLpi";
				colvarScreenLpi.DataType = DbType.String;
				colvarScreenLpi.MaxLength = 20;
				colvarScreenLpi.AutoIncrement = false;
				colvarScreenLpi.IsNullable = true;
				colvarScreenLpi.IsPrimaryKey = false;
				colvarScreenLpi.IsForeignKey = false;
				colvarScreenLpi.IsReadOnly = false;
				colvarScreenLpi.DefaultSetting = @"";
				colvarScreenLpi.ForeignKeyTableName = "";
				schema.Columns.Add(colvarScreenLpi);
				
				TableSchema.TableColumn colvarCellType = new TableSchema.TableColumn(schema);
				colvarCellType.ColumnName = "CellType";
				colvarCellType.DataType = DbType.String;
				colvarCellType.MaxLength = 20;
				colvarCellType.AutoIncrement = false;
				colvarCellType.IsNullable = true;
				colvarCellType.IsPrimaryKey = false;
				colvarCellType.IsForeignKey = false;
				colvarCellType.IsReadOnly = false;
				colvarCellType.DefaultSetting = @"";
				colvarCellType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCellType);
				
				TableSchema.TableColumn colvarStylus = new TableSchema.TableColumn(schema);
				colvarStylus.ColumnName = "Stylus";
				colvarStylus.DataType = DbType.Int32;
				colvarStylus.MaxLength = 0;
				colvarStylus.AutoIncrement = false;
				colvarStylus.IsNullable = true;
				colvarStylus.IsPrimaryKey = false;
				colvarStylus.IsForeignKey = false;
				colvarStylus.IsReadOnly = false;
				colvarStylus.DefaultSetting = @"";
				colvarStylus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStylus);
				
				TableSchema.TableColumn colvarScreen = new TableSchema.TableColumn(schema);
				colvarScreen.ColumnName = "Screen";
				colvarScreen.DataType = DbType.String;
				colvarScreen.MaxLength = 20;
				colvarScreen.AutoIncrement = false;
				colvarScreen.IsNullable = true;
				colvarScreen.IsPrimaryKey = false;
				colvarScreen.IsForeignKey = false;
				colvarScreen.IsReadOnly = false;
				colvarScreen.DefaultSetting = @"";
				colvarScreen.ForeignKeyTableName = "";
				schema.Columns.Add(colvarScreen);
				
				TableSchema.TableColumn colvarAngle = new TableSchema.TableColumn(schema);
				colvarAngle.ColumnName = "Angle";
				colvarAngle.DataType = DbType.Double;
				colvarAngle.MaxLength = 0;
				colvarAngle.AutoIncrement = false;
				colvarAngle.IsNullable = true;
				colvarAngle.IsPrimaryKey = false;
				colvarAngle.IsForeignKey = false;
				colvarAngle.IsReadOnly = false;
				colvarAngle.DefaultSetting = @"";
				colvarAngle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAngle);
				
				TableSchema.TableColumn colvarGamma = new TableSchema.TableColumn(schema);
				colvarGamma.ColumnName = "Gamma";
				colvarGamma.DataType = DbType.String;
				colvarGamma.MaxLength = 20;
				colvarGamma.AutoIncrement = false;
				colvarGamma.IsNullable = true;
				colvarGamma.IsPrimaryKey = false;
				colvarGamma.IsForeignKey = false;
				colvarGamma.IsReadOnly = false;
				colvarGamma.DefaultSetting = @"";
				colvarGamma.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGamma);
				
				TableSchema.TableColumn colvarTargetCellSize = new TableSchema.TableColumn(schema);
				colvarTargetCellSize.ColumnName = "TargetCellSize";
				colvarTargetCellSize.DataType = DbType.Double;
				colvarTargetCellSize.MaxLength = 0;
				colvarTargetCellSize.AutoIncrement = false;
				colvarTargetCellSize.IsNullable = true;
				colvarTargetCellSize.IsPrimaryKey = false;
				colvarTargetCellSize.IsForeignKey = false;
				colvarTargetCellSize.IsReadOnly = false;
				colvarTargetCellSize.DefaultSetting = @"";
				colvarTargetCellSize.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTargetCellSize);
				
				TableSchema.TableColumn colvarTargetCellWall = new TableSchema.TableColumn(schema);
				colvarTargetCellWall.ColumnName = "TargetCellWall";
				colvarTargetCellWall.DataType = DbType.Double;
				colvarTargetCellWall.MaxLength = 0;
				colvarTargetCellWall.AutoIncrement = false;
				colvarTargetCellWall.IsNullable = true;
				colvarTargetCellWall.IsPrimaryKey = false;
				colvarTargetCellWall.IsForeignKey = false;
				colvarTargetCellWall.IsReadOnly = false;
				colvarTargetCellWall.DefaultSetting = @"";
				colvarTargetCellWall.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTargetCellWall);
				
				TableSchema.TableColumn colvarTargetCellDepth = new TableSchema.TableColumn(schema);
				colvarTargetCellDepth.ColumnName = "TargetCellDepth";
				colvarTargetCellDepth.DataType = DbType.Double;
				colvarTargetCellDepth.MaxLength = 0;
				colvarTargetCellDepth.AutoIncrement = false;
				colvarTargetCellDepth.IsNullable = true;
				colvarTargetCellDepth.IsPrimaryKey = false;
				colvarTargetCellDepth.IsForeignKey = false;
				colvarTargetCellDepth.IsReadOnly = false;
				colvarTargetCellDepth.DefaultSetting = @"";
				colvarTargetCellDepth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTargetCellDepth);
				
				TableSchema.TableColumn colvarDevelopingTime = new TableSchema.TableColumn(schema);
				colvarDevelopingTime.ColumnName = "DevelopingTime";
				colvarDevelopingTime.DataType = DbType.Int32;
				colvarDevelopingTime.MaxLength = 0;
				colvarDevelopingTime.AutoIncrement = false;
				colvarDevelopingTime.IsNullable = true;
				colvarDevelopingTime.IsPrimaryKey = false;
				colvarDevelopingTime.IsForeignKey = false;
				colvarDevelopingTime.IsReadOnly = false;
				colvarDevelopingTime.DefaultSetting = @"";
				colvarDevelopingTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDevelopingTime);
				
				TableSchema.TableColumn colvarEtchingTime = new TableSchema.TableColumn(schema);
				colvarEtchingTime.ColumnName = "EtchingTime";
				colvarEtchingTime.DataType = DbType.Int32;
				colvarEtchingTime.MaxLength = 0;
				colvarEtchingTime.AutoIncrement = false;
				colvarEtchingTime.IsNullable = true;
				colvarEtchingTime.IsPrimaryKey = false;
				colvarEtchingTime.IsForeignKey = false;
				colvarEtchingTime.IsReadOnly = false;
				colvarEtchingTime.DefaultSetting = @"";
				colvarEtchingTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEtchingTime);
				
				TableSchema.TableColumn colvarChromeCellSize = new TableSchema.TableColumn(schema);
				colvarChromeCellSize.ColumnName = "ChromeCellSize";
				colvarChromeCellSize.DataType = DbType.Double;
				colvarChromeCellSize.MaxLength = 0;
				colvarChromeCellSize.AutoIncrement = false;
				colvarChromeCellSize.IsNullable = true;
				colvarChromeCellSize.IsPrimaryKey = false;
				colvarChromeCellSize.IsForeignKey = false;
				colvarChromeCellSize.IsReadOnly = false;
				colvarChromeCellSize.DefaultSetting = @"";
				colvarChromeCellSize.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChromeCellSize);
				
				TableSchema.TableColumn colvarChromeCellWall = new TableSchema.TableColumn(schema);
				colvarChromeCellWall.ColumnName = "ChromeCellWall";
				colvarChromeCellWall.DataType = DbType.Double;
				colvarChromeCellWall.MaxLength = 0;
				colvarChromeCellWall.AutoIncrement = false;
				colvarChromeCellWall.IsNullable = true;
				colvarChromeCellWall.IsPrimaryKey = false;
				colvarChromeCellWall.IsForeignKey = false;
				colvarChromeCellWall.IsReadOnly = false;
				colvarChromeCellWall.DefaultSetting = @"";
				colvarChromeCellWall.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChromeCellWall);
				
				TableSchema.TableColumn colvarChromeCellDepth = new TableSchema.TableColumn(schema);
				colvarChromeCellDepth.ColumnName = "ChromeCellDepth";
				colvarChromeCellDepth.DataType = DbType.Double;
				colvarChromeCellDepth.MaxLength = 0;
				colvarChromeCellDepth.AutoIncrement = false;
				colvarChromeCellDepth.IsNullable = true;
				colvarChromeCellDepth.IsPrimaryKey = false;
				colvarChromeCellDepth.IsForeignKey = false;
				colvarChromeCellDepth.IsReadOnly = false;
				colvarChromeCellDepth.DefaultSetting = @"";
				colvarChromeCellDepth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChromeCellDepth);
				
				TableSchema.TableColumn colvarIsCopy = new TableSchema.TableColumn(schema);
				colvarIsCopy.ColumnName = "IsCopy";
				colvarIsCopy.DataType = DbType.Byte;
				colvarIsCopy.MaxLength = 0;
				colvarIsCopy.AutoIncrement = false;
				colvarIsCopy.IsNullable = false;
				colvarIsCopy.IsPrimaryKey = false;
				colvarIsCopy.IsForeignKey = false;
				colvarIsCopy.IsReadOnly = false;
				colvarIsCopy.DefaultSetting = @"";
				colvarIsCopy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsCopy);
				
				TableSchema.TableColumn colvarColor = new TableSchema.TableColumn(schema);
				colvarColor.ColumnName = "Color";
				colvarColor.DataType = DbType.String;
				colvarColor.MaxLength = 100;
				colvarColor.AutoIncrement = false;
				colvarColor.IsNullable = true;
				colvarColor.IsPrimaryKey = false;
				colvarColor.IsForeignKey = false;
				colvarColor.IsReadOnly = false;
				colvarColor.DefaultSetting = @"";
				colvarColor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarColor);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["DataAcessProvider"].AddSchema("tblEngravingEtching",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("EngravingID")]
		[Bindable(true)]
		public int EngravingID 
		{
			get { return GetColumnValue<int>(Columns.EngravingID); }
			set { SetColumnValue(Columns.EngravingID, value); }
		}
		  
		[XmlAttribute("CylinderID")]
		[Bindable(true)]
		public int CylinderID 
		{
			get { return GetColumnValue<int>(Columns.CylinderID); }
			set { SetColumnValue(Columns.CylinderID, value); }
		}
		  
		[XmlAttribute("Sequence")]
		[Bindable(true)]
		public int Sequence 
		{
			get { return GetColumnValue<int>(Columns.Sequence); }
			set { SetColumnValue(Columns.Sequence, value); }
		}
		  
		[XmlAttribute("JobID")]
		[Bindable(true)]
		public int JobID 
		{
			get { return GetColumnValue<int>(Columns.JobID); }
			set { SetColumnValue(Columns.JobID, value); }
		}
		  
		[XmlAttribute("ScreenLpi")]
		[Bindable(true)]
		public string ScreenLpi 
		{
			get { return GetColumnValue<string>(Columns.ScreenLpi); }
			set { SetColumnValue(Columns.ScreenLpi, value); }
		}
		  
		[XmlAttribute("CellType")]
		[Bindable(true)]
		public string CellType 
		{
			get { return GetColumnValue<string>(Columns.CellType); }
			set { SetColumnValue(Columns.CellType, value); }
		}
		  
		[XmlAttribute("Stylus")]
		[Bindable(true)]
		public int? Stylus 
		{
			get { return GetColumnValue<int?>(Columns.Stylus); }
			set { SetColumnValue(Columns.Stylus, value); }
		}
		  
		[XmlAttribute("Screen")]
		[Bindable(true)]
		public string Screen 
		{
			get { return GetColumnValue<string>(Columns.Screen); }
			set { SetColumnValue(Columns.Screen, value); }
		}
		  
		[XmlAttribute("Angle")]
		[Bindable(true)]
		public double? Angle 
		{
			get { return GetColumnValue<double?>(Columns.Angle); }
			set { SetColumnValue(Columns.Angle, value); }
		}
		  
		[XmlAttribute("Gamma")]
		[Bindable(true)]
		public string Gamma 
		{
			get { return GetColumnValue<string>(Columns.Gamma); }
			set { SetColumnValue(Columns.Gamma, value); }
		}
		  
		[XmlAttribute("TargetCellSize")]
		[Bindable(true)]
		public double? TargetCellSize 
		{
			get { return GetColumnValue<double?>(Columns.TargetCellSize); }
			set { SetColumnValue(Columns.TargetCellSize, value); }
		}
		  
		[XmlAttribute("TargetCellWall")]
		[Bindable(true)]
		public double? TargetCellWall 
		{
			get { return GetColumnValue<double?>(Columns.TargetCellWall); }
			set { SetColumnValue(Columns.TargetCellWall, value); }
		}
		  
		[XmlAttribute("TargetCellDepth")]
		[Bindable(true)]
		public double? TargetCellDepth 
		{
			get { return GetColumnValue<double?>(Columns.TargetCellDepth); }
			set { SetColumnValue(Columns.TargetCellDepth, value); }
		}
		  
		[XmlAttribute("DevelopingTime")]
		[Bindable(true)]
		public int? DevelopingTime 
		{
			get { return GetColumnValue<int?>(Columns.DevelopingTime); }
			set { SetColumnValue(Columns.DevelopingTime, value); }
		}
		  
		[XmlAttribute("EtchingTime")]
		[Bindable(true)]
		public int? EtchingTime 
		{
			get { return GetColumnValue<int?>(Columns.EtchingTime); }
			set { SetColumnValue(Columns.EtchingTime, value); }
		}
		  
		[XmlAttribute("ChromeCellSize")]
		[Bindable(true)]
		public double? ChromeCellSize 
		{
			get { return GetColumnValue<double?>(Columns.ChromeCellSize); }
			set { SetColumnValue(Columns.ChromeCellSize, value); }
		}
		  
		[XmlAttribute("ChromeCellWall")]
		[Bindable(true)]
		public double? ChromeCellWall 
		{
			get { return GetColumnValue<double?>(Columns.ChromeCellWall); }
			set { SetColumnValue(Columns.ChromeCellWall, value); }
		}
		  
		[XmlAttribute("ChromeCellDepth")]
		[Bindable(true)]
		public double? ChromeCellDepth 
		{
			get { return GetColumnValue<double?>(Columns.ChromeCellDepth); }
			set { SetColumnValue(Columns.ChromeCellDepth, value); }
		}
		  
		[XmlAttribute("IsCopy")]
		[Bindable(true)]
		public byte IsCopy 
		{
			get { return GetColumnValue<byte>(Columns.IsCopy); }
			set { SetColumnValue(Columns.IsCopy, value); }
		}
		  
		[XmlAttribute("Color")]
		[Bindable(true)]
		public string Color 
		{
			get { return GetColumnValue<string>(Columns.Color); }
			set { SetColumnValue(Columns.Color, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varCylinderID,int varSequence,int varJobID,string varScreenLpi,string varCellType,int? varStylus,string varScreen,double? varAngle,string varGamma,double? varTargetCellSize,double? varTargetCellWall,double? varTargetCellDepth,int? varDevelopingTime,int? varEtchingTime,double? varChromeCellSize,double? varChromeCellWall,double? varChromeCellDepth,byte varIsCopy,string varColor)
		{
			TblEngravingEtching item = new TblEngravingEtching();
			
			item.CylinderID = varCylinderID;
			
			item.Sequence = varSequence;
			
			item.JobID = varJobID;
			
			item.ScreenLpi = varScreenLpi;
			
			item.CellType = varCellType;
			
			item.Stylus = varStylus;
			
			item.Screen = varScreen;
			
			item.Angle = varAngle;
			
			item.Gamma = varGamma;
			
			item.TargetCellSize = varTargetCellSize;
			
			item.TargetCellWall = varTargetCellWall;
			
			item.TargetCellDepth = varTargetCellDepth;
			
			item.DevelopingTime = varDevelopingTime;
			
			item.EtchingTime = varEtchingTime;
			
			item.ChromeCellSize = varChromeCellSize;
			
			item.ChromeCellWall = varChromeCellWall;
			
			item.ChromeCellDepth = varChromeCellDepth;
			
			item.IsCopy = varIsCopy;
			
			item.Color = varColor;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varEngravingID,int varCylinderID,int varSequence,int varJobID,string varScreenLpi,string varCellType,int? varStylus,string varScreen,double? varAngle,string varGamma,double? varTargetCellSize,double? varTargetCellWall,double? varTargetCellDepth,int? varDevelopingTime,int? varEtchingTime,double? varChromeCellSize,double? varChromeCellWall,double? varChromeCellDepth,byte varIsCopy,string varColor)
		{
			TblEngravingEtching item = new TblEngravingEtching();
			
				item.EngravingID = varEngravingID;
			
				item.CylinderID = varCylinderID;
			
				item.Sequence = varSequence;
			
				item.JobID = varJobID;
			
				item.ScreenLpi = varScreenLpi;
			
				item.CellType = varCellType;
			
				item.Stylus = varStylus;
			
				item.Screen = varScreen;
			
				item.Angle = varAngle;
			
				item.Gamma = varGamma;
			
				item.TargetCellSize = varTargetCellSize;
			
				item.TargetCellWall = varTargetCellWall;
			
				item.TargetCellDepth = varTargetCellDepth;
			
				item.DevelopingTime = varDevelopingTime;
			
				item.EtchingTime = varEtchingTime;
			
				item.ChromeCellSize = varChromeCellSize;
			
				item.ChromeCellWall = varChromeCellWall;
			
				item.ChromeCellDepth = varChromeCellDepth;
			
				item.IsCopy = varIsCopy;
			
				item.Color = varColor;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn EngravingIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CylinderIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SequenceColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn JobIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ScreenLpiColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CellTypeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn StylusColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ScreenColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn AngleColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn GammaColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn TargetCellSizeColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn TargetCellWallColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn TargetCellDepthColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn DevelopingTimeColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn EtchingTimeColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn ChromeCellSizeColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn ChromeCellWallColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn ChromeCellDepthColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn IsCopyColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn ColorColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string EngravingID = @"EngravingID";
			 public static string CylinderID = @"CylinderID";
			 public static string Sequence = @"Sequence";
			 public static string JobID = @"JobID";
			 public static string ScreenLpi = @"ScreenLpi";
			 public static string CellType = @"CellType";
			 public static string Stylus = @"Stylus";
			 public static string Screen = @"Screen";
			 public static string Angle = @"Angle";
			 public static string Gamma = @"Gamma";
			 public static string TargetCellSize = @"TargetCellSize";
			 public static string TargetCellWall = @"TargetCellWall";
			 public static string TargetCellDepth = @"TargetCellDepth";
			 public static string DevelopingTime = @"DevelopingTime";
			 public static string EtchingTime = @"EtchingTime";
			 public static string ChromeCellSize = @"ChromeCellSize";
			 public static string ChromeCellWall = @"ChromeCellWall";
			 public static string ChromeCellDepth = @"ChromeCellDepth";
			 public static string IsCopy = @"IsCopy";
			 public static string Color = @"Color";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}