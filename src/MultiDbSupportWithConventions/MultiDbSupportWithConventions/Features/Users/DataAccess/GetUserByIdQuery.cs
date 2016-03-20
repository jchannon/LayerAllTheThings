using MultiDbSupportWithConventions.Features.Users;
using Dapper;
using System.Linq;
using System;

namespace MultiDbSupportWithConventions.Features.Users.DataAccess
{
    public class GetUserByIdQuery : IGetUserByIdQuery
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public GetUserByIdQuery (IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
            
        }
        public User Execute(int id)
        {
            using (var conn = this.dbConnectionProvider.GetConnection())
            {
                var user =
                    conn.Query<User>("select * from users where id = @id", new {id = id}).FirstOrDefault();

                if (user == null)
                {
                    throw new InvalidOperationException("Unable to find user with id : " + id);
                }

                return user;
            }
        }
    }
}