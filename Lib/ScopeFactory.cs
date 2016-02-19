using System;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace Lib
{
    public class Provider<T> : IProvider<T>
    {
        public T Value { get; set; }
    }

    public class ScopeFactory
    {
        public IDisposable BeginScope() => Container.BeginExecutionContextScope();
        protected readonly Container Container;

        public ScopeFactory(Container container)
        {
            Container = container;
        }
    }
    public class ScopeFactory<TInput, TService> : ScopeFactory, IScopeFactory<TInput, TService> where TService : class

    {
        public virtual TService Create(TInput input)
        {
            return Create<IProvider<TInput>>(input);
            /*using (Container.GetCurrentExecutionContextScope() == null ? BeginScope() : null)
            {
                var provider = Container.GetInstance<Provider<TInput>>();
                provider.Value = input;

                return Container.GetInstance<TService>();
            }*/
        }

        protected TService Create<TProvider>(TInput input) where TProvider : class, IProvider<TInput>
        {
            var withinScope = Container.GetCurrentExecutionContextScope() != null;

            using (withinScope ? null : BeginScope())
            {
                var provider = Container.GetInstance<TProvider>();
                provider.Value = input;

                if (Container.GetRegistration(typeof(TService)).Lifestyle == Lifestyle.Scoped && !withinScope)
                    throw new ActivationException();

                return Container.GetInstance<TService>();
            }

        }

        public ScopeFactory(Container container)
            : base(container)
        {
        }
    }
}
