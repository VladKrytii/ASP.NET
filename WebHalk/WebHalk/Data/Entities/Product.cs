namespace WebHalk.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

namespace WebHalk.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductPhoto> Photos { get; set; }
    }
}

namespace WebHalk.Data.Entities
{
    public class ProductPhoto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
