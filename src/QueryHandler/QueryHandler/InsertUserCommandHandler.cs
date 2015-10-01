using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace QueryHandler
{
    public class InsertUserCommandHandler : ICommandHandler<ICommand<int>,int>
    {
        public int Handle(ICommand<int> command)
        {
            var insertUserCmd = command as InsertUserCommand;

            var errorList = new Dictionary<string,string>();

            //Validation
            var validator = new UserValidator();
            validator.ValidateAndThrow(insertUserCmd.User);

            //Basic business logic
            var existingPerson = DB.Data.Values.FirstOrDefault(x => x.EmailAddress == insertUserCmd.User.EmailAddress);

            if (existingPerson != null)
            {
                errorList.Add("EmailAddress", "User already exists");
            }

            if (errorList.Any())
            {
                var validationErrors = new List<ValidationFailure>();
                foreach (var item in errorList)
                {
                    validationErrors.Add(new ValidationFailure(item.Key, item.Value));
                }
                throw new ValidationException(validationErrors);
            }

            //Other business logic that might do checks and return errors

            var newid = DB.Data.Keys.Count + 1;
            DB.Data.Add(newid, insertUserCmd.User);

            return newid;
        }

        public bool CanHandle(ICommand<int> command)
        {
            return command is InsertUserCommand;
        }
    }
}
