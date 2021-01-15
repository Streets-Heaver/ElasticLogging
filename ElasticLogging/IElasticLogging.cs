using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticLogging
{
    public interface IElasticLogging
    {
        void Debug(string message);
        Task DebugAsync(string message);
        void Error(Exception ex);
        void Error(string errorMessage);
        Task ErrorAsync(Exception ex);
        Task ErrorAsync(string errorMessage);
        void Fatal(Exception ex);
        void Fatal(string errorMessage);
        Task FatalAsync(Exception ex);
        Task FatalAsync(string errorMessage);
    
        void Info(string message);
        Task InfoAsync(string message);
        void Warn(string message);
        Task WarnAsync(string message);
    }
}
