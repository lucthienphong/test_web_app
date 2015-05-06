using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    /// <summary>
    /// Summary description for SubworkHelper
    /// </summary>
    public class SubworkHelper
    {
        public static bool SaveSubworkMapping(int idWorkflow, string dataCells)
        {
            ObjectSubwork[] arrCell = JsonConvert.DeserializeObject<ObjectSubwork[]>(dataCells);
            if (arrCell != null && arrCell.Length > 0)
            {
                int id = 0;
                foreach (ObjectSubwork item in arrCell)
                {
                    int.TryParse(item.id, out id);
                    TblWorkFlowNode node = WorkflowNodeHelper.FindTblWorkFlowNode(id, idWorkflow);
                    if (node != null)
                    {
                        #region process

                        TblWorkTaskInNodeCollection allMapped = WorkFlowNodeManager.GetWorktaskBySubwork(node.Id);
                        if (allMapped != null && allMapped.Count > 0)
                        {
                            if (allMapped.Count >= item.datavalue.arr.Count)
                            {
                                int del = allMapped.Count - item.datavalue.arr.Count;
                                for (int i = 0; i < del; i++)
                                    WorkFlowNodeManager.Deleteworktaskinnode(allMapped[i].Id);
                                for (int i = 0; i < item.datavalue.arr.Count; i++)
                                {
                                    TblWorkTaskInNode worktaskinnode = allMapped[i];
                                    worktaskinnode.WorkTaskID = item.datavalue.arr[i];
                                    WorkFlowNodeManager.Updateworktaskinnode(worktaskinnode);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < item.datavalue.arr.Count; i++)
                                {
                                    TblWorkTaskInNode worktaskinnode = null;
                                    if (allMapped.Count > i)
                                    {
                                        worktaskinnode = allMapped[i];
                                        worktaskinnode.WorkTaskID = item.datavalue.arr[i];
                                        WorkFlowNodeManager.Updateworktaskinnode(worktaskinnode);
                                    }
                                    else
                                    {
                                        worktaskinnode = new TblWorkTaskInNode();
                                        worktaskinnode.NodeID = node.Id;
                                        worktaskinnode.WorkTaskID = item.datavalue.arr[i];
                                        WorkFlowNodeManager.Insertworktaskinnode(worktaskinnode);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (int mapped in item.datavalue.arr)
                            {
                                TblWorkTaskInNode worktaskinnode = new TblWorkTaskInNode();
                                worktaskinnode.NodeID = node.Id;
                                worktaskinnode.WorkTaskID = mapped;
                                WorkFlowNodeManager.Insertworktaskinnode(worktaskinnode);
                            }
                        }

                        #endregion

                        SaveObjectSubworkInSession(idWorkflow, item.datavalue);
                    }
                }
            }
            return false;
        }

        public static bool SaveProductionPropertiesMapping(int idWorkflow, string dataCells)
        {
            ObjectNodeWork[] arrCell = JsonConvert.DeserializeObject<ObjectNodeWork[]>(dataCells);
            if (arrCell != null && arrCell.Length > 0)
            {
                int id = 0;
                foreach (ObjectNodeWork item in arrCell)
                {
                    int.TryParse(item.id, out id);
                    TblWorkFlowNode node = WorkflowNodeHelper.FindTblWorkFlowNode(id, idWorkflow);
                    if (node != null)
                    {
                        
                        SaveObjectProductionPropertiesInSession(idWorkflow, item.datavalue);
                    }
                }
            }
            return false;
        }

        static Dictionary<long, List<WorktaskMapping>> ListObjectSubwork
        {
            get
            {
                if (HttpContext.Current.Session["ListObjectSubwork"] != null)
                    return HttpContext.Current.Session["ListObjectSubwork"] as Dictionary<long, List<WorktaskMapping>>;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["ListObjectSubwork"] = value;
            }
        }

        static Dictionary<long, List<Nodeworkproperties>> ListNodeworkproperties
        {
            get
            {
                if (HttpContext.Current.Session["ListNodeworkproperties"] != null)
                    return HttpContext.Current.Session["ListNodeworkproperties"] as Dictionary<long, List<Nodeworkproperties>>;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["ListNodeworkproperties"] = value;
            }
        }

        public static void SaveObjectSubworkInSession(int idWorkflow, WorktaskMapping item)
        {
            Dictionary<long, List<WorktaskMapping>> listObjectSubwork = ListObjectSubwork;
            if (listObjectSubwork == null)
            {
                listObjectSubwork = new Dictionary<long, List<WorktaskMapping>>();
                listObjectSubwork.Add(idWorkflow, new List<WorktaskMapping>() { item });
            }
            else
            {
                if (listObjectSubwork.Count > 0)
                {
                    if (listObjectSubwork.ContainsKey(idWorkflow))
                    {
                        if (listObjectSubwork[idWorkflow] != null && listObjectSubwork[idWorkflow].Count > 0)
                        {
                            int indx = -1;
                            for (int i = 0; i < listObjectSubwork[idWorkflow].Count; i++)
                            {
                                WorktaskMapping subwork = listObjectSubwork[idWorkflow][i];
                                if (subwork.id == item.id)
                                    indx = i;
                            }
                            if (indx > -1)
                            {
                                listObjectSubwork[idWorkflow].RemoveAt(indx);
                                listObjectSubwork[idWorkflow].Add(item);
                            }
                            else
                                listObjectSubwork[idWorkflow].Add(item);
                        }
                        else
                            listObjectSubwork[idWorkflow] = new List<WorktaskMapping>() { item };

                    }
                    else
                        listObjectSubwork.Add(idWorkflow, new List<WorktaskMapping>() { item });
                }
                else
                    listObjectSubwork.Add(idWorkflow, new List<WorktaskMapping>() { item });
            }
            ListObjectSubwork = listObjectSubwork;
        }

        public static void SaveObjectProductionPropertiesInSession(int idWorkflow, Nodeworkproperties item)
        {
            Dictionary<long, List<Nodeworkproperties>> listNodeworkproperties = ListNodeworkproperties;
            if (listNodeworkproperties == null)
            {
                listNodeworkproperties = new Dictionary<long, List<Nodeworkproperties>>();
                listNodeworkproperties.Add(idWorkflow, new List<Nodeworkproperties>() { item });
            }
            else
            {
                if (listNodeworkproperties.Count > 0)
                {
                    if (listNodeworkproperties.ContainsKey(idWorkflow))
                    {
                        if (listNodeworkproperties[idWorkflow] != null
                            && listNodeworkproperties[idWorkflow].Count > 0)
                        {
                            int indx = -1;
                            for (int i = 0; i < listNodeworkproperties[idWorkflow].Count; i++)
                            {
                                Nodeworkproperties subwork = listNodeworkproperties[idWorkflow][i];
                                if (subwork.id == item.id)
                                    indx = i;
                            }
                            if (indx > -1)
                            {
                                listNodeworkproperties[idWorkflow].RemoveAt(indx);
                                listNodeworkproperties[idWorkflow].Add(item);
                            }
                            else
                                listNodeworkproperties[idWorkflow].Add(item);
                        }
                        else
                            listNodeworkproperties[idWorkflow] = new List<Nodeworkproperties>() { item };
                    }
                    else
                        listNodeworkproperties.Add(idWorkflow, new List<Nodeworkproperties>() { item });
                }
                else
                    listNodeworkproperties.Add(idWorkflow, new List<Nodeworkproperties>() { item });
            }
            ListNodeworkproperties = listNodeworkproperties;
        }
    }

    public class ObjectSubwork
    {
        public string id { get; set; }
        public string datadesign { get; set; }
        public WorktaskMapping datavalue { get; set; }
    }

    public class WorktaskMapping
    {
        public string id { get; set; }
        public List<int> arr { get; set; }
    }

    public class ObjectNodeWork
    {
        public string id { get; set; }
        public string datadesign { get; set; }
        public Nodeworkproperties datavalue { get; set; }
    }

    public class Nodeworkproperties
    {
        public string id { get; set; }
        public List<string> arr { get; set; }
        public bool updatepropertiesvalues { get; set; }
        public bool hasvalue { get; set; }
    }
}
