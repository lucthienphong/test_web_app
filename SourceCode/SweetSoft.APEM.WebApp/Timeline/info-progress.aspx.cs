using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace SweetSoft.APEM.WebApp.Timeline
{
    public partial class info_progress : System.Web.UI.Page
    {
        static string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Progress3-2.xml");
        XmlDocument mainDoc = new XmlDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            string data = Request.Form["newsId"];

            //for test
            if (string.IsNullOrEmpty(data))
                data = Request.QueryString["newsId"];

            if (!string.IsNullOrEmpty(data))
            {
                if (mainDoc.DocumentElement == null)
                    mainDoc.Load(filePath);

                if (mainDoc.DocumentElement != null)
                {
                    //for test
                    //System.Threading.Thread.Sleep(2500);

                    XmlNode commentNode = mainDoc.DocumentElement.SelectSingleNode("//Progress/*[@Id='" + data + "']");
                    if (commentNode != null)
                    {
                        StringBuilder sbResponse = new StringBuilder();
                        /*
                        int imageThumb = 0;
                        int.TryParse(data, out imageThumb);
                        imageThumb = imageThumb % 13;//there are 13 picture
                        if (imageThumb == 0)
                            imageThumb = 13;
                        sbResponse.AppendFormat("<img class='con_borderImage' src='{0}' alt='{1}' />",
                        string.Format("photo/medium/thumb{0}.jpg", imageThumb), "Comment " + data);
                        */
                        sbResponse.AppendFormat("<div class='timeline_open_content'>{0}</div>", commentNode.InnerText);
                        Response.Write(sbResponse);
                        return;
                    }
                }
            }

            Response.Write(string.Empty);
        }
    }
}