using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Hosting;
using Nancy;
using Owin;
using Nancy.ModelBinding;
using Nancy.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace QueryHandler
{

    public class UserCommandHandler : ICommandHandler<ICommand<int>,int>
    {
        public int Handle(ICommand<int> command)
        {
            var personcmd = command as PersonCommand;

            var errorList = new Dictionary<string,string>();

            //Validation
            var validator = new PersonValidator();
            validator.ValidateAndThrow(personcmd.Person);

            //Basic business logic
            var existingPerson = DB.Data.Values.FirstOrDefault(x => x.EmailAddress == personcmd.Person.EmailAddress);

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
            DB.Data.Add(newid, personcmd.Person);

            return newid;
        }
    }
    
}
