namespace Data.Products.ReadModels
{
    public class Product
    {
        public Product(int id, string name, string code, decimal price, bool deleted)
        {
            Id = id;
            Name = name;
            Code = code;
            Price = price;
            Deleted = deleted;
        }

        public int Id { get; }
        public string Name { get; }
        public string Code { get; }
        public decimal Price { get; }
        public bool Deleted { get; }
    }
}