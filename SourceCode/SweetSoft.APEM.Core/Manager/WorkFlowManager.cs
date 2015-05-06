using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;

namespace SweetSoft.APEM.Core.Manager
{
    public class WorkFlowManager
    {
        /// <summary>
        /// Tìm wf theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TblWorkFlow GetWorkFlowByID(object id)
        {
            return new Select().From(TblWorkFlow.Schema).Where(TblWorkFlow.IdColumn).IsEqualTo(id)
                               .ExecuteSingle<TblWorkFlow>();
        }
        /// <summary>
        /// Tìm wf collection theo mã code và thuộc tính hiển thị
        /// </summary>
        /// <param name="code">Mã code</param>
        /// <param name="showAll">Lấy hết hay chỉ lấy mặc định</param>
        /// <returns></returns>
        public static TblWorkFlowCollection GetWorkFlowByCode(string code, bool showAll)
        {
            Select select = new Select();
            select.From(TblWorkFlow.Schema).Where(TblWorkFlow.CodeColumn).IsEqualTo(code);
            if (!showAll)
                select.And(TblWorkFlow.IsShowColumn).IsEqualTo(true);
            return select.ExecuteAsCollection<TblWorkFlowCollection>();
        }
        /// <summary>
        /// Tìm wf chính theo thuộc tính hiển thị
        /// </summary>
        /// <param name="showAll"></param>
        /// <returns></returns>
        public static TblWorkFlowCollection GetFirstWorkFlow(bool showAll)
        {
            Select select = new Select();
            select.From(TblWorkFlow.Schema).Where(TblWorkFlow.IsFirstPageColumn).IsEqualTo(true);
            if (!showAll)
                select.And(TblWorkFlow.IsShowColumn).IsEqualTo(true);
            return select.ExecuteAsCollection<TblWorkFlowCollection>();
        }

        public static TblWorkFlow Insert(TblWorkFlow workflow)
        {
            workflow.IsShow = true;
            return new TblWorkFlowController().Insert(workflow.FID,workflow.Code,workflow.IsFirstPage,
                workflow.IsShow,workflow.ContentXML,workflow.AxisTypeID,workflow.MachineryProduceTypeID,
                workflow.CreatedBy,workflow.CreatedOn,workflow.ModifiedBy,workflow.ModifiedOn);
        }

        public static TblWorkFlow Update(TblWorkFlow workflow)
        {
            return new TblWorkFlowController().Update(workflow.Id, workflow.FID, workflow.Code, workflow.IsFirstPage,
                workflow.IsShow, workflow.ContentXML, workflow.AxisTypeID, workflow.MachineryProduceTypeID,
                 workflow.CreatedBy, workflow.CreatedOn, workflow.ModifiedBy, workflow.ModifiedOn);
        }
        /// <summary>
        /// Thay đổi trạng thái wf, đưa về dạng ẩn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ChangeState(object id)
        {
            TblWorkFlow wf = GetWorkFlowByID(id);
            if (wf != null)
            {
                wf.IsShow = false;
                return Update(wf) != null;
            }
            return false;
        }
        /// <summary>
        /// Lấy phòng ban cho workflow
        /// </summary>
        /// <returns></returns>
        public static TblDepartmentCollection GetDeptForWorkflow()
        {
            return new Select().From(TblDepartment.Schema).Where(TblDepartment.ShowInWorkFlowColumn).IsEqualTo(true)
                               .ExecuteAsCollection<TblDepartmentCollection>();
        }

        /// <summary>
        /// Tìm tất cả workflow theo loại trục
        /// </summary>
        /// <param name="idMachineryProducetype">id Machinery Producetype</param>
        /// <returns>TblWorkFlowCollection</returns>
        public static TblWorkFlowCollection GetAllWorkFlowByIdMachineryProducetype(int idMachineryProducetype)
        {
            Select select = new Select();
            select.From(TblWorkFlow.Schema)//.Where(TblWorkFlow.IdParentColumn).IsNotEqualTo(0)
                .Where(TblWorkFlow.MachineryProduceTypeIDColumn).IsEqualTo(idMachineryProducetype);
            return select.ExecuteAsCollection<TblWorkFlowCollection>();
        }
               

        /// <summary>
        /// get all workflow which has id start dept (and optional: id end dept)
        /// </summary>
        /// <param name="idMachineryProducetype">id Machinery Producetype</param>
        /// <param name="idDeptStart">idDept Start</param>
        /// <param name="idDeptEnd">idDept End</param>
        /// <returns>List of TblWorkFlow</returns>
        public static List<TblWorkFlow> GetWorkflowByIdMachineryProducetypeAndIdDepts(int idMachineryProducetype, int idDeptStart, int? idDeptEnd)
        {
            List<TblWorkFlow> lstReturn = new List<TblWorkFlow>();
            TblWorkFlowCollection allWorkFlowByIdMachineryProducetype = GetAllWorkFlowByIdMachineryProducetype(idMachineryProducetype);
            if (allWorkFlowByIdMachineryProducetype != null && allWorkFlowByIdMachineryProducetype.Count > 0)
            {
                string[] arr = null;
                if (idDeptEnd.HasValue)
                {
                    foreach (TblWorkFlow TblWorkFlow in allWorkFlowByIdMachineryProducetype)
                    {
                        arr = TblWorkFlow.Code.Split('-');
                        if (arr.Length == 2)
                        {
                            if (arr[0].EndsWith(idDeptEnd.ToString()) && arr[1].EndsWith(idDeptStart.ToString()))
                                lstReturn.Add(TblWorkFlow);
                        }
                    }
                }
                else
                {
                    foreach (TblWorkFlow TblWorkFlow in allWorkFlowByIdMachineryProducetype)
                    {
                        arr = TblWorkFlow.Code.Split('-');
                        if (arr.Length == 2)
                        {
                            if (arr[1].EndsWith(idDeptStart.ToString()))
                                lstReturn.Add(TblWorkFlow);
                        }
                    }
                }
            }
            return lstReturn;
        }

