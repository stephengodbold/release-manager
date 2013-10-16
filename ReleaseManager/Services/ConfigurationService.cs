using System;
using System.Collections.Generic;

namespace ReleaseManager.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetRootUri()
        {
            return @"http://localhost:40666/api/";
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