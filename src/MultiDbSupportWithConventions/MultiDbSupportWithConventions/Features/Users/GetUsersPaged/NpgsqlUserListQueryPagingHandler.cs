using MultiDbSupportWithConventions.Features.Users.GetUsers;
using System.Collections.Generic;
using MultiDbSupportWithConventions.Features.Users;
using Dapper;

namespace MultiDbSupportWithConventions
{
    public class NpgsqlUserListQueryPagingHandler : AbstractNpgsqlPagingRequestHandler<PagedUserListQuery, IEnumerable<User>>
    {
        private IDbConnectionProvider dbConnectionProvider;

        public NpgsqlUserListQueryPagingHandler(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
            this.SQL = "select * from users";
        }

        public override IEnumerable<User> Handle(PagedUserListQuery message)
        {
            using (var dbConnection = this.dbConnectionProvider.GetConnection())
            {
                //Here we can do postgres paging specific sql if needs be
                var data = dbConnection.Query<User>(this.PagingSQL, 
                    new 
                    {
                        Limit = 20,
                        Offset = 0
                    });

                return data;
            }
        }
    }
}