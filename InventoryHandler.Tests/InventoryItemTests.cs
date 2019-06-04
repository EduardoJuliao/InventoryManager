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

        [SetUp]
        public void Setup()
        {
            _storeItems = Enumerable.Range(65, 25).Select(x => new ItemStub
            {
                Id = ((char)x).ToString(),
                Name = ((char)x).ToString()
            });

            _service = new InventoryService(new ItemClientStub(), new InventoryRepositoryStub());
        }

        [Test]
        [Category("Add")]
        public async Task CanAddItem()
        {
            await _service.AddItemAsync(ADVENTURERID, "A");

            var result = await _service.ShowInventoryAsync(ADVENTURERID);

            Assert.That(result.Count() == 1);
        }

        [Test]
        [Category("Add")]
        public async Task CanAddItemWithBiggerAmount()
        {
            await _service.AddItemAsync(ADVENTURERID, "A", 3);

            var result = await _service.ShowInventoryAsync(ADVENTURERID);

            Assert.That(result.Count() == 1);
            Assert.That(result.Single(x => x.Id == "A").Amount == 3);
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
            var list = new Dictionary<string, int>
            {
                {"A", 1},
                {"B", 1},
                {"C", 1},
            };

            await _service.AddItemsAsync(ADVENTURERID, list);
            var result = await _service.ShowInventoryAsync(ADVENTURERID);

            Assert.That(result.Count() == 3);
        }

        [Test]
        [Category("Add")]
        public async Task CanAddTheCorrectAmountOfItems()
        {
            var list = new Dictionary<string, int>
            {
                {"A", 2},
                {"B", 1},
                {"C", 1},
            };

            await _service.AddItemsAsync(ADVENTURERID, list);

            var result = await _service.ShowInventoryAsync(ADVENTURERID);
            Assert.That(result.Count() == 3);
            Assert.That(result.Single(x => x.Id == "A").Amount == 2);
            Assert.That(result.Single(x => x.Id == "B").Amount == 1);
            Assert.That(result.Single(x => x.Id == "C").Amount == 1);
        }

        [Test]
        [Category("Add")]
        public async Task CanAddTheCorrectAmountOfItemsWhenAddingWrongItems()
        {
            var list = new Dictionary<string, int>
            {
                {"A", 2},
                {"B", 1},
                {"b", 1},
                {"C", 1},
                {"D", 1},
            };

            await _service.AddItemsAsync(ADVENTURERID, list);

            var result = await _service.ShowInventoryAsync(ADVENTURERID);
            Assert.That(result.Count() == 4);
            Assert.That(result.Single(x => x.Id == "A").Amount == 2);
            Assert.That(result.Single(x => x.Id == "B").Amount == 1);
            Assert.That(result.Single(x => x.Id == "C").Amount == 1);
            Assert.That(result.Single(x => x.Id == "D").Amount == 1);
            Assert.That(result.SingleOrDefault(x => x.Id == "b") == null);
        }

        [Test]
        [Category("Update")]
        public async Task CanAddItemToExistingItemInInventory()
        {
            await _service.AddItemsAsync(ADVENTURERID, DataGen.GenerateInventoryValuePair());

            await _service.AddItemAsync(ADVENTURERID, "A");

            var result = await _service.ShowInventoryAsync(ADVENTURERID);
            Assert.That(result.Single(x => x.Id == "A").Amount == 11);
        }

        [Test]
        [Category("Remove")]
        public async Task CanRemoveItem()
        {
            await _service.AddItemsAsync(ADVENTURERID, DataGen.GenerateInventoryValuePair());

            await _service.RemoveItemAsync(ADVENTURERID, "A", 3);

            var result = await _service.ShowInventoryAsync(ADVENTURERID);
            Assert.That(result.Single(x => x.Id == "A").Amount == 7);
        }

        [Test]
        [Category("Remove")]
        public async Task RemovingAnItemRemovesheEntryFromIventory()
        {

            await _service.AddItemsAsync(ADVENTURERID, DataGen.GenerateInventoryValuePair());

            await _service.RemoveItemAsync(ADVENTURERID, "A", 10);

            var result = await _service.ShowInventoryAsync(ADVENTURERID);
            Assert.That(result.SingleOrDefault(x => x.Id == "A") == null);
        }

        [Test]
        [Category("Remove")]
        public async Task CantRemoveIfWrongAmoutIsInformed()
        {
            await _service.AddItemsAsync(ADVENTURERID, DataGen.GenerateInventoryValuePair());

            Assert.ThrowsAsync<InventoryException>(async () => await _service.RemoveItemAsync(ADVENTURERID, "A", 11));
        }

        [Test]
        [Category("Remove")]
        public async Task CantRemoveAnItemThatDoesntExistsInInventory()
        {
            await _service.AddItemsAsync(ADVENTURERID, DataGen.GenerateInventoryValuePair());

            Assert.ThrowsAsync<InventoryException>(async () => await _service.RemoveItemAsync(ADVENTURERID, "z", 1));
        }
    }
}