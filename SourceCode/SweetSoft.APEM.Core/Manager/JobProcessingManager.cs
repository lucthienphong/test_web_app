using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SweetSoft.APEM.Core.Manager
{
    /// <summary>
    /// Summary description for JobProcessingManager
    /// </summary>
    public class JobProcessingManager
    {
        public JobProcessingManager()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public static TblJobProcessing Insert(TblJobProcessing jp)
        {
            return new TblJobProcessingController().Insert(jp.JobID, jp.Description
               , jp.CreatedBy, jp.CreatedOn, jp.FinishedBy, jp.FinishedOn
               , jp.DepartmentID, jp.JobStatus);
        }

        public static TblJobProcessing Update(TblJobProcessing jp)
        {
            return new TblJobProcessingController().Update(jp.Id, jp.JobID, jp.Description
               , jp.CreatedBy, jp.CreatedOn, jp.FinishedBy, jp.FinishedOn
               , jp.DepartmentID, jp.JobStatus);
        }

        public static string GetDataSelectNextDepartment(int idNodeStart)
        {
            List<TblWorkFlowNode> lstNext = null;
            if (idNodeStart == 0)
            {
                TblWorkFlowCollection wfColl = WorkFlowManager.GetFirstWorkFlow(false);
                if (wfColl != null)
                {
                    lstNext = WorkFlowNodeManager.GetFirstCollection(wfColl[0].Id);
                }
            }
            else
            {
                TblWorkFlowNode node = new SubSonic.Select().From(TblWorkFlowNode.Schema)
                    .Where(TblWorkFlowNode.DepartmentIDColumn).IsEqualTo(idNodeStart)
                    .ExecuteSingle<TblWorkFlowNode>();
                if (node != null)
                {
                    TblWorkFlowNodeCollection lst = WorkFlowNodeManager.GetNextNodeByNodeStartID(node.Id);
                    if (lst != null)
                        lstNext = lst.ToList();
                }
            }
            if (lstNext != null && lstNext.Count > 0)
            {
                if (lstNext.Count > 1)
                {
                    StringBuilder sb = new StringBuilder("[");
                    foreach (TblWorkFlowNode item in lstNext)
                    {
                        if (item.TblDepartment != null)
                        {
                            sb.Append("{\"text\":\"" + item.TblDepartment.DepartmentName +
                                "\",\"id\":\"" + SecurityHelper.Encrypt(item.TblDepartment.DepartmentID.ToString()) + "\"},");
                        }
                    }

                    if (sb.Length > 1)
                        sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append("]");

                    return sb.ToString();
                }
                else
                {
                    if (lstNext[0].DepartmentID != 0)
                        return lstNext[0].DepartmentID.ToString();
                }
            }
            return null;
        }
    }

    public enum JobProcessingStatus
    {
        Inprogress,
        Finish,
        Waiting
    }
}