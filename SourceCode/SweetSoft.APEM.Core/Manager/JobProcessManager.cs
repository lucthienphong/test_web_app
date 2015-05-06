using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetSoft.APEM.Core.Manager
{
    /// <summary>
    /// Summary description for JobProcessingManager
    /// </summary>
    public class JobProcessManager
    {
        public JobProcessManager()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public static TblJobProcess Insert(TblJobProcess jp)
        {
            return new TblJobProcessController().Insert(jp.JobID, jp.StartedBy, jp.StartedOn,
                jp.FinishedBy, jp.FinishedOn);
        }

        public static TblJobProcess Update(TblJobProcess jp)
        {
            return new TblJobProcessController().Update(jp.JobID, jp.StartedBy, jp.StartedOn,
                jp.FinishedBy, jp.FinishedOn);
        }

        public static bool Delete(object JobID)
        {
            //Delete TblJobProcess
            return new TblJobProcessController().Delete(JobID);
        }
    }

}