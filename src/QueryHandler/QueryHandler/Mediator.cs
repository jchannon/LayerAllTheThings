using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryHandler
{
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
            var handler = Resolve<IHandleQueries<IQuery<TResponse>,TResponse>>(query.GetType());
            return handler.Handle(query);
        }

        public TResponse Send<TResponse>(ICommand<TResponse> cmd)
        {
            var handler = Resolve<ICommandHandler<ICommand<TResponse>, TResponse>>(cmd.GetType());
            return handler.Handle(cmd);
        }

        public T Resolve<T>(Type instance)
        {
            T item = default(T);

            foreach (var registration in Classes)
            {
                try
                {
                    var instansiatedObject = (T)Activator.CreateInstance(registration.ConcreteType);

                    var mi = typeof(T).GetMethod("CanHandle"); 

                    var canHandle = (bool)mi.Invoke(instansiatedObject, new object[] { instance });
                    if (canHandle)
                    {
                        item = instansiatedObject;
                    }
                }
                catch
                {
                }
            }


            return item;
        }


        public void Register<TInterface,TImplementation>()
        {
            Classes.Add(new Registration{ InterfaceType = typeof(TInterface), ConcreteType = typeof(TImplementation) });
        }

        private readonly List<Registration> Classes = new List<Registration>();

        public class Registration
        {
            public Type InterfaceType { get; set; }

            public Type ConcreteType { get; set; }
        }
    }
}
