using System;
using System.Text;
using System.Web.Mvc;

namespace ReleaseManager.Common
{
    public class CsvResult : ActionResult 
    {
        public string Content { get; set; }
        public string Filename { get; set; }
        public Encoding ContentEncoding { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context ==null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = "text/csv";
            response.Headers.Add("Content-Disposition",
                                 String.Format("attachment; filename={0}", Filename));

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            response.Write(Content);
        }
    }
}