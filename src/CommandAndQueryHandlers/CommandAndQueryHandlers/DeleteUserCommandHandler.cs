using System;
using System.Linq;

namespace QueryHandler
{
    public class DeleteUserCommandHandler : ICommandHandler<ICommand<int>,int>
    {
        public int Handle(ICommand<int> command)
        {
            var deleteUserCommand = command as DeleteUserCommand;
            var removed = DB.Data.Remove(DB.Data.FirstOrDefault(x => x.Id == deleteUserCommand.UserId));
            return removed ? 1 : 0;
        }

        public bool CanHandle(Type commandType)
        {
            return commandType == typeof(DeleteUserCommand);
        }
    }
}

