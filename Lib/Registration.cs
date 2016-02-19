using System;
using System.Linq.Expressions;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace Lib
{
    public static class Extensions
    {
        public static void RegisterFactory<TService, TImpl>(
            this Container container, Lifestyle lifestyle = null)
            where TService : class
            where TImpl : class, TService
        {
            lifestyle = lifestyle ?? Lifestyle.Transient;
            var producer = lifestyle.CreateProducer<TService, TImpl>(container);
            container.RegisterSingleton<Func<TService>>(() => producer.GetInstance);
        }
        public static void RegisterFunc<TInput, TService>(this Container container) where TService : class
        {
            container.Register(() => new ScopeFactory<TInput, TService>(container));
        }

        public static void RegisterParameterConvention(this ContainerOptions options, IParameterConvention convention)
        {
            options.DependencyInjectionBehavior = new ConventionDependencyInjectionBehavior(
                options.DependencyInjectionBehavior, convention);
        }

    }

    public interface IParameterConvention
    {
        bool CanResolve(InjectionTargetInfo target);
        bool IsResolvable(InjectionTargetInfo target);
        Expression BuildExpression(InjectionConsumerInfo consumer);
        Expression DefaulExpression();
    }

    public class UrlConvention : IParameterConvention
    {
        //[DebuggerStepThrough]
        public bool CanResolve(InjectionTargetInfo target)
        {
            var resolvable =
                target.TargetType.Is<IWebString>();
            if (!resolvable)
                return false;
            //target.Name == "url" &&
            //target.Property != null &&
            //target.Property.DeclaringType.IsDefined<UrlAttribute>();

            return resolvable;
        }

        public bool IsResolvable(InjectionTargetInfo target)
        {
            return target.TargetType == typeof(string);
        }

        //[DebuggerStepThrough]
        public Expression BuildExpression(InjectionConsumerInfo consumer)
        {
            return Expression.Constant(consumer.Target.Property.DeclaringType.GetCustomAttribute<UrlAttribute>().Url, typeof(string));
        }

        public Expression DefaulExpression()
        {
            return Expression.Constant("", typeof(string));
        }
    }

    internal class ConventionDependencyInjectionBehavior : IDependencyInjectionBehavior
    {
        private readonly IDependencyInjectionBehavior decoratee;
        private readonly IParameterConvention convention;

        public ConventionDependencyInjectionBehavior(
            IDependencyInjectionBehavior decoratee, IParameterConvention convention)
        {
            this.decoratee = decoratee;
            this.convention = convention;
        }

        //[DebuggerStepThrough]
        public Expression BuildExpression(InjectionConsumerInfo consumer)
        {
            return this.convention.CanResolve(consumer.Target)
                ? this.convention.BuildExpression(consumer)
                : convention.IsResolvable(consumer.Target) ? convention.DefaulExpression() : this.decoratee.BuildExpression(consumer);
        }

        //[DebuggerStepThrough]
        public void Verify(InjectionConsumerInfo consumer)
        {
        }
    }
}