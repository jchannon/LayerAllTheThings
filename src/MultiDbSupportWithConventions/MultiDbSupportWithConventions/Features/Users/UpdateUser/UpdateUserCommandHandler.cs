using MediatR;
using MultiDbSupportWithConventions.Features.Users.GetUser;
using Dapper;

namespace MultiDbSupportWithConventions.Features.Users.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
    {
        private readonly IRequestHandler<GetUserQuery, User> getUserQueryHandler;
        private readonly IDbConnectionProvider dbConnectionProvider;

        public UpdateUserCommandHandler(IDbConnectionProvider dbConnectionProvider, IRequestHandler<GetUserQuery, User> getUserQueryHandler)
        {
            //Inject another query handler to re-use some code.
            //NOTE: I DO NOT LIKE THIS, see DeleteUserCommandHandler for better approach
            this.getUserQueryHandler = getUserQueryHandler;

            this.dbConnectionProvider = dbConnectionProvider;
        }

        public int Handle(UpdateUserCommand message)
        {
            //You wouldn't do this normally as you can just try and update without getting the user first but 
            //just an example of using a shared queryhandler class
            var existingUser = this.getUserQueryHandler.Handle(new GetUserQuery(message.Id));

            using (var conn = this.dbConnectionProvider.GetConnection())
            {
                return conn.Execute(@"
                        uodate users set firstname = @firstnmae, lastname = @lastname,email = @email where id = @id",
                    new {firstname = message.FirstName, lastname = message.LastName, email = message.Email, id = message.Id});
            }
        }
    }
}