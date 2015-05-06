using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace SweetSoft.APEM.WebApp.mxgraph
{
    public class deleteHandler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // Response is always returned as text/plain
            context.Response.ContentType = "text/plain";
            int IdNode = 0;
            string temp = context.Request.Params["objId"];
            if (!string.IsNullOrEmpty(temp))
            {
                temp = Regex.Replace(temp, "[^0-9]", "");
                int.TryParse(temp, out IdNode);
                if (IdNode == 0)
                {
                    context.Response.Write(string.Empty);
                    return;
                }
            }

            string wfCode = context.Request.Params["filename"];
            //string idparent = context.Request.Params["idparent"];
            string wfidtemp = context.Request.Params["wfid"];
            if (string.IsNullOrEmpty(wfCode) )
                return;

            wfidtemp = Regex.Replace(wfidtemp, "[^0-9-]", "");
            wfCode = Path.GetFileNameWithoutExtension(wfCode);
            if (string.IsNullOrEmpty(wfCode))
                return;

            ErrorDelete obj = new ErrorDelete();

            bool data = true;


            string act = context.Request["act"];
            if (!string.IsNullOrEmpty(act) && act.ToLower() == "delete")
            {
                string idtruc = context.Request.Params["idtruc"];
                idtruc = Regex.Replace(idtruc, "[^0-9-]", "");
                if (idtruc.Length == 0)
                {
                    obj.isError = true;
                    obj.errorMessage = "Không tồn tại các bản thiết kế.";
                }
                else
                {
                    TblWorkFlow workflowByTruc = new SubSonic.Select(TblWorkFlow.IdColumn)
                                        .From(TblWorkFlow.Schema)
                                        .Where(TblWorkFlow.MachineryProduceTypeIDColumn).IsEqualTo(idtruc)
                                        .And(TblWorkFlow.CodeColumn).IsEqualTo("Main-Work")
                                        .And(TblWorkFlow.IdParentColumn).IsEqualTo(0)
                                        .ExecuteSingle<TblWorkFlow>();
                    if (workflowByTruc != null)
                    {
                        bool isDeleted = WorkFlowManager.Delete(workflowByTruc.Id);
                        if (isDeleted == true)
                        {
                            try
                            {
                                isDeleted = new TblMachinaryProduceTypeController().Delete(idtruc);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (isDeleted == false)
                        {
                            obj.isError = true;
                            obj.errorMessage = "Xảy ra lỗi trong quá trình xóa bản thiết kế.";
                        }
                        else
                            obj.isError = false;
                    }

                    #region delete workflow
                    /*
                    List<Tblright> result = new List<Tblright>();
                    if (ApplicationContext.Current.User != null)
                    {
                        result = RoleManager.GetRightByFunctionCodeAndUserID("ThietKeSanXuat",
                            ApplicationContext.Current.User.Id).ToList<Tblright>();

                        if (result != null && result.Count > 0)
                        {
                            bool isEnable = false;
                            bool isDeletable = false;
                            foreach (Tblright item in result)
                            {
                                if (isDeletable && isEnable)
                                {
                                    try
                                    {
                                         TblWorkFlow workflowByTruc = new SubSonic.Select(TblWorkFlow.IdColumn)
                                        .From(TblWorkFlow.Schema)
                                        .Where(TblWorkFlow.IdmachinertproducetypeColumn).IsEqualTo(idtruc)
                                        .And(TblWorkFlow.CodeColumn).IsEqualTo("Main-Work")
                                        .And(TblWorkFlow.IdParentColumn).IsEqualTo(0)
                                        .ExecuteSingle<TblWorkFlow>();
                                        if (workflowByTruc != null)
                                        {
                                            bool isDeleted = WorkFlowManager.Delete(workflowByTruc.Id);
                                            if (isDeleted == false)
                                            {
                                                obj.isError = true;
                                                obj.errorMessage = "Xảy ra lỗi trong quá trình xóa bản thiết kế.";
                                            }
                                            else
                                                obj.isError = false;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    break;
                                }

                                if (isEnable == false)
                                {
                                    if (item.Rightscode == "thiet_ke_workflow")
                                        isEnable = true;
                                }
                                if (isDeletable == false)
                                {
                                    if (item.Rightscode == "delete_workflow")
                                        isDeletable = true;
                                }
                            }
                        }
                    }
                    */
                    #endregion

                }
            }
            else
            {
                #region validate
                if (string.IsNullOrEmpty(wfidtemp) == false)
                {
                    TblWorkFlowNode node = WorkflowNodeHelper.GetTblWorkFlowNodeByIdFromDesign(int.Parse(wfidtemp), IdNode.ToString());
                    if (node != null)
                    {
                        if (node.NodeType.ToLower() == WorkflowNodeType.DauVaoHoacDauRa.ToString().ToLower())
                        {
                            data = false;
                            obj.errorMessage = "Thành phần ở bước đầu và bước cuối không thể bị xóa.";
                        }
                    }
                    if (data == false)
                    {
                        obj.isError = true;
                        if (obj.errorMessage.Length == 0)
                            obj.errorMessage = "Vẫn còn ống (đơn hàng) đang ở công việc này.\nHiện giờ bạn chưa thể xóa thành phần này.";
                    }
                    else
                        obj.isError = false;
                }
                #endregion
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string serializedResult = serializer.Serialize(obj);
            context.Response.Write(serializedResult);
            return;
        }

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

    }

    public class ErrorDelete
    {
        public bool isError { get; set; }
        public string errorMessage { get; set; }
    }
}