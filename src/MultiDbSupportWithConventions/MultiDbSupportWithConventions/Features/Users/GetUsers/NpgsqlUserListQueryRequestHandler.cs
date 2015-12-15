namespace MultiDbSupportWithConventions.Features.Users.GetUsers
{
    using System.Collections.Generic;

    using Dapper;

    using MediatR;

    public class NpgsqlUserListQueryRequestHandler : IRequestHandler<UserListQuery, IEnumerable<User>>
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public NpgsqlUserListQueryRequestHandler(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<User> Handle(UserListQuery message)
        {
            using (var dbConnection = this.dbConnectionProvider.GetConnection())
            {
                //Here we can do postgres specific sql if needs be
                var data = dbConnection.Query<User>("select * from users;");
                return data;
            }
        }
    }
}