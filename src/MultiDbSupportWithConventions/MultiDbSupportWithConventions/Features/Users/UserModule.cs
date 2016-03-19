using MultiDbSupportWithConventions.Features.Users.UpdateUser;
using Nancy.Routing;

namespace MultiDbSupportWithConventions.Features.Users
{
    using System;
    using System.Linq;

    using FluentValidation;

    using MediatR;

    using MultiDbSupportWithConventions.Features.Users.GetUser;
    using MultiDbSupportWithConventions.Features.Users.GetUsers;

    using Nancy;
    using Nancy.ModelBinding;

    public class UserModule : NancyModule
    {
        public UserModule(IMediator mediator)
        {
            this.Get["/"] = _ =>
            {
                var query = new UserListQuery(-1);

                return mediator.Send(query);
            };

            this.Get["/{id:int}"] = parameters =>
            {
                var query = new GetUserQuery((int)parameters.id);
                try
                {
                    return mediator.Send(query);
                }
                catch (InvalidOperationException)
                {
                    return 404;
                }
            };

            this.Post["/"] = _ =>
            {
                var incomingModel = this.Bind<AddUserCommand>();

                try
                {
                    var id = mediator.Send(incomingModel);

                    return this.Negotiate.WithStatusCode(201).WithHeader("Location", "http://example.com/" + id);
                }
                catch (ValidationException ex)
                {
                    return
                        Negotiate.WithModel(ex.Errors.Select(x => new {x.PropertyName, x.ErrorMessage}))
                            .WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
            };

            this.Put["/{id:int}"] = _ =>
            {
                var updateCmd = this.Bind<UpdateUserCommand>();
                try
                {
                    var result = mediator.Send(updateCmd);

                    return 204;
                }
                catch (InvalidOperationException ex)
                {
                    return 404;
                }
            };

            this.Delete["/{id:int}"] = parameters =>
            {
                int id = parameters.id;
                var deleteUserCommand = new DeleteUserCommand(id);
                try
                {
                    var result = mediator.Send(deleteUserCommand);
                    return 204;
                }
                catch (InvalidOperationException)
                {
                    return 404;
                }
            };
        }
    }
}