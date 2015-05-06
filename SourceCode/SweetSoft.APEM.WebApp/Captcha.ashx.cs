using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSoft.QLNKT.WebApp
{
    using System;
    using System.Web;
    using System.Drawing;
    using System.IO;
    using System.Web.SessionState;
    /// <summary>
    /// Summary description for Capcha
    /// </summary>
    public class Captcha : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Bitmap bmpOut = new Bitmap(80, 34);

            Graphics g = Graphics.FromImage(bmpOut);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.FillRectangle(new SolidBrush(Color.White), 0, 0, 80, 34);

            g.DrawString(context.Session["Captcha"].ToString(), new Font("Verdana", 18), new SolidBrush(Color.Black), 5, 2);

            MemoryStream ms = new MemoryStream();

            bmpOut.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            byte[] bmpBytes = ms.GetBuffer();

            bmpOut.Dispose();

            ms.Close();

            context.Response.BinaryWrite(bmpBytes);

            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}