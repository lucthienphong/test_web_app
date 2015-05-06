using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;

namespace SweetSoft.APEM.Core.Manager
{
    /// <summary>
    /// Summary description for CylinderProcessingManager
    /// </summary>
    public class CylinderProcessingManager
    {
        public CylinderProcessingManager()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public static TblCylinderProcessing Insert(TblCylinderProcessing cp)
        {
            return new TblCylinderProcessingController().Insert(cp.CylinderID, cp.Description
               , cp.CreatedBy, cp.CreatedOn, cp.FinishedBy, cp.FinishedOn
               , cp.DepartmentID, cp.CylinderStatus, cp.MachineID ?? (int?)null);
        }

        public static TblCylinderProcessing Update(TblCylinderProcessing cp)
        {
            return new TblCylinderProcessingController().Update(cp.Id, cp.CylinderID, cp.Description
               , cp.CreatedBy, cp.CreatedOn, cp.FinishedBy, cp.FinishedOn
               , cp.DepartmentID, cp.CylinderStatus, cp.MachineID ?? (int?)null);
        }

        public static TblCylinderProcessingCollection GetAllRecordForCylinder(int cylinderId)
        {
            return new SubSonic.Select().From(TblCylinderProcessing.Schema)
                .Where(TblCylinderProcessing.CylinderIDColumn).IsEqualTo(cylinderId)
                .ExecuteAsCollection<TblCylinderProcessingCollection>();
        }

        public static List<TblDepartment> GetDataSelectNextDepartment(int idNodeStart, out bool isEnd)
        {
            List<TblDepartment> lst = new List<TblDepartment>();
            List<TblWorkFlowNode> lstNext = GetNextProcess(idNodeStart, out isEnd);
            if (lstNext != null && lstNext.Count > 0)
            {
                if (lstNext.Count > 1)
                {
                    foreach (TblWorkFlowNode item in lstNext)
                    {
                        if (item.TblDepartment != null)
                            lst.Add(item.TblDepartment);
                    }
                }
                else if (lstNext[0].TblDepartment != null)
                    lst.Add(lstNext[0].TblDepartment);
                else if (lstNext[0].NodeType == WorkflowNodeType.DauVaoHoacDauRa.ToString()
                    && lstNext[0].WorkFlowIsSend.HasValue && lstNext[0].WorkFlowIsSend.Value == true)
                    isEnd = true;
            }
            return lst;
        }

        /*
        public static NodeTree GetCylinderProcess(int cylinderId)
        {
            if (cylinderId > 0)
            {
                TblCylinder cylinder = CylinderManager.SelectByID(cylinderId);
                if (cylinder != null && cylinder.ProcessTypeID.HasValue)
                {

                }
            }
            return null;
        }
        */

        public static List<TblWorkFlowNode> GetNextProcess(int idNodeStart, out bool isEnd)
        {
            List<TblWorkFlowNode> lstNext = null;

            isEnd = false;
            if (idNodeStart == 0)
            {
                TblWorkFlowCollection wfColl = WorkFlowManager.GetFirstWorkFlow(false);
                if (wfColl != null)
                    lstNext = WorkFlowNodeManager.GetFirstCollection(wfColl[0].Id);
            }
            else
            {
                TblWorkFlowNode node = new SubSonic.Select().From(TblWorkFlowNode.Schema)
                    .Where(TblWorkFlowNode.DepartmentIDColumn).IsEqualTo(idNodeStart)
                    .ExecuteSingle<TblWorkFlowNode>();
                if (node != null)
                {
                    TblWorkFlowNodeCollection lstItem = WorkFlowNodeManager.GetNextNodeByNodeStartID(node.Id);
                    if (lstItem != null)
                        lstNext = lstItem.ToList();
                }
            }

            return lstNext;
        }

        static NodeTree GetNodeTree(TblWorkFlowNode node)
        {
            NodeTree nodeTree = new NodeTree();

            if (node.TblDepartment != null)
            {
                nodeTree.DepartmentId = node.TblDepartment.DepartmentID.ToString();
                nodeTree.DepartmentName = node.TblDepartment.DepartmentName;
                nodeTree.ProcessTypeID = node.TblDepartment.ProcessTypeID;
                nodeTree.ProductTypeID = node.TblDepartment.ProductTypeID;
            }

            nodeTree.NodeType = node.NodeType;

            if (node.NodeType != WorkflowNodeType.DauVaoHoacDauRa.ToString())
            {
                List<NodeTree> lstNodeTree = null;
                NodeTree nodeTemp = null;
                bool isEnd;

                List<TblWorkFlowNode> lstNext = GetNextProcess(node.DepartmentID, out isEnd);
                if (lstNext != null && lstNext.Count > 0 && isEnd == false)
                {
                    lstNodeTree = new List<NodeTree>();
                    foreach (TblWorkFlowNode item in lstNext)
                    {
                        nodeTemp = GetNodeTree(item);
                        if (nodeTemp != null && nodeTemp.nextNodes != null)
                            lstNodeTree.Add(nodeTemp);
                    }
                    nodeTree.nextNodes = lstNodeTree;
                }
            }
            return nodeTree;
        }

