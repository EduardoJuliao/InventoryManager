using System.Collections.Generic;
using System.Linq;
using InventoryHandler.Interfaces.Entities;
using InventoryHandler.Models;

namespace InventoryHandler.Entities
{
    public class Inventory
    {
        public string AdventurerId { get; set; }
        public long ItemAmount => Items != null ? Items.Sum(x => x.Amount) : 0;
        public ICollection<IInventoryItem> Items { get; set; }
    }
}