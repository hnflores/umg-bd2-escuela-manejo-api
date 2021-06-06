using API_ESC_MANEJO.CORE.CORE.Enumerations;
using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace API_ESC_MANEJO.CORE.Services
{
    public class LogService : ILogService
    {
        private readonly ConfigurationLog _configurationLog;
        public LogService(IOptions<ConfigurationLog> configurationLog)
        {
            _configurationLog = configurationLog.Value;
        }
        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public void SaveLogApp(string message, LogType logType)
        {
            string type;
            if (logType == LogType.Error)
                type = "Error";
            else
                type = "Information";
            string path = $"{Directory.GetCurrentDirectory()}/{_configurationLog.Path}{_configurationLog.Date}/{type}/";
            try
            {
                CreateDirectory(path);
                string line = $"[{DateTime.Now:dd/MM/yyyy} {DateTime.Now:HH:mm:ss fff}]";
                line = $"{line}: {message}{Environment.NewLine}";
                File.AppendAllText($"{path}{_configurationLog.NameFile}", line);
            }
            catch (Exception ex)
            {
                string source = $"EpayServerAPI.{nameof(CORE)}";
                Console.WriteLine(source, ex.Message, ex.StackTrace);
            }
        }
    }
}
