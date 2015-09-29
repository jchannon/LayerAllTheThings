using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Hosting;
using Nancy;
using Owin;
using Nancy.ModelBinding;
using Nancy.Validation;
using FluentValidation;

namespace QueryHandler
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

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            var mediator = new Mediator();
            
            mediator.Register<IHandleQueries<IQuery<Person>, Person>>(delegate
                {
                    return new UserQueryHandler();
                });

            mediator.Register<ICommandHandler<ICommand<int>, int>>(delegate
                {
                    return new UserCommandHandler();
                }
            );

            container.Register<IMediate,Mediator>(mediator);
        }
    }

    public interface ICommand<out TResponse>
    {
        
    }

    public interface IQuery<out TResponse>
    {

    }

    public interface ICommandHandler<in TCommand, out TResponse>
        where TCommand : ICommand<TResponse>
    {
        TResponse Handle(TCommand command);
    }

    public interface IHandleQueries<in TQuery, out TResponse>
    where TQuery : IQuery<TResponse>
    {
        TResponse Handle(TQuery query);
    }

    public interface IMediate
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);

        TResponse Send<TResponse>(ICommand<TResponse> cmd);
    }

    public class Mediator : IMediate
    {
        public delegate object Creator(Mediator container);

        private readonly Dictionary<Type, Creator> _typeToCreator = new Dictionary<Type, Creator>();

        public void Register<T>(Creator creator)
        {
            _typeToCreator.Add(typeof(T), creator);
        }

        private T Create<T>()
        {
            return (T)_typeToCreator[typeof(T)](this);
        }

        public TResponse Request<TResponse>(IQuery<TResponse> query)
        {
            var handler = Create<IHandleQueries<IQuery<TResponse>, TResponse>>();
            return handler.Handle(query);
        }

        public TResponse Send<TResponse>(ICommand<TResponse> cmd)
        {
            var handler = Create<ICommandHandler<ICommand<TResponse>, TResponse>>();
            return handler.Handle(cmd);
        }
    }

    public class UserQuery : IQuery<Person>
    {
        public int UserId { get; private set; }

        public UserQuery(int userId)
        {
            UserId = userId;
        }
    }

    public class PersonCommand : ICommand<int>
    {
        public Person Person { get; private set; }

        public PersonCommand(Person person)
        {
            this.Person = person;
        }
    }

    public class UserCommandHandler : ICommandHandler<ICommand<int>,int>
    {
        public int Handle(ICommand<int> command)
        {
            var personcmd = command as PersonCommand;

            var errorList = new List<string>();

            //Validation
            var validator = new PersonValidator();
            var validationresult = validator.Validate(personcmd.Person);
            if (!validationresult.IsValid)
            {
                foreach (var error in validationresult.Errors)
                {
                    errorList.Add(error.ErrorMessage);
                }
            }

            //Basic business logic
            var existingPerson = DB.Data.Values.FirstOrDefault(x => x.EmailAddress == personcmd.Person.EmailAddress);

            if (existingPerson != null)
            {
                errorList.Add("User already exists");
            }

            if (errorList.Any())
            {
                throw new OopsyException("Business Logic Exception", errorList);
            }

            //Other business logic that might do checks and return errors

            var newid = DB.Data.Keys.Count + 1;
            DB.Data.Add(newid, personcmd.Person);

            return newid;
        }
    }

    public class UserQueryHandler : IHandleQueries<IQuery<Person>, Person>
    {
        public Person Handle(IQuery<Person> query)
        {
            var userQuery = query as UserQuery;
            return DB.Data.FirstOrDefault(x => x.Key == userQuery.UserId).Value;
        }
    }

    public class DB
    {
        //Our DB
        public static Dictionary<int, Person> Data = new Dictionary<int, Person>()
        { 
            { 1, new Person{ FirstName = "Jim", LastName = "Parsons", EmailAddress = "jim@parsons.com" } }, 
            { 2, new Person{ FirstName = "Fred", LastName = "Smith", EmailAddress = "fred@smith.com" } }, 
            { 3, new Person{ FirstName = "Bob", LastName = "Hope", EmailAddress = "bob@hope.com" } }, 
            { 4, new Person{ FirstName = "Bernard", LastName = "Targarian", EmailAddress = "bernard@targarian.com" } }, 
            { 5, new Person{ FirstName = "Troy", LastName = "Vega", EmailAddress = "troy@vega.com" } }
        };
    }

    public class PersonModule : NancyModule
    {
        
        public PersonModule(IMediate mediator)
        {
            Get["/"] = _ => "Hi Earth People!";

            Get["/{id:int}"] = parameters =>
            {
                var userQuery = new UserQuery((int)parameters.id);
                var person = mediator.Request(userQuery);
                return person;
            };

            Post["/"] = parameters =>
            {
                var person = this.Bind<Person>();
                var personCmd = new PersonCommand(person);
                try
                {
                    var id = mediator.Send(personCmd);
                    return Negotiate.WithStatusCode(HttpStatusCode.Created).WithHeader("Location", Context.Request.Url.ToString() + "/" + id);
                }
                catch (OopsyException ex)
                {
                    return Negotiate.WithModel(ex.ModelValidationResult.FormattedErrors).WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
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

    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.EmailAddress).NotEmpty();
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
        }
    }

    public class OopsyException : Exception
    {
        public ModelValidationResult ModelValidationResult
        {
            get;
            private set;
        }

        public OopsyException(string message, List<string> errors)
            : base(message)
        {
            this.ModelValidationResult = new ModelValidationResult();
            foreach (var item in errors)
            {
                this.ModelValidationResult.Errors.Add("Person", item);
            }
        }
    }
}
