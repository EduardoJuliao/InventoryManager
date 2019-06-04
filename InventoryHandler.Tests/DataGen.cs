using System.Collections.Generic;
using System.Linq;
using InventoryHandler.Interfaces.Entities;
using InventoryHandler.Tests.StubEntities;

namespace InventoryHandler.Tests
{
    public class DataGen
    {
        public static IEnumerable<IInventoryItem> GenerateInventory()
        {
            return Enumerable.Range(65, 25).Select(x => new InventoryItemStub
            {
                Id = ((char)x).ToString(),
                Name = ((char)x).ToString(),
                Amount = 10
            });
        }

        public static IEnumerable<KeyValuePair<string, int>> GenerateInventoryValuePair()
        {
            return Enumerable.Range(65, 25).ToDictionary(x => ((char)x).ToString(), x => 10);
        }
    }
}