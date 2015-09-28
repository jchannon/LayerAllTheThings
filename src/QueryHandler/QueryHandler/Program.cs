using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Hosting;
using Nancy;
using Owin;

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

            container.Register<IMediate,Mediator>(mediator);
        }
    }

    public interface IQuery<out TResponse>
    {

    }

    public interface IHandleQueries<in TQuery, out TResponse>
    where TQuery : IQuery<TResponse>
    {
        TResponse Handle(TQuery query);
    }

    public interface IMediate
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);
    }

    public class Mediator : IMediate
    {
        private readonly IHandleQueries<IQuery<Person>, Person> userqueryhandler;

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
    }

    public class UserQuery : IQuery<Person>
    {
        public int UserId { get; private set; }

        public UserQuery(int userId)
        {
            UserId = userId;
        }
    }

    public class UserQueryHandler : IHandleQueries<IQuery<Person>, Person>
    {
        //Our DB
        public static Dictionary<int, Person> data = new Dictionary<int, Person>()
        { 
            { 1, new Person{ FirstName = "Jim", LastName = "Parsons", EmailAddress = "jim@parsons.com" } }, 
            { 2, new Person{ FirstName = "Fred", LastName = "Smith", EmailAddress = "fred@smith.com" } }, 
            { 3, new Person{ FirstName = "Bob", LastName = "Hope", EmailAddress = "bob@hope.com" } }, 
            { 4, new Person{ FirstName = "Bernard", LastName = "Targarian", EmailAddress = "bernard@targarian.com" } }, 
            { 5, new Person{ FirstName = "Troy", LastName = "Vega", EmailAddress = "troy@vega.com" } }
        };

        public Person Handle(IQuery<Person> query)
        {
            var userQuery = query as UserQuery;
            return data.FirstOrDefault(x => x.Key == userQuery.UserId).Value;
        }
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

            // Post["/"] = parameters =>
            // {
            //     var person = this.Bind<Person>();

            //     var errors = accountService.Create(person);

            //     if (errors.Any())
            //     {
            //         foreach (var item in errors)
            //         {
            //             ModelValidationResult.Errors.Add("Person", item);
            //         }
            //         return ModelValidationResult.Errors;
            //     }

            //     return 201;
            // };

        }
    }

    public class Person
    {
        public string FirstName{ get; set; }

        public string LastName{ get; set; }

        public string FullName{ get { return FirstName + " " + LastName; } }

        public string EmailAddress { get; set; }
    }
}
