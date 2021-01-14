using ElasticLogging.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticLogging.Classes
{
    public class LogMessage
    {
        public string Message { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public Level Level { get; set; }
    }
}
