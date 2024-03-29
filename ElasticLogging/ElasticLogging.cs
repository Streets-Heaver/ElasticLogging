﻿using ElasticLogging.Classes;
using ElasticLogging.Enums;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticLogging
{
    public sealed class ElasticLogging : IElasticLogging
    {
        private readonly ElasticClient _elasticClient;
 
        private readonly List<LogMessage> _pendingLogs;
        private readonly LoggingSettings _settings;

        public ElasticLogging(ConnectionSettings connectionSettings, LoggingSettings loggingSettings)
        {
            _elasticClient = new ElasticClient(connectionSettings);
            _settings = loggingSettings;
            _pendingLogs = new List<LogMessage>();
        }

        private string GetTempPath()
        {
            string path = Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        private void LogMessageToFile(string msg)
        {
            if (_settings.LogToFile)
            {
                var path = GetTempPath();
                StreamWriter sw = File.AppendText(
                     path + _settings.Name + ".txt");
                try
                {
                    string logLine = string.Format(
                        "{0:G}: {1}.", System.DateTime.Now, msg);
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        public void Dispose()
        {
            Flush();
        }

        public async ValueTask DisposeAsync()
        {
            await FlushAsync();
        }

        public void Flush()
        {
            if (_pendingLogs.Any())
            {
                _elasticClient.IndexMany(_pendingLogs);
                _pendingLogs.Clear();
            }
        }

        public async Task FlushAsync()
        {
            if (_pendingLogs.Any())
            {
                await _elasticClient.IndexManyAsync(_pendingLogs);
                _pendingLogs.Clear();
            }
        }

        public void Fatal(Exception ex)
        {
            Fatal(ex.ToString());
        }

        public void Fatal(string errorMessage)
        {
            if (_settings.IsFatalEnabled)
            {
                _pendingLogs.Add(BuildDocument(errorMessage, Level.Fatal));
                LogMessageToFile(errorMessage);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    Flush();
            }
        }

        public async Task FatalAsync(Exception ex)
        {
            await FatalAsync(ex.ToString());
        }

        public async Task FatalAsync(string errorMessage)
        {
            if (_settings.IsFatalEnabled)
            {
                _pendingLogs.Add(BuildDocument(errorMessage, Level.Fatal));
                LogMessageToFile(errorMessage);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    await FlushAsync();
            }
        }

        public void Error(Exception ex)
        {
            Error(ex.ToString());

        }

        public void Error(string errorMessage)
        {

            if (_settings.IsErrorEnabled)
            {
                _pendingLogs.Add(BuildDocument(errorMessage, Level.Error));
                LogMessageToFile(errorMessage);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    Flush();
            }
        }

        public async Task ErrorAsync(Exception ex)
        {
            await ErrorAsync(ex.ToString());

        }

        public async Task ErrorAsync(string errorMessage)
        {
            if (_settings.IsErrorEnabled)
            {
                _pendingLogs.Add(BuildDocument(errorMessage, Level.Error));
                LogMessageToFile(errorMessage);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    await FlushAsync();
            };
        }

        public void Warn(string message)
        {
            if (_settings.IsWarnEnabled)
            {
                _pendingLogs.Add(BuildDocument(message, Level.Error));
                LogMessageToFile(message);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    Flush();
            }
        }



        public async Task WarnAsync(string message)
        {
            if (_settings.IsWarnEnabled)
            {
                _pendingLogs.Add(BuildDocument(message, Level.Warn));
                LogMessageToFile(message);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    await FlushAsync();
            };

        }

        public void Info(string message)
        {
            if (_settings.IsInfoEnabled)
            {
                _pendingLogs.Add(BuildDocument(message, Level.Info));
                LogMessageToFile(message);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    Flush();
            }

        }

        public async Task InfoAsync(string message)
        {
            if (_settings.IsWarnEnabled)
            {
                _pendingLogs.Add(BuildDocument(message, Level.Info));
                LogMessageToFile(message);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    await FlushAsync();
            };

        }

        public void Debug(string message)
        {
            if (_settings.IsDebugEnabled)
            {
                _pendingLogs.Add(BuildDocument(message, Level.Debug));
                LogMessageToFile(message);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    Flush();
            }

        }

        public async Task DebugAsync(string message)
        {
            if (_settings.IsWarnEnabled)
            {
                _pendingLogs.Add(BuildDocument(message, Level.Debug));
                LogMessageToFile(message);

                if (_pendingLogs.Count() == _settings.BatchSize)
                    await FlushAsync();
            };

        }



        private LogMessage BuildDocument(string message, Level level)
        {
            LogMessage log = new LogMessage();

            log.LoggerName = _settings.Name;
            log.Message = message;
            log.HostName = Environment.MachineName;
            log.UserName = Environment.UserName;
            log.TimeStamp = DateTime.Now;
            log.Level = level.ToString();

            return log;
        }

    }
}
