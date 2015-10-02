using Nancy;

namespace QueryHandler
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            var mediator = new Mediator();
            
//            mediator.Register<IHandleQueries<IQuery<User>, User>>(delegate
//                {
//                    return new UserQueryHandler();
//                });
//
//            mediator.Register<ICommandHandler<ICommand<int>, int>>(delegate
//                {
//                    return new InsertUserCommandHandler();
//                }
//            );

            mediator.Register<IQueryHandler<IQuery<User>,User>, UserQueryHandler>();

            mediator.Register<ICommandHandler<ICommand<int>,int>,UpdateUserCommandHandler>();
            mediator.Register<ICommandHandler<ICommand<int>,int>,InsertUserCommandHandler>();

//            mediator.Register<ICommandHandler<ICommand<int>, int>>(delegate
//                {
//                    return new UpdateUserCommandHandler();
//                }
//            );

            container.Register<IMediate,Mediator>(mediator);
        }
    }
}
