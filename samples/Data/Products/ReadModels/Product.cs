namespace Data.Products.ReadModels
{
    public class Product
    {
        public Product(int id, string name, string code, decimal price)
        {
            Id = id;
            Name = name;
            Code = code;
            Price = price;
        }

        public int Id { get; }
        public string Name { get; }
        public string Code { get; }
        public decimal Price { get; }
    }
}