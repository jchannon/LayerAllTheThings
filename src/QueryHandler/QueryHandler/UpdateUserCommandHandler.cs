using FluentValidation;

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

            DB.Data[updateUserCmd.User.Id] = updateUserCmd.User;

            //Rows affected
            return 1;
        }

        public bool CanHandle(ICommand<int> command)
        {
            return command is UpdateUserCommand;
        }
    }
}

