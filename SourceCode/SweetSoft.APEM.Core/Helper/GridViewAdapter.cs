using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
//using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SweetSoft.APEM.Core.UI;

namespace SweetSoft.APEM.Core.Helper
{
    public class GridViewAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                        ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PROTECTED        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Extender.AdapterEnabled)
            {
                RegisterScripts();
            }
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, ""); //AspNet-GridView
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderEndTag(writer);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                GridviewExtension gridView = Control as GridviewExtension;
                if (gridView != null)
                {
                    writer.Indent++;
                    writer.WriteLine();
                    writer.WriteBeginTag("table");
                    writer.WriteAttribute("summary", Control.ToolTip);

                    if (!String.IsNullOrEmpty(gridView.CssClass))
                        writer.WriteAttribute("class", gridView.CssClass);

                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    ///////////////////// CAPTION /////////////////////////////

                    if (!String.IsNullOrEmpty(gridView.Caption))
                    {
                        writer.WriteLine();
                        writer.WriteBeginTag("caption");
                        //writer.WriteAttribute("class", "AspNet-GridView-Caption");
                        // since the is no Caption Class property we could add
                        writer.WriteAttribute("class", "caption");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteEncodedText(gridView.Caption);
                        writer.WriteEndTag("caption");
                    }

                    // add header pager
                    writer.Indent++;
                    WritePagerSection(writer, PagerPosition.Top);

                    ArrayList rows = new ArrayList();

                    /* ADDED ON 3/7/07, source: http://forums.asp.net/thread/1518559.aspx */
                    /////////////// EmptyDataTemplate ///////////////////////
                    if (gridView.Rows.Count == 0)
                    {
                        //Control[0].Control[0] s/b the EmptyDataTemplate.
                        if (gridView.HasControls())
                        {
                            if (gridView.Controls[0].HasControls())
                            {
                                Control c = gridView.Controls[0].Controls[0];
                                if (c is GridViewRow)
                                {
                                    rows.Clear();
                                    rows.Add(c);
                                    WriteRows(writer, gridView,
                                              new GridViewRowCollection(rows), "tfoot");
                                }
                            }
                        }
                    }
                    /* END ADD */
                    else
                    {

                        ///////////////////// HEAD /////////////////////////////

                        rows.Clear();
                        if (gridView.ShowHeader && (gridView.HeaderRow != null))
                        {
                            rows.Add(gridView.HeaderRow);
                            WriteRows(writer, gridView, new GridViewRowCollection(rows), "thead");
                        }

                        ///////////////////// FOOT /////////////////////////////

                        rows.Clear();
                        if (gridView.ShowFooter && (gridView.FooterRow != null))
                        {
                            rows.Add(gridView.FooterRow);
                            WriteRows(writer, gridView, new GridViewRowCollection(rows), "tfoot");
                        }

                        ///////////////////// BODY /////////////////////////////

                        WriteRows(writer, gridView, gridView.Rows, "tbody");

                        ////////////////////////////////////////////////////////
                    }

                    // add footer pager
                    writer.Indent--;
                    WritePagerSection(writer, PagerPosition.Bottom);

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("table");

                    //WritePagerSection(writer, PagerPosition.Bottom);

                    writer.Indent--;
                    writer.WriteLine();
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PRIVATE        

        private void RegisterScripts()
        {
        }

        private void WriteRows(HtmlTextWriter writer, GridviewExtension gridView, GridViewRowCollection rows, string tableSection)
        {
            if (rows.Count > 0)
            {
                writer.WriteLine();
                writer.WriteBeginTag(tableSection);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                foreach (GridViewRow row in rows)
                {
                    if (!row.Visible)
                        continue;

                    writer.WriteLine();
                    writer.WriteBeginTag("tr");

                    string className = GetRowClass(gridView, row);
                    if (!String.IsNullOrEmpty(className))
                    {
                        writer.WriteAttribute("class", className);
                    }

                    //  Uncomment the following block of code if you want to add arbitrary attributes.
                    /*
                    foreach (string key in row.Attributes.Keys)
                    {
                            writer.WriteAttribute(key, row.Attributes[key]);
                    }
                    */

                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    foreach (TableCell cell in row.Cells)
                    {
                        DataControlFieldCell fieldCell = cell as DataControlFieldCell;
                        if ((fieldCell != null) && (fieldCell.ContainingField != null))
                        {
                            DataControlField field = fieldCell.ContainingField;
                            if (!field.Visible)
                            {
                                cell.Visible = false;
                            }

                            // Apply item style CSS class
                            TableItemStyle itemStyle;
                            switch (row.RowType)
                            {
                                case DataControlRowType.Header:
                                    itemStyle = field.HeaderStyle;
                                    // Add CSS classes for sorting
                                    SetHeaderCellSortingClass(gridView, field, itemStyle);
                                    break;
                                case DataControlRowType.Footer:
                                    itemStyle = field.FooterStyle;
                                    break;
                                default:
                                    itemStyle = field.ItemStyle;
                                    break;
                            }
                            if (itemStyle != null && !String.IsNullOrEmpty(itemStyle.CssClass))
                            {
                                if (!String.IsNullOrEmpty(cell.CssClass))
                                    cell.CssClass += " ";
                                cell.CssClass += itemStyle.CssClass;
                            }
                        }

                        writer.WriteLine();
                        cell.RenderControl(writer);
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("tr");
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag(tableSection);
            }
        }

        /// <summary>
        /// Sets the header cell's CSS class based on its sortability.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="field">The column.</param>
        /// <param name="itemStyle">The header cell's style.</param>
        /// <remarks>
        /// Added by Damian Edwards 2/Oct/2008 to support styling of sortable and sorted columns via CSS
        /// </remarks>
        private void SetHeaderCellSortingClass(GridviewExtension gridView, DataControlField field, TableItemStyle itemStyle)
        {
            if (!gridView.AllowSorting || String.IsNullOrEmpty(field.SortExpression))
                return;

            string sortCssClass = String.Empty;

            // Add sortable CSS class
            sortCssClass = "sortable";

            if (!String.IsNullOrEmpty(gridView.SortExpression) &&
                !String.IsNullOrEmpty(field.SortExpression) &&
                gridView.SortExpression.Contains(field.SortExpression))
            {
                // Add current sort column CSS class
                string sortDirectionClass =
                    gridView.SortDirection == SortDirection.Ascending ? "asc" : "desc";
                sortCssClass += " sorted " + sortDirectionClass;
            }

            if (!String.IsNullOrEmpty(sortCssClass))
            {
                if (!String.IsNullOrEmpty(itemStyle.CssClass))
                {
                    itemStyle.CssClass = sortCssClass + " " + itemStyle.CssClass;
                }
                else
                {
                    itemStyle.CssClass = sortCssClass;
                }
            }
        }

        /// <summary>
        /// Gets the row's CSS class.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="row">The row.</param>
        /// <returns>The CSS class.</returns>
        /// <remarks>
        /// Modified 2013-07-27 by Stephen J Naughton to use default CSS classes for each row type.
        /// </remarks>
        private string GetRowClass(GridviewExtension gridView, GridViewRow row)
        {
            string className = row.CssClass;

            switch (row.RowType)
            {
                case DataControlRowType.Header:
                    className += gridView.HeaderStyle.CssClass;
                    break;
                case DataControlRowType.Footer:
                    className += gridView.FooterStyle.CssClass;
                    break;
                case DataControlRowType.EmptyDataRow:
                    className += gridView.EmptyDataRowStyle.CssClass;
                    break;
                case DataControlRowType.Separator:
                    // we may want to indicate that there is a Separator here
                    className += " separator "; // need to check that this are not already in use in Bootstrap
                    break;
                case DataControlRowType.Pager:
                    className += gridView.PagerStyle.CssClass;
                    break;
                case DataControlRowType.DataRow:
                    switch (row.RowState)
                    {
                        case DataControlRowState.Normal:
                            className += gridView.RowStyle.CssClass;
                            break;
                        case DataControlRowState.Alternate:
                            className += gridView.AlternatingRowStyle.CssClass;
                            break;
                        case DataControlRowState.Selected | DataControlRowState.Normal:
                        case DataControlRowState.Selected | DataControlRowState.Alternate:
                            className += gridView.SelectedRowStyle.CssClass;
                            break;
                        case DataControlRowState.Edit | DataControlRowState.Normal:
                        case DataControlRowState.Edit | DataControlRowState.Alternate:
                            className += gridView.EditRowStyle.CssClass;
                            break;
                        case DataControlRowState.Insert:
                            // we may want to indicate that there is an insert here
                            className += " insert "; // need to check that this are not already in use in Bootstrap
                            break;
                    }
                    break;
            }

            return className.Trim();
        }

        /// <remarks>
        /// Patch provided by Wizzard to support PagerTemplate (CodePlex issue #3368).
        /// </remarks>
        private void WritePagerSection(HtmlTextWriter writer, PagerPosition pos)
        {
            GridviewExtension gridView = Control as GridviewExtension;
            if (gridView != null && gridView.PageCount >= 1)//Nếu số trang nhiều hơn 1 thì hiện cả 2 loại paging
            {
                string tableHeaderFooter = pos == PagerPosition.Top ? "thead" : "tfoot";
                string tableCell = pos == PagerPosition.Top ? "th" : "td";

                if (gridView.AllowPaging && gridView.PagerSettings.Visible &&
                        ((gridView.PagerSettings.Position == pos) || (gridView.PagerSettings.Position == PagerPosition.TopAndBottom)))
                {
                    GridViewRow pagerRow = (pos == PagerPosition.Top) ? gridView.TopPagerRow : gridView.BottomPagerRow;
                    string className = GetRowClass(gridView, pagerRow);
                    //className += " " + (pos == PagerPosition.Top ? "Top " : "Bottom ");

                    // get pager section type
                    writer.WriteLine();
                    writer.WriteBeginTag(tableHeaderFooter); // change div to tfoot
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    // add opening row
                    writer.WriteBeginTag("tr");

                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    // add opening cell
                    writer.WriteBeginTag(tableCell);
                    // add colspan to cell
                    writer.WriteAttribute("colspan", gridView.HeaderRow.Cells.Count.ToString());
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    //check for PagerTemplate
                    if (gridView.PagerTemplate != null)
                    {
                        if (pagerRow != null)
                        {
                            foreach (TableCell cell in pagerRow.Cells)
                            {
                                foreach (Control ctrl in cell.Controls)
                                {
                                    ctrl.RenderControl(writer);
                                }
                            }
                        }
                    }
                    else //if not a PagerTemplate 
                    {
                        Table innerTable = null;

                        if ((pos == PagerPosition.Top) &&
                            (gridView.TopPagerRow != null) &&
                            (gridView.TopPagerRow.Cells.Count == 1) &&
                            (gridView.TopPagerRow.Cells[0].Controls.Count == 1) &&
                            typeof(Table).IsAssignableFrom(gridView.TopPagerRow.Cells[0].Controls[0].GetType()))
                        {
                            innerTable = gridView.TopPagerRow.Cells[0].Controls[0] as Table;
                        }
                        else if ((pos == PagerPosition.Bottom) &&
                            (gridView.BottomPagerRow != null) &&
                            (gridView.BottomPagerRow.Cells.Count == 1) &&
                            (gridView.BottomPagerRow.Cells[0].Controls.Count == 1) &&
                            typeof(Table).IsAssignableFrom(gridView.BottomPagerRow.Cells[0].Controls[0].GetType()))
                        {
                            innerTable = gridView.BottomPagerRow.Cells[0].Controls[0] as Table;
                        }

                        if ((innerTable != null) && (innerTable.Rows.Count == 1))
                        {
                            writer.WriteLine();
                            writer.WriteBeginTag("div");
                            writer.WriteAttribute("class", className);
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;
                            //RIGHT - PAGE NUMBERS
                            writer.WriteLine();
                            writer.WriteBeginTag("ul");
                            writer.WriteAttribute("class", "pagination pagination-sm pull-right");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            TableRow row = innerTable.Rows[0];
                            foreach (TableCell cell in row.Cells)
                            {
                                foreach (Control ctrl in cell.Controls)
                                {
                                    writer.WriteLine();
                                    writer.WriteBeginTag("li");

                                    if (ctrl is Label)
                                        writer.WriteAttribute("class", "active");

                                    writer.Write(HtmlTextWriter.TagRightChar);
                                    writer.Indent++;

                                    ctrl.RenderControl(writer);
                                    writer.Indent--;
                                    writer.WriteLine();
                                    writer.WriteEndTag("li");
                                }
                            }

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("ul");

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("div");
                        }
                    }

                    // add closing cell
                    writer.Indent--;
                    writer.WriteEndTag(tableCell);

                    // add closing row
                    writer.Indent--;
                    writer.WriteEndTag("tr");

                    writer.Indent--;
                    writer.WriteEndTag(tableHeaderFooter);
                }
            }
        }

        private static int NumberedPagerButtons(GridviewExtension gridView, HtmlGenericControl cell)
        {
            var start = gridView.PageIndex;
            var pageButtonCount = gridView.PagerSettings.PageButtonCount;
            var last = 0;
            for (int i = start; i < start + pageButtonCount; i++)
            {
                //<li><a href="#">1</a></li>
                var li = new HtmlGenericControl("li");
                var linkButton = new LinkButton();
                linkButton.CommandName = "Page";
                linkButton.CommandArgument = i.ToString();
                linkButton.Text = i.ToString();
                li.Controls.Add(linkButton);
                cell.Controls.Add(li);
                last = i;
            }

            return last++;
        }
    }
}
