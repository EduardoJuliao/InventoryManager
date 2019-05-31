using InventoryHandler.Interfaces.Entities;

namespace InventoryHandler.Tests.StubEntities
{
    public class InventoryItemStub : IInventoryItem
    {
        public int Amount { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}