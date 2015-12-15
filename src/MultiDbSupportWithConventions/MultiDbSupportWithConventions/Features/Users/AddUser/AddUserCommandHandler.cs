namespace MultiDbSupportWithConventions.Features.Users.AddUser
{
    using System;

    using MediatR;

    public abstract class AddUserCommandHandler : IRequestHandler<AddUserCommand, int>
    {
        protected readonly IDbConnectionProvider connectionProvider;

        public AddUserCommandHandler(IDbConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public int Handle(AddUserCommand message)
        {
            //This method can have unit tests or acceptance tests as shown in the MultiDbSupportWithConventions.Tests project


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

        protected abstract int StoreNewUser(AddUserCommand message);

        protected abstract bool UserExists(AddUserCommand message);
    }
}