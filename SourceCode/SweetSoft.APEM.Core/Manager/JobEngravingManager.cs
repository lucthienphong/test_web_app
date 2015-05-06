using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using System.Reflection;
using System.Data;

namespace SweetSoft.APEM.Core.Manager
{
    public class EngravingTobaccoExtension : TblEngravingTobacco
    {
        //Phần bổ sung
        public TblEngravingTobacco Parent { get; set; }
        //Chuyển Parents -> Children
        public EngravingTobaccoExtension() { }
        public EngravingTobaccoExtension(TblEngravingTobacco parent)
        {
            Parent = parent;
            try
            {
                foreach (PropertyInfo prop in this.GetType().GetProperties())
                {
                    if (Parent.GetType().GetProperty(prop.Name) != null)
                        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
                }
            }
            catch
            {
            }
        }
        public string CustCylID { set; get; }
        public string HexagonalName { set; get; }
        public string CellShapeName { set; get; }
        public string GraditionName { set; get; }
    }

    public class EngravingEtchingExtension : TblEngravingEtching
    {
        //Phần bổ sung
        public TblEngravingEtching Parent { get; set; }
        //Chuyển Parents -> Children
        public EngravingEtchingExtension() { }
        public EngravingEtchingExtension(TblEngravingEtching parent)
        {
            Parent = parent;
            try
            {
                foreach (PropertyInfo prop in this.GetType().GetProperties())
                {
                    if (Parent.GetType().GetProperty(prop.Name) != null)
                        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
                }
            }
            catch
            {
            }
        }
        public string Color { set; get; }
        public string CustCylID { set; get; }
    }

    public class JobEngravingManager
    {
        public static TblEngraving Insert(TblEngraving obj)
        {
            return new TblEngravingController().Insert(obj);
        }

        public static TblEngraving SelectByID(int JobID)
        {
            return new Select().From(TblEngraving.Schema).Where(TblEngraving.JobIDColumn).IsEqualTo(JobID).ExecuteSingle<TblEngraving>();
        }

        public static TblEngraving Update(TblEngraving obj)
        {
            return new TblEngravingController().Update(obj);
        }

        public static bool Delete(int ID)
        {
            new Delete().From(TblEngravingDetail.Schema).Where(TblEngravingDetail.JobIDColumn).IsEqualTo(ID).Execute();
            return new TblEngravingController().Destroy(ID);
        }

        //Detail
        public static TblEngravingDetail InsertDetail(TblEngravingDetail obj)
        {
            return new TblEngravingDetailController().Insert(obj);
        }

        public static TblEngravingDetail SelectDetailByID(int ID)
        {
            return new Select().From(TblEngravingDetail.Schema).Where(TblEngravingDetail.EngravingIDColumn).IsEqualTo(ID).ExecuteSingle<TblEngravingDetail>();
        }

        public static TblEngravingDetail UpdateDetail(TblEngravingDetail obj)
        {
            return new TblEngravingDetailController().Update(obj);
        }

        public static bool DeleteDetail(int ID)
        {
            return new TblEngravingDetailController().Destroy(ID);
        }

