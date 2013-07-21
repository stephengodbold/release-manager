using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ReleaseManager.Common
{
    public class CsvResult : ActionResult
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public Encoding ContentEncoding { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = "text/csv";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            response.AddHeader("Content-Disposition",
                               String.Format("attachment; filename={0}", Name));

            if (Content != null)
            {
                response.Write(Content);
            }
        }
    }
}