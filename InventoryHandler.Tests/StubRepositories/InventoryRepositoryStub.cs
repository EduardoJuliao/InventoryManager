using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryHandler.Interfaces.Entities;
using InventoryHandler.Interfaces.Repositories;
using InventoryHandler.Tests.StubEntities;

namespace InventoryHandler.Tests.StubRepositories
{
    public class InventoryRepositoryStub : IInventoryRepository
    {
        public readonly ConcurrentDictionary<string, ICollection<IInventoryItem>> Data;

        public InventoryRepositoryStub()
        {
            Data = new ConcurrentDictionary<string, ICollection<IInventoryItem>>();
        }

        public async Task AddAsync(string adventurerId, IItem item, int amount)
        {
            await Task.Run(() =>
            {
                if (Data.ContainsKey(adventurerId))
                {
                    Data.TryGetValue(adventurerId, out ICollection<IInventoryItem> collection);
                    var entry = collection.SingleOrDefault(x => x.Id == item.Id);
                    if (entry != null)
                    {
                        entry.Amount += amount;
                        Data[adventurerId] = collection;
                    }
                    else
                    {
                        collection.Add(new InventoryItemStub
                        {
                            Amount = amount,
                            Id = item.Id,
                            Name = item.Name
                        });
                    }
                }
                else
                {
                    Data.TryAdd(adventurerId, new List<IInventoryItem> {
                        new InventoryItemStub
                        {
                            Amount = amount,
                            Id = item.Id,
                            Name = item.Name
                        }
                    });
                }
            });

        }

        public async Task<bool> Exists(string adventurerId, string itemId)
        {
            return await Task.Run(() =>
            {
                Data.TryGetValue(adventurerId, out ICollection<IInventoryItem> collection);
                return collection != null && collection.Any(x => x.Id == itemId);
            });
        }

        public async Task<IInventoryItem> GetItemFromIventory(string adventurerId, string itemId)
        {
            return await Task.Run(() =>
            {
                Data.TryGetValue(adventurerId, out ICollection<IInventoryItem> collection);
                return collection.SingleOrDefault(x => x.Id == itemId);
            });
        }

        public async Task RemoveAsync(string adventurerId, string itemId, int amount)
        {
            await Task.Run(() =>
            {
                Data.TryGetValue(adventurerId, out ICollection<IInventoryItem> collection);
                var entry = collection.SingleOrDefault(x => x.Id == itemId);
                entry.Amount -= amount;
                if (entry.Amount <= 0)
                    collection.Remove(entry);

                Data[adventurerId] = collection;
            });
        }
    }
}