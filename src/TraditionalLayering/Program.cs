using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Owin.Hosting;
using Nancy;
using Nancy.ModelBinding;
using Owin;

namespace TraditionalLayering
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://+:5678"))
            {
                Console.WriteLine("Running on http://localhost:5678");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
            app.UseNancy();
        }
    }

    public class PersonModule : NancyModule
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

    public class Person
    {
        public string FirstName{ get; set; }

        public string LastName{ get; set; }

        public string FullName{ get { return FirstName + " " + LastName; } }

        public string EmailAddress { get; set; }
    }

    public interface IAccountService
    {
        Person GetLoggedInUser(int id);

        List<string> Create(Person person);
    }

    public interface IAccountRepository
    {
        Person GetLoggedInUser(int id);

        Person GetUserByEmail(string emailAddress);

        void Create(Person person);
    }

    public class AccountService:IAccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
            
        }

        public Person GetLoggedInUser(int id)
        {
            return this.accountRepository.GetLoggedInUser(id);
        }

        public List<string> Create(Person person)
        {
            var errorList = new List<string>();
            var existingPerson = this.accountRepository.GetUserByEmail(person.EmailAddress);

            if (existingPerson != null)
            {
                errorList.Add("User already exists");
                return errorList;
            }

            //Other business logic that might do checks and return errors

            this.accountRepository.Create(person);

            return errorList;  
        }
    }

    public class AccountRepository : IAccountRepository
    {
        public static Dictionary<int, Person> data = new Dictionary<int, Person>()
        { 
            { 1, new Person{ FirstName = "Jim", LastName = "Parsons", EmailAddress = "jim@parsons.com" } }, 
            { 2, new Person{ FirstName = "Fred", LastName = "Smith", EmailAddress = "fred@smith.com" } }, 
            { 3, new Person{ FirstName = "Bob", LastName = "Hope", EmailAddress = "bob@hope.com" } }, 
            { 4, new Person{ FirstName = "Bernard", LastName = "Targarian", EmailAddress = "bernard@targarian.com" } }, 
            { 5, new Person{ FirstName = "Troy", LastName = "Vega", EmailAddress = "troy@vega.com" } }
        };


        public Person GetLoggedInUser(int id)
        {
            return data.FirstOrDefault(x => x.Key == id).Value;
        }

        public Person GetUserByEmail(string emailAddress)
        {
            return data.Values.FirstOrDefault(x => x.EmailAddress == emailAddress);
        }

        public void Create(Person person)
        {
            var newid = data.Keys.Count + 1;
            data.Add(newid, person);
        }
        
    }
}
