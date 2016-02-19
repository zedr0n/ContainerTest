using System;

namespace Lib
{
    public interface IProvider
    { }

    public interface IProvider<T> : IProvider
    {
        T Value { get; set; }
    }

    public interface IScopeFactory<in TInput, out TService> where TService : class
    {
        IDisposable BeginScope();
        TService Create(TInput value);
    }

    public interface IScopeFactory<in TInput, in TProvider, out TService> : IScopeFactory<TInput, TService> where TService : class
        where TProvider : class, IProvider<TInput>
    {

    }
}
