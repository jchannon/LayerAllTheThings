namespace MultiDbSupportWithConventions
{
    using Nancy.Bootstrapper;

    using Owin;

    public class Startup
    {
        private readonly INancyBootstrapper bootstrapper;

        public Startup(INancyBootstrapper bootstrapper)
        {
            this.bootstrapper = bootstrapper;
        }

        public Startup()
        {
            this.bootstrapper = new Bootstrapper();
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options => options.Bootstrapper = this.bootstrapper);
        }
    }
}