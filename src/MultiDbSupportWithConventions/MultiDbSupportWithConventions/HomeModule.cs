namespace MultiDbSupportWithConventions
{
    using MediatR;

    using MultiDbSupportWithConventions.Features.Users;
    using MultiDbSupportWithConventions.Features.Users.GetUsers;

    using Nancy;
    using Nancy.ModelBinding;

    public class HomeModule : NancyModule
    {
        public HomeModule(IMediator mediator)
        {
            Get["/"] = _ =>
            {
                var query = new UserListQuery(-1);

                return mediator.Send(query);
            };

            Post["/"] = _ =>
            {
                var incomingModel = this.Bind<UserInputModel>();//This could be moved to an extension or decorator to the command

                // This could also be added to an extension or decorator to keep modules anaemic and have cross cutting converns like validation orthogonal
                if (!this.ModelValidationResult.IsValid)
                {
                    return 422;
                }

                var id = mediator.Send(incomingModel);

                return Negotiate.WithStatusCode(201).WithHeader("Location", "http://example.com/" + id);
            };
        }
    }
}
