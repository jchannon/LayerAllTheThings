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

    public class UserQueryHandler : IHandleQueries<IQuery<Person>, Person>
    {
        public Person Handle(IQuery<Person> query)
        {
            var userQuery = query as UserQuery;
            return DB.Data.FirstOrDefault(x => x.Key == userQuery.UserId).Value;
        }
    }
    
}
