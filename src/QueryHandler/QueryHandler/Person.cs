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

    public class Person
    {
        public string FirstName{ get; set; }

        public string LastName{ get; set; }

        public string FullName{ get { return FirstName + " " + LastName; } }

        public string EmailAddress { get; set; }
    }
    
}
