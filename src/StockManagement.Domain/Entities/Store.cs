using System;
using System.Collections.Generic;
using System.Linq;

namespace StockManagement.Domain.Entities
{
    public class Store : BaseEntity
    {
        private readonly List<StockItem> _stockItems;
        public string Name { get; private set; }
        public string Address { get; private set; }
        public IList<StockItem> StockItems => _stockItems.AsReadOnly();

        public Store()
        {
            _stockItems = new List<StockItem>();
        }

        public Store(string name, string address) : this()
        {
            Name = name;
            Address = address;
        }

        public int GetStockAmount(Product product)
        {
            var stockItem = _stockItems.FirstOrDefault(x => x.ProductId == product.Id);

            return stockItem.Amount;
        }

        public void CreateStock(Product product, int amount)
        {
            if (_stockItems.Any(x => x.ProductId == product.Id))
                throw new InvalidOperationException($"There is already stock for the product {product.Id}");

            var stockItem = new StockItem(Id, product.Id, amount);

            _stockItems.Add(stockItem);
        }

        public void IncreaseStock(Product product, int amount)
        {
            var stockItem = _stockItems.FirstOrDefault(x => x.ProductId == product.Id);

            stockItem.IncreaseAmount(amount);
        }
        public void DecreaseStock(Product product, int amount)
        {
            var stockItem = _stockItems.FirstOrDefault(x => x.ProductId == product.Id);

            stockItem.DecreaseAmount(amount);
        }

        public void Update(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}