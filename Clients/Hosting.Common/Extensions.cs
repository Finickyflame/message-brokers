using Hosting.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    public static class Extensions
    {
        /// <summary>
        /// Runs a web application, blocking the calling thread until the host shuts down.<para />
        /// To execute your task, invoke the application with argument "runtask=taskname", where "taskname" is your task's name.<para/>
        /// Command line arguments should be registered as a configuration source for this functionality to work.
        /// </summary>
        /// <param name="host">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> to run.</param>
        public static async Task RunWithTasksAsync(this IHost host)
        {
            if (await FindAndRunTask(host.Services))
            {
                host.Dispose();
            }
            else
            {
                await host.RunAsync();
            }
        }

        public static IServiceCollection AddOptions<TOptions>(this IServiceCollection services, Action<OptionsBuilder<TOptions>> configure) where TOptions : class
        {
            configure(services.AddOptions<TOptions>());
            return services;
        }

        public static IServiceCollection AddApplicationTask<TApplicationTask>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TApplicationTask : IApplicationTask
        {
            services.Add(new ServiceDescriptor(typeof(IApplicationTask), typeof(TApplicationTask), serviceLifetime));
            return services.Decorate<IApplicationTask, ApplicationTaskLogger>();
        }

        private static async Task<bool> FindAndRunTask(IServiceProvider services)
        {
            var config = services.GetRequiredService<IConfiguration>();
            var taskName = config.GetValue<string>("runTask");
            if (taskName == null)
            {
                return false;
            }

            using IServiceScope scope = services.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            IApplicationTask? task = serviceProvider.GetServices<IApplicationTask>().FirstOrDefault(x => string.Equals(x.Name, taskName, StringComparison.CurrentCultureIgnoreCase));
            if (task != null)
            {
                await task.RunAsync();
            }
            else
            {
                ILogger? logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("CloudFoundryTasks");
                logger?.LogError("No task with name {TaskName} is found registered in service container", taskName);
            }

            return true;
        }
    }
}