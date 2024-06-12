namespace Delivery.Models;

public class Basket
{
    public int Id { get; set; }
    public int EateryId { get; set; }
    public Eatery? Eatery { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Dish>? Dishes { get; set; }
    public int TotalPrice { get; set; }
    
}