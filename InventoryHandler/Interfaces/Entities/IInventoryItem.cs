namespace InventoryHandler.Interfaces.Entities
{
    public interface IInventoryItem : IItem
    {
        int Amount { get; set; }
    }
}