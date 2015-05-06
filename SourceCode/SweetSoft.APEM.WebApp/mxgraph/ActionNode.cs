using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    #region Action node

    public class ActionNodeHelper
    {
        static List<ObjectSave> listLaterAdd;
        static List<ObjectSave> listLaterUpdate;
        static List<ObjectSave> listUnsaveEdge;
        static List<int> listSaveSuccess;
        static List<int> listAllObjectSave;
        static ArrayAction arrAction;
        static int WorkFlowID;

        public static ObjectSave FindIdMap(string id)
        {
            if (arrAction.ins != null && arrAction.ins.Length > 0)
            {
                for (int i = 0; i < arrAction.ins.Length; i++)
                {
                    if (arrAction.ins[i].id == id)
                        return arrAction.ins[i];
                }
            }
            if (arrAction.upd != null && arrAction.upd.Length > 0)
            {
                for (int i = 0; i < arrAction.upd.Length; i++)
                {
                    if (arrAction.upd[i].id == id)
                        return arrAction.upd[i];
                }
            }
            if (arrAction.del != null && arrAction.del.Length > 0)
            {
                for (int i = 0; i < arrAction.del.Length; i++)
                {
                    if (arrAction.del[i].id == id)
                        return arrAction.del[i];
                }
            }
            return null;
        }

        
        static TblWorkFlowNode node;
        static TblWorkFlowLine edge;
        static string code;
        static string idMachineryProducetype;

        public static bool UpdateLateConnection(ObjectSave item)
        {
            int idtemp;
            if (item.id != null && !string.IsNullOrEmpty(item.id))
                int.TryParse(item.id, out idtemp);
            else
                idtemp = 0;
            if (idtemp > 0)
            {
                TblWorkFlowNode wfnode = WorkflowNodeHelper.FindTblWorkFlowNode(idtemp, WorkFlowID);
                if (wfnode != null)
                {
                    #region from collection

                    string newFrom = string.Empty;
                    if (item.from != null && item.from.Length > 0)
                    {
                        string[] arrFrom = item.from.Split(',');
                        if (arrFrom != null && arrFrom.Length > 0)
                        {
                            for (int i = 0; i < arrFrom.Length; i++)
                            {
                                int.TryParse(arrFrom[i], out idtemp);
                                if (idtemp > 0)
                                {
                                    TblWorkFlowNode wfnodeTo = WorkflowNodeHelper.FindTblWorkFlowNode(idtemp, WorkFlowID);
                                    if (wfnodeTo != null)
                                        newFrom += wfnodeTo.Id + ",";
                                }
                            }
                        }
                    }
                    if (newFrom.Length > 0 && newFrom.EndsWith(","))
                        newFrom = newFrom.Substring(0, newFrom.Length - 1);

                    #endregion

                    #region to collection

                    string newTo = string.Empty;
                    if (item.to != null && item.to.Length > 0)
                    {
                        string[] arrTo = item.to.Split(',');
                        if (arrTo != null && arrTo.Length > 0)
                        {
                            for (int i = 0; i < arrTo.Length; i++)
                            {
                                int.TryParse(arrTo[i], out idtemp);
                                if (idtemp > 0)
                                {
                                    TblWorkFlowNode wfnodeTo = WorkflowNodeHelper.FindTblWorkFlowNode(idtemp, WorkFlowID);
                                    if (wfnodeTo != null)
                                        newTo += wfnodeTo.Id + ",";
                                }
                            }
                        }
                    }
                    if (newTo.Length > 0 && newTo.EndsWith(","))
                        newTo = newTo.Substring(0, newTo.Length - 1);

                    #endregion

                    WorkFlowNodeManager.UpdateConnection(wfnode.Id, newFrom, newTo);
                    WorkflowNodeHelper.SaveTblWorkFlowNodeInSession(wfnode, WorkFlowID);
                    return true;
                }
                else
                {
                    //for case of subwork
                    return SaveObjectSave(item, true, false);
                }
            }

            return false;
        }

        public static bool SaveObjectSave(ObjectSave item, bool checkReference, bool force)
        {
            if (WorkFlowID == null || WorkFlowID == 0)
                return false;

            string userName = string.Empty;
            try
            {
                userName = ApplicationContext.Current.UserName;
            }
            catch
            {
                userName = "Anonymous";
            }

            int id;
            bool isExist = false;

            #region edge

            if (item.isEdge == true)
            {
                if (string.IsNullOrEmpty(item.sourceid) || string.IsNullOrEmpty(item.targetid))
                    return false;

                int.TryParse(item.id, out id);
                //save for later process
                listAllObjectSave.Add(id);

                edge = WorkFlowLineManager.GetExtractWorkFlowLineByIdInXML(id.ToString(), WorkFlowID);
                if (edge != null)
                    isExist = true;
                else
                {
                    edge = new TblWorkFlowLine();
                    edge.CreatedBy = userName;
                    edge.CreatedOn = DateTime.Now;
                }

                if (item.sourceid != null && !string.IsNullOrEmpty(item.sourceid))
                    int.TryParse(item.sourceid, out id);
                else
                    id = 0;

                if (id > 0)
                {
                    TblWorkFlowNode wfnode = WorkflowNodeHelper.FindTblWorkFlowNode(id, WorkFlowID);
                    if (wfnode != null)
                        edge.Node1ID = wfnode.Id;
                    else
                    {
                        AddToLaterAdd(item);
                        return false;
                    }
                }

                if (item.targetid != null && !string.IsNullOrEmpty(item.targetid))
                    int.TryParse(item.targetid, out id);
                else
                    id = 0;

                if (id > 0)
                {
                    TblWorkFlowNode wfnode = WorkflowNodeHelper.FindTblWorkFlowNode(id, WorkFlowID);
                    if (wfnode != null)
                        edge.Node2ID = wfnode.Id;
                    else
                    {
                        AddToLaterAdd(item);
                        return false;
                    }
                }

                if (code.ToLower() == "main-work")
                    edge.LineType = item.linetype;

                int.TryParse(idMachineryProducetype, out id);
                edge.MachineryProduceTypeID = id;

                if (edge.Node1ID.HasValue && edge.Node2ID.HasValue &&
                    edge.Node1ID.Value > 0 && edge.Node2ID.Value > 0)
                {

                    edge.WorkFlowID = WorkFlowID;

                    List<TblWorkFlow> wfTo = WorkFlowManager.GetWorkflowByIdMachineryProducetypeAndBetweenDepts(edge.MachineryProduceTypeID.Value, edge.Node1ID.Value, edge.Node2ID.Value);
                    edge.WorkFlowToID = wfTo != null && wfTo.Count > 0 ? wfTo[0].Id : (int?)null;

                    /*
                    if (item.isSubWork)
                        edge.Linetype = WorkflowNodeType.CongViecPhu.ToString();
                    else
                    {
                        if (item.dataCode != null && item.dataCode.Length > 0)
                            edge.Linetype = WorkflowNodeType.Xuong.ToString();
                        else if (item.isSource == true || item.isTarget == true)
                            edge.Linetype = WorkflowNodeType.DauVaoHoacDauRa.ToString();
                        else
                            edge.Linetype = WorkflowNodeType.CongViecChinh.ToString();
                    }
                    */

                    edge.ModifiedBy = userName;
                    edge.ModifiedOn = DateTime.Now;
                    edge.WorkFlowCode = code;
                    int.TryParse(item.id, out id);
                    edge.WorkFlowIDInXML = id;

                    if (isExist == true)
                        edge = WorkFlowLineManager.Update(edge);
                    else
                        edge = WorkFlowLineManager.Insert(edge);

                    //save success
                    listSaveSuccess.Add(edge.WorkFlowIDInXML.Value);
                }
                else
                    AddToLaterAddEdge(item);
            }

            #endregion

            else
            {
                #region Tblworkflow node

                bool laterUpdate = false;

                #region validate from collection

                string newFrom = string.Empty;
                if (checkReference == false)
                {
                    laterUpdate = true;
                    AddToLaterUpdate(item);
                }
                else
                {
                    if (item.from != null && item.from.Length > 0)
                    {
                        string[] arrFrom = item.from.Split(',');
                        if (arrFrom != null && arrFrom.Length > 0)
                        {

                            int idtemp = 0;
                            for (int i = 0; i < arrFrom.Length; i++)
                            {
                                int.TryParse(arrFrom[i], out idtemp);
                                if (idtemp > 0)
                                {
                                    TblWorkFlowNode wfnode = WorkflowNodeHelper.FindTblWorkFlowNode(idtemp, WorkFlowID);
                                    if (wfnode == null)
                                    {
                                        if (item.isSource || item.isTarget)
                                        {
                                            AddToLaterUpdate(item);
                                            laterUpdate = true;
                                            break;
                                        }
                                        else
                                        {
                                            AddToLaterAdd(item);
                                            return false;
                                        }
                                    }
                                    else
                                        newFrom += wfnode.Id + ",";
                                }
                            }
                            if (laterUpdate == false)
                            {
                                if (newFrom.Length > 0 && newFrom.EndsWith(","))
                                    newFrom = newFrom.Substring(0, newFrom.Length - 1);
                            }
                        }
                    }
                }

                if (laterUpdate == true)
                    newFrom = string.Empty;

                #endregion

                #region validate to collection

                string newTo = string.Empty;
                if (checkReference == false)
                {
                    laterUpdate = true;
                    AddToLaterUpdate(item);
                }
                else
                {
                    if (item.to != null && item.to.Length > 0)
                    {
                        string[] arrTo = item.to.Split(',');
                        if (arrTo != null && arrTo.Length > 0)
                        {

                            int idtemp = 0;
                            for (int i = 0; i < arrTo.Length; i++)
                            {
                                int.TryParse(arrTo[i], out idtemp);
                                if (idtemp > 0)
                                {
                                    TblWorkFlowNode wfnode = WorkflowNodeHelper.FindTblWorkFlowNode(idtemp, WorkFlowID);
                                    if (wfnode == null)
                                    {
                                        AddToLaterUpdate(item);
                                        laterUpdate = true;
                                        break;
                                    }
                                    else
                                        newTo += wfnode.Id + ",";
                                }
                            }
                            if (laterUpdate == false)
                            {
                                if (newTo.Length > 0 && newTo.EndsWith(","))
                                    newTo = newTo.Substring(0, newTo.Length - 1);
                            }
                        }
                    }
                }

                if (laterUpdate == true)
                    newTo = string.Empty;

                #endregion


                int.TryParse(item.id, out id);
                //save for later process
                listAllObjectSave.Add(id);

                node = WorkflowNodeHelper.FindTblWorkFlowNode(id, WorkFlowID);
                if (node != null)
                    isExist = true;
                else
                {
                    node = new TblWorkFlowNode();
                    node.CreatedBy = userName;
                    node.CreatedOn = DateTime.Now;
                }

                if (item.isSubWork)
                {
                    node.NodeType = WorkflowNodeType.CongViecPhu.ToString();
                    node.WorkFlowIsSend = false;
                    node.WorkFlowIsRoot = false;
                    node.Title = item.value;
                }
                else
                {
                    if (item.isSource == true)
                    {
                        node.WorkFlowIsSend = false;
                        node.WorkFlowIsRoot = true;
                        node.Title = item.value;
                    }
                    else if (item.isTarget == true)
                    {
                        node.WorkFlowIsSend = true;
                        node.WorkFlowIsRoot = true;
                        node.Title = item.value;
                    }

                    if (item.dataGraphId != null && !string.IsNullOrEmpty(item.dataGraphId.Trim()))
                        node.WorkFlowDataGraphID = item.dataGraphId;
                    else
                        node.WorkFlowDataGraphID = string.Empty;

                    if (item.dataCode != null && item.dataCode.Length > 0)
                        node.NodeType = WorkflowNodeType.Xuong.ToString();
                    else if (node.WorkFlowIsRoot.HasValue && node.WorkFlowIsRoot.Value == true)
                        node.NodeType = WorkflowNodeType.DauVaoHoacDauRa.ToString();
                    else
                        node.NodeType = WorkflowNodeType.CongViecChinh.ToString();
                }

                if (item.dataId != null && !string.IsNullOrEmpty(item.dataId))
                {
                    int.TryParse(item.dataId, out id);
                }
                else
                    id = 0;

                if (node.NodeType == WorkflowNodeType.Xuong.ToString())
                //if (item.dataCode != null && !string.IsNullOrEmpty(item.dataCode))
                {
                    if (id > 0)
                        node.DepartmentID = (short)id;
                    else
                        node.DepartmentID = 0;
                    node.WorkTaskID = 0;
                }
                else
                {
                    if (id > 0)
                        node.WorkTaskID = id;
                    else
                        node.WorkTaskID = null;
                    node.DepartmentID = 0;
                }

                node.WorkFlowID = WorkFlowID;
                node.ModifiedBy = userName;
                node.ModifiedOn = DateTime.Now;
                node.WorkFlowCode = code;
                node.WorkFlowDataGraphID = item.dataGraphId;
                int.TryParse(item.id, out id);
                node.WorkFlowIDInXML = id;
                int.TryParse(idMachineryProducetype, out id);
                node.MachineryProduceTypeID = id;
                node.WorkFlowIsConnectItseft = item.from.Contains(item.id) && item.to.Contains(item.id);
                node.WorkFlowListFromConnection = newFrom;
                node.WorkFlowListToConnection = newTo;
                try
                {
                    if (isExist == true)
                        node = WorkFlowNodeManager.Update(node);
                    else
                        node = WorkFlowNodeManager.Insert(node);
                }
                catch (Exception ex)
                {
                    //fail in case of node is delete from database but still kept in session of client
                    WorkflowNodeHelper.ResetWorkflowSession(WorkFlowID);
                    //we try save again
                    if (force != true)
                        return SaveObjectSave(item, checkReference, true);
                    else
                        return false;
                }
                //save success
                listSaveSuccess.Add(node.WorkFlowIDInXML.Value);
                WorkflowNodeHelper.SaveTblWorkFlowNodeInSession(node, WorkFlowID);

                #endregion
            }
            return true;
        }

        static void AddToLaterAddEdge(ObjectSave item)
        {
            if (listUnsaveEdge != null)
            {
                if (listUnsaveEdge.Count > 0)
                {
                    bool found = false;
                    foreach (ObjectSave itemInLst in listUnsaveEdge)
                    {
                        if (item.id == itemInLst.id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                        listUnsaveEdge.Add(item);
                }
                else
                    listUnsaveEdge.Add(item);
            }
            else
                listUnsaveEdge = new List<ObjectSave>() { item };
        }

        static void AddToLaterAdd(ObjectSave item)
        {
            if (listLaterAdd != null)
            {
                if (listLaterAdd.Count > 0)
                {
                    bool found = false;
                    foreach (ObjectSave itemInLst in listLaterAdd)
                    {
                        if (item.id == itemInLst.id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                        listLaterAdd.Add(item);
                }
                else
                    listLaterAdd.Add(item);
            }
            else
                listLaterAdd = new List<ObjectSave>() { item };
        }

        static void AddToLaterUpdate(ObjectSave item)
        {
            if (listLaterUpdate != null)
            {
                if (listLaterUpdate.Count > 0)
                {
                    bool found = false;
                    foreach (ObjectSave itemInLst in listLaterUpdate)
                    {
                        if (item.id == itemInLst.id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                        listLaterUpdate.Add(item);
                }
                else
                    listLaterUpdate.Add(item);
            }
            else
                listLaterUpdate = new List<ObjectSave>() { item };
        }

        public static bool CheckIsModify(string data, out ActionNode action)
        {
            action = JsonConvert.DeserializeObject<ActionNode>(data);
            if (action != null)
            {
                arrAction = action.arr;
                if (arrAction.ins != null && arrAction.ins.Length > 0)
                    return true;

                if (arrAction.upd != null && arrAction.upd.Length > 0)
                    return true;

                if (arrAction.del != null && arrAction.del.Length > 0)
                    return true;

                arrAction = null;
            }
            return false;
        }
        
        public static List<int> ExtractListAllId(string data)
        {
            List<int> lstReturn = new List<int>();
            ActionNode action = JsonConvert.DeserializeObject<ActionNode>(data);
            if (action != null)
            {
                arrAction = action.arr;
                if (arrAction.ins != null && arrAction.ins.Length > 0)
                {
                    var lst = arrAction.ins.Select(x => int.Parse(x.id));
                    if (lst != null)
                        lstReturn.AddRange(lst);
                }

                if (arrAction.upd != null && arrAction.upd.Length > 0)
                {
                    var lst = arrAction.upd.Select(x => int.Parse(x.id));
                    if (lst != null)
                        lstReturn.AddRange(lst);
                }

                if (arrAction.del != null && arrAction.del.Length > 0)
                {
                    var lst = arrAction.del.Select(x => int.Parse(x.id));
                    if (lst != null)
                        lstReturn.AddRange(lst);
                }
            }
            return lstReturn;
        }

        public static void ProcessSaveObject(List<ObjectSave> filterNode)
        {
            if (filterNode != null)
            {
                foreach (ObjectSave filtered in filterNode)
                    SaveObjectSave(filtered, true, false);
            }

            if (listLaterAdd != null && listLaterAdd.Count > 0)
            {
                List<ObjectSave> lstUnSave = new List<ObjectSave>();
                for (int i = 0; i < listLaterAdd.Count; i++)
                {
                    bool isSave = SaveObjectSave(listLaterAdd[i], true, false);
                    if (isSave == false)
                        lstUnSave.Add(listLaterAdd[i]);
                }

                if (lstUnSave != null && lstUnSave.Count > 0)
                {
                    for (int i = 0; i < lstUnSave.Count; i++)
                    {
                        SaveObjectSave(lstUnSave[i], false, false);
                    }
                }
            }

            if (listLaterUpdate != null && listLaterUpdate.Count > 0)
            {
                for (int i = 0; i < listLaterUpdate.Count; i++)
                    UpdateLateConnection(listLaterUpdate[i]);
            }

            listLaterAdd.Clear();
            listLaterUpdate.Clear();
        }

        public static List<int> SaveActionNode(string data, int _WorkFlowID)
        {
            ActionNode action = JsonConvert.DeserializeObject<ActionNode>(data);
            if (action != null)
            {
                WorkFlowID = _WorkFlowID;
                listLaterAdd = new List<ObjectSave>();
                listLaterUpdate = new List<ObjectSave>();
                listUnsaveEdge = new List<ObjectSave>();
                listSaveSuccess = new List<int>();
                listAllObjectSave = new List<int>();

                arrAction = action.arr;

                string[] arrData = action.id.Split('*');
                if (arrData.Length == 2)
                {
                    code = Path.GetFileNameWithoutExtension(arrData[0]);
                    idMachineryProducetype = Regex.Replace(arrData[1], "[^0-9-]", "");
                    //if (code.ToLower() == "main-work")
                    //    idMachineryProducetype = "0";
                    if (arrAction != null)
                    {
                        #region node
                        if (arrAction.ins != null && arrAction.ins.Length > 0)
                        {
                            //node
                            var filterTemp = arrAction.ins.Where(x => x.isEdge != null && x.isEdge == false);
                            if (filterTemp != null)
                                ProcessSaveObject(filterTemp.ToList());
                        }

                        if (arrAction.upd != null && arrAction.upd.Length > 0)
                        {
                            //node
                            var filterTemp = arrAction.upd.Where(x => x.isEdge != null && x.isEdge == false);
                            if (filterTemp != null)
                                ProcessSaveObject(filterTemp.ToList());
                        }
                        #endregion

                        #region line
                        if (arrAction.ins != null && arrAction.ins.Length > 0)
                        {
                            //line
                            var filterTemp = arrAction.ins.Where(x => x.isEdge != null && x.isEdge == true);
                            if (filterTemp != null)
                                ProcessSaveObject(filterTemp.ToList());
                        }

                        if (arrAction.upd != null && arrAction.upd.Length > 0)
                        {
                            //line
                            var filterTemp = arrAction.upd.Where(x => x.isEdge != null && x.isEdge == true);
                            if (filterTemp != null)
                                ProcessSaveObject(filterTemp.ToList());
                        }
                        #endregion

                        if (listUnsaveEdge != null && listUnsaveEdge.Count > 0)
                        {
                            ProcessSaveObject(listUnsaveEdge);
                        }

                        if (arrAction.del != null && arrAction.del.Length > 0)
                        {
                            #region RegionName

                            int id = 0;
                            foreach (ObjectSave item in arrAction.del)
                            {
                                int.TryParse(item.id, out id);
                                if (id > 0)
                                {
                                    if (item.isEdge)
                                    {
                                        WorkFlowLineManager.DeletefromDesign(id, WorkFlowID);
                                    }
                                    else
                                    {
                                        //delete node in database
                                        bool canDelete = WorkFlowNodeManager.DeletefromDesign(id, _WorkFlowID);
                                        if (canDelete)
                                            //delete from session
                                            WorkflowNodeHelper.RemoveTblWorkFlowNodeInSession(id, _WorkFlowID);
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
            }

            List<int> listError = new List<int>();
            if (listSaveSuccess != null && listSaveSuccess.Count > 0)
            {
                for (int i = 0; i < listAllObjectSave.Count; i++)
                {
                    //if list save success not found then save in list error
                    if (!listSaveSuccess.Contains(listAllObjectSave[i]))
                    {
                        if (!listError.Contains(listAllObjectSave[i]))
                            listError.Add(listAllObjectSave[i]);
                    }
                }
            }

            //reset
            arrAction = null;
            listLaterAdd = null;
            listSaveSuccess = null;
            return listError;
        }
    }


    /// <summary>
    /// Summary description for ActionNode
    /// </summary>
    public class ActionNode
    {
        public string id { get; set; }
        public ArrayAction arr { get; set; }
    }

    public class ArrayAction
    {
        public ObjectSave[] ins { get; set; }
        public ObjectSave[] del { get; set; }
        public ObjectSave[] upd { get; set; }
    }

    public class ObjectSave
    {
        public string id { get; set; }
        public string dataId { get; set; }
        public int nodeId { get; set; }
        public string dataGraphId { get; set; }
        public string dataCode { get; set; }
        public string value { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string linetype { get; set; }
        public bool isEdge { get; set; }
        public bool isSubWork { get; set; }
        public bool isSource { get; set; }
        public bool isTarget { get; set; }
        public string sourceid { get; set; }
        public string targetid { get; set; }
    }

    #endregion
}