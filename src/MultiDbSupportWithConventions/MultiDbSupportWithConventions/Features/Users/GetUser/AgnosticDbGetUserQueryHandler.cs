namespace MultiDbSupportWithConventions.Features.Users.GetUser
{
    using System;
    using System.Linq;

    using Dapper;

    using MediatR;

    public class AgnosticDbGetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public AgnosticDbGetUserQueryHandler(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public User Handle(GetUserQuery message)
        {
            //This class will need to be tested against a live database in an integration test
            using (var conn = this.dbConnectionProvider.GetConnection())
            {
                var user =
                    conn.Query<User>("select * from users where id = @id", new {id = message.UserId}).FirstOrDefault();

                if (user == null)
                {
                    throw new InvalidOperationException("Unable to find user with id : " + message.UserId);
                }
                
                return user;
            }
        }
    }
}