using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using FluentValidation;

namespace QueryHandler
{
    public class UserModule : NancyModule
    {
        
        public UserModule(IMediate mediator)
        {
            Get["/"] = _ => "Hi Earth People!";


            //404 if not found!!!!


            Get["/{id:int}"] = parameters =>
            {
                var userQuery = new UserQuery((int)parameters.id);
                try
                {
                    var person = mediator.Request(userQuery);
                    return person;
                }
                catch (InvalidOperationException ex)
                {
                    return HttpStatusCode.NotFound;
                }
            };

            Put["/{id:int}"] = parameters =>
            {
                var user = this.Bind<User>();
                var updateUserCmd = new UpdateUserCommand(user); 
                try
                {
                    var id = mediator.Send(updateUserCmd);
                    return Negotiate.WithStatusCode(HttpStatusCode.NoContent);
                }
                catch (ValidationException ex)
                {
                    return Negotiate.WithModel(ex.Errors.Select(x => new{x.PropertyName, x.ErrorMessage})).WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
                catch (InvalidOperationException ex)
                {
                    return HttpStatusCode.NotFound;
                }
            };

            Post["/"] = parameters =>
            {
                var user = this.Bind<User>();
                var insertUserCmd = new InsertUserCommand(user);
                try
                {
                    var id = mediator.Send(insertUserCmd);
                    return Negotiate.WithStatusCode(HttpStatusCode.Created).WithHeader("Location", Context.Request.Url + "/" + id);
                }
                catch (ValidationException ex)
                {
                    return Negotiate.WithModel(ex.Errors.Select(x => new{x.PropertyName, x.ErrorMessage})).WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
            };

        }
    }
}
