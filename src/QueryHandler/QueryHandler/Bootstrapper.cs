using Nancy;

namespace QueryHandler
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            var mediator = new Mediator();
            
            mediator.Register<IHandleQueries<IQuery<Person>, Person>>(delegate
                {
                    return new UserQueryHandler();
                });

            mediator.Register<ICommandHandler<ICommand<int>, int>>(delegate
                {
                    return new UserCommandHandler();
                }
            );

            container.Register<IMediate,Mediator>(mediator);
        }
    }
}
