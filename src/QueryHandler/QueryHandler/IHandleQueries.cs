using System;

namespace QueryHandler
{
    public interface IHandleQueries<in TQuery, out TResponse>
    where TQuery : IQuery<TResponse>
    {
        TResponse Handle(TQuery query);

        bool CanHandle(Type commandType);
    }
}
