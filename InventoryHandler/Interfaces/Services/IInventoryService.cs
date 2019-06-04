using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryHandler.Interfaces.Entities;

namespace InventoryHandler.Interfaces.Services
{
    public interface IInventoryService
    {
        Task<bool> ExistsAsync(string adventurerId, string itemId);
        Task<IInventoryItem> GetItemFromIventoryAsync(string adventurerId, string itemId);
        Task<IEnumerable<IInventoryItem>> ShowInventoryAsync(string adventurerId);
        Task AddItemAsync(string adventurerId, string itemId, int amount = 1);
        Task AddItemsAsync(string adventurerId, IEnumerable<KeyValuePair<string, int>> items);
        Task RemoveItemAsync(string adventurerId, string itemId, int amount = 1);

    }
}