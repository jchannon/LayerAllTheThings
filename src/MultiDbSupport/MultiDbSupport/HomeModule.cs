namespace MultiDbSupport
{
    using MediatR;

    using Nancy;

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
                //We would do the same above using mediatr to call a command handler
                return 201;
            };
        }
    }
}
