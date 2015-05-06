using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SweetSoft.APEM.WebApp.Pages.Printing
{
    public partial class PrintCylinderCard : Page
    {
        private int JobID
        {
            get
            {
                if (Request.QueryString["JobID"] != null)
                {
                    int a = 0;
                    if (int.TryParse(Request.QueryString["JobID"], out a))
                    {
                        return a;
                    }
                }
                return 0;
            }
        }

        private string ListCylinderID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                {
                    return Request.QueryString["ID"];
                }
                return string.Empty;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = ListCylinderID;
            LoadCylinderCards(ListCylinderID);
        }

        protected void rptCylinder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal ltrCylSeq = e.Item.FindControl("ltrCylSeq") as Literal;
            //Literal ltrCylNr = e.Item.FindControl("ltrCylNr") as Literal;
            Literal ltrColor = e.Item.FindControl("ltrColor") as Literal;
            Literal ltrStatus = e.Item.FindControl("ltrStatus") as Literal;
            Literal ltrDiameter = e.Item.FindControl("ltrDiameter") as Literal;
            Literal ltrCylBarcode = e.Item.FindControl("ltrCylBarcode") as Literal;
            Literal ltrCylID = e.Item.FindControl("ltrCylID") as Literal;
            Literal ltrCustSteelBase = e.Item.FindControl("ltrCustSteelBase") as Literal;
            Literal ltrProcessType = e.Item.FindControl("ltrProcessType") as Literal;

            TblCylinderCollectionModel c = e.Item.DataItem as TblCylinderCollectionModel;
            ltrCylSeq.Text = c.objCylinder.Sequence.ToString();
            //ltrCylNr.Text = c.objCylinder.CylinderNo;
            ltrColor.Text = c.objCylinder.Color;
            ltrStatus.Text = c.Status;
            ltrDiameter.Text = c.objCylinder.Dirameter.ToString("N2");
            ltrCylID.Text = c.objCylinder.CusCylinderID;
            ltrCylBarcode.Text = c.objCylinder.CylinderBarcode;
            ltrCustSteelBase.Text = c.objCylinder.CusSteelBaseID;
            ltrProcessType.Text = c.ProcessType;
        }

        protected void rptPrintCylinderCard_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (Session["JobPrinting"] != null)
            {
                TblJob j = Session["JobPrinting"] as TblJob;

                #region Thong tin chung

                Literal ltrCompanyName = e.Item.FindControl("ltrCompanyName") as Literal;
                Literal ltrClient = e.Item.FindControl("ltrClient") as Literal;
                Literal ltrJobNr = e.Item.FindControl("ltrJobNr") as Literal;
                Literal ltrRevNumber = e.Item.FindControl("ltrRevNumber") as Literal;
                Literal ltrJobName = e.Item.FindControl("ltrJobName") as Literal;
                Literal ltrJobDesign = e.Item.FindControl("ltrJobDesign") as Literal;
                Literal ltrCreateBy = e.Item.FindControl("ltrCreateBy") as Literal;
                Literal ltrDate = e.Item.FindControl("ltrDate") as Literal;
                Literal ltrRemark = e.Item.FindControl("ltrRemark") as Literal;
                ltrCompanyName.Text = SettingManager.GetSettingValue(SettingNames.CompanyName);
                ltrJobName.Text = j.JobName;
                ltrJobDesign.Text = j.Design;
                ltrJobNr.Text = j.JobNumber;
                ltrRevNumber.Text = j.RevNumber.ToString();
                ltrDate.Text = j.CreatedOn != null ? ((DateTime)j.CreatedOn).ToString("dd/MM/yyyy") : "";
                //ltrRemark.Text = j.Remark.Replace("\n","<br/>");

                TblCustomer cu = CustomerManager.SelectByID(j.CustomerID);
                if (cu!=null)
                {
                    ltrClient.Text = cu.Name;
                }

                if (Session["Staff"] != null)
                {
                    TblStaff s = Session["Staff"] as TblStaff;
                    ltrCreateBy.Text = s.FirstName + " " + s.LastName;
                }

                #endregion Thong tin chung

                #region tung cylinder rieng biet
                Image barcodeImage = e.Item.FindControl("barcodeImage") as Image;
                Literal ltrCircumference = e.Item.FindControl("ltrCircumference") as Literal;
                Literal ltrFacewidt = e.Item.FindControl("ltrFacewidt") as Literal;

                TblCylinderCollectionModel cy = e.Item.DataItem as TblCylinderCollectionModel;

                if (Session["JobSheet"] != null)
                {
                    TblJobSheet js = Session["JobSheet"] as TblJobSheet;
                    ltrCircumference.Text = js.Circumference.ToString("N2");
                    ltrFacewidt.Text = js.FaceWidth.ToString("N2");
                }

                Literal ltrHeaderCylSeq = e.Item.FindControl("ltrHeaderCylSeq") as Literal;
                //Literal ltrHeaderCylNr = e.Item.FindControl("ltrHeaderCylNr") as Literal;
                Literal ltrHeaderColor = e.Item.FindControl("ltrHeaderColor") as Literal;
                Literal ltrHeaderStatus = e.Item.FindControl("ltrHeaderStatus") as Literal;
                Literal ltrHeaderDiameter = e.Item.FindControl("ltrHeaderDiameter") as Literal;

                Literal ltrHeaderCylBarcode = e.Item.FindControl("ltrHeaderCylBarcode") as Literal;
                Literal ltrHeaderCylCus = e.Item.FindControl("ltrHeaderCylCus") as Literal;
                Literal ltrHeaderCustSteelBase = e.Item.FindControl("ltrHeaderCustSteelBase") as Literal;

                Literal ltrHeaderProcessType = e.Item.FindControl("ltrHeaderProcessType") as Literal;

                ltrHeaderCylSeq.Text = cy.objCylinder.Sequence.ToString();
                //ltrHeaderCylNr.Text = cy.objCylinder.CylinderNo;
                ltrHeaderColor.Text = cy.objCylinder.Color;
                ltrHeaderStatus.Text = cy.Status;

                ltrHeaderDiameter.Text = cy.objCylinder.Dirameter.ToString("N2");
                ltrHeaderCylBarcode.Text = cy.objCylinder.CylinderBarcode;
                ltrHeaderCylCus.Text = cy.objCylinder.CusCylinderID;
                ltrHeaderCustSteelBase.Text = cy.objCylinder.CusSteelBaseID;
                ltrHeaderProcessType.Text = cy.ProcessType;

                string Image64Base = Common.Code128Rendering.MakeBarcode64BaseImage(cy.objCylinder.CylinderBarcode, 1.3, false, false);
                barcodeImage.ImageUrl = Image64Base;
                if (Session["Cylinders"] != null)
                {
                    Repeater rptCylinder = e.Item.FindControl("rptCylinder") as Repeater;
                    List<TblCylinderCollectionModel> cys = (List<TblCylinderCollectionModel>)Session["Cylinders"];
                    rptCylinder.DataSource = cys;
                    rptCylinder.DataBind();
                }

                #endregion tung cylinder rieng biet
            }
        }

        private void LoadCylinderCards(string ListCylinderID)
        {
            string[] idList = ListCylinderID.Split(',');
            TblJob j = JobManager.SelectByID(JobID);
            DataTable dt = CylinderManager.SelectAll(JobID);

            if (j != null)
            {
                Session["JobPrinting"] = j;
                TblUser u = UserManager.GetUserByUserName(j.CreatedBy);
                if (u != null)
                {
                    TblStaff s = StaffManager.SelectByID(u.UserID);
                    Session["Staff"] = s;
                }
                TblJobSheet js = JobManager.SelectJobSheetByID(JobID);
                if (js != null)
                {
                    Session["JobSheet"] = js;
                }
            }
            TblCylinderCollection ccol = CylinderManager.GetCylindersByJobID(JobID, true);
            if (ccol != null && ccol.Count > 0)
            {
                List<TblCylinderCollectionModel> ccolTemp = new List<TblCylinderCollectionModel>();
                List<TblCylinderCollectionModel> ccolForEachPageTemp = new List<TblCylinderCollectionModel>();
                foreach (var i in idList)
                {
                    int a = 0;
                    if (int.TryParse(i, out a))
                    {
                        TblCylinder cTemp = ccol.Where(q => q.CylinderID == a).FirstOrDefault();
                        if (cTemp != null)
                        {                        
                            TblCylinderCollectionModel temp = new TblCylinderCollectionModel();
                            temp.objCylinder = cTemp;
                            TblCylinderStatus cs = CylinderStatusManager.SelectCylinderStatusByID((short)cTemp.CylinderStatusID);
                            temp.Status = cs.CylinderStatusName;

                            DataRow result = dt.Select("CylinderID = " + a.ToString()).Single<DataRow>();
                            temp.ProcessType = result["CylType"].ToString();

                            ccolTemp.Add(temp);
                        }
                    }
                }

                foreach (var item in ccol)
                {
                    TblCylinderCollectionModel temp = new TblCylinderCollectionModel();
                    temp.objCylinder = item;
                    TblCylinderStatus cs = CylinderStatusManager.SelectCylinderStatusByID((short)item.CylinderStatusID);
                    temp.Status = cs.CylinderStatusName;

                    DataRow result = dt.Select("CylinderID = " + item.CylinderID.ToString()).Single<DataRow>();
                    temp.ProcessType = result["CylType"].ToString();

                    ccolForEachPageTemp.Add(temp);
                }

                Session["Cylinders"] = ccolForEachPageTemp;

                rptPrintCylinderCard.DataSource = ccolTemp;
                rptPrintCylinderCard.DataBind();
            }
        }

        //protected string ShowNumberFormat(object obj)
        //{
        //    string strPrice = "0";
        //    if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
        //    { decimal price = 0; decimal.TryParse(obj.ToString(), out price); strPrice = price > 0 ? price.ToString("N2") : "0"; }
        //    return strPrice;
        //}
    }
}