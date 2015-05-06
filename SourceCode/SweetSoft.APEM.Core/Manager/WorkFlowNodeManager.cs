using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.Core.Manager
{
    public class WorkFlowNodeManager
    {
        /// <summary>
        /// Tìm workflownode theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TblWorkFlowNode GetWorkFlowNodeByID(object id)
        {
            return new Select().From(TblWorkFlowNode.Schema).Where(TblWorkFlowNode.IdColumn).IsEqualTo(id)
                               .ExecuteSingle<TblWorkFlowNode>();
        }

        public static TblWorkFlowNode GetExtractWorkFlowNodeByWorkflow(int id, long WorkFlowID)
        {
            return new Select().From(TblWorkFlowNode.Schema)
                .Where(TblWorkFlowNode.WorkFlowIDInXMLColumn).IsEqualTo(id)
                .And(TblWorkFlowNode.WorkFlowIDColumn).IsEqualTo(WorkFlowID)
                .ExecuteSingle<TblWorkFlowNode>();
        }

        public static TblWorkFlowNode Insert(TblWorkFlowNode workflow)
        {
            return new TblWorkFlowNodeController().Insert(workflow.DepartmentID, workflow.WorkTaskID,
                workflow.WorkFlowID, workflow.NodeType, workflow.WorkFlowListFromConnection,
                workflow.WorkFlowListToConnection, workflow.WorkFlowIsSend, workflow.WorkFlowIsRoot,
                workflow.WorkFlowIsConnectItseft, workflow.WorkFlowDataGraphID, workflow.WorkFlowCode,
                workflow.MachineryProduceTypeID, workflow.WorkFlowIDInXML, workflow.Title,
                workflow.UpdatePropertiesValues, workflow.CreatedBy, workflow.CreatedOn,
                workflow.ModifiedBy, workflow.ModifiedOn);
        }

        public static TblWorkFlowNode Update(TblWorkFlowNode workflow)
        {
            return new TblWorkFlowNodeController().Update(workflow.Id, workflow.DepartmentID, workflow.WorkTaskID,
                workflow.WorkFlowID, workflow.NodeType, workflow.WorkFlowListFromConnection,
                workflow.WorkFlowListToConnection, workflow.WorkFlowIsSend, workflow.WorkFlowIsRoot,
                workflow.WorkFlowIsConnectItseft, workflow.WorkFlowDataGraphID, workflow.WorkFlowCode,
                workflow.MachineryProduceTypeID, workflow.WorkFlowIDInXML, workflow.Title,
                workflow.UpdatePropertiesValues, workflow.CreatedBy, workflow.CreatedOn,
                workflow.ModifiedBy, workflow.ModifiedOn);
        }

        public static bool Delete(TblWorkFlowNode node)
        {
            bool canDelete = CheckCanDelete(node.Id);
            if (canDelete == false)
                return false;

            //xóa các kết nối (đường chuyển)
            //delete from line connect           
            bool isDeleteSuccess = WorkFlowLineManager.DeleteallLineMapWithNode(node.Id);
            if (isDeleteSuccess)
            {
                //xóa ghi nhận thông số
                //delete from nodework properties map with worktask
                //DeleteNodeworkPropertiesMap(node.Id);

                //xóa công việc liên quan đối với thành phần phụ
                //delete from worktask map with subwork         
                //DeleteWorktaskInNodeMap(node.Id);

                //xóa node
                //delete node
                return new TblWorkFlowNodeController().Delete(node.Id);
            }
            return false;
        }

        public static bool CheckCanDelete(long nodeId)
        {
            /*
            TblproductionCollection allRefrence = new SubSonic.Select(Tblproduction.IdColumn)
                                .From(Tblproduction.Schema).WhereExpression(Tblproduction.IdNodeColumn.ColumnName).IsEqualTo(nodeId)
                                .Or(Tblproduction.IdNodeNextColumn.ColumnName).IsEqualTo(nodeId).CloseExpression()
                                .ExecuteAsCollection<TblproductionCollection>();
            if (allRefrence != null && allRefrence.Count > 0)
                return false;

            TblproductionprocessingCollection allRefrence2 = new SubSonic.Select(Tblproductionprocessing.IdColumn)
                                .From(Tblproductionprocessing.Schema).Where(Tblproductionprocessing.WorktasknodeColumn).IsEqualTo(nodeId)
                                .ExecuteAsCollection<TblproductionprocessingCollection>();
            if (allRefrence2 != null && allRefrence2.Count > 0)
                return false;
            */
            return true;
        }

        public static bool DeletefromDesign(int idInXML, long WorkFlowID)
        {
            TblWorkFlowNode nodeToDelete = GetExtractWorkFlowNodeByIdInXML(idInXML, WorkFlowID);
            if (nodeToDelete != null)
            {
                return Delete(nodeToDelete);
            }
            return false;
        }

        public static void UpdateConnection(int idNode, string lstFromCollection, string lstToCollection)
        {
            new Update(TblWorkFlowNode.Schema.TableName)
                 .Set(TblWorkFlowNode.Columns.WorkFlowListToConnection).EqualTo(lstToCollection)
                 .Set(TblWorkFlowNode.Columns.WorkFlowListFromConnection).EqualTo(lstFromCollection)
                 .Where(TblWorkFlowNode.Columns.Id).IsEqualTo(idNode)
                 .Execute();
        }

        #region for processing

        /// <summary>
        /// Get extract workflow node by [id in xml , code and IdMachineryProducetype]
        /// </summary>
        /// <param name="idInXML">id in xml</param>
        /// <param name="code">design code,eg: workas20_78-12_75</param>
        /// <param name="IdMachineryProducetype">Id Machinery Produce type</param>
        /// <returns>TblWorkFlowNode object</returns>
        public static TblWorkFlowNode GetExtractWorkFlowNodeByIdInXML(int idInXML, long WorkFlowID)
        {
            return new Select().From(TblWorkFlowNode.Schema)
                .Where(TblWorkFlowNode.WorkFlowIDInXMLColumn).IsEqualTo(idInXML)
                .And(TblWorkFlowNode.WorkFlowIDColumn).IsEqualTo(WorkFlowID)
                .ExecuteSingle<TblWorkFlowNode>();
        }

        /// <summary>
        /// Get extract workflow node by [id dept , code and IdMachineryProducetype]
        /// </summary>
        /// <param name="idInXML">id in xml</param>
        /// <param name="code">design code,eg: workas20_78-12_75</param>
        /// <param name="IdMachineryProducetype">Id Machinery Produce type</param>
        /// <returns>TblWorkFlowNodeCollection object</returns>
        public static TblWorkFlowNodeCollection GetExtractWorkFlowNodeByDepartmentID(int DepartmentID, long WorkFlowID)
        {
            return new Select().From(TblWorkFlowNode.Schema)
                .Where(TblWorkFlowNode.DepartmentIDColumn).IsEqualTo(DepartmentID)
                .And(TblWorkFlowNode.MachineryProduceTypeIDColumn).IsEqualTo(WorkFlowID)
                .ExecuteAsCollection<TblWorkFlowNodeCollection>();
        }

        /// <summary>
        /// Get all first work by code and idMachinery
        /// </summary>
        /// <param name="code">design code,eg: workas20_78-12_75</param>
        /// <param name="IdMachineryProducetype">Id Machinery Produce type</param>
        /// <returns>TblWorkFlowNodeCollection object</returns>
        public static TblWorkFlowNodeCollection GetAllNodebyIdMachineryAndCode(object WorkFlowID)
        {
            SqlQuery query = new Select().From(TblWorkFlowNode.Schema)
                .And(TblWorkFlowNode.WorkFlowIDColumn).IsEqualTo(WorkFlowID);
            return query.ExecuteAsCollection<TblWorkFlowNodeCollection>();
        }

        public static TblWorkFlowNodeCollection GetAllWorkFlowNodeByWorkflow(long WorkFlowID)
        {
            return new Select().From(TblWorkFlowNode.Schema)
                 .Where(TblWorkFlowNode.Columns.WorkFlowID).IsEqualTo(WorkFlowID)
                .ExecuteAsCollection<TblWorkFlowNodeCollection>();
        }

        /// <summary>
        /// Get Collection of TblWorkFlowNode from TblWorkFlowLine
        /// </summary>
        /// <param name="WorkFlowIDnode">id of workflownode</param>
        /// <returns>TblWorkFlowNodeCollection object</returns>
        public static TblWorkFlowNodeCollection GetAllStartWorkflownode(long WorkFlowIDnode)
        {
            return GetAllStartWorkflownode(WorkFlowIDnode, string.Empty);
        }
        /// <summary>
        /// Ducnm: Lấy line theo loại đơn hàng chính hoặc phụ
        /// </summary>
        /// <param name="WorkFlowIDnode"></param>
        /// <param name="lineType"></param>
        /// <returns></returns>
        public static TblWorkFlowNodeCollection GetAllStartWorkflownode(long WorkFlowIDnode, string lineType)
        {
            Select select = new Select(TblWorkFlowLine.Node2IDColumn);
            select.From(TblWorkFlowLine.Schema)
                    .Where(TblWorkFlowLine.Columns.Node1ID).IsEqualTo(WorkFlowIDnode);
            if (lineType.Trim().Equals("DuongChuyenPhu"))
                select.And(TblWorkFlowLine.Columns.LineType).In("DuongChuyenPhu", "CaHai");
            else
                select.AndExpression(TblWorkFlowLine.Columns.LineType).IsEqualTo("DuongChuyenChinh")
                    .Or(TblWorkFlowLine.LineTypeColumn).IsEqualTo("CaHai")
                    .Or(TblWorkFlowLine.LineTypeColumn).IsNull().CloseExpression();
            SqlQuery query = new Select().From(TblWorkFlowNode.Schema)
              .Where(TblWorkFlowNode.Columns.Id).In(select);

            return query.ExecuteAsCollection<TblWorkFlowNodeCollection>();
        }



        /// <summary>
        /// Get Collection of TblWorkFlowNode by special ids
        /// </summary>
        /// <param name="lstIds">List of id</param>
        /// <returns>TblWorkFlowNodeCollection object</returns>
        public static TblWorkFlowNodeCollection GetAllworkflownodebyIds(List<int> lstIds)
        {
            SqlQuery query = new Select().From(TblWorkFlowNode.Schema)
                .Where(TblWorkFlowNode.IdColumn).In(lstIds);
            return query.ExecuteAsCollection<TblWorkFlowNodeCollection>();
        }

        /// <summary>
        /// Get next work of special workflownode
        /// </summary>
        /// <param name="workflownode">TblWorkFlowNode object</param>
        /// <returns>List of TblWorkFlowNode</returns>
        public static List<TblWorkFlowNode> GetNextwork(TblWorkFlowNode workflownode)
        {
            return GetNextwork(workflownode, string.Empty);
        }

        public static List<TblWorkFlowNode> GetNextwork(TblWorkFlowNode workflownode, string orderType)
        {
            if (workflownode != null)
            {
                TblWorkFlowNodeCollection allNextWork = GetAllStartWorkflownode(workflownode.Id, orderType);
                if (allNextWork != null && allNextWork.Count > 0)
                    return allNextWork.ToList();
            }
            return new List<TblWorkFlowNode>();
        }


        /// <summary>
        /// Get next work of special workflownode by id
        /// </summary>
        /// <param name="WorkFlowIDnode">id of workflownode</param>
        /// <returns>List of TblWorkFlowNode</returns>
        public static List<TblWorkFlowNode> GetNextwork(long WorkFlowIDnode)
        {
            return GetNextwork(WorkFlowIDnode, string.Empty);
        }

        public static List<TblWorkFlowNode> GetNextwork(long WorkFlowIDnode, string orderType)
        {
            TblWorkFlowNode node = WorkFlowNodeManager.GetWorkFlowNodeByID(WorkFlowIDnode);
            return node != null ? GetNextwork(node, orderType) : new List<TblWorkFlowNode>();
        }

        /*
        /// <summary>
        /// Get next work by list to connection of format : 10,20,34..
        /// </summary>
        /// <param name="WorkFlowListToConnection">string of format like 10,20,34</param>
        /// <returns>List of TblWorkFlowNode</returns>
        public static List<TblWorkFlowNode> GetNextwork(string WorkFlowListToConnection)
        {
            try
            {
                List<int> lst = WorkFlowListToConnection.Split(',').Select(x => int.Parse(x)).ToList();
                if (lst != null && lst.Count > 0)
                {
                    TblWorkFlowNodeCollection mapped = GetAllworkflownodebyIds(lst);
                    if (mapped != null && mapped.Count > 0)
                        return mapped.ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        */

        /// <summary>
        /// Get all next work by Id Machinery Produce type (and optional: id TblWorkFlowNode)
        /// </summary>
        /// <param name="IdMachineryProducetype">Id Machinery Produce type</param>
        /// <param name="code">Code of TblWorkFlow</param>
        /// <param name="idTblWorkFlowNode">id of TblWorkFlowNode</param>
        /// <returns>List of TblWorkFlowNode</returns>
        public static List<TblWorkFlowNode> GetNextWorkByWorkflowCodeAndidMachineryProducetype(long WorkFlowID, int? idTblWorkFlowNode)
        {
            List<TblWorkFlowNode> lstReturn = new List<TblWorkFlowNode>();

            TblWorkFlowNodeCollection allNode = GetAllNodebyIdMachineryAndCode(WorkFlowID);
            if (allNode != null && allNode.Count > 0)
            {
                TblWorkFlowNode nodeGet = null;
                if (idTblWorkFlowNode.HasValue)
                {
                    TblWorkFlowNode node = WorkFlowNodeManager.GetWorkFlowNodeByID(idTblWorkFlowNode.Value);
                    if (node != null)
                        nodeGet = node;
                }
                else
                {
                    List<TblWorkFlowNode> firstAndLast = allNode.Where(x => (x.WorkFlowIsRoot.HasValue && x.WorkFlowIsRoot.Value == true)).ToList();
                    if (firstAndLast.Count > 0)
                    {
                        int indx = -1;

                        #region find start

                        if (firstAndLast.Count == 2)
                        {
                            if (firstAndLast[0].WorkFlowIsSend.HasValue && firstAndLast[0].WorkFlowIsSend.Value == true)
                            {
                                if (firstAndLast[1].DepartmentID > 0)
                                    indx = 1;
                            }
                            else
                            {
                                if (firstAndLast[0].DepartmentID > 0)
                                    indx = 0;
                            }
                        }
                        else
                        {
                            if (firstAndLast[0].WorkFlowIsSend.HasValue && firstAndLast[0].WorkFlowIsSend.Value == true)
                            {
                            }
                            else
                            {
                                if (firstAndLast[0].DepartmentID > 0)
                                    indx = 0;
                            }
                        }

                        #endregion

                        if (indx != -1)
                            nodeGet = firstAndLast[indx];
                    }
                }

                if (nodeGet != null)
                {
                    List<TblWorkFlowNode> mapped = GetNextwork(nodeGet);
                    if (mapped != null && mapped.Count > 0)
                        lstReturn.AddRange(mapped);
                }
            }

            return lstReturn;
        }

        /// <summary>
        /// Get next work by special workflow id 
        /// and [optional: if  workflownode id hasvalue, will return next work of that workflownode.
        /// </summary>
        /// <param name="WorkflowId">id of workflow</param>
        /// <param name="TblWorkFlowNodeId">id of workflownode</param>
        /// <returns></returns>
        public static List<TblWorkFlowNode> GetNextWorkByWorkflowIdAndTblWorkFlowNodeId(long WorkflowId, int? TblWorkFlowNodeId)
        {
            TblWorkFlow wf = WorkFlowManager.GetWorkFlowByID(WorkflowId);
            if (wf != null && wf.MachineryProduceTypeID>0)
                return GetNextWorkByWorkflowCodeAndidMachineryProducetype(wf.Id, TblWorkFlowNodeId);
            return null;
        }

        public static List<TblWorkFlowNode> GetFirstDeptCollection(object WorkFlowID)
        {
            return GetFirstCollection(WorkFlowID);
        }
        /// <summary>
        /// Code lấy công việc bắt đầu theo line
        /// </summary>
        /// <param name="nodeStart"></param>
        /// <param name="nodeEnd"></param>
        /// <returns></returns>
        public static List<TblWorkFlowNode> GetFirstWorkCollection(object nodeStart, object nodeEnd, int? idMachinery)
        {
            List<TblWorkFlowNode> lstReturn = new List<TblWorkFlowNode>();

            TblWorkFlow workflow = new Select().From(TblWorkFlow.Schema).Where(TblWorkFlow.IdColumn).In(
                    new Select(TblWorkFlowLine.Columns.WorkFlowToID).From(TblWorkFlowLine.Schema).Where(TblWorkFlowLine.Node1IDColumn).IsEqualTo(nodeStart)
                                                                    .And(TblWorkFlowLine.Node2IDColumn).IsEqualTo(nodeEnd)
                ).ExecuteSingle<TblWorkFlow>();
            if (idMachinery.HasValue && workflow != null && workflow.MachineryProduceTypeID != idMachinery)
            {
                workflow = new Select().From(TblWorkFlow.Schema).Where(TblWorkFlow.CodeColumn).IsEqualTo(workflow.Code)
                                                                .And(TblWorkFlow.MachineryProduceTypeIDColumn).IsEqualTo(idMachinery.Value)
                                                                .ExecuteSingle<TblWorkFlow>();
            }
            if (workflow != null)
            {
                TblWorkFlowNodeCollection allNodeInConnect = new Select().From(TblWorkFlowNode.Schema).Where(TblWorkFlowNode.WorkFlowIDColumn).IsEqualTo(workflow.Id).ExecuteAsCollection<TblWorkFlowNodeCollection>();
                if (allNodeInConnect != null && allNodeInConnect.Count > 0)
                {
                    List<TblWorkFlowNode> firstAndLast = allNodeInConnect.Where(x => (x.WorkFlowIsRoot.HasValue && x.WorkFlowIsRoot.Value == true)).ToList();
                    if (firstAndLast.Count > 0)
                    {
                        int indx = ExtractIndex(firstAndLast);

                        if (indx != -1)
                        {
                            List<TblWorkFlowNode> mapped = GetNextwork(firstAndLast[indx]);
                            if (mapped != null && mapped.Count > 0)
                                lstReturn.AddRange(mapped);
                        }
                    }
                }
            }
            return lstReturn;
        }

        public static List<TblWorkFlowNode> GetNextDeptCollection(long WorkFlowIDnode)
        {
            return GetNextDeptCollection(WorkFlowIDnode, string.Empty);
        }
        /// <summary>
        /// Ducnm:
        /// </summary>
        /// <param name="WorkFlowIDnode"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public static List<TblWorkFlowNode> GetNextDeptCollection(long WorkFlowIDnode, string orderType)
        {
            return GetNextwork(WorkFlowIDnode, orderType);
        }

        static int ExtractIndex(List<TblWorkFlowNode> firstAndLast)
        {
            int indx = -1;

            #region extract id

            if (firstAndLast.Count == 2)
            {
                if (firstAndLast[0].WorkFlowIsSend.HasValue && firstAndLast[0].WorkFlowIsSend.Value == false)
                {
                    indx = 0;
                }
                else if (firstAndLast[1].WorkFlowIsSend.HasValue && firstAndLast[1].WorkFlowIsSend.Value == false)
                {
                    indx = 1;
                }
            }
            else if (firstAndLast[0].WorkFlowIsSend.HasValue && firstAndLast[0].WorkFlowIsSend.Value == false)
            {
                indx = 0;
            }

            #endregion

            return indx;
        }

        public static List<TblWorkFlowNode> GetFirstCollection(object WorkFlowID)
        {
            List<TblWorkFlowNode> lstReturn = new List<TblWorkFlowNode>();
            TblWorkFlowNodeCollection allNode = GetAllNodebyIdMachineryAndCode(WorkFlowID);
            if (allNode != null && allNode.Count > 0)
            {
                List<TblWorkFlowNode> firstAndLast = allNode.Where(x => (x.WorkFlowIsRoot.HasValue && x.WorkFlowIsRoot.Value == true)).ToList();
                if (firstAndLast.Count > 0)
                {
                    int indx = ExtractIndex(firstAndLast);

                    if (indx != -1)
                    {
                        List<TblWorkFlowNode> mapped = GetNextwork(firstAndLast[indx]);
                        if (mapped != null && mapped.Count > 0)
                            lstReturn.AddRange(mapped);
                    }
                }

            }
            return lstReturn;
        }

        #endregion

        #region Tblworktask in node

        public static TblWorkTaskInNode Insertworktaskinnode(TblWorkTaskInNode subworkNode)
        {
            return new TblWorkTaskInNodeController().Insert(subworkNode.WorkTaskID, subworkNode.NodeID);
        }

        public static TblWorkTaskInNode Updateworktaskinnode(TblWorkTaskInNode subworkNode)
        {
            return new TblWorkTaskInNodeController().Update(subworkNode.Id, subworkNode.WorkTaskID, subworkNode.NodeID);
        }

        public static bool Deleteworktaskinnode(object id)
        {
            return new TblWorkTaskInNodeController().Delete(id);
        }

        public static void DeleteworktaskinnodeByIdSubwork(object idNode)
        {
            new Delete().From(TblWorkTaskInNode.Schema)
                  .Where(TblWorkTaskInNode.Columns.NodeID).IsEqualTo(idNode)
                  .Execute();
        }

        public static TblWorkTaskInNodeCollection GetWorktaskBySubwork(int idNode)
        {
            return new Select().From(TblWorkTaskInNode.Schema)
                .Where(TblWorkTaskInNode.NodeIDColumn).IsEqualTo(idNode)
                .ExecuteAsCollection<TblWorkTaskInNodeCollection>();
        }

        #endregion

        #region bulk reset
        /*
        public static void ResetDesign(string workflowCode, long WorkFlowID)
        {
            if (workflowCode.ToLower() == "main-work")
            {
                WorkFlowManager.ResetWorkflow(workflowCode, WorkFlowID);
            }
            else
            {
                //delete from nodework properties map with worktask
                new Delete().From(Tblnodeworkparameter.Schema)
                 .Where(Tblnodeworkparameter.Columns.Idnode).In(
                 new Select(TblWorkFlowNode.IdColumn).From(TblWorkFlowNode.Schema)
                       .Where(TblWorkFlowNode.Columns.Workflowcode).IsEqualTo(workflowCode)
                       .And(TblWorkFlowNode.Columns.WorkFlowID).IsEqualTo(WorkFlowID))
                    //.AndExpression(TblWorkFlowNode.Columns.WorkFlowIsRoot).IsEqualTo(false)
                    //.Or(TblWorkFlowNode.Columns.WorkFlowIsRoot).IsNull().CloseExpression())
                        .Execute();

                //delete from worktask map with subwork
                new Delete().From(TblWorkTaskInNode.Schema)
                .Where(TblWorkTaskInNode.Columns.Idnode).In(
                new Select(TblWorkFlowNode.IdColumn).From(TblWorkFlowNode.Schema)
                      .Where(TblWorkFlowNode.Columns.Workflowcode).IsEqualTo(workflowCode)
                      .And(TblWorkFlowNode.Columns.WorkFlowID).IsEqualTo(WorkFlowID)
                      .And(TblWorkFlowNode.NodetypeColumn).IsEqualTo(WorkflowNodeType.CongViecPhu.ToString()))
                      .Execute();

                //delete from line connect
                new Delete().From(TblWorkFlowLine.Schema)
                      .Where(TblWorkFlowLine.Columns.Workflowcode).IsEqualTo(workflowCode)
                      .And(TblWorkFlowLine.Columns.WorkFlowID).IsEqualTo(WorkFlowID)
                      .Execute();

                //delete node
                new Delete().From(TblWorkFlowNode.Schema)
                        .Where(TblWorkFlowNode.Columns.Workflowcode).IsEqualTo(workflowCode)
                        .And(TblWorkFlowNode.Columns.WorkFlowID).IsEqualTo(WorkFlowID)
                    //.AndExpression(TblWorkFlowNode.Columns.WorkFlowIsRoot).IsEqualTo(false)
                    //.Or(TblWorkFlowNode.Columns.WorkFlowIsRoot).IsNull().CloseExpression()
                        .Execute();
            }
        }
        */
        #endregion

        #region Code by Ducnm

        public static TblWorkFlowNodeCollection GetNextNodeByNodeStartID(object nodeId)
        {
            return new Select().From(TblWorkFlowNode.Schema).Where(TblWorkFlowNode.IdColumn).In(
                   new Select(TblWorkFlowLine.Columns.Node2ID).From(TblWorkFlowLine.Schema).Where(TblWorkFlowLine.Node1IDColumn).IsEqualTo(nodeId))
                               .ExecuteAsCollection<TblWorkFlowNodeCollection>();
        }

        /// <summary>
        /// Lấy workflow cho ghi nhận sx (created by ducnm 19.08)
        /// </summary>
        /// <param name="code">Mã workflow</param>
        /// <param name="idMachineryType">ID lọai trục</param>
        /// <returns></returns>
        private static TblWorkFlow GetWorkFlowByIdMachineryType(string wfCode, int idMachineryType)
        {
            Select select = new Select();
            select.From(TblWorkFlow.Schema).Where(TblWorkFlow.CodeColumn).IsEqualTo(wfCode)
                  .And(TblWorkFlow.MachineryProduceTypeIDColumn).IsEqualTo(idMachineryType);
            return select.ExecuteSingle<TblWorkFlow>();
        }     

        /// <summary>
        /// Lấy các node trong workflow
        /// </summary>
        /// <param name="wfCode">workflow code</param>
        /// <param name="idMachineryType">id machineryType</param>
        /// <param name="nodeType">Loại node từ WorkflowNodeType</param>
        /// <returns></returns>
        public static TblWorkFlowNodeCollection GetWorkflowNodes(string wfCode, int idMachineryType, WorkflowNodeType nodeType)
        {
            Select select = new Select();
            select.From(TblWorkFlowNode.Schema).Where(TblWorkFlowNode.WorkFlowCodeColumn).IsEqualTo(wfCode)
                  .And(TblWorkFlowNode.MachineryProduceTypeIDColumn).IsEqualTo(idMachineryType);
            if (nodeType != null)
                select.And(TblWorkFlowNode.NodeTypeColumn).IsEqualTo(nodeType.ToString());
            return select.ExecuteAsCollection<TblWorkFlowNodeCollection>();
        }
        /// <summary>
        /// Lấy các node công việc khác trong cùng xưởng với node đưa vào
        /// </summary>
        /// <param name="currentNodeId">id node công việc đưa vào</param>
        /// <param name="workflowId">id workflow</param>
        /// <returns></returns>
        public static List<TblWorkFlowNode> GetWorkNodeInsideDeptByWorkNode(object currentNodeId, object workflowId)
        {
            Select select = new Select();
            select.From(TblWorkFlowNode.Schema).Where(TblWorkFlowNode.WorkFlowIDColumn).IsEqualTo(workflowId)
                  .And(TblWorkFlowNode.NodeTypeColumn).IsNotEqualTo("Xuong");
            if (currentNodeId != null)
                select.And(TblWorkFlowNode.IdColumn).IsNotEqualTo(currentNodeId);
            return select.ExecuteTypedList<TblWorkFlowNode>();
        }

        #endregion

    }
}

