using System;

namespace StockManagement.Domain.Entities
{
    public class StockItem
    {
        public Guid StoreId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Amount { get; private set; }

        public StockItem(Guid storeId, Guid productId, int amount)
        {
            StoreId = storeId;
            ProductId = productId;
            Amount = amount;
        }
    }
}