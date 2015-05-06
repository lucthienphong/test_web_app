using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    public class saveinfocell : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // Response is always returned as text/plain
            context.Response.ContentType = "text/plain";

            //string workflowCode = context.Request.Params["filename"];
            string idWorkflow = context.Request.Params["idwf"];

            if (/*string.IsNullOrEmpty(workflowCode) || */string.IsNullOrEmpty(idWorkflow))
            {
                context.Response.Write("0");
                return;
            }

            //workflowCode = Path.GetFileNameWithoutExtension(workflowCode);
            idWorkflow = Regex.Replace(idWorkflow, "[^0-9-]", "");

            if (/*string.IsNullOrEmpty(workflowCode) || */string.IsNullOrEmpty(idWorkflow))
            {
                context.Response.Write("0");
                return;
            }

            string arrcellsub = context.Request.Params["cellsub"];
            string arrcellwork = context.Request.Params["cellwork"];

            if (string.IsNullOrEmpty(arrcellsub) && string.IsNullOrEmpty(arrcellwork))
            {
                context.Response.Write("0");
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(arrcellsub))
                    SubworkHelper.SaveSubworkMapping(int.Parse(idWorkflow), arrcellsub);
                if (!string.IsNullOrEmpty(arrcellwork))
                    SubworkHelper.SaveProductionPropertiesMapping(int.Parse(idWorkflow), arrcellwork);
                context.Response.Write("1");
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