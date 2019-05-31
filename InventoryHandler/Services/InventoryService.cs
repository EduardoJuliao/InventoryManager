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

        public async Task AddItemAsync(string adventurerId, string itemId)
        {
            var item = await _itemClient.Get(itemId);
            if (item == null)
                throw new InventoryException();
            await _inventoryRepository.AddAsync(adventurerId, item, 1);
        }

        public async Task AddItemsAsync(string adventurerId, string[] itemsIds)
        {
            var group = itemsIds.GroupBy(x => x);
            foreach (var gItem in group.Select(x => new
            {
                id = x.Key,
                amount = x.Count()
            }))
            {
                var item = await _itemClient.Get(gItem.id);
                if (item != null)
                    await _inventoryRepository.AddAsync(adventurerId, item, gItem.amount);
            }
        }

        public async Task RemoveItemAsync(string adventurerId, string itemId, int amount)
        {
            var item = await GetItemFromIventoryAsync(adventurerId, itemId);
            if (item == null || item.Amount < amount)
                throw new InventoryException();
            await _inventoryRepository.RemoveAsync(adventurerId, itemId, amount);
        }

        public Task<IItem[]> ShowInventoryAsync(string adventurerId)
        {
            throw new System.NotImplementedException();
        }
    }
}