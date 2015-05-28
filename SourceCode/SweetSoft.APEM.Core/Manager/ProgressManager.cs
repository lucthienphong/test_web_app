using SubSonic;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class ProgressManager
    {
        /// <summary>
        /// Select Progress by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static TblProgress SelectByID(int ID)
        {
            return new Select().From(TblProgress.Schema).Where(TblProgress.JobIDColumn).IsEqualTo(ID).ExecuteSingle<TblProgress>();
        }

        public static TblProgress Insert(TblProgress obj)
        {
            return new TblProgressController().Insert(obj);
        }

        public static TblProgress Update(TblProgress obj)
        {
            return new TblProgressController().Update(obj);
        }

        /// <summary>
        /// Select all job for Progress Repro
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectProgressRepro(DateTime? OrderdateBegin, DateTime? OrderdateEnd, DateTime? ProofDateBegin, DateTime? ProofDateEnd, DateTime? ReproDateBegin, DateTime? ReproDateEnd, DateTime? CylinderDateBegin, DateTime? CylinderDateEnd, int ReproStatusID, int PageIndex, int PageSize, string SortColumn, string SortType, string JobNumber, string Customer)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblJobProgressForRepro(OrderdateBegin, OrderdateEnd, ProofDateBegin, ProofDateEnd, ReproDateBegin, ReproDateEnd, CylinderDateBegin, CylinderDateEnd, ReproStatusID, PageIndex, PageSize, SortColumn, SortType, JobNumber, Customer).GetReader());
            return dt;
        }

        /// <summary>
        /// Select all job for Progress Repro
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectProgresseEngraving(DateTime? DeliveryBegin, DateTime? DeliveryEnd, DateTime? EngravingBegin, DateTime? EngravingEnd, int ReproStatusID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblProductionScheduleEngraving(DeliveryBegin, DeliveryEnd, EngravingBegin, EngravingEnd, ReproStatusID).GetReader());
            return dt;
        }

        public static DataTable SelectProgresseEngraving(DateTime? DeliveryBegin, DateTime? DeliveryEnd, DateTime? EngravingBegin, DateTime? EngravingEnd, int ReproStatusID, int PageIndex, int PageSize, string SortColumn, string SortType, string JobNumber, string Customer)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblJobProgressForEngraving(DeliveryBegin, DeliveryEnd, EngravingBegin, EngravingEnd, ReproStatusID, PageIndex, PageSize, SortColumn, SortType, JobNumber, Customer).GetReader());
            return dt;
        }

        public static DataTable SelectProgresseEmbossing(DateTime? DeliveryBegin, DateTime? DeliveryEnd, DateTime? EngravingBegin, DateTime? EngravingEnd, int ReproStatusID)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblProductionScheduleEmbossing(DeliveryBegin, DeliveryEnd, EngravingBegin, EngravingEnd, ReproStatusID).GetReader());
            return dt;
        }

        /// <summary>
        /// Select all job for Progress DeReChrome
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectProgresseDeReChrome(DateTime? DeliveryBegin, DateTime? DeliveryEnd, DateTime? DeReBegin, DateTime? DeReEnd)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblProductionScheduleDeReChrome(DeliveryBegin, DeliveryEnd, DeReBegin, DeReEnd).GetReader());
            return dt;
        }

        public static DataTable SelectProgresseDeReChrome(DateTime? DeliveryBegin, DateTime? DeliveryEnd, DateTime? DeReBegin, DateTime? DeReEnd, DateTime? CylinderDateBegin, DateTime? CylinderDateEnd, int PageIndex, int PageSize, string SortColumn, string SortType, string JobNumber, string Customer)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblJobProgressForDeReChrome(DeliveryBegin, DeliveryEnd, DeReBegin, DeReEnd, CylinderDateBegin, CylinderDateEnd, PageIndex, PageSize, SortColumn, SortType, JobNumber, Customer).GetReader());
            return dt;
        }
    }
}
