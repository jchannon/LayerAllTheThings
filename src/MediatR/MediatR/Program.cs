using System;
using System.Collections.Generic;
using MediatR;
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
            container.Register<InMemoryDatabase>().AsSingleton();
            container.Register<IMediator>(new Mediator(container.Resolve, container.ResolveAll));
            container.Register<IRequestHandler<GetInventoryItem, InventoryItem>, GetInventoryItemHandler>();
            container.Register<IRequestHandler<Envelop<SellInventory>, Unit>, SellInventoryHandler>();
        }
    }
    
    public class PersonModule : NancyModule
    {
        public PersonModule(IMediator mediator)
        {
            Get["/"] = _ => "Hi Earth People!";

            Get["/{id:int}"] = parameters =>
            {
                var query = new GetInventoryItem((int)parameters.id);
                var item = mediator.Send(query);
                return item;
            };

            Put["/sell/{commandId:guid}"] = parameters =>
            {
                var message = this.BindCommandEnvolope<SellInventory>((Guid)parameters.commandId);
                mediator.Send(message);
                return HttpStatusCode.NoContent;
            };

        }
    }

    public class InMemoryDatabase
    {
        public IList<InventoryItem> Inventory = new List<InventoryItem>
        {
            new InventoryItem(1, "Taylormade M1 460", 599.99m, 5),
            new InventoryItem(2, "Titleist 945D2", 499.99m, 8),
            new InventoryItem(3, "Callaway Golf XR", 299.99m, 3),
            new InventoryItem(4, "Nike Vapor", 329.99m, 1),
            new InventoryItem(5, "Cobra Fly Z+", 399.99m, 2),
        };

        public IList<Guid> Idempotent = new List<Guid>();
    }

    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public decimal Price { get; set; }
        public int QuantityOnHand { get; set; }

        public InventoryItem(int id, string name, decimal price, int quantityOnHand)
        {
            Id = id;
            Name = name;
            Price = price;
            QuantityOnHand = quantityOnHand;
        }

        public void SellInventory(int quantity)
        {
            if ((QuantityOnHand - quantity) < 0)
            {
                throw new InvalidOperationException("Cannot sell more than on hand.");
            }

            QuantityOnHand -= quantity;
        }
    }


}
