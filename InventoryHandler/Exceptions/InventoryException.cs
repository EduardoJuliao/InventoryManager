namespace InventoryHandler.Exceptions
{
    [System.Serializable]
    public class InventoryException : System.Exception
    {
        public InventoryException() { }
        public InventoryException(string message) : base(message) { }
        public InventoryException(string message, System.Exception inner) : base(message, inner) { }
        protected InventoryException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}