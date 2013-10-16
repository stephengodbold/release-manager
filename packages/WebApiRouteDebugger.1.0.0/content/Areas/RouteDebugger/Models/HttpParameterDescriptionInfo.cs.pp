<<<<<<< HEAD
using System;
using System.Web.Http.Controllers;

namespace $rootnamespace$.Areas.RouteDebugger.Models
{
    /// <summary>
    /// Represents the parameters.
    /// </summary>
    public class HttpParameterDescriptorInfo
    {
        public HttpParameterDescriptorInfo(HttpParameterDescriptor descriptor)
        {
            ParameterName = descriptor.ParameterName;
            ParameterType = descriptor.ParameterType;
            ParameterTypeName = descriptor.ParameterType.Name;
        }

        public string ParameterName { get; set; }

        public Type ParameterType { get; set; }

        public string ParameterTypeName { get; set; }
    }
}
=======
using System;
using System.Web.Http.Controllers;

namespace $rootnamespace$.Areas.RouteDebugger.Models
{
    /// <summary>
    /// Represents the parameters.
    /// </summary>
    public class HttpParameterDescriptorInfo
    {
        public HttpParameterDescriptorInfo(HttpParameterDescriptor descriptor)
        {
            ParameterName = descriptor.ParameterName;
            ParameterType = descriptor.ParameterType;
            ParameterTypeName = descriptor.ParameterType.Name;
        }

        public string ParameterName { get; set; }

        public Type ParameterType { get; set; }

        public string ParameterTypeName { get; set; }
    }
}
>>>>>>> c95527e5bdcad2ac94de132c1ef1acdfce80fa27
