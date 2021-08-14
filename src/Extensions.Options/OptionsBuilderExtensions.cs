using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Options
{
    public static class OptionsBuilderExtensions
    {
        public static OptionsBuilder<TOptions> Create<TOptions>(this OptionsBuilder<TOptions> builder, Func<TOptions> optionsFactory)
            where TOptions : class
        {
            if (optionsFactory == null)
            {
                throw new ArgumentNullException(nameof(optionsFactory));
            }

            builder.Services.AddSingleton<IOptionsFactory<TOptions>>(service => new CreateOptions<TOptions>(
                    service.GetServices<IConfigureOptions<TOptions>>(),
                    service.GetServices<IPostConfigureOptions<TOptions>>(),
                    service.GetServices<IValidateOptions<TOptions>>(),
                    optionsFactory
                )
            );
            return builder;
        }

        public static OptionsBuilder<TOptions> Create<TOptions, TDep>(this OptionsBuilder<TOptions> builder, Func<TDep, TOptions> optionsFactory)
            where TOptions : class
            where TDep : class
        {
            if (optionsFactory == null)
            {
                throw new ArgumentNullException(nameof(optionsFactory));
            }

            builder.Services.AddSingleton<IOptionsFactory<TOptions>>(service => new CreateOptions<TOptions, TDep>(
                    service.GetServices<IConfigureOptions<TOptions>>(),
                    service.GetServices<IPostConfigureOptions<TOptions>>(),
                    service.GetServices<IValidateOptions<TOptions>>(),
                    service.GetRequiredService<TDep>(),
                    optionsFactory
                )
            );
            return builder;
        }
        
        public static OptionsBuilder<TOptions> Create<TOptions, TDep1, TDep2>(this OptionsBuilder<TOptions> builder, Func<TDep1,TDep2, TOptions> optionsFactory)
            where TOptions : class
            where TDep1 : class
            where TDep2 : class
        {
            if (optionsFactory == null)
            {
                throw new ArgumentNullException(nameof(optionsFactory));
            }

            builder.Services.AddSingleton<IOptionsFactory<TOptions>>(service => new CreateOptions<TOptions, TDep1, TDep2>(
                    service.GetServices<IConfigureOptions<TOptions>>(),
                    service.GetServices<IPostConfigureOptions<TOptions>>(),
                    service.GetServices<IValidateOptions<TOptions>>(),
                    service.GetRequiredService<TDep1>(),
                    service.GetRequiredService<TDep2>(),
                    optionsFactory
                )
            );
            return builder;
        }
    }
}