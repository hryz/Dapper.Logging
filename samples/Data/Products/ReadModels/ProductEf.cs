namespace Data.Products.ReadModels
{
    public class ProductEf
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public bool Deleted { get; set; }
    }
}