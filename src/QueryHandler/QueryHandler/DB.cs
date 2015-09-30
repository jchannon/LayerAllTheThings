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

    public static class DB
    {
        //Our DB
        public static Dictionary<int, Person> Data = new Dictionary<int, Person>()
        { 
            { 1, new Person{ FirstName = "Jim", LastName = "Parsons", EmailAddress = "jim@parsons.com" } }, 
            { 2, new Person{ FirstName = "Fred", LastName = "Smith", EmailAddress = "fred@smith.com" } }, 
            { 3, new Person{ FirstName = "Bob", LastName = "Hope", EmailAddress = "bob@hope.com" } }, 
            { 4, new Person{ FirstName = "Bernard", LastName = "Targarian", EmailAddress = "bernard@targarian.com" } }, 
            { 5, new Person{ FirstName = "Troy", LastName = "Vega", EmailAddress = "troy@vega.com" } }
        };
    }
    
}
