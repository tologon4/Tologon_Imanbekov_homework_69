using System.ComponentModel.DataAnnotations;

namespace Delivery.Models;

public class Dish
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Заполните поле Name!")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Заполните поле Price!")]
    [Range(0, double.MaxValue, ErrorMessage = "Цена не может быть ниже 0!")]
    public double Price { get; set; }
    [Required(ErrorMessage = "Заполните поле Description!")]
    public string Description { get; set; }
    public int? EateryId { get; set; }
    public Eatery? Eatery { get; set; }
    public string? Avatar { get; set; }
}