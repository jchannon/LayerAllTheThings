
namespace MultiDbSupportWithConventions.Tests.Features.Users.GetUser
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using FakeItEasy;

    using MediatR;

    using Microsoft.Owin.Builder;

    using MultiDbSupportWithConventions.Features.Users;
    using MultiDbSupportWithConventions.Features.Users.GetUser;

    using Xunit;

    public class GetUserTests
    {
        [Fact]
        public async Task Should_return_404_when_user_not_found()
        {
            var appBuilder = new AppBuilder();

            var fakeGetUserHandler = A.Fake<IRequestHandler<GetUserQuery, User>>();
            A.CallTo(() => fakeGetUserHandler.Handle(A<GetUserQuery>.Ignored))
                .Throws(() => new InvalidOperationException());

            new Startup(new OurTestBStrapper(getUserRequestHandler: fakeGetUserHandler)).Configuration(appBuilder);

            var handler = new OwinHttpMessageHandler(appBuilder.Build())
            {
                UseCookies = true
            };

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost")
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await client.GetAsync("/1");

            Assert.Equal(404, (int)response.StatusCode);

        }
    }
}
