using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace SweetSoft.APEM.WebApp.barcode
{
    public partial class Job : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ApplicationContext.Current.User == null)
            {
                return;
            }

            string code = Request.Form["code"];
            if (string.IsNullOrEmpty(code) == false)
            {
                TblJob job = new SubSonic.Select().From(TblJob.Schema)
                    .Where(TblJob.JobBarcodeColumn).IsEqualTo(code)
                    .And(TblJob.IsClosedColumn).IsEqualTo(false)
                    .ExecuteSingle<TblJob>();
                if (job != null)
                {

                    TblJobProcess jp = new SubSonic.Select().From(TblJobProcess.Schema)
                        .Where(TblJobProcess.JobIDColumn).IsEqualTo(job.JobID)
                        .ExecuteSingle<TblJobProcess>();
                    //da duoc quet roi
                    if (jp != null)
                    {
                        //chua hoan thanh
                        if (string.IsNullOrEmpty(jp.FinishedBy))
                        {
                            jp.FinishedOn = DateTime.Now;
                            jp.FinishedBy = ApplicationContext.Current.User.UserName;
                            jp = JobProcessManager.Update(jp);
                            if (jp != null)
                            {
                                Response.Write(String.Format("1||Job finished at {0}.||{1}",
                                    DateTime.Now, job.JobID));

                                //save to notification
                                RealtimeNotificationManager
                                    .CreateOrDismissNotification(job.ToJSONString(),
                                    CommandType.ScanBarcode.ToString(),
                                    ApplicationContext.Current.User.UserName,
                                    null, null, false,
                                    ResourceTextManager.GetApplicationText(ResourceText.JOB_NOTIFICATION_SCAN_BARCODE_COMPLETE),
                                    "Job");

                                return;
                            }
                        }
                        else
                        {
                            Response.Write(String.Format("1||Job finished at {0}.||{1}",
                                jp.FinishedOn.Value, job.JobID));
                            return;
                        }
                    }
                    else
                    {
                        //chua co du lieu thi se tao moi
                        jp = new TblJobProcess();
                        jp.JobID = job.JobID;
                        jp.StartedBy = ApplicationContext.Current.User.UserName;
                        jp.StartedOn = DateTime.Now;
                        jp = JobProcessManager.Insert(jp);
                        if (jp != null)
                        {
                            Response.Write(String.Format("1||Job started at {0}.||{1}",
                               DateTime.Now, job.JobID));

                            //save to notification
                            RealtimeNotificationManager
                                .CreateOrDismissNotification(job.ToJSONString(),
                                CommandType.ScanBarcode.ToString(),
                                ApplicationContext.Current.User.UserName,
                                null, null, false,
                                ResourceTextManager.GetApplicationText(ResourceText.JOB_NOTIFICATION_SCAN_BARCODE_START),
                                "Job");

                            return;
                        }
                    }
                    Response.Write("1||" + job.JobID);
                }
                else
                    Response.Write("Wrong barcode !||");
            }
            else
                Response.Write("Please input barcode !||");
        }
    }
}