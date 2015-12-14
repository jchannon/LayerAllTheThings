namespace MultiDbSupport
{
    using System.Collections.Generic;
    using System.Data;

    using Dapper;

    using MediatR;

    public class UserListQueryRequestHandler : IRequestHandler<UserListQuery, IEnumerable<User>>
    {
        private readonly IDbConnectionProvider dbConnectionProvider;

        public UserListQueryRequestHandler(IDbConnectionProvider dbConnectionProvider)
        {
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<User> Handle(UserListQuery message)
        {
            //We need to support multi datbases so when we want to query the database for our users what db specific SQL should we use?
            //One option is to put the SQL in a resx file but then we'll need to do a if/then/else check in all our query classes
            //to work out which sql to use, not pretty.
            //Another option would be to move the abstraction to the database and use stored procedures! That way we can have one query class
            //and pass any parameters into the stored procedure and that can do whatever it needs to do. 

            using (var dbConnection = this.dbConnectionProvider.GetConnection())
            {
                var data = dbConnection.Query<User>("spGetUsers", commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}