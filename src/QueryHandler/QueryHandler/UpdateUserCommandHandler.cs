using FluentValidation;
using System;
using System.Linq;

namespace QueryHandler
{
    public class UpdateUserCommandHandler: ICommandHandler<ICommand<int>,int>
    {
        public int Handle(ICommand<int> command)
        {
            var updateUserCmd = command as UpdateUserCommand;

            //Validation
            var validator = new UserValidator();
            validator.ValidateAndThrow(updateUserCmd.User);

            var currentUser = DB.Data.FirstOrDefault(x => x.Id == updateUserCmd.User.Id);
            currentUser.FirstName = updateUserCmd.User.FirstName;
            currentUser.LastName = updateUserCmd.User.LastName;
            currentUser.EmailAddress = updateUserCmd.User.EmailAddress;


            //Rows affected
            return 1;
        }

        public bool CanHandle(Type commandType)
        {
            return commandType == typeof(UpdateUserCommand);
        }
    }
}

