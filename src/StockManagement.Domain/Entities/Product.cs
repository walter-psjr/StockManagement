namespace StockManagement.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; }
        public decimal CostPrice { get; private set; }

        public Product(string name, decimal costPrice)
        {
            Name = name;
            CostPrice = costPrice;
        }

        public void Update(string name, decimal costPrice)
        {
            Name = name;
            CostPrice = costPrice;
        }
    }
}