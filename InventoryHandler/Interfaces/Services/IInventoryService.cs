using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryHandler.Interfaces.Entities;

namespace InventoryHandler.Interfaces.Services
{
    public interface IInventoryService
    {
        Task<bool> ExistsAsync(string adventurerId, string itemId);
        Task<IInventoryItem> GetItemFromIventoryAsync(string adventurerId, string itemId);
        Task AddItemAsync(string adventurerId, string itemId);
        Task AddItemsAsync(string adventurerId, string[] itemsIds);
        Task RemoveItemAsync(string adventurerId, string itemId, int amount = 1);
        Task<IItem[]> ShowInventoryAsync(string adventurerId);
    }
}