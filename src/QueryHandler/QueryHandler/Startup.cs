using Owin;

namespace QueryHandler
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(); 
        }
    }
}
