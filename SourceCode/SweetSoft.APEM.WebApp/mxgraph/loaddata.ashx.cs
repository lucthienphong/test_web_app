using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    public class loaddata : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            // Response is always returned as text/plain
            context.Response.ContentType = "text/plain";

            string fileName = context.Request.Params["filename"];
            string idparent = context.Request.Params["idparent"];
            string idtruc = context.Request.Params["IdTruc"];
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(idtruc))
                return;

            idtruc = Regex.Replace(idtruc, "[^0-9-]", "");
            idparent = Regex.Replace(idparent, "[^0-9-]", "");
        
            if (string.IsNullOrEmpty(idtruc))
            {
            }
            else
            {
                TblWorkFlow wf = new Select().From(TblWorkFlow.Schema)
                   .Where(TblWorkFlow.MachineryProduceTypeIDColumn).IsEqualTo(idtruc)
                   .And(TblWorkFlow.IdParentColumn).IsEqualTo(idparent)
                   .And(TblWorkFlow.CodeColumn).IsEqualTo(Path.GetFileNameWithoutExtension(fileName))
                   .ExecuteSingle<TblWorkFlow>();
                if (wf != null)
                {
                    WorkflowNodeHelper.SaveWorkflowInSession(wf.Id, wf.Code + "*" + wf.Id);
                    WorkflowNodeHelper.SaveWorkFlowNode(wf.ContentXML, wf.Id);
                    context.Response.Write("{\"id\":\"" + wf.Id + "\",\"data\":\"" + wf.ContentXML.Replace("\"", "'") + "\"}");
                }
                else
                    context.Response.Write(string.Empty);
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