using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using System.ComponentModel;
using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.Core.Manager
{
    #region Workflow Node Xml helper

    public class WorkflowNodeHelper
    {
        #region Private prop

        static HtmlNodeCollection lstMxCellNode;
        static List<WorkflowNode> lstWorkFlowNode;
        static Dictionary<int, List<HtmlNode>> dicMap = null;

        static Dictionary<long, List<WorkflowNode>> DesignCollection
        {
            get
            {
                if (HttpContext.Current.Session["DesignCollection"] == null)
                    HttpContext.Current.Session["DesignCollection"] = new Dictionary<long, List<WorkflowNode>>();
                return HttpContext.Current.Session["DesignCollection"] as Dictionary<long, List<WorkflowNode>>;
            }
            set
            {
                HttpContext.Current.Session["DesignCollection"] = value;
            }
        }

        static Dictionary<long, List<WorkflowNode>> LineInDesignCollection
        {
            get
            {
                if (HttpContext.Current.Session["LineInDesignCollection"] == null)
                    HttpContext.Current.Session["LineInDesignCollection"] = new Dictionary<long, List<WorkflowNode>>();
                return HttpContext.Current.Session["LineInDesignCollection"] as Dictionary<long, List<WorkflowNode>>;
            }
            set
            {
                HttpContext.Current.Session["LineInDesignCollection"] = value;
            }
        }

        static Dictionary<int, string> WorkflowCollection
        {
            get
            {
                if (HttpContext.Current.Session["WorkflowCollection"] == null)
                    HttpContext.Current.Session["WorkflowCollection"] = new Dictionary<int, string>();
                return HttpContext.Current.Session["WorkflowCollection"] as Dictionary<int, string>;
            }
            set
            {
                HttpContext.Current.Session["WorkflowCollection"] = value;
            }
        }

        static Dictionary<long, List<TblWorkFlowNode>> TblWorkFlowNodeConverted
        {
            get
            {
                if (HttpContext.Current.Session["TblWorkFlowNodeConverted"] == null)
                    HttpContext.Current.Session["TblWorkFlowNodeConverted"] = new Dictionary<long, List<TblWorkFlowNode>>();
                return HttpContext.Current.Session["TblWorkFlowNodeConverted"] as Dictionary<long, List<TblWorkFlowNode>>;
            }
            set
            {
                HttpContext.Current.Session["TblWorkFlowNodeConverted"] = value;
            }
        }

        static Dictionary<long, List<TblWorkFlowLine>> TblWorkFlowLineConverted
        {
            get
            {
                if (HttpContext.Current.Session["TblWorkFlowLineConverted"] == null)
                    HttpContext.Current.Session["TblWorkFlowLineConverted"] = new Dictionary<long, List<TblWorkFlowLine>>();
                return HttpContext.Current.Session["TblWorkFlowLineConverted"] as Dictionary<long, List<TblWorkFlowLine>>;
            }
            set
            {
                HttpContext.Current.Session["TblWorkFlowLineConverted"] = value;
            }
        }

        static long CurrentProcessWorkFlowID
        {
            get
            {
                if (HttpContext.Current.Session["CurrentProcessWorkFlow"] != null)
                    return long.Parse(HttpContext.Current.Session["CurrentProcessWorkFlow"].ToString());
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["CurrentProcessWorkFlow"] = value;
            }
        }

        public static void RemoveAllSession()
        {
            HttpContext.Current.Session.Remove("DesignCollection");
            HttpContext.Current.Session.Remove("LineInDesignCollection");
            HttpContext.Current.Session.Remove("WorkflowCollection");
            HttpContext.Current.Session.Remove("TblWorkFlowNodeConverted");
            HttpContext.Current.Session.Remove("CurrentProcessWorkFlow");
        }

        #endregion

        #region Private method

        /// <summary>
        /// Map to connect and from connect of WorkflowNode item
        /// </summary>
        /// <param name="item">WorkflowNode object</param>
        static void MapConnect(WorkflowNode item, int idWorkflow)
        {
            if (dicMap != null && dicMap.Count > 0)
            {
                if (!dicMap.ContainsKey(item.Id))
                    return;

                List<HtmlNode> lstMap = dicMap[item.Id];
                if (lstMap != null && lstMap.Count > 0)
                {
                    List<int> lstTo = new List<int>();
                    List<int> lstFrom = new List<int>();
                    IEnumerable<HtmlNode> temp = null;
                    foreach (HtmlNode itemMap in lstMap)
                    {
                        if (itemMap.Attributes["source"] != null
                            && !string.IsNullOrEmpty(itemMap.Attributes["source"].Value)
                            && itemMap.Attributes["target"] != null
                            && !string.IsNullOrEmpty(itemMap.Attributes["target"].Value)
                            && itemMap.Attributes["source"].Value.Trim() == itemMap.Attributes["target"].Value.Trim())
                        {
                            item.IsConnectItSelf = true;
                        }
                        else if (itemMap.Attributes["source"] != null
                            && !string.IsNullOrEmpty(itemMap.Attributes["source"].Value.Trim())
                            && itemMap.Attributes["source"].Value.Trim() == item.Id.ToString())
                        {
                            temp = lstMxCellNode.Where(o =>
                                  (itemMap.Attributes["target"] != null &&
                                  o.Attributes["id"].Value.Trim() == itemMap.Attributes["target"].Value.Trim()));
                            if (temp != null && temp.Count() > 0)
                                lstTo.Add(ConvertToWorkflowNode(temp.First(), idWorkflow).Id);
                        }
                        else if (itemMap.Attributes["target"] != null
                            && !string.IsNullOrEmpty(itemMap.Attributes["target"].Value.Trim())
                            && itemMap.Attributes["target"].Value.Trim() == item.Id.ToString())
                        {
                            temp = lstMxCellNode.Where(o =>
                                  (itemMap.Attributes["source"] != null &&
                                  o.Attributes["id"].Value.Trim() == itemMap.Attributes["source"].Value.Trim()));
                            if (temp != null && temp.Count() > 0)
                                lstFrom.Add(ConvertToWorkflowNode(temp.First(), idWorkflow).Id);
                        }
                    }
                    item.toConnectCollection = lstTo;
                    item.fromConnectCollection = lstFrom;
                }
            }
        }

        /// <summary>
        /// Convert WorkflowNode from HtmlNode.
        /// </summary>
        /// <param name="item">HtmlNode from HtmlAgilityPack.</param>
        /// <returns>WorkflowNode object</returns>
        static WorkflowNode ConvertToWorkflowNode(HtmlNode item, int idWorkflow)
        {
            WorkflowNode workflowNode = null;
            if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
            {
                var found = lstWorkFlowNode.Where(o => o.Id.ToString().CompareTo(item.Attributes["id"].Value.Trim()) == 0);
                if (found != null && found.Count() > 0)
                    //return (found as List<WorkflowNode>)[0];
                    return found.First();
            }

            List<HtmlNode> lstConnect = new List<HtmlNode>();
            int id = 0;
            workflowNode = new WorkflowNode();
            if (item.Attributes["id"] != null && !string.IsNullOrEmpty(item.Attributes["id"].Value.Trim().ToString()))
                int.TryParse(item.Attributes["id"].Value.Trim(), out id);
            else
                id = 0;
            workflowNode.Id = id;
            workflowNode.Title = item.Attributes["value"] != null ? item.Attributes["value"].Value : string.Empty;

            workflowNode.idWorkflow = idWorkflow;
            if (item.Attributes["isSource"] != null)
            {
                workflowNode.IsEnd = false;
                workflowNode.IsRoot = true;
            }
            else if (item.Attributes["isTarget"] != null)
            {
                workflowNode.IsRoot = true;
                workflowNode.IsEnd = true;
            }

            if (item.Attributes["dataGraphId"] != null && !string.IsNullOrEmpty(item.Attributes["dataGraphId"].Value.Trim()))
                workflowNode.DataGraphId = item.Attributes["dataGraphId"].Value;
            else
                workflowNode.DataGraphId = string.Empty;

            if (item.Attributes["isSubWork"] != null)
                workflowNode.Type = WorkflowNodeType.CongViecPhu;
            else
            {
                if (item.Attributes["dataCode"] != null)
                    workflowNode.Type = WorkflowNodeType.Xuong;
                else if (workflowNode.IsRoot)
                    workflowNode.Type = WorkflowNodeType.DauVaoHoacDauRa;
                else
                    workflowNode.Type = WorkflowNodeType.CongViecChinh;
            }

            if (item.Attributes["dataId"] != null && !string.IsNullOrEmpty(item.Attributes["dataId"].Value.Trim()))
            {
                short idtemp = 0;
                short.TryParse(item.Attributes["dataId"].Value, out idtemp);
                if (idtemp > 0)
                {
                    if (workflowNode.Type == WorkflowNodeType.Xuong)
                        workflowNode.TblDepartment = DepartmentManager.SelectByID(idtemp);
                    else
                        workflowNode.TblWorkTask = WorkTaskManager.GetWorktaskById(idtemp);
                }
            }

            if (item.Attributes["source"] != null && !string.IsNullOrEmpty(item.Attributes["source"].Value.Trim()))
            {
                int idtemp = 0;
                if (!int.TryParse(item.Attributes["source"].Value, out idtemp))
                    idtemp = -1;
                if (idtemp > 0)
                    workflowNode.IdStart = idtemp;
            }

            if (item.Attributes["target"] != null && !string.IsNullOrEmpty(item.Attributes["target"].Value.Trim()))
            {
                int idtemp = 0;
                if (!int.TryParse(item.Attributes["target"].Value, out idtemp))
                    idtemp = -1;
                if (idtemp > 0)
                    workflowNode.IdEnd = idtemp;
            }

            /*
            if (workflowNode.IdStart > 0 && workflowNode.IdEnd == 0)
                return null;

            if (workflowNode.IdStart == 0 && workflowNode.IdEnd > 0)
                return null;
            */

            string linetype = "DuongChuyenChinh";
            workflowNode.linetype = linetype;
            if (item.Attributes["style"] != null)
            {
                if (item.Attributes["style"].Value.Contains("e43ed2"))
                    workflowNode.linetype = "DuongChuyenPhu";
                else if (item.Attributes["style"].Value.Contains("0D8517"))
                    workflowNode.linetype = "CaHai";
            }
            if (lstMxCellNode != null && lstMxCellNode.Count > 0)
            {
                //find source and target

                #region source

                lstConnect.AddRange(lstMxCellNode.Where(o => (
                    o.Attributes["edge"] != null &&
                    o.Attributes["edge"].Value == "1" &&
                    o.Attributes["source"] != null &&
                    o.Attributes["source"].Value == item.Attributes["id"].Value.Trim())).ToList());

                #endregion

                #region target

                lstConnect.AddRange(lstMxCellNode.Where(o => (
                    o.Attributes["edge"] != null &&
                    o.Attributes["edge"].Value == "1" &&
                    o.Attributes["target"] != null &&
                    o.Attributes["target"].Value == item.Attributes["id"].Value.Trim())).ToList());

                #endregion
            }

            //add for later work
            if (dicMap == null)
                dicMap = new Dictionary<int, List<HtmlNode>>();
            dicMap.Add(id, lstConnect);

            return workflowNode;
        }

        #region Get To Collection

        /// <summary>
        /// Get list of WorkflowNode to connect
        /// from given [worlflow code and idWorkflow] and xml node id.
        /// </summary>
        /// <param name="workflowCode">workflow code</param>
        /// <param name="idWorkflow">id of machinery producetype</param>
        /// <param name="id">xml node id</param>
        /// <returns>List of WorkflowNode object</returns>
        static List<WorkflowNode> GetToCollection(int idWorkflow, int id)
        {
            lstWorkFlowNode = new List<WorkflowNode>();
            WorkflowNode node = GetWorkflowNode(idWorkflow, id);
            if (node != null)
                return GetToCollection(idWorkflow, node);
            else
                return new List<WorkflowNode>();
        }

        static List<WorkflowNode> GetToCollection(int idWorkflow, List<int> toCollection)
        {
            if (idWorkflow == 0)
                return null;

            if (toCollection.Count > 0)
            {
                lstWorkFlowNode = GetAllWorkflowNode(idWorkflow);
                if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                {
                    List<WorkflowNode> lst = new List<WorkflowNode>();
                    foreach (int id in toCollection)
                    {
                        WorkflowNode node = GetWorkflowNode(idWorkflow, id);

                        //DEV: ignore CongViecPhu
                        //if (node.Type != WorkflowNodeType.CongViecPhu)
                        lst.Add(node);
                    }
                    return lst;
                }
            }

            return null;
        }

        static List<WorkflowNode> GetToCollection(int idWorkflow, WorkflowNode item)
        {
            if (item == null || idWorkflow == 0)
                return null;

            if (item.toConnectCollection != null && item.toConnectCollection.Count > 0)
                return GetToCollection(idWorkflow, item.toConnectCollection);

            return null;
        }

        #endregion

        #region Get From Collection

        static List<WorkflowNode> GetFromCollection(int idWorkflow, int id)
        {
            lstWorkFlowNode = new List<WorkflowNode>();
            WorkflowNode node = GetWorkflowNode(idWorkflow, id);
            if (node != null)
                return GetFromCollection(idWorkflow, node);
            else
                return null;
        }

        static List<WorkflowNode> GetFromCollection(int idWorkflow, List<int> fromCollection)
        {
            if (idWorkflow == 0)
                return null;

            if (fromCollection.Count > 0)
            {
                lstWorkFlowNode = GetAllWorkflowNode(idWorkflow);
                if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                {
                    List<WorkflowNode> lst = new List<WorkflowNode>();
                    foreach (int id in fromCollection)
                    {
                        WorkflowNode node = GetWorkflowNode(idWorkflow, id);

                        //DEV: ignore CongViecPhu
                        //if (node.Type != WorkflowNodeType.CongViecPhu)
                        lst.Add(node);
                    }
                }
            }

            return null;
        }

        static List<WorkflowNode> GetFromCollection(int idWorkflow, WorkflowNode item)
        {
            if (item == null || idWorkflow == 0)
                return null;

            if (item.fromConnectCollection != null && item.fromConnectCollection.Count > 0)
                return GetFromCollection(idWorkflow, item.fromConnectCollection);

            return null;
        }

        #endregion

        #endregion


        #region Get Workflow Node


        /// <summary>
        /// Get WorkflowNode from id string.
        /// </summary>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        /// <param name="idString">xml node id string</param>
        /// <returns>WorkflowNode object</returns>
        public static WorkflowNode GetWorkflowNode(int idWorkflow, string idString)
        {
            int id = 0;
            int.TryParse(idString, out id);
            if (id > 0)
                return GetWorkflowNode(idWorkflow, id);
            else
                return null;
        }

        /// <summary>
        /// Get WorkflowNode from id.
        /// </summary>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        /// <param name="id">xml node id</param>
        /// <returns>WorkflowNode object</returns>
        public static WorkflowNode GetWorkflowNode(int idWorkflow, int id)
        {
            List<WorkflowNode> lst = GetAllWorkflowNode(idWorkflow);
            if (lst != null && lst.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lst)
                {
                    if (workflowNode.Id == id)
                        return workflowNode;
                }
            }
            return null;
        }

        #endregion

        #region Get Workflow Node By Id Map From Database

        /// <summary>
        /// Get List of WorkflowNode from TblWorkflow id string.
        /// </summary>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        /// <param name="idMapString">Tbldept id string or tblworktask id string</param>
        /// <param name="type">WorkflowNodeType type</param>
        /// <returns>List of WorkflowNode object</returns>
        public static List<WorkflowNode> GetWorkflowNodeByIdMapFromDatabase(int idWorkflow, string idMapString, WorkflowNodeType type)
        {
            int id = 0;
            int.TryParse(idMapString, out id);
            if (id > 0)
                return GetWorkflowNodeByIdMapFromDatabase(idWorkflow, id, type);
            else
                return new List<WorkflowNode>();
        }

        /// <summary>
        /// Get List of WorkflowNode from TblWorkflow id.
        /// </summary>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        /// <param name="idMap">Tbldept id or tblworktask id</param>
        /// <param name="type">WorkflowNodeType type</param>
        /// <returns>List of WorkflowNode object</returns>
        public static List<WorkflowNode> GetWorkflowNodeByIdMapFromDatabase(int idWorkflow, int idMap, WorkflowNodeType type)
        {
            List<WorkflowNode> lst = new List<WorkflowNode>();
            List<WorkflowNode> lstAll = GetAllWorkflowNode(idWorkflow);
            if (lstAll != null && lstAll.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lstAll)
                {
                    if ((type == WorkflowNodeType.Xuong && workflowNode.TblDepartment != null && workflowNode.TblDepartment.DepartmentID == idMap) ||
                        (type == WorkflowNodeType.CongViecChinh && workflowNode.TblWorkTask != null && workflowNode.TblWorkTask.Id == idMap))
                    {
                        bool isExist = false;
                        foreach (WorkflowNode item in lst)
                        {
                            if (item.Id == workflowNode.Id)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)
                            lst.Add(workflowNode);
                    }
                }

            }
            return lst;
        }

        /// <summary>
        /// Get WorkflowNode from tbldept id or tblworktask id.
        /// </summary>
        /// <param name="workflowCode">string of workflow code</param>
        /// <param name="idWorkflow">idWorkflow connect by "*" character</param>
        /// <param name="idMap">Tbldept id or tblworktask id</param>
        /// <param name="type">WorkflowNodeType type</param>
        /// <returns>WorkflowNode object</returns>
        public static WorkflowNode GetWorkflowNodeByIdMapFromDatabase(int idWorkflow,
            int idMap, WorkflowNodeType type, List<int> fromCollection)
        {
            return GetExtractWorkflowNodeByIdMapFromDatabase(idWorkflow, idMap, type, fromCollection);
        }

        /// <summary>
        /// Get WorkflowNode from tbldept id or tblworktask id.
        /// </summary>
        /// <param name="workflowCode">string of workflow code</param>
        /// <param name="idWorkflow">idWorkflow connect by "*" character</param>
        /// <param name="idMapString">Tbldept id string or tblworktask id string</param>
        /// <param name="type">WorkflowNodeType type</param>
        /// <returns>WorkflowNode object</returns>
        public static WorkflowNode GetWorkflowNodeByIdMapFromDatabase(int idWorkflow,
            string idMapString, WorkflowNodeType type, List<int> fromCollection)
        {
            int id = 0;
            int.TryParse(idMapString, out id);
            if (id > 0)
                return GetExtractWorkflowNodeByIdMapFromDatabase(idWorkflow, id, type, fromCollection);
            else
                return null;
        }

        /// <summary>
        /// Get WorkflowNode from tbldept id or tblworktask id.
        /// </summary>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        /// <param name="idMap">Tbldept id or tblworktask id</param>
        /// <param name="type">WorkflowNodeType type</param>
        /// <returns>WorkflowNode object</returns>
        public static WorkflowNode GetExtractWorkflowNodeByIdMapFromDatabase(int idWorkflow, int idMap,
            WorkflowNodeType type, List<int> fromCollection)
        {
            List<WorkflowNode> lst = GetWorkflowNodeByIdMapFromDatabase(idWorkflow, idMap, type);
            if (lst != null && lst.Count > 0)
            {
                foreach (WorkflowNode item in lst)
                {
                    if ((type == WorkflowNodeType.Xuong && item.TblDepartment.DepartmentID == idMap && item.fromConnectCollection.Count == fromCollection.Count) ||
                        type == WorkflowNodeType.CongViecChinh && item.TblWorkTask.Id == idMap && item.fromConnectCollection.Count == fromCollection.Count)
                    {
                        bool isFound = true;
                        for (int i = 0; i < item.fromConnectCollection.Count; i++)
                        {
                            if (item.fromConnectCollection[i] != fromCollection[i])
                            {
                                isFound = false;
                                break;
                            }
                        }

                        if (isFound == true)
                            return item;
                    }
                }
            }
            return null;
        }

        /*
        /// <summary>
        /// Get List of WorkflowNode from TblWorkflow id.
        /// Default WorkflowCodeAndidWorkflow is lastest [worlflow code and idWorkflow]
        /// </summary>
        /// <param name="idMap">TblWorkflow id</param>
        /// <param name="type">WorkflowNodeType type</param>
        /// <returns>List of WorkflowNode object</returns>
        public static List<WorkflowNode> GetWorkflowNodeByIdMapFromDatabase(int idMap, WorkflowNodeType type)
        {
            return GetWorkflowNodeByIdMapFromDatabase(CurrentProcessWorkFlowID, idMap, type);
        }        

        /// <summary>
        /// Get List of WorkflowNode from TblWorkflow id string.
        /// Default WorkflowCodeAndidWorkflow is lastest [worlflow code and idWorkflow]
        /// </summary>
        /// <param name="idMapString">TblWorkflow id string</param>
        /// <returns>List of WorkflowNode object</returns>
        public static List<WorkflowNode> GetWorkflowNodeByIdMapFromDatabase(string idMapString, WorkflowNodeType type)
        {
            int id = 0;
            int.TryParse(idMapString, out id);
            if (id > 0)
                return GetWorkflowNodeByIdMapFromDatabase(id, type);
            else
                return null;
        }
        */

        #endregion

        #region Get All Workflow Node

        /*
        /// <summary>
        /// Get all Workflow Node from lastest [worlflow code and idWorkflow] connect by "*" character
        /// </summary>
        /// <returns>List of WorkflowNode object</returns>
        public static List<WorkflowNode> GetAllWorkflowNode()
        {
            return GetAllWorkflowNode(CurrentProcessWorkFlowID);
        }
        */


        /// <summary>
        /// Get and save all WorkFlow Node getted from content XML to session
        /// </summary>
        /// <param name="contentXML">string of Xml graph</param>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        public static void SaveWorkFlowNode(string contentXML, int idWorkflow)
        {
            if (idWorkflow == 0)
                return;

            CurrentProcessWorkFlowID = idWorkflow;
            Dictionary<long, List<WorkflowNode>> designCollection = DesignCollection;
            if (designCollection == null)
                designCollection = new Dictionary<long, List<WorkflowNode>>();

            if (designCollection.ContainsKey(idWorkflow))
                return;

            HtmlDocument xdoc = new HtmlDocument();
            xdoc.LoadHtml(contentXML);
            lstMxCellNode = xdoc.DocumentNode.SelectNodes("descendant::*[translate(name(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='mxcell']");
            lstWorkFlowNode = new List<WorkflowNode>();
            dicMap = new Dictionary<int, List<HtmlNode>>();

            if (lstMxCellNode != null && lstMxCellNode.Count > 0)
            {
                List<HtmlNode> vertexColl = lstMxCellNode.Where(o => o.Attributes["vertex"] != null && o.Attributes["vertex"].Value == "1").ToList();
                if (vertexColl != null && vertexColl.Count > 0)
                {
                    foreach (HtmlNode item in vertexColl)
                        lstWorkFlowNode.Add(ConvertToWorkflowNode(item, idWorkflow));

                    if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                    {
                        foreach (WorkflowNode workflowNode in lstWorkFlowNode)
                            MapConnect(workflowNode, idWorkflow);

                        designCollection.Add(idWorkflow, lstWorkFlowNode);
                        DesignCollection = designCollection;


                        TblWorkFlowNodeCollection allConvert = ConvertToTblWorkFlowNode(lstWorkFlowNode);
                        Dictionary<long, List<TblWorkFlowNode>> allWorkflowNode = TblWorkFlowNodeConverted;
                        if (allWorkflowNode == null || allWorkflowNode.Count == 0)
                            allWorkflowNode = new Dictionary<long, List<TblWorkFlowNode>>();
                        if (allConvert != null && allConvert.Count > 0)
                        {
                            if (allWorkflowNode.ContainsKey(idWorkflow))
                            {
                                List<TblWorkFlowNode> lst = allWorkflowNode[idWorkflow];
                                if (lst != null)
                                {
                                    if (lst.Count > 0)
                                    {
                                        foreach (TblWorkFlowNode node in allConvert)
                                        {
                                            bool found = false;
                                            foreach (TblWorkFlowNode item in lst)
                                            {
                                                if (item.Id == node.Id)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found == false)
                                                lst.Add(node);
                                        }
                                    }
                                    else
                                        lst.AddRange(allConvert.ToList());
                                }
                                else
                                    lst = allConvert.ToList();
                                allWorkflowNode[idWorkflow] = lst;
                            }
                            else
                                allWorkflowNode.Add(idWorkflow, allConvert.ToList());
                        }
                        TblWorkFlowNodeConverted = allWorkflowNode;
                    }
                }
            }
        }


        public static void RemoveTblWorkFlowNodeInSession(int IdInXML, int idWorkflow)
        {
            Dictionary<long, List<TblWorkFlowNode>> allWorkflowNode = TblWorkFlowNodeConverted;
            if (allWorkflowNode != null && allWorkflowNode.Count > 0)
            {
                if (allWorkflowNode.ContainsKey(idWorkflow))
                {
                    List<TblWorkFlowNode> lst = allWorkflowNode[idWorkflow];
                    if (lst != null && lst.Count > 0)
                    {
                        int pos = -1;
                        for (int i = 0; i < lst.Count; i++)
                        {
                            TblWorkFlowNode item = lst[i];
                            if (item.WorkFlowIDInXML.HasValue && item.WorkFlowIDInXML.Value == IdInXML)
                            {
                                pos = i;
                                break;
                            }
                        }

                        if (pos != -1)
                            lst.RemoveAt(pos);

                        allWorkflowNode[idWorkflow] = lst;

                        TblWorkFlowNodeConverted = allWorkflowNode;
                    }
                }
            }
        }

        public static void SaveTblWorkFlowNodeInSession(TblWorkFlowNode node, int idWorkflow)
        {
            Dictionary<long, List<TblWorkFlowNode>> allWorkflowNode = TblWorkFlowNodeConverted;
            if (allWorkflowNode != null && allWorkflowNode.Count > 0)
            {
                if (allWorkflowNode.ContainsKey(idWorkflow))
                {
                    List<TblWorkFlowNode> lst = allWorkflowNode[idWorkflow];
                    if (lst != null)
                    {
                        if (lst.Count == 0)
                            lst.Add(node);
                        else
                        {
                            int pos = -1;
                            for (int i = 0; i < lst.Count; i++)
                            {
                                TblWorkFlowNode item = lst[i];
                                if (item.Id == node.Id)
                                {
                                    pos = i;
                                    break;
                                }
                            }

                            if (pos == -1)
                                lst.Add(node);
                            else
                            {
                                lst.RemoveAt(pos);
                                lst.Add(node);
                            }
                        }
                    }
                    else
                        lst = new List<TblWorkFlowNode>() { node };

                    allWorkflowNode[idWorkflow] = lst;
                }
                else
                    allWorkflowNode.Add(idWorkflow, new List<TblWorkFlowNode>() { node });
            }
            else
            {
                allWorkflowNode = new Dictionary<long, List<TblWorkFlowNode>>();
                allWorkflowNode.Add(idWorkflow, new List<TblWorkFlowNode>() { node });
            }
            TblWorkFlowNodeConverted = allWorkflowNode;
        }

        public static TblWorkFlowNode FindTblWorkFlowNode(int idInXML, int idWorkflow)
        {
            Dictionary<long, List<TblWorkFlowNode>> allWorkflowNode = TblWorkFlowNodeConverted;
            if (allWorkflowNode != null && allWorkflowNode.Count > 0)
            {
                if (allWorkflowNode.ContainsKey(idWorkflow))
                {
                    foreach (TblWorkFlowNode item in allWorkflowNode[idWorkflow])
                    {
                        if (item.WorkFlowIDInXML.HasValue && item.WorkFlowIDInXML.Value == idInXML)
                        {
                            return item;
                        }
                    }

                    if (idWorkflow == 0)
                        return null;
                    TblWorkFlowNode node = WorkFlowNodeManager.GetExtractWorkFlowNodeByWorkflow(idInXML, idWorkflow);
                    if (node != null)
                    {
                        allWorkflowNode[idWorkflow].Add(node);
                        TblWorkFlowNodeConverted = allWorkflowNode;
                        return node;
                    }
                }
            }

            allWorkflowNode = new Dictionary<long, List<TblWorkFlowNode>>();

            if (idWorkflow == 0)
                return null;
            TblWorkFlowNode node1 = WorkFlowNodeManager.GetExtractWorkFlowNodeByWorkflow(idInXML, idWorkflow);
            if (node1 != null)
            {
                allWorkflowNode.Add(idWorkflow, new List<TblWorkFlowNode>() { node1 });
                TblWorkFlowNodeConverted = allWorkflowNode;
                return node1;
            }

            return null;
        }


        public static TblWorkFlowNode FindTblWorkFlowNodeInDB(int idInDatabase, int idWorkflow)
        {
            Dictionary<long, List<TblWorkFlowNode>> allWorkflowNode = TblWorkFlowNodeConverted;
            if (allWorkflowNode != null && allWorkflowNode.Count > 0)
            {
                if (allWorkflowNode.ContainsKey(idWorkflow))
                {
                    foreach (TblWorkFlowNode item in allWorkflowNode[idWorkflow])
                    {
                        if (item.Id == idInDatabase)
                        {
                            return item;
                        }
                    }

                    if (idWorkflow == 0)
                        return null;
                    TblWorkFlowNode node = WorkFlowNodeManager.GetWorkFlowNodeByID(idInDatabase);
                    if (node != null)
                    {
                        allWorkflowNode[idWorkflow].Add(node);
                        TblWorkFlowNodeConverted = allWorkflowNode;
                        return node;
                    }
                }
            }

            allWorkflowNode = new Dictionary<long, List<TblWorkFlowNode>>();

            if (idWorkflow == 0)
                return null;
            TblWorkFlowNode node1 = WorkFlowNodeManager.GetWorkFlowNodeByID(idInDatabase);
            if (node1 != null)
            {
                allWorkflowNode.Add(idWorkflow, new List<TblWorkFlowNode>() { node1 });
                TblWorkFlowNodeConverted = allWorkflowNode;
                return node1;
            }

            return null;
        }


        public static long FindWorkflow(string WorkflowCodeAndidMachineryProducetype)
        {
            Dictionary<int, string> workflowCollection = WorkflowCollection;
            if (workflowCollection == null || workflowCollection.Count == 0)
            {
                workflowCollection = new Dictionary<int, string>();
                string[] arr = WorkflowCodeAndidMachineryProducetype.Split('*');
                if (arr.Length != 2)
                    return 0;
                try
                {
                    TblWorkFlow wf = WorkFlowManager.GetWorkFlowByID(long.Parse(arr[1]));

                    if (wf != null)
                    {
                        //SaveWorkflowInSession(wf.Id, WorkflowCodeAndidMachineryProducetype);
                        SaveWorkFlowNode(wf.ContentXML, wf.Id);
                        if (workflowCollection.ContainsKey(wf.Id))
                            workflowCollection[wf.Id] = WorkflowCodeAndidMachineryProducetype;
                        else
                            workflowCollection.Add(wf.Id, WorkflowCodeAndidMachineryProducetype);
                        WorkflowCollection = workflowCollection;
                        return wf.Id;
                    }
                    else
                        return 0;
                }
                catch
                {

                }
            }
            else
            {
                foreach (KeyValuePair<int, string> item in workflowCollection)
                {
                    if (item.Value.ToLower() == WorkflowCodeAndidMachineryProducetype.ToLower())
                        return item.Key;
                }
                string[] arr = WorkflowCodeAndidMachineryProducetype.Split('*');
                if (arr.Length != 2)
                    return 0;
                TblWorkFlow wf = WorkFlowManager.GetWorkFlowByID(long.Parse(arr[1]));
                if (wf != null)
                {
                    //SaveWorkflowInSession(wf.Id, WorkflowCodeAndidMachineryProducetype);
                    SaveWorkFlowNode(wf.ContentXML, wf.Id);
                    if (workflowCollection.ContainsKey(wf.Id))
                        workflowCollection[wf.Id] = WorkflowCodeAndidMachineryProducetype;
                    else
                        workflowCollection.Add(wf.Id, WorkflowCodeAndidMachineryProducetype);
                    WorkflowCollection = workflowCollection;
                    return wf.Id;
                }
                else
                    return 0;
            }
            return 0;
        }


        public static void RemoveWorkflowInSession(string idWorkflowString, string WorkflowCodeAndidWorkflow)
        {
            if (!string.IsNullOrEmpty(idWorkflowString))
            {
                int id = 0;
                int.TryParse(idWorkflowString, out id);
                if (id > 0)
                    RemoveWorkflowInSession(id, WorkflowCodeAndidWorkflow);
            }
        }

        public static void RemoveWorkflowInSession(int idWorkflow, string WorkflowCodeAndidWorkflow)
        {
            Dictionary<int, string> workflowCollection = WorkflowCollection;
            if (workflowCollection == null)
                workflowCollection = new Dictionary<int, string>();

            if (workflowCollection.ContainsKey(idWorkflow))
            {
                workflowCollection.Remove(idWorkflow);
                WorkflowCollection = workflowCollection;
            }

            Dictionary<long, List<TblWorkFlowNode>> allWorkflowNode = TblWorkFlowNodeConverted;
            if (allWorkflowNode != null && allWorkflowNode.Count > 0)
            {
                if (allWorkflowNode.ContainsKey(idWorkflow))
                {
                    allWorkflowNode.Remove(idWorkflow);
                    TblWorkFlowNodeConverted = allWorkflowNode;
                }
            }
        }

        public static void SaveWorkflowInSession(string idWorkflowString, string WorkflowCodeAndidWorkflow)
        {
            if (!string.IsNullOrEmpty(idWorkflowString))
            {
                int id = 0;
                int.TryParse(idWorkflowString, out id);
                if (id > 0)
                    SaveWorkflowInSession(id, WorkflowCodeAndidWorkflow);
            }
        }

        public static void SaveWorkflowInSession(int idWorkflow, string WorkflowCodeAndidWorkflow)
        {
            Dictionary<int, string> workflowCollection = WorkflowCollection;
            if (workflowCollection == null)
                workflowCollection = new Dictionary<int, string>();

            if (workflowCollection.ContainsKey(idWorkflow))
                workflowCollection[idWorkflow] = WorkflowCodeAndidWorkflow;
            else
                workflowCollection.Add(idWorkflow, WorkflowCodeAndidWorkflow);

            WorkflowCollection = workflowCollection;
        }

        /// <summary>
        /// Update all WorkFlow Node getted from content XML to session
        /// </summary>
        /// <param name="contentXML">string of Xml graph</param>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        public static void UpdateWorkFlowNode(string contentXML, int idWorkflow)
        {
            CurrentProcessWorkFlowID = idWorkflow;
            Dictionary<long, List<WorkflowNode>> designCollection = DesignCollection;
            if (designCollection == null)
                designCollection = new Dictionary<long, List<WorkflowNode>>();

            if (designCollection.ContainsKey(idWorkflow))
                designCollection.Remove(idWorkflow);

            SaveWorkFlowNode(contentXML, idWorkflow);
        }

        /// <summary>
        /// Get all Workflow Node from [worlflow code and idWorkflow].
        /// </summary>
        /// <param name="WorkflowCodeAndidWorkflow">string of [workflow code and idWorkflow] connect by "*" character</param>
        /// <returns>List of WorkflowNode object</returns>
        public static List<WorkflowNode> GetAllWorkflowNode(int idWorkflow)
        {
            if (idWorkflow == 0)
                return null;

            try
            {
                CurrentProcessWorkFlowID = idWorkflow;
                Dictionary<long, List<WorkflowNode>> designCollection = DesignCollection;
                if (designCollection == null)
                    designCollection = new Dictionary<long, List<WorkflowNode>>();
                if (designCollection.ContainsKey(idWorkflow))
                {
                    return designCollection[idWorkflow];
                }
                else
                {
                    lstWorkFlowNode = new List<WorkflowNode>();
                    lstMxCellNode = null;
                    #region RegionName

                    TblWorkFlow wf = WorkFlowManager.GetWorkFlowByID(idWorkflow);
                    if (wf != null)
                    {
                        SaveWorkflowInSession(wf.Id, wf.Code + "*" + wf.Id);
                        SaveWorkFlowNode(wf.ContentXML, idWorkflow);
                    }
                    #endregion
                }
            }
            catch
            {

            }

            return lstWorkFlowNode;
        }

        public static void ResetWorkflowSession(int idWorkflow)
        {
            if (idWorkflow > 0)
            {
                Dictionary<long, List<WorkflowNode>> designCollection = DesignCollection;
                if (designCollection == null)
                    designCollection = new Dictionary<long, List<WorkflowNode>>();

                if (designCollection.ContainsKey(idWorkflow))
                {
                    designCollection.Remove(idWorkflow);
                    DesignCollection = designCollection;
                }
            }
        }

        #endregion

        #region Get next Collection

        public static List<WorkflowNode> GetNextCollection(int idWorkflow, int id)
        {
            return GetToCollection(idWorkflow, id);
        }

        public static List<WorkflowNode> GetNextCollection(int idWorkflow, List<int> toColletion)
        {
            return GetToCollection(idWorkflow, toColletion);
        }

        public static List<WorkflowNode> GetNextCollection(int idWorkflow, WorkflowNode item)
        {
            return GetToCollection(idWorkflow, item);
        }

        #endregion

        #region Get assign Collection

        public static List<WorkflowNode> GetAssignCollection(int idWorkflow, int id)
        {
            return GetFromCollection(idWorkflow, id);
        }

        public static List<WorkflowNode> GetAssignCollection(int idWorkflow, List<int> fromCollection)
        {
            return GetFromCollection(idWorkflow, fromCollection);
        }

        public static List<WorkflowNode> GetAssignCollection(int idWorkflow, WorkflowNode item)
        {
            return GetFromCollection(idWorkflow, item);
        }

        #endregion

        #region Get first dept

        public static WorkflowNode GetFirstDept(int idWorkflow)
        {
            List<WorkflowNode> lst = GetAllWorkflowNode(idWorkflow);
            if (lst != null && lst.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lst)
                {
                    if (workflowNode.IsEnd == false && workflowNode.IsRoot == true)
                    {
                        List<WorkflowNode> lstToCollection = GetToCollection(idWorkflow, workflowNode);
                        if (lstToCollection != null && lstToCollection.Count > 0)
                        {
                            foreach (WorkflowNode item in lstToCollection)
                            {
                                if (item.Type == WorkflowNodeType.Xuong)
                                    return item;
                            }
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Get first work

        public static WorkflowNode GetFirstWork(int idWorkflow)
        {
            List<WorkflowNode> lst = GetAllWorkflowNode(idWorkflow);
            if (lst != null && lst.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lst)
                {
                    if (workflowNode.IsEnd == false && workflowNode.IsRoot == true)
                    {
                        List<WorkflowNode> lstToCollection = GetToCollection(idWorkflow, workflowNode);
                        if (lstToCollection != null && lstToCollection.Count > 0)
                        {
                            foreach (WorkflowNode item in lstToCollection)
                            {
                                if (item.Type == WorkflowNodeType.CongViecChinh || item.Type == WorkflowNodeType.CongViecPhu)
                                    return item;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static WorkflowNode GetFirstWorkByTruc(int idWorkflow)
        {
            WorkflowNode firstDept = GetFirstDept(idWorkflow);
            if (firstDept != null)
                return GetFirstWork(idWorkflow);

            return null;
        }

        #endregion

        #region Get Extract Node by id

        public static WorkflowNode GetExtractNode(int idWorkflow, int idDept, List<int> fromCollection, WorkflowNodeType type)
        {
            List<WorkflowNode> lstAll = GetWorkflowNodeByIdMapFromDatabase(idWorkflow, idDept, type);
            if (lstAll != null && lstAll.Count > 0)
            {
                foreach (WorkflowNode item in lstAll)
                {
                    if (fromCollection == null && item.fromConnectCollection == null)
                        return item;
                    else
                    {
                        if (fromCollection.Count == item.fromConnectCollection.Count)
                        {
                            bool found = true;
                            for (int i = 0; i < fromCollection.Count; i++)
                            {
                                if (!item.fromConnectCollection.Contains(fromCollection[i]))
                                {
                                    found = false;
                                    break;
                                }
                            }
                            if (found == true)
                                return item;
                        }
                    }
                }
            }
            return null;
        }

        public static WorkflowNode GetExtractNode(int idWorkflow, string idDeptString, List<int> fromCollection, WorkflowNodeType type)
        {
            int id = 0;
            int.TryParse(idDeptString, out id);
            if (id > 0)
                return GetExtractNode(idWorkflow, id, fromCollection, type);
            else
                return null;
        }

        #endregion

        #region Get all node by type

        public static List<WorkflowNode> GetAllWorkFlowNodeByType(int idWorkflow, WorkflowNodeType type)
        {
            List<WorkflowNode> lst = new List<WorkflowNode>();
            List<WorkflowNode> lstAll = GetAllWorkflowNode(idWorkflow);
            if (lstAll != null && lstAll.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lstAll)
                {
                    if (workflowNode.Type == type)
                    {
                        bool isExist = false;
                        foreach (WorkflowNode item in lst)
                        {
                            if (item.Id == workflowNode.Id)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)
                            lst.Add(workflowNode);
                    }
                }
            }
            return lst;
        }

        #endregion

        #region Get ToWork By Id Map From Database

        /*
        public static List<WorkflowNode> GetToWorkByIdMapFromDatabase(string idDeptString, List<int> fromCollection, WorkflowNodeType type)
        {
            int id = 0;
            int.TryParse(idDeptString, out id);
            if (id > 0)
                return GetToWorkByIdMapFromDatabase(CurrentProcessWorkFlowID, id, fromCollection, type);
            else
                return null;
        }

        public static List<WorkflowNode> GetToWorkByIdMapFromDatabase(int idDept, List<int> fromCollection, WorkflowNodeType type)
        {
            return GetToWorkByIdMapFromDatabase(CurrentProcessWorkFlowID, idDept, fromCollection, type);
        }

        public static List<WorkflowNode> GetToWorkByIdMapFromDatabase( int idWorkflow, int idDept, List<int> fromCollection, WorkflowNodeType type)
        {
            return GetToWorkByIdMapFromDatabase( idWorkflow, idDept, fromCollection, type);
        }

        public static List<WorkflowNode> GetToWorkByIdMapFromDatabase( int idWorkflow, string idDeptString, List<int> fromCollection, WorkflowNodeType type)
        {
            int id = 0;
            int.TryParse(idDeptString, out id);
            if (id > 0)
                return GetToWorkByIdMapFromDatabase( idWorkflow, id, fromCollection, type);
            else
                return null;
        }        

        public static List<WorkflowNode> GetToWorkByIdMapFromDatabase(string WorkflowCodeAndidWorkflow, string idDeptString, List<int> fromCollection, WorkflowNodeType type)
        {
            int id = 0;
            int.TryParse(idDeptString, out id);
            if (id > 0)
                return GetToWorkByIdMapFromDatabase(WorkflowCodeAndidWorkflow, id, fromCollection, type);
            else
                return null;
        }
        
        public static List<WorkflowNode> GetToWorkByIdMapFromDatabase(string WorkflowCodeAndidWorkflow, int idDept, List<int> fromCollection, WorkflowNodeType type)
        {
            WorkflowNode node = GetExtractNode(WorkflowCodeAndidWorkflow, idDept, fromCollection, type);
            if (node != null)
                return node.GetToConnectCollection();
            return null;
        }
        */

        #endregion

        #region For manager

        #region Get first Dept

        public static List<WorkflowNode> GetFirstTblDept(int idWorkflow)
        {
            List<WorkflowNode> lst = GetAllWorkflowNode(idWorkflow);
            if (lst != null && lst.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lst)
                {
                    if (workflowNode.IsEnd == false && workflowNode.IsRoot == true)
                        return GetToCollection(idWorkflow, workflowNode);
                }
            }
            return null;
        }

        #endregion


        #region Get first Tblworktask

        public static List<WorkflowNode> GetFirstWorkTask(int idWorkflow)
        {
            List<WorkflowNode> lst = GetAllWorkflowNode(idWorkflow);
            if (lst != null && lst.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lst)
                {
                    if (workflowNode.IsEnd == false && workflowNode.IsRoot == true)
                        return GetToCollection(idWorkflow, workflowNode);
                }
            }
            return new List<WorkflowNode>();
        }

        #endregion

        #region Get first worktask beetween depts

        public static List<WorkflowNode> GetFirstWorkTaskBetWeenDepts(int idWorkflow, int idStart, int idEnd)
        {
            return GetFirstWorkTaskBetWeenDepts(idWorkflow, idStart.ToString(), idEnd.ToString());
        }

        public static List<WorkflowNode> GetFirstWorkTaskBetWeenDepts(int idWorkflow, string idStart, string idEnd)
        {
            string start = string.Empty;
            string end = string.Empty;
            List<WorkflowNode> lstNode = GetAllWorkFlowNodeByType(idWorkflow, WorkflowNodeType.Xuong);
            if (lstNode != null && lstNode.Count > 0)
            {
                foreach (WorkflowNode item in lstNode)
                {
                    if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                    {
                        if (item.TblDepartment != null && item.TblDepartment.DepartmentID.ToString() == idStart)
                            start = item.DataGraphId;
                        else if (item.TblDepartment != null && item.TblDepartment.DepartmentID.ToString() == idEnd)
                            end = item.DataGraphId;
                    }
                    else
                        break;
                }
            }

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                return GetFirstWorkTask(idWorkflow);
            else
                return new List<WorkflowNode>();
        }

        #endregion

        #region Get Next dept Collection

        /// <summary>
        /// Get list of Tbldept for next process
        /// </summary>
        /// <param name="idWorkflow">id workflow</param>
        /// <param name="id">dept id</param>
        /// <param name="stringFromCollection">Comma separated value of type, eg: 1,2,3</param>
        /// <returns>List of Tbldept object</returns>
        public static List<WorkflowNode> GetNextTblDepts(int idWorkflow, int id, string stringFromCollection)
        {
            try
            {
                List<int> numbers = stringFromCollection.Split(',').Select(n => int.Parse(n)).ToList();
                return GetNextTblDepts(idWorkflow, id, numbers);
            }
            catch (Exception ex)
            {
                return new List<WorkflowNode>();
            }
        }

        /// <summary>
        /// Get list of Tbldept for next process
        /// </summary>
        /// <param name="workflowCode">workflowCode string</param>
        /// <param name="idWorkflow">id workflow</param>
        /// <param name="id">dept id</param>
        /// <param name="toCollection">List of id from</param>
        /// <returns>List of Tbldept object</returns>
        public static List<WorkflowNode> GetNextTblDepts(int idWorkflow, int id, List<int> fromCollection)
        {
            WorkflowNode node = GetExtractWorkflowNodeByIdMapFromDatabase(idWorkflow, id, WorkflowNodeType.Xuong, fromCollection);
            if (node != null)
                return GetNextTblDepts(idWorkflow, node);
            else
                return new List<WorkflowNode>();
        }

        public static List<WorkflowNode> GetNextTblDepts(int idWorkflow, WorkflowNode item)
        {
            if (item == null || idWorkflow == 0)
                return null;

            if (item.fromConnectCollection != null && item.fromConnectCollection.Count > 0)
            {
                lstWorkFlowNode = GetAllWorkflowNode(idWorkflow);
                if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                {
                    List<WorkflowNode> lst = new List<WorkflowNode>();
                    foreach (int id in item.toConnectCollection)
                        lst.Add(GetWorkflowNode(idWorkflow, id));
                    if (lst != null && lst.Count > 0)
                        return lst;
                }
            }
            return new List<WorkflowNode>();
        }

        #endregion

        #region get current active node
        /*
        public static WorkflowNode GetCurrentActiveNode(int idWorkflow, int idMap, List<int> fromCollection)
        {
            WorkflowNode node = null;
            if (workflowCode.ToLower() == "main-work")
                node = GetExtractWorkflowNodeByIdMapFromDatabase(idWorkflow, idMap, WorkflowNodeType.Xuong, fromCollection);
            else
            {
                node = GetExtractWorkflowNodeByIdMapFromDatabase(idWorkflow, idMap, WorkflowNodeType.CongViecChinh, fromCollection);
                if (node == null)
                    node = GetExtractWorkflowNodeByIdMapFromDatabase(idWorkflow, idMap, WorkflowNodeType.CongViecPhu, fromCollection);
            }

            return node;
        }
        */
        #endregion

        #region TblWorkFlowNode

        public static TblWorkFlowLine ConvertToTblWorkFlowLine(WorkflowNode item)
        {
            TblWorkFlowLine line = WorkFlowLineManager.GetExtractWorkFlowLineByIdInXML(item.Id.ToString(), item.idWorkflow);
            if (line == null)
            {
                line = new TblWorkFlowLine();
                line.WorkFlowID = item.idWorkflow;
                line.Node1ID = item.IdStart;
                line.Node2ID = item.IdEnd;
                //line.idWorkflowTo
                //line.idWorkflow
                line.LineType = item.linetype;
                line.WorkFlowCode = item.WorkflowCode;
                line.MachineryProduceTypeID = item.IdMachineryProducetype;
                line.WorkFlowIDInXML = item.Id;
            }
            return line;
        }

        public static TblWorkFlowNode ConvertToTblWorkFlowNode(WorkflowNode item)
        {
            TblWorkFlowNode node = WorkFlowNodeManager.GetExtractWorkFlowNodeByWorkflow(item.Id, item.idWorkflow);
            if (node == null)
            {
                node = new TblWorkFlowNode();
                node.WorkFlowID = item.idWorkflow;
                if (item.TblDepartment != null)
                    node.DepartmentID = item.TblDepartment.DepartmentID;
                node.WorkFlowID = item.Id;
                if (item.TblWorkTask != null)
                    node.WorkTaskID = item.TblWorkTask.Id;
                node.NodeType = item.Type.ToString();
                node.WorkFlowCode = item.WorkflowCode;
                node.WorkFlowDataGraphID = item.DataGraphId;
                node.MachineryProduceTypeID = item.IdMachineryProducetype;
                node.WorkFlowIsConnectItseft = item.IsConnectItSelf;
                node.WorkFlowIsSend = item.IsEnd;
                node.WorkFlowIsRoot = item.IsRoot;
                node.WorkFlowIDInXML = item.Id;
                node.WorkFlowListFromConnection = item.StringFromConnection;
                node.WorkFlowListToConnection = item.StringToConnection;
            }
            return node;
        }

        public static TblWorkFlowNodeCollection GetAllTblWorkFlowNodeFromDesign(int idWorkflow)
        {
            TblWorkFlowNodeCollection lstReturn = new TblWorkFlowNodeCollection();
            List<WorkflowNode> lst = GetAllWorkflowNode(idWorkflow);
            if (lst != null && lst.Count > 0)
            {
                foreach (WorkflowNode item in lst)
                    lstReturn.Add(ConvertToTblWorkFlowNode(item));
            }
            return lstReturn;
        }

        public static TblWorkFlowNodeCollection GetAllTblWorkFlowNode(int idWorkflow, List<int> idcollection)
        {
            TblWorkFlowNodeCollection lstReturn = new TblWorkFlowNodeCollection();
            if (idcollection != null && idcollection.Count > 0)
            {
                foreach (int item in idcollection)
                {
                    WorkflowNode node = GetWorkflowNode(idWorkflow, item);
                    if (node != null)
                        lstReturn.Add(ConvertToTblWorkFlowNode(node));
                }
            }
            return lstReturn;
        }

        public static TblWorkFlowNode GetTblWorkFlowNodeByIdFromDesign(int idWorkflow, string idxmlNode)
        {
            WorkflowNode node = GetWorkflowNode(idWorkflow, idxmlNode);
            if (node != null)
                return ConvertToTblWorkFlowNode(node);
            else
                return null;
        }

        public static TblWorkFlowNodeCollection ConvertToTblWorkFlowNode(List<WorkflowNode> lstItem)
        {
            TblWorkFlowNodeCollection lstReturn = new TblWorkFlowNodeCollection();
            if (lstItem != null && lstItem.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lstItem)
                    lstReturn.Add(ConvertToTblWorkFlowNode(workflowNode));
            }
            return lstReturn;
        }

        public static TblWorkFlowLineCollection ConvertToTblWorkFlowLine(List<WorkflowNode> lstItem)
        {
            TblWorkFlowLineCollection lstReturn = new TblWorkFlowLineCollection();
            if (lstItem != null && lstItem.Count > 0)
            {
                foreach (WorkflowNode workflowNode in lstItem)
                    lstReturn.Add(ConvertToTblWorkFlowLine(workflowNode));
            }
            return lstReturn;
        }

        #endregion

        #endregion

        
        public static void SaveWorkFlowLine(string contentXML, int idWorkflow)
        {
            if (idWorkflow == 0)
                return;

            CurrentProcessWorkFlowID = idWorkflow;
            Dictionary<long, List<WorkflowNode>> lineInDesignCollection = LineInDesignCollection;
            if (lineInDesignCollection == null)
                lineInDesignCollection = new Dictionary<long, List<WorkflowNode>>();

            if (lineInDesignCollection.ContainsKey(idWorkflow))
                return;

            HtmlDocument xdoc = new HtmlDocument();
            xdoc.LoadHtml(contentXML);
            lstMxCellNode = xdoc.DocumentNode.SelectNodes("descendant::*[translate(name(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='mxcell']");
            if (lstWorkFlowNode == null)
                lstWorkFlowNode = new List<WorkflowNode>();
            dicMap = new Dictionary<int, List<HtmlNode>>();

            if (lstMxCellNode != null && lstMxCellNode.Count > 0)
            {
                List<HtmlNode> edgeColl = lstMxCellNode.Where(o => o.Attributes["edge"] != null && o.Attributes["edge"].Value == "1").ToList();
                if (edgeColl != null && edgeColl.Count > 0)
                {

                    foreach (HtmlNode item in edgeColl)
                        lstWorkFlowNode.Add(ConvertToWorkflowNode(item, idWorkflow));

                    if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                    {
                        foreach (WorkflowNode workflowNode in lstWorkFlowNode)
                            MapConnect(workflowNode, idWorkflow);

                        lineInDesignCollection.Add(idWorkflow, lstWorkFlowNode);
                        LineInDesignCollection = lineInDesignCollection;

                        #region RegionName

                        TblWorkFlowLineCollection allConvert = ConvertToTblWorkFlowLine(lstWorkFlowNode);
                        Dictionary<long, List<TblWorkFlowLine>> allWorkflowLine = TblWorkFlowLineConverted;
                        if (allWorkflowLine == null || allWorkflowLine.Count == 0)
                            allWorkflowLine = new Dictionary<long, List<TblWorkFlowLine>>();
                        if (allConvert != null && allConvert.Count > 0)
                        {
                            if (allWorkflowLine.ContainsKey(idWorkflow))
                            {
                                List<TblWorkFlowLine> lst = allWorkflowLine[idWorkflow];
                                if (lst != null)
                                {
                                    if (lst.Count > 0)
                                    {
                                        foreach (TblWorkFlowLine line in allConvert)
                                        {
                                            bool found = false;
                                            foreach (TblWorkFlowLine item in lst)
                                            {
                                                if (item.Id == line.Id)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found == false)
                                                lst.Add(line);
                                        }
                                    }
                                    else
                                        lst.AddRange(allConvert.ToList());
                                }
                                else
                                    lst = allConvert.ToList();
                                allWorkflowLine[idWorkflow] = lst;
                            }
                            else
                                allWorkflowLine.Add(idWorkflow, allConvert.ToList());
                        }
                        TblWorkFlowLineConverted = allWorkflowLine;

                        #endregion

                    }
                }
            }
        }

        /// <summary>
        /// Get all WorkFlow Node getted from content XML
        /// </summary>
        /// <param name="contentXML">string of Xml graph</param>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        public static TblWorkFlowNodeCollection ExtractWorkFlowNodeFromDesign(string contentXML, int idWorkflow)
        {
            if (idWorkflow == 0)
                return null;
            CurrentProcessWorkFlowID = idWorkflow;
            HtmlDocument xdoc = new HtmlDocument();
            xdoc.LoadHtml(contentXML);
            lstMxCellNode = xdoc.DocumentNode.SelectNodes("descendant::*[translate(name(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='mxcell']");

            //reset for load
            lstWorkFlowNode = new List<WorkflowNode>();
            dicMap = new Dictionary<int, List<HtmlNode>>();

            if (lstMxCellNode != null && lstMxCellNode.Count > 0)
            {
                List<HtmlNode> vertexColl = lstMxCellNode.Where(o => o.Attributes["vertex"] != null && o.Attributes["vertex"].Value == "1").ToList();
                if (vertexColl != null && vertexColl.Count > 0)
                {

                    foreach (HtmlNode item in vertexColl)
                        lstWorkFlowNode.Add(ConvertToWorkflowNode(item, idWorkflow));

                    if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                    {
                        foreach (WorkflowNode workflowNode in lstWorkFlowNode)
                            MapConnect(workflowNode, idWorkflow);

                        TblWorkFlowNodeCollection allConvert = ConvertToTblWorkFlowNode(lstWorkFlowNode);
                        Dictionary<long, List<TblWorkFlowNode>> allWorkflowNode = TblWorkFlowNodeConverted;
                        if (allWorkflowNode == null || allWorkflowNode.Count == 0)
                            allWorkflowNode = new Dictionary<long, List<TblWorkFlowNode>>();
                        if (allConvert != null && allConvert.Count > 0)
                        {
                            if (allWorkflowNode.ContainsKey(idWorkflow))
                            {
                                List<TblWorkFlowNode> lst = allWorkflowNode[idWorkflow];
                                if (lst != null)
                                {
                                    if (lst.Count > 0)
                                    {
                                        foreach (TblWorkFlowNode node in allConvert)
                                        {
                                            bool found = false;
                                            foreach (TblWorkFlowNode item in lst)
                                            {
                                                if (item.Id == node.Id)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found == false)
                                                lst.Add(node);
                                        }
                                    }
                                    else
                                        lst.AddRange(allConvert.ToList());
                                }
                                else
                                    lst = allConvert.ToList();
                                allWorkflowNode[idWorkflow] = lst;
                            }
                            else
                                allWorkflowNode.Add(idWorkflow, allConvert.ToList());
                        }
                        TblWorkFlowNodeConverted = allWorkflowNode;

                        return allConvert;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Get all WorkFlow Line getted from content XML
        /// </summary>
        /// <param name="contentXML">string of Xml graph</param>
        /// <param name="WorkflowCodeAndidWorkflow">string of workflow code and idWorkflow connect by "*" character</param>
        public static TblWorkFlowLineCollection ExtractWorkFlowLineFromDesign(string contentXML, int idWorkflow)
        {
            if (idWorkflow == 0)
                return null;
            CurrentProcessWorkFlowID = idWorkflow;
            HtmlDocument xdoc = new HtmlDocument();
            xdoc.LoadHtml(contentXML);
            lstMxCellNode = xdoc.DocumentNode.SelectNodes("descendant::*[translate(name(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='mxcell']");

            //reset for load
            lstWorkFlowNode = new List<WorkflowNode>();
            dicMap = new Dictionary<int, List<HtmlNode>>();

            if (lstMxCellNode != null && lstMxCellNode.Count > 0)
            {
                List<HtmlNode> edgeColl = lstMxCellNode.Where(o => o.Attributes["edge"] != null && o.Attributes["edge"].Value == "1").ToList();
                if (edgeColl != null && edgeColl.Count > 0)
                {

                    foreach (HtmlNode item in edgeColl)
                        lstWorkFlowNode.Add(ConvertToWorkflowNode(item, idWorkflow));

                    if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                    {
                        foreach (WorkflowNode workflowNode in lstWorkFlowNode)
                            MapConnect(workflowNode, idWorkflow);

                        TblWorkFlowLineCollection allConvert = ConvertToTblWorkFlowLine(lstWorkFlowNode);
                        Dictionary<long, List<TblWorkFlowLine>> allWorkflowLine = TblWorkFlowLineConverted;
                        if (allWorkflowLine == null || allWorkflowLine.Count == 0)
                            allWorkflowLine = new Dictionary<long, List<TblWorkFlowLine>>();
                        if (allConvert != null && allConvert.Count > 0)
                        {
                            if (allWorkflowLine.ContainsKey(idWorkflow))
                            {
                                List<TblWorkFlowLine> lst = allWorkflowLine[idWorkflow];
                                if (lst != null)
                                {
                                    if (lst.Count > 0)
                                    {
                                        foreach (TblWorkFlowLine node in allConvert)
                                        {
                                            bool found = false;
                                            foreach (TblWorkFlowLine item in lst)
                                            {
                                                if (item.Id == node.Id)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found == false)
                                                lst.Add(node);
                                        }
                                    }
                                    else
                                        lst.AddRange(allConvert.ToList());
                                }
                                else
                                    lst = allConvert.ToList();
                                allWorkflowLine[idWorkflow] = lst;
                            }
                            else
                                allWorkflowLine.Add(idWorkflow, allConvert.ToList());
                        }
                        TblWorkFlowLineConverted = allWorkflowLine;

                        return allConvert;
                    }
                }
            }

            return null;
        }


        public static List<WorkflowNode> GetAllWorkFlowLine(string contentXML, int idWorkflow)
        {
            if (idWorkflow == 0)
                return null;

            CurrentProcessWorkFlowID = idWorkflow;
            Dictionary<long, List<WorkflowNode>> lineInDesignCollection = LineInDesignCollection;
            if (lineInDesignCollection == null)
                lineInDesignCollection = new Dictionary<long, List<WorkflowNode>>();

            if (lineInDesignCollection.ContainsKey(idWorkflow))
                return lineInDesignCollection[idWorkflow];

            HtmlDocument xdoc = new HtmlDocument();
            xdoc.LoadHtml(contentXML);
            lstMxCellNode = xdoc.DocumentNode.SelectNodes("descendant::*[translate(name(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='mxcell']");

            lstWorkFlowNode = new List<WorkflowNode>();
            dicMap = new Dictionary<int, List<HtmlNode>>();

            if (lstMxCellNode != null && lstMxCellNode.Count > 0)
            {
                List<HtmlNode> edgeColl = lstMxCellNode.Where(o => o.Attributes["edge"] != null && o.Attributes["edge"].Value == "1").ToList();
                if (edgeColl != null && edgeColl.Count > 0)
                {

                    foreach (HtmlNode item in edgeColl)
                        lstWorkFlowNode.Add(ConvertToWorkflowNode(item, idWorkflow));

                    if (lstWorkFlowNode != null && lstWorkFlowNode.Count > 0)
                    {
                        foreach (WorkflowNode workflowNode in lstWorkFlowNode)
                            MapConnect(workflowNode, idWorkflow);

                        lineInDesignCollection.Add(idWorkflow, lstWorkFlowNode);
                        LineInDesignCollection = lineInDesignCollection;

                        return lstWorkFlowNode;
                    }
                }
            }

            return null;
        }


    }


    public enum WorkflowNodeType
    {
        DauVaoHoacDauRa,//đầu vào hoặc đầu ra
        Xuong,//Xưởng = phòng ban
        CongViecChinh,//Công việc chính
        CongViecPhu//Công việc phụ
    }

    public partial class WorkflowNode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsEnd { get; set; }
        public bool IsRoot { get; set; }
        public bool IsConnectItSelf { get; set; }
        public string DataGraphId { get; set; }
        public WorkflowNodeType Type { get; set; }
        public TblDepartment TblDepartment { get; set; }
        public TblWorkTask TblWorkTask { get; set; }
        public int IdStart { get; set; }
        public int IdEnd { get; set; }
        public int IdMachineryProducetype { get; set; }
        public long workflowToId { get; set; }
        public int idWorkflow { get; set; }
        public string linetype { get; set; }
        public string WorkflowCode { get; set; }

        public string StringFromConnection
        {
            get
            {
                if (fromConnectCollection != null && fromConnectCollection.Count > 0)
                    return string.Join(",", fromConnectCollection.Select(x => x.ToString()).ToArray());
                else
                    return string.Empty;
            }
        }

        public string StringToConnection
        {
            get
            {
                if (toConnectCollection != null && toConnectCollection.Count > 0)
                    return string.Join(",", toConnectCollection.Select(x => x.ToString()).ToArray());
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Return list of WorkflowNode from property [fromConnectCollection].
        /// </summary>
        public List<WorkflowNode> GetFromConnectCollection()
        {
            return WorkflowNodeHelper.GetAssignCollection(idWorkflow, this);
        }


        public List<int> fromConnectCollection { get; set; }


        /// <summary>
        /// Return list of WorkflowNode from property [toConnectCollection].
        /// </summary>
        public List<WorkflowNode> GetToConnectCollection()
        {
            return WorkflowNodeHelper.GetNextCollection(idWorkflow, this);
        }


        public List<int> toConnectCollection { get; set; }

    }

    #endregion
}