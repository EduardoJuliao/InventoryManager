using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryHandler.Clients;
using InventoryHandler.Exceptions;
using InventoryHandler.Interfaces.Entities;
using InventoryHandler.Interfaces.Repositories;
using InventoryHandler.Interfaces.Services;

namespace InventoryHandler.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IItemClient _itemClient;
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IItemClient itemClient,
        IInventoryRepository repo)
        {
            _itemClient = itemClient;
            _inventoryRepository = repo;
        }

        public async Task<bool> ExistsAsync(string adventurerId, string itemId)
        {
            return await _inventoryRepository.Exists(adventurerId, itemId);
        }

        public async Task<IInventoryItem> GetItemFromIventoryAsync(string adventurerId, string itemId)
        {
            return await _inventoryRepository.GetItemFromIventory(adventurerId, itemId);
        }

        public async Task AddItemAsync(string adventurerId, string itemId, int amount = 1)
        {
            var item = await _itemClient.Get(itemId);
            if (item == null)
                throw new InventoryException();
            await _inventoryRepository.AddAsync(adventurerId, item, amount);
        }

        public async Task AddItemsAsync(string adventurerId, IEnumerable<KeyValuePair<string, int>> items)
        {
            if (items == null)
                throw new System.ArgumentNullException(nameof(items));

            foreach (var gItem in items)
            {
                var item = await _itemClient.Get(gItem.Key);
                if (item != null)
                    await _inventoryRepository.AddAsync(adventurerId, item, gItem.Value);
            }
        }

        public async Task RemoveItemAsync(string adventurerId, string itemId, int amount)
        {
            var item = await GetItemFromIventoryAsync(adventurerId, itemId);
            if (item == null || item.Amount < amount)
                throw new InventoryException();
            await _inventoryRepository.RemoveAsync(adventurerId, itemId, amount);
        }

        public async Task<IEnumerable<IInventoryItem>> ShowInventoryAsync(string adventurerId)
        {
            return await _inventoryRepository.GetItemsFromIventory(adventurerId);
        }
    }
}