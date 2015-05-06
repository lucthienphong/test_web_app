using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.Core.UI
{
    public class GridviewExtension : GridView
    {
        private const string _virtualItemCount = "virtualItemCount";
        private const string _virtualPageCount = "_virtualPageCount";
        private const string _currentPageIndex = "currentPageIndex";

        [Browsable(true), Category("Custom")]
        [Description("Set the virtual item count for this grid")]
        public int VirtualItemCount
        {
            get
            {
                if (ViewState[_virtualItemCount] == null)
                    ViewState[_virtualItemCount] = -1;
                return Convert.ToInt32(ViewState[_virtualItemCount]);
            }
            set
            {
                ViewState[_virtualItemCount] = value;
            }
        }
        private int CurrentPageIndex
        {
            get
            {
                if (ViewState[_currentPageIndex] == null)
                    ViewState[_currentPageIndex] = 0;
                return Convert.ToInt32(ViewState[_currentPageIndex]);
            }
            set
            {
                ViewState[_currentPageIndex] = value;
            }
        }

        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;
                this.CurrentPageIndex = this.PageIndex;
            }
        }

        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            if (CustomPaging)
            {
                pagedDataSource.AllowCustomPaging = CustomPaging;
                pagedDataSource.VirtualCount = this.VirtualItemCount;
                pagedDataSource.CurrentPageIndex = this.CurrentPageIndex;
            }
            base.InitializePager(row, columnSpan, pagedDataSource);
        }

        public bool CustomPaging
        {
            get { return (this.VirtualItemCount != -1); }
        }
    }
}
