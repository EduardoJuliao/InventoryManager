using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryHandler.Clients;
using InventoryHandler.Entities;
using InventoryHandler.Exceptions;
using InventoryHandler.Interfaces.Entities;
using InventoryHandler.Interfaces.Repositories;
using InventoryHandler.Interfaces.Services;
using InventoryHandler.Services;
using InventoryHandler.Tests.ClientStubs;
using InventoryHandler.Tests.StubEntities;
using InventoryHandler.Tests.StubRepositories;
using NUnit.Framework;

namespace InventoryHandler.Tests
{
    [TestFixture]
    public class InventoryItemTests
    {
        private IEnumerable<IItem> _storeItems;
        private IInventoryService _service;
        private const string ADVENTURERID = "98758E0D-B3E0-4ECA-A837-AE239715371E";
        private IItemClient _itemClient = new ItemClientStub();
        private InventoryRepositoryStub _repository;

        [SetUp]
        public void Setup()
        {
            _storeItems = Enumerable.Range(65, 25).Select(x => new ItemStub
            {
                Id = ((char)x).ToString(),
                Name = ((char)x).ToString()
            });
            _repository = new InventoryRepositoryStub();
            _service = new InventoryService(_itemClient, _repository);
        }

        [Test]
        [Category("Add")]
        public async Task CanAddItem()
        {
            await _service.AddItemAsync(ADVENTURERID, "A");

            Assert.That(_repository.Data[ADVENTURERID].Count == 1);
        }

        [Test]
        [Category("Add")]
        public void CantAddItemThatDoesntExist()
        {
            Assert.ThrowsAsync<InventoryException>(async () =>
            {
                await _service.AddItemAsync(ADVENTURERID, "a");
            });
        }

        [Test]
        [Category("Add")]
        public async Task CanAddMultipleItems()
        {
            await _service.AddItemsAsync(ADVENTURERID, new[] { "A", "B", "C" });

            Assert.That(_repository.Data[ADVENTURERID].Count == 3);
        }

        [Test]
        [Category("Add")]
        public async Task CanAddTheCorrectAmountOfItems()
        {
            await _service.AddItemsAsync(ADVENTURERID, new[] { "A", "A", "B", "C" });

            _repository.Data.TryGetValue(ADVENTURERID, out ICollection<IInventoryItem> collection);
            Assert.That(collection.Count == 3);
            Assert.That(collection.Single(x => x.Id == "A").Amount == 2);
            Assert.That(collection.Single(x => x.Id == "B").Amount == 1);
            Assert.That(collection.Single(x => x.Id == "C").Amount == 1);
        }

        [Test]
        [Category("Add")]
        public async Task CanAddTheCorrectAmountOfItemsWhenAddingWrongItems()
        {
            await _service.AddItemsAsync(ADVENTURERID, new[] { "A", "A", "B", "b", "C", "D" });

            _repository.Data.TryGetValue(ADVENTURERID, out ICollection<IInventoryItem> collection);
            Assert.That(collection.Count == 4);
            Assert.That(collection.Single(x => x.Id == "A").Amount == 2);
            Assert.That(collection.Single(x => x.Id == "B").Amount == 1);
            Assert.That(collection.Single(x => x.Id == "C").Amount == 1);
            Assert.That(collection.Single(x => x.Id == "D").Amount == 1);
            Assert.That(collection.SingleOrDefault(x => x.Id == "b") == null);
        }

        [Test]
        [Category("Update")]
        public async Task CanAddItemToExistingItemInInventory()
        {
            _repository.Data.TryAdd(ADVENTURERID, DataGen.GenerateInventory().ToList());

            await _service.AddItemAsync(ADVENTURERID, "A");

            _repository.Data.TryGetValue(ADVENTURERID, out ICollection<IInventoryItem> collection);
            Assert.That(collection.Single(x => x.Id == "A").Amount == 11);
        }

        [Test]
        [Category("Remove")]
        public async Task CanRemoveItem()
        {
            _repository.Data.TryAdd(ADVENTURERID, DataGen.GenerateInventory().ToList());

            await _service.RemoveItemAsync(ADVENTURERID, "A", 3);

            _repository.Data.TryGetValue(ADVENTURERID, out ICollection<IInventoryItem> collection);
            Assert.That(collection.Single(x => x.Id == "A").Amount == 7);
        }

        [Test]
        [Category("Remove")]
        public async Task RemovingAnItemRemovesheEntryFromIventory()
        {
            _repository.Data.TryAdd(ADVENTURERID, DataGen.GenerateInventory().ToList());

            await _service.RemoveItemAsync(ADVENTURERID, "A", 10);

            _repository.Data.TryGetValue(ADVENTURERID, out ICollection<IInventoryItem> collection);
            Assert.That(collection.SingleOrDefault(x => x.Id == "A") == null);
        }

        [Test]
        [Category("Remove")]
        public void CantRemoveIfWrongAmoutIsInformed()
        {
            _repository.Data.TryAdd(ADVENTURERID, DataGen.GenerateInventory().ToList());

            Assert.ThrowsAsync<InventoryException>(async () => await _service.RemoveItemAsync(ADVENTURERID, "A", 11));
        }

        [Test]
        [Category("Remove")]
        public void CantRemoveAnItemThatDoesntExistsInInventory()
        {
            _repository.Data.TryAdd(ADVENTURERID, DataGen.GenerateInventory().ToList());

            Assert.ThrowsAsync<InventoryException>(async () => await _service.RemoveItemAsync(ADVENTURERID, "z", 1));
        }
    }
}