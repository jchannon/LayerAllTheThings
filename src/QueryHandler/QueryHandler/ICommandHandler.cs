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

    public interface ICommandHandler<in TCommand, out TResponse>
        where TCommand : ICommand<TResponse>
    {
        TResponse Handle(TCommand command);
    }
    
}
