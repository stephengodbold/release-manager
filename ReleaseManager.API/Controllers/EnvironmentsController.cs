using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using ReleaseManager.API.Queries;

namespace ReleaseManager.API.Controllers
{
    public class EnvironmentsController : ApiController, IEnvironmentController
    {
        private readonly IEnvironmentSettingsQuery query;

        public EnvironmentsController(IEnvironmentSettingsQuery query)
        {
            this.query = query;
        }

        public JsonResult Get()
        {
            var environments = query.Execute();

            return new JsonResult
                {
                    Data = JsonConvert.SerializeObject(environments, Formatting.Indented)
                };
        }
    }

    public interface IEnvironmentController
    {
        JsonResult Get();
    }
}