        public static List<NodeTree> GetAllProcess()
        {
            if (HttpContext.Current.Session["SweetSoft.APEM.Core.Manager-GetAllProcess"] != null)
                return HttpContext.Current.Session["SweetSoft.APEM.Core.Manager-GetAllProcess"] as List<NodeTree>;
            else
            {
                List<NodeTree> lstNodeTree = null;

                bool isEnd;
                NodeTree nodeTree = null;

                List<TblWorkFlowNode> lstNext = GetNextProcess(0, out isEnd);

                if (lstNext != null && lstNext.Count > 0)
                {
                    lstNodeTree = new List<NodeTree>();
                    foreach (TblWorkFlowNode item in lstNext)
                    {
                        nodeTree = GetNodeTree(item);
                        if (nodeTree != null && nodeTree.nextNodes != null)
                            lstNodeTree.Add(nodeTree);
                    }
                }

                HttpContext.Current.Session["SweetSoft.APEM.Core.Manager-GetAllProcess"] = lstNodeTree;

                return lstNodeTree;
            }
        }

        public static List<List<ProgressItem>> GetAllLineProcess()
        {
            if (HttpContext.Current.Session["SweetSoft.APEM.Core.Manager-GetAllLineProcess"] != null)
                return HttpContext.Current.Session["SweetSoft.APEM.Core.Manager-GetAllLineProcess"] as List<List<ProgressItem>>;
            else
            {
                List<List<ProgressItem>> lst = new List<List<ProgressItem>>();
                List<NodeTree> data = GetAllProcess();
                if (data != null && data.Count > 0)
                {
                    foreach (NodeTree nodeTree in data)
                        LoadToList(nodeTree, new List<ProgressItem>(), lst);
                }
                HttpContext.Current.Session["SweetSoft.APEM.Core.Manager-GetAllLineProcess"] = lst;
                return lst;
            }
        }

        static void LoadToList(NodeTree node, List<ProgressItem> lst, List<List<ProgressItem>> total)
        {
            lst.Add(new ProgressItem()
            {
                DepartmentId = node.DepartmentId,
                DepartmentName = node.DepartmentName,
                ProcessTypeID = node.ProcessTypeID,
                ProductTypeID = node.ProductTypeID
            });

            if (node.nextNodes != null && node.nextNodes.Count > 0)
            {
                if (node.nextNodes.Count == 1)
                {
                    foreach (NodeTree item in node.nextNodes)
                        LoadToList(item, lst, total);
                }
                else
                {
                    foreach (NodeTree item in node.nextNodes)
                    {
                        List<ProgressItem> temp = new List<ProgressItem>(lst);
                        LoadToList(item, temp, total);
                    }
                }
            }
            else
            {
                total.Add(lst);
            }
        }

        public static List<ProgressItem> GetLineProcessForCylinder(TblCylinder cylinder)
        {
            if (cylinder.ProcessTypeID.HasValue)
            {
                List<List<ProgressItem>> lst = GetAllLineProcess();
                if (lst != null && lst.Count > 0)
                {
                    foreach (List<ProgressItem> lstObj in lst)
                    {
                        bool isValid = true;
                        foreach (ProgressItem item in lstObj)
                        {
                            if (isValid == false)
                                break;

                            if (item.ProcessTypeID.HasValue == false || item.ProcessTypeID.Value == 0)
                                continue;
                            if (item.ProcessTypeID.HasValue == true && item.ProcessTypeID.Value > 0
                                && cylinder.ProcessTypeID.HasValue == true && cylinder.ProcessTypeID.Value > 0
                                && cylinder.ProcessTypeID.Value == item.ProcessTypeID.Value)
                            { }
                            else
                                isValid = false;
                        }

                        if (isValid)
                            return lstObj;
                    }
                }
            }
            return null;
        }
    }

    public enum CylinderProcessingStatus
    {
        Inprogress,
        Finish
    }

    public class ProgressItem
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public short? ProcessTypeID { get; set; }
        public string ProductTypeID { get; set; }
    }

    public class NodeTree
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public short? ProcessTypeID { get; set; }
        public string ProductTypeID { get; set; }
        public string NodeType { get; set; }
        public List<NodeTree> nextNodes { get; set; }
    }

    /// <summary>
    /// Summary description for CylinderProgress
    /// </summary>
    public class CylinderProgress
    {
        public CylinderProgress()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Status { get; set; }
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public string TimeProcess { get; set; }
    }
}