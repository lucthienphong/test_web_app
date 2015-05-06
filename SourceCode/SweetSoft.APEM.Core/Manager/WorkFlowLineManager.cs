using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;

namespace SweetSoft.APEM.Core.Manager
{
    public class WorkFlowLineManager
    {
        /// <summary>
        /// Tìm workflowline theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TblWorkFlowLine GetWorkFlowNodeByID(object id)
        {
            return new Select().From(TblWorkFlowLine.Schema).Where(TblWorkFlowLine.IdColumn).IsEqualTo(id)
                               .ExecuteSingle<TblWorkFlowLine>();
        }

        public static TblWorkFlowLine GetExtractWorkFlowLineBetWeenNodes(int idNode1, int idNode2, string code, long idWorkflow)
        {
            return new Select().From(TblWorkFlowLine.Schema)
                .Where(TblWorkFlowLine.Node1IDColumn).IsEqualTo(idNode1)
                .And(TblWorkFlowLine.Node2IDColumn).IsEqualTo(idNode2)
                .And(TblWorkFlowLine.WorkFlowCodeColumn).IsEqualTo(code)
                .And(TblWorkFlowLine.WorkFlowIDColumn).IsEqualTo(idWorkflow)
                .ExecuteSingle<TblWorkFlowLine>();
        }

        public static TblWorkFlowLine GetExtractWorkFlowLineByIdInXML(string idInXML, long idWorkflow)
        {
            return new Select().From(TblWorkFlowLine.Schema)
                .Where(TblWorkFlowLine.WorkFlowIDInXMLColumn).IsEqualTo(idInXML)
                .And(TblWorkFlowLine.WorkFlowIDColumn).IsEqualTo(idWorkflow)
                .ExecuteSingle<TblWorkFlowLine>();
        }

        public static TblWorkFlowLine Insert(TblWorkFlowLine workflow)
        {
            return new TblWorkFlowLineController().Insert(workflow.Node1ID,workflow.Node2ID,workflow.WorkFlowID
                ,workflow.LineType,workflow.MachineryProduceTypeID,workflow.WorkFlowCode,workflow.WorkFlowIDInXML,
                workflow.WorkFlowID,workflow.CreatedBy,workflow.CreatedOn,workflow.ModifiedBy,
                workflow.ModifiedOn);
        }

        public static TblWorkFlowLine Update(TblWorkFlowLine workflow)
        {
            return new TblWorkFlowLineController().Update(workflow.Id, workflow.Node1ID, workflow.Node2ID, workflow.WorkFlowID
                , workflow.LineType, workflow.MachineryProduceTypeID, workflow.WorkFlowCode, workflow.WorkFlowIDInXML,
                workflow.WorkFlowID, workflow.CreatedBy, workflow.CreatedOn, workflow.ModifiedBy,
                workflow.ModifiedOn);
        }

        public static TblWorkFlowLineCollection GetAllWorkFlowLineByNode(int idNode)
        {
            return new Select().From(TblWorkFlowLine.Schema)
                 .WhereExpression(TblWorkFlowLine.Columns.Node1ID).IsEqualTo(idNode)
                 .Or(TblWorkFlowLine.Columns.Node2ID).IsEqualTo(idNode).CloseExpression()
                .ExecuteAsCollection<TblWorkFlowLineCollection>();
        }

        public static TblWorkFlowLineCollection GetAllWorkFlowLineByWorkflow(long idWorkflow)
        {
            return new Select().From(TblWorkFlowLine.Schema)
                 .Where(TblWorkFlowLine.Columns.WorkFlowID).IsEqualTo(idWorkflow)
                .ExecuteAsCollection<TblWorkFlowLineCollection>();
        }

        public static bool Delete(TblWorkFlowLine line)
        {
            if (line != null)
            {
                //xóa các node trong workflow mà đường chuyển này trỏ tới.            
                if (line.WorkFlowToID.HasValue && line.WorkFlowToID.Value > 0)
                    WorkFlowManager.Delete(line.WorkFlowToID.Value);

                return new TblWorkFlowLineController().Delete(line.Id);
            }
            return false;
        }

        public static void DeletefromDesign(int idInXML, long idWorkflow)
        {
            /*
            new Delete().From(TblWorkFlowLine.Schema)
                 .Where(TblWorkFlowLine.Columns.Workflowidinxml).IsEqualTo(idInXML)
                 .And(TblWorkFlowLine.Columns.Workflowcode).IsEqualTo(workflowCode)
                 .And(TblWorkFlowLine.Columns.WorkflowlineIdMachineryProducetype).IsEqualTo(workflowIdMachineryProduce)
                 .Execute();
            */
            TblWorkFlowLine line = GetExtractWorkFlowLineByIdInXML(idInXML.ToString(), idWorkflow);
            if (line != null)
                Delete(line);
        }

        public static bool DeleteallLineMapWithNode(int id)
        {
            try
            {
                TblWorkFlowLineCollection allLineMapWithNode = GetAllWorkFlowLineByNode(id);
                if (allLineMapWithNode != null && allLineMapWithNode.Count > 0)
                {
                    foreach (TblWorkFlowLine item in allLineMapWithNode)
                        Delete(item);
                }
                return true;
            }
            catch { }
            return false;
        }

        #region ducnm 03/10
        /// <summary>
        /// Lấy line từ 2 node bắt đầu
        /// </summary>
        /// <param name="idnode1"></param>
        /// <param name="idnode2"></param>
        /// <returns></returns>
        public static TblWorkFlowLine GetLineByTwoNode(object idnode1, object idnode2)
        {
            return new Select().From(TblWorkFlowLine.Schema).Where(TblWorkFlowLine.Node1IDColumn).IsEqualTo(idnode1)
                               .And(TblWorkFlowLine.Node2IDColumn).IsEqualTo(idnode2)
                               .ExecuteSingle<TblWorkFlowLine>();
        }
        #endregion
    }
}
