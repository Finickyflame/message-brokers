using System;
using System.Collections.Generic;


namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of <see cref="IOptionsFactory{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type being configured.</typeparam>
    public class CreateOptions<TOptions> : OptionsFactory<TOptions> where TOptions : class
    {
        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        /// <param name="factory"></param>
        public CreateOptions(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, Func<TOptions> factory)
            : base(setups, postConfigures)
        {
            this.Factory = factory;
        }

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        /// <param name="validations">The validations to run.</param>
        /// <param name="factory"></param>
        public CreateOptions(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, IEnumerable<IValidateOptions<TOptions>> validations, Func<TOptions> factory)
            : base(setups, postConfigures, validations)
        {
            this.Factory = factory;
        }

        /// <summary>
        /// The factory action.
        /// </summary>
        public Func<TOptions> Factory { get; }


        /// <inheritdoc cref="IOptionsFactory{TOptions}.Create"/>
        protected override TOptions CreateInstance(string name) => this.Factory();
    }

    /// <summary>
    /// Implementation of <see cref="IOptionsFactory{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type being configured.</typeparam>
    /// <typeparam name="TDep">Dependency type.</typeparam>
    public class CreateOptions<TOptions, TDep> : OptionsFactory<TOptions> where TOptions : class
    {
        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="factory"></param>
        public CreateOptions(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, TDep dependency, Func<TDep, TOptions> factory)
            : base(setups, postConfigures)
        {
            this.Dependency = dependency;
            this.Factory = factory;
        }

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        /// <param name="validations">The validations to run.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="factory"></param>
        public CreateOptions(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, IEnumerable<IValidateOptions<TOptions>> validations, TDep dependency, Func<TDep, TOptions> factory)
            : base(setups, postConfigures, validations)
        {
            this.Dependency = dependency;
            this.Factory = factory;
        }

        /// <summary>
        /// The dependency.
        /// </summary>
        public TDep Dependency { get; }

        /// <summary>
        /// The factory action.
        /// </summary>
        public Func<TDep, TOptions> Factory { get; }


        /// <inheritdoc cref="IOptionsFactory{TOptions}.Create"/>
        protected override TOptions CreateInstance(string name) => this.Factory(this.Dependency);
    }
    
    /// <summary>
    /// Implementation of <see cref="IOptionsFactory{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type being configured.</typeparam>
    /// <typeparam name="TDep1">Dependency type.</typeparam>
    /// <typeparam name="TDep2">Dependency type.</typeparam>
    public class CreateOptions<TOptions, TDep1, TDep2> : OptionsFactory<TOptions> where TOptions : class
    {
        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        /// <param name="dependency1">A dependency.</param>
        /// <param name="dependency2">A dependency.</param>
        /// <param name="factory"></param>
        public CreateOptions(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, TDep1 dependency1,TDep2 dependency2, Func<TDep1,TDep2, TOptions> factory)
            : base(setups, postConfigures)
        {
            this.Dependency1 = dependency1;
            this.Dependency2 = dependency2;
            this.Factory = factory;
        }

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        /// <param name="validations">The validations to run.</param>
        /// <param name="dependency1">A dependency.</param>
        /// <param name="dependency2">A dependency.</param>
        /// <param name="factory"></param>
        public CreateOptions(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, IEnumerable<IValidateOptions<TOptions>> validations, TDep1 dependency1,TDep2 dependency2, Func<TDep1,TDep2, TOptions> factory)
            : base(setups, postConfigures, validations)
        {
            this.Dependency1 = dependency1;
            this.Dependency2 = dependency2;
            this.Factory = factory;
        }


        /// <summary>
        /// The dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }
        
        /// <summary>
        /// The dependency.
        /// </summary>
        public TDep2 Dependency2 { get; set; }

        /// <summary>
        /// The factory action.
        /// </summary>
        public Func<TDep1,TDep2, TOptions> Factory { get; }


        /// <inheritdoc cref="IOptionsFactory{TOptions}.Create"/>
        protected override TOptions CreateInstance(string name) => this.Factory(this.Dependency1, this.Dependency2);
    }
}