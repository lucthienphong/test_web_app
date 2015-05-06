using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintEtchingProtocol : Page
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

            DataTable dtModel = JobManager.GetJobForPrinting(JobID);
            if (dtModel != null && dtModel.Rows.Count > 0)
            {
                ltrCircumference.Text = dtModel.Rows[0]["Circumference"].ToString();
                ltrClient.Text = dtModel.Rows[0]["Name"].ToString();
                ltrCommonJobNr.Text = dtModel.Rows[0]["CommonJobNumber"].ToString();
                ltrCreateBy.Text = dtModel.Rows[0]["CreatedBy"].ToString();
                ltrDate.Text = dtModel.Rows[0]["CreatedDate"].ToString();
                ltrFaceWidth.Text = dtModel.Rows[0]["FaceWidth"].ToString();
                ltrJobName.Text = dtModel.Rows[0]["JobName"].ToString();
                ltrJobDesign.Text = dtModel.Rows[0]["Design"].ToString();
                ltrJobNr.Text = dtModel.Rows[0]["JobNumber"].ToString();
                ltrPrinting.Text = dtModel.Rows[0]["Printing"].ToString();
                ltrRevNumber.Text = dtModel.Rows[0]["RevNumber"].ToString();
                ltrRootJobNr.Text = dtModel.Rows[0]["RootJobNumber"].ToString();

                LoadEngraving();
                //LoadCylinder();
                DataTable dt = JobEngravingManager.EtchingSelectAllForPrint(JobID);
                rptCylinder.DataSource = dt;
                rptCylinder.DataBind();
            }
        }

        private void LoadEngraving()
        {
            TblEngraving obj = JobEngravingManager.SelectByID(JobID);
            if (obj != null)
            {
                ltrChromeThickness.Text = obj.ChromeThickness;
                ltrRoughness.Text = obj.Roughness;
                ltrEngravingStart.Text = obj.EngrStartEtching != null ? ((double)obj.EngrStartEtching).ToString("N2") : string.Empty;
                ltrEngravingWidth.Text = obj.EngrWidthEtching != null ? ((double)obj.EngrWidthEtching).ToString("N2") : string.Empty;
                ltrUnitSizeHor.Text = obj.FileSizeHEtching != null ? ((double)obj.FileSizeHEtching).ToString("N2") : string.Empty;
                ltrUnitSizeVer.Text = obj.FileSizeVEtching != null ? ((double)obj.FileSizeVEtching).ToString("N2") : string.Empty;
                ltrLaserStart.Text = obj.LaserStart;
                ltrLaserOperator.Text = obj.LaserOperator;
                ltrFinalControl.Text = obj.FinalControl;
                ltrRemark.Text = obj.SRRemarkEtching.Replace("\n", "<br/>");
            }
        }

        private void LoadCylinder()
        {
            TblCylinderCollection cy = CylinderManager.SelectCylinderByJobID(JobID);
            if (cy != null && cy.Count() > 0)
            {
                List<TblCylinderCollectionModel> cc = new List<TblCylinderCollectionModel>();
                foreach (var item in cy)
                {
                    TblCylinderCollectionModel c = new TblCylinderCollectionModel();
                    c.objCylinder = item;
                    TblPricing p = PricingManager.SelectByPricingID(item.PricingID);
                    c.PricingName = p != null ? p.PricingName : string.Empty;

                    TblReference ct = ReferenceTableManager.SelectByID(item.ProcessTypeID != null ? (int)item.ProcessTypeID : 0);
                    c.Status = ct != null ? ct.Name : string.Empty;

                    if (Convert.ToBoolean(item.SteelBase))
                    {
                        c.Base = "New";
                    }
                    else
                        c.Base = "Old";

                    cc.Add(c);
                }
                rptCylinder.DataSource = cc;
                rptCylinder.DataBind();
            }
        }
    }
}