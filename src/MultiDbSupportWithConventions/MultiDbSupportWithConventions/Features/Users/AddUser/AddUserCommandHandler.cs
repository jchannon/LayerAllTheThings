namespace MultiDbSupportWithConventions.Features.Users.AddUser
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

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

            //Move validation out of modules and validate here and throw on error for module to catch
            var validator = new AddUserValidator();
            validator.ValidateAndThrow(message);

            //Contrived shared logic across shared across multi db implementations
            var userAlreadyExist = this.UserExists(message);

            if (userAlreadyExist)
            {
                throw new ValidationException(new[]{new ValidationFailure("Email","User with this email already exists") });
            }

            var id = this.StoreNewUser(message);

            return id;
        }

        protected abstract int StoreNewUser(AddUserCommand message);
        protected abstract bool UserExists(AddUserCommand message);
    }
}