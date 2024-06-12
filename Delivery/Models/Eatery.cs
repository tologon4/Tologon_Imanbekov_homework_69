namespace Delivery.Models;

public class Eatery
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Description { get; set; }
    public ICollection<Dish>? Dishes { get; set; }
}