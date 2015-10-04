using System;
using System.Linq;
using MediatR;

namespace QueryHandler
{

    public class Envelope<T> : IRequest
    {
        public DateTime Created { get; private set; }
        public Guid CommandId { get; private set; }
        public T Command { get; private set; }

        public Envelope(Guid commandId, T command)
        {
            CommandId = commandId;
            Created = DateTime.UtcNow;
            Command = command;
        }
    }

    public class SellInventory
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public SellInventory() { }

        public SellInventory(int id, int quantity)
        {
            Quantity = quantity;
            Id = Id;
        }
    }

    public class SellInventoryHandler : IRequestHandler<Envelope<SellInventory>, Unit>
    {
        private readonly InMemoryDatabase _db;

        public SellInventoryHandler(InMemoryDatabase db)
        {
            _db = db;
        }

        public Unit Handle(Envelope<SellInventory> message)
        {
            // Skip any commands already processed
            if (_db.Idempotent.Any(x => x == message.CommandId)) return new Unit();

            var item = (from x in _db.Inventory where x.Id == message.Command.Id select x).SingleOrDefault();
            if (item == null) throw new InvalidOperationException("Inventory item not found.");
            
            item.SellInventory(message.Command.Quantity);
            
            _db.Idempotent.Add(message.CommandId);

            return new Unit();
        }
    }
}
