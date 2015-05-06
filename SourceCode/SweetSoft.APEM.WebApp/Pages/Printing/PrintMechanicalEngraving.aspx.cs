using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Data;
using System.Web.UI;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintMechanicalEngraving : Page
    {
        private int JobID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                {
                    int a = 0;
                    if (int.TryParse(Request.QueryString["ID"], out a))
                    {
                        return a;
                    }
                }
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
            TblJob job = JobManager.SelectByID(JobID);
            if (job != null)
            {
                TblCustomer customer = CustomerManager.SelectByID(job.CustomerID);
                if (customer != null)
                {
                    ltrClient.Text = customer.Name;
                }
                ltrJobNr.Text = job.JobNumber;
                ltrRevNumber.Text = job.RevNumber.ToString();
                ltrJobName.Text = job.JobName;
                ltrJobDesign.Text = job.Design;
                ltrRootJobNr.Text = job.RootJobNo;
                ltrCommonJobNr.Text = job.CommonJobNumber;
                txtNumber.Text = job.JobNumber;
                txtR.Text = job.RevNumber.ToString();

                TblEngraving engraving = JobEngravingManager.SelectByID(JobID);
                if (engraving != null)
                {
                    ltrJobCoop.Text = engraving.JobCoOrd; 
                }

                TblUser user = UserManager.GetUserByUserName(job.CreatedBy);
                if (user != null)
                    ltrCreateBy.Text = user.DisplayName;

                ltrDate.Text = job.CreatedOn.HasValue ? job.CreatedOn.Value.ToString("dd/MM/yyyy") : string.Empty;

                TblJobSheet jobSheet = JobManager.SelectJobSheetByID(job.JobID);
                if (jobSheet != null)
                {
                    txtCircumference.Text = jobSheet.Circumference.ToString("N2");
                    txtFacewidth.Text = jobSheet.FaceWidth.ToString("N2");
                    txtUnitSizeVertical.Text = jobSheet.UNSizeV.HasValue ? jobSheet.UNSizeV.Value.ToString("N2") : string.Empty;
                    txtUnitSizeHorizontal.Text = jobSheet.UNSizeH.HasValue ? jobSheet.UNSizeH.Value.ToString("N2") : string.Empty;
                    ltrPrint.Text = string.Format("<strong style='font-size: 30px;'>{0}</strong>", jobSheet.Printing);
                }

                TblEngraving eng = JobEngravingManager.SelectByID(job.JobID);
                if (eng != null)
                {

                    txtEngravingStart.Text = eng.EngravingStart != null ? ((double)eng.EngravingStart).ToString("N2") : "";
                    txtEngravingWidth.Text = eng.EngravingWidth != null ? ((double)eng.EngravingWidth).ToString("N2") : "";
                    string _newRemark = eng.SRRemarkEMG.Replace("\n", "<br/>");
                    ltrRemark.Text = _newRemark;
                }

                DataTable dt = JobEngravingManager.EMGSelectAllForPrint(job.JobID);
                rptCylinder.DataSource = dt;
                rptCylinder.DataBind();
            }
        }
    }
}