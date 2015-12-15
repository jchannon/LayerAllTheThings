namespace MultiDbSupportWithConventions.Features.Users.GetUsers
{
    using System.Collections.Generic;

    using Dapper;

    using MediatR;

    public class MssqlUserListQueryRequestHandler : IRequestHandler<UserListQuery, IEnumerable<User>>
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public MssqlUserListQueryRequestHandler(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<User> Handle(UserListQuery message)
        {
            using (var dbConnection = this.dbConnectionProvider.GetConnection())
            {
                //Here we can do MSSQL specific sql if needs be
                var data = dbConnection.Query<User>("select * from users");
                return data;
            }
        }
    }
}