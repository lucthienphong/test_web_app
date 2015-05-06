using System;
using System.Globalization;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.UI;
using SweetSoft.APEM.WebApp.Common;
using System.Collections.Generic;
using SweetSoft.APEM.Core;

namespace SweetSoft.APEM.WebApp.Pages
{
    public partial class GridViewPager : System.Web.UI.UserControl
    {
        private GridviewExtension _gridView;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set pagesize to dropdownlist
            Page.PreRender += new EventHandler(Page_PreRender);
            Control c = Parent;
            while (c != null)
            {
                if (c is GridviewExtension)
                {
                    _gridView = (GridviewExtension)c;
                    break;
                }
                c = c.Parent;
            }
        }

        protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_gridView == null)
            {
                return;
            }
            DropDownList dropdownlistpagersize = (DropDownList)sender;
            ApplicationContext.Current.CurrentPageSize = dropdownlistpagersize.SelectedValue;
            _gridView.PageSize = Convert.ToInt32(dropdownlistpagersize.SelectedValue, CultureInfo.CurrentCulture);
            ((ModalBasePage)this.Page).BindData();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (_gridView != null)
            {
                DropDownListPageSize.SelectedValue = ApplicationContext.Current.CurrentPageSize;
                _gridView.PageSize = Convert.ToInt32(ApplicationContext.Current.CurrentPageSize);

                int totalRow = _gridView.VirtualItemCount;
                int pageSize = _gridView.PageSize;
                int pageIndex = _gridView.PageIndex + 1;
                int totalPage = _gridView.PageCount;
                //DropDownListPageSize.SelectedValue = _gridView.PageSize.ToString(CultureInfo.CurrentCulture);
                lbTotalRow.Text = " results (Total " + totalRow.ToString("N0") + " results)";
                createPagingButtons(totalPage, pageIndex);
            }
        }

        private void createPagingButtons(int totalPage, int currentPage)
        {
            //PAGE INDEX
            int index = currentPage == 0 ? 0 : (currentPage - 1) / 5;
            for (int i = 1; i <= 5; i++)
            {
                if (index * 5 + i > totalPage)
                {
                    HtmlGenericControl li = (HtmlGenericControl)this.FindControl("li" + i.ToString());
                    li.Visible = false;
                }
                else
                {
                    //li
                    HtmlGenericControl li = (HtmlGenericControl)this.FindControl("li" + i.ToString());
                    li.Visible = true;
                    //a
                    LinkButton a = (LinkButton)this.FindControl("btn" + i.ToString());
                    a.Text = (index * 5 + i).ToString();
                    a.CommandArgument = (index * 5 + i).ToString();

                    if (index * 5 + i == currentPage)
                    {
                        li.Attributes.Add("class", "active");
                        a.Enabled = false;
                        //Label lb = new Label();
                        //lb.Text = (index * 5 + i).ToString();
                        //li.Controls.Add(lb);
                    }
                }
            }
            //FIRST, PREV, NEXT, LAST
            if (totalPage <= 5)
            {
                liFirst.Visible = false;
                liPrev.Visible = false;
                liNext.Visible = false;
                liLast.Visible = false;
            }
            else if (currentPage <= 5)
            {
                liFirst.Visible = false;
                liPrev.Visible = false;
            }
            else if (totalPage/5 == currentPage/5 && currentPage%5 != 0)
            {
                liNext.Visible = false;
                liLast.Visible = false;
            }
            else
            {
                liFirst.Visible = true;
                liPrev.Visible = true;
                liNext.Visible = true;
                liLast.Visible = true;
            }
        }
    }
}