        /// <summary>
        /// Get extract TblWorkFlow by id Machinery Producetype and [id start dept AND id end dept]
        /// </summary>
        /// <param name="idMachineryProducetype"></param>
        /// <param name="idDeptStart"></param>
        /// <param name="idDeptEnd"></param>
        /// <returns></returns>
        public static List<TblWorkFlow> GetWorkflowByIdMachineryProducetypeAndBetweenDepts(int idMachineryProducetype, int idDeptStart, int idDeptEnd)
        {
            List<TblWorkFlow> allFound = GetWorkflowByIdMachineryProducetypeAndIdDepts(idMachineryProducetype, idDeptStart, idDeptEnd);
            if (allFound != null && allFound.Count > 0)
                return allFound;
            return null;
        }

        public static bool Delete(object id)
        {
            TblWorkFlow wf = GetWorkFlowByID(id);
            if (wf != null)
            {
                if (ResetWorkflow(wf.Code, wf.Id))
                {
                    return new TblWorkFlowController().Delete(id);
                }
            }
            return false;
        }

        public static bool ResetWorkflow(string workflowCode, long idWorkflow)
        {
            TblWorkFlow workflow = WorkFlowManager.GetWorkFlowByID(idWorkflow);
            if (workflow != null)
            {
                bool canContinue = true;
                //xóa các node trong bản thiết kế
                TblWorkFlowNodeCollection allNodeInWorkflow = WorkFlowNodeManager.GetAllWorkFlowNodeByWorkflow(workflow.Id);
                if (allNodeInWorkflow != null && allNodeInWorkflow.Count > 0)
                {
                    foreach (TblWorkFlowNode item in allNodeInWorkflow)
                    {
                        canContinue = WorkFlowNodeManager.Delete(item);
                        if (canContinue == false)
                            return false;
                    }
                }
                //xóa các đường chuyển trong bản thiết kế
                TblWorkFlowLineCollection allLineInWorkflow = WorkFlowLineManager.GetAllWorkFlowLineByWorkflow(workflow.Id);
                if (allLineInWorkflow != null && allLineInWorkflow.Count > 0)
                {
                    foreach (TblWorkFlowLine item in allLineInWorkflow)
                    {
                        canContinue = WorkFlowLineManager.Delete(item);
                        if (canContinue == false)
                            return false;
                    }
                }

                TblWorkFlowCollection allChildWF = new SubSonic.Select(TblWorkFlow.IdColumn)
                .From(TblWorkFlow.Schema)
                    .Where(TblWorkFlow.IdParentColumn).IsEqualTo(workflow.Id)
                    .ExecuteAsCollection<TblWorkFlowCollection>();
                if (allChildWF != null && allChildWF.Count > 0)
                {
                    foreach (TblWorkFlow item in allChildWF)
                    {
                        canContinue = Delete(item.Id);
                        if (canContinue == false)
                            return false;
                    }
                }
                return canContinue;
            }
            return false;
        }

        public static bool ResetWorkflow(object id)
        {
            TblWorkFlow workflow = WorkFlowManager.GetWorkFlowByID(id);
            if (workflow != null)
                return ResetWorkflow(workflow.Code, workflow.Id);
            return false;
        }

        #region Tblnode ducnm 01.09

        public static TblWorkFlowNode GetNodeByID(object id)
        {
            return new Select().From(TblWorkFlowNode.Schema).Where(TblWorkFlowNode.IdColumn).IsEqualTo(id).ExecuteSingle<TblWorkFlowNode>();
        }
        /*
        public static TblWorkFlowNode GetNodeStartByDeptId(string deptCode)
        {
            Select select = new Select();
            select.From(TblWorkFlowNode.Schema);
            select.InnerJoin(TblDepartment.Schema.TableName, TblDepartment.Columns.DepartmentID, TblWorkFlowNode.Schema.TableName, TblWorkFlowNode.Columns.DepartmentID);
            select.Where(TblWorkFlowNode.NodeTypeColumn).IsEqualTo("Xuong");
            select.And(TblWorkFlowNode.WorkFlowCodeColumn).IsEqualTo("Main-Work");
            //select.And(TblDepartment.CodeColumn).IsEqualTo(deptCode);
            return select.ExecuteSingle<TblWorkFlowNode>();
        }
        */

        /// <summary>
        /// Lấy workflow theo code và loại machinery
        /// </summary>
        /// <param name="workflowcode"></param>
        /// <param name="idmacinery"></param>
        /// <returns></returns>
        public static TblWorkFlow GetWorkflowByWorkflowCodeAndMachineryID(object workflowcode, object idmacinery)
        {
            return new Select().From(TblWorkFlow.Schema).Where(TblWorkFlow.CodeColumn).IsEqualTo(workflowcode)
                               .And(TblWorkFlow.MachineryProduceTypeIDColumn).IsEqualTo(idmacinery)
                               .ExecuteSingle<TblWorkFlow>();
        }
        /// <summary>
        /// Ducnm 15.10: Hàm lấy các workflow main theo loại sản phẩm
        /// </summary>
        /// <returns></returns>
        public static TblWorkFlowCollection GetWorkflowMainCollection()
        {
            return new Select().From(TblWorkFlow.Schema).Where(TblWorkFlow.CodeColumn).IsEqualTo("Main-Work")
                               .And(TblWorkFlow.MachineryProduceTypeIDColumn).IsNotNull()
                               .ExecuteAsCollection<TblWorkFlowCollection>();
        }

        #endregion
    }
}
