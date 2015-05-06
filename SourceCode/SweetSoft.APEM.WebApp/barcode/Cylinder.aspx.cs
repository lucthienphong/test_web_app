using Newtonsoft.Json;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetSoft.APEM.WebApp.barcode
{
    public partial class Cylinder : System.Web.UI.Page
    {
        static TblDepartment FindDepartment(TblCylinder cylinder, out bool isEnd)
        {
            List<ProgressItem> lst = CylinderProcessingManager.GetLineProcessForCylinder(cylinder);

            isEnd = false;
            TblCylinderProcessingCollection all = new SubSonic.Select()
            .From(TblCylinderProcessing.Schema).Where(TblCylinderProcessing.CylinderIDColumn)
            .IsEqualTo(cylinder.CylinderID)
                //.OrderDesc(TblCylinderProcessing.FinishedOnColumn.ColumnName)
            .ExecuteAsCollection<TblCylinderProcessingCollection>();
            if (all != null && all.Count > 0)
            {
                int countFinish = 0;
                short deparmentProcess = 0;
                foreach (TblCylinderProcessing item in all)
                {
                    if (item.CylinderStatus == CylinderProcessingStatus.Finish.ToString())
                        countFinish += 1;
                    else if (item.CylinderStatus == CylinderProcessingStatus.Inprogress.ToString())
                        deparmentProcess = item.DepartmentID.Value;
                }

                if (countFinish == all.Count)
                {
                    if (lst != null && lst.Count > 0)
                    {
                        //loop through process line
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var found = all.Where(x => x.DepartmentID.HasValue && x.DepartmentID.ToString() == lst[i].DepartmentId);
                            if (found != null && found.Count() > 0)
                            { }
                            else
                            {
                                deparmentProcess = short.Parse(lst[i].DepartmentId);
                                break;
                            }
                        }
                    }
                }

                if (deparmentProcess > 0)
                    //get last process department
                    return DepartmentManager.SelectByID(deparmentProcess);
                else
                    isEnd = true;
            }
            else
            {
                if (lst != null && lst.Count > 0 && string.IsNullOrEmpty(lst[0].DepartmentId) == false)
                    return DepartmentManager.SelectByID(short.Parse(lst[0].DepartmentId));
            }
            return null;
        }

        static object ProcessCylinder(TblCylinder cylinder, TblDepartment dept)
        {
            if (dept != null)
            {
                #region RegionName

                //main process here
                TblCylinderProcessing cp = new SubSonic.Select()
                .From(TblCylinderProcessing.Schema).Where(TblCylinderProcessing.CylinderIDColumn)
                .IsEqualTo(cylinder.CylinderID)
                .And(TblCylinderProcessing.DepartmentIDColumn).IsEqualTo(dept.DepartmentID)
                .ExecuteSingle<TblCylinderProcessing>();

                TblMachine machine = new SubSonic.Select().Top("1").From(TblMachine.Schema)
                    .Where(TblMachine.DepartmentIDColumn).IsEqualTo(dept.DepartmentID)
                    .ExecuteSingle<TblMachine>();

                //neu chua co ghi nhan
                if (cp == null)
                {
                    cp = new TblCylinderProcessing();
                    cp.CreatedBy = ApplicationContext.Current.User.UserName;
                    cp.CreatedOn = DateTime.Now;
                    cp.CylinderID = cylinder.CylinderID;
                    cp.CylinderStatus = CylinderProcessingStatus.Inprogress.ToString();
                    cp.DepartmentID = dept.DepartmentID;
                    cp.Description = string.Empty;
                    cp.MachineID = machine != null ? machine.Id : (int?)null;
                    CylinderProcessingManager.Insert(cp);

                    return new
                    {
                        barcode = cylinder.CylinderBarcode,
                        dept = dept.DepartmentName,
                        status = cp.CylinderStatus.ToString(),
                        isComplete = false
                    };

                }
                //neu co roi
                else
                {
                    //neu da bat dau
                    if (cp.CylinderStatus == CylinderProcessingStatus.Inprogress.ToString())
                    {
                        cp.CylinderStatus = CylinderProcessingStatus.Finish.ToString();
                        cp.FinishedBy = ApplicationContext.Current.User.UserName;
                        cp.FinishedOn = DateTime.Now;
                        CylinderProcessingManager.Update(cp);

                        return new
                        {
                            barcode = cylinder.CylinderBarcode,
                            dept = dept.DepartmentName,
                            status = cp.CylinderStatus.ToString(),
                            isComplete = false
                        };
                    }
                    else
                    {
                        //da hoan thanh
                        return new
                        {
                            barcode = cylinder.CylinderBarcode,
                            dept = dept.DepartmentName,
                            status = cp.CylinderStatus.ToString(),
                            isComplete = true
                        };
                    }
                }

                #endregion
            }

            return null;
        }

        bool ValidateCylinder(TblCylinder cylinder, out TblDepartment nextDepartment)
        {
            //lay phong ban se xu ly cylinder
            bool isEnd;
            nextDepartment = FindDepartment(cylinder, out isEnd);
            if (isEnd)
            {
                Response.Write("1||" + "Cylinder has been complete.");
                return false;
            }

            //kiem tra co dung phong ban
            if (nextDepartment != null)
            {
                TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
                if (curentUser != null)
                {
                    TblDepartment curDept = curentUser.TblStaff.TblDepartment;
                    if (curDept != null)
                    {
                        if (nextDepartment.DepartmentID != curDept.DepartmentID)
                        {
                            Response.Write("Wrong cylinder to process.||");
                            return false;
                        }


                        //common pocess type
                        if (nextDepartment.ProcessTypeID.HasValue == false || nextDepartment.ProcessTypeID.Value == 0)
                        {
                            return true;
                        }
                        else
                        {
                            if (cylinder.ProcessTypeID > 0 && cylinder.ProductTypeID != null)
                            {
                                if (nextDepartment.ProcessTypeID.HasValue && cylinder.ProcessTypeID.HasValue
                                    && nextDepartment.ProcessTypeID.Value == cylinder.ProcessTypeID.Value)
                                    return true;
                                else if (string.IsNullOrEmpty(nextDepartment.ProductTypeID) == false
                                    && cylinder.ProductTypeID != null &&
                                    nextDepartment.ProductTypeID.Contains(string.Format("--{0}--", cylinder.ProductTypeID)))
                                    return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ApplicationContext.Current.User == null)
            {
                return;
            }

            //System.Threading.Thread.Sleep(5000);

            string code = Request.Form["code"];
            string num = Request.Form["num"];
            string indx = Request.Form["indx"];
            if (string.IsNullOrEmpty(code) == false)
            {
                if (indx == num)
                {
                    string[] arr = code.Split(',');
                    if (arr != null && arr.Length > 0)
                        code = arr[arr.Length - 1];
                }

                TblCylinder cylinder = new SubSonic.Select().From(TblCylinder.Schema)
                    .Where(TblCylinder.CylinderBarcodeColumn).IsEqualTo(code)
                    //.And(TblCylinder.IsClosedColumn).IsEqualTo(false)
                    .ExecuteSingle<TblCylinder>();

                if (cylinder != null)
                {
                    //kiem tra job co bi hu ko
                    if (cylinder.TblJob != null && cylinder.TblJob.IsClosed == 1)
                    {
                        Response.Write("Wrong cylinder !||");
                        return;
                    }
                }

                if (cylinder != null)
                {
                    TblDepartment nextDepartment;
                    bool isValid = ValidateCylinder(cylinder, out nextDepartment);
                    if (isValid == false)
                        return;

                    int count = 0;
                    int.TryParse(num, out count);
                    if (count > 0)
                    {
                        int index = 0;
                        int.TryParse(indx, out index);
                        if (index > 0)
                        {
                            TblUser curentUser = UserManager.GetUserByUserName(ApplicationContext.Current.UserName);
                            if (curentUser != null)
                            {
                                if (index == count)
                                {
                                    code = Request.Form["code"];
                                    string[] arr = code.Split(',');
                                    if (arr != null && arr.Length > 0)
                                    {
                                        List<object> lstResult = new List<object>();
                                        object val = null;
                                        foreach (string item in arr)
                                        {
                                            cylinder = new SubSonic.Select().From(TblCylinder.Schema)
                                                    .Where(TblCylinder.CylinderBarcodeColumn).IsEqualTo(item)
                                                //.And(TblCylinder.IsClosedColumn).IsEqualTo(false)
                                                    .ExecuteSingle<TblCylinder>();
                                            if (cylinder != null)
                                            {
                                                val = ProcessCylinder(cylinder, nextDepartment);
                                                if (val != null)
                                                    lstResult.Add(val);
                                            }
                                        }
                                        if (lstResult != null && lstResult.Count > 0)
                                        {
                                            Response.Write(JsonConvert.SerializeObject(lstResult));
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    //main process here
                                    TblCylinderProcessing cp = new SubSonic.Select()
                                    .From(TblCylinderProcessing.Schema).Where(TblCylinderProcessing.CylinderIDColumn)
                                    .IsEqualTo(cylinder.CylinderID)
                                    .And(TblCylinderProcessing.DepartmentIDColumn).IsEqualTo(nextDepartment.DepartmentID)
                                    .ExecuteSingle<TblCylinderProcessing>();

                                    Response.Write(JsonConvert.SerializeObject(new
                                    {
                                        barcode = cylinder.CylinderBarcode,
                                        dept = curentUser.TblStaff.TblDepartment.DepartmentName,
                                        //status = cp != null ? cp.CylinderStatus.ToString() : "",
                                        isComplete = cp != null && cp.CylinderStatus.ToString() == CylinderProcessingStatus.Finish.ToString() ? true : false
                                    }));
                                }
                                return;
                            }
                        }
                        else
                        {
                            Response.Write("Wrong index !||");
                            return;
                        }
                    }
                    else
                    {
                        Response.Write("Must select number !||");
                        return;
                    }
                    Response.Write("1||" + cylinder.CylinderID);
                }
                else
                    Response.Write("Wrong barcode !||");
            }
            else
                Response.Write("Please input barcode !||");
        }
    }
}