using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    public class getmachinery : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // Response is always returned as text/plain
            context.Response.ContentType = "text/plain";

            string idtruc = context.Request.Params["Id"];
            string idparent = context.Request.Params["idparent"];
            string temp = Regex.Replace(idtruc, "[^0-9-]", "");
            idparent = Regex.Replace(idparent, "[^0-9-]", "");
            if (temp.Length == 0)
            {
                context.Response.Write(string.Empty);
                return;
            }
            //check if user input filename
            if (string.IsNullOrEmpty(idtruc))
            {
            }
            else
            {
                TblWorkFlow wf = new Select().From(TblWorkFlow.Schema)
                    //.Where(TblWorkFlow.MachineryProduceTypeIDColumn).IsEqualTo(temp)
                    .And(TblWorkFlow.CodeColumn).IsEqualTo("Main-Work")
                    .And(TblWorkFlow.IdParentColumn).IsEqualTo(0)
                               .ExecuteSingle<TblWorkFlow>();
                if (wf != null)
                {
                    WorkflowNodeHelper.SaveWorkflowInSession(wf.Id, wf.Code + "*" + temp);
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