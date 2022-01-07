using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticLogging.Classes
{
    public class LoggingSettings
    {
        public int BatchSize { get; set; } = 100;
        public bool IsFatalEnabled { get; set; } = true;
        public bool IsErrorEnabled { get; set; } = true;
        public bool IsInfoEnabled { get; set; } = true;
        public bool IsWarnEnabled { get; set; } = true;
        public bool IsDebugEnabled { get; set; } = true;
        public string Name { get; set; }
        public bool LogToFile { get; set; }
    }
}
