namespace MultiDbSupportWithConventions.Features.Users.AddUser
{
    using System.Linq;

    using Dapper;

    public class MsSqlAddUserCommandCommandHandler : AddUserCommand
    {
        public MsSqlAddUserCommandCommandHandler(IDbConnectionProvider connectionProvider) : base(connectionProvider)
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
