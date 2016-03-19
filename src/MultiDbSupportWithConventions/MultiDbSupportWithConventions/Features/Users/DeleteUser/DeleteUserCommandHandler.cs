using MediatR;
using MultiDbSupportWithConventions.Features.Users.DataAccess;
using Dapper;

namespace MultiDbSupportWithConventions
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IDbConnectionProvider dbConnectionProvider;
        private readonly IGetUserByIdQuery getUserByIdQuery;

        public DeleteUserCommandHandler(IDbConnectionProvider dbConnectionProvider, IGetUserByIdQuery getUserByIdQuery)
        {
            //A shared query that can be used by other handlers by injecting it. This is a class with one method not a repository with lots of methods
            this.getUserByIdQuery = getUserByIdQuery;

            this.dbConnectionProvider = dbConnectionProvider;
        }

        public int Handle(DeleteUserCommand message)
        {
            //You wouldn't do this normally as you can just try and delete without getting the user first but 
            //just an example of using a shared query class
            var user = this.getUserByIdQuery.Execute(message.Id);

            using (var conn = this.dbConnectionProvider.GetConnection())
            {
                return conn.Execute("delete from user where id = @id",
                    new {id = message.Id});
            }
        }
    }
}