using ApiStore.Data.Entities.Identity;

public class CartEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public ICollection<CartItemEntity> CartItems { get; set; }
}
