using System.Text.Json;
using System.Text.Json.Serialization;
using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Delivery.Controllers;

public class BasketController : Controller
{
    private DeliveryDb _db;
    private UserManager<User> _userManager;

    public BasketController(DeliveryDb db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Create()
    {
        return Ok();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Order(string? dishes, string address, string phoneNumber, int eateryId)
    {
        
        Eatery? eatery = await _db.Eateries.FirstOrDefaultAsync(e => e.Id == eateryId);
        User? user = await _userManager.GetUserAsync(User);

        if (dishes != null && address != null && phoneNumber != null && eatery != null && user != null)
        {
            List<DishViewModel> buffer = JsonConvert.DeserializeObject<List<DishViewModel>>(dishes);
            List<Dish> dishesNew = new List<Dish>();
            double totalPrice = 0;
            
            foreach (var dishDto in buffer)
            {
                dishesNew.Add(await _db.Dishes.FirstOrDefaultAsync(d => d.Id == int.Parse(dishDto.DishId)));
                totalPrice += double.Parse(dishDto.Price) ;
            }

            Basket basket = new Basket()
            {
                EateryId = eateryId,
                Eatery = eatery,
                UserId = user.Id,
                TotalPrice = totalPrice,
                Dishes = dishesNew
            };

            Order order = new Order()
            {
                Basket = basket,
                PhoneNumber = phoneNumber,
                Address = address,
                UserId = user.Id
            };

            _db.Baskets.Add(basket);
            _db.Orders.Add(order);

            int n = await _db.SaveChangesAsync();

            if (n > 0)
                return RedirectToAction("OwnOrders", "Basket");
        }

        return BadRequest();
    }

    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> OwnOrders()
    {
        User user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var orders = await _db.Orders.Include(u => u.User)
                .Include(b => b.Basket)
                .Include(b => b.Basket.Eatery)
                .Include(b => b.Basket.Dishes)
                .Where(o => o.UserId == user.Id).ToListAsync();
            return View(orders);
        }
        return NotFound();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        var orders = await _db.Orders.Include(u => u.User)
            .Include(b => b.Basket)
            .Include(b => b.Basket.Eatery)
            .Include(b => b.Basket.Dishes)
            .ToListAsync();
        return View(orders);
    }
}