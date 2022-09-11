using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project.Web.Mvc4.Models
{
    public class DownloadFileActionResult : ActionResult
    {

        public GridView ExcelGridView { get; set; }
        public string fileName { get; set; }


        public DownloadFileActionResult(GridView gv, string pFileName)
        {
            ExcelGridView = gv;
            fileName = pFileName;
        }


        public override void ExecuteResult(ControllerContext context)
        {

            ////Create a response stream to create and write the Excel file
            //HttpContext curContext = HttpContext.Current;
            //curContext.Response.Clear();
            //curContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            //curContext.Response.Charset = "";
            //curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //curContext.Response.ContentType = "application/vnd.ms-excel";

            ////Convert the rendering of the gridview to a string representation 
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //ExcelGridView.RenderControl(htw);

            ////Open a memory stream that you can use to write back to the response
            //byte[] byteArray = Encoding.UTF8.GetBytes(sw.ToString());
            //MemoryStream s = new MemoryStream(byteArray);
            //Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            //Encoding utf8 = Encoding.UTF8;
            //StreamReader sr = new StreamReader(s, Encoding.UTF8);


            ////Write the stream back to the response
            //curContext.Response.Write(sr.ReadToEnd());
            //curContext.Response.End();

            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", "attachment;filename="+fileName);
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.Unicode;
            curContext.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);

            ExcelGridView.RenderControl(hw);

            curContext.Response.Write(sw.ToString());
            curContext.Response.End();

        }

    }
}