        public static bool DeleteDetailByJobID(int ID)
        {
            try
            {
                new SubSonic.Delete().From(TblEngravingDetail.Schema).Where(TblEngravingDetail.JobIDColumn).IsEqualTo(ID).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static TblEngravingScreenAngle SelectEngravingScreenAngleByScreenAndAngle(string screen, int angle)
        {
            return new Select()
                .From(TblEngravingScreenAngle.Schema)
                .Where(TblEngravingScreenAngle.ScreenColumn).IsEqualTo(screen)
                .And(TblEngravingScreenAngle.AngleColumn).IsEqualTo(angle)
                .ExecuteSingle<TblEngravingScreenAngle>();
        }


        public static TblEngravingStylu SelectEngravingStylusByStylusAndSH(int stylus,int sh)
        {
            return new Select()
            .From(TblEngravingStylu.Schema)
            .Where(TblEngravingStylu.StylusColumn).IsEqualTo(stylus)
            .And(TblEngravingStylu.ShColumn).IsEqualTo(sh)
            .ExecuteSingle<TblEngravingStylu>();
        }

        public static List<int> GetListEngravingIdByJobID(int jobID)
        {
            return new Select(TblEngravingDetail.EngravingIDColumn).From(TblEngravingDetail.Schema).Where(TblEngravingDetail.JobIDColumn).IsEqualTo(jobID).ExecuteTypedList<int>();
        }

        public static DataTable EMGSelectAllForPrint(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblEngravingDetailSelectAllForPrint(JobID).GetReader());
            return dt;
        }

        #region Tobacco
        public static List<int> GetListEngravingIdTobaccoByJobID(int jobID)
        {
            return new Select(TblEngravingTobacco.EngravingIDColumn).From(TblEngravingTobacco.Schema).Where(TblEngravingTobacco.JobIDColumn).IsEqualTo(jobID).ExecuteTypedList<int>();
        }

        public static DataTable TobaccoSelectAll(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblEngravingTobaccoSelectAll(JobID).GetReader());
            return dt;
        }

        public static DataTable TobaccoSelectAllForPrinting(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblEngravingTobaccoSelectAllForPrint(JobID).GetReader());
            return dt;
        }

        public static List<EngravingTobaccoExtension> TobaccoSelectAllForPrint(int JobID)
        {
            List<EngravingTobaccoExtension> list = new List<EngravingTobaccoExtension>();
            //Select cylinder by Job
            TblCylinderCollection cylColl = CylinderManager.SelectCylinderByJobID(JobID);
            //Select Engraving Tobbacco
            TblEngravingTobaccoCollection tobColl = new Select().From(TblEngravingTobacco.Schema)
                                                            .Where(TblEngravingTobacco.JobIDColumn).IsEqualTo(JobID)
                                                            .ExecuteAsCollection<TblEngravingTobaccoCollection>();
            //Select Hexagonal
            List<TblReference> hexaColl = ReferenceTableManager.SelectHexagonalForDDL();
            //Select CellShape
            List<TblReference> cellShapeColl = ReferenceTableManager.SelectCellShapeForDDL();
            //Select CellShape
            List<TblReference> gradationColl = ReferenceTableManager.SelectGradationForDDL();
            //Fill dữ liệu từ cylinder gốc
            foreach (TblCylinder item in cylColl)
            {
                //if (item.Protocol == EngravingProtocol.DLS.ToString())
                {
                    int HexagonalID = 0, CellShapeID = 0, GradationID = 0;
                    TblEngravingTobacco obj = tobColl.Where(x => x.CylinderID == item.CylinderID && x.Sequence <= 10).FirstOrDefault();
                    if (obj == null)
                        obj = new TblEngravingTobacco();
                    else
                    {
                        HexagonalID = obj.Hexagonal != null ? (int)obj.Hexagonal : 0;
                        CellShapeID = obj.CellShape != null ? (int)obj.CellShape : 0;
                        GradationID = obj.Gradation != null ? (int)obj.Gradation : 0;
                    }
                    EngravingTobaccoExtension eObj = new EngravingTobaccoExtension(obj);
                    eObj.Sequence = obj.Sequence != 0 ? eObj.Sequence : item.Sequence;
                    eObj.CustCylID = item.CusCylinderID;
                    if (obj.EngravingID == 0)
                        eObj.Color = item.Color;
                    eObj.HexagonalName = HexagonalID == 0 ? string.Empty : hexaColl.Where(x => x.ReferencesID == HexagonalID).Select(x => x.Name).FirstOrDefault();
                    eObj.CellShapeName = CellShapeID == 0 ? string.Empty : cellShapeColl.Where(x => x.ReferencesID == CellShapeID).Select(x => x.Name).FirstOrDefault();
                    eObj.GraditionName = GradationID == 0 ? string.Empty : gradationColl.Where(x => x.ReferencesID == GradationID).Select(x => x.Name).FirstOrDefault();
                    list.Add(eObj);
                }
            }
            //Fill dữ liệu từ cylinder copy
            foreach (TblEngravingTobacco item in tobColl)
            {
                if (item.Sequence > 10)
                {
                    int HexagonalID = 0, CellShapeID = 0, GradationID = 0;
                    HexagonalID = item.Hexagonal != null ? (int)item.Hexagonal : 0;
                    CellShapeID = item.CellShape != null ? (int)item.CellShape : 0;
                    GradationID = item.Gradation != null ? (int)item.Gradation : 0;
                    EngravingTobaccoExtension eObj = new EngravingTobaccoExtension(item);
                    eObj.HexagonalName = HexagonalID == 0 ? string.Empty : hexaColl.Where(x => x.ReferencesID == HexagonalID).Select(x => x.Name).FirstOrDefault();
                    eObj.CellShapeName = CellShapeID == 0 ? string.Empty : cellShapeColl.Where(x => x.ReferencesID == CellShapeID).Select(x => x.Name).FirstOrDefault();
                    eObj.GraditionName = GradationID == 0 ? string.Empty : gradationColl.Where(x => x.ReferencesID == GradationID).Select(x => x.Name).FirstOrDefault();
                    list.Add(eObj);
                }
            }
            return list;
        }

