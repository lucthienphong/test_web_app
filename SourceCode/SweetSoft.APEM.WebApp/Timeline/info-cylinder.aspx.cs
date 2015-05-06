using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Timeline
{
    public partial class info_cylinder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string data = Request.Form["Id"];//cylinder id
            if (!string.IsNullOrEmpty(data))
            {
                int id = 0;
                int.TryParse(data, out id);
                if (id > 0)
                {
                    List<CylinderProgress> lst = GetInfoCylinderProcess(id);
                    if (lst != null && lst.Count > 0)
                    {
                        Response.ContentType = "application/json";
                        //Response.Write(JsonConvert.SerializeObject(lst, Formatting.Indented));
                        Response.Write(JsonConvert.SerializeObject(lst, Formatting.None,
                            new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy hh:mm:ss tt" }));
                    }
                }
            }
        }

        static List<CylinderProgress> GetInfoCylinderProcess(int cylinderId)
        {
            List<CylinderProgress> lst = new List<CylinderProgress>();
            TblCylinder cylinder = CylinderManager.SelectByID(cylinderId);
            if (cylinder != null)
            {
                TblCylinderProcessingCollection allRecord = CylinderProcessingManager.GetAllRecordForCylinder(cylinderId);
                List<ProgressItem> lstP = CylinderProcessingManager.GetLineProcessForCylinder(cylinder);
                if (lstP != null && lstP.Count > 0)
                {
                    if (allRecord != null && allRecord.Count > 0)
                    {
                        foreach (ProgressItem progressItem in lstP)
                        {
                            var found = allRecord.Where(x => x.DepartmentID != null && x.DepartmentID.ToString() == progressItem.DepartmentId);
                            if (found != null && found.Count() > 0)
                            {
                                int machineId = found.First().MachineID ?? 0;
                                lst.Add(new CylinderProgress()
                                {
                                    DepartmentId = progressItem.DepartmentId,
                                    DepartmentName = progressItem.DepartmentName,
                                    Status = found.First().FinishedOn.HasValue ? "Finish" : (found.First().CreatedOn.HasValue ? "Inprogress" : ""),
                                    FinishedOn = found.First().FinishedOn,
                                    StartedOn = found.First().CreatedOn,
                                    MachineName = machineId > 0 ? (found.First().TblMachine != null ? found.First().TblMachine.Name : string.Empty) : string.Empty,
                                    TimeProcess = (found.First().CreatedOn.HasValue && found.First().FinishedOn.HasValue) ? ((found.First().FinishedOn.Value - found.First().CreatedOn.Value).TotalHours).ToString("F2") + " hours" : string.Empty
                                });
                            }
                            else
                                lst.Add(new CylinderProgress()
                                {
                                    DepartmentId = progressItem.DepartmentId,
                                    DepartmentName = progressItem.DepartmentName,
                                    Status = ""
                                });
                        }
                    }
                    else
                    {
                        foreach (ProgressItem progressItem in lstP)
                        {
                            lst.Add(new CylinderProgress()
                            {
                                DepartmentId = progressItem.DepartmentId,
                                DepartmentName = progressItem.DepartmentName,
                                Status = ""
                            });
                        }
                    }
                }
            }
            return lst;
        }
    }
}