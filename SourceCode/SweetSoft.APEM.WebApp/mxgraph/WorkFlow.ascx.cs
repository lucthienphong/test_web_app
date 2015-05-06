using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using SweetSoft.APEM.Core;
using SweetSoft.APEM.Core.Manager;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.APEM.WebApp.Controls
{
    public partial class WorkFlow : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadTruc();
                LoadDepts();
                //LoadGraphicType();
                //LoadMainWorkflow();

                btnConfirmSubmit.Value = ResourceTextManager.GetApplicationText(ResourceText.DIALOG_SUBMIT);
                btnConfirmClose.Value = ResourceTextManager.GetApplicationText(ResourceText.DIALOG_CLOSE);
                lblMessageTitle.InnerText = ResourceTextManager.GetApplicationText(ResourceText.DIALOG_MESSAGEBOX_TITLE);
                btnSaveAndContinue.Value = ResourceTextManager.GetApplicationText(ResourceText.WORKFLOW_SAVE_AND_CONTINUE);
            }
        }

        void LoadDepts()
        {
            TblDepartmentCollection all = DepartmentManager.GetDeptForWorkflow();
            if (all != null && all.Count > 0)
            {
                StringBuilder sbData = new StringBuilder();
                sbData.Append("var dataDepts=[");
                foreach (TblDepartment item in all)
                {
                    sbData.Append("{");
                    Type t = item.GetType();
                    PropertyInfo p = t.GetProperty("DepartmentID", BindingFlags.Public
                                                | BindingFlags.Instance
                                                | BindingFlags.IgnoreCase);

                    if (p != null)
                        sbData.AppendFormat("{0}:'{1}',", "Id", p.GetValue(item, null));
                    else
                        sbData.AppendFormat("{0}:'{1}',", "Id", "");

                    p = t.GetProperty("Code", BindingFlags.Public
                                              | BindingFlags.Instance
                                              | BindingFlags.IgnoreCase);

                    if (p != null)
                        sbData.AppendFormat("{0}:'{1}',", "Code", p.GetValue(item, null));
                    else
                        sbData.AppendFormat("{0}:'{1}',", "Code", item.DepartmentID);

                    p = t.GetProperty("DepartmentName", BindingFlags.Public
                                               | BindingFlags.Instance
                                               | BindingFlags.IgnoreCase);

                    if (p != null)
                        sbData.AppendFormat("{0}:'{1}'", "Name", p.GetValue(item, null));
                    else
                        sbData.AppendFormat("{0}:'{1}'", "Name", "");
                    sbData.Append("},");
                }
                sbData.Remove(sbData.Length - 1, 1);
                sbData.Append("]");
                ltrDataPB.Text = string.Format("<script type='text/javascript'>{0}</script>", sbData);
            }
        }

        /*
        void LoadTruc()
        {
            TblmachineryproducetypeCollection all = MachineryManager.GetAllMachineryProducetype(true);
            if (all != null && all.Count > 0)
            {
                PropertyInfo[] piColl = null;
                object defaultObject;
                StringBuilder sbData = new StringBuilder();
                sbData.Append("var dataTruc=[");
                foreach (Tblmachineryproducetype item in all)
                {
                    sbData.Append("{");
                    Type t = item.GetType();
                    piColl = new PropertyInfo[2];
                    PropertyInfo p = t.GetProperty("Id", BindingFlags.Public
                                                | BindingFlags.Instance
                                                | BindingFlags.IgnoreCase);

                    piColl[0] = p;

                    p = t.GetProperty("Name", BindingFlags.Public
                                               | BindingFlags.Instance
                                               | BindingFlags.IgnoreCase);

                    piColl[1] = p;
                    if (piColl != null && piColl.Length > 0)
                    {
                        foreach (PropertyInfo pi in piColl)
                        {
                            defaultObject = pi.GetValue(item, null);

                            sbData.AppendFormat("{0}:'{1}',", pi.Name, defaultObject.ToString().Replace("'", @"\'"));
                        }

                        sbData.Remove(sbData.Length - 1, 1);
                    }
                    sbData.Append("},");
                }

                //for test
                //sbData.Append("{Id:'60',Name:'test'},");

                sbData.Remove(sbData.Length - 1, 1);
                sbData.Append("]");
                ltrDataTruc.Text = string.Format("<script type='text/javascript'>{0}</script>", sbData);
            }
        }

        void LoadGraphicType()
        {
            TblmachineryproducetypeCollection all = MachineryManager.GetAllGraphicType(true);
            if (all != null && all.Count > 0)
            {
                PropertyInfo[] piColl = null;
                object defaultObject;
                StringBuilder sbData = new StringBuilder();
                sbData.Append("var dataGraphictype=[");
                foreach (Tblmachineryproducetype item in all)
                {
                    sbData.Append("{");
                    Type t = item.GetType();
                    piColl = new PropertyInfo[2];
                    PropertyInfo p = t.GetProperty("Id", BindingFlags.Public
                                                | BindingFlags.Instance
                                                | BindingFlags.IgnoreCase);

                    piColl[0] = p;

                    p = t.GetProperty("Name", BindingFlags.Public
                                               | BindingFlags.Instance
                                               | BindingFlags.IgnoreCase);

                    piColl[1] = p;
                    if (piColl != null && piColl.Length > 0)
                    {
                        foreach (PropertyInfo pi in piColl)
                        {
                            defaultObject = pi.GetValue(item, null);

                            sbData.AppendFormat("{0}:'{1}',", pi.Name, defaultObject.ToString().Replace("'", @"\'"));
                        }

                        sbData.Remove(sbData.Length - 1, 1);
                    }
                    sbData.Append("},");
                }

                //for test
                //sbData.Append("{Id:'30',Name:'test graphic'},");

                sbData.Remove(sbData.Length - 1, 1);
                sbData.Append("]");
                ltrDataGraphictype.Text = string.Format("<script type='text/javascript'>{0}</script>", sbData);
            }
        }
      
        void LoadMainWorkflow()
        {
            SubSonic.SqlQuery qq = new SubSonic.Select().From(TblWorkFlow.Schema);

            TblWorkFlowCollection allWF = qq.ExecuteAsCollection<TblWorkFlowCollection>();
            string ss = qq.BuildSqlStatement();
            if (allWF != null && allWF.Count > 0)
            {
                StringBuilder sbData = new StringBuilder();
                sbData.Append("var dataMainWF=[");
                foreach (Tblworkflow item in allWF)
                {
                    sbData.Append("{");
                    sbData.AppendFormat("Id: '{0}', Name: '{1}'", item.Id, item.ListFromConnection);
                    sbData.Append("},");
                }
                sbData = sbData.Remove(sbData.Length - 1, 1);
                sbData.Append("];");
                ltrDataMainWF.Text = "<script type='text/javascript'>" + sbData.ToString() + "</script>";
            }
        } 
        */
    }
}