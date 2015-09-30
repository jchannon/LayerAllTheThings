using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using FluentValidation;

namespace QueryHandler
{

    public class PersonModule : NancyModule
    {
        
        public PersonModule(IMediate mediator)
        {
            Get["/"] = _ => "Hi Earth People!";

            Get["/{id:int}"] = parameters =>
            {
                var userQuery = new UserQuery((int)parameters.id);
                var person = mediator.Request(userQuery);
                return person;
            };

            Post["/"] = parameters =>
            {
                var person = this.Bind<Person>();
                var personCmd = new PersonCommand(person);
                try
                {
                    var id = mediator.Send(personCmd);
                    return Negotiate.WithStatusCode(HttpStatusCode.Created).WithHeader("Location", Context.Request.Url.ToString() + "/" + id);
                }
                catch (ValidationException ex)
                {
                    return Negotiate.WithModel(ex.Errors.Select(x => new{x.PropertyName, x.ErrorMessage})).WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
            };

        }
    }
    
}
