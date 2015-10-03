using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using TraditionalLayering.Model;
using TraditionalLayering.Service;

namespace TraditionalLayering.NancyModule {
    public class PersonModule : Nancy.NancyModule
    {
        
        public PersonModule(IAccountService accountService)
        {
            Get["/"] = _ => "Hi Earth People!";

            Get["/{id:int}"] = parameters =>
            {
                var person = accountService.GetLoggedInUser((int)parameters.id);
                return person;
            };

            Post["/"] = parameters =>
            {
                var person = this.Bind<Person>();

                var errors = accountService.Create(person);

                if (errors.Any())
                {
                    foreach (var item in errors)
                    {
                        ModelValidationResult.Errors.Add("Person", item);
                    }
                    return ModelValidationResult.Errors;
                }

                return 201;
            };

        }
    }
}