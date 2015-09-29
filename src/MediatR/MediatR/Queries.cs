using System.Linq;
using MediatR;

namespace QueryHandler
{
    public class GetInventoryItem : IRequest<InventoryItem>
    {
        public int Id { get; private set; }

        public GetInventoryItem(int id)
        {
            Id = id;
        }
    }

    public class GetInventoryItemHandler : IRequestHandler<GetInventoryItem, InventoryItem>
    {
        private readonly InMemoryDatabase _db;

        public GetInventoryItemHandler(InMemoryDatabase db)
        {
            _db = db;
        }

        public InventoryItem Handle(GetInventoryItem message)
        {
            return _db.Inventory.FirstOrDefault(x => x.Id == message.Id);
        }
    }
}
