using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hosting.Common
{
    public sealed class ApplicationTaskLogger : IApplicationTask
    {
        private readonly IApplicationTask _base;
        private readonly ILogger _logger;
        private readonly Type _baseType;

        public ApplicationTaskLogger(IApplicationTask @base, ILoggerFactory loggerFactory)
        {
            this._base = @base;
            this._baseType = @base.GetType();
            this._logger = loggerFactory.CreateLogger(this._baseType);
        }

        public string Name => this._base.Name;

        public async Task RunAsync()
        {
            string taskName = this._baseType.Name;
            this._logger.LogInformation("{Task} Started", taskName);
            var watch = Stopwatch.StartNew();
            
            await this._base.RunAsync();
            
            watch.Stop();
            this._logger.LogInformation("{Task} Completed ({Duration}ms)", taskName, watch.ElapsedMilliseconds);
        }
    }
}