        public static TblEngravingTobacco SelectTobaccoByID(int EngravingID)
        {
            return new Select().From(TblEngravingTobacco.Schema)
                                .Where(TblEngravingTobacco.EngravingIDColumn).IsEqualTo(EngravingID)
                                .ExecuteSingle<TblEngravingTobacco>();
        }

        public static TblEngravingTobacco TobaccoInsert(TblEngravingTobacco obj)
        {
            return new TblEngravingTobaccoController().Insert(obj);
        }

        public static TblEngravingTobacco TobaccoUpdate(TblEngravingTobacco obj)
        {
            return new TblEngravingTobaccoController().Update(obj);
        }

        public static bool TobaccoDelete(int EngravingID)
        {
            bool yesno = false;
            try
            {
                new Delete().From(TblEngravingTobacco.Schema).Where(TblEngravingTobacco.EngravingIDColumn).IsEqualTo(EngravingID).Execute();
                yesno = true;
            }
            catch
            {
                yesno = false;
            }
            return yesno;
        }
        #endregion


        #region Etching
        public static List<int> GetListEngravingIdEtchingByJobID(int jobID)
        {
            return new Select(TblEngravingEtching.EngravingIDColumn).From(TblEngravingEtching.Schema).Where(TblEngravingEtching.JobIDColumn).IsEqualTo(jobID).ExecuteTypedList<int>();
        }

        public static DataTable EtchingSelectAll(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblEngravingEtchingSelectAll(JobID).GetReader());
            return dt;
        }

        public static DataTable EtchingSelectAllForPrint(int JobID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblEngravingEtchingSelectAllForPrint(JobID).GetReader());
            return dt;
        }

        public static TblEngravingEtching SelectEtchingByID(int EngravingID)
        {
            return new Select().From(TblEngravingEtching.Schema)
                                .Where(TblEngravingEtching.EngravingIDColumn).IsEqualTo(EngravingID)
                                .ExecuteSingle<TblEngravingEtching>();
        }

        public static TblEngravingEtching EtchingInsert(TblEngravingEtching obj)
        {
            return new TblEngravingEtchingController().Insert(obj);
        }

        public static TblEngravingEtching EtchingUpdate(TblEngravingEtching obj)
        {
            return new TblEngravingEtchingController().Update(obj);
        }

        public static bool EtchingDelete(int EngravingID)
        {
            bool yesno = false;
            try
            {
                new Delete().From(TblEngravingEtching.Schema).Where(TblEngravingEtching.EngravingIDColumn).IsEqualTo(EngravingID).Execute();
                yesno = true;
            }
            catch
            {
                yesno = false;
            }
            return yesno;
        }
        #endregion
    }
}
