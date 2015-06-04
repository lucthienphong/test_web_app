using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintJobDetail : Page
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
                bool chkEyeMark = dtModel.Rows[0]["EyeMark"].ToString() == bool.TrueString;

                ltrBacking.Text = dtModel.Rows[0]["BackingName"].ToString();
                ltrBarcodeColour.Text = dtModel.Rows[0]["BarcodeColor"].ToString();
                ltrBarcodeNumber.Text = dtModel.Rows[0]["BarcodeNo"].ToString();
                ltrBarcodeSize.Text = dtModel.Rows[0]["BarcodeSize"].ToString();
                ltrBW.Text = ConvertToDecimal(dtModel.Rows[0]["BWR"], "N2");
                ltrCircumference.Text = dtModel.Rows[0]["Circumference"].ToString();
                ltrClient.Text = dtModel.Rows[0]["Name"].ToString();
                ltrColorTarget.Text = dtModel.Rows[0]["ColorTarget"].ToString();
                ltrCommonJobNr.Text = dtModel.Rows[0]["CommonJobNumber"].ToString();
                ltrCreateBy.Text = dtModel.Rows[0]["CreatedBy"].ToString();
                ltrCustomerContact.Text = dtModel.Rows[0]["ContactName"].ToString();
                ltrCylinderDate.Text = dtModel.Rows[0]["CylinderCreateDate"].ToString();
                ltrCylinderType.Text = dtModel.Rows[0]["TypeOfCylinder"].ToString();
                ltrDate.Text = dtModel.Rows[0]["CreatedDate"].ToString();
                ltrDeliveryNotes.Text = dtModel.Rows[0]["DeilveryNotes"].ToString();
                ltrEMColour.Text = dtModel.Rows[0]["EMColor"].ToString();
                //ltrEMSH.Text = ConvertToDecimal(dtModel.Rows[0]["EMHeight"], "N0");
                //ltrEMSV.Text = ConvertToDecimal(dtModel.Rows[0]["EMWidth"], "N0");
                string sSize = ConvertToDecimal(dtModel.Rows[0]["EMHeight"], "N0") + " x " + ConvertToDecimal(dtModel.Rows[0]["EMWidth"], "N0") + " mm";
                ltrSize.Text = chkEyeMark ? sSize : string.Empty;
                ltrFacewidt.Text = dtModel.Rows[0]["FaceWidth"].ToString();
                ltrIrisProof.Text = dtModel.Rows[0]["IrisProof"].ToString();
                ltrJobCoop.Text = dtModel.Rows[0]["CoopName"].ToString();
                ltrJobName.Text = dtModel.Rows[0]["JobName"].ToString();
                ltrJobDesign.Text = dtModel.Rows[0]["Design"].ToString();
                ltrJobNr.Text = dtModel.Rows[0]["JobNumber"].ToString();

                if ((bool)dtModel.Rows[0]["LeavingAPE"] || string.IsNullOrEmpty(dtModel.Rows[0]["LeavingAPE"].ToString()))
                {
                    ltrLeaving.Text = "Leaving APE";
                }
                else
                {
                    ltrLeaving.Text = "Expected at client";
                }

                ltrOpaqueInkRate.Text = ConvertToDecimal(dtModel.Rows[0]["OpaqueInkRate"], "N2");
                ltrPrinting.Text = dtModel.Rows[0]["Printing"].ToString(); ;
                ltrProofingMaterial.Text = dtModel.Rows[0]["ProofingMaterial"].ToString().Replace("\n", "<br/>");
                ltrRemark.Text = dtModel.Rows[0]["Remark"].ToString().Replace("\n", "<br/>");
                ltrReproDate.Text = dtModel.Rows[0]["ReproCreateDate"].ToString();
                ltrRevNumber.Text = dtModel.Rows[0]["RevNumber"].ToString();
                ltrRootJobNr.Text = dtModel.Rows[0]["RootJobNumber"].ToString() + (string.IsNullOrEmpty(dtModel.Rows[0]["RootJobRevNumber"].ToString()) ? string.Empty : string.Format(" - R: {0}", dtModel.Rows[0]["RootJobRevNumber"].ToString()));
                ltrNumberS_H.Text = dtModel.Rows[0]["NumberOfRepeatH"].ToString();
                ltrNumberS_V.Text = dtModel.Rows[0]["NumberOfRepeatV"].ToString(); ;
                ltrSaleRep.Text = dtModel.Rows[0]["SalePerson"].ToString();
                ltrSupply.Text = dtModel.Rows[0]["SupplyName"].ToString();
                ltrTrapSize.Text = dtModel.Rows[0]["Size"].ToString();
                //ltrUnitSizeH.Text = dtModel.Rows[0]["UNSizeH"].ToString();
                //ltrUnitSizeV.Text = dtModel.Rows[0]["UNSizeV"].ToString();
                string sUnitSize = "V: " + dtModel.Rows[0]["UNSizeH"].ToString() + " x " +
                                   "H: " + dtModel.Rows[0]["UNSizeV"].ToString();
                ltrUnitSize.Text = sUnitSize;
                string eyesmark = string.Empty;
                if (!string.IsNullOrEmpty(dtModel.Rows[0]["EMPonsition"].ToString()))
                    eyesmark = string.Format("<img src='/img/eye-mark/eye-mark-{0}.png' style='height: 15mm; vertical-align: top'/>", dtModel.Rows[0]["EMPonsition"].ToString());
                ltrEyeImageImage.Text = eyesmark;

                if (!string.IsNullOrEmpty(dtModel.Rows[0]["PrintingDirection"].ToString()))
                {
                    string img = string.Empty;
                    switch (dtModel.Rows[0]["PrintingDirection"].ToString())
                    {
                        case "U":
                            img = "up";
                            break;

                        case "L":
                            img = "left";
                            break;

                        case "R":
                            img = "right";
                            break;

                        case "D":
                            img = "down";
                            break;
                    }
                    ltrDirection.Text = string.Format("<img src='/img/{0}.png' class='img-responsive' style='display: inline-block' />", img);

                }
            }
            TblJob j = JobManager.SelectByID(JobID);
            if (j != null)
            {
                string Image64Base = Common.Code128Rendering.MakeBarcode64BaseImage(j.JobBarcode, 1.3, false, false);
                barcodeImage.ImageUrl = Image64Base;

                TblJobSheet js = JobManager.SelectJobSheetByID(JobID);
                if (js != null)
                {
                    ltrActualPrintingMaterial.Text = js.ActualPrintingMaterial;
                    ltrMaterialWidth.Text = js.MaterialWidth;
                }

            }

            LoadCylinder();
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
                    //TblCustomerQuotationDetail cqDetail = CustomerQuotationManager.SelectDetailByID(item.PricingID);
                    //c.PricingName = cqDetail != null ? cqDetail.PricingName : string.Empty;

                    TblCylinderStatus ct = CylinderStatusManager.SelectCylinderStatusByID((short)item.CylinderStatusID);
                    c.Status = ct != null ? ct.CylinderStatusName : string.Empty;

                    TblReference productType = ReferenceTableManager.SelectByID((int)item.ProductTypeID);
                    TblReference processType = ReferenceTableManager.SelectByID((int)item.ProcessTypeID);

                    c.ProcessType = item.PricingID == 0 ? string.Empty : string.Format("{0} - {1}", productType != null ? productType.Code : string.Empty, processType != null ? processType.Code : string.Empty);

                    if (Convert.ToBoolean(item.SteelBase))
                    {
                        c.Base = "New";
                    }
                    else
                        c.Base = "Old";

                    cc.Add(c);
                }
                gvCylinder.DataSource = cc;
                gvCylinder.DataBind();
            }
        }

        public string ConvertToDecimal(object obj, string target)
        {
            string convert = string.Empty;
            decimal temp = 0;
            if (decimal.TryParse(obj.ToString(), out temp))
                convert = temp.ToString(target);
            else
                convert = obj.ToString();
            return convert;
        }
    }
}