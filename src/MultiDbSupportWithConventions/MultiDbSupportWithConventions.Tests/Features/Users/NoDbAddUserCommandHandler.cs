namespace MultiDbSupportWithConventions.Tests.Features.Users
{
    using MultiDbSupportWithConventions.Features.Users;
    using MultiDbSupportWithConventions.Features.Users.AddUser;

    public class NoDbAddUserCommandHandler : AddUserCommandHandler
    {
        private readonly int newUserId;
        private readonly bool userExists;

        public NoDbAddUserCommandHandler(bool userExists, int newUserId)
            : base(null)
        {
            this.userExists = userExists;
            this.newUserId = newUserId;
        }

        protected override int StoreNewUser(AddUserCommand message)
        {
            return this.newUserId;
        }

        protected override bool UserExists(AddUserCommand message)
        {
            return this.userExists;
        }
    }
}