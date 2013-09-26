using System;
using System.Collections.Generic;

namespace ReleaseManager.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetRootUri()
        {
            return @"http://localhost:8181/api/";
        }

        public string GetMode()
        {
            return "Demo";
        }
    }

    public interface IConfigurationService
    {
        string GetRootUri();
        string GetMode();
    }

}