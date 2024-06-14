using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Models;

public class Eatery
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Заполните поле Name!")]
    [Remote(action: "CheckEateryName", controller:"Validation", ErrorMessage = "Данный Name уже занят!")]
    public string Name { get; set; }
    public string? Avatar { get; set; }
    [Required(ErrorMessage = "Заполните поле Description!")]
    public string Description { get; set; }
    public ICollection<Dish>? Dishes { get; set; }
}