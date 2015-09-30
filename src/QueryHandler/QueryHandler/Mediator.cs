using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Hosting;
using Nancy;
using Owin;
using Nancy.ModelBinding;
using Nancy.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace QueryHandler
{

    public class Mediator : IMediate
    {
        public delegate object Creator(Mediator container);

        private readonly Dictionary<Type, Creator> _typeToCreator = new Dictionary<Type, Creator>();

        public void Register<T>(Creator creator)
        {
            _typeToCreator.Add(typeof(T), creator);
        }

        private T Create<T>()
        {
            return (T)_typeToCreator[typeof(T)](this);
        }

        public TResponse Request<TResponse>(IQuery<TResponse> query)
        {
            var handler = Create<IHandleQueries<IQuery<TResponse>, TResponse>>();
            return handler.Handle(query);
        }

        public TResponse Send<TResponse>(ICommand<TResponse> cmd)
        {
            var handler = Create<ICommandHandler<ICommand<TResponse>, TResponse>>();
            return handler.Handle(cmd);
        }
    }
    
}
