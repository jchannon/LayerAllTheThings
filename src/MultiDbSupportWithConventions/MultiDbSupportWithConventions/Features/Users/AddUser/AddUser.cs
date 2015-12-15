namespace MultiDbSupportWithConventions.Features.Users.AddUser
{
    using System;

    using MediatR;

    public abstract class AddUserCommand : IRequestHandler<UserInputModel, int>
    {
        protected readonly IDbConnectionProvider connectionProvider;

        public AddUserCommand(IDbConnectionProvider connectionProvider)
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
}