using System;

namespace QueryHandler
{
    public interface ICommandHandler<in TCommand, out TResponse>
        where TCommand : ICommand<TResponse>
    {
        TResponse Handle(TCommand command);

        bool CanHandle(Type commandType);
    }
}
