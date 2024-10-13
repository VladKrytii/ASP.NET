using ApiStore.Data.Entities.Identity;

public class OrderEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItemEntity> OrderItems { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; internal set; }
}