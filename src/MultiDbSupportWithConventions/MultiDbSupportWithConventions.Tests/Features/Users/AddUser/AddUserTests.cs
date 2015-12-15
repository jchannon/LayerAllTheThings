namespace MultiDbSupportWithConventions.Tests.Features.Users.AddUser
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Owin.Builder;

    using MultiDbSupportWithConventions.Features.Users;

    using Xunit;

    public class AddUserTests
    {
        [Fact]
        public async Task Should_contain_location_header_and_201()
        {
            var appBuilder = new AppBuilder();
            new Startup(new OurTestBStrapper(new NoDbAddUserCommandHandler(userExists: false, newUserId: 1)))
                .Configuration(appBuilder);

            var handler = this.GetOwinHttpMessageHandler(appBuilder);

            var client = this.GetHttpClient(handler);

            var response = await client.PostAsJsonAsync("/", new AddUserCommand
            {
                FirstName = "vincent",
                LastName = "vega",
                Email = "vincent@home.com"
            });

            Assert.Equal(201, (int)response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task Should_return_422_on_invalid_data()
        {
            var appBuilder = new AppBuilder();
            new Startup(new OurTestBStrapper(new NoDbAddUserCommandHandler(userExists: false, newUserId: 1)))
                .Configuration(appBuilder);

            var handler = this.GetOwinHttpMessageHandler(appBuilder);

            var client = this.GetHttpClient(handler);

            var response = await client.PostAsJsonAsync("/", new AddUserCommand());

            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task Should_return_422_if_existing_user_exists()
        {
            var appBuilder = new AppBuilder();
            new Startup(new OurTestBStrapper(new NoDbAddUserCommandHandler(userExists: true, newUserId: 1)))
                .Configuration(appBuilder);

            var handler = this.GetOwinHttpMessageHandler(appBuilder);

            var client = this.GetHttpClient(handler);

            var response = await client.PostAsJsonAsync("/", new AddUserCommand
            {
                FirstName = "Vincent",
                LastName = "Vega",
                Email = "vincent@home.com"
            });

            Assert.Equal(422, (int)response.StatusCode);
        }

        private HttpClient GetHttpClient(OwinHttpMessageHandler handler)
        {
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost")
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        private OwinHttpMessageHandler GetOwinHttpMessageHandler(AppBuilder appBuilder)
        {
            var handler = new OwinHttpMessageHandler(appBuilder.Build())
            {
                UseCookies = true
            };
            return handler;
        }
    }
}