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

    public class PersonCommand : ICommand<int>
    {
        public Person Person { get; private set; }

        public PersonCommand(Person person)
        {
            this.Person = person;
        }
    }
    
}
