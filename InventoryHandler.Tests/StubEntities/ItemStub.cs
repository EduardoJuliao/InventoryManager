using InventoryHandler.Interfaces.Entities;

namespace InventoryHandler.Tests.StubEntities
{
    public class ItemStub : IItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}