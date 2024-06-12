namespace Delivery.Models;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public int? EateryId { get; set; }
    public Eatery? Eatery { get; set; }
}