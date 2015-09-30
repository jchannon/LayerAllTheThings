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

    public class UserQuery : IQuery<Person>
    {
        public int UserId { get; private set; }

        public UserQuery(int userId)
        {
            UserId = userId;
        }
    }
    
}
