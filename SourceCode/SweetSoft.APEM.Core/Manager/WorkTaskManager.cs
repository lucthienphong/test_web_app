using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;
using SubSonic;
using System.Data;
using SweetSoft.APEM.Core.Helper;

namespace SweetSoft.APEM.Core.Manager
{
    public class WorkTaskManager
    {
        public static TblWorkTask GetWorktaskById(object id)
        {
            return new Select().From(TblWorkTask.Schema).Where(TblWorkTask.IdColumn).IsEqualTo(id)
                               .ExecuteSingle<TblWorkTask>();
        }

        public static TblWorkTaskCollection GetWorktaskByIdDept(object id)
        {
            return new Select().From(TblWorkTask.Schema).Where(TblWorkTask.DepartmentIDColumn).IsEqualTo(id)
                               .And(TblWorkTask.CurrentStateColumn).IsEqualTo(true)
                               .And(TblWorkTask.ShowInWorkFlowColumn).IsEqualTo(true)
                               .OrderAsc(TblWorkTask.NameColumn.ColumnName)
                               .ExecuteAsCollection<TblWorkTaskCollection>();
        }

        public static TblWorkTaskCollection GetAll()
        {
            Select select = new Select();
            select.From(TblWorkTask.Schema).Where(TblWorkTask.IdColumn).IsNotNull();
            return select.ExecuteAsCollection<TblWorkTaskCollection>();
        }

        /// <summary>
        /// Get Worktask for workflow
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TblWorkTaskCollection GetWorktaskForWLByIdDept(int id)
        {
            return new Select().From(TblWorkTask.Schema).Where(TblWorkTask.DepartmentIDColumn).IsEqualTo(id)
                               .And(TblWorkTask.CurrentStateColumn).IsEqualTo(true)
                               .And(TblWorkTask.ShowInWorkFlowColumn).IsEqualTo(true)
                               .OrderAsc(TblWorkTask.NameColumn.ColumnName)
                               .ExecuteAsCollection<TblWorkTaskCollection>();
        }
        
        public static TblWorkFlow GetWorkflowDoHoaChepKhac()
        {
            Select select = new Select();
            select.From(TblWorkFlow.Schema);
            select.InnerJoin(TblMachinaryProduceType.Schema.TableName, 
                TblMachinaryProduceType.Columns.Id, TblWorkFlow.Schema.TableName, 
                TblWorkFlow.Columns.MachineryProduceTypeID);
            select.Where(TblMachinaryProduceType.TypeColumn).IsEqualTo("graphicEngraver");
            return select.ExecuteSingle<TblWorkFlow>();
        }

        public static TblWorkTask Insert(TblWorkTask worktask)
        {
            return new TblWorkTaskController().Insert(worktask.Name,worktask.DepartmentID,
                worktask.Constant1,worktask.Constant2,worktask.Constant3,worktask.Constant4,
                worktask.IsPlating,worktask.CurrentState,worktask.ShowInWorkFlow,worktask.CreatedBy,
                worktask.CreatedOn,worktask.ModifiedBy,worktask.ModifiedOn);
        }

        public static TblWorkTask Update(TblWorkTask worktask)
        {
            return new TblWorkTaskController().Update(worktask.Id, worktask.Name, worktask.DepartmentID,
                worktask.Constant1, worktask.Constant2, worktask.Constant3, worktask.Constant4,
                worktask.IsPlating, worktask.CurrentState, worktask.ShowInWorkFlow, worktask.CreatedBy,
                worktask.CreatedOn, worktask.ModifiedBy, worktask.ModifiedOn);
        }

        public static bool Delete(object id)
        {
            return new TblWorkTaskController().Delete(id);
        }
        /*
        public static List<TblWorkTask> SearchAndPaging(int pageIndex, int pageSize, string columnList, string tableName,
                                              string conditionClause, string orderBy, out int rowTotal)
        {
            rowTotal = 0;
            StoredProcedure sp = SPs.spSearchAndPaging(pageIndex, pageSize, columnList, tableName,
                                                        conditionClause, orderBy, out rowTotal);
            DataSet ds = sp.GetDataSet();
            List<TblWorkTask> lst = ds.ToCollection<TblWorkTask>();
            if (sp.OutputValues.Count > 0)
                rowTotal = Convert.ToInt32(sp.OutputValues[0]);
            return lst;
        }
        */
        #region Ducnm

        /// <summary>
        /// Hàm lấy công việc của thành phần phụ
        /// </summary>
        /// <param name="idnode"></param>
        /// <returns></returns>
        public static TblWorkTaskCollection GetWorkTaskByNodeID(object idnode)
        {
            Select select = new Select();
            select.From(TblWorkTask.Schema).InnerJoin(TblWorkTaskInNode.Schema.TableName, TblWorkTaskInNode.Columns.WorkTaskID, TblWorkTask.Schema.TableName, TblWorkTask.Columns.Id);
            select.Where(TblWorkTaskInNode.NodeIDColumn).IsEqualTo(idnode);
            select.And(TblWorkTask.CurrentStateColumn).IsEqualTo(true);
            return select.ExecuteAsCollection<TblWorkTaskCollection>();
        }

        #endregion
        
    }
}
