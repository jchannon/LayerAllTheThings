namespace MultiDbSupportWithConventions.Tests.Features.GetUsers
{
    using System.Collections.Generic;

    using FakeItEasy;

    using MediatR;

    using MultiDbSupportWithConventions.Features.Users;
    using MultiDbSupportWithConventions.Features.Users.GetUsers;

    using Nancy.TinyIoc;

    public class OurTestBStrapper : Bootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var fakeUserQuery = A.Fake<IRequestHandler<UserListQuery, IEnumerable<User>>>();
            A.CallTo(() => fakeUserQuery.Handle(A<UserListQuery>.Ignored))
                .Returns(new[] { new User { Email = "fred@home.com" } });

            container.Register<IRequestHandler<UserListQuery, IEnumerable<User>>>(fakeUserQuery);
        }
    }
}