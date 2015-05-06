using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SweetSoft.APEM.Core.UI
{
    public class CheckBoxExtension : CheckBox
    {
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("onclick", "MarkAsChanged();");
            base.AddAttributesToRender(writer);
        }
    }
}
