using ElasticLogging.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ElasticLogging.Classes
{
    public class LogMessage
    {
        public string Message { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Level { get; set; }
        public string LoggerName { get; set; }
    }
}
