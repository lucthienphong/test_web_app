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
        public TblJobSheet Insert(TblJobSheet obj)
        {
            obj.Save(UserName);
            return obj;
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblJobSheet Update(TblJobSheet obj)
        {
            TblJobSheet item = new TblJobSheet();
            item.MarkOld();
            item.IsLoaded = true;

            item.JobID = obj.JobID;

            item.ReproOperator = obj.ReproOperator;

            item.Circumference = obj.Circumference;

            item.FaceWidth = obj.FaceWidth;

            item.Diameter = obj.Diameter;

            item.DiameterDiff = obj.DiameterDiff;

            item.ReproDate = obj.ReproDate;

            item.HasIrisProof = obj.HasIrisProof;

            item.IrisProof = obj.IrisProof;

            item.CylinderDate = obj.CylinderDate;

            item.PreAppoval = obj.PreAppoval;

            item.LeavingAPE = obj.LeavingAPE;

            item.DeilveryNotes = obj.DeilveryNotes;

            item.EyeMark = obj.EyeMark;

            item.EMWidth = obj.EMWidth;

            item.EMHeight = obj.EMHeight;

            item.EMColor = obj.EMColor;

            item.BackingID = obj.BackingID;

            item.EMPonsition = obj.EMPonsition;

            item.UNSizeV = obj.UNSizeV;

            item.UNSizeH = obj.UNSizeH;

            item.OpaqueInk = obj.OpaqueInk;

            item.OpaqueInkRate = obj.OpaqueInkRate;

            item.IsEndless = obj.IsEndless;

            item.PrintingDirection = obj.PrintingDirection;

            item.Barcode = obj.Barcode;

            item.BarcodeSize = obj.BarcodeSize;

            item.BarcodeColor = obj.BarcodeColor;

            item.BarcodeNo = obj.BarcodeNo;

            item.SupplyID = obj.SupplyID;

            item.Bwr = obj.Bwr;

            item.Traps = obj.Traps;

            item.Size = obj.Size;

            item.ColorTarget = obj.ColorTarget;

            item.TypeOfCylinder = obj.TypeOfCylinder;

            item.Printing = obj.Printing;

            item.ProofingMaterial = obj.ProofingMaterial;

            item.NumberOfRepeatH = obj.NumberOfRepeatH;

            item.NumberOfRepeatV = obj.NumberOfRepeatV;

            item.SRRemark = obj.SRRemark;

            item.ActualPrintingMaterial = obj.ActualPrintingMaterial;

            item.MaterialWidth = obj.MaterialWidth;

            item.Save(UserName);

            return item;
        }
    }
}