namespace MultiDbSupportWithConventions.Features.Users.AddUser
{
    using System;
    using System.Linq;

    using Dapper;

    using MediatR;

    public abstract class AddUser : IRequestHandler<UserInputModel, int>
    {
        protected readonly IDbConnectionProvider connectionProvider;

        public AddUser(IDbConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public int Handle(UserInputModel message)
        {
            //Contrived shared logic across shared across multi db implementations
            var userAlreadyExist = this.UserExists(message);

            if (userAlreadyExist)
            {
                //We could add a custom validation error to gracefully return a message
                //We could throw an exception, I think the validation would bet better but I'm currently feeling lazy on a Tuesday morning in 2015
                throw new Exception("User exists");
            }

            var id = this.StoreNewUser(message);

            return id;
        }

        protected abstract int StoreNewUser(UserInputModel message);

        protected abstract bool UserExists(UserInputModel message);
    }

    public class MsSqlAddUserCommandHandler : AddUser
    {
        public MsSqlAddUserCommandHandler(IDbConnectionProvider connectionProvider) : base(connectionProvider)
        {
        }

        protected override int StoreNewUser(UserInputModel message)
        {
            using (var conn = this.connectionProvider.GetConnection())
            {
                return conn.Query<int>(@"
                        insert into users(firstname,lastname,email) values (@firstname,@lastname,@email);
                        select cast(SCOPE_IDENTITY() as int)",
                        new {firstname = message.FirstName, lastname = message.LastName, email = message.Email})
                        .Single();
            }
        }

        protected override bool UserExists(UserInputModel message)
        {
            using (var conn = this.connectionProvider.GetConnection())
            {
                var count= conn.ExecuteScalar<int>("select count(*) from users where email = @email",new{email = message.Email });
                return count > 0;
            }
        }
    }
}
