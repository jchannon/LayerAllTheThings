namespace MultiDbSupportWithConventions.Tests
{
    using System.Collections.Generic;

    using FakeItEasy;

    using MediatR;

    using MultiDbSupportWithConventions.Features.Users;
    using MultiDbSupportWithConventions.Features.Users.GetUsers;
    using MultiDbSupportWithConventions.Tests.Features.Users.AddUser;

    using Nancy.TinyIoc;

    public class OurTestBStrapper : Bootstrapper
    {
        private readonly IRequestHandler<AddUserCommand, int> addUserRequestHandler;

        public OurTestBStrapper(IRequestHandler<AddUserCommand, int> addUserRequestHandler = null)
        {
            this.addUserRequestHandler = addUserRequestHandler ?? new NoDbAddUserCommandHandler(userExists:false,newUserId:1);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var fakeUserQuery = A.Fake<IRequestHandler<UserListQuery, IEnumerable<User>>>();
            A.CallTo(() => fakeUserQuery.Handle(A<UserListQuery>.Ignored))
                .Returns(new[] { new User { Email = "fred@home.com" } });

            container.Register<IRequestHandler<UserListQuery, IEnumerable<User>>>(fakeUserQuery);
            container.Register<IRequestHandler<AddUserCommand, int>>(this.addUserRequestHandler);
        }
    }
}