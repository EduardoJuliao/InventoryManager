using System.Threading.Tasks;
using InventoryHandler.Interfaces.Entities;

namespace InventoryHandler.Clients
{
    public interface IItemClient
    {
        Task<bool> ItemExists(string itemId);
        Task<IItem> Get(string itemId);
    }
}