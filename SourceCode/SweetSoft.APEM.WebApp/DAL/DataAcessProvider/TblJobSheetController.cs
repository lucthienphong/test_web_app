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
    /// Controller class for tblJobSheet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblJobSheetController
    {
        // Preload our schema..
        TblJobSheet thisSchemaLoad = new TblJobSheet();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TblJobSheetCollection FetchAll()
        {
            TblJobSheetCollection coll = new TblJobSheetCollection();
            Query qry = new Query(TblJobSheet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblJobSheetCollection FetchByID(object JobID)
        {
            TblJobSheetCollection coll = new TblJobSheetCollection().Where("JobID", JobID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblJobSheetCollection FetchByQuery(Query qry)
        {
            TblJobSheetCollection coll = new TblJobSheetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object JobID)
        {
            return (TblJobSheet.Delete(JobID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object JobID)
        {
            return (TblJobSheet.Destroy(JobID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int JobID,string ReproOperator,double Circumference,double FaceWidth,double Diameter,double DiameterDiff,DateTime? ReproDate,bool? HasIrisProof,int? IrisProof,DateTime? CylinderDate,bool PreAppoval,bool LeavingAPE,string DeilveryNotes,bool? EyeMark,int? EMWidth,int? EMHeight,string EMColor,short? BackingID,short? EMPonsition,double? UNSizeV,double? UNSizeH,bool OpaqueInk,int? OpaqueInkRate,bool? IsEndless,string PrintingDirection,bool? Barcode,int? BarcodeSize,string BarcodeColor,string BarcodeNo,short? SupplyID,double? Bwr,bool? Traps,double? Size,string ColorTarget,string TypeOfCylinder,string Printing,string ProofingMaterial,int? NumberOfRepeatH,int? NumberOfRepeatV,string SRRemark,string ActualPrintingMaterial,string MaterialWidth)
	    {
		    TblJobSheet item = new TblJobSheet();
		    
            item.JobID = JobID;
            
            item.ReproOperator = ReproOperator;
            
            item.Circumference = Circumference;
            
            item.FaceWidth = FaceWidth;
            
            item.Diameter = Diameter;
            
            item.DiameterDiff = DiameterDiff;
            
            item.ReproDate = ReproDate;
            
            item.HasIrisProof = HasIrisProof;
            
            item.IrisProof = IrisProof;
            
            item.CylinderDate = CylinderDate;
            
            item.PreAppoval = PreAppoval;
            
            item.LeavingAPE = LeavingAPE;
            
            item.DeilveryNotes = DeilveryNotes;
            
            item.EyeMark = EyeMark;
            
            item.EMWidth = EMWidth;
            
            item.EMHeight = EMHeight;
            
            item.EMColor = EMColor;
            
            item.BackingID = BackingID;
            
            item.EMPonsition = EMPonsition;
            
            item.UNSizeV = UNSizeV;
            
            item.UNSizeH = UNSizeH;
            
            item.OpaqueInk = OpaqueInk;
            
            item.OpaqueInkRate = OpaqueInkRate;
            
            item.IsEndless = IsEndless;
            
            item.PrintingDirection = PrintingDirection;
            
            item.Barcode = Barcode;
            
            item.BarcodeSize = BarcodeSize;
            
            item.BarcodeColor = BarcodeColor;
            
            item.BarcodeNo = BarcodeNo;
            
            item.SupplyID = SupplyID;
            
            item.Bwr = Bwr;
            
            item.Traps = Traps;
            
            item.Size = Size;
            
            item.ColorTarget = ColorTarget;
            
            item.TypeOfCylinder = TypeOfCylinder;
            
            item.Printing = Printing;
            
            item.ProofingMaterial = ProofingMaterial;
            
            item.NumberOfRepeatH = NumberOfRepeatH;
            
            item.NumberOfRepeatV = NumberOfRepeatV;
            
            item.SRRemark = SRRemark;
            
            item.ActualPrintingMaterial = ActualPrintingMaterial;
            
            item.MaterialWidth = MaterialWidth;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int JobID,string ReproOperator,double Circumference,double FaceWidth,double Diameter,double DiameterDiff,DateTime? ReproDate,bool? HasIrisProof,int? IrisProof,DateTime? CylinderDate,bool PreAppoval,bool LeavingAPE,string DeilveryNotes,bool? EyeMark,int? EMWidth,int? EMHeight,string EMColor,short? BackingID,short? EMPonsition,double? UNSizeV,double? UNSizeH,bool OpaqueInk,int? OpaqueInkRate,bool? IsEndless,string PrintingDirection,bool? Barcode,int? BarcodeSize,string BarcodeColor,string BarcodeNo,short? SupplyID,double? Bwr,bool? Traps,double? Size,string ColorTarget,string TypeOfCylinder,string Printing,string ProofingMaterial,int? NumberOfRepeatH,int? NumberOfRepeatV,string SRRemark,string ActualPrintingMaterial,string MaterialWidth)
	    {
		    TblJobSheet item = new TblJobSheet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.JobID = JobID;
				
			item.ReproOperator = ReproOperator;
				
			item.Circumference = Circumference;
				
			item.FaceWidth = FaceWidth;
				
			item.Diameter = Diameter;
				
			item.DiameterDiff = DiameterDiff;
				
			item.ReproDate = ReproDate;
				
			item.HasIrisProof = HasIrisProof;
				
			item.IrisProof = IrisProof;
				
			item.CylinderDate = CylinderDate;
				
			item.PreAppoval = PreAppoval;
				
			item.LeavingAPE = LeavingAPE;
				
			item.DeilveryNotes = DeilveryNotes;
				
			item.EyeMark = EyeMark;
				
			item.EMWidth = EMWidth;
				
			item.EMHeight = EMHeight;
				
			item.EMColor = EMColor;
				
			item.BackingID = BackingID;
				
			item.EMPonsition = EMPonsition;
				
			item.UNSizeV = UNSizeV;
				
			item.UNSizeH = UNSizeH;
				
			item.OpaqueInk = OpaqueInk;
				
			item.OpaqueInkRate = OpaqueInkRate;
				
			item.IsEndless = IsEndless;
				
			item.PrintingDirection = PrintingDirection;
				
			item.Barcode = Barcode;
				
			item.BarcodeSize = BarcodeSize;
				
			item.BarcodeColor = BarcodeColor;
				
			item.BarcodeNo = BarcodeNo;
				
			item.SupplyID = SupplyID;
				
			item.Bwr = Bwr;
				
			item.Traps = Traps;
				
			item.Size = Size;
				
			item.ColorTarget = ColorTarget;
				
			item.TypeOfCylinder = TypeOfCylinder;
				
			item.Printing = Printing;
				
			item.ProofingMaterial = ProofingMaterial;
				
			item.NumberOfRepeatH = NumberOfRepeatH;
				
			item.NumberOfRepeatV = NumberOfRepeatV;
				
			item.SRRemark = SRRemark;
				
			item.ActualPrintingMaterial = ActualPrintingMaterial;
				
			item.MaterialWidth = MaterialWidth;
				
	        item.Save(UserName);
	    }
    }
}
