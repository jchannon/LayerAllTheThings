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

            Get["/{id:int}"] = parameters =>
            {
                var userQuery = new UserQuery((int)parameters.id);
                try
                {
                    var person = mediator.Request(userQuery);
                    return person;
                }
                catch (InvalidOperationException)
                {
                    return HttpStatusCode.NotFound;
                }
            };

            Put["/{id:int}"] = _ =>
            {
                var user = this.Bind<User>();
                var updateUserCmd = new UpdateUserCommand(user);
                try
                {
                    mediator.Send(updateUserCmd);
                    return Negotiate.WithStatusCode(HttpStatusCode.NoContent);
                }
                catch (ValidationException ex)
                {
                    return Negotiate.WithModel(ex.Errors.Select(x => new{x.PropertyName, x.ErrorMessage})).WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
                catch (InvalidOperationException)
                {
                    return HttpStatusCode.NotFound;
                }
            };

            Post["/"] = _ =>
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

            Delete["/{id:int}"] = parameters =>
            {
                var deleteUserCommand = new DeleteUserCommand((int)parameters.id);
                try
                {
                    mediator.Send(deleteUserCommand);
                }
                catch (InvalidOperationException)
                {
                    return HttpStatusCode.NotFound;
                }
                return HttpStatusCode.NoContent;
            };
        }
    }
}
