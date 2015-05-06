
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SweetSoft.APEM.Core.Manager;

namespace SweetSoft.APEM.WebApp.Timeline
{
    public class AddTemplateToGridView : ITemplate
    {
        ListItemType _type;
        string _colName;
        bool _useId;
        bool _useLink;

        public AddTemplateToGridView(ListItemType type, string colname)
        {
            _type = type;
            _useId = true;
            _useLink = true;
            _colName = colname;
        }

        public AddTemplateToGridView(ListItemType type, string colname, bool useId, bool useLink)
        {
            _type = type;
            _useId = useId;
            _useLink = useLink;
            _colName = colname;
        }

        public AddTemplateToGridView(ListItemType type, string colname, bool useId)
        {
            _type = type;
            _useId = useId;
            _useLink = true;
            _colName = colname;
        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_type)
            {
                case ListItemType.Item:
                    if (_colName == "Finish on")
                    {
                        Literal ltrDate = new Literal();
                        ltrDate.DataBinding += new EventHandler(literal_DataBinding);
                        container.Controls.Add(ltrDate);
                    }
                    else
                    {
                        Literal ltr = new Literal();
                        ltr.DataBinding += new EventHandler(literal_DataBinding);
                        container.Controls.Add(ltr);
                    }
                    if (_useId)
                    {
                        HtmlInputHidden hdfId = new HtmlInputHidden();
                        hdfId.DataBinding += new EventHandler(hdfId_DataBinding);
                        container.Controls.Add(hdfId);
                    }
                    break;
            }
        }

        void hdfId_DataBinding(object sender, EventArgs e)
        {
            HtmlInputHidden hdfId = (HtmlInputHidden)sender;
            GridViewRow container = (GridViewRow)hdfId.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, "Id");
            if (dataValue != DBNull.Value)
                hdfId.Value = dataValue.ToString();
        }

        void literal_DataBinding(object sender, EventArgs e)
        {
            Literal ltr = (Literal)sender;
            GridViewRow container = (GridViewRow)ltr.NamingContainer;
            if (_colName == "Finish on")
            {
                object dataValue = DataBinder.Eval(container.DataItem, _colName);
                if (dataValue != null && dataValue != DBNull.Value)
                {
                    ltr.Text = (DateTime)dataValue == DateTime.MinValue ? "" : ((DateTime)dataValue).ToString("MM/dd/yyyy hh:mm:ss tt");
                    return;
                }
            }
            else
            {
                ltr.Text = DataBinder.Eval(container.DataItem, _colName).ToString();
            }
        }
    }
}