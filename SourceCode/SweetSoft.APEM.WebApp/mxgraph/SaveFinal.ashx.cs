using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    public class SaveFinal : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            /*
            if (ApplicationContext.Current.UserName.ToLower() == "anonymous")
            {
                context.Response.Write("login");
                return;
            }
            */

            //Kiểm tra quyền
            if (!RoleManager.AllowEdit(ApplicationContext.Current.UserName, "workflow_manager"))
            {
                context.Response.Write(ResourceTextManager.GetApplicationText(ResourceText.NOT_ALLOW));
                return;
            }

            // Response is always returned as text/plain
            context.Response.ContentType = "text/plain";

            //return;

            string fileName = context.Request.Params["filename"];
            string title = context.Request.Params["title"];
            string idparent = context.Request.Params["idparent"];
            string wfidtemp = context.Request.Params["wfid"];
            string idtruc = context.Request.Params["IdTruc"];
            string actionNode = context.Request.Params["an"];

            //check if user input filename
            if (string.IsNullOrEmpty(fileName))
            {
                context.Response.Write("Can't find workflow.");
                return;
            }
            fileName = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrEmpty(fileName))
            {
                context.Response.Write("Can't find workflow.");
                return;
            }
            if (fileName.ToLower().StartsWith("truc"))
            {
                return;
            }

            if (string.IsNullOrEmpty(idparent))
            {
                context.Response.Write("Can't save workflow.");
                return;
            }

            if (string.IsNullOrEmpty(idtruc))
            {
                context.Response.Write("Can't save workflow.");
                return;
            }
            if (string.IsNullOrEmpty(actionNode))
            {
                context.Response.Write("Can't save workflow.");
                return;
            }

            idtruc = Regex.Replace(idtruc, "[^0-9-]", "");
            idparent = Regex.Replace(idparent, "[^0-9-]", "");
            string xml = HttpUtility.UrlDecode(context.Request.Params["xml"]);

            if (xml != null && xml.Length > 0)
            {
                #region save to database

                string userName = string.Empty;
                try
                {
                    userName = ApplicationContext.Current.UserName;
                }
                catch
                {
                    //context.Response.Write("Error with user name.");
                    //return;
                    userName = "Anonymous";
                }

                string act = context.Request.Params["act"];
                if (string.IsNullOrEmpty(act) == false && act.ToLower() == "reset" && string.IsNullOrEmpty(wfidtemp) == false)
                {
                    int idworkflow = 0;
                    int.TryParse(wfidtemp, out idworkflow);
                    bool canReset = WorkFlowManager.ResetWorkflow(fileName, idworkflow);
                    if (canReset == false)
                    {
                        context.Response.Write("Can't reset workflow.");
                        return;
                    }

                    TblWorkFlow wf = WorkFlowManager.GetWorkFlowByID(idworkflow);
                    if (wf != null)
                        WorkflowNodeHelper.RemoveWorkflowInSession(wf.Id, fileName + "*" + wfidtemp);
                }


                bool isUpdate = false;
                TblWorkFlow workFlow = null;
                long idWorkflow = 0;
                string oldXML = string.Empty;
                if (idtruc != "-1")//khong phai la tao moi main-work
                {
                    if (title.Length > 0)
                        long.TryParse(wfidtemp, out idWorkflow);
                    else if (string.IsNullOrEmpty(wfidtemp) == false)
                        idWorkflow = WorkflowNodeHelper.FindWorkflow(fileName + "*" + wfidtemp);
                    if (idWorkflow > 0)
                    {
                        workFlow = WorkFlowManager.GetWorkFlowByID(idWorkflow);
                        if (workFlow != null)
                        {
                            isUpdate = true;
                            oldXML = workFlow.ContentXML;
                        }
                    }
                }

                if (title.Length > 0)
                {
                    //check insert machinery productype
                    string type = context.Request.Params["type"];
                    if (string.IsNullOrEmpty(type) == false)
                    {
                        int idmachinery = 0;
                        int.TryParse(idtruc, out idmachinery);
                        bool isExists = true;
                        TblMachinaryProduceType machineryproducetype = null;
                        if (idmachinery > 0)
                            machineryproducetype = new SubSonic.Select().From(TblMachinaryProduceType.Schema)
                                .Where(TblMachinaryProduceType.IdColumn).IsEqualTo(idmachinery)
                                .ExecuteSingle<TblMachinaryProduceType>();
                        if (machineryproducetype == null)
                        {
                            isExists = false;
                            machineryproducetype = new TblMachinaryProduceType();
                            machineryproducetype.IsObsolete = false;
                            //machineryproducetype.CreatedBy = userName;
                            machineryproducetype.Type = type;
                            //machineryproducetype.CreatedDate = DateTime.Now;
                        }


                        machineryproducetype.Name = title;
                        //machineryproducetype.UpdatedBy = userName;
                        //machineryproducetype.UpdatedDate = DateTime.Now;
                        if (isExists == false)
                            machineryproducetype = new TblMachinaryProduceTypeController().Insert(machineryproducetype.Name, machineryproducetype.Type, machineryproducetype.IsObsolete);
                        else
                            machineryproducetype = new TblMachinaryProduceTypeController().Update(machineryproducetype.Id, machineryproducetype.Name, machineryproducetype.Type, machineryproducetype.IsObsolete);
                        idtruc = machineryproducetype.Id.ToString();
                    }
                }

                if (workFlow == null)
                {
                    workFlow = new TblWorkFlow();
                    workFlow.CreatedBy = userName;
                    workFlow.CreatedOn = DateTime.Now;
                    workFlow.IsFirstPage = true;
                    workFlow.IsShow = true;
                }

                workFlow.ContentXML = string.Empty;
                workFlow.Code = fileName;

                //workFlow.ListFromConnection = title;
                workFlow.ModifiedBy = userName;
                workFlow.ModifiedOn = DateTime.Now;
                //workFlow.IdParent = idparent.Length > 0 ? long.Parse(idparent) : 0;

                //not update parent for main workflow
                if (workFlow.Code.ToLower() != "main-work")
                    workFlow.IdParent = idparent.Length > 0 ? int.Parse(idparent) : 0;
                else
                    workFlow.IdParent = 0;

                workFlow.MachineryProduceTypeID = idtruc.Length > 0 ? int.Parse(idtruc) : 0;

                //if (title.Length > 0)
                //    workFlow.Idmachinertproducetype = 0;

                /*old
                if (isUpdate)
                {
                    workFlow = WorkFlowManager.Update(workFlow);
                    WorkflowNodeHelper.UpdateWorkFlowNode(workFlow.ContentXML, workFlow.Code + "*" + idtruc);
                }
                else
                {
                    workFlow = WorkFlowManager.Insert(workFlow);
                    WorkflowNodeHelper.SaveWorkflowInSession(workFlow.Id, workFlow.Code + "*" + idtruc);
                    WorkflowNodeHelper.SaveWorkFlowNode(workFlow.ContentXML, workFlow.Code + "*" + idtruc);
                }
                */

                List<int> lstResult = null;
                if (isUpdate == false)
                {
                    workFlow = WorkFlowManager.Insert(workFlow);
                    if (workFlow != null)
                        lstResult = ActionNodeHelper.SaveActionNode(actionNode, workFlow.Id);
                }
                else
                    lstResult = ActionNodeHelper.SaveActionNode(actionNode, workFlow.Id);

                //save errror
                if (lstResult.Count > 0)
                {
                    //if error, we restore old xml content
                    if (isUpdate == true)
                    {
                        workFlow.ContentXML = oldXML;
                        workFlow = WorkFlowManager.Update(workFlow);
                    }
                    context.Response.Write(JsonConvert.SerializeObject(lstResult));
                    WorkflowNodeHelper.RemoveAllSession();
                    return;
                }
                else
                {
                    try
                    {
                        //update
                        workFlow.ContentXML = xml;
                        workFlow = WorkFlowManager.Update(workFlow);
                        WorkflowNodeHelper.SaveWorkflowInSession(workFlow.Id, workFlow.Code + "*" + workFlow.Id);
                        WorkflowNodeHelper.SaveWorkFlowNode(workFlow.ContentXML, workFlow.Id);

                        //update TblWorkFlowline idworkflowTo
                        //line map to which TblWorkFlow
                        string lineidTemp = context.Request.Params["lineid"];
                        if (string.IsNullOrEmpty(lineidTemp) == false)
                        {
                            int lineId = 0;
                            int.TryParse(lineidTemp, out lineId);
                            if (lineId > 0)
                            {
                                TblWorkFlowLine lineGet = WorkFlowLineManager.GetExtractWorkFlowLineByIdInXML(lineId.ToString(), workFlow.IdParent ?? 0);
                                if (lineGet != null)
                                {
                                    lineGet.WorkFlowToID = workFlow.Id;
                                    WorkFlowLineManager.Update(lineGet);
                                }
                            }
                        }

                        context.Response.Write("Data saved successfully.||" + workFlow.Id + "||" + workFlow.MachineryProduceTypeID);
                    }
                    catch { }
                }

                #endregion
            }
            else
            {
                context.Response.Write("Empty or missing request parameter.");
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