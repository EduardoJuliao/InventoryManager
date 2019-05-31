using System.Linq;
using System.Threading.Tasks;
using InventoryHandler.Clients;
using InventoryHandler.Interfaces.Entities;
using InventoryHandler.Tests.StubEntities;

namespace InventoryHandler.Tests.ClientStubs
{
    public class ItemClientStub : IItemClient
    {
        public async Task<IItem> Get(string itemId)
        {
            if (!(await ItemExists(itemId)))
                return null;

            return await Task.Run(() => new ItemStub
            {
                Id = itemId,
                Name = itemId
            });
        }

        public async Task<bool> ItemExists(string itemId)
        {
            return await Task.Run(() => { return itemId.All(char.IsUpper); });
        }
    }
}