using MultiDbSupportWithConventions.Features.Users.DataAccess;

namespace MultiDbSupportWithConventions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using MediatR;

    using MultiDbSupportWithConventions.Features.Users;
    using MultiDbSupportWithConventions.Features.Users.AddUser;
    using MultiDbSupportWithConventions.Features.Users.GetUser;
    using MultiDbSupportWithConventions.Features.Users.GetUsers;
    using MultiDbSupportWithConventions.Features.Users.UpdateUser;

    using Nancy;
    using Nancy.TinyIoc;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            StaticConfiguration.DisableErrorTraces = false;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            //base.ConfigureApplicationContainer(container); // Lets our app do all the wiring up

            //Instead of having stored procedures do the abstraction we can tell IOC to do it. 
            //That way we have commands/queries that can use db specific sql for example. 
            switch (ConfigurationManager.ConnectionStrings["mydb"].ProviderName.ToLower())
            {
                case "system.data.sqlclient":
                    container.Register<IDbConnectionProvider, SqlServerConnectionProvider>();
                    container.Register<IRequestHandler<UserListQuery, IEnumerable<User>>, MssqlUserListQueryRequestHandler>();
                    container.Register<IRequestHandler<AddUserCommand, int>, MsSqlAddUserCommandHandlerCommandHandlerHandler>();
                    container.Register<IRequestHandler<GetUserQuery, User>, AgnosticDbGetUserQueryHandler>();
                    container.Register<IRequestHandler<UpdateUserCommand,int>, UpdateUserCommandHandler>();
                    container.Register<IRequestHandler<DeleteUserCommand,int>, DeleteUserCommandHandler>();
                    container.Register<IGetUserByIdQuery,GetUserByIdQuery>();
                    break;
                case "npgsql":
                    container.Register<IDbConnectionProvider, PostgresConnectionProvider>();
                    container.Register<IRequestHandler<UserListQuery, IEnumerable<User>>, NpgsqlUserListQueryRequestHandler>();
                    container.Register<IRequestHandler<GetUserQuery, User>, AgnosticDbGetUserQueryHandler>();
                    container.Register<IRequestHandler<UpdateUserCommand,int>, UpdateUserCommandHandler>();
                    container.Register<IRequestHandler<DeleteUserCommand,int>, DeleteUserCommandHandler>();
                    container.Register<IGetUserByIdQuery,GetUserByIdQuery>();
                    container.Register<IRequestHandler<PagedUserListQuery, IEnumerable<User>>, NpgsqlUserListQueryPagingHandler>();
                    break;
                default:
                    throw new ArgumentException("Invalid ProviderName in connection string.");
            }
            container.Register<IMediator>(new Mediator(container.Resolve, container.ResolveAll));
        }
    }
}