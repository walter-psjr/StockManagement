namespace StockManagement.Domain.Entities
{
    public class Store : BaseEntity
    {
        public string Name { get; private set; }
        public string Address { get; private set; }

        public Store(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public void Update(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}