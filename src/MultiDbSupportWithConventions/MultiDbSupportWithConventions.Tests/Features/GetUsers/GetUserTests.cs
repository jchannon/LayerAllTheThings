namespace MultiDbSupportWithConventions.Tests.Features.GetUsers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Owin.Builder;

    using MultiDbSupportWithConventions.Features.Users;

    using Xunit;

    public class GetUserTests
    {
        [Fact]
        public async Task FactMethodName()
        {
            var appBuilder = new AppBuilder();
            //var meditaR = A.Fake<IMediator>();
            //A.CallTo(() => meditaR.Send(A<IRequest<IEnumerable<User>>>.Ignored))
            //    .Returns(new[] {new User() {Email = "qwe@qwe.com"}});

            //new Startup(new ConfigurableBootstrapper(with =>
            //{
            //    with.Module<HomeModule>();
            //    with.Dependency<IMediator>(meditaR);
            //})).Configuration(appBuilder);
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

            Assert.Equal(1,data.Count);
        }
    }
}