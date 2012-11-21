using System.Collections.Generic;
using ReleaseManager.Models;

namespace ReleaseManager.Services
{
    public interface IEnvironmentService
    {
        IEnumerable<Environment> GetEnvironments();
    }
}