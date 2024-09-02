using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningDotnet.Jobs
{
    public class CustomJobLogger
    {
        private readonly ILogger _logger;
        public CustomJobLogger(ILogger<CustomJobLogger> logger) => _logger = logger;

        public void WriteLog(string logMessage) {
            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd hh:mm:ss tt} {logMessage}");
        }
    }
}