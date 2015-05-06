using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace SweetSoft.APEM.WebApp.barcode
{
    public partial class Default : Page
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
                    .ExecuteSingle<TblJob>();
                if (job != null)
                {
                    #region process

                    TblJobProcessingCollection allRecords = new SubSonic.Select()
                    .From(TblJobProcessing.Schema).Where(TblJobProcessing.JobIDColumn).IsEqualTo(job.JobID)
                    .ExecuteAsCollection<TblJobProcessingCollection>();
                    if (allRecords != null && allRecords.Count > 0)
                    {
                        var found = allRecords.Where(x => x.JobStatus != JobProcessingStatus.Finish.ToString());
                        if (found != null && found.Count() > 0)
                        {
                            TblJobProcessing jp = found.First();
                            #region inprogress

                            if (jp.JobStatus == JobProcessingStatus.Inprogress.ToString())
                            {
                                string data = JobProcessingManager.GetDataSelectNextDepartment(jp.DepartmentID.Value);

                                if (string.IsNullOrEmpty(data))
                                {
                                    //het phong ban de lam
                                    //cap nhat tinh trang la hoan thanh
                                    jp.JobStatus = JobProcessingStatus.Finish.ToString();
                                    JobProcessingManager.Update(jp);

                                    Response.Write("1||" + "Job finished.||" + job.JobID);
                                    return;
                                }
                                else
                                {
                                    //da hoan thanh nhung doi de lam job tiep theo
                                    jp.JobStatus = JobProcessingStatus.Waiting.ToString();
                                    jp.FinishedBy = ApplicationContext.Current.User.UserName;
                                    jp.FinishedOn = DateTime.Now;
                                    JobProcessingManager.Update(jp);
                                    Response.Write("1||" + "Job finished at [" + jp.TblDepartment.DepartmentName + "].||" + job.JobID);
                                    return;
                                }
                            }

                            #endregion
                            else
                            {
                                #region cho doi nhan vien chon phong ban tiep theo
                                string data = JobProcessingManager.GetDataSelectNextDepartment(jp.DepartmentID.Value);

                                if (string.IsNullOrEmpty(data))
                                {
                                    //het phong ban de lam
                                    //cap nhat tinh trang la hoan thanh
                                    jp.JobStatus = JobProcessingStatus.Finish.ToString();
                                    jp.FinishedOn = DateTime.Now;
                                    jp.FinishedBy = ApplicationContext.Current.User.UserName;
                                    JobProcessingManager.Update(jp);

                                    Response.Write("1||" + "Job finished.||" + job.JobID);
                                    return;
                                }
                                else
                                {
                                    int DepartmentID = 0;
                                    if (data.Length < 10)
                                        int.TryParse(data, out DepartmentID);
                                    #region chi co 1 phong ban ke tiep
                                    if (DepartmentID > 0)
                                    {
                                        //cap nhat tinh trang la hoan thanh va tao tien trinh moi o phong ban moi
                                        jp.JobStatus = JobProcessingStatus.Finish.ToString();
                                        JobProcessingManager.Update(jp);

                                        TblJobProcessing jpNext = new TblJobProcessing();
                                        jpNext.CreatedBy = ApplicationContext.Current.User.UserName;
                                        jpNext.CreatedOn = DateTime.Now;
                                        jpNext.DepartmentID = DepartmentID;
                                        jpNext.Description = string.Empty;
                                        jpNext.JobID = job.JobID;
                                        jpNext.JobStatus = JobProcessingStatus.Inprogress.ToString();
                                        JobProcessingManager.Insert(jpNext);
                                        Response.Write("1||" + "Job start at [" + jpNext.TblDepartment.DepartmentName + "].||" + job.JobID);
                                        return;
                                    }
                                    #endregion
                                    #region nhieu phong ban
                                    else if (data.Length > 0)
                                    {
                                        #region kiem tra xem nhan vien da chon phong ban
                                        string requestDepart = Request.Form["depart"];
                                        if (string.IsNullOrEmpty(requestDepart) == false)
                                        {
                                            string temp = string.Empty;
                                            try
                                            {
                                                temp = SecurityHelper.Decrypt(Server.HtmlDecode(requestDepart));
                                            }
                                            catch { }
                                            int.TryParse(temp, out DepartmentID);
                                            if (DepartmentID > 0)
                                            {
                                                //cap nhat tinh trang la hoan thanh va tao tien trinh moi o phong ban moi
                                                jp.JobStatus = JobProcessingStatus.Finish.ToString();
                                                JobProcessingManager.Update(jp);

                                                TblJobProcessing jpNext = new TblJobProcessing();
                                                jpNext.CreatedBy = ApplicationContext.Current.User.UserName;
                                                jpNext.CreatedOn = DateTime.Now;
                                                jpNext.DepartmentID = DepartmentID;
                                                jpNext.Description = string.Empty;
                                                jpNext.JobID = job.JobID;
                                                jpNext.JobStatus = JobProcessingStatus.Inprogress.ToString();
                                                JobProcessingManager.Insert(jpNext);
                                                Response.Write("1||" + "Job start at [" + jpNext.TblDepartment.DepartmentName + "].||" + job.JobID);
                                                return;
                                            }
                                        }
                                        #endregion
                                        else
                                        {
                                            //tai len du lieu cho nhan vien chon phong ban
                                            Response.Write(data);
                                            return;
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            #region tim cac phong ban ke tiep
                            string data = JobProcessingManager.GetDataSelectNextDepartment(allRecords[allRecords.Count - 1].DepartmentID.Value);

                            if (string.IsNullOrEmpty(data))
                            {
                                Response.Write("1||" + "Job finished.||" + job.JobID);
                                return;
                            }
                            else
                            {
                                #region chi co 1 phong ban ke tiep
                                int DepartmentID = 0;
                                if (data.Length < 10)
                                    int.TryParse(data, out DepartmentID);
                                if (DepartmentID > 0)
                                {
                                    TblJobProcessing jpNext = new TblJobProcessing();
                                    //jp.CreatedBy = ApplicationContext.Current.User.UserName;
                                    jpNext.CreatedBy = "phamnhatvuong";
                                    jpNext.CreatedOn = DateTime.Now;
                                    jpNext.DepartmentID = DepartmentID;
                                    jpNext.Description = string.Empty;
                                    jpNext.JobID = job.JobID;
                                    jpNext.JobStatus = JobProcessingStatus.Inprogress.ToString();
                                    JobProcessingManager.Insert(jpNext);
                                    Response.Write("1||" + "Job start at [" + jpNext.TblDepartment.DepartmentName + "].||" + job.JobID);
                                    return;
                                }
                                #endregion
                                else if (data.Length > 0)
                                {
                                    //tai len du lieu cho nhan vien chon phong ban
                                    Response.Write(data);
                                    return;
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region tim phong ban dau tien

                        string data = JobProcessingManager.GetDataSelectNextDepartment(0);
                        if (string.IsNullOrEmpty(data))
                        {
                            Response.Write("This job does not undergone any department !||");
                            return;
                        }
                        else
                        {
                            #region chi co 1 phong ban
                            int DepartmentID = 0;
                            if (data.Length < 10)
                                int.TryParse(data, out DepartmentID);
                            if (DepartmentID > 0)
                            {
                                TblJobProcessing jpNext = new TblJobProcessing();
                                //jp.CreatedBy = ApplicationContext.Current.User.UserName;
                                jpNext.CreatedBy = "phamnhatvuong";
                                jpNext.CreatedOn = DateTime.Now;
                                jpNext.DepartmentID = DepartmentID;
                                jpNext.Description = string.Empty;
                                jpNext.JobID = job.JobID;
                                jpNext.JobStatus = JobProcessingStatus.Inprogress.ToString();
                                JobProcessingManager.Insert(jpNext);
                                Response.Write("1||" + "Job start at [" + jpNext.TblDepartment.DepartmentName + "].||" + job.JobID);
                                return;
                            }
                            #endregion
                            else if (data.Length > 0)
                            {
                                //tai len du lieu cho nhan vien chon phong ban
                                Response.Write(data);
                                return;
                            }
                        }

                        #endregion
                    }

                    #endregion

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