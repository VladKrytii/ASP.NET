using ApiStore.Data.Entities;

public class CartItemEntity
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public CartEntity Cart { get; set; }
    public int ProductId { get; set; }
    public ProductEntity Product { get; set; }
    public int Quantity { get; set; }
}
