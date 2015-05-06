using System;
using System.Collections.Generic;
using System.Linq;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.Core.LoggingManager
{
    public class LoggingManager
    {
        public static TblLoggingCollection GetLogAll()
        {
            return new TblLoggingController().FetchAll();
        }
        /// <summary>
        /// Add new log
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static TblLogging AddNewLog(TblLogging log)
        {
            return new TblLoggingController().Insert(log);
        }

        public static TblLoggingCollection SearchLogging(List<string> searchFields, Dictionary<string, Comparison> comparisons,
                   Dictionary<string, object> searchValues1, Dictionary<string, object> searchValues2)
        {
            Query searchQuery = new Query(TblLogging.Schema.TableName);

            for (int i = 0; i < searchFields.Count; i++)
            {
                if (searchValues2 != null && comparisons[searchFields[i]] == Comparison.BetweenAnd)
                    searchQuery.BETWEEN_AND(searchFields[i], (DateTime)searchValues1[searchFields[i]], (DateTime)searchValues2[searchFields[i]]);
                else
                    searchQuery.AND(searchFields[i], comparisons[searchFields[i]], searchValues1[searchFields[i]]);

            }
            searchQuery.ORDER_BY(TblLogging.ActionDateColumn, "DESC");
            return new TblLoggingController().FetchByQuery(searchQuery);
        }

        public static void LogAction(string activity, string functionPage, string description)
        {
            if (ApplicationContext.Current.User != null)
                LogAction(ApplicationContext.Current.UserName, activity,
                    functionPage, description);
        }

        public static void LogAction(string currentUserName, string activity, string functionPage, string description)
        {

            TblLogging logging = new TblLogging();
            logging.UserName = currentUserName;
            logging.UserIP = ApplicationContext.Current.CurrentUserIp;
            logging.Activity = activity;
            logging.Description = description;
            logging.ActionDate = DateTime.Now;

            AddNewLog(logging);

            #region save notification

            RealtimeNotificationManager.CreateOrDismissNotification(description, activity, currentUserName, functionPage, null);

            #endregion
        }

        public static void LogAction(string currentUserName, string currentUserIp, string activity, string functionPage, string description)
        {

            TblLogging logging = new TblLogging();
            logging.UserName = currentUserName;
            logging.UserIP = currentUserIp;
            logging.Activity = activity;
            logging.Description = description;
            logging.ActionDate = DateTime.Now;

            AddNewLog(logging);

        }

        public static bool Delete(object idLogging)
        {
            return new TblLoggingController().Delete(idLogging);

        }

        public static int DeleteLogByTime(DateTime? timeFrom, DateTime? timeTo)
        {
            Delete delete = new Delete();
            delete.From(TblLogging.Schema).Where(TblLogging.IdColumn).IsNull();
            if (timeFrom.HasValue && timeTo.HasValue)
                delete.OrExpression(TblLogging.Columns.ActionDate).IsGreaterThanOrEqualTo(DateTimeHelper.AddFirstTimeOfDay(timeFrom.Value))
                      .And(TblLogging.Columns.ActionDate).IsLessThanOrEqualTo(DateTimeHelper.AddLastTimeOfDay(timeTo.Value));
            else
                if (timeFrom.HasValue)
                    delete.Or(TblLogging.ActionDateColumn).IsGreaterThanOrEqualTo(DateTimeHelper.AddFirstTimeOfDay(timeFrom.Value));
                else if (timeTo.HasValue)
                    delete.Or(TblLogging.ActionDateColumn).IsLessThanOrEqualTo(DateTimeHelper.AddLastTimeOfDay(timeTo.Value));
            return delete.Execute();
        }

        public static int DeleteAll()
        {
            return new Delete().From(TblLogging.Schema).Execute();
        }
        //public static List<TblLogging> GetLoggingCollection(int pageIndex, bool isPageIndex, int pageSize, string columnList, string tableName,
        //                                               string conditionClause, string orderBy, out int rowTotal)
        //{
        //    rowTotal = 0;
        //    StoredProcedure sp = SPs.spSearchAndPaging(pageIndex, isPageIndex, pageSize, columnList, tableName, conditionClause, orderBy, out rowTotal);
        //    DataSet ds = sp.GetDataSet();
        //    List<TblLogging> lst = ds.ToCollection<TblLogging>();
        //    if (sp.OutputValues.Count > 0)
        //        rowTotal = Convert.ToInt32(sp.OutputValues[0]);
        //    return lst;
        //}
        public static TblLogging GetLoggingByID(object ID)
        {
            return new Select().From(TblLogging.Schema).Where(TblLogging.IdColumn).IsEqualTo(ID).ExecuteSingle<TblLogging>();
        }
    }
}
