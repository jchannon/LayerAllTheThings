namespace MultiDbSupportWithConventions.Tests
{
    using System.Collections.Generic;

    using FakeItEasy;

    using MediatR;

    using MultiDbSupportWithConventions.Features.Users;
    using MultiDbSupportWithConventions.Features.Users.GetUser;
    using MultiDbSupportWithConventions.Features.Users.GetUsers;
    using MultiDbSupportWithConventions.Tests.Features.Users;

    using Nancy.TinyIoc;

    public class OurTestBStrapper : Bootstrapper
    {
        private readonly IRequestHandler<GetUserQuery, User> getUserRequestHandler;
        private readonly IRequestHandler<AddUserCommand, int> addUserRequestHandler;

        public OurTestBStrapper(IRequestHandler<AddUserCommand, int> addUserRequestHandler = null, IRequestHandler<GetUserQuery, User> getUserRequestHandler = null)
        {
            this.getUserRequestHandler = getUserRequestHandler;
            this.addUserRequestHandler = addUserRequestHandler;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var fakeUserQuery = A.Fake<IRequestHandler<UserListQuery, IEnumerable<User>>>();
            A.CallTo(() => fakeUserQuery.Handle(A<UserListQuery>.Ignored))
                .Returns(new[] { new User { Email = "fred@home.com" } });

            container.Register<IRequestHandler<UserListQuery, IEnumerable<User>>>(fakeUserQuery);
            container.Register<IRequestHandler<AddUserCommand, int>>(this.addUserRequestHandler);
            container.Register<IRequestHandler<GetUserQuery, User>>(this.getUserRequestHandler);
        }
    }
}