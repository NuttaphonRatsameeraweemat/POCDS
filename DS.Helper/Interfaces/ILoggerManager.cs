using System;
using Microsoft.AspNetCore.Http;

namespace DS.Helper.Interfaces
{
    public interface ILoggerManager
    {
        void CreateNewSession(HttpContext context);
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(Exception ex, string message = null);
    }
}
