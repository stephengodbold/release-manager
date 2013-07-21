using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using ReleaseManager.API.Queries;

namespace ReleaseManager.API.Controllers
{
    public class EnvironmentController : ApiController
    {
        private readonly IEnvironmentSettingsQuery query;

        public EnvironmentController(IEnvironmentSettingsQuery query)
        {
            this.query = query;
        }

        public JsonResult List()
        {
            var environments = query.Execute();

            return new JsonResult
                {
                    Data = JsonConvert.SerializeObject(environments, Formatting.Indented)
                };
        }
    }
}
