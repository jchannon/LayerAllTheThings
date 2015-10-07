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

            //Validation - put this here to avoid having code check ModelValidationResult in module. Just a personal preference
            var validator = new UserValidator();
            validator.ValidateAndThrow(insertUserCmd.User);

            //Basic business logic
            var existingPerson = DB.Data.FirstOrDefault(x => x.EmailAddress == insertUserCmd.User.EmailAddress);

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

            var newid = DB.Data.Last().Id + 1;
            insertUserCmd.User.Id = newid;
            DB.Data.Add(insertUserCmd.User);

            return newid;
        }

        public bool CanHandle(Type commandType)
        {
            return commandType == typeof(InsertUserCommand);
        }
    }
}
