using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    public class getwork : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // Response is always returned as text/plain
            context.Response.ContentType = "text/plain";

            string temp = context.Request.Params["IdDept"];
            temp = Regex.Replace(temp, "[^0-9-]", "");
            if (temp.IndexOf("-") <= 0)
            {
                context.Response.Write(string.Empty);
                return;
            }
            string[] arr = temp.Split('-');//arr : 0 - targer id,1 - source id
            if (arr.Length == 1)
                context.Response.Write(string.Empty);


            int IdDept = 0;
            int.TryParse(arr[1], out IdDept);//we need source id,where arrow point from
            if (IdDept == 0)
            {
                context.Response.Write(string.Empty);
                return;
            }

            StringBuilder sbReturn = new StringBuilder("{\"works\":");
            TblWorkTaskCollection allTask = WorkTaskManager.GetWorktaskForWLByIdDept(IdDept);
            if (allTask != null && allTask.Count > 0)
            {
                object defaultObject = null;

                List<workobject> list = new List<workobject>();

                foreach (TblWorkTask item in allTask)
                {
                    workobject obj = new workobject();
                    Type t = item.GetType();

                    PropertyInfo p = t.GetProperty("ID", BindingFlags.Public
                                                | BindingFlags.Instance
                                                | BindingFlags.IgnoreCase);

                    if (p != null)
                        defaultObject = p.GetValue(item, null);
                    if (defaultObject != null)
                        obj.Id = Convert.ToInt32(defaultObject);

                    p = t.GetProperty("DepartmentID", BindingFlags.Public
                                                | BindingFlags.Instance
                                                | BindingFlags.IgnoreCase);

                    if (p != null)
                        defaultObject = p.GetValue(item, null);
                    if (defaultObject != null)
                        obj.IdDept = Convert.ToInt32(defaultObject);

                    p = t.GetProperty("Name", BindingFlags.Public
                                               | BindingFlags.Instance
                                               | BindingFlags.IgnoreCase);
                    if (p != null)
                        defaultObject = p.GetValue(item, null);
                    if (defaultObject != null)
                        obj.Name = Convert.ToString(defaultObject);

                    list.Add(obj);
                }

                if (list != null && list.Count > 0)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    sbReturn.Append(serializer.Serialize(list));
                }
                else
                    sbReturn.Append("[]");
            }
            else 
                sbReturn.Append("[]");
            sbReturn.Append(",\"map\":[");

            sbReturn.Append("]}");
            context.Response.Write(sbReturn.ToString());
        }

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
    }

    public class workobject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdDept { get; set; }
    }
}