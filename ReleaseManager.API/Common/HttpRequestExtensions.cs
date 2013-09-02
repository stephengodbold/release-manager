using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using ReleaseManager.API.App_Start;

namespace ReleaseManager.API.Common
{
    public static class HttpRequestExtensions 
    {
        public const string ApiModeKey = "x-api-mode";

        public static ApiMode GetApiMode(this HttpRequestMessage request)
        {
            return ExtractApiModeHeader(request.Headers);
        }

        public static void SetApiMode(this HttpRequestMessage request)
        {
            var apiMode = ExtractApiModeHeader(request.Headers);
            request.Properties.Add(ApiModeKey, apiMode);
        }

        private static ApiMode ExtractApiModeHeader(HttpHeaders headers)
        {
            if (headers == null) throw new ArgumentNullException("headers");
            var apiMode = ApiMode.Demo;

            if (!headers.Contains(ApiModeKey)) return apiMode;

            var headerValue = headers.First(h => h.Key.Equals(ApiModeKey)).Value.First();
            if (!headerValue.Equals("demo", StringComparison.InvariantCultureIgnoreCase))
            {
                apiMode = ApiMode.Production;
            }

            return apiMode;
        }
    }
}