using System.ComponentModel.DataAnnotations;

namespace Delivery.Models;

public class Order
{
    public int Id { get; set; }
    public int BasketId { get; set; }
    public Basket? Basket { get; set; }
    [Required(ErrorMessage = "Заполните поле Phone Number")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Заполните в формате x-цифра(0-9): 0 xxx xx xx xx")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Заполните адрес отправки")]
    public string Address { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }
}