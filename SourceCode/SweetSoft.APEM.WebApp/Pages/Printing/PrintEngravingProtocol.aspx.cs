using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintEngravingProtocol : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                BindEngravingData();
                BindCylinderData();
            }
        }

        private void BindEngravingData()
        {
            //Lấy thông tin Job
            TblJob jObj = JobManager.SelectByID(JobID);
            if(jObj != null)
            {
                //Lấy thông tin RootJob
                if (jObj.RootJobID != null)
                {
                    TblJob root = JobManager.SelectByID((int)jObj.RootJobID);
                    ltrRootJob.Text = root != null ? string.Format("{0} - R{1}", root.JobNumber, root.RevNumber) : string.Empty;
                }
                else
                    ltrRootJob.Text = string.Format("{0} - R{1}", jObj.RootJobNo, jObj.RootJobRevNumber);
                ltrCommonJob.Text = jObj.CommonJobNumber;

                //Fill thông tin Job
                ltrJobNumber.Text = string.Format("{0} - R{1}", jObj.JobNumber, jObj.RevNumber);
                ltrJobName.Text = jObj.JobName;
                ltrDesign.Text = jObj.Design;
                ltrCreatedOn.Text = ((DateTime)jObj.CreatedOn).ToString("dd.MM.yyyy");
                //Lấy thông tin khách hàng
                TblCustomer cObj = CustomerManager.SelectByID(jObj.CustomerID);
                if(cObj != null)
                {
                    ltrCustomer.Text = cObj.Name;
                }

                //Lấy thông tin JobSheet
                TblJobSheet jsObj = JobManager.SelectJobSheetByID(JobID);
                if (jsObj != null)
                {
                    ltrCircumference.Text = jsObj.Circumference.ToString("N2");
                    ltrJobOperator.Text = jsObj.ReproOperator;
                    ltrFaceWidth.Text = jsObj.FaceWidth.ToString("N2");
                    ltrSurface.Text = jsObj.Printing == JobPrinting.Surface.ToString() ? "[X]" : "[&nbsp;&nbsp;]";
                    ltrReserse.Text = jsObj.Printing == JobPrinting.Reverse.ToString() ? "[X]" : "[&nbsp;&nbsp;]";
                }

                //Lấy thông tin Engraving
                TblEngraving eObj = JobEngravingManager.SelectByID(JobID);
                if (eObj != null)
                {
                    ltrEngraveOnNut.Text = Convert.ToBoolean(eObj.EngravingOnNut) ? "[X]" : "[&nbsp;&nbsp;]";
                    ltrEngraveCloseToBoarder.Text = Convert.ToBoolean(eObj.EngravingOnBoader) ? "[X]" : "[&nbsp;&nbsp;]";
                    ltrChromeThickness.Text = eObj.ChromeThickness;
                    ltrRoughness.Text = eObj.Roughness;
                    string SizeH = eObj.FileSizeHDLS != null ? ((double)eObj.FileSizeHDLS).ToString("N2") : "0.00";
                    string SizeV = eObj.FileSizeVDLS != null ? ((double)eObj.FileSizeVDLS).ToString("N2") : "0.00";
                    ltrReproSize.Text = string.Format("(H)   {0}mm  X  (V)   {1}mm", SizeH, SizeV);
                    ltrRemark.Text = eObj.SRRemarkDLS;
                }
            }
        }

        private void BindCylinderData()
        {
            List<EngravingTobaccoExtension> source = JobEngravingManager.TobaccoSelectAllForPrint(JobID);
            foreach (EngravingTobaccoExtension item in source)
            {
                //Color
                Literal ltrColor = this.FindControl(string.Format("ltrColor{0}", item.Sequence.ToString())) as Literal;
                if (ltrColor != null)
                    ltrColor.Text = item.Color;
                //CusCylID
                Literal ltrCylID = this.FindControl(string.Format("ltrCylID{0}", item.Sequence.ToString())) as Literal;
                if (ltrCylID != null)
                    ltrCylID.Text = item.CustCylID;
                //Screen
                Literal ltrScreen = this.FindControl(string.Format("ltrScreen{0}", item.Sequence.ToString())) as Literal;
                if (ltrScreen != null)
                    ltrScreen.Text = item.Screen;
                //MasterScreen
                Literal ltrMasterScreen = this.FindControl(string.Format("ltrMasterScreen{0}", item.Sequence.ToString())) as Literal;
                if (ltrMasterScreen != null)
                    ltrMasterScreen.Text = Convert.ToBoolean(item.MasterScreen ?? 0) ? "<span style='border: solid 1px; padding: 1px;'><i class='fa fa-check'></i></span>" : "<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                //Angle
                Literal ltrAngle = this.FindControl(string.Format("ltrAngle{0}", item.Sequence.ToString())) as Literal;
                if (ltrAngle != null)
                    ltrAngle.Text = item.Angle;
                //Elongation
                Literal ltrElongation = this.FindControl(string.Format("ltrElongation{0}", item.Sequence.ToString())) as Literal;
                if (ltrElongation != null)
                    ltrElongation.Text = item.Elongation;
                //Distortion
                Literal ltrDistortion = this.FindControl(string.Format("ltrDistortion{0}", item.Sequence.ToString())) as Literal;
                if (ltrDistortion != null)
                    ltrDistortion.Text = item.Distotion;
                //Resolution
                Literal ltrResolution = this.FindControl(string.Format("ltrResolution{0}", item.Sequence.ToString())) as Literal;
                if (ltrResolution != null)
                    ltrResolution.Text = item.Resolution;
                //Hexagotnal
                Literal ltrHexagotnal = this.FindControl(string.Format("ltrHexagotnal{0}", item.Sequence.ToString())) as Literal;
                if (ltrHexagotnal != null)
                    ltrHexagotnal.Text = item.HexagonalName;
                //ImageSmoothness
                Literal ltrImageSmoothness = this.FindControl(string.Format("ltrImageSmoothness{0}", item.Sequence.ToString())) as Literal;
                if (ltrImageSmoothness != null)
                    ltrImageSmoothness.Text = Convert.ToBoolean(item.ImageSmoothness ?? 0) ? "<span style='border: solid 1px; padding: 1px;'><i class='fa fa-check'></i></span>" : "<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                //UnsharpMarking
                Literal ltrUnsharpMarking = this.FindControl(string.Format("ltrUnsharpMarking{0}", item.Sequence.ToString())) as Literal;
                if (ltrUnsharpMarking != null)
                    ltrUnsharpMarking.Text = item.UnsharpMasking;
                //Antialiasing
                Literal ltrAntialiasing = this.FindControl(string.Format("ltrAntialiasing{0}", item.Sequence.ToString())) as Literal;
                if (ltrAntialiasing != null)
                    ltrAntialiasing.Text = item.Antialiasing;
                //LineworkWidening
                Literal ltrLineWorkWidening = this.FindControl(string.Format("ltrLineWorkWidening{0}", item.Sequence.ToString())) as Literal;
                if (ltrLineWorkWidening != null)
                    ltrLineWorkWidening.Text = item.LineworkWidening;
                //EngravingStart
                Literal ltrEngravingStart = this.FindControl(string.Format("ltrEngravingStart{0}", item.Sequence.ToString())) as Literal;
                if (ltrEngravingStart != null)
                    ltrEngravingStart.Text = item.EngravingStart;
                //EngravingWidth
                Literal ltrEngravingWidth = this.FindControl(string.Format("ltrEngravingWidth{0}", item.Sequence.ToString())) as Literal;
                if (ltrEngravingWidth != null)
                    ltrEngravingWidth.Text = item.EngravingWidth;
                //CellShape
                Literal ltrCellShape = this.FindControl(string.Format("ltrCellShape{0}", item.Sequence.ToString())) as Literal;
                if (ltrCellShape != null)
                    ltrCellShape.Text = item.CellShapeName;
                //Gradition
                Literal ltrGradation = this.FindControl(string.Format("ltrGradation{0}", item.Sequence.ToString())) as Literal;
                if (ltrGradation != null)
                    ltrGradation.Text = item.GraditionName;
                //Gamma
                Literal ltrGamma = this.FindControl(string.Format("ltrGamma{0}", item.Sequence.ToString())) as Literal;
                if (ltrGamma != null)
                    ltrGamma.Text = item.Gamma;
                //CellShape
                Literal ltrLaser = this.FindControl(string.Format("ltrLaser{0}", item.Sequence.ToString())) as Literal;
                if (ltrLaser != null)
                {
                    ltrLaser.Text = Convert.ToBoolean(item.LaserA ?? 0) ? "A <span style='border: solid 1px; padding: 1px;'><i class='fa fa-check'></i></span>" : "A <span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
                    ltrLaser.Text += "&nbsp;" + (Convert.ToBoolean(item.LaserB ?? 0) ? "B <span style='border: solid 1px; padding: 1px;'><i class='fa fa-check'></i></span>" : "B <span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
                }
                //CellWidth
                Literal ltrCellWidth = this.FindControl(string.Format("ltrCellWidth{0}", item.Sequence.ToString())) as Literal;
                if (ltrCellWidth != null)
                    ltrCellWidth.Text = item.CellWidth;
                //ChannelWidth
                Literal ltrChannelWidth = this.FindControl(string.Format("ltrChannelWidth{0}", item.Sequence.ToString())) as Literal;
                if (ltrChannelWidth != null)
                    ltrChannelWidth.Text = item.ChannelWidth;
                //CellDepth
                Literal ltrCellDepth = this.FindControl(string.Format("ltrCellDepth{0}", item.Sequence.ToString())) as Literal;
                if (ltrCellDepth != null)
                    ltrCellDepth.Text = item.CellDepth;
                //EngravingTime
                Literal ltrEngravingTime = this.FindControl(string.Format("ltrEngravingTime{0}", item.Sequence.ToString())) as Literal;
                if (ltrEngravingTime != null)
                    ltrEngravingTime.Text = item.EngravingTime;
                //Beam
                Literal ltrBeam = this.FindControl(string.Format("ltrBeam{0}", item.Sequence.ToString())) as Literal;
                if (ltrBeam != null)
                    ltrBeam.Text = item.Beam;
                //Threshold
                Literal ltrThreshold = this.FindControl(string.Format("ltrThreshold{0}", item.Sequence.ToString())) as Literal;
                if (ltrThreshold != null)
                    ltrThreshold.Text = item.Threshold;
                //CheckedBy
                Literal ltrCheckedBy = this.FindControl(string.Format("ltrCheckedBy{0}", item.Sequence.ToString())) as Literal;
                if (ltrCheckedBy != null)
                    ltrCheckedBy.Text = item.CheckedBy;
                //CheckedOn
                Literal ltrCheckedOn = this.FindControl(string.Format("ltrCheckedOn{0}", item.Sequence.ToString())) as Literal;
                if (ltrCheckedOn != null)
                    ltrCheckedOn.Text = item.CheckedOn != null ? ((DateTime)item.CheckedOn).ToString("dd.MM.yyyy") : string.Empty;
            }
        }
    }
}