using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    public class getinfocell : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // Response is always returned as text/plain
            context.Response.ContentType = "text/plain";

            //string fileName = context.Request.Params["filename"];
            string wfidtemp = context.Request.Params["wfid"];
            string idcell = context.Request.Params["idcell"];
            string type = context.Request.Params["type"];
            if (/*string.IsNullOrEmpty(fileName) ||*/
                string.IsNullOrEmpty(wfidtemp) ||
                string.IsNullOrEmpty(idcell) ||
                string.IsNullOrEmpty(type))
                return;

            //fileName = Path.GetFileNameWithoutExtension(fileName);
            wfidtemp = Regex.Replace(wfidtemp, "[^0-9-]", "");
            idcell = Regex.Replace(idcell, "[^0-9-]", "");
            type = type.Trim();
            if (string.IsNullOrEmpty(type)/* || string.IsNullOrEmpty(fileName)*/)
            {
                context.Response.Write(string.Empty);
                return;
            }

            if (type.ToLower() == "subwork")
            {
                #region RegionName

                WorktaskMapping item = new WorktaskMapping();
                //check if user input filename
                if (!string.IsNullOrEmpty(wfidtemp) && !string.IsNullOrEmpty(idcell))
                {
                    int id = 0;
                    int.TryParse(idcell, out id);
                    if (id > 0)
                    {
                        /*
                        WorkflowNode workflowNode = WorkflowNodeHelper.GetWorkflowNode(Path.GetFileNameWithoutExtension(fileName), idtruc, idcell);
                        if (workflowNode != null)
                            context.Response.Write(JsonConvert.SerializeObject(workflowNode));
                        */
                        TblWorkFlowNode node = WorkflowNodeHelper.FindTblWorkFlowNode(id, int.Parse(wfidtemp));
                        if (node != null)
                        {
                            List<int> lstWorktaskId = new List<int>();
                            item.id = node.WorkFlowIDInXML.Value.ToString();
                            TblWorkTaskInNodeCollection allChildWork = WorkFlowNodeManager.GetWorktaskBySubwork(node.Id);
                            if (allChildWork != null && allChildWork.Count > 0)
                            {
                                foreach (TblWorkTaskInNode none in allChildWork)
                                    lstWorktaskId.Add(none.WorkTaskID);
                            }
                            item.arr = lstWorktaskId;
                            context.Response.Write(JsonConvert.SerializeObject(item));
                            return;
                        }
                    }
                }
                #endregion

                item.id = "0";
                item.arr = new List<int>();
                context.Response.Write(JsonConvert.SerializeObject(item));
            }
        }

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
    }
}