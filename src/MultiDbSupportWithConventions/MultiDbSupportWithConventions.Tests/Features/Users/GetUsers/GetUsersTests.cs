namespace MultiDbSupportWithConventions.Tests.Features.Users.GetUsers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Owin.Builder;

    using MultiDbSupportWithConventions.Features.Users;

    using Xunit;

    public class GetUsersTests
    {
        [Fact]
        public async Task Should_Return_List_Of_Users()
        {
            var appBuilder = new AppBuilder();
            new Startup(new OurTestBStrapper()).Configuration(appBuilder);

            var handler = new OwinHttpMessageHandler(appBuilder.Build())
            {
                UseCookies = true
            };

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost")
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await client.GetAsync("/");
            var data = await response.Content.ReadAsAsync<List<User>>();

            Assert.Equal(1, data.Count);
        }
    }
}