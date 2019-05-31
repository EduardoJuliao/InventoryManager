using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryHandler.Interfaces.Entities;

namespace InventoryHandler.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        Task AddAsync(string adventurerId, IItem item, int amount);

        Task RemoveAsync(string adventurerId, string itemId, int amount);
        Task<bool> Exists(string adventurerId, string itemId);
        Task<IInventoryItem> GetItemFromIventory(string adventurerId, string itemId);
    }
}