namespace DbConnectionAndCommands
{
    using Nancy;
    using Dapper;

    public class HomeModule : NancyModule
    {
        public HomeModule(IDbConnectionProvider connectionProvider)
        {
            Get["/"] = _ =>
            {
                using (var conn = connectionProvider.GetConnection())
                {
                    return conn.Query<User>("select * from users");
                }
            };

            Get["/complicatedquery"] = _ =>
            {
                //What if we need to build up a big view model and need to execute a couple of queries?
                //We could create some methods in this class and separate those queries out and build the view model up.
                //What if we have 3-4 endpoints that need to do the same? We then have a large module class which in my
                //opinion should be anaemic and have its single responsibility be for return http specific things.

                return 200;
            };

            Post["/"] = _ =>
            {
                //So we could inject a dbconnection and return data from this module but I dont like that due to the comments above. 
                //What about POST/PUT? We need somewhere to do business logic etc which I think should be a service or command layer
                return 201;
            };
        }
    }
}
